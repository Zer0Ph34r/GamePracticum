using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    #region Fields

    // bool for being selected
    bool isSelected = false;
    bool canSelect = true;

    #region EventFields

    // Create delegate for adding in method calls
    public delegate void callMethod();
    // create event for calling that delegate
    public static event callMethod onSelected;

    #endregion

    #endregion

    private void Start()
    {
        GameControllerScript.getGems += ReturnThis;
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
