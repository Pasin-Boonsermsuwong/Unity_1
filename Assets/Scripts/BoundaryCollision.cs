using UnityEngine;
using System.Collections;

public class BoundaryCollision : MonoBehaviour {

	void OnTriggerExit(Collider other){
		if(other.tag=="Enemy"){
			Destroy (other.gameObject);
		}
	}
}
