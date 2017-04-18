using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour
{

    #region Fields

    // Numbers for iterating through each colr/song
    int GUIColor = 0;
    int BGColor = 0;
    int SongSelect = 0;

    #region Serialze Fields

    #region Text
    // text fields
    [SerializeField]
    Text GUIColorText;
    [SerializeField]
    Text BGColorText;
    [SerializeField]
    Text SongSelectText;
    [SerializeField]
    Text MusicSliderText;
    [SerializeField]
    Text EffectSliderText;
    #endregion

    #region Sliders
    // slider values
    [SerializeField]
    Slider musicSlider;
    [SerializeField]
    Slider effectSlider;
    #endregion

    #endregion

    #endregion

    #region Start Method
    private void Start()
    {
        // check for any saved options and set options accordingly
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            MusicSliderText.text = "Music Volume: " + PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            SetBGVolume(PlayerPrefs.GetFloat("MusicVolume"));
        }

        if (PlayerPrefs.HasKey("EffectVolume"))
        {
            EffectSliderText.text = "Sound Effect Volume: " + PlayerPrefs.GetFloat("EffectVolume");
            effectSlider.value = PlayerPrefs.GetFloat("EffectVolume");
            SetEffectVolume(PlayerPrefs.GetFloat("EffectVolume"));
        }

        if (PlayerPrefs.HasKey("GUIColor"))
        {
            GUIColor = PlayerPrefs.GetInt("GUIColor");
            SetGUIColor(GUIColor);
        }

        if (PlayerPrefs.HasKey("BGColor"))
        {
            BGColor = PlayerPrefs.GetInt("BGColor");
            SetBGColor(BGColor);
        }

        if (PlayerPrefs.HasKey("SongSelect"))
        {
            SongSelect = PlayerPrefs.GetInt("SongSelect");
            SetSong(SongSelect);
        }

    }
    #endregion

    #region Methods

    #region GUI Color Setting Methods

    #region Right GUI Color
    public void RightGUIColor()
    {

        GUIColor++;
        PlayerPrefs.SetInt("GUIColor", GUIColor);
        switch (GUIColor)
        {
            case 0:
                ButtonColorLERP.setGUIColor = Color.black;
                GUIColorText.text = "Rotating";
                break;
            case 1:
                ButtonColorLERP.setGUIColor = Color.white;
                GUIColorText.text = "White";
                break;
            case 2:
                ButtonColorLERP.setGUIColor = Color.cyan;
                GUIColorText.text = "Blue";
                break;
            case 3:
                ButtonColorLERP.setGUIColor = Color.magenta;
                GUIColorText.text = "Purple";
                break;
            case 4:
                ButtonColorLERP.setGUIColor = Color.red;
                GUIColorText.text = "Red";
                break;
            case 5:
                ButtonColorLERP.setGUIColor = Color.yellow;
                GUIColorText.text = "Yellow";
                break;
            case 6:
                ButtonColorLERP.setGUIColor = Color.green;
                GUIColorText.text = "Green";
                break;
            case 7:
                GUIColor = 0;
                ButtonColorLERP.setGUIColor = Color.black;
                GUIColorText.text = "Rotating";
                break;
        }
    }
    #endregion

    #region Left GUI Color

    public void LeftGUIColor()
    {
        GUIColor--;
        PlayerPrefs.SetInt("GUIColor", GUIColor);
        switch (GUIColor)
        {
            case -1:
                GUIColor = 6;
                ButtonColorLERP.setGUIColor = Color.green;
                GUIColorText.text = "Green";
                break;
            case 0:
                ButtonColorLERP.setGUIColor = Color.black;
                GUIColorText.text = "Rotating";
                break;
            case 1:
                ButtonColorLERP.setGUIColor = Color.white;
                GUIColorText.text = "White";
                break;
            case 2:
                ButtonColorLERP.setGUIColor = Color.cyan;
                GUIColorText.text = "Blue";
                break;
            case 3:
                ButtonColorLERP.setGUIColor = Color.magenta;
                GUIColorText.text = "Purple";
                break;
            case 4:
                ButtonColorLERP.setGUIColor = Color.red;
                GUIColorText.text = "Red";
                break;
            case 5:
                ButtonColorLERP.setGUIColor = Color.yellow;
                GUIColorText.text = "Yellow";
                break;
            case 6:
                ButtonColorLERP.setGUIColor = Color.green;
                GUIColorText.text = "Green";
                break;
        }
    }
    #endregion

    #region Set GUI Color

    /// <summary>
    /// Set GUI Color based on given int
    /// </summary>
    /// <param name="value"></param>
    void SetGUIColor(int value)
    {
        switch (value)
        {
            case 0:
                ButtonColorLERP.setGUIColor = Color.black;
                GUIColorText.text = "Rotating";
                break;
            case 1:
                ButtonColorLERP.setGUIColor = Color.white;
                GUIColorText.text = "White";
                break;
            case 2:
                ButtonColorLERP.setGUIColor = Color.cyan;
                GUIColorText.text = "Blue";
                break;
            case 3:
                ButtonColorLERP.setGUIColor = Color.magenta;
                GUIColorText.text = "Purple";
                break;
            case 4:
                ButtonColorLERP.setGUIColor = Color.red;
                GUIColorText.text = "Red";
                break;
            case 5:
                ButtonColorLERP.setGUIColor = Color.yellow;
                GUIColorText.text = "Yellow";
                break;
            case 6:
                ButtonColorLERP.setGUIColor = Color.green;
                GUIColorText.text = "Green";
                break;
            case 7:
                GUIColor = 0;
                ButtonColorLERP.setGUIColor = Color.black;
                GUIColorText.text = "Rotating";
                break;
        }
    }

    #endregion

    #endregion

    #region BG Color Setting Methods

    #region Right BG Color
    public void RightBGColor()
    {
        BGColor++;
        PlayerPrefs.SetInt("BGColor", BGColor);
        switch (BGColor)
        {
            case 0:
                BackgroundColorLERP.setBGColor = Color.black;
                BGColorText.text = "Rotating";
                break;
            case 1:
                BackgroundColorLERP.setBGColor = Color.white;
                BGColorText.text = "White";
                break;
            case 2:
                BackgroundColorLERP.setBGColor = Color.cyan;
                BGColorText.text = "Blue";
                break;
            case 3:
                BackgroundColorLERP.setBGColor = Color.magenta;
                BGColorText.text = "Purple";
                break;
            case 4:
                BackgroundColorLERP.setBGColor = Color.red;
                BGColorText.text = "Red";
                break;
            case 5:
                BackgroundColorLERP.setBGColor = Color.yellow;
                BGColorText.text = "Yellow";
                break;
            case 6:
                BackgroundColorLERP.setBGColor = Color.green;
                BGColorText.text = "Green";
                break;
            case 7:
                BGColor = 0;
                BackgroundColorLERP.setBGColor = Color.black;
                BGColorText.text = "Rotating";
                break;
        }
    }
    #endregion

    #region Left BG Color

    public void LeftBGColor()
    {
        BGColor--;
        PlayerPrefs.SetInt("BGColor", BGColor);
        switch (BGColor)
        {
            case -1:
                BGColor = 6;
                BackgroundColorLERP.setBGColor = Color.green;
                BGColorText.text = "Green";
                break;
            case 0:
                BackgroundColorLERP.setBGColor = Color.black;
                BGColorText.text = "Rotating";
                break;
            case 1:
                BackgroundColorLERP.setBGColor = Color.white;
                BGColorText.text = "White";
                break;
            case 2:
                BackgroundColorLERP.setBGColor = Color.cyan;
                BGColorText.text = "Blue";
                break;
            case 3:
                BackgroundColorLERP.setBGColor = Color.magenta;
                BGColorText.text = "Purple";
                break;
            case 4:
                BackgroundColorLERP.setBGColor = Color.red;
                BGColorText.text = "Red";
                break;
            case 5:
                BackgroundColorLERP.setBGColor = Color.yellow;
                BGColorText.text = "Yellow";
                break;
            case 6:
                BackgroundColorLERP.setBGColor = Color.green;
                BGColorText.text = "Green";
                break;
        }
    }
    #endregion

    #region Set BG Color

    void SetBGColor(int value)
    {
        switch (value)
        {
            case 0:
                BackgroundColorLERP.setBGColor = Color.black;
                BGColorText.text = "Rotating";
                break;
            case 1:
                BackgroundColorLERP.setBGColor = Color.white;
                BGColorText.text = "White";
                break;
            case 2:
                BackgroundColorLERP.setBGColor = Color.cyan;
                BGColorText.text = "Blue";
                break;
            case 3:
                BackgroundColorLERP.setBGColor = Color.magenta;
                BGColorText.text = "Purple";
                break;
            case 4:
                BackgroundColorLERP.setBGColor = Color.red;
                BGColorText.text = "Red";
                break;
            case 5:
                BackgroundColorLERP.setBGColor = Color.yellow;
                BGColorText.text = "Yellow";
                break;
            case 6:
                BackgroundColorLERP.setBGColor = Color.green;
                BGColorText.text = "Green";
                break;
            case 7:
                BGColor = 0;
                BackgroundColorLERP.setBGColor = Color.black;
                BGColorText.text = "Rotating";
                break;
        }
    }

    #endregion

    #endregion

    #region Song Select

    #region Select Right
    public void SelectSongRight()
    {
        SongSelect++;
        PlayerPrefs.SetInt("SongSelect", SongSelect);
        AudioManager.instance.SetSong(SongSelect);
        switch (SongSelect)
        {
            case 0:
                AudioManager.instance.SetSong(0);
                SongSelectText.text = "Random";
                break;
            case 1:

                SongSelectText.text = "Depart";
                break;
            case 2:

                SongSelectText.text = "Fallwind";
                break;
            case 3:

                SongSelectText.text = "Homesick";
                break;
            case 4:

                SongSelectText.text = "I Dunno";
                break;
            case 5:

                SongSelectText.text = "Like Music";
                break;
            case 6:

                SongSelectText.text = "Paper Planes";
                break;
            case 7:

                SongSelectText.text = "Sunday";
                break;
            case 8:

                SongSelectText.text = "Who We Are";
                break;
            case 9:
                SongSelect = 0;
                AudioManager.instance.SetSong(0);
                SongSelectText.text = "Random";
                break;
        }

    }
    #endregion

    #region Select Left
    public void SelectSongLeft()
    {
        SongSelect--;
        PlayerPrefs.SetInt("SongSelect", SongSelect);
        AudioManager.instance.SetSong(SongSelect);
        switch (SongSelect)
        {
            case -1:
                SongSelect = 8;
                AudioManager.instance.SetSong(8);
                SongSelectText.text = "Who We Are";
                break;
            case 0:

                AudioManager.instance.SetSong(0);
                SongSelectText.text = "Random";
                break;
            case 1:

                SongSelectText.text = "Depart";
                break;
            case 2:

                SongSelectText.text = "Fallwind";
                break;
            case 3:

                SongSelectText.text = "Homesick";
                break;
            case 4:

                SongSelectText.text = "I Dunno";
                break;
            case 5:

                SongSelectText.text = "Like Music";
                break;
            case 6:

                SongSelectText.text = "Paper Planes";
                break;
            case 7:

                SongSelectText.text = "Sunday";
                break;
            case 8:

                SongSelectText.text = "Who We Are";
                break;
        }

    }
    #endregion

    #region Set Song

    void SetSong(int value)
    {
        switch (value)
        {
            case 0:
                AudioManager.instance.SetSong(0);
                SongSelectText.text = "Random";
                break;
            case 1:

                SongSelectText.text = "Depart";
                break;
            case 2:

                SongSelectText.text = "Fallwind";
                break;
            case 3:

                SongSelectText.text = "Homesick";
                break;
            case 4:

                SongSelectText.text = "I Dunno";
                break;
            case 5:

                SongSelectText.text = "Like Music";
                break;
            case 6:

                SongSelectText.text = "Paper Planes";
                break;
            case 7:

                SongSelectText.text = "Sunday";
                break;
            case 8:

                SongSelectText.text = "Who We Are";
                break;
            case 9:
                SongSelect = 0;
                AudioManager.instance.SetSong(0);
                SongSelectText.text = "Random";
                break;
        }
    }

    #endregion

    #endregion

    #region Volume Scale

    #region Music Volume
    /// <summary>
    /// Set volume of BGM
    /// </summary>
    /// <param name="slider"></param>
    public void VolumeChange(Slider slider)
    {
        PlayerPrefs.SetFloat("MusicVolume", slider.value);
        AudioManager.instance.SetMusicVolume(slider.value);
        MusicSliderText.text = "Music Volume: " + slider.value;
    }

    /// <summary>
    /// Set music volume based on float
    /// </summary>
    /// <param name="volume"></param>
    void SetBGVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }
    #endregion

    #region Sound Effects
    /// <summary>
    /// Set volume of Sound Effects
    /// </summary>
    /// <param name="slider"></param>
    public void EffectVolumeChange(Slider slider)
    {
        PlayerPrefs.SetFloat("EffectVolume", slider.value);
        AudioManager.instance.SetEffectVolume(slider.value);
        EffectSliderText.text = "Sound Effect Volume: " + slider.value;
    }

    /// <summary>
    /// Set sound effect volume based on given float
    /// </summary>
    /// <param name="volume"></param>
    void SetEffectVolume(float volume)
    {
        AudioManager.instance.SetEffectVolume(volume);
    }
    #endregion

    #endregion

    #endregion
}
