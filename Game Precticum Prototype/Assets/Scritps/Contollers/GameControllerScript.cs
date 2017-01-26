using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    #region Feilds

    // Gem game objects
    GameObject whiteGem;
    GameObject redGem;
    GameObject yellowGem;
    GameObject greenGem;
    GameObject blueGem;
    GameObject purpleGem;

    // table size int X int
    int tableSize = GlobalVariables.TABLE_SIZE;

    #endregion

    // Use this for initialization
    void Start () {

        #region Load Assets
        whiteGem = Resources.Load<GameObject>("Prefabs/PryamidWhite");
        redGem = Resources.Load<GameObject>("Prefabs/PryamidRed");
        yellowGem = Resources.Load<GameObject>("Prefabs/PryamidYelow");
        greenGem = Resources.Load<GameObject>("Prefabs/PryamidGreen");
        blueGem = Resources.Load<GameObject>("Prefabs/PryamidBlue");
        purpleGem = Resources.Load<GameObject>("Prefabs/PryamidPurple");
        #endregion

        #region Create Game Board

        #endregion

    }

    // Update is called once per frame
    void Update () {
		
	}
}
