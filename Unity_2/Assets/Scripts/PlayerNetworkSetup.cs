using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkSetup : NetworkBehaviour {

	GameController gc;
	public GameObject canvas;

	// Use this for initialization
	void Start () {
		if(isLocalPlayer){
			gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
			transform.Find("Camera").gameObject.SetActive(true);
			GetComponent<RbFPC_Custom>().enabled = true;
			ChangeLayersRecursively(transform,"PlayerLocal");
			gc.playerSpawned = true;
			gc.LockCursor();
		}else{
			canvas.SetActive(true);
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
