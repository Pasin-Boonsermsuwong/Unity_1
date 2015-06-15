using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {
	//Ship stats
	public float strafeForce = 100f;
	public float RotationSpeed = 300f;
	public float power = 200f;
	public float thrust;
	public float maxSpeed;
	public Rigidbody rb;
	public Boundary boundary;
	public float reverseFraction = 0.3f;
	private Vector3 mousePosition;
	//Ship weapons
	public float fireRate = 0.5f;
	public float nextFire = 0.0f;
	public GameObject shot;
	public Transform gun;
	public float shotDeviation = 5f;
	public float gunDamage = 200;
	//public GameObject bullet;
	void OnStart(){
		rb = GetComponent<Rigidbody>();
		if(rb==null)Debug.Log("Rigidbody is null");
	}

	void Update(){
		if(GameController.pause)return;
		Vector3 tf = transform.forward;
		Vector3 tp = transform.position;
		//////////WEAPON		

		if (Input.GetButton("Fire1") && Time.time > nextFire) {											
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			Vector3 eulerAngle = transform.rotation.eulerAngles;
			GameObject instantiated = Instantiate(shot, gun.position, 
			            Quaternion.Euler(new Vector3(
											eulerAngle.x, 
											eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
											eulerAngle.z))) as GameObject;
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
		};
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

	//	strafeDirection.x += (strafe>0)? 90:-90;
		rb.AddForce(Quaternion.Euler(0, 90, 0) * tf
	//	rb.AddForce(Quaternion.AngleAxis(-45, transform.forward)
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

}
