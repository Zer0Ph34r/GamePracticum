using UnityEngine;

public class BackButtonScript : MonoBehaviour {

    [SerializeField]
    Canvas Main;

    // Reference to audio manager
    AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
    }

    /// <summary>
    ///  Turn on main canvas and turn this one off
    /// </summary>
    public void BackButton()
    {
        audioManager.PlayClick();
        Main.transform.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResetScore()
    {
        if (PlayerPrefs.HasKey("Highscore"))
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }
    }

}
