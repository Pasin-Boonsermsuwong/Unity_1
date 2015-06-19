using UnityEngine;
using System.Collections;

public class BoundaryCollision : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if(other.tag=="Enemy"||other.tag=="Loot"){
			Destroy (other.gameObject);
		}
	}
}
