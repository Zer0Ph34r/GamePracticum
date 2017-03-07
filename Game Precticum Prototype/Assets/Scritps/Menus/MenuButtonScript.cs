using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour {

    #region Fields

    // other canvases
    [SerializeField]
    Canvas CreditsCanvas;
    [SerializeField]
    Canvas InstructionsCanvas;

    // Sound Effect for clicking on buttons
    AudioClip click;
    AudioSource audioSource;

    // Set all static events to null
    GemScript.callMethod Selected = null;
    GemScript.runNext fireSoundEvent = null;
    GemScript.runNext runNextMethod = null;
    GemScript.check checkGems = null;

    #endregion

    #region Start Method

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

    #region Tutorial Button
    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {
        audioSource.PlayOneShot(click);
        gameObject.SetActive(false);
        InstructionsCanvas.transform.gameObject.SetActive(true);
    }
    #endregion

    #region Credits Button
    /// <summary>
    /// Open credits canvas and close button canvas
    /// </summary>
    public void CreditsButton()
    {
        audioSource.PlayOneShot(click);
        CreditsCanvas.transform.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    #endregion

    #region Slider Button
    // Gets current slider setting
    public void SubmitSliderSetting(Slider mainSlider)
    {
        //Displays the value of the slider in the console.
        GlobalVariables.TURNS = (int)mainSlider.value;

        Debug.Log(mainSlider.value);
    }
    #endregion

    #region Disable
    /// <summary>
    /// Disable given game object
    /// </summary>
    public void Disable()
    {
        audioSource.PlayOneShot(click);
        gameObject.SetActive(false);
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
