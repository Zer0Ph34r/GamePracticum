﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GemScript : MonoBehaviour
{

    #region Fields

    // bool for being selected
    public bool isSelected { get; set; }
    public bool isHand { get; set; }

    // particle effect for destruction
    [SerializeField]
    ParticleSystem particles;
   
    #region lerp
    // all variables needed for movement lerping
    float speed = GlobalVariables.LERP_SPEED;
    Vector3 endPos = Vector3.zero;

    #endregion

    #region EventFields

    // Create delegate for adding in method calls
    public delegate void callMethod(GameObject go);
    // create event for calling that delegate
    public static event callMethod Selected;

    // event for finishing coroutines
    public delegate void runNext();
    public static event runNext runNextMethod;


    #endregion

    #endregion

    private void Start()
    {
        // set initial state
        isSelected = false;

    }

    #region Methods

    // When the Mouse clicks on a gem
    public void OnMouseDown()
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

    /// <summary>
    /// Reset a Piece to how it began it's life
    /// </summary>
    public void Reset()
    {
        // reset piece to starting conditions
        isSelected = false;
        transform.GetChild(0).gameObject.SetActive(false);

    }

    public void BlowUp()
    {
        ParticleSystem.EmissionModule em = particles.emission;
        em.enabled = true;
        Destroy(gameObject);
    }

    #endregion

    #region Co-Routines

    /// <summary>
    /// Coroutine for moving gems to different positions
    /// </summary>
    /// <returns></returns>
    IEnumerator LerpPosition()
    {
        // loops for learping between positions
        while (Vector3.Distance(transform.position, endPos) > 0.1)
        {
            //float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, endPos, speed);
            yield return null;
        }
        // perfectly align gem
        transform.position = endPos;
        //Fire Event after coroutine ends
        if (runNextMethod != null)
        {
            runNextMethod();
        }
        
        
    }

    /// <summary>
    /// public method for calling coroutine
    /// </summary>
    /// <param name="endPos"></param>
    public void RunMotion(Vector3 endPos)
    {
        this.endPos = endPos;
        StartCoroutine(LerpPosition());
    }

    #endregion
}
