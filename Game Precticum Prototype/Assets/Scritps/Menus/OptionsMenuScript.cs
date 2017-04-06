using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuScript : MonoBehaviour {

    #region Fields

    int GUIColor = 0;
    int BGColor = 0;

    


    #endregion

    #region Methods

    public void RightGUIColor()
    {
        GUIColor++;
    }

    public void LeftGUIColor()
    {
        GUIColor--;
    }

    #endregion
}
