using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinController : MonoBehaviour
{


    public bool is_disp = false;

    public RectTransform winScreen;
    public RectTransform winMessage;
    public RectTransform buenasButton;
    public RectTransform stackButton;

    // button displayed after someone wins that takes the user to the next screen.
    public RectTransform nextButton;

    public RectTransform congratsPanel;
    public Text congratsText;

    public Text timerText;
    public Text winText;
    public Text loseText;

    public Text goingToNextText;
   

    // Use this for initialization
    void Start()
    {
        winMessage.gameObject.SetActive(false);
    }

    // displays a congratulations screen with the name of the player that won.
    public void DispWinScreen(int stars, int money_won, int player_won, string player)
    {
        PlayerStats.money_won = money_won;
        PlayerStats.stars_won = stars;

        PlayerStats.stars += stars;
        PlayerStats.coins += money_won;
       
        // disable the stuff at the bottom. 
        buenasButton.gameObject.SetActive(false);
        winMessage.gameObject.SetActive(false);
        stackButton.gameObject.SetActive(false);

        winScreen.gameObject.SetActive(true);
        //nextButton.gameObject.SetActive(true);
        //congratsPanel.gameObject.SetActive(true);

        if (player == "player0")
            congratsText.text = "¡ganaste!";
        else congratsText.text = "¡" + player + " gano!";

        StartCoroutine(GoToScene(5, 3));
    }

    public void DispWinMessage(bool player_won)
    {
        //is_disp = true;
        is_disp = false;
        buenasButton.GetComponent<Button>().interactable = false;

        winMessage.gameObject.SetActive(true);
        //start animation. 
        winMessage.GetComponent<Animator>().SetBool("isMessage", true);

        

        timerText.enabled = false;
        winText.enabled = false;
        loseText.enabled = false;
        goingToNextText.enabled = false;



        StartCoroutine(DrumRoll(player_won));
    }

    // go to scene after a timer
    IEnumerator GoToScene(int scene, int timer)
    {
        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(scene);
    }
    IEnumerator DrumRoll(bool player_won)
    { 

        if (player_won) {
            winText.gameObject.SetActive(true);
            winText.enabled = true;

            loseText.gameObject.SetActive(false);
            loseText.enabled = false;

            yield return new WaitForSeconds(3);


            // display who won. 

            winText.gameObject.SetActive(false);
            winText.enabled = false;

            loseText.gameObject.SetActive(false);
            loseText.enabled = false;

            // display message that player is being taken to next screen. 
            goingToNextText.gameObject.SetActive(true);
            goingToNextText.enabled = true;

            while (true)
            {
                goingToNextText.text = "Please Wait.";
                yield return new WaitForSeconds(0.5f);
                goingToNextText.text = "Please Wait..";
                yield return new WaitForSeconds(0.5f);
                goingToNextText.text = "Please Wait...";
                yield return new WaitForSeconds(0.5f);


            }
            //yield return new WaitForSeconds(3);

        }
        else {
            winText.gameObject.SetActive(false);
            winText.enabled = false;

            loseText.gameObject.SetActive(true);
            loseText.enabled = true;


            yield return new WaitForSeconds(3);

            winText.gameObject.SetActive(false);
            loseText.gameObject.SetActive(false);
            timerText.gameObject.SetActive(true);

            winText.enabled = false;
            loseText.enabled = false;
            timerText.enabled = true;

            StartCoroutine(DisableButton(buenasButton));
        }
    }

    IEnumerator DisableButton(RectTransform button)
    {
        float timer = 12;
        timerText.text = "" + timer;

        button.GetComponent<Button>().interactable = false;
        //yield return new WaitForSeconds(15);
        while (timer >= 0)
        {
            timer -= Time.deltaTime;
            timerText.text = "" + (int)timer;
            yield return null;
        }
        timerText.enabled = false;
        winMessage.gameObject.SetActive(false);
        button.GetComponent<Button>().interactable = true;

    }
}
