using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	[SyncVar] public string playerUniqueName;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;
	public override void OnStartLocalPlayer (){
		GetNetIdentity ();
		SetIdentity ();
	}
	void Awake (){
		myTransform = transform;
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
		if(!isLocalPlayer){
			myTransform.name = MakeUniqueIdentity ();
		}else{
			myTransform.name = MakeUniqueIdentity ();
		}
	}
	string MakeUniqueIdentity (){
		string uniqueName = "Player" + playerNetID;
		return uniqueName;
	}
	[Command]
	void CmdTellServerMyIdentity (string name){
		playerUniqueName = name;
	} 

}
