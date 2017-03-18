using UnityEngine;
using UnityEngine.Networking;
using System;

#region Custom Gem Message Class

public class GemMessageAssistant
{

    #region Send Board Info

    /// <summary>
    /// Send gem data to server
    /// </summary>
    /// <param name="messageClient"></param>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendGemBoardInfo(NetworkClient messageClient, short xPos, short yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = xPos;
        sentMsg.yPos = yPos;
        sentMsg.gemColor = color;

        //  Send message data to server
        messageClient.Send(GemMsg.boardMessage, sentMsg);
    }

    /// <summary>
    /// Send message from server to client
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendBoardInfoServer(short xPos, short yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = xPos;
        sentMsg.yPos = yPos;
        sentMsg.gemColor = color;

        //  Send message data to client
        NetworkServer.SendToClient(1, GemMsg.boardMessage, sentMsg);

    }

    #endregion

    #region Send Hand Info

    /// <summary>
    /// Send gem data to server
    /// </summary>
    /// <param name="messageClient"></param>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendGemHandInfo(NetworkClient messageClient, short xPos, short yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = xPos;
        sentMsg.yPos = yPos;
        sentMsg.gemColor = color;

        //  Send message data to server
        messageClient.Send(GemMsg.handMessage, sentMsg);
    }

    /// <summary>
    /// Send hand message to client from server
    /// </summary>
    /// <param name="messageClient"></param>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendHandInfoServer(short xPos, short yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = xPos;
        sentMsg.yPos = yPos;
        sentMsg.gemColor = color;

        //  Send message data to server
        NetworkServer.SendToClient(1, GemMsg.handMessage, sentMsg);
    }

    #endregion

    #region On Change Method
    /// <summary>
    /// Called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    //public void OnChange(NetworkMessage netMsg)
    //{
    //    GemMessage msg = netMsg.ReadMessage<GemMessage>();
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    player.GetComponent<NetworkPlayerScript>().SetOpponantBoard(msg.xPosition, msg.yPosition, msg.gemColor);

    //}
    #endregion

    #region On Connected Method
    /// <summary>
    /// Method is called when a connection is made witht the server
    /// </summary>
    /// <param name="netMsg"></param>
    //public void OnConnected(NetworkMessage netMsg)
    //{
    //    Debug.Log("Connected to server");
    //}
    #endregion

}

#endregion

#region Gem Message Class
// Container for all data to be sent over network
public class GemMessage : MessageBase
{
    #region Fields

    // all info to be sent over message
    public short xPos;
    public short yPos;
    public short gemColor;

    #endregion

    // Empty Constructor
    public GemMessage() { }

}

#endregion

#region Info Message Class

/// <summary>
/// Class for sending score and turn info
/// </summary>
public class InfoMessage : MessageBase
{
    #region Fields

    // turn count, score info 
    public short turnCount;
    public short score;

    #endregion

}

#endregion

#region Gem Message Type Class

public class GemMsg
{
    // Message types for sending baord messages and hand messages
    public static short boardMessage = 1000;
    public static short handMessage = 1100;
    public static short infoMessage = 1200;
}

#endregion

#region SyncGem Serializable Class
[Serializable]
public class SyncGem
{

    #region Fields
    // All info to be sent in message between clients
    public short xPos;//{ get; set; }
    public short yPos;//{ get; set; }
    public short colorEnum;//{ get; set; }
    #endregion

    #region Constructor
    /// <summary>
    /// Creates new instance of SyncGem with given values
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="color"></param>
    public SyncGem(short x, short y, short color)
    {
        xPos = x;
        yPos = y;
        colorEnum = color;
    }
    #endregion

}
#endregion

public class NetworkScript : NetworkManager
{

    #region Message Fields

    //const short MyBeginMsg = 1002;
    //Set up special class for sending and recieving gem messages

    GemMessageAssistant gemMessenger;

    NetworkClient m_client;
    int connection;
    uint myNetID;

    #endregion

    // instance of this object
    public static NetworkScript instance;

    // player object using this script
    [SerializeField]
    GameObject playerInstance;

    // bool for switching between client and server objects
    public bool isClient = true;
    public bool canSend { get; set; }

    #region Awake
    private void Awake()
    {
        //Save instance f this object
        if (instance == null)
        {
            instance = this;
        }

        // create gemMessenger that will send messages to server
        gemMessenger = new GemMessageAssistant();

        // if this is the client machine
        if (isClient)
        {
            StartClient();
            client.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);

        }
        else
        {
            // start a server
            StartServer();
            NetworkServer.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);
        }
    }
    #endregion

    #region Send Info
    /// <summary>
    /// Pass through method for sending data to other player with client info
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendInfo(short xPos, short yPos, short color, bool isClient)
    {
        if (isClient)
        {
            gemMessenger.SendGemBoardInfo(client, xPos, yPos, color);
        }
        else
        {
            gemMessenger.SendBoardInfoServer(xPos, yPos, color); 
        }
        
    }
    #endregion

    #region On Connected Client
    // when this client connects with the server
    public override void OnClientConnect(NetworkConnection conn)
    {
        // call base OnClientConnect method
        base.OnClientConnect(conn);
        canSend = true;
        // send client info to server to set opponant's board
        playerInstance.GetComponent<NetworkPlayerScript>().SendBoard();
        playerInstance.GetComponent<NetworkPlayerScript>().SendHand();
    }
    #endregion

    #region On Server Connect
    // When the server connects to a client
    public override void OnServerConnect(NetworkConnection conn)
    {
        // use base onserverconnect
        base.OnServerConnect(conn);
        connection = conn.connectionId;

        // Send board and hand info
        playerInstance.GetComponent<NetworkPlayerScript>().SendBoard();
        playerInstance.GetComponent<NetworkPlayerScript>().SendHand();

    }
    #endregion

    #region On Board Message Recieve

    /// <summary>
    /// Method called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    void OnBoardMessageReceived(NetworkMessage netMsg)
    {
        // Exctract data from message
        GemMessage receivedMsg = netMsg.ReadMessage<GemMessage>();

        // get info from message
        short x = receivedMsg.xPos;
        short y = receivedMsg.yPos;
        short color = receivedMsg.gemColor;

        // display sent data to console
        //Debug.Log("Message Data - X " + x + " Y " + y + " Color " + color);

        // set piece in opponant board
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantBoard(x, y, color);
    }

    #endregion

    #region On Hand Message Recieved

    /// <summary>
    /// Method called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    void OnHandMessageReceived(NetworkMessage netMsg)
    {
        // Exctract data from message
        GemMessage receivedMsg = netMsg.ReadMessage<GemMessage>();

        // get info from message
        short x = receivedMsg.xPos;
        short y = receivedMsg.yPos;
        short color = receivedMsg.gemColor;

        // set piece in opponant board
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantHand(x, y, color);
    }

    #endregion

    #region On Info Message Recieved

    public void OnInfoMessageReceived(NetworkMessage netMsg)
    {
        // Read in received message
        InfoMessage receivedMsg = netMsg.ReadMessage<InfoMessage>();

        // Set info to temp variables
        short turnCount = receivedMsg.turnCount;
        short score = receivedMsg.score;

    }

    #endregion

}
