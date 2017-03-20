using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerLoadScript : MonoBehaviour {

    // Create a list of all saved games player has participated in
    public void OnGUI()
    {
        // foreach savedata file present, create a button for loading that info in the next scene
        foreach (SaveData g in SaveLoadScript.savedGames)
        {
            // if the button is pressed, load that data
            if (GUILayout.Button("- " + g.playerScore + " -"))
            {
                // set current save data to selected data
                SaveData.current = g;

            }
        }
    }
}
