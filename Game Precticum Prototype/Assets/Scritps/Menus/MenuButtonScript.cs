﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    #region Fields

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

    // Sound Effect for clicking on buttons
    AudioClip click;
    AudioSource audioSource;

    // Set all static events to null
    GemScript.callMethod Selected = null;
    GemScript.runNext fireSoundEvent = null;
    GemScript.runNext runNextMethod = null;
    GemScript.check checkGems = null;

    #endregion

    #region Start

    private void Start()
    {
        click = Resources.Load<AudioClip>("Sounds/Click");

        // Save reference to audio source
        if (audioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }
    }
    #endregion

    #region Button Methods

    #region Load Scene
    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void LoadSceneButton(string sceneName)
    {
        audioSource.PlayOneShot(click);
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region Mutliplayer Button

    public void LoadMutliplayer()
    {
        SaveLoadScript.Load();
        audioSource.PlayOneShot(click);
        SceneManager.LoadScene("TwoPlayerSetUpScene");
    }

    #endregion

    #region Tutorial Button
    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {
        audioSource.PlayOneShot(click);
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
        audioSource.PlayOneShot(click);
        CreditsStuff.transform.gameObject.SetActive(true);
        MainMenuStuff.transform.gameObject.SetActive(false);
    }
    #endregion

    #region Slider Button
    // Gets current slider setting
    public void SubmitSliderSetting(Slider mainSlider)
    {
        //Displays the value of the slider in the console.
        GlobalVariables.TURNS = (int)mainSlider.value;
        TurnSliderText.text = "# of Turns: " + mainSlider.value;
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
        audioSource.PlayOneShot(click);
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
        audioSource.PlayOneShot(click);
        go.SetActive(true);
    }
    #endregion

    #region End Button
    /// <summary>
    /// Exit game
    /// </summary>
    public void QuitButton()
    {
        audioSource.PlayOneShot(click);
        Application.Quit();
    }
    #endregion

    #endregion
}
