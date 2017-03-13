using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript {

    #region Fields

    List<GameObject> chain = new List<GameObject>();

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

    /// <summary>
    /// Adds a game object to list
    /// </summary>
    /// <param name="go"></param>
    public void AddMove(GameObject go)
    {
        chain.Add(go);
    }

    /// <summary>
    /// Adds all game objects in both moves to a single move that is then returned
    /// </summary>
    /// <param name="ms1"></param>
    /// <param name="ms2"></param>
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

    /// <summary>
    /// returns one object from chain at specified location
    /// If location doesn't exist, return null
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public GameObject returnPiece(int i)
    {
        if (i < chain.Count)
        {
            return chain[i];
        }
        else
        {
            return null;
        }
        
    }

    /// <summary>
    /// Check if all objects in the chain have the same xPosition
    /// </summary>
    /// <returns></returns>
    public bool sameX()
    {

        if (chain[0].transform.position.x == chain[1].transform.position.x &&
            chain[1].transform.position.x == chain[2].transform.position.x)
        {
            return true;
        }
        else
        {
            return false;
        }
        
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
