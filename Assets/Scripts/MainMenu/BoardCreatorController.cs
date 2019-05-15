//code written by: Antonio Adame, 20237657

using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreatorController : MonoBehaviour {

    // test variables (NOT USED FOR FINAL PRODUCT). 


    // outside scripts
    private CardSelectorController cardSelector;

    // holds all cards for each board.

    Board[] boards = PlayerStats.Boards;

    //public Transform[] _board_1_cards;
    //public Transform[] _board_2_cards;
    //public Transform[] _board_3_cards;
    //public Transform[] _board_4_cards;

    public Sprite[] _card_img;

    public RectTransform board;
    public RectTransform[] _boards;

    public GameObject leftButton, rightButton; // holders for the left and right nav buttons.
    public GameObject randomButton, clearButton, saveButton;
    public GameObject editButton;


    private float distance; 
    private int index;
    private float boardPos; // get distance of boards from each other.
    private bool[] boardIsFull;

    private string blankName = "Blank2";

    public bool isEditing; // true when user wants to edit board. (edit button is pressed).

    public bool[,] _alreadyPicked; // 2D array that will hold information about each card reaching its pick rate per board.

    public int currBoard = 0; // tells us current selected board.

    // touch variables. 
    RaycastHit hit;
    private Vector2 touchPos;


    //=== player object===//
    ///public static Player player;
    ///
    public Player player;

    private void Awake()
    {
        int l = _card_img.Length;
        _alreadyPicked = new bool[4, l];
        for (int i = 0; i < l; i++)
        {
            if (!PlayerStats.dict.ContainsKey(_card_img[i].name )) 
                PlayerStats.dict.Add(_card_img[i].name, i);
        }

        //LoadBoard("board1.sav", _board_1_cards, 0);
        //LoadBoard("board2.sav", _board_2_cards, 1);
        //LoadBoard("board3.sav", _board_3_cards, 2);
        //LoadBoard("board4.sav", _board_4_cards, 3);

        //player.SetBoard(_board_1_cards, 1);
        //player.SetBoard(_board_2_cards, 2);
        //player.SetBoard(_board_3_cards, 3);
        //player.SetBoard(_board_4_cards, 4);
    }

    private void Start()
    {
        Time.timeScale = 1;
        Screen.orientation = ScreenOrientation.Portrait;

        player = new Player();
        int l = _card_img.Length;
        Debug.Log("Loading cards");
        for (int i = 0; i < 4; i++)
        {
            //DontDestroyOnLoad(_boards[i].gameObject);
            PlayerStats.boards[i] = new Board();
            for (int c = 0; c < 16; c++)
                PlayerStats.boards[i].cards[c] = _boards[i].GetChild(c);
        }



        PlayerStats.LoadBoards(_alreadyPicked);

        cardSelector = GetComponent<CardSelectorController>();

        boardIsFull = new bool[4];

        // initialize board edit buttons. 
        editButton.SetActive(true);

        randomButton.SetActive(false);
        clearButton.SetActive(false);
        saveButton.SetActive(false);

        // find distance between boards. 
        distance = Mathf.Abs(_boards[1].anchoredPosition.x - _boards[0].anchoredPosition.x);

        boardPos = board.anchoredPosition.x;
        currBoard = 0;
    }
    void Update()
    {

        // manages detecting where user taps and placing a card there. 
        #region Board Creator
        if (isEditing)
        {
            // disable board scroller buttons while editing.
            rightButton.SetActive(false);
            leftButton.SetActive(false);
            #region Mobile
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // record position where touch was detected in 2D. 
                    touchPos = (Vector2)Input.GetTouch(0).position;
                    Ray ray = Camera.main.ScreenPointToRay(touchPos);

                    // if collision with something was detected. 
                    if (Physics.Raycast(ray, out hit))
                    {
                        Transform cardDectected = hit.transform; 
                        // check detections on all boards.
                        // possible feature: start 4 threads to check all 4 boards at once.
                        for (int i = 0; i < 4; i++)
                        {
                            string board_name = "Board" + (i+1);
                            // remove card from board.
                            if (cardDectected.GetComponent<Transform>().parent.name == board_name && currBoard == i)
                            {
                                DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                                ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));
                                _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                            }
                            // add card to board.
                            else
                            {
                                if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                                {
                                    ChangeImage(cardDectected, cardSelector.cardSelected);
                                    SearchIfPicked(cardSelector.cardSelected, i);
                                }
                            }
                        }

                        #region old code
                        //if (cardDectected.parent.name == "Board1" && currBoard == 0)
                        //{
                        //    if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        //    {
                        //        // remove card from board.
                        //        DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                        //        ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                        //        _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        //    }
                        //    else
                        //    {
                        //        if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                        //        {
                        //            // add card to board.
                        //            ChangeImage(cardDectected, cardSelector.cardSelected);
                        //            SearchIfPicked(cardSelector.cardSelected, 0);
                        //        }
                        //    }
                        //}
                        //if (cardDectected.parent.name == "Board2" && currBoard == 1)
                        //{
                        //    if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        //    {
                        //        // when card on board is removed by user.
                        //        DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                        //        ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                        //        _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        //    }
                        //    else
                        //    {
                        //        if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                        //        {
                        //            // when user adds card to the board.
                        //            ChangeImage(cardDectected, cardSelector.cardSelected);
                        //            SearchIfPicked(cardSelector.cardSelected, 1);
                        //        }
                        //    }
                        //}
                        //if (cardDectected.parent.name == "Board3" && currBoard == 2)
                        //{
                        //    if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        //    {
                        //        // when card on board is removed by user.
                        //        DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                        //        ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                        //        _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        //    }
                        //    else
                        //    {
                        //        if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                        //        {
                        //            // when user adds card to the board.
                        //            ChangeImage(cardDectected, cardSelector.cardSelected);
                        //            SearchIfPicked(cardSelector.cardSelected, 2);
                        //        }
                        //    }
                        //}
                        //if (cardDectected.parent.name == "Board4" && currBoard == 3)
                        //{
                        //    if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        //    {
                        //        // when card on board is removed by user.
                        //        DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                        //        ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                        //        _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        //    }
                        //    else
                        //    {
                        //        if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                        //        {
                        //            // when user adds card to the board.
                        //            ChangeImage(cardDectected, cardSelector.cardSelected);
                        //            SearchIfPicked(cardSelector.cardSelected, 3);
                        //        }
                        //    }
                        //}
                        #endregion

                    }
                }
            }
            #endregion
            #region PC
            else if (Input.GetMouseButtonDown(0))
            {
                touchPos = (Vector2)Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(touchPos);

                if (Physics.Raycast(ray, out hit))
                {
                    Transform cardDectected = hit.transform;
                    // basically doing the same thing when user adds or removes cards accross the different boards. 
                    if (cardDectected.parent.name == "Board1" && currBoard == 0)
                    {
                        Debug.Log(cardDectected.GetComponent<Image>().sprite.name);
                        if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        {
                            // when card on board is removed by user.
                            DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                            ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                            _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        }
                        else
                        {
                            if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                            {
                                // when user adds card to the board.
                                ChangeImage(cardDectected, cardSelector.cardSelected);
                                SearchIfPicked(cardSelector.cardSelected, 0);
                            }
                        }
                    }
                    if (cardDectected.parent.name == "Board2" && currBoard == 1)
                    {
                        if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        {
                            // when card on board is removed by user.
                            DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                            ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                            _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        }
                        else
                        {
                            if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                            {
                                // when user adds card to the board.
                                ChangeImage(cardDectected, cardSelector.cardSelected);
                                SearchIfPicked(cardSelector.cardSelected, 1);
                            }
                        }
                    }
                    if (cardDectected.parent.name == "Board3" && currBoard == 2)
                    {
                        if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        {
                            // when card on board is removed by user.
                            DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                            ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                            _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        }
                        else
                        {
                            if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                            {
                                // when user adds card to the board.
                                ChangeImage(cardDectected, cardSelector.cardSelected);
                                SearchIfPicked(cardSelector.cardSelected, 2);
                            }
                        }
                    }
                    if (cardDectected.parent.name == "Board4" && currBoard == 3)
                    {
                        if (cardDectected.GetComponent<Image>().sprite.name != blankName)
                        {
                            // when card on board is removed by user.
                            DeSelectCard(cardDectected.GetComponent<Image>().sprite.name);
                            ChangeImage(cardDectected, Resources.Load<Sprite>(blankName));

                            _alreadyPicked[currBoard, cardSelector.cardSelectedIndex] = false;
                        }
                        else
                        {
                            if (!_alreadyPicked[currBoard, cardSelector.cardSelectedIndex])
                            {
                                // when user adds card to the board.
                                ChangeImage(cardDectected, cardSelector.cardSelected);
                                SearchIfPicked(cardSelector.cardSelected, 3);
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion


        #region Arrow Button Behaviours
        // only show buttons where user can go to.
        // ie. if there is no board to the left, do NOT display left button. 

        if (index == 0)
        {
            // on board 1
            if (!isEditing)
            {
                leftButton.SetActive(false);
                rightButton.SetActive(true);
            }

            currBoard = 0; 
        }
        else if (index == 1)
        {
            if (!isEditing)
            {
                // on board 2
                leftButton.SetActive(true);
                rightButton.SetActive(true);
            }

            currBoard = 1;
        }
        else if (index == 2)
        {
            if (!isEditing)
            {
                // on board 3
                leftButton.SetActive(true);
                rightButton.SetActive(true);
            }

            currBoard = 2;
        }
        else if (index == 3)
        {
            if (!isEditing)
            {
                leftButton.SetActive(true);
                rightButton.SetActive(false);
            }

            currBoard = 3;
        }

        
        LerpToBoard(-distance*index+boardPos);

        #endregion


    }

    void LerpToBoard(float pos)
    {

        //Debug.Log(index);
        float newX = Mathf.Lerp(board.anchoredPosition.x, pos, Time.deltaTime * 10f);
        Vector2 newPos = new Vector2(newX, board.anchoredPosition.y);

        board.anchoredPosition = newPos;
    }
    // overloaded function that is called on button pressed. 
    public void LerpToBoard(bool right)
    {
        
        if (right)
        {
            if ((index+1) < _boards.Length)
                index++;
        }
        else if (!right)
        {
            if ((index-1) >= 0)
                index--;
        }
        Debug.Log("boardIndex::" + index);
    }

    //functions that change the current image to a new image.
    private void ChangeImage(Transform currImg, Transform newImg)
    {
        currImg.GetComponent<Image>().sprite = newImg.GetComponent<Image>().sprite;
    }
    private void ChangeImage(Transform currImg, Sprite newImg)
    {
        currImg.GetComponent<Image>().sprite = newImg;
    }
    
    // function that searches if a card has already been picked.
    private void SearchIfPicked(Transform cardSelected, int board)
    {
        // uses binary search algorithm to search through sprites faster.
        int min = 0;
        int max = _card_img.Length;
        int pivot = (max + min) / 2;
        
        while (max >= min) {
            if (cardSelected.GetComponent<Image>().sprite.name.CompareTo(_card_img[pivot].name) == 1)
            {
                min = pivot+1;
                pivot = (min + max) / 2;
            }
            else if (cardSelected.GetComponent<Image>().sprite.name.CompareTo(_card_img[pivot].name) == -1)
            {
                max = pivot-1;
                pivot = (min + max) / 2;
            }
            
            else
            {
                Debug.Log(pivot);
                if (!_alreadyPicked[board, pivot])
                    _alreadyPicked[board, pivot] = true;

                SelectCard(cardSelected);

                break;
            }
        }
        if (min > max)
        {
            Debug.Log("ERROR: CARD NOT FOUND");
        }
    }
    // when a card has already been selected to the board, we want to "gray it out" (guess in this case its black it out) 
    // this is to signify the player that they cannot select that card for their board more than once. 
    public void SelectCard(Transform card)
    {

        Color newColor = new Color(0.2f, 0.2f, 0.2f);
        card.GetComponent<Image>().color = newColor;
    }
    public void SelectCard(string card_name)
    {
        int i = 0;
        while (card_name != _card_img[i].name)
        {
            i++;
        }

    }
    // this deselect method is used to mark a card not currently in the board. 
    // used in cardselectorcontroller script which goes through the whole selector panel and marks and unmarks cards accordingly.
    public void DeSelectCard(Transform card)
    {
        Color newColor = new Color(1, 1, 1);
        card.GetComponent<Image>().color = newColor;
    }

    // this overloaded deselect method searches for a card with a given name and changes its color.
    // rather than simply changing color, this method serves to help the game know when a card from the board was removed. 
    public void DeSelectCard(string cardName)
    {
        int min = 0;
        int max = _card_img.Length;
        int pivot = (max + min) / 2;

        while (max >= min)
        {
            if (cardName.CompareTo(cardSelector._cards[pivot].GetComponent<Image>().sprite.name) == 1)
            {
                min = pivot + 1;
                pivot = (min + max) / 2;
            }
            else if (cardName.CompareTo(cardSelector._cards[pivot].GetComponent<Image>().sprite.name) == -1)
            {
                max = pivot - 1;
                pivot = (min + max) / 2;
            }
            else
            {
                Color newColor = new Color(1, 1, 1);
                cardSelector._cards[pivot].GetComponent<Image>().color = newColor;
                cardSelector.cardSelectedIndex = pivot;

                _alreadyPicked[currBoard, pivot] = false;
                break;
            }
        }
    }

    // randomizes board content, only called upon button click. 
    public void RandomBoard()
    {
        int rand = 0;
        for (int i = 0; i < boards[currBoard].cards.Length; i++)
        {
            do {
                rand = (int)Random.Range(0, _card_img.Length);
                if (!_alreadyPicked[currBoard, rand])
                {
                    // deselect previously randomly selected card on cards scroller. 
                    if (boards[currBoard].cards[i].GetComponent<Image>().sprite.name != blankName)
                        DeSelectCard(boards[currBoard].cards[i].GetComponent<Image>().sprite.name);

                    boards[currBoard].cards[i].GetComponent<Image>().sprite = _card_img[rand];
                    _alreadyPicked[currBoard, rand] = true;
                    break;
                }
            } while (_alreadyPicked[0, rand]);
        }
    }

    // clear board of all cards. 
    public void ClearBoard()
    {
        for (int i = 0; i < boards[currBoard].cards.Length; i++)
        {
            if (boards[currBoard].cards[i].GetComponent<Image>().sprite.name != blankName)
            {
                DeSelectCard(boards[currBoard].cards[i].GetComponent<Image>().sprite.name);
                ChangeImage(boards[currBoard].cards[i], Resources.Load<Sprite>(blankName));
            }
        }
    }
    // function called when edit button is pressed. 
    public void EditBoard(bool _edit)
    {
        isEditing = _edit;
        if (isEditing)
        {
            leftButton.SetActive(false);
            rightButton.SetActive(false);

            editButton.SetActive(false);

            randomButton.SetActive(true);
            clearButton.SetActive(true);
            saveButton.SetActive(true);
        }
        else if (!isEditing)
        {

            editButton.SetActive(true);

            randomButton.SetActive(false);
            clearButton.SetActive(false);
            saveButton.SetActive(false);
        }
    }

    public void SaveBoard()
    {
        // check if the board is filled when user presses save. 
        for (int i = 0; i < boards.Length; i++)
        {
            for (int j = 0; j < PlayerStats.Boards[i].cards.Length; j++)
            {
                if (PlayerStats.Boards[i].cards[j].GetComponent<Image>().sprite.name == blankName)
                {
                    PlayerStats.Boards[i].isFull = false;
                }

            }
        }
        string filename = "Board" + (currBoard + 1) + ".sav";
        SaveLoadController.SaveBoards(PlayerStats.Boards);   
    }
}
