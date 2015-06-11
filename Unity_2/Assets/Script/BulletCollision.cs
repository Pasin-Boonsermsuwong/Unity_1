using UnityEngine;
using System.Collections;

public class BulletCollision : MonoBehaviour {
	public ParticleSystem explosion;

	void Start(){
		/*
		PlayerController p = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		damage = p.gunDamage;
		*/
	}

	void OnCollisionEnter2D(Collision2D other){
	//	Debug.Log("Collided");
		if(other.gameObject.tag=="Player"){
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
			other.gameObject.GetComponent<PlayerController>().TakeDamage(GetComponent<BulletData>().Damage);
		}
	}
}
