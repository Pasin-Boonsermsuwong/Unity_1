using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	PlayerData pd;

	float[][] characterParameter = {
		//	TRANSFORM SCALE, MASS, MAXHP, ARMOR, 	FWD_SPEED,	BCK_SPEED,	STRAFE_SPD,	JUMP_FORCE
		new float[]{2f,8000000,310,10,	7040000,	3520000,	3520000,	61600000},//fighter
		new float[]{1.5f,3000000,175,5,	800000,		400000,		400000,		7000000},//healer
		new float[]{0,0,0,0,0,0},//range
		new float[]{0,0,0,0,0,0},//scout
		new float[]{0,0,0,0,0,0},//tank
		new float[]{0,0,0,0,0,0},//spartan
		new float[]{0,0,0,0,0,0},//juggernaut
	};



	public enum enumClass
	{
		Fighter = 0,
		Healer = 1,
		Ranger = 2,
		Assassin = 3,
		Tank = 4,
		Spartan = 5,
		Juggernaut = 6,
	}
	public int currentClass;
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
			currentClass = (int)(enumClass)System.Enum.Parse( typeof( enumClass ), pd.playerClass);
			CmdSetupChar(currentClass);
		}else{
			displayNameText.text = displayName;
		}


	}

	[Command]
	void CmdSetupChar(int curClass){
		RpcSetupChar(curClass);
	}
	[ClientRpc]
	void RpcSetupChar(int curClass){
		SetupChar();
	}

	void SetupChar(){
		transform.localScale = new Vector3(characterParameter[currentClass][0],characterParameter[currentClass][0],characterParameter[currentClass][0]);
		GetComponent<Rigidbody>().mass = characterParameter[currentClass][1];
		
		Health h = GetComponent<Health>();
		h.maxHP = (int)characterParameter[currentClass][2];
		h.armor = (int)characterParameter[currentClass][3];
		h.LateStart();

		RbFPC_Custom c = GetComponent<RbFPC_Custom>();
		c.movementSettings.ForwardSpeed = characterParameter[currentClass][4];
		c.movementSettings.BackwardSpeed = characterParameter[currentClass][5];
		c.movementSettings.StrafeSpeed = characterParameter[currentClass][6];
		c.movementSettings.JumpForce = characterParameter[currentClass][7];
		c.SetCapsuleSize();
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

	[Command]
	void CmdUpdatePlayerName(string dName){
	//	Debug.Log("CmdUpdatePlayerName");
		displayName = dName;
	}
}
