using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using Random = UnityEngine.Random;

public class FirstPersonControllerForce : MonoBehaviour{
		
	MouseLook m_MouseLook;
	Camera cam;
	Transform camTransform;
	Rigidbody rb;

	bool isWalking;

	Vector3 moveDir;

	public float walkSpeed;
	public float runSpeed;
	float speed;


    // Use this for initialization
     void Start(){
		cam = GetComponentInChildren<Camera>();
		camTransform = cam.GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		m_MouseLook.Init(transform , cam.transform);
    }

     void Update(){
		RotateView();
    }

     void PlayLandingSound(){

    }

     void FixedUpdate(){
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		bool waswalking = isWalking;
		isWalking = !Input.GetButton("Walk");
		speed = isWalking ? walkSpeed : runSpeed;



		moveDir = new Vector3(horizontal, 0f ,vertical);

		if (moveDir.sqrMagnitude > 1){
			moveDir.Normalize();
		}
		rb.AddForce(moveDir);
    }

    void PlayJumpSound(){

    }

    void RotateView(){
		m_MouseLook.LookRotation (transform, camTransform);
    }


}
