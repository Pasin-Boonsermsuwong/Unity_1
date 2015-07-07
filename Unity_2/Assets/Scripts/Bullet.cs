using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
	Transform myTransform;
	public ParticleSystem explosion;
	public float damage;
	public string ownerName;
	//explosion

	public bool bounce;
	public bool collideWithOtherBullet;

	public bool isExplode;
	public float explodeRadius;
	void Start(){
		myTransform = transform;
	}
	void OnCollisionEnter(Collision other){
		Transform otherTransform = other.transform;
		if(otherTransform.tag == "Untagged")return;
		if(bounce&&otherTransform.tag=="Terrain")return;
		if(collideWithOtherBullet&&otherTransform.tag=="Bullet")return;

		Instantiate(explosion, myTransform.position, myTransform.rotation);
		if(!isExplode){
			//EXPLOSIVE BULLET CALCULATION
			Collider[] objectsInRange = Physics.OverlapSphere(myTransform.position, explodeRadius); 
			foreach (Collider col in objectsInRange) {
				if(col.tag == "Player"){
					float proximity = (myTransform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / explodeRadius);
					float calculatedDamage = damage * effect;
					col.GetComponent<Health>().TakeDamage(calculatedDamage);
				}
			}
		}else{
			//NORMAL BULLET CALCULATION
			if(otherTransform.tag=="Player")otherTransform.GetComponent<Health>().TakeDamage(damage);
		}
		Destroy (gameObject);

	}
}
