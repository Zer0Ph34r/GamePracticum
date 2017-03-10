using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LgOctEngine.CoreClasses;

public class NetManager : NetworkManager {
	
	// Use this for initialization
	void Start () {
		Debug.Log ("NetMangager:Start()");
		StartClient ();
		client.RegisterHandler (MessageType.USER_LOGIN_MSG, OnServerReceiveMessage<UserLoginAttempt>);
		client.RegisterHandler (MessageType.USER_HOME_UPDATE, OnServerReceiveMessage<UserHomeUpdate>);
		client.RegisterHandler (MessageType.STAFF_LOGIN_MSG, OnServerReceiveMessage<StaffAcknowledgement>);
		client.RegisterHandler (MessageType.ASSIGNMENT, OnServerReceiveMessage<StaffInstruction>);
		client.RegisterHandler (MessageType.REGISTER_MESSAGE, OnServerReceiveMessage<RegisterInformation>);
		client.RegisterHandler (MessageType.CREATE_TEAM, OnServerReceiveMessage<CreateTeam>);
		client.RegisterHandler (MessageType.APPLY_TO_TEAM, OnServerReceiveMessage<SendTeamApplication>);
		client.RegisterHandler (MessageType.LEAVE_TEAM, OnServerReceiveMessage<LeaveTeam>);
		client.RegisterHandler (MessageType.DISBAND_TEAM, OnServerReceiveMessage<DisbandTeam>);
		client.RegisterHandler (MessageType.REQUEST_TEAM_INFO, OnServerReceiveMessage<RequestForTeamList>);
		client.RegisterHandler (MessageType.REQUEST_APPLICANT_INFO, OnServerReceiveMessage<RequestForApplicants>);
		client.RegisterHandler (MessageType.GET_CONTACT_INFO, OnServerReceiveMessage<GetContactInfo>);
		client.RegisterHandler (MessageType.REMOVE_TEAM_MEMBER, OnServerReceiveMessage<RemoveTeamMember>);
        client.RegisterHandler (MessageType.BRACKET_INFO, OnServerReceiveMessage<BracketInfo>);
	}
	
	void OnServerReceiveMessage<T>(NetworkMessage netMsg) where T : LgJsonDictionary, IJsonable, new()
	{
		JsonMessage<T> jsonMessage = netMsg.ReadMessage<JsonMessage<T>>();
		Debug.Log("OnServerReceiveMessage: Received Message " + jsonMessage.message);
		T obj = LgJsonNode.CreateFromJsonString<T>(jsonMessage.message);
		obj.HandleNewObject(netMsg.conn.connectionId);
	}

    public void SendRequestForBracketInfo()
    {
        BracketInfo info = LgJsonNode.Create<BracketInfo>();
        //info.jsonString = info.BuildJsonString(teams);
        JsonMessage<BracketInfo> msg = new JsonMessage<BracketInfo>();
        msg.message = info.Serialize();
        //client.Send(MessageType.BRACKET_INFO, msg);
    }

