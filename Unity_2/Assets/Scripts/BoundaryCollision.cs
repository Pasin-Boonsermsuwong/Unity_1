using UnityEngine;
using System.Collections;

public class BoundaryCollision : MonoBehaviour {

	void OnTriggerEnter(Collider other){

		if(other.tag == "Player"){
			Debug.Log ("Player hit boundary");
			other.transform.GetComponent<Health>().TakeDamage(50000);
		}else{
			Debug.Log (other.name+" hit boundary");
			Destroy(other.gameObject);
		}
	}
}
