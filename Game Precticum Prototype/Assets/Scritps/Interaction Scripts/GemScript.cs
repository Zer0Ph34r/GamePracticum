using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour {

    #region Fields

    // bool for being selected
    bool isSelected = false;

    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectGem();
        }
    }

    #region Methods

    // When the Mouse clicks on a gem
    private void SelectGem()
    {
        // check if gem is selected or not and set bool accordingly
        if (isSelected)
        {
            isSelected = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            isSelected = true;
            transform.GetChild(0).gameObject.SetActive(true);
        }
        
    }

    #endregion
}
