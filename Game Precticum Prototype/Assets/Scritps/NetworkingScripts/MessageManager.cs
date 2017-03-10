using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Networking;
using LgOctEngine.CoreClasses;
using UnityEngine.SceneManagement;

public static class MessageType
{
	public static short STAFF_LOGIN_MSG = 1000;
	public static short MASTER_CLIENT_LOGIN = 1001;
	public static short USER_LOGIN_MSG = 1002;
	public static short REGISTER_MESSAGE = 1003;
	public static short USER_HOME_UPDATE = 1004;
	public static short STAFF_CLIENT = 1010;
	public static short TEAM_INFO = 1020;
	public static short BRACKET_INFO = 1021;
	public static short GAME_RESULT = 1030;
	public static short ASSIGNMENT = 1040;
	public static short ERROR = 1050;
	public static short CREATE_TEAM = 1060;
	public static short APPLY_TO_TEAM = 1061;
	public static short LEAVE_TEAM = 1062;
	public static short DISBAND_TEAM = 1063;
	public static short REQUEST_TEAM_INFO = 1064;
	public static short REQUEST_APPLICANT_INFO = 1065;
	public static short ACCEPT_APPLICANT = 1066;
	public static short DECLINE_APPLICANT = 1067;
	public static short GET_CONTACT_INFO = 1068;
	public static short REMOVE_TEAM_MEMBER = 1069;
}

public static class MessageValue
{
	public static short LOGIN_FAILURE = 0;
	public static short LOGIN_SUCCESS = 1;
}

public class EmptyMessage : MessageBase
{
	
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

public class GameResult : LgJsonDictionary, IJsonable
{
	public string result { get { return GetValue<string>("result", ""); } set { SetValue<string>("result", value); } }
	public string name { get { return GetValue<string> ("name", ""); } set { SetValue<string> ("name", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	public string home { get { return GetValue<string>("home", ""); } set { SetValue<string>("home", value); } }
	public string away { get { return GetValue<string>("away", ""); } set { SetValue<string>("away", value); } }

	public void HandleNewObject(int conn)
	{
		
	}
}

public class StaffClientMessage : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	
	public void HandleNewObject(int conn)
	{
		//NetManager.staffClients.Add (new StaffClient (username, connectionId));
	}
}

public class StaffLoginAttempt : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public string password { get { return GetValue<string>("password", ""); } set { SetValue<string>("password", value); } }
	
	public void HandleNewObject(int conn)
	{
		
	}
}

public class UserLoginAttempt : LgJsonDictionary, IJsonable
{
	public string username { get { return GetValue<string>("username", ""); } set { SetValue<string>("username", value); } }
	public string password { get { return GetValue<string>("password", ""); } set { SetValue<string>("password", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int conn)
	{
		if (isValid) {
			SceneManager.LoadScene ("UserHome");
		} else {
			GameObject.Find ("ErrorText").GetComponent<Text> ().text = "Username or password is incorrect. Please try again.";
		}
	}
}

public class StaffAcknowledgement : LgJsonDictionary, IJsonable
{
	public int ack { get { return GetValue<int>("ack", 0); } set { SetValue<int>("ack", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		if (ack == 1) {
			Debug.Log ("Connection Success...");
			CONSTANTS.isStaff = true;
			SceneManager.LoadScene ("StaffScoreTracking");
		} else {
			Debug.Log ("Connection Failure...");
			GameObject.Find ("ErrorText").GetComponent<Text>().text = "Error: Incorrect event password.";
		}
	}
}

public class UserAcknowledgement : LgJsonDictionary, IJsonable
{
	public int ack { get { return GetValue<int>("ack", 0); } set { SetValue<int>("ack", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (ack == 1) {
			Debug.Log ("Connection Success...");
			CONSTANTS.isStaff = true;
			SceneManager.LoadScene ("StaffScoreTracking");
		} else {
			Debug.Log ("Connection Failure...");
			GameObject.Find ("ErrorText").GetComponent<Text>().text = "Error: Incorrect event password.";
		}
	}
}

public class BracketInfo : LgJsonDictionary, IJsonable
{
    public Dictionary<int, List<TeamStruct>> teams = new Dictionary<int, List<TeamStruct>>();
    public string dictionaryInStringFormat = "";

    public string jsonString
    {
        get
        {
            return GetValue<string>("jsonString", "");
        }
        set
        {
            SetValue<string>("jsonString", value);
        }
    }


    /// <summary>
    /// Converts the dictionary into a json string
    /// </summary>
    /// <param name="teams"></param>
    public string BuildJsonString(Dictionary<int, List<TeamStruct>> teams)
    {
        for (int i = 0; i < teams.Count; i++)
        {
            foreach (TeamStruct team in teams[i])
            {
                dictionaryInStringFormat += "([" + team.teamName + "," + team.player1 + "," + team.player2 + "," + team.player3 + "])";
            }
            dictionaryInStringFormat += "|";
        }
        jsonString = dictionaryInStringFormat;
        return dictionaryInStringFormat;
    }

    /// <summary>
    /// Converts the json String into the dictionary
    /// </summary>
    /// <param name="jsonString"></param>
    public void BuildDictionaryFromJsonString(string jsonString)
    {
        if (jsonString.Length < 10)
        {
            return;
        }
        char[] delimiterChars = { '(', ')' };
        string[] dictKeys = jsonString.Split('|');
        for (int i = 0; i < dictKeys.Length; i++)
        {
            string[] teamVals = dictKeys[i].Split(delimiterChars);
            teams.Add(i, new List<TeamStruct>());
            if (teamVals.Length > 2)
            {
                for (int j = 0; j < teamVals.Length; j++)
                {
                    string[] t = teamVals[j].Split(',');
                    if (t.Length > 2)
                    {
                        teams[i].Add(new TeamStruct(t[0], t[1], t[2], t[3]));
                    }
                }
            }
        }
    }
	
	public void HandleNewObject(int connectionId)
	{
        //This is called whenenver you recieve a packet that contains the bracket info
        BuildDictionaryFromJsonString(jsonString);

        //"teams" is a Dictionary<int, List<TeamStruct>>. 
        GameObject.Find("Main Camera").GetComponent<BracketScript>().ReceiveBracketData(teams);
	}
}

public class StaffInstruction : LgJsonDictionary, IJsonable
{
	public string homeTeam
    {
        get
        {
            return GetValue<string> ("homeTeam", "");
        }
        set
        {
            SetValue<string> ("homeTeam", value);
        }
    }
	public string awayTeam { get { return GetValue<string> ("awayTeam", ""); } set { SetValue<string> ("awayTeam", value); } }
	public int connectionId { get { return GetValue<int> ("connectionId", 0); } set { SetValue<int> ("connectionId", value); } }
	public int fieldNum { get { return GetValue<int> ("fieldNum", 0); } set { SetValue<int> ("fieldNum", value); } }
	
	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.homeTeamName = homeTeam;
		CONSTANTS.awayTeamName = awayTeam;
		CONSTANTS.fieldNum = fieldNum;
		if (SceneManager.GetActiveScene().name == "StaffScoreTracking") {
			GameObject.Find ("Canvas").GetComponent<StaffClientControl>().ChangeWaitingMessage();
		}
	}
}

public class RegisterInformation : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }
	public string firstName { get { return GetValue<string> ("firstName", ""); } set { SetValue<string> ("firstName", value); } }
	public string lastName { get { return GetValue<string> ("lastName", ""); } set { SetValue<string> ("lastName", value); } }
	public string email { get { return GetValue<string> ("email", ""); } set { SetValue<string> ("email", value); } }
	public string password { get { return GetValue<string> ("password", ""); } set { SetValue<string> ("password", value); } }
	public string dateOfBirth { get { return GetValue<string> ("dateOfBirth", ""); } set { SetValue<string> ("dateOfBirth", value); } }
	public string cell{ get { return GetValue<string> ("cell", ""); } set { SetValue<string> ("cell", value); } }
	public string address { get { return GetValue<string> ("address", ""); } set { SetValue<string> ("address", value); } }
	public string zipCode { get { return GetValue<string> ("zipCode", ""); } set { SetValue<string> ("zipCode", value); } }
	public int gender { get { return GetValue<int> ("gender", 0); } set { SetValue<int> ("gender", value); } } //0 = female :: 1 = male
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.mainMenuControl.SignUpClick ();
			GameObject.Find ("ErrorText").GetComponent<Text> ().text = "Your account has been created!\nYou can now log in.";
		} else {
			GameObject.Find ("RegisterErrorText").GetComponent<Text> ().text = "That username is already in use.";
		}
	}
}

public class RequestUserHomeUpdate : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }

