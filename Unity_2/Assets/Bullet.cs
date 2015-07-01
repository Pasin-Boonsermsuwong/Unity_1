using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public ParticleSystem explosion;
	public float damage;
	public string ownerName;
	//explosion
	public bool isExplode;
	public bool bounce;
	public float explodeRadius;
	void Start(){
	}
	void OnCollisionEnter(Collision other){
		Transform otherTransform = other.transform;
		if(otherTransform.tag=="Player"||!bounce){
			Instantiate(explosion, transform.position, transform.rotation);
			if(!isExplode){
				if(otherTransform.tag=="Player")otherTransform.GetComponent<Health>().TakeDamage(damage);
			}else{
				Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
				foreach (Collider col in objectsInRange) {
					if(col.tag == "Player"){
						float proximity = (transform.position - col.transform.position).magnitude; 
						float effect = 1 - (proximity / explodeRadius);
						float calculatedDamage = damage * effect;
						col.GetComponent<Health>().TakeDamage(calculatedDamage);
					}
				}
			}
			Destroy (gameObject);
		}
	}
}
