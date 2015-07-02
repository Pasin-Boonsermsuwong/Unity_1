using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerNetworkSetup : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		if(!base.isLocalPlayer)return;
		transform.Find("Camera").gameObject.SetActive(true);
		GetComponent<Gun>().enabled = true;
		GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
	}

}
