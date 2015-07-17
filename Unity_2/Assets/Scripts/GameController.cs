using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	
	public bool playerSpawned {get; set;}
	public bool pause;
	public GameObject deadPanel;
	public Text respawnCountDown;
//	public bool respawn;
	public GameObject pauseObject;
	public Slider localSliderHealth;
	public Slider localSliderReload;
	public Slider localSliderCharge;
	public Slider localSliderRun;
	public GameObject weaponPanel;
	public static int currentClass = 0;

	//CHAT
	public GameObject chatPanel;
	public InputField chatInputField;
	public Text chatInputText;	//INPUT FIELD TEXT
	public Text chatText;
	public Scrollbar chatScrollbar;
	StringBuilder chatSb = new StringBuilder();
	int chatLineCount;
	int chatLineLimit = 25;
	bool deleteTopLine; //Will be true once amount of chat lines reached limit, trigger delete topmost line everytime
	public bool chatState;
	bool chatMoveScrollbar;	//move scrollbar to lowest position
	
	void Update () {
		if(chatState)return;
		if(Input.GetButtonDown("Pause")||Input.GetButtonDown("Cancel")){
			pause = !pause;
			CheckPauseState();
		}
		if(chatMoveScrollbar&&chatScrollbar.value>0){
			chatMoveScrollbar = false;
			chatScrollbar.value = 0;
		}
	}
	public void ChangeChatState(){
		chatState = !chatState;
		if(chatState){
			chatInputField.Select();
			chatInputField.ActivateInputField();
			chatInputField.GetComponent<Image>().color = new Color32(0, 0, 255,190);
		}else{	//SUBMIT CHAT
			chatInputField.text = string.Empty;
			chatInputField.DeactivateInputField();

			chatInputField.GetComponent<Image>().color = new Color32(50,50,50,190);
		}
	}

	public void AddChatMessage(string s){
		chatSb.Append(s);
		chatText.text = chatSb.ToString();
		chatSb.AppendLine();
		chatLineCount++;
		if(!deleteTopLine&&chatLineCount>chatLineLimit){
			deleteTopLine = true;
		}
		if(deleteTopLine){	//DELETE TOPMOST LINE
			chatSb.Remove(0, chatSb.ToString().IndexOf('\n')+1);
		}
		chatMoveScrollbar = true;
	}
	
	void CheckPauseState(){
		if(pause){
			pauseObject.SetActive(true);
			UnlockCursor();
		}
		else if(!pause){
			pauseObject.SetActive(false);
			LockCursor();
		}
	}
	public void LockCursor(){
	//	Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void UnlockCursor(){
	//	Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	void OnApplicationFocus(bool focus){
		if(!playerSpawned)return;
	//	pause = !focus;
		CheckPauseState();
	//	gainFocus = focus;
	}

	public IEnumerator DeadScreen(Health h) {
	//	respawn = false;
		deadPanel.SetActive(true);
		for(int i = 6;i>0;i--){
			respawnCountDown.text = i+"";
			yield return new WaitForSeconds(1);
		}
		deadPanel.SetActive(false);
	//	respawn = true;
	}
	/*
	public void nameInputChanged(string s){
	//	Debug.Log ("nameInputChanged");
		displayName = s;
	}
*/

}
