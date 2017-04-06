using UnityEngine;
using UnityEngine.UI;

public class ButtonColorLERP : MonoBehaviour {

    #region Fields

    // setColor variable for options
    public static Color setGUIColor = Color.black;

    // how long it takes to move from one color to the next
    float duration = 7;

    // button image (apparently global)
    Image buttonImage;
    // timer
    float t;
    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        // get reference to global GUI image
        buttonImage = GetComponent<Image>();

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {

        if (setGUIColor == Color.black)
        {
            // get current color time
            t = GlobalVariables.LERP_TIME;

            // check the different t times and lerp accordingly
            if (t < 1)
            {
                // lerp from blue to red
                buttonImage.material.color = Color.Lerp(Color.white, Color.cyan, t);
            }
            else if (t > 1 &&
                t < 2)
            {
                // lerp from red to blue
                buttonImage.material.color = Color.Lerp(Color.cyan, Color.magenta, t - 1);
            }
            else if (t > 2 &&
                t < 3)
            {
                // lerp from red to blue
                buttonImage.material.color = Color.Lerp(Color.magenta, Color.red, t - 2);
            }
            else if (t > 3 &&
                t < 4)
            {
                // lerp from red to blue
                buttonImage.material.color = Color.Lerp(Color.red, Color.yellow, t - 3);
            }
            else if (t > 4 &&
                t < 5)
            {
                // lerp from red to blue
                buttonImage.material.color = Color.Lerp(Color.yellow, Color.green, t - 4);
            }
            else if (t > 5 &&
                t < 6)
            {
                // lerp from red to blue
                buttonImage.material.color = Color.Lerp(Color.green, Color.white, t - 5);
            }
        }
        else
        {
            buttonImage.material.color = setGUIColor;
        }

    }
    #endregion

    #region Methods

    /// <summary>
    /// Sets the setColor variable
    /// </summary>
    /// <param name="color"></param>
    public void SetColor(Color color)
    {
        setGUIColor = color;
    }

    #endregion
}
