using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class NetworkPlayerSelect : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		GameObject player = (GameObject)Instantiate((GameObject)Resources.Load("CharFighter"), GameObject.FindWithTag("SpawnPosition").transform.position, Quaternion.identity);
	//	player.GetComponent<Player>().color = Color.Red;
		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}
}
