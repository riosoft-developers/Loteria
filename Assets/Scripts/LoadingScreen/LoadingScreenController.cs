//code witten by: Antonio Adame, 20237657





using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{

    AsyncOperation ao;
    public GameObject loadingScreen;
    public Slider loadingBar;
    public Text loadingText;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        Screen.orientation = ScreenOrientation.Portrait;


        LoadGame();
    }


    public void LoadGame()
    {
        StartCoroutine(loadGame());
    }
    IEnumerator loadGame()
    {
        ao = SceneManager.LoadSceneAsync(1);
        ao.allowSceneActivation = false;

        
        while (!ao.isDone)
        {
            loadingBar.value = ao.progress;
            loadingText.text = (int)(loadingBar.value*100) + "%";

            if (ao.progress == 0.9f)
            {
                
                loadingBar.value = 1f;
                loadingText.text = (int)(loadingBar.value * 100) + "%";

                ao.allowSceneActivation = true;
            }



            yield return null;
        }
        
    }
}
