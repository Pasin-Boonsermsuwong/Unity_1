using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
//	Transform myTransform;
	public ParticleSystem explosion;
	public int damage;
	public string ownerName;
	//explosion

	public bool ignoreTerrain;
	public bool ignoreBullet;
	public bool isExplode;
	public float explodeRadius;
	void Start(){
//		myTransform = transform;
	}
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
		Transform otherTransform = other.transform;
		if(otherTransform.tag == "Bouncy"	||
		   ignoreTerrain&&otherTransform.tag=="Untagged" ||
		   ignoreBullet&&otherTransform.tag=="Bullet"
		   )return;

		Instantiate(explosion, transform.position, transform.rotation);
//		RpcExplosion();
	//	Debug.Log ("N: "+otherTransform.name);
	//	Debug.Log ("T: "+otherTransform.tag);
		if(isExplode){
			//EXPLOSIVE BULLET CALCULATION
			Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
			foreach (Collider col in objectsInRange) {
				if(col.tag == "Player"){
					float proximity = (transform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / explodeRadius);
					int calculatedDamage = Mathf.RoundToInt(damage * effect);
					col.GetComponent<Health>().TakeDamage(calculatedDamage);
				}
			}
		}else{
			//NORMAL BULLET HIT
			if(otherTransform.tag=="Player"){
				otherTransform.GetComponent<Health>().TakeDamage(damage);
			}
		}
		Destroy (gameObject);
	}
	[ClientRpc]
	void RpcExplosion(){
		Instantiate(explosion, transform.position, transform.rotation);
	}
}
