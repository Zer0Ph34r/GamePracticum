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

    #region Sound Effect Fields
    // Sound Stuff
    AudioSource audioSource;
    AudioClip break1;
    AudioClip break2;
    AudioClip break3;
    AudioClip break4;

    // BGM
    AudioClip BGMusic;

    #endregion

    #endregion

    #region Start Method

    // Use this for initialization
    void Start()
    {

        #region Get / Save Objects

        // save instance of player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<OnePlayerScript>();

        // Save reference to audio source
        gameObject.AddComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();

        // Save reference to canvas'
        UI = GameObject.FindGameObjectWithTag("UI");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        endScreen = GameObject.FindGameObjectWithTag("EndScreen");
        endScreen.SetActive(false);

        // Load in sound effects
        break1 = Resources.Load<AudioClip>("Sounds/Break1");
        break2 = Resources.Load<AudioClip>("Sounds/Break2");
        break3 = Resources.Load<AudioClip>("Sounds/Break3");
        break4 = Resources.Load<AudioClip>("Sounds/Break4");

        // Score Display
        scoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        turnText = GameObject.FindGameObjectWithTag("Text2").GetComponent<Text>();
        scoreText.text = "Score: " + score;
        turnText.text = "Turns Left: " + player.GetComponent<OnePlayerScript>().turns;

        #endregion

        #region Load BGM

        switch (Random.Range(0, 8))
        {
            case 0:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM0");
                break;
            case 1:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM1");
                break;
            case 2:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM2");
                break;
            case 3:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM3");
                break;
            case 4:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM4");
                break;
            case 5:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM5");
                break;
            case 6:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM6");
                break;
            case 7:
                BGMusic = Resources.Load<AudioClip>("Sounds/Music/BGM7");
                break;
        }


        #endregion

        // Play Loaded Music
        audioSource.PlayOneShot(BGMusic);

        #region Set Event Methods

        // add method to event call
        GemScript.fireSoundEvent += PlaySound;
        OnePlayerScript.fireScore += SetScore;
        OnePlayerScript.endGame += GameOver;

        #endregion
    }

    #endregion

    #region Methods

    #region Play Sound
    /// <summary>
    /// Plays random crash sound
    /// </summary>
    void PlaySound()
    {
        // Play random sound effect
        switch (Random.Range(0, 4))
        {
            case 0:
                audioSource.PlayOneShot(break1);
                break;
            case 1:
                audioSource.PlayOneShot(break2);
                break;
            case 2:
                audioSource.PlayOneShot(break3);
                break;
            case 3:
                audioSource.PlayOneShot(break4);
                break;
        }

    }

    #endregion

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
