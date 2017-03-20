using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class GemScript : MonoBehaviour
{
    #region Fields

    #region Regular Fields 
    // bool for being selected
    public bool isSelected { get; set; }
    // Bool for hand pieces
    public bool isHand; /*{ get; set; }*/

    // Bool to prevent gem selectoin while gem is moving
    public bool canSelect { get; set; }

    // Serialable field
    public SyncGem serialGem { get; set; }

    // particle effect for destruction
    [SerializeField]
    ParticleSystem particleSystem;
    short color;
    #endregion

    #region Move
    // all variables needed for movement lerping
    float speed = GlobalVariables.MOVE_SPEED;
    Vector3 endPos = Vector3.zero;

    #endregion

    #region EventFields

    // Create delegate for locking gems
    public delegate void callMethod(GameObject go);
    public static event callMethod Selected = null;

    // Create delegate and event for playing sounds during the game
    public delegate void runNext();
    public static event runNext fireSoundEvent = null;

    // event for finishing coroutines
    public static event runNext runNextMethod = null;

    //Check falling state event
    public delegate bool check();
    public static event check checkGems = null;

    #endregion

    #endregion

    #region Start

    private void Awake()
    {
        #region Get Color
        // set color short based on color tag
        switch (gameObject.tag)
        {
            case "White":
                color = 0;
                break;
            case "Yellow":
                color = 1;
                break;
            case "Blue":
                color = 2;
                break;
            case "Green":
                color = 3;
                break;
            case "Red":
                color = 4;
                break;
            case "Purple":
                color = 5;
                break;
        }
        #endregion

        SetGemInfo();
    }

    public GemScript()
    {
        // set initial state
        isSelected = false;
        canSelect = true;
    }

    #endregion

    #region Methods

    public void SetGemInfo()
    {
        serialGem = new SyncGem((short)transform.localPosition.x, (short)transform.localPosition.y, color);
    }

    #region Select
    // When the Mouse clicks on a gem
    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canSelect)
            {
                // Flips between selected and unselected states
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

                // fire selected Event
                Selected(gameObject);
            }
        }
        
    }
    #endregion

    #region Reset Gem
    /// <summary>
    /// Reset a Piece to how it began it's life
    /// </summary>
    public void Reset()
    {
        // reset piece to starting conditions
        isSelected = false;
        transform.GetChild(0).gameObject.SetActive(false);

    }

    #endregion

    #region Destory Gem

    /// <summary>
    /// Destroys gem and creates a new particle effect
    /// </summary>
    public void BlowUp()
    {
        // Play blow up sound effect
        fireSoundEvent();
        // create new particle effect
        ParticleSystem ps = Instantiate(particleSystem);
        ps.transform.position = transform.position;
        ParticleSystem.MainModule mm = ps.main;
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = true;
        // Destroy game object
        //gameObject.GetComponent<MeshRenderer>().enabled = false;
        Destroy(gameObject);
    }

    #endregion

    #endregion

    #region Co-Routines

    /// <summary>
    /// Coroutine for swapping gems
    /// </summary>
    /// <returns></returns>
    IEnumerator SwapPieces()
    {
        canSelect = false;
        // loops for learping between positions
        while (Vector3.Distance(transform.localPosition, endPos) > 0.01)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPos, speed * Time.deltaTime);
            yield return null;
        }
        // perfectly align gem
        transform.localPosition = endPos;
        // No longer moving, can select gem
        canSelect = true;
        // Reset Serializable Info
        serialGem = new SyncGem((short)transform.localPosition.x, (short)transform.localPosition.y, color);
        //Fire Event after coroutine ends
        if (runNextMethod != null &&
            checkGems())
        {
            runNextMethod();
        }
        
    }

    /// <summary>
    /// Coroutine for falling gems
    /// </summary>
    /// <returns></returns>
    IEnumerator Fall()
    {
        canSelect = false;
        // loops for learping between positions
        while (Vector3.Distance(transform.position, endPos) > 0.01)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, 1.7f * Time.deltaTime);
            yield return null;
        }
        // perfectly align gem
        transform.position = endPos;
        // No longer moving, can select gem
        canSelect = true;
        // Reset Serializable Info
        serialGem = new SyncGem((short)transform.localPosition.x, (short)transform.localPosition.y, color);
        //Fire Event after coroutine ends
        if (runNextMethod != null &&
            checkGems())
        {
            runNextMethod();
        }
        
    }

    #region Call Coroutines
    /// <summary>
    /// public method for calling coroutine
    /// </summary>
    /// <param name="endPos"></param>
    public void RunSwap(Vector3 endPos)
    {
        this.endPos = endPos;
        StartCoroutine(SwapPieces());
    }

    /// <summary>
    /// public method for calling coroutine
    /// </summary>
    /// <param name="endPos"></param>
    public void RunFall(Vector3 endPos)
    {
        this.endPos = endPos;
        StartCoroutine(Fall());
    }
    #endregion

    #endregion
}
