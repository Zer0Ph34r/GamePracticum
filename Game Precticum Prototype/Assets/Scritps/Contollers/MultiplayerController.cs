using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MultiplayerController : MonoBehaviour
{

    #region Fields

    // table size int X int
    int tableSize = GlobalVariables.SCREEN_POSITION;

    #region UI Display Info
    // Score Tracker
    public int player1Score { get; set; }
    Text player1ScoreText;
    public int player2Score { get; set; }
    Text player2ScoreText;
    public int turns { get; set; }
    Text turnText;
    #endregion

    #region UI Objects
    // Reference to UI
    [SerializeField]
    Canvas UI;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject endScreen;
    #endregion

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

    #region Server Fields
    // variables for getting server client connections
    NetworkClient myClient;
    //NetworkMessageDelegate OnConnected;
    NetworkMessageDelegate OnMessageReceive;
    #endregion

    // Get reference to player 
    NetworkPlayerScript player;

    // bool to make sure info is sent only once
    bool infoSent = false;

    #endregion

    #region Start Method

    // Use this for initialization
    void Start()
    {

        #region Get / Save Objects

        // Save reference to audio source
        if (audioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }

        // save instance of player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerScript>();

        // Save reference ot pause canvas
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

        #region Sounds

        // Load in sound effects
        break1 = Resources.Load<AudioClip>("Sounds/Break1");
        break2 = Resources.Load<AudioClip>("Sounds/Break2");
        break3 = Resources.Load<AudioClip>("Sounds/Break3");
        break4 = Resources.Load<AudioClip>("Sounds/Break4");

        #endregion

        #region Score Display

        // Score Display
        player1ScoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        player1ScoreText.text = "Player 1: " + player1Score;
        player2ScoreText = GameObject.FindGameObjectWithTag("Text2").GetComponent<Text>();
        player2ScoreText.text = "Player 2: " + player2Score;
        turnText = GameObject.FindGameObjectWithTag("Turns").GetComponent<Text>();
        turnText.text = "Turns: " + turns;
        #endregion

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
        NetworkPlayerScript.fireScore += SetScore;

        #endregion

        // set starting count
        player1Score = 0;
        player2Score = 0;
        turns = GlobalVariables.MULTIPLAYER_TURNS;

        // set text
        turnText.text = "Turns: " + turns;

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
    /// <summary>
    /// Updates current score values
    /// </summary>
    public void SetScore()
    {
        // set turn count
        turnText.text = "Turns: " + turns;

        // set score fields
        player1Score = player.score;
        player1ScoreText.text = "Player 1: " + player1Score * 10;
        player2ScoreText.text = "Player 2: " + player2Score * 10;
    }

    #endregion

    #region Set Turn

    public void SetTurn()
    {
        // check for game over
        if (turns == 0)
        {
            GameOver();
        }
    }

    #endregion

    #region GameOver

    /// <summary>
    /// Sets game to game over state showing final score and end game options
    /// </summary>
    public void GameOver()
    {
        // turn off all objects other than the ending canvas
        UI.gameObject.SetActive(false);
        endScreen.SetActive(true);
        // Check who is the winner
        if (player1Score > player2Score)
        {
            endScreen.GetComponent<EndingScript>().SetMultiplayerEnd(player1Score, player2Score, true);
        }
        else if (player2Score > player1Score)
        {
            endScreen.GetComponent<EndingScript>().SetMultiplayerEnd(player2Score, player1Score, false);
        }

        if (!infoSent)
        {
            // Send results to other player
            player.SendInfo();
            // Set infoSent to true
            infoSent = true;
            Debug.Log("sent");
        }
        
    }

    #endregion

    #region On Destroy
    /// <summary>
    /// Called when object is destroyd
    /// </summary>
    private void OnDestroy()
    {
        GemScript.fireSoundEvent -= PlaySound;
        NetworkPlayerScript.fireScore -= SetScore;
    }

    #endregion

    #endregion

}
