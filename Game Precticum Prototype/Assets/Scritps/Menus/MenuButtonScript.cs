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

    // buttons on this canvas
    [SerializeField]
    Button startButton;
    [SerializeField]
    Button instructionsButton;
    [SerializeField]
    Button creditsButton;
    [SerializeField]
    Button quitButton;

    #endregion

    //private void Start()
    //{
    //    startButton = GetComponent<Button>();
    //}

    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void StartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {

    }

    /// <summary>
    /// Open credits canvas and close button canvas
    /// </summary>
    public void CreditsButton()
    {

    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}
