using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public enum InfoType { board, hand, info};

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
    public void SendGemBoardInfo(NetworkClient messageClient, int xPos, int yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = (short)xPos;
        sentMsg.yPos = (short)yPos;
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
    public void SendBoardInfoServer(int xPos, int yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = (short)xPos;
        sentMsg.yPos = (short)yPos;
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
    public void SendGemHandInfo(NetworkClient messageClient, int xPos, int yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = (short)xPos;
        sentMsg.yPos = (short)yPos;
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
    public void SendHandInfoServer(int xPos, int yPos, short color)
    {
        // create a new GemMessage object
        GemMessage sentMsg = new GemMessage();

        // Set message data
        sentMsg.xPos = (short)xPos;
        sentMsg.yPos = (short)yPos;
        sentMsg.gemColor = color;

        //Debug.Log("Sent Data - X " + xPos + " Y " + yPos + " Color " + color);

        //  Send message data to server
        NetworkServer.SendToClient(1, GemMsg.handMessage, sentMsg);
    }

    #endregion

    #region Send Game Info

    public void SendGameInfo(NetworkClient messageClient, int score, short turns)
    {
        InfoMessage sentMsg = new InfoMessage();

        sentMsg.score = score;
        sentMsg.turnCount = turns;

        //  Send message data to server
        messageClient.Send(GemMsg.infoMessage, sentMsg);
    }

    public void SendGameInfoServer(int score, short turns)
    {
        InfoMessage sentMsg = new InfoMessage();

        sentMsg.score = score;
        sentMsg.turnCount = turns;

        //  Send message data to server
        NetworkServer.SendToClient(1, GemMsg.infoMessage, sentMsg);
    }

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
    public int score;

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

    public bool isAtStartup = true;

    NetworkClient myClient;

    #endregion

    // instance of this object
    public static NetworkScript instance;

    // player object using this script
    [SerializeField]
    GameObject playerInstance;

    // bool for switching between client and server objects
    public bool isClient = true;
    public bool canSend { get; set; }

    TimerScript timer = new TimerScript(1);

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
            TimerScript.timerEnded += SetupClient;
            // set client player to not have the current turn
            playerInstance.GetComponent<NetworkPlayerScript>().currTurn = false;
            // star client and connect to server
            //StartClient();
            SetupClient();
            // register all messages with their perspective methods

            //client.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);
            //client.RegisterHandler(GemMsg.handMessage, OnHandMessageReceived);
            //client.RegisterHandler(GemMsg.infoMessage, OnInfoMessageReceived);
        }
        else
        {
            TimerScript.timerEnded += SetupServer;
            // set server player to active turn
            playerInstance.GetComponent<NetworkPlayerScript>().currTurn = true;
            // start a server
            //StartServer();
            SetupServer();
            // register all methods for the different messages
            NetworkServer.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);
            NetworkServer.RegisterHandler(GemMsg.handMessage, OnHandMessageReceived);
            NetworkServer.RegisterHandler(GemMsg.infoMessage, OnInfoMessageReceived);
        }

        
    }
    #endregion

    private void Update()
    {
        timer.Update(Time.deltaTime);
    }

    #region Send Info
    /// <summary>
    /// Pass through method for sending data to other player with client info
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    /// <param name="color"></param>
    public void SendInfo(int xPos, int yPos, short color, InfoType type)
    {
        // check if the message should be sent over from the server or from the client
        if (isClient)
        {
            // send massage based on message type to server
            switch (type)
            {
                case InfoType.board:
                    gemMessenger.SendGemBoardInfo(client, xPos, yPos, color);
                    break;
                case InfoType.hand:
                    gemMessenger.SendGemHandInfo(client, xPos, yPos, color);
                    break;
                case InfoType.info:
                    gemMessenger.SendGameInfo(client, xPos, (short)yPos);
                    break;
            }
        }
        else
        {
            // send message based on message type to client
            switch (type)
            {
                case InfoType.board:
                    gemMessenger.SendBoardInfoServer(xPos, yPos, color);
                    break;
                case InfoType.hand:
                    gemMessenger.SendHandInfoServer(xPos, yPos, color);
                    break;
                case InfoType.info:
                    gemMessenger.SendGameInfoServer(xPos, (short)yPos);
                    break;
            }
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

    #region On Client Disconnect

    /// <summary>
    /// Method called when the client is disconnected from the server
    /// </summary>
    /// <param name="conn"></param>
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        // Call base opperations
        base.OnClientDisconnect(conn);
        // send debug message
        Debug.Log("This client has disconnected from the server.");
        // return user to main menu
        SceneManager.LoadScene("Main Menu");
    }

    #endregion

    #region On Server Disconnect

    /// <summary>
    /// Method is called when server disconnects from the client
    /// </summary>
    /// <param name="conn"></param>
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        // call base opperations
        base.OnServerDisconnect(conn);
        // send debug message
        Debug.Log("Client has disconnected from the server");
        // Send user to main menu
        SceneManager.LoadScene("Main Menu");
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
        int score = receivedMsg.score;

        // Set opponants score value
        playerInstance.GetComponent<NetworkPlayerScript>().SetInfo(turnCount, score);

    }

    #endregion

    #region SetUpServer

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(7777);
        isAtStartup = false;
    }
    #endregion

    #region Local Server
    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;
    }

    #endregion


    #region SetUpClient

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect("13.65.46.156", 7777);
        isAtStartup = false;

        client.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);
        client.RegisterHandler(GemMsg.handMessage, OnHandMessageReceived);
        client.RegisterHandler(GemMsg.infoMessage, OnInfoMessageReceived);
    }

    #endregion

    #region OnConnect

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }

    public override void OnStopClient()
    {
        Debug.Log("Re-connect");
        timer.ChangeTime(1);
        timer.StartTimer();
        
    }

    private void OnDestroy()
    {
        TimerScript.timerEnded -= SetupServer;
        TimerScript.timerEnded -= SetupClient;
    }

    #endregion
}
