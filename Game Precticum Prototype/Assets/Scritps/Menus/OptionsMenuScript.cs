using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour {

    #region Fields

    int GUIColor = 0;
    int BGColor = 0;
    int SongSelect = 0;

    [SerializeField]
    Text GUIColorText;
    [SerializeField]
    Text BGColorText;
    [SerializeField]
    Text SongSelectText;

    #endregion

    #region Methods

    #region GUI Color Setting Methods

    #region Right GUI Color
    public void RightGUIColor()
    {
        GUIColor++;
        switch(GUIColor)
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

    #endregion

    #region BG Color Setting Methods

    #region Right BG Color
    public void RightBGColor()
    {
        BGColor++;
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

    #endregion

    #region Song Select

    #region Select Right
    public void SelectSongRight()
    {
        SongSelect++;
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

    #endregion

    #region Volume Scale

    /// <summary>
    /// Set volume of BGM
    /// </summary>
    /// <param name="slider"></param>
    public void VolumeChange(Slider slider)
    {
        AudioManager.instance.SetMusicVolume(slider.value);
    }

    /// <summary>
    /// Set volume of Sound Effects
    /// </summary>
    /// <param name="slider"></param>
    public void EffectVolumeChange(Slider slider)
    {
        AudioManager.instance.SetEffectVolume(slider.value);
    }

    #endregion

    #endregion
}
