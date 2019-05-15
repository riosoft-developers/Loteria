using UnityEngine;

using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;


[Serializable]
public class Board
{
    public Transform[] cards;
    public Sprite[] ai_cards;
    public bool isFull;

    public Board(string type)
    {
        isFull = false;
        if (type == "Player")
            cards = new Transform[16];
        else if (type == "AI")
            ai_cards = new Sprite[16];
    }
    public Board()
    {
        isFull = false;
        cards = new Transform[16];
    }
}

