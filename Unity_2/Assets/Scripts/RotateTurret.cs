using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RotateTurret : NetworkBehaviour {

	[SyncVar] Quaternion syncPlayerRotation;
	[SerializeField] Transform playerTransform;
	[SerializeField] float lerpRate = 15;
	Transform camTransform;

	void Start () {
		camTransform = transform.Find("Camera").transform;
	}

	void FixedUpdate () {
		transform.rotation = camTransform.rotation;
	}
	void LerpRotation(){
		if(!isLocalPlayer)playerTransform = Quaternion.Lerp(playerTransform.rotation,syncPlayerRotation,Time.deltaTime*lerpRate);
	}


}
