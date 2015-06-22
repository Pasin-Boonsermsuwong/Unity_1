﻿using UnityEngine;
using System.Collections;

public class AIGun : MonoBehaviour {
	//represents gun hardpoint on ship
	Transform myTransform;
	Rigidbody rb;
	public float fireSpeed;
	public GameObject shot;
	public float shotDeviation;
	public float minCooldown;
	public float maxCooldown;
	public float engageRange;
	AIController AI;

	//modifier from parent
	float dmgModifier;
	
	void Start () {
		AI = GetComponentInParent<AIController>();
		myTransform = GetComponent<Transform>();
		rb = GetComponentInParent<Rigidbody>();
		StartCoroutine (FireGun());
	}
	
	void Fire(){
		if(AI.distanceToTarget>engageRange)return;
		GetComponent<AudioSource>().Play();
		Vector3 eulerAngle = myTransform.rotation.eulerAngles;
		//Fire deviation
		GameObject instantiated = Instantiate(shot, myTransform.position, 
		                                      Quaternion.Euler(new Vector3(
			eulerAngle.x, 
			eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
			eulerAngle.z))) as GameObject;
		//Initial velocity
		instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
		//Fire speed
		instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*fireSpeed);
	}

	IEnumerator FireGun ()
	{
		while (true)
		{
			yield return new WaitForSeconds (Random.Range(minCooldown,maxCooldown));
			Fire ();
			
		}
	}

	
}
