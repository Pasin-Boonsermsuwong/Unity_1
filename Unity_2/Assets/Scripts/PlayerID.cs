using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	GameController gc;
	PlayerData pd;

	[SyncVar] public string playerUniqueName;
	public Text displayerNameText;
	//NAME ON CANVAS
	[SyncVar (hook = "OnDisplayNameChanged")]string displayName;
	NetworkInstanceId playerNetID;
	Transform myTransform;

	public override void OnStartLocalPlayer (){
		/*
		GetNetIdentity ();
		SetIdentity ();
		*/
	}
	void Awake (){
		myTransform = transform;
	}

	void Start(){

		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();

		if(isLocalPlayer){
			SetPlayerName();
		}else{
			displayerNameText.text = displayName;
		}
	}
	void SetPlayerName(){
		string s = pd.playerName;
		
		if(s==""){
			s = "Player"+GetComponent<NetworkIdentity>().netId;;
		}
		
		Debug.Log("DisplayName = "+s);
		CmdUpdatePlayerName(s);
	}

	void OnDisplayNameChanged(string s){
		Debug.Log ("OnDisplayNameChanged: "+s);
		displayName = s;
		displayerNameText.text = displayName;
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
		Debug.Log("CmdUpdatePlayerName");
		displayName = dName;
	}
	/*
	[Command]
	void CmdTellServerMyIdentity (string name){
		playerUniqueName = name;
	} 
*/
}
