using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerScript : NetworkBehaviour
{

    #region Fields

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
    public GameObject[,] gems; /*{ get; set; }*/
    GameObject[] playerHand;

    // save object positions for swapping
    public Vector3 handPos;
    public Vector3 boardPos;

    // Objects to swap
    GameObject handPiece;
    GameObject boardPiece;

    // bool for checking locked status
    bool handLocked = false;
    bool gridLocked = false;

    #endregion

    List<GameObject> tempCheck;

    #endregion

    #region Start
    private void Start()
    {
        #region Load Assets
        // Load Gems
        whiteGem = Resources.Load<GameObject>("Prefabs/Gems/WhiteD20");
        redGem = Resources.Load<GameObject>("Prefabs/Gems/RedD12");
        yellowGem = Resources.Load<GameObject>("Prefabs/Gems/YellowD9");
        greenGem = Resources.Load<GameObject>("Prefabs/Gems/GreenD8");
        blueGem = Resources.Load<GameObject>("Prefabs/Gems/BlueD6");
        purpleGem = Resources.Load<GameObject>("Prefabs/Gems/PurpleD10");
        // Load Sprites
        gridBackground = Resources.Load<Sprite>("Sprites/GridBackground");
        #endregion

        tempCheck = new List<GameObject>();

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
            GameObject go = (GameObject)Instantiate(RandomizeObject(), new Vector3(transform.position.x + 11, i + 7, 0), Quaternion.identity);
            go.GetComponent<GemScript>().isHand = true;
            playerHand[i] = go;
        }

        #endregion

        #region Create Background
        //// creat game object , add spreite renderer and set the background sprite as the render sprite
        //GameObject background = new GameObject();
        //background.AddComponent<SpriteRenderer>();
        //background.GetComponent<SpriteRenderer>().sprite = gridBackground;
        //// Move game object behind gems
        //background.transform.position = new Vector3((int)transform.position.x + 4.5f, 4.5f, -1);

        #endregion

        #region Add Event Methods

        // add method for when a gem is selected
        GemScript.gridSelected += lockGridGems;
        GemScript.handSelected += lockHandGems;

        #endregion

        #region Check Grid
        // Prevent start board from having chains
        // Make sure the board starts without any chains in it
        ResolveGrid();
        
        #endregion
    }
    #endregion

    #region Methods

    #region Instantiation Methods

    #region Create Board
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
                CreateBoardPiece( i, k);
            }
        }
    }
    #endregion

    #region Create Piece

    /// <summary>
    /// Creates Random Piece and adds it ot the board of gems
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CreateBoardPiece(int x, int y)
    {
        GameObject go = Instantiate(RandomizeObject(),
            new Vector3((int)transform.position.x + x, y, 0), Quaternion.identity, transform);
        go.GetComponent<GemScript>().isHand = false;
        go.GetComponent<GemScript>().xPos = x;
        go.GetComponent<GemScript>().yPos = y;
        gems[x, y] = go;
    }
    #endregion

    #region Refill Board

    /// <summary>
    /// Creates Random Piece and adds it ot the board of gems
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void FillBoardPiece(int x, int y)
    {
        GameObject go = Instantiate(RandomizeObject(),
            new Vector3((int)transform.position.x + x, y, 0), Quaternion.identity, transform);
        handPiece = go;
        go.GetComponent<GemScript>().isHand = false;
        go.GetComponent<GemScript>().xPos = x;
        go.GetComponent<GemScript>().yPos = y;
        gems[x, y] = go;
        // Check if this new gem creates a chain
        if (CheckValidSwap(x, y))
        {
            Destroy(gems[x, y]);
            gems[x, y] = null;
            CreateBoardPiece(x, y);
        }
        handPiece = null;
    }

    #endregion

    #region Randomize Object

    /// <summary>
    /// Returns random gem color to create
    /// </summary>
    /// <returns></returns>
    GameObject RandomizeObject()
    {
        // return object
        GameObject returnGem = null;
        // radom number between 0 and number of gems
        switch ((int)Random.Range(0, 6))
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

    #endregion

    #region Events

    #region Lock Hand

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

    #endregion

    #endregion

    #region Lock Grid
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
        // List for storing all chains created
        List<MoveScript> chains;

        // FInd all possible Chains created
        chains = CheckGrid();

        if (chains.Count > 0)
        {
            // Delete Chains and reset grid
            ResolveGrid(chains);
        }
    }

    /// <summary>
    /// Resolves board with given list of chains
    /// </summary>
    /// <param name="list"></param>
    void ResolveGrid(List<MoveScript> list)
    {
        // Remove all gems that form chains of 3 or more 
        DeleteChains(list);

        // Fill all holes in grid
        RefillGrid();
    }
    #endregion

    #region CheckGrid

    /// <summary>
    /// Finds all possible Chains in grid and returns a list of all unique chains
    /// </summary>
    /// <returns></returns>
    List<MoveScript> CheckGrid()
    {

        #region Fields
        // MoveScript is a container for holding lists of cells
        // currColor is the current tag for checking matches
        string currColor = "";
        // currCell is the current cell being checked
        GameObject currCell;
        // leftMove is a movesGrid at [row, column - 1]
        //aboveColor is the tag of the gem above the current gem
        // aboveMove is the move above the currMove [row - 1, column]
        #endregion

        #region Find all possible solutions

        // movesGrid is an empty 2D array of size [tablesize, tablesize]
        //MoveScript[,] movesGrid = new MoveScript[tableSize, tableSize];
        List<MoveScript> moves = new List<MoveScript>();
        // This loop will look through the whole 
        // table and find all possible matches
        #region Old Code
        ////  - Loop through each row
        //for (int i = 0; i < tableSize; ++i)
        //{
        //    // prevColor is the last color checked
        //    string prevColor = "";
        //    //  - Loop through each column
        //    for (int k = 0; k < tableSize; ++k)
        //    {
        //        // set currCell and currColor
        //        currCell = gems[i, k];
        //        currColor = currCell.tag;
        //        // Check for null references
        //        if (currColor != "")
        //        {
        //            // check currColor against prevColor
        //            if (currColor != prevColor)
        //            {
        //                // check if currColor is equal to above color
        //                if (i > 0 && gems[i - 1, k].tag == currColor)
        //                {
        //                    // mave current move equal move above it
        //                    movesGrid[i, k] = movesGrid[i - 1, k];
        //                }
        //                else
        //                {
        //                    // create a new empty move
        //                    movesGrid[i, k] = new MoveScript();
        //                }
        //            }
        //            else
        //            {
        //                // if left move and above move are not equal
        //                if (i > 0 && gems[i - 1, k].tag == currColor &&
        //                    movesGrid[i - 1, k] != movesGrid[i, k - 1])
        //                {
        //                    // combine left and above moves into one move
        //                    MoveScript combinedMoves = new MoveScript();
        //                    combinedMoves.AddMoves(movesGrid[i - 1, k], movesGrid[i, k - 1]);
        //                    foreach (GameObject go in combinedMoves.GetList)
        //                    {
        //                        movesGrid[(int)go.GetComponent<GemScript>().xPos, (int)go.GetComponent<GemScript>().yPos] = combinedMoves;
        //                    }
        //                    // set current move equal to combines moves
        //                    movesGrid[i, k] = combinedMoves;
        //                }

        //                else
        //                {
        //                    // set currMove to leftMove
        //                    movesGrid[i, k] = movesGrid[i, k - 1];
        //                }
        //            }
        //            // add currCell to currMove
        //            movesGrid[i, k].AddMove(currCell);
        //        }
        //        // set prevColor to currColor for next iteration
        //        prevColor = currColor;
        //    }
        //}
        #endregion

        #region Two Nested loops for finding all straight Chains
        // check Columns
        int moveCount = -1;
        for (int i = 0; i < tableSize; ++i)
        {
            // prevColor is the last color checked
            string prevColor = "";
            for (int k = 0; k < tableSize; ++k)
            {
                currCell = gems[i, k];
                currColor = currCell.tag;
                if (currColor != prevColor)
                {
                    moves.Add(new MoveScript());
                    moveCount++;
                }
                moves[moveCount].AddMove(currCell);
                prevColor = currColor;
            }
        }

        for (int i = 0; i < tableSize; ++i)
        {
            // prevColor is the last color checked
            string prevColor = "";
            for (int k = 0; k < tableSize; ++k)
            {
                currCell = gems[k, i];
                currColor = currCell.tag;
                if (currColor != prevColor)
                {
                    moves.Add(new MoveScript());
                    moveCount++;
                }
                moves[moveCount].AddMove(currCell);
                prevColor = currColor;
            }
        }


        #endregion
        #endregion

        #region Prevent Duplicate "Moves"

        // Create a new list to house unique solutions
        List<MoveScript> nonDuplicate = new List<MoveScript>();
        // iterate through each movescript in movesGrid
        foreach (MoveScript move in moves)
        {
            // check if solution is longer than 2
            if (move.GetList.Count > 2)
            {
                // check if solution exists already
                if (!nonDuplicate.Contains(move))
                {
                    // If criteria is met, add this solution
                    nonDuplicate.Add(move);
                }
            }
        }

        return nonDuplicate;

        #endregion


    }

    #endregion

    #region Delete Chains

    void DeleteChains(List<MoveScript> list)
    {
        // Iterate through each unique solution and delete all gems contained within
        foreach (MoveScript move in list)
        {
            foreach (GameObject go in move.GetList)
            {
                // Check for null objects
                if (go)
                {
                    gems[(int)go.GetComponent<GemScript>().xPos, (int)go.GetComponent<GemScript>().yPos] = null;
                    Destroy(go);
                }
            }
        }
    }

    #endregion

    #region Swap Pieces
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
            #region Move Board Piece
            // Move board piece to hand position
            boardPiece.transform.position = handPos;
            // Add board piece to player hand
            playerHand[((int)handPos.y - 7)] = boardPiece;
            // Turn board piece into hand piece
            boardPiece.GetComponent<GemScript>().isHand = true;
            #endregion

            #region Move Hand Piece
            // set new positions to hand Piece
            handPiece.transform.position = boardPos;
            // set handPiece into gem array
            gems[(int)boardPos.x, (int)boardPos.y] = handPiece;
            // make handPiece a board piece
            handPiece.GetComponent<GemScript>().isHand = false;
            #endregion

            // reset swapping objects
            boardPiece.GetComponent<GemScript>().Reset();
            handPiece.GetComponent<GemScript>().Reset();

            // reset hand and board pieces
            boardPiece = null;
            handPiece = null;
            handPos = Vector3.zero;
            boardPos = Vector3.zero;


            // resolve all possible solutions on board
            ResolveGrid();
        }
        else
        {
            // Reset board so payer can pick again
            ResetBoard();

            // reset nad and board pieces
            boardPiece = null;
            handPiece = null;
        }

    }
    #endregion

    #region Check Swap

    /// <summary>
    /// Checks if a swap of tiles is valid of not based on which gem in the grid is being swapped
    /// </summary>
    bool CheckValidSwap(int x, int y)
    {
        // check if the piece is at least 3 away from the left edge
        if (x - 2 >= 0)
        {
            if (gems[x - 1, y] != null &&
                gems[x - 2, y] != null &&
                handPiece.CompareTag(gems[x - 1, y].tag) &&
                handPiece.CompareTag(gems[x - 2, y].tag))
            {
                return true;
            }
        }
        // check if the piece is at least 3 away from the right edge
        if (x + 2 < tableSize)
        {
            if (gems[x + 1, y] != null &&
                gems[x + 2, y] != null &&
                handPiece.CompareTag(gems[x + 1, y].tag) &&
                handPiece.CompareTag(gems[x + 2, y].tag))
            {
                return true;
            }
        }
        // check if the piece is at least 3 away from the bottom edge
        if (y - 2 >= 0)
        {
            if (gems[x, y - 1] != null &&
                gems[x, y - 2] != null &&
                handPiece.CompareTag(gems[x, y - 1].tag) &&
                handPiece.CompareTag(gems[x, y - 2].tag))
            {
                return true;
            }
        }
        // check if the piece is at least 3 away from the top edge
        if (y + 2 < tableSize)
        {
            if (gems[x, y + 1] != null &&
                gems[x, y + 2] != null &&
                handPiece.CompareTag(gems[x, y + 1].tag) &&
                handPiece.CompareTag(gems[x, y + 2].tag))
            {
                return true;
            }
        }
        // Check for Middle gem swap validity up and down
        if (y + 1 < tableSize &&
            y -1 > 0)
        {
            if(gems[x, y + 1] != null &&
                gems[x, y - 1] != null &&
                handPiece.CompareTag(gems[x, y + 1].tag) &&
                handPiece.CompareTag(gems[x, y - 1].tag))
            {
                return true;
            }
        }
        // Check for Middle gem swap validity left and right
        if (x + 1 < tableSize &&
            x - 1 > 0)
        {
            if (gems[x + 1, y] != null &&
                gems[x - 1, y] != null &&
                handPiece.CompareTag(gems[x + 1, y].tag) &&
                handPiece.CompareTag(gems[x - 1, y].tag))
            {
                return true;
            }
        }

        return false;

    }

    #endregion

    #region Cancel Swap

    // Resets gems back to starting position and writes out warning to player
    void CancelSwap()
    {
        // reset pieces selected to starting positions
        boardPiece.transform.position = boardPos;
        handPiece.transform.position = handPos;

        //NOTE: Tell Player that swap was invalid
    }

    #endregion

    #region Move and Refill Board
    /// <summary>
    /// Has pieces "fall" into place, then creates new gems above the holes to 
    /// fill in grid completely
    /// </summary>
    void RefillGrid()
    {
        // First, drop all the gems as low as they can go
        for (int i = tableSize; i < 0; i--)
        {
            for (int k = tableSize; k < 1; k--)
            {
                CheckFalling(i, k);
            }
        }

        // Now fill empty spaces with new gems
        for (int j = 0; j < tableSize; j++)
        {
            for (int l = 0; l < tableSize; ++l)
            {
                if (gems[j, l] == null)
                {
                    FillBoardPiece(j, l);
                }
            }
        }

        // reset game for next round
        ResetBoard();

    }

    #endregion

    #region CheckFalling

    // Checks if there is a gem beneath this one and moves it
    // if there isn't one
    void CheckFalling(int x, int y)
    {
        // check if the space below this is null
        if (y >= 1 &&
            gems[x, y] != null &&
            gems[x, y + 1] == null)
        {
            // move gem to new position
            gems[x, y].transform.position = new Vector3((int)transform.position.x + x, y + 1, 0);
            // set gem to new grid position
            gems[x, y + 1] = gems[x, y];
            // set old position to null
            gems[x, y] = null;
            // check below this new position
            CheckFalling(x, y + 1);
        }
    }

    #endregion

    #region Reset Board

    /// <summary>
    /// Resets all board and ahdn pieces so they can be selected again
    /// </summary>
    void ResetBoard()
    {
        // reset all gems to unlocked and unselected state 
        foreach (GameObject gem in gems)
        {
            gem.GetComponent<GemScript>().Reset();
        }
        foreach (GameObject gem in playerHand)
        {
            gem.GetComponent<GemScript>().Reset();
        }

        // reset game board and hand
        // null reference pieces
        boardPiece = null;
        handPiece = null;

        // reset locked status
        gridLocked = false;
        handLocked = false;
    }

    #endregion

    #endregion

    #endregion
}