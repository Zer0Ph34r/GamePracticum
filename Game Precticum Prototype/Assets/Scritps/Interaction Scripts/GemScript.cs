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
    public delegate void callMethod(bool TF);
    // create event for calling that delegate
    public static event callMethod handSelected;
    public static event callMethod gridSelected;

    #endregion

    #endregion

    private void Start()
    {
        isSelected = false;
        canSelect = true;
        //GameControllerScript.BlockGems += ReturnThis;
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
            // locks hand or grid
            if (isHand)
            {
                handSelected(true);
            }
            else
            {
                gridSelected(true);
            }
            
        }
		else if (isSelected && !canSelect)
		{
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(false);
            if (isHand)
            {
                handSelected(false);
            }
            else
            {
                gridSelected(false);
            }
        }

	}

    GameObject ReturnThis()
    {
        return gameObject;
    }

    #endregion
}
