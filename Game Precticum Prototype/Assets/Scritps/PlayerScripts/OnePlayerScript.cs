using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OnePlayerScript : NetworkBehaviour
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

    // background for making it easier to see everything
    Sprite background;

    #endregion

    #region Table/Hand Stuff

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    // 2D array of table contents
    GameObject[,] gems;
    GameObject[,] rotation;
    GameObject[] playerHand;

    // save object positions for swapping
    Vector3 handPos;
    Vector3 boardPos;

    // Objects to swap
    GameObject handPiece;
    GameObject boardPiece;

    // bool for checking locked status
    bool handLocked = false;
    bool gridLocked = false;

    #endregion

    // bool for checking if the top row has been destroyed in a swap
    bool topSwapped = false;

    // Main Cemera
    Camera mainCamera;

    // Score tracker
    public int score { get; set; }

    // Current number of turns
    public int turns { get; set; }
    bool canSelect = true;

    #region Events

    // Create event for setting score on gem destruction
    public delegate void setScore();
    public static event setScore fireScore;
    public static event setScore endGame;

    #endregion

    #endregion

    #region Start

    private void Start()
    {
        // initialize score
        score = 0;
        turns = GlobalVariables.TURNS;

        #region Load Assets

        // Load Gems
        whiteGem = Resources.Load<GameObject>("Prefabs/Gems/WhiteD20");
        redGem = Resources.Load<GameObject>("Prefabs/Gems/RedD12");
        yellowGem = Resources.Load<GameObject>("Prefabs/Gems/YellowD9");
        greenGem = Resources.Load<GameObject>("Prefabs/Gems/GreenD8");
        blueGem = Resources.Load<GameObject>("Prefabs/Gems/BlueD6");
        purpleGem = Resources.Load<GameObject>("Prefabs/Gems/PurpleD10");
        // Load Sprites
        gridBackground = Resources.Load<Sprite>("Sprites/GridBacking");

        #endregion

        #region Create Game Board
        // create table
        gems = new GameObject[tableSize, tableSize];
        rotation = new GameObject[tableSize, tableSize];
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
            GameObject go = (GameObject)Instantiate(RandomizeObject(), new Vector3(transform.localPosition.x + i, tableSize + 1, 0), Quaternion.identity, transform);
            go.GetComponent<GemScript>().isHand = true;
            playerHand[i] = go;
            GameObject handBG = new GameObject();
            handBG.transform.SetParent(transform);
            handBG.AddComponent<SpriteRenderer>();
            handBG.GetComponent<SpriteRenderer>().sprite = gridBackground;
            handBG.transform.position = new Vector3(transform.localPosition.x + i, tableSize + 1, -0.5f);
        }

        #endregion

        #region Add Event Methods

        // add method for when a gem is selected
        GemScript.Selected += lockGems;

        // Add event for checking if gems are falling
        GemScript.checkGems += CheckGems;

        #endregion

        #region Set Camera

        //get main camera
        mainCamera = Camera.main;
        // set camera's position according to table size
        mainCamera.transform.localPosition = new Vector3(tableSize / 2 + transform.position.x,
            tableSize * (6 / 8f) + transform.position.y,
            tableSize * 5);
        // Move Camera to face the gems instantiated
        mainCamera.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));

        #endregion

        #region Create Background
        // Load in Sprite
        background = Resources.Load<Sprite>("Sprites/Gembg2");
        // create empty object, set parent and then add sprite renderer
        GameObject bg = new GameObject();
        bg.transform.SetParent(transform);
        bg.AddComponent<SpriteRenderer>().sprite = background;
        bg.transform.position = new Vector3(tableSize / 2, tableSize / 2, -40);

        #endregion

        #region Check Grid
        // Prevent start board from having chains
        // Make sure the board starts without any chains in it
        ResolveOnStart();

        #endregion
                
    }

    #endregion

    #region Methods

    #region Check Gems

    /// <summary>
    /// Checks all gems to see how many are currently falling
    /// returns true if one or fewer are falling
    /// </summary>
    /// <returns></returns>
    bool CheckGems()
    {
        // set falling to 0
        int falling = 0;
        foreach (GameObject gem in gems)
        {
            // Add 1 if a gem cannot be selected, which means it is falling
            if (gem !=null &&
                !gem.GetComponent<GemScript>().canSelect)
            {
                falling++;
            }
        }
        if (falling > 0)
        {
            // If any gem is still falling
            return false;
        }
        else
        {
            return true;
        }
        
    }

    #endregion

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
                CreateBoardPiece(i, k);
                GameObject go = new GameObject();
                go.transform.SetParent(transform);
                go.AddComponent<SpriteRenderer>();
                go.GetComponent<SpriteRenderer>().sprite = gridBackground;
                go.transform.localPosition = new Vector3(i, k , -0.5f);
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
            new Vector3((int)transform.localPosition.x + x,
            (int)transform.localPosition.y + y,
            0), Quaternion.identity, transform);
        go.GetComponent<GemScript>().isHand = false;
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
        // create new piece at array position plus parent transform
        GameObject go = Instantiate(RandomizeObject(),
            new Vector3((int)transform.localPosition.x + x,
            (int)transform.localPosition.y + y,
            0), Quaternion.identity, transform);
        // set handpiece to new game object for checking 
        handPiece = go;
        go.GetComponent<GemScript>().isHand = false;
        gems[x, y] = go;
        // Check if this new gem creates a chain
        if (CheckValidSwap(x, y))
        {
            Destroy(gems[x, y]);
            gems[x, y] = null;
            FillBoardPiece(x, y);
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

    #region Lock Gems

    /// <summary>
    /// Prevents multiple gems from being selected or swapped
    /// </summary>
    void lockGems(GameObject go)
    {
        GemScript gs = go.GetComponent<GemScript>();
        if (gs.isHand)
        {
            if (go.GetComponent<GemScript>().isSelected)
            {
                // set handPiece to go
                handPiece = go;
            }
            else
            {
                handPiece = null;
            }
            
            // lock each peice
            foreach (GameObject gem in playerHand)
            {
                if (gem != go)
                {
                    gem.GetComponent<GemScript>().isSelected = false;
                    gem.GetComponent<GemScript>().transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            // Show hand is locked
            handLocked = true;
        }
        else
        {
            if (go.GetComponent<GemScript>().isSelected)
            {
                // set handPiece to go
                boardPiece = go;
            }
            else
            {
                boardPiece = null;
            }

            // lock each peice
            foreach (GameObject gem in gems)
            {
                if (gem != go)
                {
                    gem.GetComponent<GemScript>().isSelected = false;
                    gem.GetComponent<GemScript>().transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            // Show hand is locked
            gridLocked = true;
        }
        // check if both grid and hand are locked
        if (boardPiece != null &&
            handPiece != null &&
            handLocked && gridLocked)
        {
            SwapPieces();
        }
    }

    #endregion

    #region OnStart Grid Resolution

    /// <summary>
    /// Used for starting the game board with no pieces without seeing them move or earning score
    /// </summary>
    void ResolveOnStart()
    {
        // List for storing all chains created
        List<MoveScript> chains;

        // FInd all possible Chains created
        chains = CheckGrid();

        if (chains.Count > 0)
        {
            // Remove all gems that form chains of 3 or more 
            DeleteOnStart(chains);

            // Fill all holes in grid
            RefillOnStart();

            // check for any chains made after grid has fallen and been refilled
            ResolveOnStart();
        }
    }

    /// <summary>
    /// Moves pieces down the board if there are any matching chains
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void CheckEmptyStart(int x, int y)
    {
        // check if the space below this is null
        if (y >= 1 &&
            gems[x, y] != null &&
            gems[x, y - 1] == null)
        {
            // Move gem into correct position
            gems[x, y].transform.position = new Vector3((int)transform.localPosition.x + x, y - 1, 0);
            // set gem to new grid position
            gems[x, y - 1] = gems[x, y];
            // set old position to null
            gems[x, y] = null;
            // check below this new position
            CheckEmptyStart(x, y - 1);
        }
    }

    /// <summary>
    /// Has pieces "fall" into place, then creates new gems above the holes to 
    /// fill in grid completely
    /// </summary>
    void RefillOnStart()
    {
        // First, drop all the gems as low as they can go
        //for (int i = 0; i < tableSize; i++)
        //{
        //    for (int k = 1; k < tableSize; k++)
        //    {
        //        CheckEmptyStart(i, k);
        //    }
        //}

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

    // deletes all gems in every chain
    void DeleteOnStart(List<MoveScript> list)
    {
        // Iterate through each unique solution and delete all gems contained within
        foreach (MoveScript move in list)
        {
            foreach (GameObject go in move.GetList)
            {
                // Check for null objects
                if (go)
                {
                    gems[(int)(go.GetComponent<GemScript>().transform.localPosition.x),
                        (int)go.GetComponent<GemScript>().transform.localPosition.y] = null;
                    Destroy(go);
                }
            }
        }
    }

    #endregion

    #region Grid Methods

    #region Resolve Grid
    /// <summary>
    /// Creates a new 2D array "Moves" which list all possible chains
    /// then sorts for unique chians and destroys all gems in those chains
    /// </summary>
    void ResolveGrid()
    {
        // remove method from event
        GemScript.runNextMethod -= ContinueSwap;

        // List for storing all chains created
        List<MoveScript> chains;

        // FInd all possible Chains created
        chains = CheckGrid();

        if (chains.Count > 0)
        {
            // Remove all gems that form chains of 3 or more 
            DeleteChains(chains);

            // Fill all holes in grid
            RefillGrid();
        }
        
    }

    #endregion

    #region Check Grid

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

        // moves is a list of all valid moves (move is a chain of two or more)
        List<MoveScript> moves = new List<MoveScript>();

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
        // This loop will look through the whole 
        // table and find all possible matches
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

        if (nonDuplicate.Count == 1 &&
            nonDuplicate[0].GetList.Count == 3)
        {
            if (nonDuplicate[0].sameX() &&
                nonDuplicate[0].returnPiece(2).
                transform.position.y == tableSize - 1)
            {
                topSwapped = true;
            }
        }
        else if (nonDuplicate.Count == 1 && 
            nonDuplicate[0].GetList.Count == 4)
        {
            if (nonDuplicate.Count == 1 &&
            nonDuplicate[0].sameX() &&
            nonDuplicate[0].returnPiece(3).
            transform.position.y == tableSize - 1)
            {
                topSwapped = true;
            }
        }
        else if (nonDuplicate.Count == 1 &&
            nonDuplicate[0].GetList.Count == 5)
        {
            if (nonDuplicate.Count == 1 &&
            nonDuplicate[0].sameX() &&
            nonDuplicate[0].returnPiece(4).
            transform.position.y == tableSize - 1)
            {
                topSwapped = true;
            }
        }


        return nonDuplicate;

        #endregion

    }

    #endregion

    #region Delete Chains

    // deletes all gems in every chain
    void DeleteChains(List<MoveScript> list)
    {
        // Iterate through each unique solution and delete all gems contained within
        foreach (MoveScript move in list)
        {
            foreach (GameObject go in move.GetList)
            {
                // Check for null objects
                if (go != null)
                {
                    gems[(int)(go.GetComponent<GemScript>().transform.localPosition.x),
                        (int)go.GetComponent<GemScript>().transform.localPosition.y] = null;
                    go.GetComponent<GemScript>().BlowUp();
                    score++;
                    fireScore();
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
        handPos = handPiece.transform.localPosition;
        boardPos = boardPiece.transform.localPosition;

        // check if it's a valid swap
        if (canSelect &&
            CheckValidSwap((int)boardPos.x, (int)boardPos.y))
        {
            //Check if game is over when game board is done reseting
            if (turns == 0)
            {
                canSelect = false;
                endGame();
            }
            else
            {
                // deduct turn count
                turns--;

                #region Move Board Piece

                // Add board piece to player hand
                playerHand[(int)handPos.x] = boardPiece;
                // Turn board piece into hand piece
                boardPiece.GetComponent<GemScript>().isHand = true;
                // Move board piece to hand position
                boardPiece.GetComponent<GemScript>().RunSwap(handPos);

                #endregion

                #region Move Hand Piece

                // make handPiece a board piece
                handPiece.GetComponent<GemScript>().isHand = false;
                // set handPiece into gem array
                gems[(int)boardPos.x, (int)boardPos.y] = handPiece;
                // set new positions to hand Piece
                handPiece.GetComponent<GemScript>().RunSwap(boardPos);

                #endregion

                if (boardPos.y == tableSize - 1)
                {
                    topSwapped = true;
                }

                GemScript.runNextMethod += ContinueSwap;
            }        
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

    void ContinueSwap()
    {
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
            y - 1 >= 0)
        {
            if (gems[x, y + 1] != null &&
                gems[x, y - 1] != null &&
                handPiece.CompareTag(gems[x, y + 1].tag) &&
                handPiece.CompareTag(gems[x, y - 1].tag))
            {
                return true;
            }
        }
        // Check for Middle gem swap validity left and right
        if (x + 1 < tableSize &&
            x - 1 >= 0)
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
        boardPiece.transform.localPosition = boardPos;
        handPiece.transform.localPosition = handPos;

        //NOTE: Tell Player that swap was invalid
    }

    #endregion

    #region Refill Grid
    /// <summary>
    /// Has pieces "fall" into place, then creates new gems above the holes to 
    /// fill in grid completely
    /// </summary>
    void RefillGrid()
    {
        // remove called event
        GemScript.runNextMethod -= RefillGrid;

        // First, drop all the gems as low as they can go
        for (int i = 0; i < tableSize; i++)
        {
            for (int k = 1; k < tableSize; k++)
            {
                CheckFalling(i, k);
            }
        }

        if (topSwapped)
        {
            FillEmpty();
            topSwapped = false;
        }
        else
        {
            // run FillEmpty script once all gems have fallen
            GemScript.runNextMethod += FillEmpty;
        }
        
    }


    #endregion

    #region Fill Empty
    /// <summary>
    /// Fills all empty grid cells
    /// </summary>
    void FillEmpty()
    {
        // remove Fill Empty Event registration
        GemScript.runNextMethod -= FillEmpty;

        // Now fill empty spaces with new gems
        for (int i = 0; i < tableSize; i++)
        {
            for (int k = 0; k < tableSize; ++k)
            {
                if (gems[i, k] == null)
                {
                    FillBoardPiece(i, k);
                }
            }
        }

        // reset game for next round
        ResetBoard();

        // check for any more gems to delete
        ResolveGrid();
    }
    #endregion

    #region Check Falling

    // Checks if there is a gem beneath this one and moves it
    // if there isn't one
    void CheckFalling(int x, int y)
    {
        // check if the space below this is null
        if (y >= 1 &&
            gems[x, y] != null &&
            gems[x, y - 1] == null)
        {

            // tell gem to move and where to move to
            gems[x, y].GetComponent<GemScript>().RunFall(new Vector3((int)transform.localPosition.x + x, y - 1, 0));

            // set gem to new grid position
            gems[x, y - 1] = gems[x, y];
            
            // set old position to null
            gems[x, y] = null;

            // check below this new position
            CheckFalling(x, y - 1);
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

    #region Rotate Board

    /// <summary>
    /// Rotates board visually while just saving gems into a new array that is "rotated" 90 deg 
    /// </summary>
    public void RotateRight()
    {
        if (CheckGems())
        {
            // Rotate peices using a simple temp object to not lose any gems
            for (int x = 0; x < tableSize / 2; x++)
            {
                // Consider elements in group of 4 in 
                // current square
                for (int y = x; y < tableSize - x - 1; y++)
                {
                    // store current cell in temp variable
                    GameObject temp = gems[x, y];

                    // move values from right to top
                    gems[x, y] = gems[y, tableSize - 1 - x];

                    // move values from bottom to right
                    gems[y, tableSize - 1 - x] = gems[tableSize - 1 - x, tableSize - 1 - y];

                    // move values from left to bottom
                    gems[tableSize - 1 - x, tableSize - 1 - y] = gems[tableSize - 1 - y, x];

                    // assign temp to left
                    gems[tableSize - 1 - y, x] = temp;
                }
            }

            //Now that we have our array rotated we need to move all gems to reflect their new rotation
            for (int i = 0; i < tableSize; ++i)
            {
                for (int k = 0; k < tableSize; ++k)
                {
                    gems[i, k].gameObject.GetComponent<GemScript>().RunSwap(new Vector3(i, k, 0));
                }
            }
        }
    }

    public void RotateLeft()
    {
        if (CheckGems())
        {
            // Rotate peices using a simple temp object to not lose any gems
            for (int x = 0; x < tableSize / 2; x++)
            {
                // Consider elements in group of 4 in 
                // current square
                for (int y = x; y < tableSize - x - 1; y++)
                {
                    // store current cell in temp variable
                    GameObject temp = gems[x, y];

                    // move values from right to top
                    gems[x, y] = gems[tableSize - 1 - y, x];

                    // move values from bottom to right
                    gems[tableSize - 1 - y, x] = gems[tableSize - 1 - x, tableSize - 1 - y];

                    // move values from left to bottom
                    gems[tableSize - 1 - x, tableSize - 1 - y] = gems[y, tableSize - 1 - x];

                    // assign temp to left
                    gems[y, tableSize - 1 - x] = temp;
                }
            }

            //Now that we have our array rotated we need to move all gems to reflect their new rotation
            for (int i = 0; i < tableSize; ++i)
            {
                for (int k = 0; k < tableSize; ++k)
                {
                    gems[i, k].gameObject.GetComponent<GemScript>().RunSwap(new Vector3(i, k, 0));
                }
            }
        }
    }

    #endregion

    #endregion

    #region On Destroy

    private void OnDestroy()
    {
        // Remove methods from events
        GemScript.Selected -= lockGems;
        GemScript.checkGems -= CheckGems;
    }

    #endregion

    #endregion

}