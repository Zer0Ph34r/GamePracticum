using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MultiplayerController : NetworkBehaviour
{

    #region Fields

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    #region UI Display Info
    // Score Tracker
    int player1Score = 0;
    Text player1ScoreText;
    int player2Score = 0;
    Text player2ScoreText;
    int turns = GlobalVariables.MULTIPLAYER_TURNS;
    Text turnText;
    #endregion

    #region UI Objects
    // Reference to UI
    [SerializeField]
    Canvas UI;
    [SerializeField]
    GameObject networkManager;
    NetworkManagerHUD networkHUD;
    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject endScreen;
    #endregion

    #region Player Refereces
    // Get reference to player 
    [SyncVar]
    GameObject player1Obj;
    [SyncVar]
    GameObject player2Obj;
    NetworkPlayerScript player1;
    NetworkPlayerScript player2;
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
    NetworkMessageDelegate OnConnected;
    #endregion

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

        // Save reference ot pause canvas
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(false);

        // Load in sound effects
        break1 = Resources.Load<AudioClip>("Sounds/Break1");
        break2 = Resources.Load<AudioClip>("Sounds/Break2");
        break3 = Resources.Load<AudioClip>("Sounds/Break3");
        break4 = Resources.Load<AudioClip>("Sounds/Break4");

        // Score Display
        player1ScoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        player1ScoreText.text = "Player 1: " + player1Score;
        player2ScoreText = GameObject.FindGameObjectWithTag("Text2").GetComponent<Text>();
        player2ScoreText.text = "Player 2: " + player2Score;
        turnText = GameObject.FindGameObjectWithTag("Turns").GetComponent<Text>();
        turnText.text = "Turns: " + turns;

        UI.gameObject.SetActive(false);
        networkHUD = networkManager.GetComponent<NetworkManagerHUD>();

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
    void SetScore()
    {
        // Update Players Score
        player1Score = player1.score * 10;
        player1ScoreText.text = "Player 1: " + player1Score;
        player2Score = player2.score * 10;
        player2ScoreText.text = "Player 2: " + player2Score;
        turnText.text = "Turns: " + turns;
    }

    #endregion

    #region Set Turn

    public void SetTurn()
    {
        if (!player1.currTurn)
        {
            player1.currTurn = true;
            turns--;
        }
        if (!player2.currTurn)
        {
            player2.currTurn = true;
        }
        if (turns == 0)
        {
            GameOver();
        }
    }

    #endregion

    #region Set Players
    /// <summary>
    /// Set player script references
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayers(GameObject player)
    {
        // check if player 1 is null
        if (player1 == null)
        {
            SetupServer();
            player1 = player.GetComponent<NetworkPlayerScript>();
            player1Obj = player.gameObject;
            
        }
        else
        {
            SetupClient();
            player2 = player.GetComponent<NetworkPlayerScript>();
            player2Obj = player.gameObject;
            player.GetComponent<NetworkPlayerScript>().currTurn = false;
            
        }
        if (!UI.isActiveAndEnabled)
        {
            UI.gameObject.SetActive(true);
            networkHUD.showGUI = false;
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
            endScreen.GetComponent<EndingScript>().SetMultiplayerEnd(player2Score, true);
        }
        else if (player2Score > player1Score)
        {
            endScreen.GetComponent<EndingScript>().SetMultiplayerEnd(player2Score, false);
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

    #region Server Methods

    #region Set UpServer 
    /// <summary>
    /// Creates an instance of the server on this machine
    /// </summary>
    public void SetupServer()
    {
        NetworkServer.Listen(443);
    }
    #endregion

    #region Set Up Client
    /// <summary>
    /// Create a client and connect to the server port
    /// </summary>
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("192.168.0.1", 443);
    }
    #endregion

    #region Set Up Local Client
    /// <summary>
    /// Create a local client and connect to the local server
    /// </summary>
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
    }
    #endregion

    #endregion

    #endregion

}
