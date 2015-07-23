using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayerSelect : NetworkManager {

	PlayerData pd;
	void Start(){
		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
	}
	public override void OnClientConnect(NetworkConnection conn){
		base.OnClientConnect(conn);
		Debug.Log("OnClientConnect");
	}
	public override void OnClientSceneChanged(NetworkConnection conn){
		base.OnClientSceneChanged(conn);
		Debug.Log("OnClientSceneChanged");
	}
	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
	//	NetworkServer.SendToClient(playerControllerId,int,);
		Debug.Log("OnServerAddPlayer");
		string playerClass = pd.playerClass;
		GameObject playerClassPrefab = (GameObject)Resources.Load("Char"+playerClass);
		GameObject player = (GameObject)Instantiate(playerClassPrefab, GetStartPosition().position, Quaternion.identity);
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
}
