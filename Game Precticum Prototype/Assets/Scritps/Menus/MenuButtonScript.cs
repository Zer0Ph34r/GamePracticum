﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    #region Fields

    #region Serializable Fields
    // other canvases
    [SerializeField]
    GameObject CreditsStuff;
    [SerializeField]
    GameObject InstructionsStuff;
    [SerializeField]
    GameObject MainMenuStuff;
    [SerializeField]
    Text TurnSliderText;
    [SerializeField]
    Text GridSizeSliderText;

    [SerializeField]
    GameObject loadingIcon;
    #endregion

    // Get reference ot audio manager
    AudioManager audioManager;

    #region Reset Events
    // Set all static events to null
    GemScript.callMethod Selected = null;
    GemScript.runNext fireSoundEvent = null;
    GemScript.runNext runNextMethod = null;
    GemScript.check checkGems = null;

    #endregion

    #endregion

    #region Start

    // Get Audio Manager
    private void Start()
    {
        audioManager = AudioManager.instance;

        #region Set Up Highscore Player Pref

        // check if highscores have been added to the player prefs
        if (!PlayerPrefs.HasKey("Highscore59"))
        {
            // create a new 2D array of highscores
            GlobalVariables.HIGHSCORE_TABLE = new int[6, 10];

            // set a new player prefbased highscore table
            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    PlayerPrefs.SetInt(("Highscore" + i + k), GlobalVariables.HIGHSCORE_TABLE[i, k]);
                }
            }
        }
        else
        {
            // create a new 2D array of highscores
            GlobalVariables.HIGHSCORE_TABLE = new int[6, 10];

            // set highscores based on saved highscores
            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    GlobalVariables.HIGHSCORE_TABLE[i, k] = PlayerPrefs.GetInt("Highscore" + i + k);
                }
            }
        }
        

        //// checks if playerPrefs contains a highscore
        //if (!PlayerPrefs.HasKey("Highscore"))
        //{
        //    // create new player pref key and value
        //    PlayerPrefs.SetInt("Highscore", GlobalVariables.HIGHSCORE);
        //}
        //else
        //{
        //    // set global highscore based on players previous highscore
        //    GlobalVariables.HIGHSCORE = PlayerPrefs.GetInt("Highscore");
        //}

        #endregion
    }
    #endregion

    #region Button Methods

    #region Load Scene
    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void LoadSceneButton(string sceneName)
    {
        // play click
        audioManager.PlayClick();
        // get laoding text
        loadingText();
        // laod scene
        SceneManager.LoadScene(sceneName);

    }
    #endregion

    #region Tutorial Button
    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {
        audioManager.PlayClick();
        MainMenuStuff.transform.gameObject.SetActive(false);
        InstructionsStuff.transform.gameObject.SetActive(true);
    }
    #endregion

    #region Credits Button
    /// <summary>
    /// Open credits canvas and close button canvas
    /// </summary>
    public void CreditsButton()
    {
        audioManager.PlayClick();
        CreditsStuff.transform.gameObject.SetActive(true);
        MainMenuStuff.transform.gameObject.SetActive(false);
    }
    #endregion

    #region Slider Button
    // Gets current slider setting
    public void SubmitSliderSetting(Slider mainSlider)
    {
        GlobalVariables.TURNS = (int)(mainSlider.value * 10);

        // displays vlaue to player
        TurnSliderText.text = "# of Turns: " + GlobalVariables.TURNS;
    }

    // Gets current slider setting
    public void SubmitGridSliderSetting(Slider mainSlider)
    {
        //Displays the value of the slider in the console.
        GlobalVariables.TABLE_SIZE = (int)mainSlider.value;
        GridSizeSliderText.text = "Grid Size: " + mainSlider.value + " x " + mainSlider.value;
    }
    #endregion

    #region Disable
    /// <summary>
    /// Disable given game object
    /// </summary>
    public void Disable(GameObject go)
    {
        audioManager.PlayClick();
        go.SetActive(false);
    }
    #endregion

    #region Enable
    /// <summary>
    /// Set given object active
    /// </summary>
    /// <param name="go"></param>
    public void Enable(GameObject go)
    {
        audioManager.PlayClick();
        go.SetActive(true);
    }
    #endregion

    #region End Button
    /// <summary>
    /// Exit game
    /// </summary>
    public void QuitButton()
    {
        audioManager.PlayClick();
        Application.Quit();
    }
    #endregion

    #region Create Loading Text

    /// <summary>
    /// Deactivate 
    /// </summary>
    public void loadingText()
    {
        // Get all canvases in the scene
        Canvas[] canvases = new Canvas[FindObjectsOfType<Canvas>().Length];
        canvases = FindObjectsOfType<Canvas>();

        // turn off player childeren
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player &&
            player.name == "Player1")
        {
            player.GetComponent<OnePlayerScript>().TurnOffChildren();
        }

        // disable all convases
        foreach (Canvas c in canvases)
        {
            c.gameObject.SetActive(false);
        }

        loadingIcon.SetActive(true);

        // Pauses game in unity editor
        //UnityEditor.EditorApplication.isPaused = true;

    }

    #endregion

    #endregion
}
