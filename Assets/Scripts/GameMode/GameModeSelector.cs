using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameModeSelector : MonoBehaviour {

    // selector arrows. 
    public GameObject gameMode_left, deckSpeed_left;
    public GameObject gameMode_right, deckSpeed_right;

    public Text[] gameModeOptions, deckSpeedOptions;
    public GameObject GameModeScrollCont; // object to be scrolled on gamemode.
    public GameObject DeckSpeedScrollCont; // object to be scrolled on deckspeed.
    public GameObject Center; // reference point to scroll the options. 
    public GameObject DeckCenter;

    private int gameMode_index = 0;
    private int deckSpeed_index = 1;
    private bool gameMode_scrollerDone = true;
    private bool deckSpeed_scrollerDone = true;

    private float option_switch_speed = 10;

    private int gameMode_options_num;
    private int deckSpeed_options_num;


    // Use this for initialization
    void Start() {
        gameMode_options_num = gameModeOptions.Length;
        deckSpeed_options_num = deckSpeedOptions.Length;
    }

    // Update is called once per frame
    void Update() {

        #region gameMode selector arrows. 
        if (gameMode_index == 0)
        {
            gameMode_left.GetComponent<Button>().interactable = false;
            gameMode_right.GetComponent<Button>().interactable = true;
            //gameMode_left.SetActive(false);
            //gameMode_right.SetActive(true);
        }
        else if (gameMode_index == gameMode_options_num - 1)
        {
            gameMode_left.GetComponent<Button>().interactable = true;
            gameMode_right.GetComponent<Button>().interactable = false;
            //gameMode_left.SetActive(true);
            //gameMode_right.SetActive(false);
        }
        else
        {
            gameMode_left.GetComponent<Button>().interactable = true;
            gameMode_right.GetComponent<Button>().interactable = true;
            //gameMode_left.SetActive(true);
            //gameMode_right.SetActive(true);
        }
        #endregion

        #region deckSpeed selector arrows.
        if (deckSpeed_index == 0)
        {
            deckSpeed_left.GetComponent<Button>().interactable = false;
            deckSpeed_right.GetComponent<Button>().interactable = true;
            //deckSpeed_left.SetActive(false);
            //deckSpeed_right.SetActive(true);
        }
        else if (deckSpeed_index == deckSpeed_options_num - 1)
        {
            deckSpeed_left.GetComponent<Button>().interactable = true;
            deckSpeed_right.GetComponent<Button>().interactable = false;
            //deckSpeed_left.SetActive(true);
            //deckSpeed_right.SetActive(false);
        }
        else
        {
            deckSpeed_left.GetComponent<Button>().interactable = true;
            deckSpeed_right.GetComponent<Button>().interactable = true;
            //deckSpeed_left.SetActive(true);
            //deckSpeed_right.SetActive(true);
        }
        #endregion
    }

    public void SelectGameMode(bool right)
    {
        if (gameMode_scrollerDone)
        {
            if (right && gameMode_index + 1 < gameMode_options_num) gameMode_index++;
            else if (!right && gameMode_index > 0) gameMode_index--;

            // scroll object to position.
            StartCoroutine(LerpToGameOption(gameMode_index, right));
        }
    }
    public void SelectDeckSpeed(bool right)
    {
        if (deckSpeed_scrollerDone)
        {
            if (right && deckSpeed_index + 1 < deckSpeed_options_num) deckSpeed_index++;
            else if (!right && deckSpeed_index > 0) deckSpeed_index--;

            // scroll object to position.

            StartCoroutine(LerpToDeckSpeedOption(deckSpeed_index, right));
        }
    }
    IEnumerator LerpToDeckSpeedOption(int index, bool right)
    {
        deckSpeed_scrollerDone = false;
        Vector2 dist = new Vector2(DeckCenter.transform.position.x - deckSpeedOptions[index].transform.position.x, GameModeScrollCont.transform.position.y);
        //dist = new Vector2(Mathf.Abs(dist.x), dist.y);
        if (dist.x < 0)
        {
            while (dist.x <= -0.1f)
            {

                dist = new Vector2(dist.x + Time.deltaTime * 10, dist.y);

                DeckSpeedScrollCont.transform.position -= new Vector3(Time.deltaTime * option_switch_speed, 0, 0);

                yield return null;

            }
            if (dist.x >= -0.1f) deckSpeed_scrollerDone = true;
        }
        else if (dist.x >= 0)
        {
            while (dist.x >= 0.1f)
            {

                dist = new Vector2(dist.x - Time.deltaTime * 10, dist.y);

                DeckSpeedScrollCont.transform.position += new Vector3(Time.deltaTime * option_switch_speed, 0, 0);

                yield return null;
            }
            if (dist.x <= 0.1f) deckSpeed_scrollerDone = true;

        }

    }
    IEnumerator LerpToGameOption(int index, bool right)
    {
        gameMode_scrollerDone = false;
        Vector2 dist = new Vector2(Center.transform.position.x - gameModeOptions[index].transform.position.x, GameModeScrollCont.transform.position.y);

        if (dist.x < 0)
        {
            while (dist.x <= -0.1f)
            {

                dist = new Vector2(dist.x + Time.deltaTime * 10, dist.y);

                GameModeScrollCont.transform.position -= new Vector3(Time.deltaTime * option_switch_speed, 0, 0);

                yield return null;

            }
            if (dist.x >= -0.1f) gameMode_scrollerDone = true;
        }
        else if (dist.x >= 0)
        {
            while (dist.x >= 0.1f)
            {

                dist = new Vector2(dist.x - Time.deltaTime * 10, dist.y);

                GameModeScrollCont.transform.position += new Vector3(Time.deltaTime * option_switch_speed, 0, 0);

                yield return null;
            }
            if (dist.x <= 0.1f) gameMode_scrollerDone = true;
        }

    }

    public void SelectScene(int scene)
    {
        PlayerPrefs.SetInt("GameMode", gameMode_index);
        PlayerPrefs.SetInt("DeckSpeed", deckSpeed_index);
        SceneManager.LoadScene(scene);
    }
}
