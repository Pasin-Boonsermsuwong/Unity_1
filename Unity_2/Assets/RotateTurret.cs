using UnityEngine;
using System.Collections;

public class RotateTurret : MonoBehaviour {

	Transform myTransform;
	Transform cameraTransform;

	void Start () {
		myTransform = GetComponent<Transform>();
		cameraTransform = transform.root.Find("Camera").transform;
	}

	void Update () {
		myTransform.rotation = cameraTransform.rotation;
	}
}
