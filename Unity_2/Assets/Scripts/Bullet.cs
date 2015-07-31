using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Bullet : NetworkBehaviour {
//	Transform myTransform;
	public ParticleSystem explosion;
	public ParticleSystem explosion2;
	public int damage;

	public string descriptiveName;
	[SyncVar]public string ownerName;	//FIERER'S NAME
	[SyncVar]public string ownerGun;	//FIERER'S WEAPON

	public bool explodeOnPlayerContact;	//GRENADE
	public float lifeTime;		

	public bool ignoreTerrain;
	public bool ignoreBullet;
	public bool isExplode;
	public float explodeRadius;
	public string specialTag;

	void Start(){
		if(!isServer)return;
		StartCoroutine(LifeTime(lifeTime));
	}
//	[Server]
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
	//	Debug.Log("Bullet hit: "+other.transform.name);
	//	Transform other.transform = other.transform;
		if(ignoreTerrain&&other.transform.tag=="Untagged" ||
		   other.transform.tag == "Bouncy" ||
		   ignoreBullet&&other.transform.tag=="Bullet"	||
		   !explodeOnPlayerContact&&other.transform.tag=="Player" 
		   )return;
		BulletHit(other);
	}
	IEnumerator LifeTime(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		if(isExplode){
			BulletHit(null);
		}else{
			NetworkServer.Destroy(this.gameObject);
		}
	}
	[Server]
	void BulletHit(Collision other){
		Instantiate(explosion, transform.position, transform.rotation);
		if(explosion2!=null)Instantiate(explosion2, transform.position, transform.rotation);
		RpcExplosion();
		if(isExplode){
			//EXPLOSIVE BULLET CALCULATION
			Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
			foreach (Collider col in objectsInRange) {
				if(col.tag == "Player"){
					float proximity = (transform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / explodeRadius);
					int calculatedDamage = Mathf.RoundToInt(damage * effect);
					col.GetComponent<Health>().TakeDamage(calculatedDamage,ownerName,ownerGun,specialTag);
				}
			}
		}else{
			//NORMAL BULLET HIT
			if(other!=null&&other.transform.tag=="Player"){
				other.transform.GetComponent<Health>().TakeDamage(damage,ownerName,ownerGun,specialTag);
			}
		}
		NetworkServer.Destroy(this.gameObject);//TODO:

	
	}
	[ClientRpc]
	void RpcExplosion(){
		Instantiate(explosion, transform.position, transform.rotation);
		if(explosion2!=null)Instantiate(explosion2, transform.position, transform.rotation);
	}



	/*
	[ClientRpc]
	void RpcDestroy(){
		Destroy(gameObject);
	}
	*/

}
