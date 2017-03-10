using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LgOctEngine.CoreClasses;

public class NetManager : NetworkManager {
	public static List<StaffClient> staffClients = new List<StaffClient> ();
	public static List<TeamInfo> teams = new List<TeamInfo>();

	//temp storage for usernames
	public static List<string> usernames = new List<string>();

	// Use this for initialization
	void Start () {
		CONSTANTS.customDebug.Log ("NetManager:Start()");
		StartServer ();
		NetworkServer.RegisterHandler(MessageType.STAFF_LOGIN_MSG, OnServerReceiveMessage<Login>);
		NetworkServer.RegisterHandler(MessageType.MASTER_CLIENT_LOGIN, MasterClientConnect);
		NetworkServer.RegisterHandler(MessageType.REQ_STAFF_CLIENTS, SendStaffClientsToMaster);
		NetworkServer.RegisterHandler(MessageType.STAFF_CLIENT, OnServerReceiveMessage<StaffClientMessage>);
		NetworkServer.RegisterHandler(MessageType.TEAM_INFO, OnServerReceiveMessage<TeamInfoMessage>);
		NetworkServer.RegisterHandler(MessageType.GAME_RESULT, OnServerReceiveMessage<GameResult>);
		NetworkServer.RegisterHandler(MessageType.BRACKET_INFO, OnServerReceiveMessage<BracketInfo>);
		NetworkServer.RegisterHandler(MessageType.ASSIGNMENT, OnServerReceiveMessage<StaffInstruction>);
		NetworkServer.RegisterHandler(MessageType.REGISTER_MESSAGE, OnServerReceiveMessage<RegisterInformation>);
	}

	public static void OnServerSendLoginSuccess(int connectionId)
	{
		CONSTANTS.customDebug.Log ("Staff Client successfully connected at: " + connectionId);
		var msg = new JsonMessage<Acknowledgement>();
		
		Acknowledgement acknowledgement = LgJsonNode.Create<Acknowledgement>();
		acknowledgement.ack = MessageValue.LOGIN_SUCCESS;
		
		msg.message = acknowledgement.Serialize();
		NetworkServer.SendToClient(connectionId, MessageType.STAFF_LOGIN_MSG, msg);
	}
	
	public static void OnServerSendLoginFailure(int connectionId)
	{
		CONSTANTS.customDebug.Log ("Staff client fail to connect at: " + connectionId);
		var msg = new JsonMessage<Acknowledgement>();
		
		Acknowledgement acknowledgement = LgJsonNode.Create<Acknowledgement>();
		acknowledgement.ack = MessageValue.LOGIN_FAILURE;
		
		msg.message = acknowledgement.Serialize();
		NetworkServer.SendToClient(connectionId, MessageType.STAFF_LOGIN_MSG, msg);
	}

	public static void SendResultToMasterClient(int connectionId, string result, string name)
	{
		CONSTANTS.customDebug.Log ("Sending game results from " + connectionId + " to master client");
		var msg = new JsonMessage<GameResult> ();

		GameResult gameResult = LgJsonNode.Create<GameResult> ();
		gameResult.result = result;
		gameResult.connectionId = connectionId;
		gameResult.name = name;

		msg.message = gameResult.Serialize ();
		NetworkServer.SendToClient (CONSTANTS.masterClient, MessageType.GAME_RESULT, msg);
	}

	void SendStaffClientsToMaster(NetworkMessage netMsg)
	{
		CONSTANTS.customDebug.Log ("Sending list of staff to master client");
		foreach (StaffClient staffClient in NetManager.staffClients)
		{
			StaffClientMessage staff = LgJsonNode.Create<StaffClientMessage>();
			staff.username = staffClient.username;
			staff.connectionId = staffClient.connectionId;

			JsonMessage<StaffClientMessage> msg = new JsonMessage<StaffClientMessage>();
			msg.message = staff.Serialize ();
			NetworkServer.SendToClient (CONSTANTS.masterClient, MessageType.REQ_STAFF_CLIENTS, msg);
		}
	}

	void OnServerReceiveMessage<T>(NetworkMessage netMsg) where T : LgJsonDictionary, IJsonable, new()
	{
		JsonMessage<T> jsonMessage = netMsg.ReadMessage<JsonMessage<T>>();
		CONSTANTS.customDebug.Log ("OnServerReceiveMessage: Received Message " + jsonMessage.message);
		T obj = LgJsonNode.CreateFromJsonString<T>(jsonMessage.message);
		obj.HandleNewObject(netMsg.conn.connectionId);
	}

	public override void OnServerConnect(NetworkConnection conn)
	{
		base.OnServerConnect(conn);
		CONSTANTS.customDebug.Log ("Client Connected: " + conn.connectionId);
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		for (int i = NetManager.staffClients.Count - 1; i >= 0; i--)
		{
			if (NetManager.staffClients[i].connectionId == conn.connectionId)
			{
				NetManager.staffClients.RemoveAt (i);
				CONSTANTS.customDebug.Log ("Removed staff client at connection: " + conn.connectionId);
				return;
			}
		}

		if (CONSTANTS.masterClient == conn.connectionId) {
			CONSTANTS.customDebug.Log ("Master client has disconnected");
			return;
		}

		CONSTANTS.customDebug.Log ("Client disconnected at connection: " + conn.connectionId);
	}

	void MasterClientConnect(NetworkMessage netMsg)
	{
		CONSTANTS.customDebug.Log("Master client has connected on: " + netMsg.conn.connectionId);
		CONSTANTS.masterClient = netMsg.conn.connectionId;
	}
}

public class StaffClient
{
	public string username;
	public int connectionId;

	public StaffClient(string username, int connectionId)
	{
		this.username = username;
		this.connectionId = connectionId;
	}
}

public class TeamInfo
{
	string teamName, player1, player2, player3;

	public TeamInfo(string teamName, string player1, string player2, string player3)
	{
		this.teamName = teamName;
		this.player1 = player1;
		this.player2 = player2;
		this.player3 = player3;
	}
}
