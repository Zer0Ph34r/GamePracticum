using UnityEngine;
using UnityEngine.UI;

public class CameraSwapButtonScript : MonoBehaviour {

    #region Fields

    // This button prevents players from interacting on the other players screen
    [SerializeField]
    Button overayButton;
    GameObject opponant;

    // bool for checking swap state
    bool swapped = false;

    // Main Camera
    Camera mainCamera;
    int tableSize = GlobalVariables.TABLE_SIZE;

    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        // find the opponants position
        GameObject[] positions = new GameObject[2];
        positions = GameObject.FindGameObjectsWithTag("Position");
        foreach (GameObject go in positions)
        {
            if (go.transform.position != transform.parent.position)
            {
                opponant = go;
            }
        }
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
            SetCamera(gameObject.transform.parent.gameObject);
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

    void SetCamera(GameObject go)
    {
        // set camera's position according to given object and table size
        mainCamera.transform.localPosition = new Vector3(tableSize / 2 + go.transform.position.x,
            tableSize * (7 / 8f) + go.transform.position.y,
            tableSize * 2);
    }
}
