using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkSetup : NetworkBehaviour {

	GameController gc;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		float currentClass = GetComponent<PlayerID>().currentClass;
		if(isLocalPlayer){
			Debug.Log("PlayerNetworkSetup: "+currentClass);
			if(Mathf.Approximately(currentClass,200)){
				Debug.Log("SpectatorModeSetup");
				gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
				transform.Find("Camera").gameObject.SetActive(true);
				GetComponent<SpectatorController>().enabled = true;
			//	ChangeLayersRecursively(transform,"PlayerLocal");
				gc.playerSpawned = true;
				gc.LockCursor();
			}else{
				gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
				transform.Find("Camera").gameObject.SetActive(true);
				GetComponent<RbFPC_Custom>().enabled = true;
				ChangeLayersRecursively(transform,"PlayerLocal");
				gc.playerSpawned = true;
				gc.LockCursor();
			}
		}else{
			if(canvas!=null)canvas.SetActive(true);
		}
	}
	public void ChangeLayersRecursively(Transform trans, string name)
	{
		if(trans.gameObject.layer == LayerMask.NameToLayer("Player"))trans.gameObject.layer = LayerMask.NameToLayer(name);
		foreach(Transform child in trans){            
			ChangeLayersRecursively(child,name);
		}
	}
}
