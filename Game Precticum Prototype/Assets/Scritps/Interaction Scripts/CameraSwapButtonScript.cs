using UnityEngine;
using UnityEngine.UI;

public class CameraSwapButtonScript : MonoBehaviour {

    #region Fields

    // This button prevents players from interacting on the other players screen
    [SerializeField]
    Button overayButton;
    Vector3 position;
    Vector3 opponant;

    // bool for checking swap state
    bool swapped = false;

    // Main Camera
    Camera mainCamera;
    int tableSize = GlobalVariables.TABLE_SIZE;

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        // Set both positions
        position = gameObject.transform.parent.transform.position;
        opponant = new Vector3(position.x, position.y + 50, 0);
    }

    #region Swap Camera View
    /// <summary>
    /// Swaps which camera is rendering to the screen
    /// </summary>
    public void SwapCameraView()
    {
        // check swapped state
        if (swapped)
        {
            // set main camera active and deactivate the other camera
            SetCamera(position);
            overayButton.gameObject.SetActive(false);
            swapped = false;
        }
        else
        {
            SetCamera(opponant);
            overayButton.gameObject.SetActive(true);
            swapped = true;
        }

    }
    #endregion

    void SetCamera(Vector3 Pos)
    {
        // set camera's position according to given object and table size
        mainCamera.transform.position = new Vector3(tableSize / 2 + Pos.x,
            tableSize * (6 / 8f) + Pos.y,
            tableSize * 1.5f);
    }
}