	public void HandleNewObject(int connectionId)
	{

	}
}

public class UserHomeUpdate : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string teamLeader { get { return GetValue<string> ("teamLeader", ""); } set { SetValue<string> ("teamLeader", value); } }
	public string player1 { get { return GetValue<string> ("player1", ""); } set { SetValue<string> ("player1", value); } }
	public string player2 { get { return GetValue<string> ("player2", ""); } set { SetValue<string> ("player2", value); } }
	public int status { get { return GetValue<int> ("status", 0); } set { SetValue<int> ("status", value); } } //0=not on team, 1=leader, 2=member

	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.userHomeControl.ReceiveUpdate (teamName, teamLeader, player1, player2, status);
	}
}

public class CreateTeam : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string teamLeader { get { return GetValue<string> ("teamLeader", ""); } set { SetValue<string> ("teamLeader", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.userHomeControl.CreateTeamClose ();
			CONSTANTS.netManager.RequestUserHomeUpdate ();
		} else {
			GameObject.Find ("CreateTeamMenu").transform.GetChild(0).FindChild ("ErrorText").GetComponent<Text> ().text = "That team name is already taken";
		}
	}
}

public class SendTeamApplication : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string playerName { get { return GetValue<string> ("playerName", ""); } set { SetValue<string> ("playerName", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.userHomeControl.ApplyForTeamClose ();
			CONSTANTS.netManager.RequestUserHomeUpdate ();
			CONSTANTS.userHomeControl.DisplayVerificationMenu ("You have applied for team: <color=teal>" + teamName + "</color>. The team leader will have to accept your request before you are put on the team.");
		} else {
			GameObject.Find ("ApplyToTeamMenu").transform.GetChild(0).FindChild ("ErrorText").GetComponent<Text> ().text = "Error: You already have a pending application to this team.";
		}
	}
}

