using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenScript : MonoBehaviour {

    #region Fields

    [SerializeField]
    GameObject MainMenu;
    [SerializeField]
    GameObject stuff;

    TimerScript timer;

    bool move = false;

    #endregion

    // Use this for initialization
    void Start () {
        if (GlobalVariables.SPLASH)
        {
            GlobalVariables.SPLASH = false;
            timer = new TimerScript(2);
            timer.StartTimer();
            TimerScript.timerEnded += ToggleSplashScreen;
        }
        else
        {
            MainMenu.SetActive(true);
            Destroy(gameObject);
        }
        
	}
	
	void ToggleSplashScreen()
    {
        TimerScript.timerEnded -= ToggleSplashScreen;
        MainMenu.SetActive(true);
        move = true;
        
    }

    private void Update()
    {
        timer.Update(Time.deltaTime);
        if (move)
        {
            stuff.transform.position -= new Vector3(0, 10 * Time.deltaTime, 0);
        }
        if (stuff.transform.position.y < -15)
        {
            Destroy(stuff);
            Destroy(gameObject);
        }
    }
}
