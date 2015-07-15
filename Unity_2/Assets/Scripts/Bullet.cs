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
		if(isLocalPlayer){
			GameObject obj = NetworkServer.FindLocalObject(ownerID);
			Physics.IgnoreCollision(GetComponent<Collider>(), obj.GetComponent<Collider>());
		}
	}
	*/
	//public override void OnStartClient() {
	void Awake(){

	}
	/*
	void OnTriggerExit(Collider other) {
		Debug.Log(other.name);
	}
	*/
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
		Debug.Log("Bullet hit: "+other.transform.name);
		Transform otherTransform = other.transform;
		if(otherTransform.tag == "Bouncy"	||
		   ignoreTerrain&&otherTransform.tag=="Untagged" ||
		   ignoreBullet&&otherTransform.tag=="Bullet"
		   )return;
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	//	RpcDestroy();
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

	}
	/*
	[ClientRpc]
	void RpcExplosion(){
		Instantiate(explosion, transform.position, transform.rotation);
	}
	*/


	/*
	[ClientRpc]
	void RpcDestroy(){
		Destroy(gameObject);
	}
	*/

}
