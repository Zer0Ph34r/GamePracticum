using UnityEngine;
using System.Collections.Generic;
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
    #endregion

    // Get reference ot audio manager
    AudioManager audioManager;

    // Get laoding Icon
    Sprite loadingIcon;

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

        // load in loading sprite
        loadingIcon = Resources.Load<Sprite>("Sprites/LoadingIcon");
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
        // disable all convases
        foreach (Canvas c in canvases)
        {
            c.gameObject.SetActive(false);
        }
        // create loading object and set its position
        GameObject loading = new GameObject();
        loading.AddComponent<SpriteRenderer>().sprite = loadingIcon;
        loading.transform.position = new Vector3(GlobalVariables.TABLE_SIZE * 0.1f, (GlobalVariables.TABLE_SIZE * -0.8f), 10);
        loading.GetComponent<SpriteRenderer>().sortingOrder = 20;

        // Pauses game in unity editor
        //UnityEditor.EditorApplication.isPaused = true;

    }

    #endregion

    #endregion
}
