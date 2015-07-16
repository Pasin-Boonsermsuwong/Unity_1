using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerChatControl : NetworkBehaviour {

	GameController gc;
	PlayerID playerID;
	// Use this for initialization
	void Start () {
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		playerID = GetComponent<PlayerID>();
		if(isLocalPlayer)Invoke("JoinedMessage", 1);
	}
	void JoinedMessage(){
		CmdSendChatMsg(playerID.displayName+" has joined");
	}
	void Update () {
		if(!isLocalPlayer)return;
		if(Input.GetButtonDown("Submit")){
			if(gc.chatState&&gc.chatInputText.text!=""){		//IF CURRENTLY WRITING MSG, SEND MSG FIRST
				CmdSendChatMsg(playerID.displayName+": "+gc.chatInputText.text);
				gc.chatInputText.text = "";
			}
			gc.ChangeChatState();
		}
	}
	[Command]
	void CmdSendChatMsg(string s){
		if(!isServer)return;
		RpcChat(s);
	}
	[ClientRpc]
	void RpcChat(string s){
		gc.AddChatMessage(s);
	}
}
