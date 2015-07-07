using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameController : MonoBehaviour {
	
	public bool playerSpawned {get; set;}
	public bool pause;
	public GameObject deadPanel;
	public Text respawnCountDown;
	public bool respawn;
	public GameObject pauseObject;
	public Slider localSliderHealth;
	public Slider localSliderReload;
	public Slider localSliderCharge;
	public GameObject weaponPanel;
	public static int currentClass = 0;

	GameObject player;

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pause")||Input.GetButtonDown("Cancel")){
			pause = !pause;
			CheckPauseState();
	//		gainFocus = false;
		}
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
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void UnlockCursor(){
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	void OnApplicationFocus(bool focus){
		if(!playerSpawned)return;
	//	pause = !focus;
		CheckPauseState();
	//	gainFocus = focus;
	}
	/*
	public void DeadScreenInit(){
		yield return StartCoroutine(DeadScreen);
	}
	*/
	public IEnumerator DeadScreen(Health h) {
		respawn = false;
		deadPanel.SetActive(true);
		for(int i = 6;i>0;i--){
			respawnCountDown.text = i+"";
			yield return new WaitForSeconds(1);
		}
		deadPanel.SetActive(false);
		respawn = true;
		h.RpcRespawn();
	}


}
