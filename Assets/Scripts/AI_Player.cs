using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AI_Player
{
    // AI boards. 
    //private Sprite[] AI_Board1, AI_Board2, AI_Board3, AI_Board4;
    public Board[] ai_boards;
    public bool[,] AI_isSelected;
    public int num_boards;
    public Text ai_name;

    public int win_index = -1;



    public AI_Player()
    {
        ai_boards = new Board[4];
        AI_isSelected = new bool[4, 16];
        num_boards = 0;
    }
    public AI_Player(int n)
    {
        ai_boards = new Board[4];
        AI_isSelected = new bool[4, 16];
        num_boards = n;
    }
    
    public void SetAIBoard(Sprite[] cards, int index)
    {
        ai_boards[index].ai_cards = cards;
    }
    public Sprite[] GetAIBoards(int index)
    {
        return ai_boards[index].ai_cards;
    }

    public void GenerateName(Text obj)
    {
        ai_name = obj;

        string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        int[] rand_index = { (int)Random.Range(0, 26), (int)Random.Range(0, 26) };

        this.ai_name.text = "" + letters[rand_index[0]] + letters[rand_index[1]] + "";
    }
}
