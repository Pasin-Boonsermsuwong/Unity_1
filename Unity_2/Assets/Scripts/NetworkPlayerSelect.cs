using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class NetworkPlayerSelect : NetworkManager {
	short MsgClassReceive = 5001;
	public string playerClass;
	NetworkClient m_client;
	public override void OnClientConnect(NetworkConnection conn){
	//	base.OnClientConnect(conn);
	//	Debug.Log("OnClientConnect");
		SendClassInfoToServer();

	//	DataSend dataSend = GameObject.Find("DataSend").GetComponent<DataSend>();
	//	dataSend.CmdSendPlayerClass(playerData.playerClass);
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
	//	Debug.Log("OnServerAddPlayer, connID: "+conn.connectionId);
		if(playerClass == "")Debug.LogError("playerClass is empty!");
	//	Debug.Log("ServerPclass: Char"+playerClass);
		playerPrefab = (GameObject)Resources.Load("Char"+playerClass);
		GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
	/*
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate(playerPrefab, GetStartPosition().position, Quaternion.identity);
		player.GetComponent<PlayerID>().currentClass = 
		player.GetComponent<Player>().color = Color.Red;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
*/

	/*
	 * 
	 * 
	short MsgClassRequest = 5000;

	List<ClassItem> PlayerClassList = new List<ClassItem>();

	short localConnID;
	PlayerData pd;
	void Start(){
		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
	}




	public override void OnClientSceneChanged(NetworkConnection conn){
		base.OnClientSceneChanged(conn);
		Debug.Log("OnClientSceneChanged");
	}
	*/
	public override void OnStartClient(NetworkClient client1){
		base.OnStartClient(client1);
	//	Debug.Log("OnStartClient");
		m_client = client1;
	}
	void OnServerReceiveClass(NetworkMessage netMsg){
		StringMessage message = netMsg.ReadMessage<StringMessage>();
		playerClass = message.value;
	//	Debug.Log("Server receive class: "+playerClass);
	}
	public override void OnStartServer(){
		base.OnStartServer();
	//	Debug.Log("OnStartServer");
		NetworkServer.RegisterHandler(MsgClassReceive, OnServerReceiveClass);
	}
	void SendClassInfoToServer(){
	//	Debug.Log("SendClassInfoToServer");
		PlayerData playerData = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
		StringMessage message = new StringMessage(playerData.playerClass);
		m_client.Send(MsgClassReceive,message);
	}
	

}
