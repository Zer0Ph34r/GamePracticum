using UnityEngine;
using UnityEngine.Networking;
using System;

#region Custom Gem Message Class

public class GemMessageAssistant
{

    #region Send Message Method
    public void SendGemInfo(NetworkClient messageClient, short xPos, short yPos, short color)
    {
        GemMessage msg = new GemMessage();
        msg.xPosition = xPos;
        msg.yPosition = yPos;
        msg.gemColor = color;

        messageClient.Send(GemMsg.messageType, msg);

    }

    #endregion

    #region On Change Method
    /// <summary>
    /// Called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnChange(NetworkMessage netMsg)
    {
        GemMessage msg = netMsg.ReadMessage<GemMessage>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<NetworkPlayerScript>().SetOpponantBoard(msg.xPosition, msg.yPosition, msg.gemColor);

    }
    #endregion

    #region On Connected Method
    /// <summary>
    /// Method is called when a connection is made witht the server
    /// </summary>
    /// <param name="netMsg"></param>
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
    #endregion

}

#endregion

#region Gem Message Class
public class GemMessage : MessageBase
{
    public short xPosition { get; set; }
    public short yPosition { get; set; }
    public short gemColor { get; set; }
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

    public static NetworkScript instance;

    [SerializeField]
    GameObject playerInstance;

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

    public void SendInfo(short xPos, short yPos, short color)
    {
        gemMessenger.SendGemInfo(client, xPos, yPos, color);
    }

    #region On Connected Client
    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("This client has connected to the server - NetConn: " + conn);
        canSend = true;
        playerInstance.GetComponent<NetworkPlayerScript>().SendBoard();
    }
    #endregion

    #region On Message Recieve

    // Method called when message is recieved
    void OnMessageReceive(NetworkMessage netMsg)
    {
        Debug.Log("Message Recieved - " + netMsg.ReadMessage<GemMessage>().xPosition + " " +
            netMsg.ReadMessage<GemMessage>().yPosition + " " + netMsg.ReadMessage<GemMessage>().gemColor);
        // get info from message
        short x = netMsg.ReadMessage<GemMessage>().xPosition;
        short y = netMsg.ReadMessage<GemMessage>().yPosition;
        short color = netMsg.ReadMessage<GemMessage>().gemColor;

        // set piece
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantBoard(x, y, color);
    }

    #endregion

}
