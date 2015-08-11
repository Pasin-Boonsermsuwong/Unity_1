using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
public class SpectatorController : MonoBehaviour{

	public float horizontalSpd;
	public float verticalSpd;

	public Camera cam;
	public MouseLook mouseLook = new MouseLook();
	
	Rigidbody m_RigidBody;
	float m_YRotation;
	GameController gc;
	
	public Vector3 Velocity{
		get { return m_RigidBody.velocity; }
	}
	
	void Start(){
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		m_RigidBody = GetComponent<Rigidbody>();
		mouseLook.Init (transform, cam.transform);
	}
	
	void Update(){
		RotateView();
	}

	void FixedUpdate(){
		Vector3 input = GetInput();
	//	m_RigidBody.
		if (Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon || Mathf.Abs(input.z) > float.Epsilon) {
			// always move along the camera forward as it is the direction that it being aimed at
			//	Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
			Vector3 desiredMove = transform.forward*input.z + transform.right*input.x + transform.up * input.y;
			//	desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;
			desiredMove = desiredMove.normalized;
			desiredMove.x = desiredMove.x*horizontalSpd;
			desiredMove.z = desiredMove.z*horizontalSpd;
			desiredMove.y = desiredMove.y*verticalSpd;
		//	Debug.Log(desiredMove);
			transform.position += desiredMove;
		}
	}
	
	Vector3 GetInput(){
		if(gc.chatState)return new Vector3(0,0,0);

		float zMove = 0;
		if(Input.GetButton("Jump")){
			zMove = verticalSpd;
		}else if(Input.GetButton("Run")){
			zMove = -verticalSpd;
		}

		Vector3 input = new Vector3(
			Input.GetAxis("Horizontal"),
			zMove,
			Input.GetAxis("Vertical"));
		return input;
	}

	void RotateView(){
		//avoids the mouse looking if the game is effectively paused
		if ((Mathf.Abs(Time.timeScale) < float.Epsilon)) return;
		
		// get the rotation before it's changed
	//	float oldYRotation = transform.eulerAngles.y;
		
		mouseLook.LookRotation (transform, cam.transform);
	}

}