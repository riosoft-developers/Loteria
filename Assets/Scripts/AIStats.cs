using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


using System;

public static class AIStats
{
    public static AI_Player[] ai = new AI_Player[4];
    public static Board[] boards = new Board[4];
    public static int won_index = -1;
    public static int index_ai = -1;

    public static AI_Player[] AI { 
        get
        {
            return ai;
        }
        set
        {
            ai = value;
        }
    }

    // load board on index.
    public static void LoadBoard()
    {
        string filename = "AI" + (index_ai) + "_board.sav";
        string[] board_data = SaveLoadController.LoadBoard(filename);

        int card_i = 0;
        foreach (string cardname in board_data)
        {
            if (board_data != null)
                boards[won_index].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>(cardname);
            else if (board_data == null)
                boards[won_index].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");

            card_i++;
        }
    }
}