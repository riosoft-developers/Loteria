using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerStats
{
    public static Board[] boards = new Board[4];

    public static int coins, stars;
    public static Dictionary<string, int> dict = new Dictionary<string, int>();

    public static int won_index = -1;
    public static bool[] winning_board = new bool[4];
    public static int stars_won = 0;
    public static int money_won = 0;

    public static int Coins
    {
        get
        {
            return coins;
        }
        set
        {
            coins = value;
        }
    }
    public static int Stars
    {
        get
        {
            return stars;
        }
        set
        {
            stars = value;
        }
    }
    public static Board[] Boards
    {
        get
        {
            return boards;
        }
        set
        {
            boards = value;
        }
    }


    // load board on index.
    public static void LoadBoard(int index)
    {
        string filename = "Board" + (index+1) + ".sav";
        string[] board_data = SaveLoadController.LoadBoard(filename);

        int card_i = 0;
        foreach (string cardname in board_data)
        {
            if (board_data != null)
                boards[index].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>(cardname);
            else if (board_data == null)
                boards[index].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");

            card_i++;
        }
    }
    // call this if you only need to load cards into the boards without affecting card selectors.
    public static void LoadBoards()
    {
        string[] board_data;
        for (int i = 0; i < boards.Length; i++)
        {
            string filename = "board" + (i + 1) + ".sav";
            board_data = SaveLoadController.LoadBoard(filename);

            int card_i = 0;
            foreach (string cardname in board_data)
            {

                if (board_data != null)
                    boards[i].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>(cardname);
                else if (board_data == null)
                    boards[i].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");

                card_i++;
            }

        }
    }
    // call this if loading the board will affect selected cards on board creation.
    public static void LoadBoards(bool[,] already_picked)
    {
        string[] board_data;
        for (int i = 0; i < boards.Length; i++)
        {
            string filename = "board" + (i + 1) + ".sav";
            board_data = SaveLoadController.LoadBoard(filename);
            boards[i].isFull = true;

            int card_i = 0;
            if (board_data != null)
            {
                foreach (string cardname in board_data)
                {
                    boards[i].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>(cardname);
                    if (cardname == "Blank2" || cardname == "Blank3") {
                        boards[i].isFull = false;
                    }
                        //already_picked[ i, dict[cardname] ] = true;

                    card_i++;
                }
            }
            else if (board_data == null)
            {
                boards[i].cards[card_i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");
                boards[i].isFull = false;
                card_i++;
            }

            //Debug.Log(i + "::" + boards[i].isFull);
        }
    }
    
    public static void SaveBoards(Board[] boards)
    {

    }
}
