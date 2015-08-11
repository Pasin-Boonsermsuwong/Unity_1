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
	public float specialTagParameter;

	public float safety;		//SECONDS UNTIL BULLET CAN DEAL DAMAGE
	public bool safetyOn;

	void Start(){
		if(!isServer)return;
		if(safety>0){
			safetyOn = true;
			Invoke("DisableSafety",safety);
		}
		if(lifeTime>=0)StartCoroutine(LifeTime(lifeTime));
	}

	void DisableSafety(){
		safetyOn = false;
	}

	[ServerCallback]
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
		//Debug.Log("Bullethit: "+other.collider.tag);
		string tag1 = other.collider.tag;
		if(ignoreTerrain&&tag1=="Untagged" ||
		   tag1 == "Bouncy" ||
		   ignoreBullet&&tag1=="Bullet"	||
		   !explodeOnPlayerContact&&(tag1=="Player"||tag1=="Destructible") 
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
	public void BulletHit(Collision other){
		if(safetyOn)return;
		Instantiate(explosion, transform.position, Quaternion.identity);
		if(explosion2!=null)Instantiate(explosion2, transform.position, Quaternion.identity);
	//	RpcExplosion(transform.position);
		if(isExplode){
			//EXPLOSIVE BULLET CALCULATION
			Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
			foreach (Collider col in objectsInRange) {
				if(col.tag == "Player"){
					float proximity = (transform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / explodeRadius);
					int calculatedDamage = Mathf.RoundToInt(damage * effect);
					col.GetComponent<Health>().TakeDamage(calculatedDamage,ownerName,ownerGun,specialTag,specialTagParameter);
				}else if(col.tag == "Destructible"){
					float proximity = (transform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / explodeRadius);
					int calculatedDamage = Mathf.RoundToInt(damage * effect);
					col.GetComponent<HealthSimple>().TakeDamage(calculatedDamage,ownerName,ownerGun,specialTag,specialTagParameter);
				}
			}
		}else{
			//NORMAL BULLET HIT
			if(other!=null){
				if(other.transform.tag=="Player"){
					other.transform.GetComponent<Health>().TakeDamage(damage,ownerName,ownerGun,specialTag,specialTagParameter);
				}else if(other.transform.tag=="Destructible"){
					other.transform.GetComponent<HealthSimple>().TakeDamage(damage,ownerName,ownerGun,specialTag,specialTagParameter);
				}
			}
		}
		NetworkServer.Destroy(this.gameObject);//TODO:
	}

	public override void OnNetworkDestroy()
	{
		Instantiate(explosion, transform.position, Quaternion.identity);
		if(explosion2!=null)Instantiate(explosion2, transform.position, Quaternion.identity);
	}
	/*
	[ClientRpc]
	void RpcExplosion(Vector3 pos){
		Instantiate(explosion, pos, Quaternion.identity);
		if(explosion2!=null)Instantiate(explosion2, pos, Quaternion.identity);
	}
*/
}
