using System.Collections;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    #region Fields

    #region Regular Fields 
    // bool for being selected
    public bool isSelected { get; set; }
    // Bool for hand pieces
    public bool isHand { get; set; }

    // Bool to prevent gem selectoin while gem is moving
    public bool canSelect { get; set; }

    // particle effect for destruction
    [SerializeField]
    ParticleSystem particleSystem;
    Color color;
    #endregion

    #region Move
    // all variables needed for movement lerping
    float speed = GlobalVariables.MOVE_SPEED;
    Vector3 endPos = Vector3.zero;

    #endregion

    #region EventFields

    // Create delegate for locking gems
    public delegate void callMethod(GameObject go);
    public static event callMethod Selected;

    // Create delegate and event for playing sounds during the game
    public delegate void playSound();
    public static event playSound fireSoundEvent;

    // event for finishing coroutines
    public delegate void runNext();
    public static event runNext runNextMethod;

    //Check falling state event
    public delegate bool check();
    public static event check checkGems;

    #endregion

    #endregion

    #region Start

    private void Start()
    {
        // set initial state
        isSelected = false;
        canSelect = true;

        // get the color fo the particle effects based on gem tag
        switch(gameObject.tag)
        {
            case "White":
                color = Color.white;
                break;
            case "Red":
                color = Color.red;
                break;
            case "Green":
                color = Color.green;
                break;
            case "Purple":
                color = Color.magenta;
                break;
            case "Yellow":
                color = Color.yellow;
                break;
            case "Blue":
                color = Color.blue;
                break;
        }
    }


    #endregion

    #region Methods

    #region Select
    // When the Mouse clicks on a gem
    public void OnMouseDown()
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
    public void BlowUp()
    {
        fireSoundEvent();
        ParticleSystem ps = Instantiate<ParticleSystem>(particleSystem);
        ps.transform.position = transform.position;
        ParticleSystem.MainModule mm = ps.main;
        mm.startColor = color;
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
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
        while (Vector3.Distance(transform.position, endPos) > 0.1)
        {
            //float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed);
            yield return null;
        }
        // perfectly align gem
        transform.position = endPos;
        // No longer moving, can select gem
        canSelect = true;
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
        while (Vector3.Distance(transform.position, endPos) > 0.1)
        {
            //float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, 0.05f);
            yield return null;
        }
        // perfectly align gem
        transform.position = endPos;
        // No longer moving, can select gem
        canSelect = true;
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