	public void SendRemoveMemberRequest(string name)
	{
		RemoveTeamMember instruction = LgJsonNode.Create<RemoveTeamMember> ();
		instruction.teamName = CONSTANTS.teamName;
		instruction.playerName = name;
		instruction.isValid = false;
		JsonMessage<RemoveTeamMember> msg = new JsonMessage<RemoveTeamMember> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.REMOVE_TEAM_MEMBER, msg);
	}

	public void SendRequestForTeamContactInfo()
	{
		GetContactInfo request = LgJsonNode.Create<GetContactInfo> ();
		request.teamName = CONSTANTS.teamName;
		request.player0Name = "";
		request.player0Email = "";
		request.player0Phone = "";
		request.player1Name = "";
		request.player1Email = "";
		request.player1Phone = "";
		request.player2Name = "";
		request.player2Email = "";
		request.player2Phone = "";
		JsonMessage<GetContactInfo> msg = new JsonMessage<GetContactInfo> ();
		msg.message = request.Serialize ();
		client.Send (MessageType.GET_CONTACT_INFO, msg);
	}

	public void SendApplicantAcceptance(string username, string teamName)
	{
		AcceptApplicant instruction = LgJsonNode.Create<AcceptApplicant> ();
		instruction.teamName = teamName;
		instruction.username = username;
		JsonMessage<AcceptApplicant> msg = new JsonMessage<AcceptApplicant> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.ACCEPT_APPLICANT, msg);
	}

	public void SendApplicantDenial(string username, string teamName)
	{
		DeclineApplicant instruction = LgJsonNode.Create<DeclineApplicant> ();
		instruction.teamName = teamName;
		instruction.username = username;
		JsonMessage<DeclineApplicant> msg = new JsonMessage<DeclineApplicant> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.DECLINE_APPLICANT, msg);
	}

	public void SendDeleteTeamRequest(string teamName)
	{
		DisbandTeam instruction = LgJsonNode.Create<DisbandTeam> ();
		instruction.teamName = teamName;
		instruction.playerName = CONSTANTS.username;
		instruction.isValid = false;
		JsonMessage<DisbandTeam> msg = new JsonMessage<DisbandTeam> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.DISBAND_TEAM, msg);
	}

	public void SendCreateTeamRequest(string teamName)
	{
		CreateTeam instruction = LgJsonNode.Create<CreateTeam> ();
		instruction.teamName = teamName;
		instruction.teamLeader = CONSTANTS.username;
		instruction.isValid = false;
		JsonMessage<CreateTeam> msg = new JsonMessage<CreateTeam> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.CREATE_TEAM, msg);
	}

	public void SendLeaveTeamRequest(string teamName)
	{
		LeaveTeam instruction = LgJsonNode.Create<LeaveTeam> ();
		instruction.teamName = teamName;
		instruction.playerName = CONSTANTS.username;
		instruction.isValid = false;
		JsonMessage<LeaveTeam> msg = new JsonMessage<LeaveTeam> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.LEAVE_TEAM, msg);
	}

	public void SendRequestForTeamList()
	{
		RequestForTeamList request = LgJsonNode.Create<RequestForTeamList> ();
		request.teamName = "1110274";
		request.teamLeader = "NULL";
		request.player1 = "NULL";
		request.player2 = "NULL";

		JsonMessage<RequestForTeamList> msg = new JsonMessage<RequestForTeamList> ();
		msg.message = request.Serialize ();
		client.Send (MessageType.REQUEST_TEAM_INFO, msg);
	}

	public void SendPlayerTeamApplication(string teamName)
	{
		SendTeamApplication instruction = LgJsonNode.Create<SendTeamApplication> ();
		instruction.teamName = teamName;
		instruction.playerName = CONSTANTS.username;
		instruction.isValid = false;
		JsonMessage<SendTeamApplication> msg = new JsonMessage<SendTeamApplication> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.APPLY_TO_TEAM, msg);
	}

	public void SendRequestForApplicants()
	{
		RequestForApplicants instruction = LgJsonNode.Create<RequestForApplicants> ();
		instruction.username = CONSTANTS.teamName;
		instruction.playerName = "";
		instruction.dateOfBirth = "";
		instruction.gender = 0;
		JsonMessage<RequestForApplicants> msg = new JsonMessage<RequestForApplicants> ();
		msg.message = instruction.Serialize ();
		client.Send (MessageType.REQUEST_APPLICANT_INFO, msg);
	}

	public void RequestUserHomeUpdate()
	{
		RequestUserHomeUpdate request = LgJsonNode.Create<RequestUserHomeUpdate> ();
		request.username = CONSTANTS.username;
		JsonMessage<RequestUserHomeUpdate> msg = new JsonMessage<RequestUserHomeUpdate> ();
		msg.message = request.Serialize ();
		client.Send (MessageType.USER_HOME_UPDATE, msg);
	}

	public void SendUserLoginRequest(GameObject[] gameArray)
	{
		CONSTANTS.username = gameArray [0].GetComponent<InputField> ().text;
		UserLoginAttempt user = LgJsonNode.Create<UserLoginAttempt> ();
		user.username = CONSTANTS.username;
		user.password = gameArray [1].GetComponent<InputField> ().text;
		user.isValid = false;
		JsonMessage<UserLoginAttempt> msg = new JsonMessage<UserLoginAttempt> ();
		msg.message = user.Serialize ();
		client.Send (MessageType.USER_LOGIN_MSG, msg);
	}
	
	public void SendStaffLoginRequest(GameObject[] gameArray)
	{
		CONSTANTS.name = gameArray[0].GetComponent<InputField> ().text;
		StaffLoginAttempt staff = LgJsonNode.Create<StaffLoginAttempt> ();
		staff.username = CONSTANTS.name;
		staff.password = gameArray[1].GetComponent<InputField> ().text;
		JsonMessage<StaffLoginAttempt> msg = new JsonMessage<StaffLoginAttempt> ();
		msg.message = staff.Serialize ();
		client.Send(MessageType.STAFF_LOGIN_MSG, msg);
	}

	public void SendRegistrationRequest(string username, string firstName, string lastName, string email, string password, string dateOfBirth, string cell, string address, string zipcode, int gender)
	{
		RegisterInformation info = LgJsonNode.Create<RegisterInformation> ();
		info.username = username;
		info.firstName = firstName;
		info.lastName = lastName;
		info.email = email;
		info.password = password;
		info.dateOfBirth = dateOfBirth;
		info.cell = cell;
		info.address = address;
		info.zipCode = zipcode;
		info.gender = gender;
		info.isValid = false;
		JsonMessage<RegisterInformation> msg = new JsonMessage<RegisterInformation> ();
		msg.message = info.Serialize ();
		client.Send (MessageType.REGISTER_MESSAGE, msg);
	}

	public void SendResults(string winner)
	{
		GameResult result = LgJsonNode.Create<GameResult> ();
		result.result = winner;
		result.connectionId = 0;
		result.name = CONSTANTS.name;
		result.home = CONSTANTS.homeTeamName;
		result.away = CONSTANTS.awayTeamName;
		JsonMessage<GameResult> msg = new JsonMessage<GameResult> ();
		msg.message = result.Serialize ();
		client.Send (MessageType.GAME_RESULT, msg);
	}

	public override void OnClientConnect(NetworkConnection con)
	{
		CONSTANTS.isConnectedToServer = true;
	}

	public override void OnClientDisconnect(NetworkConnection con)
	{
		CONSTANTS.isConnectedToServer = false;
	}
}
