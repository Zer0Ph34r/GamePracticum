using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using UnityEngine.UI;

public class MatchmakerScript : MonoBehaviour
{
    #region Fields
    //bool for checking status 
    bool done; // Not Used

    // List of matches save dfor use in other methods
    List<MatchInfoSnapshot> matches;

    #region Serializable Text Entries
    // all text entries needee for setting up and joining custom matches
    [SerializeField]
    InputField matchNameField;
    [SerializeField]
    InputField passwordField;

    int screenSize = GlobalVariables.SCREEN_POSITION;

    #endregion

    #endregion

    #region Start
    // Use this for initialization
    void Start()
    {
        // start network matchmaker
        NetworkManager.singleton.StartMatchMaker();
        // list al availible matches
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, "Match", false, 0, 1, OnMatchList);

        // initialize lists
        matches = new List<MatchInfoSnapshot>();
    }
    #endregion

    #region On GUI

    private void OnGUI()
    {
        
    }

    #endregion

    #region On Match List

    /// <summary>
    /// Allows user to join a match that is already been created or create a new one if none exist
    /// </summary>
    /// <param name="success"></param>
    /// <param name="extendedInfo"></param>
    /// <param name="matchList"></param>
    public virtual void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        // if a list was created
        if (success)
        {
            // if there is one or more matches in the list
            if (matchList.Count != 0)
            {
                // Join the match
                NetworkManager.singleton.matchMaker.JoinMatch(matchList[0].networkId, "", "", "", 0, 1, OnMatchJoined);
            }
            else
            {
                // create a new match
                NetworkManager.singleton.matchMaker.CreateMatch("Match", 2, true, "", "", "", 0, 1, OnMatchCreate);
            }
        }
        else
        {
            Debug.Log("ERROR : Match Search Failure");
        }
    }
    #endregion

    #region Join Match

    public void JoinMatch()
    {
        if (matchNameField.text == "")
        {
            NetworkManager.singleton.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 1, OnMatchJoined);
        }
        else
        {
            NetworkManager.singleton.matchMaker.JoinMatch(matches[0].networkId, "", "", "", 0, 1, OnMatchJoined);
        }
    }

    #endregion

    #region Create Match

    public void CreateMatch()
    {
        if (matchNameField.text == "" &&
            passwordField.text == "")
        {
            NetworkManager.singleton.matchMaker.CreateMatch("Match", 2, true, "", "", "", 0, 1, OnMatchCreate);
        }
        else
        {
            NetworkManager.singleton.matchMaker.CreateMatch(matchNameField.text, 2, true, matchNameField.text, "", "", 0, 1, OnMatchCreate);
        }
    }

    #endregion

    #region On Match Joined

    /// <summary>
    /// Joins a created match and creates a cliet to connect to the server
    /// </summary>
    /// <param name="success"></param>
    /// <param name="extendedInfo"></param>
    /// <param name="matchInfo"></param>
    public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        // if joining the match was successful
        if (success)
        {
            // set host info based on match info 
            MatchInfo hostInfo = matchInfo;
            // start the client with host info
            NetworkManager.singleton.StartClient(hostInfo);

            //OnConnect();
        }
        else
        {
            Debug.Log("ERROR : Match Join Failure");
        }

    }
    #endregion

    #region On Match Create

    /// <summary>
    /// Creates a new instance of a match and creates a server for it
    /// </summary>
    /// <param name="success"></param>
    /// <param name="extendedInfo"></param>
    /// <param name="matchInfo"></param>
    public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        // if a match was created
        if (success)
        {
            // set host info based on match info
            MatchInfo hostInfo = matchInfo;
            // start server
            NetworkServer.Listen(hostInfo, 9000);
            // start hosting a game
            NetworkManager.singleton.StartHost(hostInfo);

            //OnConnect();
        }
        else
        {
            Debug.Log("ERROR : Match Create Failure");
        }
    }
    #endregion
}
