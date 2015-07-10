using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RotateTurret : NetworkBehaviour {

	[SyncVar] Quaternion syncPlayerRotation;
	[SerializeField] Transform turretTransform;
	[SerializeField] float lerpRate = 15;
	Transform camTransform;
	Quaternion lastRot;
	public float thresholdAngle = 1;
	void Start () {

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
	//	Debug.Log("CmdProvideRotation");
	}

	[Client]
	void TransmitRotation(){
		if(isLocalPlayer&&
		   Quaternion.Angle(lastRot,turretTransform.rotation)>thresholdAngle){
			CmdProvideRotation(turretTransform.rotation);
			lastRot = turretTransform.rotation;
		}
			
	}
}
