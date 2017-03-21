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

    #region Server Fields
    // variables for getting server client connections
    NetworkClient myClient;
    //NetworkMessageDelegate OnConnected;
    NetworkMessageDelegate OnMessageReceive;
    #endregion

    // Get reference to player 
    NetworkPlayerScript player;

    AudioManager audioManager;

    // bool to make sure info is sent only once
    bool infoSent = false;

    #endregion

    #region Start Method

    // Use this for initialization
    void Start()
    {

        #region Get / Save Objects

        // Save reference to audio source
        audioManager = AudioManager.instance;

        // save instance of player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<NetworkPlayerScript>();

        // Save reference ot pause canvas
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

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
        audioManager.PlayCrash();
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

    #region Save Scene

    /// <summary>
    /// Saves game info to continue playing same game later later
    /// </summary>
    public void SaveScene()
    {
        //SaveData.current = new SaveData("TestGameSave", player.boardSync, );
    }

    #endregion

    #region Load Scene

    /// <summary>
    /// Loads game info if there is a saved state
    /// </summary>
    public void LoadScene()
    {

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

    #region On Application Exit

    /// <summary>
    /// What does the mutiplayer manager do when the application exits
    /// </summary>
    private void OnApplicationQuit()
    {
        if (turns > 0)
        {
            SaveScene();
        }

    }

    #endregion

    #endregion

}
