using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MineTrigger : NetworkBehaviour {

	Bullet bullet;
	// Use this for initialization
	void Start () {
		bullet = GetComponentInParent<Bullet>();
	}
	[ServerCallback]
	void OnTriggerStay(Collider other){
		if(other.tag == "Player"){
		//	Debug.Log("Player triggered mine");
			bullet.BulletHit(null);
		}
	}
}
