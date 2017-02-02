using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSelectionScript : MonoBehaviour {

	// selects gem at point relative to button
	public void Clicked()
	{
		// finds game controller, and passes through to the gem at selected position and selects that gem
		FindObjectOfType<GameControllerScript> ().gems [(int)transform.position.x, (int)transform.position.y].GetComponent<GemScript> ().SelectGem ();
	}
}
