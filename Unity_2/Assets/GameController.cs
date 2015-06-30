using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	
	bool playerSpawned {get; set;}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LockCursor(){
		//Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void UnlockCursor(){
		//Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}


}
