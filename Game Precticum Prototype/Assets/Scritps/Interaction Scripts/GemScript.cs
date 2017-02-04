using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    #region Fields

    // bool for being selected
    public bool isSelected { get; set; }
    public bool canSelect { get; set; }
    public bool isHand { get; set; }

    #region EventFields

    // Create delegate for adding in method calls
    public delegate void callMethod();
    // create event for calling that delegate
    public static event callMethod onSelected;

    #endregion

    #endregion

    private void Start()
    {
        isSelected = false;
        canSelect = true;
        GameControllerScript.BlockGems += ReturnThis;
    }

    #region Methods

    // When the Mouse clicks on a gem
    public void OnMouseDown()
    {
        // Flips between selected and unselected states
        ChangeState();
    }

	// changes gem state so only one gem can be selected at a time
	public void ChangeState()
	{
		if (!isSelected && canSelect) 
		{
            isSelected = true;
            transform.GetChild(0).gameObject.SetActive(true);
            onSelected();
        }
		else
		{
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }

	}

    GameObject ReturnThis()
    {
        return gameObject;
    }

    #endregion
}
