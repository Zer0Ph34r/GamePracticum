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
	GameObject[] playerHand;

    // save object positions for swapping
    Vector3 handPos;
    Vector3 boardPos;

    // Objects to swap
    GameObject handPiece;
    GameObject boardPiece;

    // Bool for checking valid moves
    bool isValid = false;

    #region Events

    // create delegate and event for returning selected gems
    public delegate GameObject getGem();
    public static event getGem getGems;

    #endregion

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

		#region Create Player Hand
		// Create Player Hand Empty
		playerHand = new GameObject[3];
		// Fill Player Hand with random gems
		for (int i = 0; i < 3; ++i)
		{
            // Add gem to hand array for checking later on
            playerHand[i] = (GameObject)Instantiate(RandomizeObject(), new Vector3(11, i + 4, 0), Quaternion.identity);
		}

		#endregion

        #region Create Background
        // creat game object , add spreite renderer and set the background sprite as the render sprite
        GameObject background = new GameObject();
        background.AddComponent<SpriteRenderer>();
        background.GetComponent<SpriteRenderer>().sprite = gridBackground;
        // Move game object behind gems
        background.transform.position = new Vector3(4.6f, 4.6f, -1);

        #endregion

    }
		
    #region Instantiation Methods

    /// <summary>
    /// Creates game board according to game board size
    /// </summary>
    void CreateGameBoard()
    {
        // creates 2D array of gems on the field
        for (int i = 0; i < tableSize; ++i)
        {
            for (int k = 0; k < tableSize; ++k)
            {
                GameObject go = (GameObject)Instantiate(RandomizeObject(), new Vector3(i, k, 0), Quaternion.identity);
                //go.GetComponent<GemScript>().AddEvent(SaveBoardPiece);
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

        // returns randomly generated gem
        return returnGem;
    }

    #endregion

    #region Events

    void SaveBoardPiece(GameObject boardObject)
    {
        boardPiece = boardObject;
    }

    #endregion

    #region Grid Methods

    /// <summary>
    /// Creates a new 2D array of the board to resolve all possible chains
    /// then swaps this new array with the old and calls the refill method
    /// </summary>
    void ResolveGrid()
    {
        // NOTE: if there are strings of 3 or more, resolve them all, then call refill grid method
    }

    /// <summary>
    /// Swaps two pieces and then checks for validity
    /// </summary>
    void SwapPieces()
    {
        // set positions
        handPos = handPiece.transform.position;
        boardPos = boardPiece.transform.position;
        // set new positions
        handPiece.transform.position = boardPos;
        boardPiece.transform.position = handPos;

        // after swapping pieces, check if it's a valid swap
        CheckValidSwap((int)boardPos.x, (int)boardPos.y);
    }

    /// <summary>
    /// Checks if a swap of tiles is valid of not based on which gem in the grid is being swapped
    /// </summary>
    void CheckValidSwap(int x, int y)
	{
        // check if the piece is at least 3 away from the left edge
		if (x - 2 >= 0) 
		{
			if (gems [x, y].tag == gems [x - 1, y].tag &&
				gems [x, y].tag == gems [x - 2, y].tag)
			{
				isValid = true;
			}
		}
        // check if the piece is at least 3 away from the right edge
        if (x + 2 <= tableSize)
		{
			if (gems [x, y].tag == gems [x + 1, y].tag &&
				gems [x, y].tag == gems [x + 2, y].tag)
			{
				isValid = true;
			}
		}
        // check if the piece is at least 3 away from the bottom edge
        if (y - 2 >= 0) 
		{
		
			if (gems [x, y].tag == gems [x, y - 1].tag &&
				gems [x, y].tag == gems [x, y - 1].tag)
			{
				isValid = true;
			}
		}
        // check if the piece is at least 3 away from the top edge
        if (y + 2 <= tableSize) {
			if (gems [x, y].tag == gems [x, y + 1].tag &&
			    gems [x, y].tag == gems [x, y + 2].tag) 
			{
				isValid = true;
			}
		} 
		else 
		{
            // the swap is invalid
			isValid = false;
		}

        // if the swap is valid resolve all chains on the board
		if (isValid) 
		{
			ResolveGrid ();
		}
        // if it's not valid, reset the pieces and tell the player
		else
		{
			CancelSwap ();
		}

	}

	// Resets gems back to starting position and writes out warning to player
	void CancelSwap()
	{
        // reset pieces selected to starting positions
        boardPiece.transform.position = boardPos;
        handPiece.transform.position = handPos;

        // Tell Player that swap was invalid
	}

    /// <summary>
    /// Has pieces "fall" into place, then creates new gems above the holes to 
    /// fill in grid completely
    /// </summary>
    void RefillGrid()
    {
        // NOTE: Drop gems above empty grid spaces until Grid is full
    }

    #endregion
}
