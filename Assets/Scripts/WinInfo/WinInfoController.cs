using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class WinInfoController : MonoBehaviour
{

    int won_index;
    int stars_earned;
    int money_earned;

    private int curr_stars;
    private int curr_money;

    private bool player_won;

    public GameObject[] board_cards;
    public Transform[] Stars;
    public Transform moneyEarned_object;
    private Text moneyEarned_txt;

    public Transform[] board_display;
    public Sprite[] card_img_winInfo;

    public RectTransform board;


    // Use this for initialization
    void Start()
    {

        Debug.Log("player:: " + PlayerStats.won_index);
        Debug.Log("AI:: " + AIStats.won_index);

        moneyEarned_txt = moneyEarned_object.GetComponent<Text>();

        // initialize variables from the preferences.
        stars_earned = PlayerPrefs.GetInt("stars_num");
        money_earned = PlayerPrefs.GetInt("money_won");
        Debug.Log("win_index:: " + PlayerStats.won_index);
        


        // load player board winner.
        if (PlayerStats.won_index > -1)
        {
            for (int i = 0; i < 16; i++)
                PlayerStats.boards[PlayerStats.won_index].cards[i] = board.GetChild(0).GetChild(i);

            PlayerStats.LoadBoard(PlayerStats.won_index);
        }
        // load AI board winner.
        else if (AIStats.won_index > -1)
        {
            for (int i = 0; i < 16; i++)
                AIStats.boards[AIStats.won_index].cards[i] = board.GetChild(0).GetChild(i);

            AIStats.LoadBoard();
        }

        // AI won.
        if (AIStats.won_index > -1)
        {
            stars_earned = 0;
            money_earned = Random.Range(5, 10);

            player_won = false;

            if (PlayerStats.coins - money_earned > 0) PlayerStats.coins -= money_earned;
            else PlayerStats.coins = 0;


            SaveLoadController.SaveStats(PlayerStats.coins, PlayerStats.stars, "player_stats.sav");
        }
        // Player won.
        else if (PlayerStats.won_index > -1)
        {
            player_won = true;

            PlayerStats.coins += money_earned;
            PlayerStats.stars += stars_earned;

            SaveLoadController.SaveStats(PlayerStats.coins, PlayerStats.stars, "player_stats.sav");
        }


        // update user stats (money, exp). 
        StartCoroutine(StarAwards(stars_earned));
        StartCoroutine(MoneyAwards(money_earned));

        StartCoroutine(GoToScene(0));
    }



    IEnumerator MoneyAwards(int earned)
    {
        int i = 0;

        while (i <= earned)
        {
            yield return new WaitForSeconds(0.05f);

            if (player_won)
                moneyEarned_txt.text = "+$" + i;
            else
                moneyEarned_txt.text = "-$" + i;

            moneyEarned_object.GetComponent<Text>().text = moneyEarned_txt.text;

            i++;
        }

    }
    IEnumerator StarAwards(int stars)
    {
        for (int i = 0; i < stars; i++) {
            yield return new WaitForSeconds(0.5f);
            Stars[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("StarWin");
        }
    }

    IEnumerator GoToScene(int scene_num)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(scene_num);
    }

    // load board from save file to display on the WinInfo screen. 
    private void LoadBoard(string filename, Transform[] board)
    {
        string[] _board_data = SaveLoadController.LoadBoard(filename + ".sav");

        // insert cards into board.
        int card_index = 0;
        for (int i = 0; i < _board_data.Length; i++)
        {
            if (_board_data[i] == "Blank2" || _board_data[i] == "Blank3")
            {
                board[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("Blank2");
            }
            else
            {
                while (_board_data[i] != card_img_winInfo[card_index].name)
                    card_index++;
                Debug.Log(i);
                board[i].GetComponent<Image>().sprite = card_img_winInfo[card_index];
                card_index = 0;
            }
        }
    }
}
