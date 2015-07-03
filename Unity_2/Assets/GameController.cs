using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public bool playerSpawned {get; set;}
	public bool pause;
//	bool gainFocus;
	public GameObject pauseObject;
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


}
