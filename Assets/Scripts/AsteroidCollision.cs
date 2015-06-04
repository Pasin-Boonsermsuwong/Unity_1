using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {

	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Destroy (other.gameObject);
		}
	}
}
