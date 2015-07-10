using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RotateTurret : NetworkBehaviour {

	[SyncVar] Quaternion syncPlayerRotation;
	[SerializeField] Transform turretTransform;
	[SerializeField] float lerpRate = 15;
	Transform camTransform;

	void Start () {
	//	turretTransform = transform;
		camTransform = transform.Find("Camera").transform;
	}

	void FixedUpdate () {
		if(isLocalPlayer)turretTransform.rotation = camTransform.rotation;
		LerpRotation();
		TransmitRotation();
	}
	void LerpRotation(){
		if(!isLocalPlayer)turretTransform.rotation = Quaternion.Lerp(turretTransform.rotation,syncPlayerRotation,Time.deltaTime*lerpRate);
	}
	[Command]
	void CmdProvideRotation(Quaternion playerRot){
		syncPlayerRotation = playerRot;
	}

	[Client]
	void TransmitRotation(){
		if(isLocalPlayer)CmdProvideRotation(turretTransform.rotation);
	}
}
