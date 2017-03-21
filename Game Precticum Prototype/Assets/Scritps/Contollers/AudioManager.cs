using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    #region Fields

    public static AudioManager instance;

    #region Music and Sound Effects

    // Sound Stuff
    AudioSource BGMSource;
    AudioSource soundEffectSource;

    // List of all sound effects and BGM
    List<AudioClip> music;

    #endregion

    #endregion

    #region Singleton

    /// <summary>
    /// Create a instance of this if none exist
    /// </summary>
    private void Awake()
    {

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        #region Create and Save Audio Source

        // Save reference to audio source
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSource>();
        // save both audio sources
        BGMSource = GetComponents<AudioSource>()[0];
        soundEffectSource = GetComponents<AudioSource>()[1];

        #endregion

        #region Load Sound Effects

        //Initialize music list
        music = new List<AudioClip>();

        // Load in sound effects (music 0 - 3 = crash, 4 = click)
        music.Add(Resources.Load<AudioClip>("Sounds/Break1"));
        music.Add(Resources.Load<AudioClip>("Sounds/Break2"));
        music.Add(Resources.Load<AudioClip>("Sounds/Break3"));
        music.Add(Resources.Load<AudioClip>("Sounds/Break4"));
        music.Add(Resources.Load<AudioClip>("Sounds/Click"));

        #endregion

        #region Load BGM

        // Load all the BGM (music 5 - 12 = BGM)
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM0"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM1"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM2"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM3"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM4"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM5"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM6"));
        music.Add(Resources.Load<AudioClip>("Sounds/Music/BGM7"));

        #endregion

    }

    #endregion

    #region Methods

    #region Play Crash
    /// <summary>
    /// Play a randomly pitched crash sound effect
    /// </summary>
    public void PlayCrash()
    {
        soundEffectSource.pitch = Random.Range(0, 1);
        switch (Random.Range(0,4))
        {
            case 0:
                soundEffectSource.PlayOneShot(music[0]);
                break;
            case 1:
                soundEffectSource.PlayOneShot(music[1]);
                break;
            case 2:
                soundEffectSource.PlayOneShot(music[2]);
                break;
            case 3:
                soundEffectSource.PlayOneShot(music[3]);
                break;
        }

    }

    #endregion

    #region Play CLick Sound
    /// <summary>
    /// Play clicking sound
    /// </summary>
    public void PlayClick()
    {
        soundEffectSource.PlayOneShot(music[4]);
    }

    #endregion

    #region Play Music
    /// <summary>
    /// Play a random BGM
    /// </summary>
    public void PlayBGM()
    {
        // Play a random BGM
        switch (Random.Range(0, 8))
        {
            case 0:
                BGMSource.PlayOneShot(music[5]);
                break;
            case 1:
                BGMSource.PlayOneShot(music[6]);
                break;
            case 2:
                BGMSource.PlayOneShot(music[7]);
                break;
            case 3:
                BGMSource.PlayOneShot(music[8]);
                break;
            case 4:
                BGMSource.PlayOneShot(music[9]);
                break;
            case 5:
                BGMSource.PlayOneShot(music[10]);
                break;
            case 6:
                BGMSource.PlayOneShot(music[11]);
                break;
            case 7:
                BGMSource.PlayOneShot(music[12]);
                break;
        }
    }
    #endregion

    #region Stop Music
    /// <summary>
    /// Stop the music currently playing
    /// </summary>
    public void StopBGM()
    {
        BGMSource.Stop();
    }

    #endregion

    #endregion
}
