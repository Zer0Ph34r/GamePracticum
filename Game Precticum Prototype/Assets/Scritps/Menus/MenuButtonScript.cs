using System.Collections;
using System.Collections.Generic;
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

    #endregion

    #region Button Methods

    private void Start()
    {
        click = Resources.Load<AudioClip>("Sounds/Click");
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void LoadSceneButton(string sceneName)
    {
        audioSource.PlayOneShot(click);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {
        audioSource.PlayOneShot(click);
        gameObject.SetActive(false);
        InstructionsCanvas.transform.gameObject.SetActive(true);
    }

    /// <summary>
    /// Open credits canvas and close button canvas
    /// </summary>
    public void CreditsButton()
    {
        audioSource.PlayOneShot(click);
        CreditsCanvas.transform.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Disable given game object
    /// </summary>
    public void Disable()
    {
        audioSource.PlayOneShot(click);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Set given object active
    /// </summary>
    /// <param name="go"></param>
    public void Enable(GameObject go)
    {
        audioSource.PlayOneShot(click);
        go.SetActive(true);
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void QuitButton()
    {
        audioSource.PlayOneShot(click);
        Application.Quit();
    }

    #endregion
}
