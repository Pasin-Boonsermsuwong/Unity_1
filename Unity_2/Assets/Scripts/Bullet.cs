using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
//	Transform myTransform;
	public ParticleSystem explosion;
	public int damage;
	public string ownerName;
	//public NetworkInstanceId ownerID;
	//explosion

	public bool ignoreTerrain;
	public bool ignoreBullet;
	public bool isExplode;
	public float explodeRadius;
	/*
	void Start(){

		SphereCollider sph = GetComponent<SphereCollider>();
		if(sph == null){
			Debug.Log("Bullet collider is not sphere");
			return;
		}
		float radius = sph.radius;
		Debug.Log("Bullet start overlapLength: "+Physics.OverlapSphere(transform.position,radius).Length);
		//~(1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerLocal"))
		if(Physics.OverlapSphere(transform.position,radius).Length>2){	
			//IGNORE PLAYER/PLAYERLOCAL
			//BY DEFAULT WILL HIT TURRETCOLLIDER AND BULLET ITSELF, SO LENGTH = 2;
			Debug.Log("SpawnCollide");
			if(isServer)BulletHit(null);
		}
	}
	*/
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
	//	Debug.Log("Bullet hit: "+other.transform.name);
		Transform otherTransform = other.transform;
		if(otherTransform.tag == "Bouncy"	||
		   ignoreTerrain&&otherTransform.tag=="Untagged" ||
		   ignoreBullet&&otherTransform.tag=="Bullet"
		   )return;
		BulletHit(otherTransform);
	}
	void BulletHit(Transform otherTransform){
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
			if(otherTransform!=null && otherTransform.tag=="Player"){
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
