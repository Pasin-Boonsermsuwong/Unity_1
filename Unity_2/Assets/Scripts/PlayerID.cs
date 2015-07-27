﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerID : NetworkBehaviour
{
	PlayerData pd;

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

	public enum enumClass
	{
		Fighter = 0,
		Healer = 1,
		Sniper = 2,
		Assassin = 3,
		Tank = 4,
		Spartan = 5,
		Juggernaut = 6,
	}

	[SyncVar (hook = "PlayerClassSet")]public int currentClass;
	[SyncVar] public string playerUniqueName;
	public Text displayNameText;
	//NAME ON CANVAS
	[SyncVar (hook = "OnDisplayNameChanged")]public string displayName;	//USE THIS TO SYNC PLAYER NAME
	NetworkInstanceId playerNetID;

	void Start(){
	//	Debug.Log("PlayerIDStart");
		if(isLocalPlayer){
			pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
			SetPlayerName();
			currentClass = (int)(enumClass)System.Enum.Parse( typeof( enumClass ), pd.playerClass);
			Debug.Log("Start currentClass: "+currentClass);
		}else{
			displayNameText.text = displayName;
		}
		SetupChar();
	}

	void PlayerClassSet(int a){
		Debug.Log("PlayerClassSet");
		currentClass = a;
	}

	void SetupChar(){
		if(currentClass==-1)Debug.LogError("Class not set");
		Debug.Log("SetupChar: "+currentClass);
		transform.localScale = new Vector3(characterParameter[currentClass][0],characterParameter[currentClass][0],characterParameter[currentClass][0]);
		GetComponent<Rigidbody>().mass = characterParameter[currentClass][1];
		
		Health h = GetComponent<Health>();
		h.maxHP = (int)characterParameter[currentClass][2];
		h.armor = (int)characterParameter[currentClass][3];

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
