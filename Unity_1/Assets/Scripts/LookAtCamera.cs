using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	Transform parent;
	Transform myTransform;
	Quaternion rotation;
	public float yOffset;
	public float zOffset;

	void Start(){
		myTransform = GetComponent<Transform>();
		rotation = Quaternion.Euler(90, 0, 0); 
		parent = transform.root;
		myTransform.position = new Vector3(parent.position.x,parent.position.y+yOffset,parent.position.z+zOffset);
		myTransform.rotation = rotation;
	}
	void Update() {
		myTransform.position = new Vector3(parent.position.x,parent.position.y+yOffset,parent.position.z+zOffset);
		myTransform.rotation = rotation;

	}
}
