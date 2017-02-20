using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwapButtonScript : MonoBehaviour {

    #region Fields

    // This button prevents players from interacting on the other players screen
    [SerializeField]
    Button overayButton;

    // bool for checking swap state
    bool swapped = false;

    // Main Camera
    Camera mainCamera;
    Camera otherCamera;

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
    }

    #region Swap Camera View
    /// <summary>
    /// Swaps which camera is rendering to the screen
    /// </summary>
    public void SwapCameraView()
    {
        // get the other camera in the scene
        otherCamera = FindObjectOfType<Camera>();
        // check for null
        if (otherCamera != null)
        {
            // if the second camera is active
            if (swapped)
            {
                // set main camera active and deactivate the other camera
                mainCamera.enabled = true;
                otherCamera.enabled = false;
                overayButton.enabled = false;
            }
            else
            {
                mainCamera.enabled = false;
                otherCamera.enabled = true;
                overayButton.enabled = true;
            }
        }
        
    }
    #endregion
}
