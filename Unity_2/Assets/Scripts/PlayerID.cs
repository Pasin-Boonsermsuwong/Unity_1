using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	PlayerData pd;
	[SyncVar (hook = "OnDisplayNameChanged")]public string displayName;	//USE THIS TO SYNC PLAYER NAME
	public Text displayNameText;
	public int currentClass;

	void Start(){
	//	Debug.Log("PlayerIDStart");
		if(isLocalPlayer){
			pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
			currentClass = ClassStringToInt( pd.playerClass);
			SetPlayerName();
		}else{
			if(displayNameText!=null)displayNameText.text = displayName;
		}
	}

	void SetPlayerName(){
		string s = pd.playerName;
		
		if(s==""){
			s = "Player"+GetComponent<NetworkIdentity>().netId;;
			displayName = s;
		}

		CmdUpdatePlayerName(s);
	}

	[Command]
	void CmdUpdatePlayerName(string dName){
	//	Debug.Log("CmdUpdatePlayerName");
		displayName = dName;
	}

	void OnDisplayNameChanged(string s){
		//	Debug.Log ("OnDisplayNameChanged: "+s);
		displayName = s;
		if(displayNameText!=null)displayNameText.text = displayName;
	}


	int ClassStringToInt(string className){
		switch (className)
		{
		case "Fighter":
			return 0;
		case "Healer":
			return 1;
		case "Sniper":
			return 2;
		case "Assassin":
			return 3;
		case "Tank":
			return 4;
		case "Juggernaut":
			return 5;
		case "Spectator":
			return 200;
		default:
			Debug.LogError("Class name not found!");
			return -1;
		}
	}

}
