// code written by: Antonio Adame, 20237657


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.UI;


public static class SaveLoadController {

    public static void DeleteFile(string name)
    {
        File.Delete(Application.persistentDataPath + "/" + name);
    }

    // saves things such as coins and experience the player earns after each match. 
    public static void SaveStats(int coins, int exp, string filename)
    {
        //JSON. 
        



        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Create);

        // transfer coins and experience to class object.
        PlayerData pd = null;
        pd = new PlayerData(coins, exp);

        // save stats to file. 
        bf.Serialize(stream, pd);
        stream.Close();
    }
    public static void LoadStats(string filename)
    {
        if (File.Exists(Application.persistentDataPath + "/" + filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Open);

            PlayerData pd = (PlayerData)bf.Deserialize(stream);
            stream.Close();

            PlayerStats.Coins = pd.coins;
            PlayerStats.Stars = pd.exp;
        }
        else
        {
            // default to 0 if no save file is found. 
            Debug.LogError("ERROR: Stats File Not Found!");
            PlayerStats.Coins = 0;
            PlayerStats.Stars = 0;
        }
    }
    public static void SaveBoards(Board[] boards)
    {
        BinaryFormatter bf;
        FileStream stream;
        for (int i = 0; i < boards.Length; i++)
        {
            string filename = "Board" + (i + 1) + ".sav";
            bf = new BinaryFormatter();
            stream = new FileStream(Application.persistentDataPath + "/" + filename, FileMode.Create);

            PlayerData pd = new PlayerData(boards[i].cards);
            bf.Serialize(stream, pd);
            stream.Close();
        }

    }
    public static void SaveBoard(Transform[] board, string file_name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + file_name, FileMode.Create);

        PlayerData pd = null;
        pd = new PlayerData(board);

        // save to file.
        bf.Serialize(stream, pd);
        stream.Close();
    }
    // this overload function can be used when you only wish to save the list of sprites instead of the full TRANSFORM object.
    public static void SaveBoard(Sprite[] board, string file_name)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/" + file_name, FileMode.Create);

        PlayerData pd = null;
        pd = new PlayerData(board);

        // save to file.
        bf.Serialize(stream, pd);
        stream.Close();
    }
    public static string[] LoadBoard(string file_name)
    {
        if (File.Exists(Application.persistentDataPath + "/" + file_name))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/" + file_name, FileMode.Open);

            PlayerData pd = (PlayerData)bf.Deserialize(stream);
            stream.Close();

            return pd.board;
        }
        else
        {
            Debug.LogError("ERROR: Load File Not Found!");
            return null;
        }



    }

}

[Serializable]
public class PlayerData
{
    public string[] board = new string[16];
    public int coins;
    public int exp;

    //-----------------------------//
    public Player player; //<<-- we don't want this variable here but binary formatter is being a B... 
    // Due to testing some stuff i added this variable here and since the formatter saved with this variable.. 
    // when it deserializes the BoardData object it knows this obj has 'Player' object in it. 
    // if removed.. Formatter Demands for both the BoardData object in the stream to be equal to the one its being set equal to. 
    //NEED TO RESET SAVE FILES SO WE CAN REMOVE THIS player VARIABLE THAT WE DON'T WANT HERE ANYMORE.//
    //-----------------------------//

    // index here will refer to which board we are saving to. 
    //   index of 1 means board1 is being saved etc...
    public PlayerData(Transform[] _player_board)
    {
        // copy array of board data into the local array.
        for (int i = 0; i < _player_board.Length; i++)
        {
            board[i] = _player_board[i].GetComponent<Image>().sprite.name;
        }
    }
    public PlayerData(Sprite[] _player_board)
    {
        for (int i = 0; i < _player_board.Length; i++)
        {
            board[i] = _player_board[i].name;
        }
    }
    // save stats such as level, exp and coins.
    public PlayerData(int _coins, int _exp)
    {
        coins = _coins;
        exp = _exp;
    }
}
