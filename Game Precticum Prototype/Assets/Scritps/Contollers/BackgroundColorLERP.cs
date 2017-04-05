using UnityEngine;

public class BackgroundColorLERP : MonoBehaviour {

    #region Fields

    float duration = 7;
    float t = GlobalVariables.LERP_TIME;

    SpriteRenderer spriteRenderer;

    #endregion

    #region Start
    // Use this for initialization
    void Start () {

        spriteRenderer = GetComponent<SpriteRenderer>();

	}
    #endregion

    #region Update
    // Update is called once per frame
    void Update () {

        //spriteRenderer.material.color = Color.Lerp(Color.blue, Color.red, t);
        // check the different t times and lerp accordingly
        if (t < 1)
        { 
            // lerp from blue to red
            spriteRenderer.material.color = Color.Lerp(Color.white, Color.cyan, t);
            t += Time.deltaTime / duration;
        }
        else if (t > 1 &&
            t < 2)
        {
            // lerp from red to blue
            spriteRenderer.material.color = Color.Lerp(Color.cyan, Color.magenta, t - 1);
            t += Time.deltaTime / duration;
        }
        else if (t > 2 &&
            t < 3)
        {
            // lerp from red to blue
            spriteRenderer.material.color = Color.Lerp(Color.magenta, Color.red, t - 2);
            t += Time.deltaTime / duration;
        }
        else if (t > 3 &&
            t < 4)
        {
            // lerp from red to blue
            spriteRenderer.material.color = Color.Lerp(Color.red, Color.yellow, t - 3);
            t += Time.deltaTime / duration;
        }
        else if (t > 4 &&
            t < 5)
        {
            // lerp from red to blue
            spriteRenderer.material.color = Color.Lerp(Color.yellow, Color.green, t - 4);
            t += Time.deltaTime / duration;
        }
        else if (t > 5 &&
            t < 6)
        {
            // lerp from red to blue
            spriteRenderer.material.color = Color.Lerp(Color.green, Color.white, t - 5);
            t += Time.deltaTime / duration;
        }
        else
        {
            t = 0;
        }

        GlobalVariables.LERP_TIME = t;

    }
    #endregion
}
