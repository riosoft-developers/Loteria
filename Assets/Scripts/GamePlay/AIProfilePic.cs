using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AIProfilePic : MonoBehaviour
{
    public Text[] _name = new Text[4];

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < name.Length; i++)
            GenerateRandomName(_name[i]);
    }



    // generate initial letters for the name on the AI profile picture.
    private void GenerateRandomName(Text name)
    {
        string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

        int[] rand_index = { (int)Random.Range(0, 26), (int)Random.Range(0, 26) };
        
        Debug.Log(name.text);
        name.text = "" + letters[rand_index[0]] + letters[rand_index[1]] + "";
    }
}
