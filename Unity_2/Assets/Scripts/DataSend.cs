using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DataSend : NetworkBehaviour {

	public string playerClass;
	//PlayerData pd;
	void Start(){
//		pd = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>();
	}
	[Command]
	public void CmdSendPlayerClass(string s){
		Debug.Log("CmdSendPlayerClass: "+s);
		playerClass = s;
	}
}
