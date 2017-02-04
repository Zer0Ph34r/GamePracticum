using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    #region Fields

    // Main Cemera
    Camera mainCamera;

    #region Load Objects
    // Gem game objects
    GameObject whiteGem;
    GameObject redGem;
    GameObject yellowGem;
    GameObject greenGem;
    GameObject blueGem;
    GameObject purpleGem;

    // grid for gems on table
    Sprite gridBackground;
    #endregion

    #region Table/Hand Stuff

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    // 2D array of table contents
	public GameObject[,] gems {get; set; }
	GameObject[] playerHand;

    // save object positions for swapping
    public Vector3 handPos;
    public Vector3 boardPos;

    #endregion

    // Objects to swap
    GameObject handPiece;
    GameObject boardPiece;

    // bool for checking locked status
    bool handLocked = false;
    bool gridLocked = false;

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
            GameObject go = (GameObject)Instantiate(RandomizeObject(), new Vector3(11, i + 7, 0), Quaternion.identity);
            go.GetComponent<GemScript>().isHand = true;
            playerHand[i] = go;
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

        #region Add Event Methods

        // add method for when a gem is selected
        GemScript.gridSelected += lockGridGems;
        GemScript.handSelected += lockHandGems;

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

    /// <summary>
    /// Prevents multiple gems from being selected or swapped
    /// </summary>
    void lockHandGems(bool tf, GameObject go)
    {
        // set handPiece to go
        handPiece = go;
        // lock each peice
        foreach (GameObject gem in playerHand)
        {
            gem.GetComponent<GemScript>().canSelect = tf;
        }
        // Show hand is locked
        handLocked = true;
        // check if both grid and hand are locked
        if (handLocked && gridLocked)
        {
            SwapPieces();
        }
    }

    /// <summary>
    /// Prevents multiple gems from being selected or swapped
    /// </summary>
    void lockGridGems(bool tf, GameObject go)
    {
        // set board piece to go
        boardPiece = go;
        // lock each peice
        foreach (GameObject gem in gems)
        {
            gem.GetComponent<GemScript>().canSelect = tf;
        }
        // Show hand is locked
        gridLocked = true;
        // check if both grid and hand are locked
        if (handLocked && gridLocked)
        {
            SwapPieces();
        }
    }

    #endregion

    #region Grid Methods

    #region Resolve Method
    /// <summary>
    /// Creates a new 2D array "Moves" which list all possible chains
    /// then sorts for unique chians and destroys all gems in those chains
    /// </summary>
    void ResolveGrid()
    {
        // MoveScript is a container for holding lists of cells
        
        // currColor is the current tag for checking matches
        string currColor = "";
        // currCell is the current cell being checked
        GameObject currCell;
        // leftMove is a movesGrid at [row, column - 1]
        
        //aboveColor is the tag of the gem above the current gem
        string aboveColor = "";
        // aboveMove is the move above the currMove [row - 1, column]

        // movesGrid is an empty 2D array of size [tablesize, tablesize]
        MoveScript[,] movesGrid = new MoveScript[tableSize, tableSize];
        // This loop will look through the whole 
        // table and find all possible matches
        //  - Loop through each row
        for (int i = 0; i < tableSize; ++i)
        {
            // prevColor is the last color checked
            string prevColor = "";
            //  - Loop through each column
            for (int k = 0; k < tableSize; ++k)
            {
                // set currCell and currColor
                currCell = gems[i, k];
                currColor = currCell.tag;
                // Check for null references
                if (currColor != "")
                {
                    // check currColor against prevColor
                    if (currColor != prevColor)
                    {
                        // check if currColor is equal to above color
                        if (i > 0 && aboveColor == currColor)
                        {
                            // mave current move equal move above it
                            movesGrid[i, k] = movesGrid[i - 1, k];
                        }
                        else
                        {
                            // create a new empty move
                            movesGrid[i,k] = new MoveScript();
                        }
                    }
                    else
                    {
                        // if left move and above move are not equal
                        if (i > 0 && aboveColor == currColor &&
                            movesGrid[i - 1, k] != movesGrid[i, k - 1])
                        {
                            // combine left and above moves into one move
                            MoveScript combinedMoves = new MoveScript();
                            combinedMoves.AddMoves(movesGrid[i - 1, k], movesGrid[i, k - 1]);
                            foreach (GameObject go in combinedMoves.GetList)
                            {
                                movesGrid[(int)go.transform.position.x, (int)go.transform.position.y] = combinedMoves;
                            }
                            // set current move equal to combines moves
                            movesGrid[i, k] = combinedMoves;
                        }
                        else
                        {
                            // set currMove to leftMove
                            movesGrid[i, k] = movesGrid[i, k - 1];
                        }
                    }
                    // add currCell to currMove
                    movesGrid[i, k].AddMove(currCell);
                }
                // set prevColor to currColor for next iteration
                prevColor = currColor;
            }
        }

        // NOTE: Add in checking for all unique chains and destroying them than calling the refill method

        RefillGrid();
    }
    #endregion

    /// <summary>
    /// Swaps two pieces and then checks for validity
    /// </summary>
    void SwapPieces()
    {
        // set positions
        handPos = handPiece.transform.position;
        boardPos = boardPiece.transform.position;
        
        // check if it's a valid swap
        if (CheckValidSwap((int)boardPos.x, (int)boardPos.y))
        {    
            // set new positions
            handPiece.transform.position = boardPos;
            boardPiece.transform.position = handPos;

            // reset swapping objects
            boardPiece = null;
            handPiece = null;

            //ResolveGrid();
        }
        else
        {
            // reset all gems to unlocked and unselected state 
            foreach(GameObject gem in gems)
            {
                gem.GetComponent<GemScript>().Reset();
            }
            foreach(GameObject gem in playerHand)
            {
                gem.GetComponent<GemScript>().Reset();
            }
            // reset locked status
            gridLocked = false;
            handLocked = false;

            // reset swapping objects
            boardPiece = null;
            handPiece = null;
        }
        
    }

    /// <summary>
    /// Checks if a swap of tiles is valid of not based on which gem in the grid is being swapped
    /// </summary>
    bool CheckValidSwap(int x, int y)
	{
        bool returnValue = false;
        // check if the piece is at least 3 away from the left edge
		if (x - 2 >= 0) 
		{
            if (gems [x, y].tag == gems [x - 1, y].tag &&
				gems [x, y].tag == gems [x - 2, y].tag)
			{
                returnValue = true;
			}
		}
        // check if the piece is at least 3 away from the right edge
        if (x + 2 <= tableSize)
		{
            if (gems [x, y].tag == gems [x + 1, y].tag &&
				gems [x, y].tag == gems [x + 2, y].tag)
			{
                returnValue = true;
			}
		}
        // check if the piece is at least 3 away from the bottom edge
        if (y - 2 >= 0) 
		{
			if (gems [x, y].tag == gems [x, y - 1].tag &&
				gems [x, y].tag == gems [x, y - 1].tag)
			{
                returnValue =  true;
			}
		}
        // check if the piece is at least 3 away from the top edge
        if (y + 2 <= tableSize)
        {
            if (gems [x, y].CompareTag(gems [x, y + 1].tag) &&
			    (gems [x, y].tag == gems [x, y + 2].tag ||
                gems[x, y].tag == gems[x, y - 1].tag)) 
			{
                //CompareTag()
                returnValue = true;
			}
		}

        return returnValue;

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
        // First, drop all the gems as low as they can go
        //for (int i = )
    }

    #endregion
}
