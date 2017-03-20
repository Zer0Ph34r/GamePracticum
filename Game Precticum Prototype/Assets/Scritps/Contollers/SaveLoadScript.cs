using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadScript
{
    #region Fields
    public static List<SaveData> savedGames = new List<SaveData>();
    #endregion

    #region Save
    public static void Save()
    {
        // add saved data to list of saves
        savedGames.Add(SaveData.current);
        // create binary formatter for converting data to binary
        BinaryFormatter bf = new BinaryFormatter();
        // open file stream to a given file location
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        // save data in binary to prevent users form messing with data
        bf.Serialize(file, SaveLoadScript.savedGames);
        // close file location to prevent leaks and errors
        file.Close();
    }
    #endregion

    #region Load
    public static void Load()
    {
        // Check if file exists
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            // create binary formatter for reading data
            BinaryFormatter bf = new BinaryFormatter();
            // open file stream at file location based on saved data path
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            // COnvert saved data from binary to actual data
            SaveLoadScript.savedGames = (List<SaveData>)bf.Deserialize(file);
            // close file path to prevent leaks and errors
            file.Close();
        }
    }
    #endregion
}


#region Serializable Save Data Class
[System.Serializable]
public class SaveData
{
    #region Fields
    // NOTE: All fields are public for serialization, non-Public fields cannot be serialized

        // reference to this save data class
    public static SaveData current;

    // Name of this game
    public string gameName;

    // Save the lists of all gems in play at the moment
    public List<SyncGem> playerGems;
    public List<SyncGem> opponantGems;
    public List<SyncGem> playerHand;
    public List<SyncGem> opponantHand;

    // save both players current score
    public int playerScore;
    public int opponantScore;

    // is player the server or client (0 == server, 1 == client)
    public int isClient;

    // save current turn count
    public int turns;
    #endregion

    #region Constructor

    /// <summary>
    /// Set data to be saved
    /// </summary>
    /// <param name="PlayerBoard">current state of player's board</param>
    /// <param name="OpponantBoard">current state of opponants board</param>
    /// <param name="PlayerHand"> current state of players hand</param>
    /// <param name="OpponantHand">current stae of opponants hand</param>
    /// <param name="PlayerScore">current player score</param>
    /// <param name="OpponantScore">current opponant score</param>
    /// <param name="Turns">current turn count</param>
    public SaveData(string GameName, List<SyncGem> PlayerBoard, List<SyncGem> OpponantBoard, List<SyncGem> PlayerHand, List<SyncGem> OpponantHand,
        int PlayerScore, int OpponantScore, int IsClient, int Turns)
    {
        // Set all data to be saved
        gameName = GameName;  // Capitalized values are submitted through code
        playerGems = PlayerBoard; 
        opponantGems = OpponantBoard;
        playerHand = PlayerHand;
        opponantHand = OpponantHand;
        playerScore = PlayerScore;
        opponantScore = OpponantScore;
        isClient = IsClient;
        turns = Turns;

    }

    #endregion
}

#endregion

