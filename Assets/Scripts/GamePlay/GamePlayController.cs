using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Linq;



public class GamePlayController : MonoBehaviour {

    private bool board_select_toggle = false;
    // TEMPORARY AI VARIABLES. 
    public GameObject[] AI_lights;


    private bool[] AI_canCall = new bool[4];
    // if random number falls in this index, the AI calls buenas.
    // random number is generated every deck call.
    private int AI_callIndex;
    private bool[,] AI_isSelected = new bool[4,54];
    public bool[] AI_winMethod = new bool[7];
    // number of boards per AI player.
    private int AI_1_numBoards, AI_2_numBoards, AI_3_numBoards, AI_4_numBoards;
    private int[] AI_stars;


    // visual feedback for AI actions. 
    private Sprite[,] AI_recentlyPlayed;

    public GameObject[] P1_cards;
    public GameObject[] P2_cards;
    public GameObject[] P3_cards;
    public GameObject[] P4_cards;


    //selector variables. 
    int board_selected_index = 0;

    public WinController win_message;

    private int all_boards, board_1, board_2, board_3, board_4;

    public Canvas gameCanvas;
    public GameObject[] boards;
    private Vector3[] _board_pos;

    public Transform Deck;

    private GameObject[] spawnedBoards;
    private GameObject[] spawnedBoards_inOrder;

    public Transform selectorsHolder;
    public GameObject[] boardSelectors;
    public GameObject viewToggle;

    public GameObject deckCard;
    private Sprite[] _deck_cards_holder; // holds preset array of cards set in random order to be called into the main deckCard.
    private int[] _deck_cards_index;

    public Text[] AI_NAMES;
    public Sprite[] card_img;
    public GameObject beans;

    public bool[,] isSelected;
    private GameObject[,] beansOnBoard;
    int boardPlayed;

    private bool checking_win; // set this value to true when player presses win. 

    private bool single_player = true;

    // indices of player_won array meaning: 
    // 0 = vertical win. 
    // 1 = horizontal win. 
    // 2 = diagonal win. 
    // 3 = 'posito' win. 
    // 4 = corners win.
    public bool[,] player_won; // indicates which win was detected.
    public bool win_exists = false;
    //public bool[] board_with_win;
    // makes sure that player calls buenas ON winning card (on deck). 
    private bool can_win;

    private int player_stars = 3;
    private int money_won = 0;

    private Sprite[] recently_played;
    private List<int> _cards_called;
    private Sprite current_card_called;
    private int called_index;
    private bool[,] eligible_play; // holds which places can be played. 

    // board index is the key and the value is a dictionary that maps a card index.
    private Dictionary<int, Dictionary<int, int>> _cards_played = new Dictionary<int, Dictionary<int, int>>();
    private GameObject[,] board_cards;

    // touch vars
    RaycastHit hit;
    Vector2 touch_pos;



    private int gameMode = 0;
    private float deckSpeed = 0;

