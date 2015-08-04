using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BulletSticky : NetworkBehaviour {

	Rigidbody rb;
	bool stuck;
	bool stuckToPlayer;
	Vector3 stuckPos;

	void Start () {
		rb = GetComponent<Rigidbody>();
	}

	void Update(){
		if(stuck||stuckToPlayer){
			rb.velocity = Vector3.zero;
		}
		if(stuckToPlayer){
			transform.localPosition = stuckPos;
		}
	}
	[ServerCallback]
	void OnCollisionEnter(Collision other) {
	//	Debug.Log("OnCollisionEnter");
		if(other.transform.tag == "Player"){
			if(stuckToPlayer)return;
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			stuckToPlayer = true;
			transform.parent = other.transform;
			stuckPos = transform.localPosition;
		}else if(other.transform.tag == "Untagged"){
			if(stuck)return;
			rb.useGravity = false;
			rb.velocity = Vector3.zero;
			stuck = true;
		}
	}
}
