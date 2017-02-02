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
	public GameObject[,] gems {get; set; }

	// Bool for checking valid moves
	bool isValid = false;

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

        #region Set Camera
        //get main camera
        mainCamera = Camera.main;
        // set camera's position according to table size
        mainCamera.transform.position = new Vector3(tableSize / 2, tableSize * (7 / 8f), tableSize * 6);
        // Move Camera to face the gems instantiated
        mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        #endregion

		#region Create Game Board
		// create table
		gems = new GameObject[tableSize, tableSize];
		// fill table and create game board
		CreateGameBoard();

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
	private void Update()
	{
		// Check if person has selected a gem or not
		if ( Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			//Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast(Camera.main.WorldToScreenPoint (Input.mousePosition), Vector3.forward, out hit, 1000f, 0, QueryTriggerInteraction.Collide))
			{
				hit.transform.gameObject.GetComponent<GemScript>().SelectGem();
			}
		}

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

    void ResolveGrid()
    {
        // NOTE: if there are strings of 3 or more, reolve them all, then call refill grid
    }

	/// <summary>
	/// Checks if a swap of tiles is valid of not based on which gem in the grid is being swapped
	/// </summary>
	void CheckValidSwap(int x, int y)
	{
		if (x - 2 >= 0) 
		{
			if (gems [x, y].tag == gems [x - 1, y].tag &&
				gems [x, y].tag == gems [x - 2, y].tag)
			{
				isValid = true;
			}
		}
		if (x + 2 <= tableSize)
		{
			if (gems [x, y].tag == gems [x + 1, y].tag &&
				gems [x, y].tag == gems [x + 2, y].tag)
			{
				isValid = true;
			}
		}
		if (y - 2 >= 0) 
		{
		
			if (gems [x, y].tag == gems [x, y - 1].tag &&
				gems [x, y].tag == gems [x, y - 1].tag)
			{
				isValid = true;
			}
		}
		if (y + 2 <= tableSize) {
			if (gems [x, y].tag == gems [x, y + 1].tag &&
			    gems [x, y].tag == gems [x, y + 2].tag) 
			{
				isValid = true;
			}
		} 
		else 
		{
			isValid = false;
		}

		if (isValid) 
		{
			ResolveGrid ();
		}
		else
		{
			CancelSwap ();
		}

	}

	// Resets gems back to starting position and writes out warning to player
	void CancelSwap()
	{
		Debug.Log ("Invalid Move: You must connect at least three similar gems");
		//NOTE: Add code for moving gems abck into starting positions
	}

    void RefillGrid()
    {
        // NOTE: Drop gems above empty grid spaces until Grid is full
    }

    #endregion
}
