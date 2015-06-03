using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {

	public float torque = 300f;
	public float power = 2000f;
	public Rigidbody rb;
	public Boundary boundary;
	public float reverseFraction = 0.3f;

	public float fireRate = 0.5f;
	public float nextFire = 0.0f;
	public GameObject shot;
	public Transform gun;
	public float shotDeviation = 5f;

	void OnStart(){
		rb = GetComponent<Rigidbody>();
		if(rb==null)Debug.Log("Rigidbody is null");
	}

	void Update(){
		//Weapon
		if (Input.GetButton("Fire1") && Time.time > nextFire) {
			nextFire = Time.time + fireRate;
		//	Debug.Log (transform.rotation.x + " " + transform.rotation.y + " " + transform.rotation.z);

			Instantiate(shot, gun.position, 
			            Quaternion.Euler(new Vector3(
											transform.rotation.eulerAngles.x, 
											transform.rotation.eulerAngles.y+Random.Range(-shotDeviation,shotDeviation), 
											transform.rotation.eulerAngles.z)));
		};
		//Movement
		float thrust = Input.GetAxis ("Vertical") * power;
		float rotation = Input.GetAxis ("Horizontal") * torque;
		thrust *= Time.deltaTime;
		rotation *= Time.deltaTime;
		if(thrust > 0.0f){
			rb.AddForce (transform.forward * thrust);
		}else{
			rb.AddForce (transform.forward * thrust * reverseFraction);
		};
		transform.Rotate (0, rotation, 0);
	//	rb.AddTorque(transform.up * torque * rotation);

		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z,boundary.zMin,boundary.zMax)
			);

		if(transform.position.x>boundary.xMax)rb.AddForce(new Vector3(-power/5,0,0));
		if(transform.position.x<boundary.xMin)rb.AddForce(new Vector3(power/5,0,0));
		if(transform.position.z>boundary.zMax)rb.AddForce(new Vector3(0,0,-power/5));
		if(transform.position.z<boundary.zMin)rb.AddForce(new Vector3(0,0,power/5));
	}

}
