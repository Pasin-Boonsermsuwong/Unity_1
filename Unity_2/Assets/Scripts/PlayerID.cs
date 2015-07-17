using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	PlayerData pd;
	//GameController gc;
	[SyncVar] public string playerUniqueName;
	public Text displayNameText;
	//NAME ON CANVAS
	[SyncVar (hook = "OnDisplayNameChanged")]public string displayName;	//USE THIS TO SYNC PLAYER NAME
	NetworkInstanceId playerNetID;

	void Start(){
		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

		if(isLocalPlayer){
			SetPlayerName();
		}else{
			displayNameText.text = displayName;
		}



	}
	void SetPlayerName(){
		string s = pd.playerName;
		
		if(s==""){
			s = "Player"+GetComponent<NetworkIdentity>().netId;;
			displayName = s;
		}
		
	//	Debug.Log("DisplayName = "+s);
		CmdUpdatePlayerName(s);
	}

	void OnDisplayNameChanged(string s){
	//	Debug.Log ("OnDisplayNameChanged: "+s);
		displayName = s;
		displayNameText.text = displayName;
	}
	/*
	void Update (){
		if (transform.name == "" || transform.name == "CharFighterRB(Clone)") { 
			{
				SetIdentity();
		//		firstNameCheck = true;
			}  
		}   
	}
*/
	/*
	[Client]
	void GetNetIdentity (){

		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity (MakeUniqueIdentity ());

	}

	void SetIdentity (){
		if(!isLocalPlayer){
			transform.name = playerUniqueName;
		}
		else transform.name = MakeUniqueIdentity ();
	}

	string MakeUniqueIdentity (){
		string uniqueName = "Player" + playerNetID;
		return uniqueName;
	}
*/
	[Command]
	void CmdUpdatePlayerName(string dName){
	//	Debug.Log("CmdUpdatePlayerName");
		displayName = dName;
	}
	/*
	[Command]
	void CmdTellServerMyIdentity (string name){
		playerUniqueName = name;
	} 
*/
}
