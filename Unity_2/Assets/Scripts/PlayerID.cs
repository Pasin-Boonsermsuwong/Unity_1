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
			displayNameText.text = displayName;
		}
	}


	/*
	float[][] characterParameter = {
	//	TRANSFORM SCALE, MASS, MAXHP, ARMOR, 	FWD_SPEED,	BCK_SPEED,	STRAFE_SPD,	JUMP_FORCE
		new float[]{2f,8000000,310,10,	7040000,	3520000,	3520000,	61600000},//fighter
		new float[]{1.5f,3000000,175,5,	2400000,	1200000,	1200000,	21000000},//healer
		new float[]{0,0,0,0,0,0},//snipe
		new float[]{0,0,0,0,0,0},//scout
		new float[]{0,0,0,0,0,0},//tank
		new float[]{0,0,0,0,0,0},//spartan
		new float[]{0,0,0,0,0,0},//juggernaut
	};

	[SyncVar (hook = "PlayerClassSet")]
	public int currentClass; //ARRAY INDEX STARTS WITH 0 (Fighter)

//	[SyncVar] public string playerUniqueName;


	//NAME ON CANVAS

	NetworkInstanceId playerNetID;


	void SetupCharLocal(int c){
		Debug.Log("SetupCharLocal: "+c);
		transform.localScale = new Vector3(characterParameter[currentClass][0],characterParameter[currentClass][0],characterParameter[currentClass][0]);
		GetComponent<Rigidbody>().mass = characterParameter[currentClass][1];
		
		RbFPC_Custom c1 = GetComponent<RbFPC_Custom>();
		c1.movementSettings.ForwardSpeed = characterParameter[currentClass][4];
		c1.movementSettings.BackwardSpeed = characterParameter[currentClass][5];
		c1.movementSettings.StrafeSpeed = characterParameter[currentClass][6];
		c1.movementSettings.JumpForce = characterParameter[currentClass][7];
		c1.SetCapsuleSize();
	}
	void SetupCharRemote(){
		Debug.Log("SetupCharRemote: "+currentClass);
		transform.localScale = new Vector3(characterParameter[currentClass][0],characterParameter[currentClass][0],characterParameter[currentClass][0]);
		GetComponent<Rigidbody>().mass = characterParameter[currentClass][1];
		
		RbFPC_Custom c = GetComponent<RbFPC_Custom>();
		c.movementSettings.ForwardSpeed = characterParameter[currentClass][4];
		c.movementSettings.BackwardSpeed = characterParameter[currentClass][5];
		c.movementSettings.StrafeSpeed = characterParameter[currentClass][6];
		c.movementSettings.JumpForce = characterParameter[currentClass][7];
		c.SetCapsuleSize();
	}
	[Command]
	void CmdTellServerMyClass(int c){
		currentClass = c;
		SetupCharServer(c);
	}
	[Server]
	void SetupCharServer(int c){
		Debug.Log("SetupCharServer: "+c);
		Health h = GetComponent<Health>();
		h.maxHP = (int)characterParameter[c][2];
		h.armor = (int)characterParameter[c][3];
		h.SetCurHP();
	}
	void PlayerClassSet(int currentClass1){
		currentClass = currentClass1;
		Debug.Log("PlayerClassSet: "+currentClass);
		SetupCharRemote();
	}

*/
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
		displayNameText.text = displayName;
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
		default:
			Debug.LogError("Class name not found!");
			return -1;
		}
	}

}
