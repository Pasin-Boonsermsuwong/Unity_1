using UnityEngine;
using System.Collections;

//Change player's modifier
public class ModifierChangeRequest{
	// ALL VALUE ARE ADDED
	public float rofModifier;
	public float dmgAdder;
	public ModifierChangeRequest(float rof,float dmg){
		rofModifier = rof;
		dmgAdder = dmg;
	}
}
[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {
	//Ship stats


	public string[] mountList = {"Weapon1","Weapon2"};
	public Vector3[] mountPosition;
	public float strafeForce = 100f;
	public float RotationSpeed = 300f;
	public float power = 200f;
	public float thrust;
	public float maxSpeed;
	public Rigidbody rb;
	public Boundary boundary;
	public float reverseFraction = 0.3f;
	Vector3 mousePosition;


	//WEAPONS MODIFIER
	public float rofModifier = 1;
	public float dmgAdder;


	void OnStart(){
		rb = GetComponent<Rigidbody>();
		if(rb==null)Debug.Log("Rigidbody is null");
		boundary = new Boundary();

		
	}

	void Update(){
		if(GameController.pause)return;
		Vector3 tf = transform.forward;
		Vector3 tp = transform.position;
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition.y = 0;
		////////ROTATION

		Vector3 _direction = (mousePosition - tp).normalized;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * RotationSpeed);

		////////THRUST

		rb.AddForce (tf * thrust * 
		             Mathf.Clamp(Vector3.Distance (tp, mousePosition)-4.5f,0,maxSpeed)
		             );
		float backwardThrust = Input.GetAxis("Vertical");
		if(backwardThrust<0){
			rb.AddForce(tf * thrust * backwardThrust * reverseFraction);
		}
		////////STRAFE

		rb.AddForce(Quaternion.Euler(0, 90, 0) * tf
		            * Input.GetAxis("Horizontal") * strafeForce * Time.deltaTime);

		////////BORDER
		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z,boundary.zMin,boundary.zMax)
		);
		if(tp.x>boundary.xMax)rb.AddForce(new Vector3(-power/5,0,0));
		else if(tp.x<boundary.xMin)rb.AddForce(new Vector3(power/5,0,0));
		if(tp.z>boundary.zMax)rb.AddForce(new Vector3(0,0,-power/5));
		else if(tp.z<boundary.zMin)rb.AddForce(new Vector3(0,0,power/5));
	}

	public void editModifier(ModifierChangeRequest m){
		dmgAdder += m.dmgAdder;
		rofModifier = Mathf.Max(rofModifier + m.rofModifier,0.1f);
		Gun[] gArray = GetComponentsInChildren<Gun>();
		foreach(Gun g in gArray){
			g.RefreshModifier();
		}
	}

	public void updateMount(){

	}
}
