using UnityEngine;
using System.Collections;

public class RotateTurret : MonoBehaviour {

	//public Transform turretTransform;
	Transform cameraTransform;

	void Start () {
	//	turretTransform = GetComponent<Transform>();
		cameraTransform = transform.Find("Camera").transform;
	}

	void Update () {
		transform.rotation = cameraTransform.rotation;
	}
}
