using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    #region Fields

    // bool for being selected
    bool isSelected = false;
	public bool canSelect = true;

    #endregion

	#region EventFields
	// Create delegate for adding in method calls
	public delegate void callMethod();
	public delegate void checkSwap(int i, int k);
	// create event for calling that delegate
	public static event callMethod onSelected;
	public static event checkSwap onSwap;

	#endregion

	void Start()
	{
		onSelected += ChangeState;
	}

    #region Methods

    // When the Mouse clicks on a gem
    public void OnMouseDown()
    {
        // check if gem is selected or not and set bool accordingly
        if (isSelected)
        {
			//onSelected ();
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(false);

        }
		else if (canSelect)
        {
            //isSelected = true;
            transform.GetChild(0).gameObject.SetActive(true);
			onSelected ();
        }
        
    }

	// changes gem state so only one gem can be selected at a time
	public void ChangeState()
	{
		if (!isSelected &&
		canSelect) 
		{
			canSelect = false;
		}
		else if (!canSelect) 
		{
			canSelect = true;
			isSelected = false;
		}

	}

	void SwapPieces(GameObject hand, GameObject board)
	{
		// save object positions
		Vector3 handPos;
		Vector3 boardPos;
		// set positions
		handPos = hand.transform.position;
		boardPos = board.transform.position;
		// set new positions
		hand.transform.position = boardPos;
		board.transform.position = handPos;
		if (onSwap != null)
		{
			onSwap ((int)boardPos.x, (int)boardPos.y);
		}

	}

    #endregion
}
