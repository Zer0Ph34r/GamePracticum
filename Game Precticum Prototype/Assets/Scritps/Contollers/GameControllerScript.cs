using UnityEngine;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour {

    #region Fields

    // table size int X int
    int tableSize = GlobalVariables.SCREEN_POSITION;

    // Score Tracker
    int score = 0;
    Text scoreText;
    Text turnText;

    // Get reference to player 
    OnePlayerScript player;

    // objects to start as inactive
    GameObject UI;
    GameObject pauseMenu;
    GameObject endScreen;

    AudioManager audioManager;

    #endregion

    #region Start Method

    // Use this for initialization
    void Start()
    {

        #region Get / Save Objects

        // save instance of player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerScript>();

        // Save reference to audio source
        audioManager = AudioManager.instance;

        // Save reference to canvas'
        UI = GameObject.FindGameObjectWithTag("UI");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        endScreen = GameObject.FindGameObjectWithTag("EndScreen");
        endScreen.SetActive(false);

        // Score Display
        scoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        turnText = GameObject.FindGameObjectWithTag("Text2").GetComponent<Text>();
        scoreText.text = "Score: " + score;
        turnText.text = "Turns Left: " + player.GetComponent<OnePlayerScript>().turns;

        #endregion

        // Star new song
        audioManager.PlayBGM();

        #region Set Event Methods

        // add method to event call
        GemScript.fireSoundEvent += PlaySound;
        OnePlayerScript.fireScore += SetScore;
        OnePlayerScript.endGame += GameOver;

        #endregion
    }

    #endregion

    #region Methods

    public void PlaySound()
    {
        audioManager.PlayCrash();
    }

    #region Set Score

    void SetScore()
    {
        // Update Players Score and turn count
        score = player.score * 10;
        scoreText.text = "Score: " + score;
        turnText.text = "Turns Left: " + player.GetComponent<OnePlayerScript>().turns;
    }

    #endregion

    #region GameOver
    /// <summary>
    /// Sets game to game over state showing final score and end game options
    /// </summary>
    public void GameOver()
    {
        // turn off all objects other than the ending canvas
        UI.SetActive(false);
        endScreen.SetActive(true);
        // Set endign score values
        endScreen.GetComponent<EndingScript>().setEnd(score);
    }

    #endregion

    #region On Destroy
    /// <summary>
    /// Called when object is destroyd
    /// </summary>
    private void OnDestroy()
    {
        // remove all events from delegates
        GemScript.fireSoundEvent -= PlaySound;
        OnePlayerScript.fireScore -= SetScore;
        OnePlayerScript.endGame -= GameOver;

    }

    #endregion

    #endregion

}
