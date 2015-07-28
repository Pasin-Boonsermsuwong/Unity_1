using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BulletPhraser : NetworkBehaviour {

	public float lifeTime;		
	
	void Start(){
		if(!isServer)return;
		StartCoroutine(LifeTime(lifeTime));
	}
	//	[Server]
	void OnCollisionEnter(Collision other){
		if(!isServer)return;
		BulletHit(other);
	}
	IEnumerator LifeTime(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		NetworkServer.Destroy(this.gameObject);
	}
	[Server]
	void BulletHit(Collision other){
		if(other!=null&&other.transform.tag=="Bullet"){
			NetworkServer.Destroy(other.gameObject);
		}
		//NetworkServer.Destroy(this.gameObject);//TODO:	
	}
}
