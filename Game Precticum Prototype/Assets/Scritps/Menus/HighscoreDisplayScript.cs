using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplayScript : MonoBehaviour {

    #region Fields

    [SerializeField]
    Text highscoreText;
    [SerializeField]
    Text TableSizeText;

    int tableSize = GlobalVariables.TABLE_SIZE;

    // string fields
    string table;
    string highscores = "";

    #endregion

    // Use this for initialization
    void Start () {

        table = tableSize + "X" + tableSize;

        for (int i = 0; i < 10; i++)
        {
            highscores += (i * 10) + " Turns:   " + GlobalVariables.HIGHSCORE_TABLE[tableSize - 5, i] + "\n";
        }

        // set text areas
        highscoreText.text = highscores;
        TableSizeText.text = table;
		
	}
	
}
