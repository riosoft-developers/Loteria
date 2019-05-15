using UnityEngine;

using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class Player
{

    // Boards here will refer to the set of cards in the board.. 
    // NOT the board object itself.
    private Board[] boards;

    public int won_index = -1;
    public int coins = 0;
    public int exp = 0;

    
    public Board GetBoard(int index)
    {
        if (index > 4) Debug.LogError("Invalid Board Selection");
        return boards[index];    
    }

    public void SetBoards(Board[] b)
    {
        boards = b;
    }

    //public void SetBoards(Transform[] board_1, Transform[] board_2, Transform[] board_3, Transform[] board_4)
    //{
    //    Board1 = board_1;
    //    Board2 = board_2;
    //    Board3 = board_3;
    //    Board4 = board_4;
    //}

    public void SetBoard(Board b, int i)
    {
        boards[i] = b;
    }
    //public void SetBoard(Transform[] board, int index)
    //{
    //    if (index == 1) Board1 = board;
    //    else if (index == 2) Board2 = board;
    //    else if (index == 3) Board3 = board;
    //    else if (index == 4) Board4 = board;
    //}

}
