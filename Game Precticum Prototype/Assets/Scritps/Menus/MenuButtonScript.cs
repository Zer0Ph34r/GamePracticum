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

    #endregion

    /// <summary>
    /// Change scene to Main Scene
    /// </summary>
    public void LoadSceneButton(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Open Tutoriel button
    /// </summary>
    public void TutorialButton()
    {
        gameObject.SetActive(false);
        InstructionsCanvas.transform.gameObject.SetActive(true);
    }

    /// <summary>
    /// Open credits canvas and close button canvas
    /// </summary>
    public void CreditsButton()
    {
        CreditsCanvas.transform.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Exit game
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}
