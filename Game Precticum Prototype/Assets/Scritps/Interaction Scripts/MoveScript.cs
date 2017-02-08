using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript {

    #region Fields

    List<GameObject> chain = new List<GameObject>();

    #endregion

    #region Constructor



    #endregion

    #region Methods

    /// <summary>
    /// Returns the color of gem in chain
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public string GetColor(int i)
    {
        return chain[i].tag;
    }

    public void AddMove(GameObject go)
    {
        chain.Add(go);
    }

    public void AddMoves(MoveScript ms1, MoveScript ms2)
    {
        List<GameObject> newMove = new List<GameObject>();

        foreach (GameObject go in ms1.GetList)
        {
            newMove.Add(go);
        }
        foreach (GameObject go in ms2.GetList)
        {
            newMove.Add(go);
        }
        chain = newMove;
    }

    #endregion

    #region Properties

    public List<GameObject> GetList
    {
        get
        {
            return chain;
        }
    }

    #endregion

}
