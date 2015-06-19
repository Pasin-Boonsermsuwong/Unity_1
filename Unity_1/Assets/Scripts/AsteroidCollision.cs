using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {
	public GameObject explosion;
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			other.GetComponent<HealthPlayer>().TakeDamage(this.GetComponent<Health>().curHP);
			GetComponent<Health>().Death();
		}
	}
}
