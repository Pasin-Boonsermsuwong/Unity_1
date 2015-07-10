using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	GameController gc;

	[SyncVar] public string playerUniqueName;
	public Text displayerNameText;
	//NAME ON CANVAS
	[SyncVar (hook = "UpdateDisplayName")]string displayName;
	NetworkInstanceId playerNetID;
	Transform myTransform;

	public override void OnStartLocalPlayer (){
		GetNetIdentity ();
		SetIdentity ();
	}
	void Awake (){
		myTransform = transform;
	}

	void Start(){

		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		if(isLocalPlayer){


			string s = gc.displayName;
			
			if(s=="Enter player name..." || s==""){
				s = "Noname";
			}
			Debug.Log("DisplayName = "+s);
			CmdUpdatePlayerName(s);
		}
	}

	void UpdateDisplayName(string s){
		Debug.Log ("UpdateDisplayName: "+s);
		displayName = s;
		displayerNameText.text = displayName;

	}

	void Update (){
		if (myTransform.name == "" || myTransform.name == "Player(Clone)") { 
			{
				SetIdentity();
			}  
		}    
	}

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

	[Command]
	void CmdUpdatePlayerName(string dName){
		Debug.Log("CmdUpdatePlayerName");
		displayName = dName;
	}
	[Command]
	void CmdTellServerMyIdentity (string name){
		playerUniqueName = name;
	} 

}
