using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	
	public Transform target;
	Transform myTransform;
	public float distanceToTarget;
	// Use this for initialization
	void Start () {
		myTransform = GetComponent<Transform>();
		target = GameObject.FindWithTag("Player").transform;
	}
	void Update(){
		if(target==null)return;
		distanceToTarget = Vector3.Distance(target.position,myTransform.position);
	//	Debug.Log(distanceToTarget);
	}
}
