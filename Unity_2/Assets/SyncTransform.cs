using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SyncTransform : NetworkBehaviour {
	
	[SyncVar] Vector3 syncPos;
	[SerializeField] Transform myTransform;
	[SerializeField] float lerpRate = 15;
	[SerializeField] float threshold = 0.5f;
	Vector3 lastPos;
	void Start () {	
		if(myTransform==null)myTransform = transform;
	}
	
	void FixedUpdate () {
		LerpRotation();
		TransmitPos();
	}
	void LerpRotation(){
		if(!isLocalPlayer)myTransform.position = Vector3.Lerp(myTransform.position,syncPos,Time.deltaTime*lerpRate);
	}

	[Command]
	void CmdProvidePos(Vector3 playerPos){
		syncPos = playerPos;
//		Debug.Log("CmdProvidePos");
	}
	
	[Client]
	void TransmitPos(){	
		if(isLocalPlayer && Vector3.Distance(myTransform.position,lastPos)>threshold){
			CmdProvidePos(myTransform.position);
			lastPos = myTransform.position;
		}
	}
}
