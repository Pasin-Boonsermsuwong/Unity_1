using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	bool playerSpawned {get; set;}
	public bool pause;
	public GameObject pauseObject;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pause")||Input.GetButtonDown("Cancel")){
			pause = !pause;
			ChangePauseState();
			
		}
	}
	void ChangePauseState(){
		if(pause){
			pauseObject.SetActive(true);
			UnlockCursor();
		}
		if(!pause){
			pauseObject.SetActive(false);
			LockCursor();
		}
	}
	public void LockCursor(){
	//	Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Debug.Log ("LockCursor");
	}

	public void UnlockCursor(){
	//	Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
	void OnApplicationFocus(bool focus){
		Debug.Log(focus);
		pause = !focus;
		ChangePauseState();
	}


}
