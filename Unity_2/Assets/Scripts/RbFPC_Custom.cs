﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
public class RbFPC_Custom : MonoBehaviour{
	[System.Serializable]
	public class MovementSettings
	{
		GameController gc;
		public float ForwardSpeed = 0;   // Speed when walking forward
		public float BackwardSpeed = 0;  // Speed when walking backwards
		public float StrafeSpeed = 0;    // Speed when walking sideways
		public float RunMultiplier = 0;   // Speed when sprinting
	//	public KeyCode RunKey = KeyCode.LeftShift;
		public float JumpForce = 0;
		public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
		[HideInInspector] public float CurrentTargetSpeed = 8f;

		public float RunMax = 50f;
		float RunCurrent;
		public float RunRegen = 0.125f;

		#if !MOBILE_INPUT
		bool m_Running;
		#endif
		public void SetRunData(){
			RunCurrent = RunMax;
			gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		}
		public void UpdateRunData(){
			RunCurrent = Mathf.Min(RunCurrent + RunRegen,RunMax);
			gc.localSliderRun.value = RunCurrent/RunMax;
		}
		public void UpdateDesiredTargetSpeed(Vector2 input)
		{
			if (input == Vector2.zero) return;
			if (input.x > 0 || input.x < 0)
			{
				//strafe
				CurrentTargetSpeed = StrafeSpeed;
			}
			if (input.y < 0)
			{
				//backwards
				CurrentTargetSpeed = BackwardSpeed;
			}
			if (input.y > 0)
			{
				//forwards
				//handled last as if strafing and moving forward at the same time forwards speed should take precedence
				CurrentTargetSpeed = ForwardSpeed;
			}
			#if !MOBILE_INPUT
			if (Input.GetButton("Run")&&RunCurrent>0)
			{
				CurrentTargetSpeed *= RunMultiplier;
				m_Running = true;
				RunCurrent--;
			}
			else
			{
				m_Running = false;
			}
			#endif
		}
		
		#if !MOBILE_INPUT
		public bool Running
		{
			get { return m_Running; }
		}
		#endif
	}
	
	
	[System.Serializable]
	public class AdvancedSettings
	{
		public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
		public float stickToGroundHelperDistance = 0.5f; // stops the character
		public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
		public bool airControl; // can the user control the direction that is being moved in the air
	}
	
	
	public Camera cam;
	public MovementSettings movementSettings = new MovementSettings();
	public MouseLook mouseLook = new MouseLook();
	public AdvancedSettings advancedSettings = new AdvancedSettings();


	
	Rigidbody m_RigidBody;
	CapsuleCollider m_Capsule;
	float m_YRotation;
	Vector3 m_GroundContactNormal;
	bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;
	GameController gc;
	public bool isDead;
	public bool isStun;
	float capsuleHeight;
	float capsuleRadius;


	public Vector3 Velocity
	{
		get { return m_RigidBody.velocity; }
	}
	
	public bool Grounded
	{
		get { return m_IsGrounded; }
	}
	
	public bool Jumping
	{
		get { return m_Jumping; }
	}
	
	public bool Running
	{
		get
		{
			#if !MOBILE_INPUT
			return movementSettings.Running;
			#else
			return false;
			#endif
		}
	}


	void Start()
	{
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		movementSettings.SetRunData();
		m_RigidBody = GetComponent<Rigidbody>();
		mouseLook.Init (transform, cam.transform);
		SetCapsuleSize();
	}
	public void SetCapsuleSize(){
		m_Capsule = GetComponent<CapsuleCollider>();
		capsuleHeight = m_Capsule.height * transform.localScale.x;
		capsuleRadius = m_Capsule.radius * transform.localScale.x;
	}
	
	void Update()
	{
		RotateView();
		if (!gc.chatState && Input.GetButtonDown("Jump") && !m_Jump)
		{
			m_Jump = true;
		}
	}
	
	
	void FixedUpdate()
	{
		movementSettings.UpdateRunData();
		GroundCheck();
		Vector2 input = GetInput();
		
		if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
		{
			// always move along the camera forward as it is the direction that it being aimed at
		//	Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
			Vector3 desiredMove = transform.forward*input.y + transform.right*input.x;
		//	desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;
			desiredMove = desiredMove.normalized;
			desiredMove.x = desiredMove.x*movementSettings.CurrentTargetSpeed;
			desiredMove.z = desiredMove.z*movementSettings.CurrentTargetSpeed;
			desiredMove.y = desiredMove.y*movementSettings.CurrentTargetSpeed;
			if (m_RigidBody.velocity.sqrMagnitude <
			    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
			{
				m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
			}
		}
		
		if (m_IsGrounded)
		{
			m_RigidBody.drag = 5f;
			
			if (m_Jump)
			{
				m_RigidBody.drag = 0f;
				m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
				m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
				m_Jumping = true;
			}
			
			if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
			{
				m_RigidBody.Sleep();
			}
		}
		else
		{
			m_RigidBody.drag = 0f;
			if (m_PreviouslyGrounded && !m_Jumping)
			{
				StickToGroundHelper();
			}
		}
		m_Jump = false;
	}
	
	
	float SlopeMultiplier()
	{
		float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
		return movementSettings.SlopeCurveModifier.Evaluate(angle);
	}
	
	
	void StickToGroundHelper()
	{
		RaycastHit hitInfo;
		if (Physics.SphereCast(transform.position, capsuleRadius, Vector3.down, out hitInfo,
		                       ((capsuleHeight/2f) - capsuleRadius) +
		                       advancedSettings.stickToGroundHelperDistance))
		{
			if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
			{
				m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
			}
		}
	}
	
	
	Vector2 GetInput()
	{
		if(gc.chatState||isDead||isStun)return new Vector2(0,0);
		Vector2 input = new Vector2
		{
			x = Input.GetAxis("Horizontal"),
			y = Input.GetAxis("Vertical")
		};
		movementSettings.UpdateDesiredTargetSpeed(input);
		return input;
	}
	
	
	void RotateView()
	{
		//avoids the mouse looking if the game is effectively paused
		if ((Mathf.Abs(Time.timeScale) < float.Epsilon)||isStun) return;
		
		// get the rotation before it's changed
		float oldYRotation = transform.eulerAngles.y;
		
		mouseLook.LookRotation (transform, cam.transform);
		
		if (m_IsGrounded || advancedSettings.airControl)
		{
			// Rotate the rigidbody velocity to match the new direction that the character is looking
			Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
			m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
		}
	}

	/// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
	void GroundCheck()
	{
		m_PreviouslyGrounded = m_IsGrounded;
		RaycastHit hitInfo;
		if (Physics.SphereCast(transform.position, capsuleRadius, Vector3.down, out hitInfo,
		                       ((capsuleHeight/2f) - capsuleRadius) + advancedSettings.groundCheckDistance))
		{
			m_IsGrounded = true;
			m_GroundContactNormal = hitInfo.normal;
		}
		else
		{
			m_IsGrounded = false;
			m_GroundContactNormal = Vector3.up;
		}
		if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
		{
			m_Jumping = false;
		}
	}

	public void Dash(){
		m_RigidBody.drag = 0f;
		m_Jumping = true;
	}
}