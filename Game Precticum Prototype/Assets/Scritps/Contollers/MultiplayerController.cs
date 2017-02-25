using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MultiplayerController : MonoBehaviour {

    #region Fields

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    // Score Tracker
    int player1Score = 0;
    Text player1ScoreText;
    int player2Score = 0;
    Text player2ScoreText;

    // Reference to UI
    [SerializeField]
    Canvas UI;
    [SerializeField]
    GameObject networkManager;
    NetworkManagerHUD networkHUD;

    // Get reference to player 
    NetworkPlayerScript player1;
    NetworkPlayerScript player2;

    // reference to pause menu
    GameObject pauseMenu;

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

        // Save reference to audio source
        if (audioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }

        // Save reference ot pause canvas
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.gameObject.SetActive(false);

        // Load in sound effects
        break1 = Resources.Load<AudioClip>("Sounds/Break1");
        break2 = Resources.Load<AudioClip>("Sounds/Break2");
        break3 = Resources.Load<AudioClip>("Sounds/Break3");
        break4 = Resources.Load<AudioClip>("Sounds/Break4");

        // Score Display
        player1ScoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        player1ScoreText.text = "Player 1: " + player1Score;
        player2ScoreText = GameObject.FindGameObjectWithTag("Text").GetComponent<Text>();
        player2ScoreText.text = "Player 2: " + player2Score;

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

    #region Set Score
    /// <summary>
    /// Updates current score values
    /// </summary>
    void SetScore()
    {
        // Update Players Score
        player1Score = player1.score * 100;
        player1ScoreText.text = "Player 1: " + player1Score;
        player2Score = player2.score * 100;
        player2ScoreText.text = "Player 2: " + player2Score;
    }

    #endregion

    #region Set Turn

    public void SetTurn()
    {
        if (!player1.currTurn)
        {
            player1.currTurn = true;
        }
        if (!player2.currTurn)
        {
            player2.currTurn = true;
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
            player1 = player.GetComponent<NetworkPlayerScript>();
        }
        else
        {
            player2 = player.GetComponent<NetworkPlayerScript>();
            player.GetComponent<NetworkPlayerScript>().currTurn = false;
        }
        if (!UI.isActiveAndEnabled)
        {
            UI.gameObject.SetActive(true);
            networkHUD.showGUI = false;
        }
    }

    #endregion

    #endregion

}
