using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MineTrigger : NetworkBehaviour {

	Bullet bullet;
	bool alreadyTriggered;
	// Use this for initialization
	void Start () {
		bullet = GetComponentInParent<Bullet>();
	}
	[ServerCallback]
	void OnTriggerStay(Collider other){
		if(other.tag == "Player"&&!bullet.safetyOn&&!alreadyTriggered){
		//	Debug.Log("Player triggered mine");
			bullet.BulletHit(null);
			alreadyTriggered = true;
		}
	}
}
