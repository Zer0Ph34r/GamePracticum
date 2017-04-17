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
        // if player prefs have been added
        if (!PlayerPrefs.HasKey("Highscore610"))
        {
            // iterate through all parts of the table
            for (int i = 0; i < 6; i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    // reset the highscore to 0
                    PlayerPrefs.SetInt(("Highscore" + i + k), GlobalVariables.HIGHSCORE_TABLE[i, k]);
                }
            }
        }
    }

}
