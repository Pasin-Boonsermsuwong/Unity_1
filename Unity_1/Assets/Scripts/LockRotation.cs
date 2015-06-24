using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour {

	Transform myTransform;
	Quaternion q;
	void Start () {
		myTransform = transform;
		q = myTransform.rotation;
	}

	void Update () {
		myTransform.rotation = q;
	}
}
