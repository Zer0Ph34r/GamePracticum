using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingScript : MonoBehaviour {

    #region fields

    // highscore field
    int highscore = GlobalVariables.HIGHSCORE;

    // reference to Text boxes
    [SerializeField]
    Text finalScore;
    [SerializeField]
    Text highScoreText;

    #endregion

    #region Methods

    /// <summary>
    /// sets score display
    /// </summary>
    /// <param name="score"></param>
    public void setEnd(int score)
    {
        finalScore.text = "Final Score: " + score * 10;
        if (score > highscore)
        {
            highscore = score;
            highScoreText.text = "New High Score!" + "\n" + highscore * 10;
        }
        else
        {
            highScoreText.text = "High Score: " + highscore * 10;
        }
    }

    /// <summary>
    /// Set winning players score display and winner text
    /// </summary>
    /// <param name="score"></param>
    /// <param name="winner"></param>
    public void SetMultiplayerEnd(int score, bool winner)
    {
        finalScore.text = "Winners Score: " + score * 10;
        if (winner)
        {
            highScoreText.text = "Player 1 Wins!";
        }
        else
        {
            highScoreText.text = "Player 2 Wins";
        }
        
    }

    #endregion
}
