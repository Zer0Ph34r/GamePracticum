using UnityEngine;
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

    // Get reference ot audio manager
    AudioManager audioManager;

    // Set all static events to null
    GemScript.callMethod Selected = null;
    GemScript.runNext fireSoundEvent = null;
    GemScript.runNext runNextMethod = null;
    GemScript.check checkGems = null;

    #endregion

    #region Start

    // Get Audio Manager
    private void Start()
    {
        audioManager = AudioManager.instance;
    }
    #endregion

    #region Button Methods

    #region Load Scene
    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void LoadSceneButton(string sceneName)
    {
        audioManager.PlayClick();
        audioManager.StopBGM();
        SceneManager.LoadScene(sceneName);
    }
    #endregion

    #region Mutliplayer Button

    public void LoadMutliplayer()
    {
        SaveLoadScript.Load();
        audioManager.PlayClick();
        audioManager.StopBGM();
        SceneManager.LoadScene("TwoPlayerSetUpScene");
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

    #endregion
}
