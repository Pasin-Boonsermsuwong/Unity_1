﻿using UnityEngine;
using System.Collections;

public class AISeeker : MonoBehaviour {

	Transform target;
	Transform myTransform;
	Rigidbody rb;
	public float rotationSpeed;
	public float moveSpeed;


	// Use this for initialization
	void Start () {
		target = GameObject.FindWithTag("Player").transform;
		myTransform = this.transform;
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(target==null)return;	
		Vector3 _direction = (target.position - myTransform.position).normalized;
		myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * rotationSpeed);
		//move towards the player
		rb.AddForce(myTransform.forward * Time.deltaTime * moveSpeed);
	//	rb.AddForce(myTransform.forward * Mathf.Clamp(Vector3.Distance (myTransform.position, target.position),0,moveSpeed) * Time.deltaTime);
	}
}