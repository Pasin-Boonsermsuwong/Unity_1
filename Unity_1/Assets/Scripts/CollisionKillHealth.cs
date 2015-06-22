using UnityEngine;
using System.Collections;

public class CollisionKillHealth : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			other.GetComponent<HealthPlayer>().TakeDamage(this.GetComponent<Health>().curHP);
			GetComponent<Health>().Death();
		}
	}
}
