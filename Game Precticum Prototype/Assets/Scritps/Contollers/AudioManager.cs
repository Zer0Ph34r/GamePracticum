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
    List<AudioClip> soundEffects;

    AudioClip currBGM;

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
        soundEffects = new List<AudioClip>();

        // Load in sound effects (music 0 - 3 = crash, 4 = click)
        soundEffects.Add(Resources.Load<AudioClip>("Sounds/Break1"));
        soundEffects.Add(Resources.Load<AudioClip>("Sounds/Break2"));
        soundEffects.Add(Resources.Load<AudioClip>("Sounds/Break3"));
        soundEffects.Add(Resources.Load<AudioClip>("Sounds/Break4"));
        soundEffects.Add(Resources.Load<AudioClip>("Sounds/Click"));

        #endregion

        #region Load BGM

        

        // Load all the BGM (music 5 - 12 = BGM)
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM0"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM1"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM2"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM3"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM4"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM5"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM6"));
        //soundEffects.Add(Resources.Load<AudioClip>("Sounds/Music/BGM7"));

        #endregion

        PlayBGM();
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
                soundEffectSource.PlayOneShot(soundEffects[0]);
                break;
            case 1:
                soundEffectSource.PlayOneShot(soundEffects[1]);
                break;
            case 2:
                soundEffectSource.PlayOneShot(soundEffects[2]);
                break;
            case 3:
                soundEffectSource.PlayOneShot(soundEffects[3]);
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
        soundEffectSource.PlayOneShot(soundEffects[4]);
    }

    #endregion

    #region Play Music
    /// <summary>
    /// Play a random BGM
    /// </summary>
    public void PlayBGM()
    {
        // Load and Play a random BGM (Saves on load time when starting game)
        switch (Random.Range(0, 8))
        {
            case 0:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM0");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 1:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM1");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 2:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM2");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 3:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM3");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 4:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM4");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 5:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM5");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 6:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM6");
                BGMSource.PlayOneShot(currBGM);
                break;
            case 7:
                currBGM = Resources.Load<AudioClip>("Sounds/Music/BGM7");
                BGMSource.PlayOneShot(currBGM);
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
