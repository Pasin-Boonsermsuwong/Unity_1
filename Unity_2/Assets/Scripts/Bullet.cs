using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
//	Transform myTransform;
	public ParticleSystem explosion;
	public int damage;

	public string descriptiveName;
	public string ownerName;
	//public NetworkInstanceId ownerID;
	//explosion

	public bool ignoreTerrain;
	public bool ignoreBullet;
	public bool isExplode;
	public float explodeRadius;
	
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
	//	Debug.Log("Bullet hit: "+other.transform.name);
		Transform otherTransform = other.transform;
		if(otherTransform.tag == "Bouncy"	||
		   ignoreTerrain&&otherTransform.tag=="Untagged" ||
		   ignoreBullet&&otherTransform.tag=="Bullet"
		   )return;
	//	BulletHit(otherTransform);
		Instantiate(explosion, transform.position, transform.rotation);
		RpcExplosion();
		Destroy(gameObject);//TODO:
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
	}

	[ClientRpc]
	void RpcExplosion(){
		Instantiate(explosion, transform.position, transform.rotation);
	}



	/*
	[ClientRpc]
	void RpcDestroy(){
		Destroy(gameObject);
	}
	*/

}
