using UnityEngine;
using UnityEngine.UI;

public class HighscoreDisplayScript : MonoBehaviour {

    #region Fields

    [SerializeField]
    Text highscoreText;
    [SerializeField]
    Text TableSizeText;

    int tableSize;

    // string fields
    string table;
    string highscores = "";

    #endregion

    #region On Enable
    private void OnEnable()
    {
        tableSize = GlobalVariables.TABLE_SIZE;
        table = tableSize + "X" + tableSize;
        highscores = "";

        for (int i = 1; i <= 10; i++)
        {
            highscores += (i * 10) + " Turns:   " + GlobalVariables.HIGHSCORE_TABLE[tableSize - 5, i - 1] + "\n";
        }

        // set text areas
        highscoreText.text = highscores;
        TableSizeText.text = table;
    }
    #endregion
}
