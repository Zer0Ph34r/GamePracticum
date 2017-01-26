using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    #region Feilds

    // Main Cemera
    Camera mainCamera;

    // Gem game objects
    GameObject whiteGem;
    GameObject redGem;
    GameObject yellowGem;
    GameObject greenGem;
    GameObject blueGem;
    GameObject purpleGem;

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    // 2D array of table contents
    GameObject[,] gems;

    #endregion

    // Use this for initialization
    void Start () {

        #region Load Assets
        whiteGem = Resources.Load<GameObject>("Prefabs/PryamidWhite");
        redGem = Resources.Load<GameObject>("Prefabs/PyramidRed");
        yellowGem = Resources.Load<GameObject>("Prefabs/PryamidYellow");
        greenGem = Resources.Load<GameObject>("Prefabs/PryamidGreen");
        blueGem = Resources.Load<GameObject>("Prefabs/PyramidBlue");
        purpleGem = Resources.Load<GameObject>("Prefabs/PyramidPurple");
        #endregion

        #region Create Game Board
        // create table
        gems = new GameObject[tableSize, tableSize];
        // fill table and create game board
        CreateGameBoard();

        #endregion

        #region Set Camera
        //get main camera
        mainCamera = Camera.main;
        // set camera's position according to table size
        mainCamera.transform.position = new Vector3(tableSize, tableSize, tableSize);

        #endregion

    }

    // Update is called once per frame
    void Update () {
		
	}

    #region Methods

    /// <summary>
    /// Creates game board according to game board size
    /// </summary>
    void CreateGameBoard()
    {
        for (int i = 0; i < tableSize; ++i)
        {
            for (int k = 0; k < tableSize; ++k)
            {
                GameObject go = (GameObject)Instantiate(RandomizeObject(), new Vector3(i, k, 0), Quaternion.identity);
                gems[i, k] = go;
            }
        }
    }

    /// <summary>
    /// Returns random gem color to create
    /// </summary>
    /// <returns></returns>
    GameObject RandomizeObject()
    {
        // return object
        GameObject returnGem = new GameObject();
        // radom number between 0 and number of gems
        switch ((int)Random.Range(0,6))
        {
            case 0:
                {
                    returnGem = whiteGem;
                    break;
                }
            case 1:
                {
                    returnGem = redGem;
                    break;
                }
            case 2:
                {
                    returnGem = blueGem;
                    break;
                }
            case 3:
                {
                    returnGem = greenGem;
                    break;
                }
            case 4:
                {
                    returnGem = yellowGem;
                    break;
                }
            case 5:
                {
                    returnGem = purpleGem;
                    break;
                }
        }

        return returnGem;
    }

    #endregion
}
