//code written by: Antonio Adame, 20237657



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanguageEditor : MonoBehaviour {

    MenuController menuController;

    #region Text Fields
    [SerializeField]
    public Text LangSettingText;

    [SerializeField]
    private Text playText;
    [SerializeField]
    private Text friendlyText;
    [SerializeField]
    private Text tablasText;
    [SerializeField]
    private Text amigosText;

    [SerializeField]
    private Text settingsText;
    [SerializeField]
    private Text musicText;
    [SerializeField]
    private Text soundText;
    [SerializeField]
    private Text languageText;

    [SerializeField]
    private Text updateSettingsText;
    #endregion

    public Toggle spToggler;
    public Toggle enToggler;

    // variables used to prompt user to change settings. 
    private bool changeSettings;
    private bool newSettings = false;


    public GameObject updateSettingsAlert;

    public string lang = "en";


    // Use this for initialization
    void Start () {
        menuController = GetComponent<MenuController>();
        
        // initialize menu windows as closed. 
        updateSettingsAlert.SetActive(false);

        lang = PlayerPrefs.GetString("lang");

        if (lang == "sp")
        {
            LangSettingText.text = "Espanol";

            // text on main menu.
            playText.text = "Jugar";
            friendlyText.text = "Amistoso";
            tablasText.text = "Tablas";
            amigosText.text = "Amigos";

            // text on settings. 
            settingsText.text = "Ajustes";
            musicText.text = "Musica";
            soundText.text = "Sonido";
            languageText.text = "Lenguaje";

            // message notifications. 
            updateSettingsText.text = "Estas seguro de que quieres cambiar los ajustes?";
        }
        else if (lang == "en")
        {
            LangSettingText.text = "English";

            // text on main menu.
            playText.text = "Play";
            friendlyText.text = "Friendly";
            tablasText.text = "Boards";
            amigosText.text = "Friends";

            // text on settings. 
            settingsText.text = "Settings";
            musicText.text = "Music";
            soundText.text = "Sound";
            languageText.text = "Language";

            // message notifications. 
            updateSettingsText.text = "Are you sure you want to change the settings?";
        }
    }

    public void Update()
    {
        
        #region prev

        /*
        if (spToggler.isOn == true)
        {
            if (spToggler != currLangToggle)
            {
                
                // new language selected. 

                // prompt user if hes sure about the change. 
                updateSettingsAlert.SetActive(true);
                enToggler.enabled = false;

                if (ChangeSettings() && choiceMade)
                {
                    // if settings are to be changed, restart application. 
                    currLangToggle = spToggler;
                    PlayerPrefs.SetString("lang", "sp");
                    //Debug.Log(PlayerPrefs.GetString("lang"));

                    //resets application. 
                    SceneManager.LoadScene("MainMenu");

                    choiceMade = false;
                }
                else if (!ChangeSettings() && choiceMade)
                {
                    enToggler.enabled = true;
                    // if settings are not to be changed, simply close notification. 
                    // keep old settings.
                    spToggler.isOn = false;
                    enToggler.isOn = true;

                    updateSettingsAlert.SetActive(false);

                    choiceMade = false;
                }
            }
     
        }
        else if (enToggler.isOn == true)
        {
            if (enToggler != currLangToggle)
            {
                // new language selected. 

                // prompt user if hes sure about the change. 
                updateSettingsAlert.SetActive(true);
                spToggler.enabled = false;
                if (ChangeSettings() && choiceMade)
                {
                    // if settings are to be changed, restart application. 
                    currLangToggle = enToggler;
                    PlayerPrefs.SetString("lang", "en");
                    //Debug.Log(PlayerPrefs.GetString("lang"));

                    //resets application. 
                    SceneManager.LoadScene("MainMenu");

                    choiceMade = false;
                }
                else if (!ChangeSettings() && choiceMade)
                {
                    spToggler.enabled = true;
                    // if settings are not to be changed, simply close notification. 
                    // keep old settings.
                    enToggler.isOn = false;
                    spToggler.isOn = true;

                    updateSettingsAlert.SetActive(false);

                    choiceMade = false;
                }
            }
        }
        */
        #endregion

    }


    // prompt user to keep changed settings when user attempts to close settings menu.
    public void ApplyChanges()
    {
        if (newSettings)
        {
            updateSettingsAlert.SetActive(true);
            newSettings = false;
        }
        else
        {
            menuController.settingsObj.SetActive(false);
            menuController.settingsFilter.SetActive(false);
            menuController.menuScrollerCont.GetComponent<ScrollRect>().enabled = true;
            Time.timeScale = 1;
        }

    }
    public void ChangeLang()
    {
        newSettings = true;
        if (lang == "en")
        {
            lang = "sp";
            LangSettingText.text = "Espanol";

        }
        else if (lang == "sp")
        {
            lang = "en";
            LangSettingText.text = "English";
        }
    }

    public void ChangeSettings(bool choice)
    {
        changeSettings = choice;

        if (choice)
        {
            PlayerPrefs.SetString("lang", lang);
            SceneManager.LoadScene(0);
        }
        else
        {
            updateSettingsAlert.SetActive(false);
        }
    }
    public bool ChangeSettings()
    {
        return changeSettings;
    }


    
}
