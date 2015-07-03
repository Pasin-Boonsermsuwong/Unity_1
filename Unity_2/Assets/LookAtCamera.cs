using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	Transform parent;
	Transform myTransform;
	Quaternion rotation;
	Transform cameraTransform;
//	public float yOffset;
//	public float zOffset;

	void Start(){
		myTransform = GetComponent<Transform>();
		 

		cameraTransform = GameObject.FindWithTag("MainCamera").transform;
	//	cameraTransform = transform.Find("Camera");


	}
	void Update() {
	//	myTransform.position = new Vector3(parent.position.x,parent.position.y+yOffset,parent.position.z);
		myTransform.LookAt(cameraTransform);

	}
}
