using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.SceneManagement;
using Byn.Net;
using System.Collections.Generic;
using Byn.Common;

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
    #region Fields

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

    #region Custom Message Fields

    public string uSignalingUrl = "wss://because-why-not.com:12777/chatapp";
    public string uStunServer = "stun:stun.l.google.com:19302";
    public bool uLog = false;
    public bool uDebugConsole = false;
    private IBasicNetwork mNetwork = null;
    private bool mIsServer = false;
    private List<ConnectionId> mConnections = new List<ConnectionId>();

    #endregion

    #region Misc.

    // instance of this object
    public static NetworkScript instance;

    // player object using this script
    [SerializeField]
    GameObject playerInstance;

    // bool for switching between client and server objects
    public bool isClient = true;
    public bool canSend { get; set; }

    TimerScript timer = new TimerScript(1);

    bool started = true;
    bool connected = false;

    #endregion

    #endregion

    #region Awake
    private void Awake()
    {
        #region Custom Message Start

        //shows the console on all platforms. for debugging only
        if (uDebugConsole)
            DebugHelper.ActivateConsole();
        if (uLog)
            SLog.SetLogger(OnLog);

        SLog.LV("Verbose log is active!");
        SLog.LD("Debug mode is active");

        //Debug.Log("Setting up WebRtcNetworkFactory");
        WebRtcNetworkFactory factory = WebRtcNetworkFactory.Instance;
        if (factory != null)
            //Debug.Log("WebRtcNetworkFactory created");

        #endregion

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
            //NetworkServer.RegisterHandler(GemMsg.boardMessage, OnBoardMessageReceived);
            //NetworkServer.RegisterHandler(GemMsg.handMessage, OnHandMessageReceived);
            //NetworkServer.RegisterHandler(GemMsg.infoMessage, OnInfoMessageReceived);
        }
    }

    #endregion

    #region Update Message Class

    /// <summary>
    /// Fixed Update is always called a certain number of times each second
    /// </summary>
    private void FixedUpdate()
    {
        //check each fixed update if we have got new events
        HandleNetwork();

        if (isClient &&
            started &&
            connected)
        {
            Debug.Log("client sent info");
            started = false;
            ClientConnected();
        }
        else if (started &&
            connected)
        {
            Debug.Log("Server sent info");
            started = false;
            ServerConnected();
        }

    }

    /// <summary>
    /// Handles all networking componants
    /// </summary>
    private void HandleNetwork()
    {
        //check if the network was created
        if (mNetwork != null)
        {
            //first update it to read the data from the underlaying network system
            mNetwork.Update();

            //handle all new events that happened since the last update
            NetworkEvent evt;
            //check for new messages and keep checking if mNetwork is available. it might get destroyed
            //due to an event
            while (mNetwork != null && mNetwork.Dequeue(out evt))
            {
                //print to the console for debugging
                Debug.Log(evt);

                //check every message
                switch (evt.Type)
                {
                    case NetEventType.ServerInitialized:
                        {
                            //server initialized message received
                            //this is the reaction to StartServer -> switch GUI mode
                            mIsServer = true;
                            string address = evt.Info;
                            Debug.Log("Server started. Address: " + address);
                        }
                        break;
                    case NetEventType.ServerInitFailed:
                        {
                            //user tried to start the server but it failed
                            //maybe the user is offline or signaling server down?
                            mIsServer = false;
                            Debug.Log("Server start failed.");
                            Reset();
                        }
                        break;
                    case NetEventType.ServerClosed:
                        {
                            //server shut down. reaction to "Shutdown" call or
                            //StopServer or the connection broke down
                            mIsServer = false;
                            Debug.Log("Server closed. No incoming connections possible until restart.");
                        }
                        break;
                    case NetEventType.NewConnection:
                        {
                            // Add connection to connection ID
                            mConnections.Add(evt.ConnectionId);
                            connected = true;
                        }
                        break;
                    case NetEventType.ConnectionFailed:
                        {
                            //Outgoing connection failed. Inform the user.
                            Debug.Log("Connection failed");
                            started = true;
                            Reset();
                        }
                        break;
                    case NetEventType.Disconnected:
                        {
                            mConnections.Remove(evt.ConnectionId);
                            //A connection was disconnected
                            //If this was the client then he was disconnected from the server
                            if (mIsServer == false)
                            {
                                Reset();
                            }
                        }
                        break;
                    case NetEventType.ReliableMessageReceived:
                    case NetEventType.UnreliableMessageReceived:
                        {
                            HandleIncommingMessage(ref evt);
                        }
                        break;
                }
            }

            //finish this update by flushing the messages out if the network wasn't destroyed during update
            if (mNetwork != null)
                mNetwork.Flush();
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
    public void SendInfo(int xPos, int yPos, short color, InfoType type)
    {
        // check if the message should be sent over from the server or from the client
        if (isClient)
        {
            // send massage based on message type to server
            switch (type)
            {
                case InfoType.board:
                    //gemMessenger.SendGemBoardInfo(client, xPos, yPos, color);
                    Debug.Log("Board Piece Sent");
                    SendMessageBoard("0" + xPos + yPos + color, true);
                    break;
                case InfoType.hand:
                    //gemMessenger.SendGemHandInfo(client, xPos, yPos, color);
                    Debug.Log("Hand Piece Sent");
                    SendMessageHand("1" + xPos + color, true);
                    break;
                case InfoType.info:
                    //gemMessenger.SendGameInfo(client, xPos, (short)yPos);
                    Debug.Log("Info Sent");
                    SendMessageInfo("2" + yPos + xPos, true);
                    break;
            }
        }
        else
        {
            // send message based on message type to client
            switch (type)
            {
                case InfoType.board:
                    //gemMessenger.SendBoardInfoServer(xPos, yPos, color);
                    Debug.Log("Board Piece Sent");
                    SendMessageBoard("0" + xPos + yPos + color, true);
                    break;
                case InfoType.hand:
                    //gemMessenger.SendHandInfoServer(xPos, yPos, color);
                    Debug.Log("Hand Piece Sent");
                    SendMessageHand("1" + xPos + color, true);
                    break;
                case InfoType.info:
                    //gemMessenger.SendGameInfoServer(xPos, (short)yPos);
                    Debug.Log("Info Sent");
                    SendMessageInfo("2" + yPos + xPos, true);
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

    public void ClientConnected()
    {
        // call base OnClientConnect method
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

    public void ServerConnected()
    {
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
        // Send user to main menu
        SceneManager.LoadScene("Main Menu");
    }

    #endregion

    #region On Board Message Recieve

    /// <summary>
    /// Method called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    void OnBoardMessageReceived(string /*NetworkMessage*/ netMsg)
    {

        // Exctract data from message
        //GemMessage receivedMsg = netMsg.ReadMessage<GemMessage>();
        Debug.Log("Board Piece Recieved");

        // get info from message
        int x = Int32.Parse("" + netMsg[1]);//receivedMsg.xPos;
        int y = Int32.Parse("" + netMsg[2]);//receivedMsg.yPos;
        int color = Int32.Parse("" + netMsg[3]);//receivedMsg.gemColor;

        // set piece in opponant board
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantBoard(x, y, color);
    }

    #endregion

    #region On Hand Message Recieved

    /// <summary>
    /// Method called when message is recieved
    /// </summary>
    /// <param name="netMsg"></param>
    void OnHandMessageReceived(string /*NetworkMessage*/ netMsg)
    {
        // Exctract data from message
        //GemMessage receivedMsg = netMsg.ReadMessage<GemMessage>();

        // get info from message
        int x = Int32.Parse("" + netMsg[1]);//receivedMsg.xPos;
        int y = 11;//Convert.ToInt16(netMsg[2]);//receivedMsg.yPos;
        int color = Int32.Parse("" + netMsg[2]);//receivedMsg.gemColor;

        // set piece in opponant board
        playerInstance.GetComponent<NetworkPlayerScript>().SetOpponantHand(x, y, color);
    }

    #endregion

    #region On Info Message Recieved

    public void OnInfoMessageReceived(string /*NetworkMessage*/ netMsg)
    {
        string scoreString = "";
        // Read in received message
        //InfoMessage receivedMsg = netMsg.ReadMessage<InfoMessage>();

        // Set info to temp variables
        int turnCount = Int32.Parse("" + netMsg[1]);//receivedMsg.turnCount;
        // Get all characters after turn count
        for (int i = 2; i < netMsg.Length; i++)
        {
            scoreString += netMsg[i];
        }

        int score = Int32.Parse(scoreString); //receivedMsg.score;

        // Set opponants score value
        playerInstance.GetComponent<NetworkPlayerScript>().SetInfo(turnCount, score);

    }

    #endregion

    #region Server Client Connection

    #region SetUpServer

    // Create a server and listen on a port
    public void SetupServer()
    {

        //NetworkServer.Listen(7777);
        //isAtStartup = false;

        // Custom Message start
        Setup();
        EnsureLength();
        mNetwork.StartServer("PowerSwapRoom");

    }
    #endregion

    #region SetUpClient

    // Create a client and connect to the server port
    public void SetupClient()
    {
        //myClient = new NetworkClient();
        //myClient.RegisterHandler(MsgType.Connect, OnConnected);
        //myClient.Connect("127.0.0.1", 7777);
        //isAtStartup = false;

        // Custom Messaging Connect
        Setup();
        mNetwork.Connect("PowerSwapRoom");
        //Debug.Log("Connecting to " + "PowerSwapRoom" + " ...");

    }

    #endregion

    #endregion

    #region On Destroy

    private void OnDestroy()
    {
        //TimerScript.timerEnded -= SetupServer;
        //TimerScript.timerEnded -= SetupClient;

        if (mNetwork != null)
        {
            Cleanup();
        }
    }

    #endregion

    #region Custom Message Client

    /// <summary>
    /// Create instance of the server
    /// </summary>
    private void Setup()
    {
        //Debug.Log("Initializing webrtc network");

        mNetwork = WebRtcNetworkFactory.Instance.CreateDefault(uSignalingUrl, new IceServer[] { new IceServer(uStunServer) });
        if (mNetwork != null)
        {
            Debug.Log("WebRTCNetwork created");
        }
        else
        {
            Debug.Log("Failed to access webrtc ");
        }
    }

    private void EnsureLength()
    {
        //if (uRoomName.text.Length > MAX_CODE_LENGTH)
        //{
        //    uRoomName.text = uRoomName.text.Substring(0, MAX_CODE_LENGTH);
        //}
    }

    private void Reset()
    {
        Debug.Log("Reset Cleanup!");

        mIsServer = false;
        mConnections = new List<ConnectionId>();
        Cleanup();
    }

    private void Cleanup()
    {
        mNetwork.Dispose();
        mNetwork = null;
    }

    private void OnLog(object msg, string[] tags)
    {
        StringBuilder builder = new StringBuilder();
        TimeSpan time = DateTime.Now - DateTime.Today;
        builder.Append(time);
        builder.Append("[");
        for (int i = 0; i < tags.Length; i++)
        {
            if (i != 0)
                builder.Append(",");
            builder.Append(tags[i]);
        }
        builder.Append("]");
        builder.Append(msg);
    }

    #endregion

    #region Send Message

    /// <summary>
    /// Sends Data to all other players in given room
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="reliable"></param>
    private void SendMessageInfo(string msg, bool reliable = true)
    {
        if (mNetwork == null || mConnections.Count == 0)
        {
            Debug.Log("No connection. Can't send message.");
        }
        else
        {
            byte[] msgData = Encoding.UTF8.GetBytes(msg);
            foreach (ConnectionId id in mConnections)
            {
                mNetwork.SendData(id, msgData, 0, msgData.Length, reliable);
            }
        }
    }

    /// <summary>
    /// Sends Data to all other players in given room
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="reliable"></param>
    private void SendMessageBoard(string msg, bool reliable = true)
    {
        if (mNetwork == null || mConnections.Count == 0)
        {
            Debug.Log("No connection. Can't send message.");
        }
        else
        {
            byte[] msgData = Encoding.UTF8.GetBytes(msg);
            foreach (ConnectionId id in mConnections)
            {
                mNetwork.SendData(id, msgData, 0, msgData.Length, reliable);
            }
        }
    }

    /// <summary>
    /// Sends Data to all other players in given room
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="reliable"></param>
    private void SendMessageHand(string msg, bool reliable = true)
    {
        if (mNetwork == null || mConnections.Count == 0)
        {
            Debug.Log("No connection. Can't send message.");
        }
        else
        {
            byte[] msgData = Encoding.UTF8.GetBytes(msg);
            foreach (ConnectionId id in mConnections)
            {
                mNetwork.SendData(id, msgData, 0, msgData.Length, reliable);
            }
        }
    }

    #endregion

    #region Handle Messages

    private void HandleIncommingMessage(ref NetworkEvent evt)
    {
        MessageDataBuffer buffer = (MessageDataBuffer)evt.MessageData;

        string msg = Encoding.UTF8.GetString(buffer.Buffer, 0, buffer.ContentLength);
        
        //Debug.Log("Message Recieved" + msg);

        //if server -> forward the message to everyone else including the sender
        //if (mIsServer)
        //{
        //we use the server side connection id to identify the client
        string idAndMessage = evt.ConnectionId + ":" + msg;
        if (msg[0] == '0')
        {
            //Debug.Log("Board Recieved" + msg);
            OnBoardMessageReceived(msg);
        }
        else if (msg[0] == '1')
        {
            //Debug.Log("Hand Recieved" + msg);
            OnHandMessageReceived(msg);
        }
        else if (msg[0] == '2')
        {
            //Debug.Log("Info Recieved" + msg);
            OnInfoMessageReceived(msg);
        }
        buffer.Dispose();
    }

    #endregion
}
