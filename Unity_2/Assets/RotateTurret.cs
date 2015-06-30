using UnityEngine;
using System.Collections;

public class RotateTurret : MonoBehaviour {

	Transform myTransform;
	Transform cameraTransform;

	void Start () {
		myTransform = GetComponent<Transform>();
		cameraTransform = myTransform.root.Find("FirstPersonCharacter");
	}

	void Update () {
		myTransform.rotation = cameraTransform.rotation;
	}
}
