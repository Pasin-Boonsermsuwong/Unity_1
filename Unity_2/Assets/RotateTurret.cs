using UnityEngine;
using System.Collections;

public class RotateTurret : MonoBehaviour {

	Transform myTransform;
//	Transform rootTransform;
	Transform cameraTransform;

	void Start () {
		myTransform = GetComponent<Transform>();
	//	rootTransform = ;
		cameraTransform = myTransform.root.Find("FirstPersonCharacter");
	}

	void Update () {
		myTransform.rotation = cameraTransform.rotation;
			//Quaternion.Euler(new Vector3(rootTransform.eulerAngles.x,cameraTransform.rotation.eulerAngles.y,rootTransform.eulerAngles.z));
	}
}