public class LeaveTeam : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string playerName { get { return GetValue<string> ("playerName", ""); } set { SetValue<string> ("playerName", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.userHomeControl.LeaveTeamClose ();
			CONSTANTS.netManager.RequestUserHomeUpdate ();
		} else {
			GameObject.Find ("LeaveTeamMenu").transform.GetChild(0).FindChild ("ErrorText").GetComponent<Text> ().text = "Error: Did not leave team.\nTeam name is case sensitive.";
		}
	}
}

public class DisbandTeam : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string playerName { get { return GetValue<string> ("playerName", ""); } set { SetValue<string> ("playerName", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.userHomeControl.DisbandTeamClose ();
			CONSTANTS.netManager.RequestUserHomeUpdate ();
		} else {
			GameObject.Find ("DisbandTeamMenu").transform.GetChild(0).FindChild ("ErrorText").GetComponent<Text> ().text = "Error: Team not deleted.\nTeam name is case sensitive.";
		}
	}
}

public class RequestForTeamList : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string teamLeader { get { return GetValue<string> ("teamLeader", ""); } set { SetValue<string> ("teamLeader", value); } }
	public string player1 { get { return GetValue<string> ("player1", ""); } set { SetValue<string> ("player1", value); } }
	public string player2 { get { return GetValue<string> ("player2", ""); } set { SetValue<string> ("player2", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (teamName != "1110274") {
			CONSTANTS.userHomeControl.AddTeamToList (teamName, teamLeader, player1, player2);
		} else {
			CONSTANTS.userHomeControl.FinishedAddingTeamsToList ();
		}

	}
}

public class RequestForApplicants : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }
	public string playerName { get { return GetValue<string> ("playerName", ""); } set { SetValue<string> ("playerName", value); } }
	public string dateOfBirth { get { return GetValue<string> ("dateOfBirth", ""); } set { SetValue<string> ("dateOfBirth", value); } }
	public int gender { get { return GetValue<int> ("gender", 0); } set { SetValue<int> ("gender", value); } } //0 = female :: 1 = male

	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.userHomeControl.AddApplicantToList (username, playerName, dateOfBirth, gender);
	}
}

public class AcceptApplicant : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }

	public void HandleNewObject(int connectionId)
	{
		
	}
}

public class DeclineApplicant : LgJsonDictionary, IJsonable
{
	public string username{ get { return GetValue<string> ("username", ""); } set { SetValue<string> ("username", value); } }
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }

	public void HandleNewObject(int connectionId)
	{

	}
}

public class GetContactInfo : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string player0Name { get { return GetValue<string> ("player0Name", ""); } set { SetValue<string> ("player0Name", value); } }
	public string player0Email { get { return GetValue<string> ("player0Email", ""); } set { SetValue<string> ("player0Email", value); } }
	public string player0Phone { get { return GetValue<string> ("player0Phone", ""); } set { SetValue<string> ("player0Phone", value); } }
	public string player1Name { get { return GetValue<string> ("player1Name", ""); } set { SetValue<string> ("player1Name", value); } }
	public string player1Email { get { return GetValue<string> ("player1Email", ""); } set { SetValue<string> ("player1Email", value); } }
	public string player1Phone { get { return GetValue<string> ("player1Phone", ""); } set { SetValue<string> ("player1Phone", value); } }
	public string player2Name { get { return GetValue<string> ("player2Name", ""); } set { SetValue<string> ("player2Name", value); } }
	public string player2Email { get { return GetValue<string> ("player2Email", ""); } set { SetValue<string> ("player2Email", value); } }
	public string player2Phone { get { return GetValue<string> ("player2Phone", ""); } set { SetValue<string> ("player2Phone", value); } }

	public void HandleNewObject(int connectionId)
	{
		CONSTANTS.userHomeControl.SetTeamContactInfo (player0Name, player0Email, player0Phone, player1Name, player1Email, player1Phone, player2Name, player2Email, player2Phone);
	}
}

public class RemoveTeamMember : LgJsonDictionary, IJsonable
{
	public string teamName { get { return GetValue<string> ("teamName", ""); } set { SetValue<string> ("teamName", value); } }
	public string playerName { get { return GetValue<string> ("playerName", ""); } set { SetValue<string> ("playerName", value); } }
	public bool isValid { get { return GetValue<bool> ("isValid", false); } set { SetValue<bool> ("isValid", value); } }

	public void HandleNewObject(int connectionId)
	{
		if (isValid) {
			CONSTANTS.userHomeControl.RemoveTeamMemberClose ();
			CONSTANTS.netManager.RequestUserHomeUpdate ();
		} else {
			CONSTANTS.userHomeControl.kickMemberMenu.transform.GetChild (0).FindChild ("ErrorText").GetComponent<Text> ().text = "Error: Player not removed.\nPlayer name is case sensitive.";
		}
	}
}