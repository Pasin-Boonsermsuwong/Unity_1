using UnityEngine;
using System.Collections;

public class Hazard : MonoBehaviour {

	void OnCollisionEnter(Collision other){
		Debug.Log(other.transform.name);
		Transform otherTransform = other.transform;
		if(otherTransform.tag=="Player"){
			otherTransform.GetComponent<Health>().TakeDamage(10000);
		}
	}
}
