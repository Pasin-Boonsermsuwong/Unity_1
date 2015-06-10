using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {
	//Ship stats
	public float torque = 300f;
	public float power = 200f;
	public float thrust;
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
		//Weapon
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
		//	Debug.Log (transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z);

			GameObject instantiated = Instantiate(shot, gun.position, 
			            Quaternion.Euler(new Vector3(
											transform.rotation.eulerAngles.x, 
											transform.rotation.eulerAngles.y+Random.Range(-shotDeviation,shotDeviation), 
											transform.rotation.eulerAngles.z))) as GameObject;
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
		};

		//Movement

		/*
		float thrust = Input.GetAxis ("Vertical") * power;

		thrust *= Time.deltaTime;

		if(thrust > 0.0f){
			rb.AddForce (transform.forward * thrust);
		}else{
			rb.AddForce (transform.forward * thrust * reverseFraction);
		};
*/
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition.y = 0;
		transform.LookAt(mousePosition);
		//Debug.Log(mousePosition.ToString());
		//rb.AddForce(Vector3.Lerp(transform.position, mousePosition,Time.deltaTime*thrust));
		rb.AddForce (transform.forward * thrust * 
		             Mathf.Clamp(Vector3.Distance (transform.position, mousePosition)-4,0,15)
		             );


	//	rb.AddTorque(transform.up * torque * rotation);
	/*
			float rotation = Input.GetAxis ("Horizontal") * torque;
			rotation *= Time.deltaTime;
			transform.Rotate (0, rotation, 0);
			*/
			rb.position = new Vector3(
			Mathf.Clamp(rb.position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z,boundary.zMin,boundary.zMax)
			);

		if(transform.position.x>boundary.xMax)rb.AddForce(new Vector3(-power/5,0,0));
		else if(transform.position.x<boundary.xMin)rb.AddForce(new Vector3(power/5,0,0));
		if(transform.position.z>boundary.zMax)rb.AddForce(new Vector3(0,0,-power/5));
		else if(transform.position.z<boundary.zMin)rb.AddForce(new Vector3(0,0,power/5));
	}

}
