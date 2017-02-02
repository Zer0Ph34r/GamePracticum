using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    #region Fields

    // bool for being selected
    bool isSelected = false;

    #endregion

	#region EventFields
	// Create delegate for adding in method calls
	public delegate void callMethod();
	// create event for calling that delegate
	public static event callMethod onSelected;

	#endregion

	//void Start()
	//{
	//	onSelected += ChangeState;
	//}

    #region Methods

    // When the Mouse clicks on a gem
    public void OnMouseDown()
    {
        // Flips between selected and unselected states
        ChangeState();
        onSelected();
    }

	// changes gem state so only one gem can be selected at a time
	public void ChangeState()
	{
		if (isSelected) 
		{
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(true);
        }
		else 
		{
            isSelected = true;
            transform.GetChild(0).gameObject.SetActive(false);
        }

	}

    // add event to this object to call when it is selected
    public void AddEvent(callMethod methodName)
    {
        onSelected += methodName;
    }
    

    #endregion
}
