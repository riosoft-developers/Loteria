//code written by: Antonio Adame, 20237657



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class MenuController : MonoBehaviour {

    bool isClicked;
    int dest = 1;
    float swipeSpeed;
    bool done = true;
    bool rightSide;
    ScrollRect menuScroller;

    int index = 1;

    #region Public variables
    //outside scripts. 
    BoardCreatorController bc;
    LanguageEditor languageEditor;

    public RectTransform panel; // holds panel container.
    public RectTransform[] _pan; // holds individual panel.
    public RectTransform center; // center pivot;

    // we use these to disable scrolling when settings are opened.
    public RectTransform menuScrollerCont;

    public GameObject[] _selectors; // slider selectors that will animate when on current menu slide.

    // Settings spawner variables. 
    public Canvas menuObj;
    public GameObject settingsObj;
    public GameObject settingsFilter;

    // stats displays. 
    public Transform coinDisp;
    public Transform starDisp;
    #endregion
    #region Private variables
    // general stats. 
    private int coins, stars;

    private float[] distance; // holds distances from center to each panel. 
    private bool isDragging = false;
    private int panDistance; // distance between buttons. 
    private int minPanNum; // number of panel closest to the center.

    //--//
    private int currMin;
    private bool isChanged;

    // animation related variables.
    private Animator[] _selectAnim = new Animator[3];
    public bool mainIsOnScreen, amigosIsOnScreen, tablasIsOnScreen;

    private Vector2 curr_menu_pos;
    private Vector2 last_menu_pos;
    private bool is_draging;
    #endregion

    private void Awake()
    {
        // set card dict. 
        

        //save_load.DeleteFile("player_stats.sav");
        //save_load.DeleteFile("board1.sav");
        //save_load.DeleteFile("board2.sav");
        //save_load.DeleteFile("board3.sav");
        //save_load.DeleteFile("board4.sav");

        // load in player stats. 
        SaveLoadController.LoadStats("player_stats.sav");
        coins = PlayerStats.Coins;
        stars = PlayerStats.Stars;
        
    }

    // Use this for initialization
    void Start()
    {
        coinDisp.GetComponent<Text>().text = coins.ToString();
        starDisp.GetComponent<Text>().text = stars.ToString();

        // set default of timescale.
        Time.timeScale = 1;
        // enable only portrait mode for menu screen. 
        Screen.orientation = ScreenOrientation.Portrait;

        bc = GetComponent<BoardCreatorController>();
        languageEditor = GetComponent<LanguageEditor>();


        // always initialize menu windows as inactive. 
        settingsObj.SetActive(false);
        settingsFilter.SetActive(false);

        // get animation components from selectors.
        for (int a = 0; a < _selectors.Length; a++)
        {
            _selectAnim[a] = _selectors[a].GetComponent<Animator>();
        }

        int panLen = _pan.Length;
        distance = new float[panLen];

        // distance between first panel and second panel. 
        panDistance = (int)Mathf.Abs(_pan[1].GetComponent<RectTransform>().anchoredPosition.x - _pan[0].GetComponent<RectTransform>().anchoredPosition.x);

        menuScroller = menuScrollerCont.GetComponent<ScrollRect>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (bc.isEditing)
        {
            // disable board scrollers while editing. 
            menuScrollerCont.GetComponent<ScrollRect>().enabled = false;

        }
        else if (!bc.isEditing)
        {
            // enable board scrollers when NOT scrolling. 
            menuScrollerCont.GetComponent<ScrollRect>().enabled = true;
        }

        if (isClicked)
        {
            menuScrollerCont.GetComponent<ScrollRect>().enabled = false; //-----//
            StartCoroutine(MoveMenu(index));
        }
        if (!isClicked)
        {
            

            menuScroller.GetComponent<ScrollRect>().enabled = true;  //---------//


            // find distance of each panel to the center. 
            if (!isDragging)
            {
                for (int i = 0; i < _pan.Length; i++)
                {
                    distance[i] = Mathf.Abs(center.transform.position.x - _pan[i].transform.position.x);
                }
                // find smallest distance. 
                float minDist = Mathf.Min(distance);
                // look for closest panel to center.
                for (int j = 0; j < _pan.Length; j++)
                {
                    if (minDist == distance[j])
                    {
                        // store location of closest panel to center.
                        if (done)
                            minPanNum = j;
                    }
                }


                // set index based on closes panel. 
                if (minPanNum == 0) index = 0;
                else if (minPanNum == 1) index = 1;
                else if (minPanNum == 2) index = 2;


                //===========================================================//
                if (done)
                {

                    LerpToPanel(minPanNum * -panDistance * 2.3f);
                }
                // determine if swipe was fast enough. 
                if (Mathf.Abs(menuScroller.velocity.x) > 600f)
                {
                    swipeSpeed = menuScroller.velocity.x;
                    done = false;
                    isChanged = false;
                }
                // 
                if (Mathf.Abs(swipeSpeed) >= 600f && !done)
                {
                    // determine which direction was swiped. 
                    if (swipeSpeed < 0)
                    {
                        if (!isChanged)
                        {
                            currMin++;
                            isChanged = true;
                        }
                        dest = currMin;
                        if (dest >= _pan.Length)
                            dest = _pan.Length - 1;
                        LerpToPanel(dest * -panDistance * 2.3f);
                    }
                    else if (swipeSpeed > 0)
                    {
                        if (!isChanged)
                        {
                            currMin--;
                            isChanged = true;
                        }
                        dest = currMin;
                        if (dest <= 0)
                            dest = 0;
                        LerpToPanel(dest * -panDistance * 2.3f);
                    }

                  
                }
                // determine if panel has arrived at destination.
                if (Mathf.Abs((((dest) * -panDistance * 2.3f) - panel.anchoredPosition.x)) < 100f)
                {
                    currMin = minPanNum;
                    done = true;
                    swipeSpeed = 0;
                    isChanged = false;
                }


            }

        }


        #region Selector Animations
        // trigger selector animations depending on which menu screen is present.
        if (index == 0)
        {

            // on tablas screen
            _selectAnim[0].SetBool("isOn", true);
            _selectAnim[1].SetBool("isOn", false);
            _selectAnim[2].SetBool("isOn", false);
        }
        else if (index == 1)
        {
            // on main screen
            _selectAnim[0].SetBool("isOn", false);
            _selectAnim[1].SetBool("isOn", true);
            _selectAnim[2].SetBool("isOn", false);
        }
        else if (index == 2)
        {
            // on amigos screen
            _selectAnim[0].SetBool("isOn", false);
            _selectAnim[1].SetBool("isOn", false);
            _selectAnim[2].SetBool("isOn", true);
        }
        #endregion
    }

    // function that will interpolate to closest panel. 
    void LerpToPanel(float pos)
    {
        float newX;
        if (isClicked)
        {
            newX = Mathf.Lerp(panel.anchoredPosition.x, pos, Time.deltaTime * 2f);

        }
        else
            newX = Mathf.Lerp(panel.anchoredPosition.x, pos, Time.deltaTime * 6f);

        Vector2 newPos = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPos;
        
    }

    public void StartDragging()
    {
        isDragging = true;
    }
    public void EndDragging()
    {
        isDragging = false;
    }

    // this function is used by the inspector when the user clicks the button.
    public void SelectorClick(int i)
    {
        if (!isClicked && !bc.isEditing)
        {
            isDragging = true;
            isClicked = true;
            done = false;
            index = i;
            currMin = i;
        }
    }
    IEnumerator MoveMenu(int index)
    {
        
        while (true)
        {
            
            LerpToPanel(index * -panDistance * 2.3f);

            if ((int)Mathf.Abs((index * -panDistance * 2.3f) - panel.anchoredPosition.x) <= 0)
            {

                done = true;
                isClicked = false;
                isDragging = false;
                break;
            }
            yield return null;
        }
       
    }

    public void ToggleSettings(bool choice)
    {
        
        float timer = settingsObj.GetComponent<Animation>().clip.length;

        if (choice)
        {
            settingsFilter.SetActive(true);
            menuScrollerCont.GetComponent<ScrollRect>().enabled = false;
            settingsObj.GetComponent<Animation>().Play();
        }
        else
        {
            languageEditor.updateSettingsAlert.SetActive(false);
            if (PlayerPrefs.GetString("lang") == "en")
                languageEditor.LangSettingText.text = "English";
            else
                languageEditor.LangSettingText.text = "Espanol";

            settingsFilter.SetActive(false);
            menuScrollerCont.GetComponent<ScrollRect>().enabled = true;
            Time.timeScale = 1;
        }
        settingsObj.SetActive(choice);
    }


    public void BoardSelectorScene()
    {
        // player presses button to play locally. 
        SceneManager.LoadScene(2);
    }
}
