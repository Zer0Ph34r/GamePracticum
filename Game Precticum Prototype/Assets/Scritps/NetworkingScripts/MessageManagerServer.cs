using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using LgOctEngine.CoreClasses;

public static class MessageType
{
	public static short STAFF_LOGIN_MSG = 1000;
	public static short MASTER_CLIENT_LOGIN = 1001;
	public static short USER_LOGIN_MSG = 1002;
	public static short REGISTER_MESSAGE = 1003;
	public static short STAFF_CLIENT = 1010;
	public static short REQ_STAFF_CLIENTS = 1011;
	public static short TEAM_INFO = 1020;
	public static short BRACKET_INFO = 1021;
	public static short GAME_RESULT = 1030;
	public static short ASSIGNMENT = 1040;
	public static short ERROR = 1050;
}

public static class MessageValue
{
	public static short LOGIN_FAILURE = 0;
	public static short LOGIN_SUCCESS = 1;
}

// Use messages of this type to send ANY JSON formatted message
// Once you pull the string from this object, you can then
// decode it based on the message type

interface IJsonable
{
	void HandleNewObject(int connectionId);
}

public class JsonMessage<T> : MessageBase
{
	public string message;
}

public class Login : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public string password { get { return GetValue<string>("password", ""); } set { SetValue<string>("password", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.customDebug.Log("Handling Login");
		if (password == CONSTANTS.password) {
			NetManager.OnServerSendLoginSuccess (connectionId);
			NetManager.staffClients.Add (new StaffClient(username, connectionId));
		} else {
			NetManager.OnServerSendLoginFailure(connectionId);
		}
	}
}

public class StaffClientMessage : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }

	public void HandleNewObject(int conn)
	{
		NetManager.staffClients.Add (new StaffClient (username, connectionId));
	}
}

public class TeamInfoMessage : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string>("teamName", ""); } set { SetValue<string>("teamName", value); } }
	public string player1 { get { return GetValue<string>("player1", ""); } set { SetValue<string>("player1", value); } }
	public string player2 { get { return GetValue<string>("player2", ""); } set { SetValue<string>("player2", value); } }
	public string player3 { get { return GetValue<string>("player3", ""); } set { SetValue<string>("player3", value); } }

	public void HandleNewObject(int connectionId)
	{
		NetManager.teams.Add (new TeamInfo (teamName, player1, player2, player3));
	}
}

public class GameResult : LgJsonDictionary, IJsonable
{
	public string result { get { return GetValue<string> ("result", ""); } set { SetValue<string> ("result", value); } }
	public string name { get { return GetValue<string> ("name", ""); } set { SetValue<string> ("name", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }

	public void HandleNewObject(int connectionId)
	{
		NetManager.SendResultToMasterClient(connectionId, result, name);
	}
}

public class BracketInfo : LgJsonDictionary, IJsonable
{
	public int numberOfTeams { get { return GetValue<int> ("numberOfTeams", 0); } set { SetValue<int> ("numberOfTeams", value); } }
	public int firstRowCount { get { return GetValue<int> ("firstRowCount", 0); } set { SetValue<int> ("firstRowCount", value); } }
	public int leftOvers { get { return GetValue<int> ("leftOvers", 0); } set { SetValue<int> ("leftOvers", value); } }
	public int tiers { get { return GetValue<int> ("tiers", 0); } set { SetValue<int> ("tiers", value); } }

	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.bracketInfo = this.Serialize ();
		CONSTANTS.eventStarted = true;
	}
}

public class StaffInstruction : LgJsonDictionary, IJsonable
{
	public string homeTeam { get { return GetValue<string> ("homeTeam", ""); } set { SetValue<string> ("homeTeam", value); } }
	public string awayTeam { get { return GetValue<string> ("awayTeam", ""); } set { SetValue<string> ("awayTeam", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	public int fieldNum { get { return GetValue<int> ("fieldNum", 0); } set { SetValue<int> ("fieldNum", value); } }
	
	public void HandleNewObject(int conn)
	{
		JsonMessage<StaffInstruction> msg = new JsonMessage<StaffInstruction> ();
		msg.message = this.Serialize ();
		NetworkServer.SendToClient (connectionId, MessageType.ASSIGNMENT, msg);
	}
}

public class RegisterInformation : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }
	public string firstName { get { return GetValue<string> ("firstName", ""); } set { SetValue<string> ("firstName", value); } }
	public string lastName { get { return GetValue<string> ("lastName", ""); } set { SetValue<string> ("lastName", value); } }
	public string email { get { return GetValue<string> ("email", ""); } set { SetValue<string> ("email", value); } }
	public string password { get { return GetValue<string> ("password", ""); } set { SetValue<string> ("password", value); } }
	public int age { get { return GetValue<int> ("age", 0); } set { SetValue<int> ("age", value); } }
	public int gender { get { return GetValue<int> ("gender", 0); } set { SetValue<int> ("gender", value); } } //0 = female :: 1 = male
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		isValid = true;
		foreach (string user in NetManager.usernames) {
			if (user == username) {
				isValid = false;
			}
		}
		NetManager.usernames.Add (username);
		JsonMessage<RegisterInformation> msg = new JsonMessage<RegisterInformation> ();
		msg.message = this.Serialize ();
		NetworkServer.SendToClient (connectionId, MessageType.REGISTER_MESSAGE, msg);
	}
}












public class Acknowledgement : LgJsonDictionary, IJsonable
{
	public int ack { get { return GetValue<int>("ack", 0); } set { SetValue<int>("ack", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.customDebug.Log("Handling Acknowledgement");
		// TODO: put code that does something with this object
	}
}
