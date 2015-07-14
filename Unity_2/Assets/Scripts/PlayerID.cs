using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	PlayerData pd;

	//[SyncVar] public string playerUniqueName;
	public Text displayNameText;
	//NAME ON CANVAS
	[SyncVar (hook = "OnDisplayNameChanged")]public string displayName;
	NetworkInstanceId playerNetID;

	public override void OnStartLocalPlayer (){
		/*
		GetNetIdentity ();
		SetIdentity ();
		*/
	}
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
	
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") { 
			{
				SetIdentity();
			}  
		}   

		if (displayName==""){
			SetPlayerName();
		}
//		displayName = "PARTY: "+Random.value;
	}
*/

	/*
	[Client]
	void GetNetIdentity (){

		playerNetID = GetComponent<NetworkIdentity>().netId;
		CmdTellServerMyIdentity (MakeUniqueIdentity ());

	}

	void SetIdentity (){
		myTransform.name = MakeUniqueIdentity ();
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
