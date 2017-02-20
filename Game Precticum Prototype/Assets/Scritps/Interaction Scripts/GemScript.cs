using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GemScript : MonoBehaviour
{

    #region Fields

    // bool for being selected
    public bool isSelected { get; set; }
    public bool canSelect { get; set; }
    public bool isHand { get; set; }

    float speed = GlobalVariables.LERP_SPEED;

    #region EventFields

    // Create delegate for adding in method calls
    public delegate void callMethod(bool TF, GameObject go);
    // create event for calling that delegate
    public static event callMethod handSelected;
    public static event callMethod gridSelected;

    #endregion

    #endregion

    private void Start()
    {
        isSelected = false;
        canSelect = true;
    }

    #region Methods

    // When the Mouse clicks on a gem
    public void OnMouseDown()
    {
        // Flips between selected and unselected states
        ChangeState();
    }

    /// <summary>
    /// Reset a Piece to how it began it's life
    /// </summary>
    public void Reset()
    {
        // reset piece to starting conditions
        isSelected = false;
        canSelect = true;
        transform.GetChild(0).gameObject.SetActive(false);

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
                handSelected(false , gameObject);
            }
            else
            {
                gridSelected(false, gameObject);
            }
            
        }
		else if (isSelected && !canSelect)
		{
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(false);
            if (isHand)
            {
                handSelected(true, gameObject);
            }
            else
            {
                gridSelected(true, gameObject);
            }
        }
	}

    GameObject ReturnThis()
    {
        return gameObject;
    }

    #endregion

    #region Co Routines

    public IEnumerator LerpPosition(Vector3 newPos)
    {
        transform.position = Vector3.Lerp(transform.position, newPos, speed);
        yield return null;
    }

    #endregion
}
