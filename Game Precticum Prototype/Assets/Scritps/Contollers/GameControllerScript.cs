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

    // grid for gems on table
    Sprite gridBackground;

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    // 2D array of table contents
    GameObject[,] gems;

    #endregion

    // Use this for initialization
    void Start () {

        #region Load Assets
        // Load Gems
        whiteGem = Resources.Load<GameObject>("Prefabs/PyramidWhite");
        redGem = Resources.Load<GameObject>("Prefabs/PyramidRed");
        yellowGem = Resources.Load<GameObject>("Prefabs/PyramidYellow");
        greenGem = Resources.Load<GameObject>("Prefabs/PyramidGreen");
        blueGem = Resources.Load<GameObject>("Prefabs/PyramidBlue");
        purpleGem = Resources.Load<GameObject>("Prefabs/PyramidPurple");
        // Load Sprites
        gridBackground = Resources.Load<Sprite>("Sprites/GridBackground");
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
        mainCamera.transform.position = new Vector3(tableSize / 2, tableSize * (7 / 8f), tableSize * 6);
        // Move Camera to face the gems instantiated
        mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        //mainCamera.backgroundColor = Color.white;

        #endregion

        #region Create Background
        // creat game object , add spreite renderer and set the background sprite as the render sprite
        GameObject background = new GameObject();
        background.AddComponent<SpriteRenderer>();
        background.GetComponent<SpriteRenderer>().sprite = gridBackground;
        // Move game object behind gems
        background.transform.position = new Vector3(4.6f, 4.6f, -1);
        //background.transform.localScale = new Vector3(1.9f, 1.9f, 1);

        #endregion

    }

    // Update is called once per frame
    void Update () {
		
	}

    #region Instantiation Methods

    /// <summary>
    /// Creates game board according to game board size
    /// </summary>
    void CreateGameBoard()
    {
        for (int i = 0; i < tableSize; ++i)
        {
            for (int k = 0; k < tableSize; ++k)
            {
                gems[i, k] = (GameObject)Instantiate(RandomizeObject(), new Vector3(i, k, 0), Quaternion.identity);
                //gems[i, k] = go;
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
        GameObject returnGem = null;
        // radom number between 0 and number of gems
        switch ((int)Random.Range(0,6))
        {
            case 0:
                    returnGem = whiteGem;
                    break;
            case 1:
                    returnGem = redGem;
                    break;
            case 2:
                    returnGem = blueGem;
                    break;
            case 3:
                    returnGem = greenGem;
                    break;
            case 4:
                    returnGem = yellowGem;
                    break;
            case 5:
                    returnGem = purpleGem;
                    break;
        }

        return returnGem;
    }

    #endregion

    #region Grid Methods

    void CheckGrid()
    {
        // NOTE: if there are strings of 3 or more, reolve them all, then call refill grid
    }

    void RefillGrid()
    {
        // NOTE: Drop gems above empty grid spaces until Grid is full
    }

    #endregion
}
