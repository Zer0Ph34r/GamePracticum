using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    #region Fields

    // Main Cemera
    Camera mainCamera;

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    PlayerScript player1 = new PlayerScript();

    #endregion

    #region Start Method

    // Use this for initialization
    void Start () {

        #region Set Camera
        //get main camera
        mainCamera = Camera.main;
        // set camera's position according to table size
        mainCamera.transform.position = new Vector3(tableSize / 2, tableSize * (7 / 8f), tableSize * 6);
        // Move Camera to face the gems instantiated
        mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        #endregion



    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    } 
}
