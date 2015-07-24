using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

using System.Collections.Generic;


/*
public class ClassItem{
	public short playerControllerId;
	public string playerClass;
	public ClassItem(short ID, string playerClass1){
		playerControllerId = ID;
		playerClass = playerClass1;
	}
}

public class ClassMessage : MessageBase
{
	public short connectionID;
	public string playerClass;
}
*/


public class NetworkPlayerSelect : NetworkManager {
	/*
	 * 
	 * 
	short MsgClassRequest = 5000;
	short MsgClassReceive = 5001;
	List<ClassItem> PlayerClassList = new List<ClassItem>();
	NetworkClient m_client;
	short localConnID;
	PlayerData pd;
	void Start(){
		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
	}
	public override void OnStartServer(){
		base.OnStartServer();
		Debug.Log("OnStartServer");
		NetworkServer.RegisterHandler(MsgClassReceive, OnServerReceiveClass);
	}
	public override void OnStartClient(NetworkClient client1){
		base.OnStartClient(client1);
		Debug.Log("OnStartClient");
		m_client = client1;
	//	localConnID = (short)client1.connection.connectionId;
	}
	public override void OnClientConnect(NetworkConnection conn){
		base.OnClientConnect(conn);
		Debug.Log("OnClientConnect");
		SendClassInfoToServer(pd.playerClass,(short)m_client.connection.connectionId);
	}

	public override void OnClientSceneChanged(NetworkConnection conn){
		base.OnClientSceneChanged(conn);
		Debug.Log("OnClientSceneChanged");
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Debug.Log("OnServerAddPlayer, connID: "+conn.connectionId);
		string playerClass = "";
		bool found = false;
		foreach(ClassItem s in PlayerClassList){
			if(s.playerControllerId == conn.connectionId){
				Debug.Log("FoundPlayer");
				int index = PlayerClassList.IndexOf(s);
				playerClass = s.playerClass;
				PlayerClassList.RemoveAt(index);		
				found = true;
				break;
			}
		}
		if(!found){
			Debug.Log("Player class not found, resetting to fighter");
			playerClass = "Fighter";
		}


	//	playerClass = pd.playerClass;
		if(playerClass == "")Debug.LogError("playerClass is empty!");
		GameObject playerClassPrefab = (GameObject)Resources.Load("Char"+playerClass);
		GameObject player = (GameObject)Instantiate(playerClassPrefab, GetStartPosition().position, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

	}
	void OnServerReceiveClass(NetworkMessage netMsg){
		ClassMessage message = netMsg.ReadMessage<ClassMessage>();
		Debug.Log("Added: "+message.connectionID);
		PlayerClassList.Add(new ClassItem(message.connectionID,message.playerClass));
	}

	void SendClassInfoToServer(string cls,short connID){
		Debug.Log("SendClassInfoToServer");
		ClassMessage message = new ClassMessage();
		message.playerClass = cls;
		message.connectionID = connID;
		m_client.Send(MsgClassReceive,message);
	}

	*/
	

}
