using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {


	public Transform parent;

	void Update() {
		transform.position = new Vector3(parent.position.x,parent.position.y+5,parent.position.z+5);
	//	transform.LookAt(cameraToLookAt.transform.position, -Vector3.up); 
		transform.rotation = Quaternion.Euler(90, 0, 0); 
	}
}
