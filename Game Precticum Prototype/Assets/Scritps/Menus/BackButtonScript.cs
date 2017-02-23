using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonScript : MonoBehaviour {

    [SerializeField]
    Canvas Main;

    // Sound Effect for clicking on buttons
    AudioClip click;
    AudioSource audioSource;

    private void Start()
    {
        click = Resources.Load<AudioClip>("Sounds/Click");
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    ///  Turn on main canvas and turn this one off
    /// </summary>
    public void BackButton()
    {
        audioSource.PlayOneShot(click);
        Main.transform.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

}
