using UnityEngine;
using UnityEngine.Networking;
using System;

#region Custom Gem Message Class

public class GemMessageAssistant
{

    #region Send Message Method
    /// <summary>
    /// Send gem data to server
    /// </summary>
    /// <param name="messageClient"></param>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendGemInfo(NetworkClient messageClient, short xPos, short yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.positionX = xPos;
        sentMsg.positionY = yPos;
        sentMsg.Color = color;

        //  Send message data to server
        messageClient.Send(GemMsg.messageType, sentMsg);
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
    short m_x = 5;
    short m_y = 5;
    short m_color = 5;

    #endregion

    // Empty Constructor
    public GemMessage() { }

    #region Properties

    #region X
    public short positionX
    {
        get
        {
            return m_x;
        }
        set
        {
            m_x = value;
        }
    }
    #endregion

    #region Y
    public short positionY
    {
        get
        {
            return m_y;
        }
        set
        {
            m_y = value;
        }
    }
    #endregion

    #region Color
    public short Color
    {
        get
        {
            return m_color;
        }
        set
        {
            m_color = value;
        }
    }
    #endregion

    #endregion
}

#endregion

#region Gem Message Type Class

public class GemMsg
{
    public static short messageType = 1000;
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
            client.RegisterHandler(GemMsg.messageType, OnMessageReceive);
        }
        else
        {
            // start a server
            StartServer();
            NetworkServer.RegisterHandler(GemMsg.messageType, OnMessageReceive);
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
    public void SendInfo(short xPos, short yPos, short color)
    {
        gemMessenger.SendGemInfo(client, xPos, yPos, color);
    }
    #endregion

    #region On Connected Client
    // when this client connects with the server
    public override void OnClientConnect(NetworkConnection conn)
    {
        // call base OnClientConnect method
        base.OnClientConnect(conn);
        Debug.Log("This client has connected to the server - NetConn: " + conn);
        canSend = true;
        // send client info to server to set opponant's board
        playerInstance.GetComponent<NetworkPlayerScript>().SendBoard();
    }
    #endregion

    #region On Message Recieve

    // Method called when message is recieved
    void OnMessageReceive(NetworkMessage netMsg)
    {
        // Exctract data from message
        GemMessage receivedMsg = netMsg.ReadMessage<GemMessage>();

        // get info from message
        short x = receivedMsg.positionX;
        short y = receivedMsg.positionY;
        short color = receivedMsg.Color;

        // display sent data to console
        Debug.Log("Message Data - X " + x + " Y " + y + " Color " + color);

        // set piece in opponant board
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantBoard(x, y, color);
    }

    #endregion

}
