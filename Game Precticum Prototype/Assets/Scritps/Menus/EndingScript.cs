using UnityEngine;
using UnityEngine.UI;

public class EndingScript : MonoBehaviour {

    #region Fields

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
        finalScore.text = "Final Score: " + score;
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt("Highscore", highscore);
            highScoreText.text = "New High Score!" + "\n" + highscore;
        }
        else
        {
            highScoreText.text = "High Score: " + highscore;
        }
    }

    /// <summary>
    /// Set winning players score display and winner text
    /// </summary>
    /// <param name="winnerScore">score of winning player</param>
    /// <param name="loserScore">score of losing player</param>
    /// <param name="winner">whether player 1 is the winner</param>
    public void SetMultiplayerEnd(int winnerScore, int loserScore,bool winner)
    {
        finalScore.text = "Winners Score: " + winnerScore * 10;
        if (winner)
        {
            highScoreText.text = "You Won!" + "\n" + (winnerScore * 10) + " to : " + (loserScore * 10);
        }
        else
        {
            highScoreText.text = "You Lost, Too Bad" + "\n" + (loserScore * 10) + " to : " + (winnerScore * 10);
        }
        
    }

    #endregion
}
