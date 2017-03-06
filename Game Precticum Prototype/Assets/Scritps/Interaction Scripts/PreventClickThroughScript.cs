using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class FingerMove : MonoBehaviour,
        IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    #region Fields

    Vector3 prevPointWorldSpace;
    Vector3 thisPointWorldSpace;
    Vector3 realWorldTravel;
    Camera theCam;

    #endregion

    private void Start()
    {
        theCam = Camera.main;
    }

    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("FINGER DOWN");
        prevPointWorldSpace =
                theCam.ScreenToWorldPoint(data.position);
    }

    public void OnDrag(PointerEventData data)
    {
        thisPointWorldSpace =
               theCam.ScreenToWorldPoint(data.position);
        realWorldTravel =
               thisPointWorldSpace - prevPointWorldSpace;
        //_processRealWorldtravel();
        prevPointWorldSpace = thisPointWorldSpace;
    }

    public void OnPointerUp(PointerEventData data)
    {
        Debug.Log("clear finger...");
    }
}