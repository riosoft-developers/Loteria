using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardSelectorController : MonoBehaviour
{


    private int total_boards = 0;

    // contains all boards.
    public RectTransform[] boards;

    public Transform[] _board_1_cards;
    public Transform[] _board_2_cards;
    public Transform[] _board_3_cards;
    public Transform[] _board_4_cards;

    public GameObject[] toggles;
    public GameObject listo;
    private Button listoButton;

    public Sprite[] _card_img;

    private void Awake()
    {
        
        for (int i = 0; i < 4; i++)
        {
            PlayerStats.boards[i] = new Board();

            for (int c = 0; c < 16; c++) {

                PlayerStats.boards[i].cards[c] = boards[i].Find("Cards").GetChild(c);
            }

        }
        PlayerStats.LoadBoards(new bool[4, 54]);
        
    }
    // Use this for initialization
    void Start()
    {
        listoButton = listo.GetComponent<Button>();

        for (int k = 0; k < boards.Length; k++)
            boards[k].gameObject.SetActive(false);

        List<GameObject> d_boards = new List<GameObject>();
        // dynamically set boards positions on select screen as we find filled boards.
        for (int i = 0; i < 4; i++) {
            if (PlayerStats.Boards[i].isFull)
            {
                if(d_boards.Count == 0)
                {
                    d_boards.Add(boards[i].gameObject);
                    d_boards[d_boards.Count -1].SetActive(true);

                    d_boards[0].transform.localPosition = new Vector2(0, 0);
                    d_boards[0].transform.localScale *= 1.5f;
                }
                else if (d_boards.Count == 1)
                {
                    d_boards.Add(boards[i].gameObject);
                    d_boards[d_boards.Count - 1].SetActive(true);

                    d_boards[0].transform.localPosition = new Vector2(0, 156f);
                    d_boards[0].transform.localScale /= 1.5f; // reset scale.
                    d_boards[1].transform.localPosition = new Vector2(0, -240f);
                }
                else if (d_boards.Count == 2)
                {
                    d_boards.Add(boards[i].gameObject);
                    d_boards[d_boards.Count - 1].SetActive(true);

                    boards[0].transform.localPosition = new Vector2(-180, 156);
                    boards[1].transform.localPosition = new Vector2(180, 156);
                    boards[2].transform.localPosition = new Vector2(0, -240);
                }
                else if (d_boards.Count == 3)
                {
                    d_boards.Add(boards[i].gameObject);
                    d_boards[d_boards.Count - 1].SetActive(true);

                    boards[0].transform.localPosition = new Vector2(-180, 156);
                    boards[1].transform.localPosition = new Vector2(180, 156);
                    boards[2].transform.localPosition = new Vector2(-180, -240);
                    boards[3].transform.localPosition = new Vector2(180, -240);

                }

            }
        }
        #region old code

        //if (total_boards == 1)
        //{
        //    // only display whichever board is available, and don't show the other ones. 
        //    // get index of available board. 
        //    int j = 1;
        //    while (PlayerPrefs.GetInt("board" + j + "full") != 1)
        //        j++;

        //    // display that board while deactivating the rest. 
        //    for (int a = 0; a < boards.Length; a++)
        //    {
        //        boards[a].SetActive(false);
        //        toggles[a].GetComponent<Toggle>().isOn = false;
        //    }
        //    boards[j - 1].SetActive(true);
        //    toggles[j - 1].SetActive(false);


        //    // position said board in the middle. 
        //    boards[j - 1].transform.localPosition = new Vector2(0, 0);
        //    boards[j - 1].transform.localScale *= 1.5f;

        //}
        //else if (total_boards == 2)
        //{
        //    int a = 0;
        //    int j = 1;
        //    int[] board_indices = new int[total_boards];

        //    // gather indices of boards that are eligible.
        //    while(a <= 1)
        //    {
        //        if (PlayerPrefs.GetInt("board"+j+"full") == 1)
        //        {
        //            board_indices[a] = j - 1;
        //            a++;
        //        }
        //        j++;
        //    }

        //    // deactivate all other boards. 
        //    for (int k = 0; k < boards.Length; k++)
        //    {
        //        boards[k].SetActive(false);
        //    }
        //    boards[board_indices[0]].SetActive(true);
        //    boards[board_indices[1]].SetActive(true);

        //    // position those two boards. 
        //    Debug.Log(board_indices[0] + board_indices[1]);
        //    boards[board_indices[0]].transform.localPosition = new Vector2(0, 156f);
        //    boards[board_indices[1]].transform.localPosition = new Vector2(0, -240f);

        //}
        //else if (total_boards == 3)
        //{
        //    int a = 0;
        //    int j = 1;
        //    int[] board_indices = new int[total_boards];

        //    // gather indices of boards that are eligible.
        //    while (a <= 2)
        //    {
        //        if (PlayerPrefs.GetInt("board" + j + "full") == 1)
        //        {
        //            board_indices[a] = j - 1;
        //            a++;
        //        }
        //        j++;
        //    }

        //    // deactivate all other boards. 
        //    for (int k = 0; k < boards.Length; k++)
        //    {
        //        boards[k].SetActive(false);
        //    }
        //    boards[board_indices[0]].SetActive(true);
        //    boards[board_indices[1]].SetActive(true);
        //    boards[board_indices[2]].SetActive(true);

        //    // position those two boards. 
        //    boards[board_indices[0]].transform.localPosition = new Vector2(-180, 156);
        //    boards[board_indices[1]].transform.localPosition = new Vector2(180, 156);
        //    boards[board_indices[2]].transform.localPosition = new Vector2(0, -240);
        //}
        //else
        //{

        //    // activate all boards. 
        //    for (int k = 0; k < boards.Length; k++)
        //        boards[k].SetActive(true);

        //}
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        // only allow player to play if at least one board is selected. 
        // disable button if no boards are selected.
        int filled_board_count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (PlayerPrefs.GetInt("board" + (i + 1) + "chosen") == 1)
                filled_board_count++;
        }
        if (filled_board_count == 0)
            listoButton.interactable = false;
        else
            listoButton.interactable = true;

            // detect which board was selected. 
            for (int i = 0; i < toggles.Length; i++)
            {
                if (toggles[i].GetComponent<Toggle>().isOn && boards[i].gameObject.activeInHierarchy && toggles[i].activeInHierarchy)
                {
                    PlayerPrefs.SetInt("board" + (i + 1) + "chosen", 1);
                    if (i == 0) PlayerPrefs.SetInt("board1", i);
                    else if (i == 1) PlayerPrefs.SetInt("board2", i);
                    else if (i == 2) PlayerPrefs.SetInt("board3", i);
                    else PlayerPrefs.SetInt("board4", i);
                }
                else if (boards[i].gameObject.activeInHierarchy && !toggles[i].activeInHierarchy)
                {
                    PlayerPrefs.SetInt("board" + (i + 1) + "chosen", 1);
                    PlayerPrefs.SetInt("single_board", i);
                }
                else
                    PlayerPrefs.SetInt("board" + (i + 1) + "chosen", 0);
            }

    }



    // load boards from save file. 
    public void LoadBoard2(string filename, Transform[] board)
    {
        int card_index = 0;

        // access all string arrays in the save file. 
        string[] board_data = SaveLoadController.LoadBoard(filename);

        // transfer saved boards into the board selector screen. 
        for (int i = 0; i < board_data.Length; i++)
        {
            if (board_data[i] == "Blank2" || board_data[i] == "Blank3")
            {
                board[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");
            }
            else
            {
                // search for cards using the string aquired from the save file. 
                while (_card_img[card_index].name != board_data[i])
                    card_index++;

                board[i].GetComponent<Image>().sprite = _card_img[card_index];
                card_index = 0;
            }
        }
    }

    // converts card names from string to the actual image then passes them to the board we want to fill. 
    public void LoadBoard(Transform[] target_board, string[] file_board)
    {
        //int i = 0;
        int j = 0;
        for (int i = 0; i < file_board.Length; i++)
        {
            if (file_board[i] == "Blank2" || file_board[i] == "Blank3")
            {
                target_board[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");
            }
            else
            {
                while (file_board[i] != _card_img[j].name)
                    j++;

                target_board[i].GetComponent<Image>().sprite = _card_img[j];
                j = 0;
            }
        }
    }
    public void ToggleBoard(int board_i)
    {
        if (toggles[board_i].GetComponent<Toggle>().isOn)
            toggles[board_i].GetComponent<Toggle>().isOn = false;
        else
            toggles[board_i].GetComponent<Toggle>().isOn = true;
    }
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }
}
