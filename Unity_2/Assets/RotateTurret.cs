using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RotateTurret : NetworkBehaviour {

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
