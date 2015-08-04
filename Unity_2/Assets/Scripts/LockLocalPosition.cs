using UnityEngine;
using System.Collections;

public class LockLocalPosition : MonoBehaviour {

	public float x;
	public float y;
	public float z;
//	Vector3 pos;
	Transform rootTransform;

	void Start(){
		Debug.Log("LockLocalPosition start");
	//	pos = new Vector3(x,y,z);
		rootTransform = transform.root;
	}

	void Update () {
		transform.position = new Vector3(rootTransform.position.x+x,rootTransform.position.y+y,rootTransform.position.z+z);
	}
}