    #region Mono Functions
    private void Awake()
    {
        _deck_cards_holder = new Sprite[54];
        _deck_cards_index = new int[54];
        _cards_called = new List<int>();

        board_cards = new GameObject[4, 16];
        beansOnBoard = new GameObject[4, 16];
        isSelected = new bool[4, 16];
        spawnedBoards = new GameObject[4];
        spawnedBoards_inOrder = new GameObject[4];
        recently_played = new Sprite[2];
        AI_recentlyPlayed = new Sprite[4, 2];

        for (int i = 0; i < 4; i++)
        {
            _cards_played[i] = new Dictionary<int, int>();
        }
        
    }
    // Use this for initialization
    void Start() {
        gameMode = PlayerPrefs.GetInt("GameMode");

        for (int i = 0; i < 4; i++)
        {
            
        }

        deckSpeed = PlayerPrefs.GetInt("DeckSpeed");
        // set the amount of seconds for deckspeed into the same variable to avoid redundancy. 
        if (deckSpeed == 0) deckSpeed = 7;
        else if (deckSpeed == 1) deckSpeed = 5;
        else if (deckSpeed == 2) deckSpeed = 3;


        for (int i = 0; i < AI_canCall.Length; i++)
            AI_canCall[i] = true;
        player_won = new bool[7,4];
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                player_won[i, j] = false;
            }
        }
        //board_with_win = new bool[4];

        AI_stars = new int[4];
        for (int i = 0; i < AI_stars.Length; i++)
            AI_stars[i] = 3;


        Time.timeScale = 1;
        Screen.orientation = ScreenOrientation.Portrait;



        win_message = gameObject.GetComponent<WinController>();

        int random = (int)Random.Range(0, 54);
        deckCard.GetComponent<Image>().sprite = card_img[random];
        
        _cards_called.Add(random);

        // initialize arrays.
        eligible_play = new bool[4, 16];
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                eligible_play[i, j] = true;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 16; j++) {
                isSelected[i,j] = false;
                beansOnBoard[i, j] = null;
            }
        }




        // '1' indicated that the board has been chosen.
        board_1 = PlayerPrefs.GetInt("board1chosen");
        board_2 = PlayerPrefs.GetInt("board2chosen");
        board_3 = PlayerPrefs.GetInt("board3chosen");
        board_4 = PlayerPrefs.GetInt("board4chosen");
        all_boards = board_1 + board_2 + board_3 + board_4;


        if (all_boards == 1)
        {
            // check which board was selected. 
            if (board_1 == 1) spawnedBoards[0] = SpawnBoard(0, new Vector2(0, -50), "board1", new Vector2(1.34f, 1.03f));
            else if (board_2 == 1) spawnedBoards[1] = SpawnBoard(1, new Vector2(0, -50), "board2", new Vector2(1.34f, 1.03f));
            else if (board_3 == 1) spawnedBoards[2] = SpawnBoard(2, new Vector2(0, -50), "board3", new Vector2(1.34f, 1.03f));
            else spawnedBoards[3] = SpawnBoard(3, new Vector2(0, -50), "board4", new Vector2(1.34f, 1.03f));

            // center selectors. 
            selectorsHolder.localPosition = new Vector2(180, selectorsHolder.localPosition.y);


        }
        else if (all_boards == 2)
        {
            // check which combo of boards was chosen (in duos).
            if (board_1 == 1 && board_2 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-195, 0, 0), "board1", new Vector2(0.78f, 0.60f));
                spawnedBoards[1] = SpawnBoard(1, new Vector3(195, 0, 0), "board2", new Vector2(0.78f, 0.60f));
            }
            else if (board_1 == 1 && board_3 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-195, 0, 0), "board1", new Vector2(0.78f, 0.60f));
                spawnedBoards[2] = SpawnBoard(2, new Vector3(195, 0, 0), "board3", new Vector2(0.78f, 0.60f));
            }
            else if (board_1 == 1 && board_4 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-195, 0, 0), "board1", new Vector2(0.78f, 0.60f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(195, 0, 0), "board4", new Vector2(0.78f, 0.60f));
            }

            else if (board_2 == 1 && board_3 == 1)
            {
                spawnedBoards[1] = SpawnBoard(1, new Vector3(-195, 0, 0), "board2", new Vector2(0.78f, 0.60f));
                spawnedBoards[2] = SpawnBoard(2, new Vector3(195, 0, 0), "board3", new Vector2(0.78f, 0.60f));
            }
            else if (board_2 == 1 && board_4 == 1)
            {
                spawnedBoards[1] = SpawnBoard(1, new Vector3(-195, 0, 0), "board2", new Vector2(0.78f, 0.60f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(195, 0, 0), "board4", new Vector2(0.78f, 0.60f));
            }

            else if (board_3 == 1 && board_4 == 1)
            {
                spawnedBoards[2] = SpawnBoard(2, new Vector3(-195, 0, 0), "board3", new Vector2(0.78f, 0.60f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(195, 0, 0), "board4", new Vector2(0.78f, 0.60f));
            }

            // center selectors.
            selectorsHolder.localPosition = new Vector2(125, selectorsHolder.localPosition.y);

        }
        else if (all_boards == 3)
        {
            if (board_1 == 1 && board_2 == 1 && board_3 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-206, 180, 0), "board1", new Vector2(0.76f, 0.57f));
                spawnedBoards[1] = SpawnBoard(1, new Vector3(206, 180, 0), "board2", new Vector2(0.76f, 0.57f));
                spawnedBoards[2] = SpawnBoard(2, new Vector3(0, -290, 0), "board3", new Vector2(0.76f, 0.57f));
            }
            else if (board_1 == 1 && board_2 == 1 && board_4 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-206, 180, 0), "board1", new Vector2(0.76f, 0.57f));
                spawnedBoards[1] = SpawnBoard(1, new Vector3(206, 180, 0), "board2", new Vector2(0.76f, 0.57f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(0, -290, 0), "board4", new Vector2(0.76f, 0.57f));
            }
            else if (board_1 == 1 && board_3 == 1 && board_4 == 1)
            {
                spawnedBoards[0] = SpawnBoard(0, new Vector3(-206, 180, 0), "board1", new Vector2(0.76f, 0.57f));
                spawnedBoards[2] = SpawnBoard(2, new Vector3(206, 180, 0), "board3", new Vector2(0.76f, 0.57f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(0, -290, 0), "board4", new Vector2(0.76f, 0.57f));
            }

            else if (board_2 == 1 && board_3 == 1 && board_4 == 1)
            {
                spawnedBoards[1] = SpawnBoard(1, new Vector3(-206, 180, 0), "board2", new Vector2(0.76f, 0.57f));
                spawnedBoards[2] = SpawnBoard(2, new Vector3(206, 180, 0), "board3", new Vector2(0.76f, 0.57f));
                spawnedBoards[3] = SpawnBoard(3, new Vector3(0, -290, 0), "board4", new Vector2(0.76f, 0.57f));
            }

            // center selectors.
            selectorsHolder.localPosition = new Vector2(60, selectorsHolder.localPosition.y);

        }
        else if (all_boards == 4)
        {
            // spawn all boards. 
            spawnedBoards[0] = SpawnBoard(0, new Vector3(-206, 180, 0), "board1", new Vector2(0.76f, 0.57f));
            spawnedBoards[1] = SpawnBoard(1, new Vector3(206, 180, 0), "board2", new Vector2(0.76f, 0.57f));
            spawnedBoards[2] = SpawnBoard(2, new Vector3(-206, -290, 0), "board3", new Vector2(0.76f, 0.57f));
            spawnedBoards[3] = SpawnBoard(3, new Vector3(206, -290, 0), "board4", new Vector2(0.76f, 0.57f));

            selectorsHolder.localPosition = new Vector2(0, selectorsHolder.localPosition.y);
        }

        int order_i = 0;
        for (int i = 0; i < spawnedBoards.Length; i++)
        {
            if (spawnedBoards[i] != null)
            {
                spawnedBoards_inOrder[order_i] = spawnedBoards[i];
                order_i++;
            }
        }
        // we make a duplicate so we dont have to reposition whenever changing between view modes.
        _board_pos = new Vector3[4];
        for (int i = 0; i < 4; i++)
        {
            if (spawnedBoards[i] != null) _board_pos[i] = spawnedBoards[i].transform.position;
        }

        // this array can only be instantiated at the end since we have to wait until the boards are spawned.
        
        #region fill preset deck of cards to be called.
        // randomize order of indices;
        int[] _rand_list = new int[54];
        for (int i = 0; i < 54; i++)
            _rand_list[i] = i;
        for (int a = 0; a < 54; a++)
        {
            int rand = (int)Random.Range(0, 54);
            int tmp = _rand_list[a];
            _rand_list[a] = _rand_list[rand];
            _rand_list[rand] = tmp;
        }

        // store randomized ordered deck with its index.
        for (int i = 0; i < 54; i++)
        {
            _deck_cards_holder[i] = card_img[_rand_list[i]];
            _deck_cards_index[i] = _rand_list[i];
        }

        #endregion
        // mark the selected board as the first one available.
        int first_available_index = 0;

        while (spawnedBoards[first_available_index] == null)
            first_available_index++;

        board_selected_index = first_available_index;
        boardSelectors[board_selected_index].transform.localScale *= 1.3f; // size up to indicate selection.


        // grab currently spawned cards from all the boards that are spawned.
        for (int i = 0; i < spawnedBoards.Length; i++)
        {
            if (spawnedBoards[i] != null)
            {
                for (int j = 0; j < 16; j++)
                {
                    board_cards[i, j] = spawnedBoards[i].transform.GetChild(0).GetChild(j).gameObject;
                }
            }
        }

        if (single_player)
        {
            #region AI
            // initialize AI players
            for (int i = 0; i < 4; i++)
            {
                AIStats.ai[i] = new AI_Player(Random.Range(1, 5));
            }

            // generate boards for the AI according to their number of boards.
            for (int i = 0; i < AIStats.ai.Length; i++)
            {
                AIStats.ai[i].GenerateName(AI_NAMES[i]);
                int num_boards = AIStats.ai[i].num_boards;
                for (int b = 0; b < num_boards; b++)
                {
                    AIStats.ai[i].ai_boards[b] = new Board("AI");
                    AIStats.ai[i].ai_boards[b].ai_cards = GenerateRandomBoard();
                }
            }
            #endregion
        }

        // start the coroutine that calls a card every set amount of time. 
        StartCoroutine(CallSequence());
    }
	
	// Update is called once per frame
	void Update () {

        board_select_toggle = viewToggle.GetComponent<Toggle>().isOn;

        if (!win_message.is_disp && !win_exists)
        {

            #region Mobile Input
            /*
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // get position of touch.
                    touch_pos = (Vector2)Input.GetTouch(0).position;
                    Ray ray = Camera.main.ScreenPointToRay(touch_pos);

                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.Log(hit.transform.parent.transform.parent.name);

                        // the cards reside in a card holder, so we need to get the parent of that card holder to get the board name. 
                        Transform Board = hit.transform.parent.transform.parent;
                        if (Board.name == "Board1 (Clone)") boardPlayed = 0;

                    }
                }
            }
            */
            #endregion

            #region PC Input
                if (Input.GetMouseButtonDown(0))
                {
                    // get position of touch.
                    Vector3 mouse_pos = Input.mousePosition;
                    touch_pos = mouse_pos;
                    Ray ray = Camera.main.ScreenPointToRay(touch_pos);

                    if (Physics.Raycast(ray, out hit))
                    {

                        Transform card_selected = hit.transform;
                        Transform Board = card_selected.parent.transform.parent;

                        if (Board.name == "Board1(Clone)") boardPlayed = 0;
                        if (Board.name == "Board2(Clone)") boardPlayed = 1;
                        if (Board.name == "Board3(Clone)") boardPlayed = 2;
                        if (Board.name == "Board4(Clone)") boardPlayed = 3;

                        // mark selected card.
                        int i = 0;
                        Transform[] cards = new Transform[16];
                        for (int j = 0; j < 16; j++)
                        {
                            cards[j] = Board.Find("Cards").GetChild(j).transform;
                        }
                        int t = 0;
                        foreach (Sprite card in card_img)
                        {
                            if (card.name == card_selected.GetComponent<Image>().sprite.name) break;
                            t++;
                        }
                        while (cards[i] != card_selected)
                            i++;

                        if (isSelected[boardPlayed, i] == false)
                        {
                            // generate random rotation for bean placement. 
                            Sprite tmp;
                            
                            float rot_amount = 0;
                            rot_amount = Random.Range(0, 359);
                            Vector3 rot = new Vector3(0, 0, rot_amount);

                            tmp = recently_played[0];
                            recently_played[0] = card_selected.GetComponent<Image>().sprite;
                            recently_played[1] = tmp;

                            // spawn bean and add it to array that saves bean locations.
                            beansOnBoard[boardPlayed, i] = (GameObject)Instantiate(beans, card_selected.transform.parent.transform.localPosition, Quaternion.Euler(rot));
                            beansOnBoard[boardPlayed, i].transform.SetParent(card_selected.transform, false);

                        }
                        else if (beansOnBoard[boardPlayed, i] != null && isSelected[boardPlayed, i] == true)
                        {
                            // destroy bean at location pressed.
                            Destroy(beansOnBoard[boardPlayed, i]);
                            beansOnBoard[boardPlayed, i] = null;
                        }

                        // toggle selection. 
                        isSelected[boardPlayed, i] = !isSelected[boardPlayed, i];
                        if (isSelected[boardPlayed, i])
                        {


                            if (!_cards_played.ContainsKey(boardPlayed))
                                _cards_played.Add(boardPlayed, new Dictionary<int, int>());

                            _cards_played[boardPlayed].Add(i, t);
                        }
                        else if (!isSelected[boardPlayed, i])
                        {
                            if (_cards_played.ContainsKey(boardPlayed))
                                _cards_played[boardPlayed].Remove(i);
                        }



                        // detects eligible plays.
                        if (isSelected[boardPlayed, i] == true) {
                            for (int a = 0; a < _cards_called.Count; a++)
                            {
                                if (card_img[_cards_called[a]] == card_selected.GetComponent<Image>().sprite)
                                {
                                    eligible_play[boardPlayed, i] = true;
                                    break;
                                }
                                else
                                {
                                    eligible_play[boardPlayed, i] = false;
                                }

                            }
                        }
                        else
                        {
                            eligible_play[boardPlayed, i] = true;
                        }


                    }
                }
                #endregion
        }
        if (!board_select_toggle)
        {

            // activate all full view boards, deactivate buttons and section view boards. 
            for (int i = 0; i < 4; i++)
            {
                boardSelectors[i].SetActive(false);

                if (spawnedBoards[i] != null)
                {
                    spawnedBoards[i].SetActive(true);
                    if (all_boards > 1) 
                        spawnedBoards[i].transform.localScale = new Vector2(0.76f, 0.57f);
                    else 
                        spawnedBoards[i].transform.localScale = new Vector2(1.3f, 1f);

                    spawnedBoards[i].transform.position = _board_pos[i];
                }

            }


        }
        else if (board_select_toggle)
        {
            // activate selector buttons and deactivate full view boards. 
            for (int i = 0; i < 4; i++)
            {
                boardSelectors[i].SetActive(true);
            }
            for (int i = all_boards; i < 4; i++)
            {
               // boardSelectors[i].GetComponent<Button>().interactable = false;
                boardSelectors[i].SetActive(false);
            }
            // put first board on main.
            for (int i = 0; i < 4; i++)
            {
                if (spawnedBoards[i] != null)
                {
                    spawnedBoards[i].SetActive(false);
                }
            }
            spawnedBoards_inOrder[board_selected_index].SetActive(true);
            spawnedBoards_inOrder[board_selected_index].transform.localPosition = new Vector2(0, -50);
            spawnedBoards_inOrder[board_selected_index].transform.localScale = new Vector2(1.3f, 1);

            // scale up the deck to fit better on screen. 
            //spawnedBoards[board_selected_index].SetActive(true);
        }
	}
    #endregion




    #region Custom Functions
    // spawns board.
    private GameObject SpawnBoard(int index, Vector3 pos, string board_name, Vector3 scale)
    {
        GameObject board = Instantiate(boards[index], pos, boards[index].transform.rotation);
        // board.transform.localScale = new Vector2(1, 1);
        board.transform.SetParent(gameCanvas.transform, false);
        // fill board with saved cards. 
        FillBoard(board.transform, board_name);
        board.transform.localScale = scale;
        board.transform.SetSiblingIndex(4);

        return board;
    }
    // fills boards with saved cards. 
    private void FillBoard(Transform board, string board_name)
    {
        Transform board_cards = board.transform.Find("Cards");
        Transform[] cards = new Transform[16];

        for (int i = 0; i < board_cards.childCount; i++)
        {
            cards[i] = board_cards.transform.GetChild(i);
        }

        LoadBoard(board_name + ".sav", cards);
        
    }
    private void LoadBoard(string filename, Transform[] board)
    {
        string[] _board_data = SaveLoadController.LoadBoard(filename);

        int card_index = 0;
        for (int i = 0; i < _board_data.Length; i++)
        {
            if (_board_data[i] == "Blank2" || _board_data[i] == "Blank3" )
            {
                board[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");
            }
            else
            {
                while (_board_data[i] != card_img[card_index].name)
                    card_index++;

                board[i].GetComponent<Image>().sprite = card_img[card_index];
                card_index = 0;
            }
        }
    }

    public void ScanBoards()
    {
        PlayerStats.won_index = -1;
        if (!win_message.is_disp)
        {
            if (recently_played[0] == current_card_called)
            {
                can_win = true;
            }
            else
            {
                can_win = false;
            }


            bool eligible_board = false;
            int board_with_win = -1;

            // go board by board.
            for (int board_i = 0; board_i < spawnedBoards.Length; board_i++)
            {
                if (spawnedBoards[board_i] != null)
                {
                    // only eligible if all placed cards have been called by the deck.
                    foreach (var item in _cards_played[board_i])
                    {
                        Debug.Log("called card:: "+item.Key);
                        if (!_cards_called.Contains(_cards_played[board_i][item.Key]))
                        {
                            eligible_board = false;
                            break;
                        }
                        else if (_cards_called.Contains(_cards_played[board_i][item.Key]))
                        {
                            eligible_board = true;
                        }
                    }

                    // check patterns on eligible board.
                    if (eligible_board)
                    {
                        if (gameMode == 0)
                        {
                            #region vertical check
                            for (int i = 0; i < 4; i++)
                            {
                                if (isSelected[board_i, i] && isSelected[board_i, i+4] && isSelected[board_i, i+8] && isSelected[board_i, i+12])
                                {
                                    PlayerPrefs.SetInt("vertical_play", i + 1);
                                    PlayerStats.winning_board[board_i] = true;
                                    PlayerStats.won_index = board_i;

                                    board_with_win = board_i;
                                }
                            }
                            #endregion

                            #region horizontal check
                            if (isSelected[board_i, 0] && isSelected[board_i, 1] && isSelected[board_i, 2] && isSelected[board_i, 3])
                            {
                                PlayerPrefs.SetInt("horizontal_play", 1);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[1, 0] = true;
                            }
                            else if (isSelected[board_i, 4] && isSelected[board_i, 5] && isSelected[board_i, 6] && isSelected[board_i, 7])
                            {
                                PlayerPrefs.SetInt("horizontal_play", 2);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[1, 1] = true;
                            }
                            else if (isSelected[board_i, 8] && isSelected[board_i, 9] && isSelected[board_i, 10] && isSelected[board_i, 11])
                            {
                                PlayerPrefs.SetInt("horizontal_play", 3);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[1, 2] = true;
                            }
                            else if (isSelected[board_i, 12] && isSelected[board_i, 13] && isSelected[board_i, 14] && isSelected[board_i, 15])
                            {
                                PlayerPrefs.SetInt("horizontal_play", 4);
                                PlayerStats.winning_board[board_i] = true;
                                //board_with_win[board_i] = true;
                                player_won[1, 3] = true;
                            }

                            #endregion
                            #region diagonal check
                            // top left to bottom right.
                            if (isSelected[board_i, 0] && isSelected[board_i, 5] && isSelected[board_i, 10] && isSelected[board_i, 15])
                            {
                                PlayerPrefs.SetInt("diagonal_play", 1);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[2, 0] = true;
                            }
                            // top right to bottom left.
                            else if (isSelected[board_i, 3] && isSelected[board_i, 6] && isSelected[board_i, 9] && isSelected[board_i, 12])
                            {
                                PlayerPrefs.SetInt("diagonal_play", 2);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[2, 1] = true;
                            }
                            #endregion
                        }
                        if (gameMode == 0 || gameMode == 1)
                        {
                            #region pozito check
                            if (isSelected[board_i, 5] && isSelected[board_i, 6] && isSelected[board_i, 9] && isSelected[board_i, 10])
                            {
                                PlayerPrefs.SetInt("posito_play", 1);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[3, 0] = true;
                            }
                            #endregion
                        }
                        if (gameMode == 0 || gameMode == 2)
                        {
                            #region corners check
                            if (isSelected[board_i, 0] && isSelected[board_i, 3] && isSelected[board_i, 12] && isSelected[board_i, 15])
                            {
                                PlayerPrefs.SetInt("corners_play", 1);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;
                                //board_with_win[board_i] = true;
                                player_won[4, 0] = true;
                            }
                            #endregion
                        }
                        
                        if (gameMode == 3)
                        {
                            // double diagonal check. 
                            #region jaras check
                            if ((isSelected[board_i, 0] && isSelected[board_i, 5] && isSelected[board_i, 10] && isSelected[board_i, 15]) && (isSelected[board_i, 3] && isSelected[board_i, 6] && isSelected[board_i, 9] && isSelected[board_i, 12]))
                            {
                                PlayerPrefs.SetInt("jaras_play", 1);
                                PlayerStats.winning_board[board_i] = true;
                                PlayerStats.won_index = board_i;

                                //board_with_win[board_i] = true;
                                player_won[5, 0] = true;
                            }
                            #endregion
                        }
                        if (gameMode == 4)
                        {
                            // full house (full board) check
                            #region cartallena check
                            for (int i = 0; i < 16; i++)
                            {
                                if (!isSelected[board_i, i])
                                {
                                    PlayerPrefs.SetInt("cartallena_play", 0);
                                    PlayerStats.winning_board[board_i] = true;
                                    PlayerStats.won_index = board_i;

                                    player_won[6, 0] = false;
                                    break;
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("cartallena_play", 1);
                                    PlayerStats.winning_board[board_i] = true;
                                    PlayerStats.won_index = board_i;

                                    player_won[6, 0] = true;
                                }
                            }
                            #endregion
                        }


                    }
                }
            }

            // check if there was a win. 
            win_exists = false;
            for (int i = 0; i < 7; i++)
            {
                if (player_won[i, 0] || player_won[i, 1] || player_won[i, 2] || player_won[i, 3])
                {
                    if (can_win)
                    {
                        win_exists = true;
                        win_message.DispWinMessage(true);
                    }
                    else
                    {
                        win_exists = false;

                        // highlight the wins the player missed. 
                        for (int j = 0; j < 4; j++)
                        {

                            //if (_board_with_win == j)
                            if (PlayerStats.winning_board[j])
                            {
                                // check which pattern to highlight.
                                if (i == 0)
                                {
                                    #region vertical highlights.
                                    if (player_won[0, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 0]));
                                        StartCoroutine(HighlightCard(board_cards[j, 4]));
                                        StartCoroutine(HighlightCard(board_cards[j, 8]));
                                        StartCoroutine(HighlightCard(board_cards[j, 12]));
                                    }
                                    if (player_won[0, 1])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 1]));
                                        StartCoroutine(HighlightCard(board_cards[j, 5]));
                                        StartCoroutine(HighlightCard(board_cards[j, 9]));
                                        StartCoroutine(HighlightCard(board_cards[j, 13]));
                                    }
                                    if (player_won[0, 2])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 2]));
                                        StartCoroutine(HighlightCard(board_cards[j, 6]));
                                        StartCoroutine(HighlightCard(board_cards[j, 10]));
                                        StartCoroutine(HighlightCard(board_cards[j, 14]));
                                    }
                                    if (player_won[0, 3])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 3]));
                                        StartCoroutine(HighlightCard(board_cards[j, 7]));
                                        StartCoroutine(HighlightCard(board_cards[j, 11]));
                                        StartCoroutine(HighlightCard(board_cards[j, 15]));
                                    }
                                    #endregion
                                }
                                else if (i == 1)
                                {
                                    #region horizontal highlights
                                    if (player_won[1, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 0]));
                                        StartCoroutine(HighlightCard(board_cards[j, 1]));
                                        StartCoroutine(HighlightCard(board_cards[j, 2]));
                                        StartCoroutine(HighlightCard(board_cards[j, 3]));
                                    }
                                    if (player_won[1, 1])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 4]));
                                        StartCoroutine(HighlightCard(board_cards[j, 5]));
                                        StartCoroutine(HighlightCard(board_cards[j, 6]));
                                        StartCoroutine(HighlightCard(board_cards[j, 7]));
                                    }
                                    if (player_won[1, 2])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 8]));
                                        StartCoroutine(HighlightCard(board_cards[j, 9]));
                                        StartCoroutine(HighlightCard(board_cards[j, 10]));
                                        StartCoroutine(HighlightCard(board_cards[j, 11]));
                                    }
                                    if (player_won[1, 3])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 12]));
                                        StartCoroutine(HighlightCard(board_cards[j, 13]));
                                        StartCoroutine(HighlightCard(board_cards[j, 14]));
                                        StartCoroutine(HighlightCard(board_cards[j, 15]));
                                    }
                                    #endregion
                                }
                                else if (i == 2)
                                {
                                    #region diagonal highlights. 
                                    if (player_won[2, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 0]));
                                        StartCoroutine(HighlightCard(board_cards[j, 5]));
                                        StartCoroutine(HighlightCard(board_cards[j, 10]));
                                        StartCoroutine(HighlightCard(board_cards[j, 15]));
                                    }
                                    if (player_won[2, 1])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 3]));
                                        StartCoroutine(HighlightCard(board_cards[j, 6]));
                                        StartCoroutine(HighlightCard(board_cards[j, 9]));
                                        StartCoroutine(HighlightCard(board_cards[j, 12]));
                                    }
                                    #endregion
                                }
                                else if (i == 3)
                                {
                                    #region pozito highlights
                                    if (player_won[3, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 5]));
                                        StartCoroutine(HighlightCard(board_cards[j, 6]));
                                        StartCoroutine(HighlightCard(board_cards[j, 9]));
                                        StartCoroutine(HighlightCard(board_cards[j, 10]));
                                    }
                                    #endregion
                                }
                                else if (i == 4)
                                {
                                    #region corner highlights.
                                    if (player_won[4, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 0]));
                                        StartCoroutine(HighlightCard(board_cards[j, 3]));
                                        StartCoroutine(HighlightCard(board_cards[j, 12]));
                                        StartCoroutine(HighlightCard(board_cards[j, 15]));
                                    }
                                    #endregion
                                }
                                else if (i == 5)
                                {
                                    #region jaras highlights.
                                    if (player_won[5, 0])
                                    {
                                        StartCoroutine(HighlightCard(board_cards[j, 0]));
                                        StartCoroutine(HighlightCard(board_cards[j, 5]));
                                        StartCoroutine(HighlightCard(board_cards[j, 10]));
                                        StartCoroutine(HighlightCard(board_cards[j, 15]));

                                        StartCoroutine(HighlightCard(board_cards[j, 3]));
                                        StartCoroutine(HighlightCard(board_cards[j, 6]));
                                        StartCoroutine(HighlightCard(board_cards[j, 9]));
                                        StartCoroutine(HighlightCard(board_cards[j, 12]));
                                    }
                                    #endregion
                                }
                                else if (i == 6)
                                {
                                    #region cartallena highlights. 
                                    if (player_won[6, 0])
                                    {
                                        for (int a = 0; a < 16; a++)
                                        {
                                            StartCoroutine(HighlightCard(board_cards[j, a]));
                                        }
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                    break;
                }
            }
            if (win_exists)
            {
                PlayerPrefs.SetInt("player_won", 0);
                money_won = Random.Range(15, 30);

                win_message.DispWinScreen(player_stars, money_won, PlayerStats.won_index, "player0");
            }
            else if (!win_exists)
            {
                // for each incorrect call, the player loses one star. better make the right call!
                if (player_stars > 0) player_stars--;
                PlayerStats.won_index = -1;
                win_message.DispWinMessage(false);
            }
            
            return;
        }

    }
    public void Quit(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void SetSelectIndex(int _index)
    {
        board_selected_index = _index;

        for (int i = 0; i < 4; i++)
            boardSelectors[i].transform.localScale = new Vector2(1, 1);

        boardSelectors[_index].transform.localScale *= 1.3f;
    }
    public void SetView(bool _is_full_view)
    {
        board_select_toggle = _is_full_view;
    }

    #region AI functions. 
    // each time the deck gets called, AI will look at boards and select a card if it has the card that was called.
    private void AI_SelectCard(int index, AI_Player player, Sprite card_called)
    {
        List<Sprite[]> AI_boards = new List<Sprite[]>();
        for (int i = 0; i < player.num_boards; i++)
        {
            if (player.ai_boards[i] != null)
                AI_boards.Add(player.ai_boards[i].ai_cards);
        }

        Sprite tmp;

        // scan boards for card called. 
        for (int i = 0; i < player.num_boards; i++)
        {
            for (int c = 0; c < 16; c++)
            {
                if (AI_boards[i][c] == card_called && !player.AI_isSelected[0, i])
                {
                    player.AI_isSelected[0, i] = true;

                    tmp = AI_recentlyPlayed[index, 0];
                    AI_recentlyPlayed[index, 0] = card_called;
                    AI_recentlyPlayed[index, 1] = tmp;

                    if (index == 0)
                    {
                        if (AI_recentlyPlayed[index, 0] != null)
                            P1_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
                        if (AI_recentlyPlayed[index, 1] != null)
                            P1_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
                    }
                    else if (index == 1)
                    {
                        if (AI_recentlyPlayed[index, 0] != null)
                            P2_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
                        if (AI_recentlyPlayed[index, 1] != null)
                            P2_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
                    }
                    else if (index == 2)
                    {
                        if (AI_recentlyPlayed[index, 0] != null)
                            P3_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
                        if (AI_recentlyPlayed[index, 1] != null)
                            P3_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
                    }
                    else if (index == 3)
                    {
                        if (AI_recentlyPlayed[index, 0] != null)
                            P4_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
                        if (AI_recentlyPlayed[index, 1] != null)
                            P4_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
                    }
                }
            }

            #region old code
            //if (AI_board1 != null && AI_board1[i] == card_called)
            //{
            //    if (!player.AI_isSelected[0, i])
            //    {
            //        player.AI_isSelected[0, i] = true;

            //        tmp = AI_recentlyPlayed[index, 0];
            //        AI_recentlyPlayed[index, 0] = card_called;
            //        AI_recentlyPlayed[index, 1] = tmp;

            //        if (index == 0)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P1_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P1_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 1)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P2_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P2_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 2)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P3_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P3_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 3)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P4_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P4_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //    }
            //}
            //if (AI_board2 != null && AI_board2[i] == card_called)
            //{
            //    if (!player.AI_isSelected[1, i])
            //    {
            //        player.AI_isSelected[1, i] = true;

            //        tmp = AI_recentlyPlayed[index, 0];
            //        AI_recentlyPlayed[index, 0] = card_called;
            //        AI_recentlyPlayed[index, 1] = tmp;

            //        if (index == 0)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P1_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P1_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 1)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P2_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P2_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 2)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P3_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P3_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 3)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P4_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P4_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //    }
            //}
            //if (AI_board3 != null && AI_board3[i] == card_called)
            //{
            //    if (!player.AI_isSelected[2, i])
            //    {
            //        player.AI_isSelected[2, i] = true;

            //        tmp = AI_recentlyPlayed[index, 0];
            //        AI_recentlyPlayed[index, 0] = card_called;
            //        AI_recentlyPlayed[index, 1] = tmp;

            //        if (index == 0)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P1_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P1_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 1)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P2_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P2_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 2)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P3_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P3_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 3)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P4_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P4_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //    }
            //}
            //if (AI_board4 != null && AI_board4[i] == card_called)
            //{
            //    if (AI_isSelected[3, i])
            //    {
            //        player.AI_isSelected[3, i] = true;

            //        tmp = AI_recentlyPlayed[index, 0];
            //        AI_recentlyPlayed[index, 0] = card_called;
            //        AI_recentlyPlayed[index, 1] = tmp;

            //        if (index == 0)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P1_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P1_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 1)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P2_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P2_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 2)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P3_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P3_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //        else if (index == 3)
            //        {
            //            if (AI_recentlyPlayed[index, 0] != null)
            //                P4_cards[0].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 0];
            //            if (AI_recentlyPlayed[index, 1] != null)
            //                P4_cards[1].GetComponent<Image>().sprite = AI_recentlyPlayed[index, 1];
            //        }
            //    }
            //}
            #endregion

        }
    }
    private void AI_ScanBoards(AI_Player player, int ai_index)
    {
        for (int board_i = 0; board_i < player.num_boards; board_i++)
        {
            if (player.ai_boards[board_i].ai_cards != null)
            {

                if (gameMode == 0)
                {
                    #region vertical check. 
                    if (player.AI_isSelected[board_i, 0] && player.AI_isSelected[board_i, 4] && player.AI_isSelected[board_i, 8] && player.AI_isSelected[board_i, 12])
                    {
                        PlayerPrefs.SetInt("AI"+ai_index+"_vertical_play", 1);
                        AIStats.won_index = board_i;

                        AI_winMethod[0] = true;
                    }
                    if (player.AI_isSelected[board_i, 1] && player.AI_isSelected[board_i, 5] && player.AI_isSelected[board_i, 9] && player.AI_isSelected[board_i, 13])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_vertical_play", 2);
                        AIStats.won_index = board_i;
                        AI_winMethod[0] = true;
                    }
                    if (player.AI_isSelected[board_i, 2] && player.AI_isSelected[board_i, 6] && player.AI_isSelected[board_i, 10] && player.AI_isSelected[board_i, 14])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_vertical_play", 3);
                        AIStats.won_index = board_i;
                        AI_winMethod[0] = true;
                    }
                    if (player.AI_isSelected[board_i, 3] && player.AI_isSelected[board_i, 7] && player.AI_isSelected[board_i, 11] && player.AI_isSelected[board_i, 15])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_vertical_play", 4);
                        AIStats.won_index = board_i;
                        AI_winMethod[0] = true;
                    }
                    #endregion
                    #region horizontal check. 
                    if (player.AI_isSelected[board_i, 0] && player.AI_isSelected[board_i, 1] && player.AI_isSelected[board_i, 2] && player.AI_isSelected[board_i, 3])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_horizontal_play", 1);
                        AIStats.won_index = board_i;
                        AI_winMethod[1] = true;
                    }
                    if (player.AI_isSelected[board_i, 4] && player.AI_isSelected[board_i, 5] && player.AI_isSelected[board_i, 6] && player.AI_isSelected[board_i, 7])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_horizontal_play", 2);
                        AIStats.won_index = board_i;
                        AI_winMethod[1] = true;
                    }
                    if (player.AI_isSelected[board_i, 8] && player.AI_isSelected[board_i, 9] && player.AI_isSelected[board_i, 10] && player.AI_isSelected[board_i, 11])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_horizontal_play", 3);
                        AIStats.won_index = board_i;
                        AI_winMethod[1] = true;
                    }
                    if (player.AI_isSelected[board_i, 12] && player.AI_isSelected[board_i, 13] && player.AI_isSelected[board_i, 14] && player.AI_isSelected[board_i, 15])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_horizontal_play", 4);
                        AIStats.won_index = board_i;
                        AI_winMethod[1] = true;
                    }
                    #endregion
                    #region diagonal check. 
                    if (player.AI_isSelected[board_i, 0] && player.AI_isSelected[board_i, 5] && player.AI_isSelected[board_i, 10] && player.AI_isSelected[board_i, 15])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_diagonal_play", 1);
                        AIStats.won_index = board_i;
                        AI_winMethod[2] = true;
                    }
                    else if (player.AI_isSelected[board_i, 3] && player.AI_isSelected[board_i, 6] && player.AI_isSelected[board_i, 9] && player.AI_isSelected[board_i, 12])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_diagonal_play", 2);
                        AIStats.won_index = board_i;
                        AI_winMethod[2] = true;
                    }
                    #endregion
                }
                if (gameMode == 0 || gameMode == 1)
                {
                    #region posito check. 
                    if (player.AI_isSelected[board_i, 5] && player.AI_isSelected[board_i, 6] && player.AI_isSelected[board_i, 9] && player.AI_isSelected[board_i, 10])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_positot_play", 1);
                        AIStats.won_index = board_i;
                        AI_winMethod[3] = true;
                    }
                    #endregion
                }
                if (gameMode == 0 || gameMode == 2)
                {
                    #region corners check. 
                    if (player.AI_isSelected[board_i, 0] && player.AI_isSelected[board_i, 3] && player.AI_isSelected[board_i, 12] && player.AI_isSelected[board_i, 15])
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_corners_play", 1);
                        AIStats.won_index = board_i;
                        AI_winMethod[4] = true;
                    }
                    #endregion
                }
                if (gameMode == 3)
                {
                    #region jaraz check. 
                    if ((player.AI_isSelected[board_i, 3] && player.AI_isSelected[board_i, 6] && player.AI_isSelected[board_i, 9] && player.AI_isSelected[board_i, 12]) && (player.AI_isSelected[board_i, 0] && player.AI_isSelected[board_i, 5] && player.AI_isSelected[board_i, 10] && player.AI_isSelected[board_i, 15]))
                    {
                        PlayerPrefs.SetInt("AI" + ai_index + "_jaraz_play", 1);
                        AIStats.won_index = board_i;
                        AI_winMethod[5] = true;
                    }

                    #endregion
                }
                if (gameMode == 4)
                {
                    #region fullhouse check. 
                    for (int i = 0; i < 16; i++)
                    {
                        if (!player.AI_isSelected[board_i, i])
                        {
                            PlayerPrefs.SetInt("AI" + ai_index + "_fullhouse_play", 0);
                            AIStats.won_index = board_i;
                            AI_winMethod[6] = false;
                            break;
                        }
                        else
                        {
                            PlayerPrefs.SetInt("AI" + ai_index + "_fullhouse_play", 1);
                            AIStats.won_index = board_i;
                            AI_winMethod[6] = true;
                        }
                    }
                    #endregion
                }
            }
        }


        win_exists = false;
        foreach (bool did_win in AI_winMethod)
        {
            if (did_win)
            {
                win_exists = true;
                StartCoroutine(TurnLight(ai_index, AI_lights[ai_index], Color.green));
                break;
            }
        }
        
        if (win_exists)
        {
            AIStats.index_ai = ai_index + 1; // save index of the AI that won.
            SaveLoadController.SaveBoard(AIStats.AI[ai_index].GetAIBoards(0), "AI" + (ai_index+1) + "_board.sav");
            win_message.DispWinScreen(AI_stars[ai_index], 0, ai_index + 1,AIStats.ai[ai_index].ai_name.text);

        }
        else if (!win_exists)
        {
            // for each incorrect call, the player loses a star.
            if (AI_stars[ai_index] > 0) AI_stars[ai_index]--;
            StartCoroutine(TurnLight(ai_index, AI_lights[ai_index], Color.red));
        }
    }
    // generates random list of 16 cards for the AI board.
    private Sprite[] GenerateRandomBoard()
    {
        // come up with an ordered list from 1-54.
        int[] card_indexes = new int[54];
        for (int i = 0; i < 54; i++)
            card_indexes[i] = i;

        Sprite[] cards = new Sprite[16];
        int rand = 0;

        for (int i = 0; i < 54; i++)
        {
            // randomize card indexes list. 
            // swap to random index.
            rand = (int)Random.Range(0, 54);
            int tmp = card_indexes[i];
            card_indexes[i] = card_indexes[rand];
            card_indexes[rand] = tmp;
        }
        // grab first 16 indexes from the random list we generated. 
        // place those first 16 into the board holder.
        for (int i = 0; i < 16; i++)
        {
            cards[i] = card_img[card_indexes[i]];
        }
        return cards;

    }
    #endregion

    #endregion
    #region Enums
    // temp ai function that turns light green or red depending on whether ai won or not. 
    IEnumerator TurnLight(int index, GameObject light, Color _color)
    {
        //light.GetComponent<Image>().color = _color;
        AI_canCall[index] = false;
        yield return new WaitForSeconds(20);
        AI_canCall[index] = true;
        //light.GetComponent<Image>().color = Color.gray;
        
    }

    IEnumerator HighlightCard(GameObject card)
    {
        if (card != null)
        {
            Debug.Log("isred");
            //card.GetComponent<Image>().color = Color.red;
            card.GetComponent<Image>().color = new Color(100, 100, 100, 0.3f);
            yield return new WaitForSeconds(0.2f);
            card.GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
            //card.GetComponent<Image>().color = Color.red;
            card.GetComponent<Image>().color = new Color(100, 100, 100, 0.3f);
            yield return new WaitForSeconds(0.2f);
            card.GetComponent<Image>().color = Color.white;
            yield return new WaitForSeconds(0.5f);
            //card.GetComponent<Image>().color = Color.red;
            card.GetComponent<Image>().color = new Color(100, 100, 100, 0.3f);
            yield return new WaitForSeconds(0.2f);
            card.GetComponent<Image>().color = Color.white;
        }
    }
    IEnumerator CallSequence()
    {
        int i = 0;
        while (i < 55 && !win_exists)
        {
            current_card_called = _deck_cards_holder[i];
            //_cards_called[i] = _deck_cards_holder[i];
            deckCard.GetComponent<Image>().sprite = _deck_cards_holder[i];
            _cards_called.Add(_deck_cards_index[i]);

            /////AI calls//////
            int rand = Random.Range(1, 21);
            for (int a = 0; a < 4; a++)
            {
                AI_SelectCard(a, AIStats.AI[a], _deck_cards_holder[i]);
                if (AI_canCall[a] && rand == (a+1)*5)
                    AI_ScanBoards(AIStats.AI[a], a);
            }

            i++;
            yield return new WaitForSeconds(deckSpeed);
        }
        if (i >= 55) Debug.Log("END OF DECK");
    }
    #endregion

}
