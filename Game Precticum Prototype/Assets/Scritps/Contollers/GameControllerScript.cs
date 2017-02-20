using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {

    #region Fields

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    GameObject player1;

    bool twoPlayer;

    #endregion

    #region Start Method

    // Use this for initialization
    void Start () {

        player1 = GameObject.FindGameObjectWithTag("Player");

        if (SceneManager.GetActiveScene().name == "TwoPlayerScene");
        {
            twoPlayer = true;
        }

    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        //if (twoPlayer)
    }

    #region Methods

    void CreateUICanvas()
    {

    }

    #endregion
}
