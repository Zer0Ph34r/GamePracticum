using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasControllerScript : MonoBehaviour {

	#region Fields

	//Fields for saving UI art objects
	Object buttonPrefab;

	// Grid of invisible UI buttons for selecting gems
	Button[, ] buttons;
	int tableSize = GlobalVariables.TABLE_SIZE;

	// Reference to Main Camera
	Camera mainCamera;

	#endregion

	// Use this for initialization
	void Start () {

		// create array of buttons for table selection
		buttons = new Button [tableSize, tableSize];

		// Save reference to main camera
		mainCamera = Camera.main;

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
				buttons[i, k] = (Button)Instantiate(buttonPrefab, mainCamera.WorldToScreenPoint(new Vector3(i, k, 0)), Quaternion.identity);
			}
		}
	}
}
