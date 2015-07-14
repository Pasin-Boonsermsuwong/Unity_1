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
	public GameObject weaponPanel;
	public static int currentClass = 0;

//	public GameObject chatInput;
	public GameObject chatPanel;
	public InputField chatInputField;
	public Text chatInputText;	//INPUT FIELD TEXT
	public Text chatText;
	public Scrollbar chatScrollbar;
	StringBuilder chatSb = new StringBuilder();

	public bool chatState = false;
	PlayerData pd;
	/*
	void Start(){
		Debug.Log("GameController start");
	}
*/
	void Update () {
		if(Input.GetButtonDown("Pause")||Input.GetButtonDown("Cancel")){
			pause = !pause;
			CheckPauseState();
		}

	}
	public void ChangeChatState(){
		chatState = !chatState;
		if(chatState){
			chatInputField.Select();
			chatInputField.ActivateInputField();
		}else{	//SUBMIT CHAT
			chatInputField.text = string.Empty;
			chatInputField.DeactivateInputField();
		}
	}

	public void AddChatMessage(string s){
		chatSb.Append(s);
		chatText.text = chatSb.ToString();
		chatSb.AppendLine();
		chatScrollbar.value = 0;
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
