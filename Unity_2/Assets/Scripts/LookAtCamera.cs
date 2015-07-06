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

		GameObject c = GameObject.FindWithTag("MainCamera");
		if(c==null)return;
		cameraTransform = c.transform;
	}
	void Update() {
		myTransform.LookAt(cameraTransform);
	}
}
