using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControllerScript : MonoBehaviour {

	#region Fields

	//Fields for saving UI art objects
	Object buttonPrefab;

	// Grid of invisible UI buttons for selecting gems
	GameObject[, ] buttons;
	int tableSize = GlobalVariables.TABLE_SIZE;

	// Reference to Main Camera
//	Camera mainCamera;

	#endregion

	// Use this for initialization
	void Start () {

		// create array of buttons for table selection
		buttons = new GameObject [tableSize, tableSize];

		// Save reference to main camera
//		mainCamera = Camera.main;

		// Load Butotn Prefab
		buttonPrefab = Resources.Load("Prefabs/ButtonUI");

		//Creates all the buttons for ui
		CreateButtonOverlay();

	}
	
	/// <summary>
	/// Creates game board according to game board size
	/// </summary>
	void CreateButtonOverlay()
	{
		for (int i = 0; i < tableSize; ++i)
		{
			for (int k = 0; k < tableSize; ++k)
			{
				GameObject go = (GameObject)Instantiate(buttonPrefab, new Vector3(i, k, transform.position.z), Quaternion.identity);
				Parent (go);
				buttons [i, k] = go;
			}
		}
	}

	// Sets object as child of parent object
	void Parent(GameObject childOb )
	{
		childOb.transform.SetParent (transform);
	}
}
