using UnityEngine;
using System.Collections;

public class BoltCollision : MonoBehaviour {
	public float damage = 100;
	//public AsteroidHealth sc;
	void OnTriggerEnter(Collider other){
		if(other.tag=="Enemy"){
			Destroy (gameObject);
			other.GetComponent<AsteroidHealth>().TakeDamage(damage);

		}
	}
}
