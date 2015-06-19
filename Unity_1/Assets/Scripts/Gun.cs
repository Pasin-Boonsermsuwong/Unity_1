using UnityEngine;
using System.Collections;

public class BulletMod{

	public float dmgAdder;
	public BulletMod(float dmgAdd){
		dmgAdder = dmgAdd;
	}
}
public class Gun : MonoBehaviour {
//represents gun hardpoint on ship
	Rigidbody rb;

	public int ID;
	public float fireRate = 0.5f;
	float nextFire;
	public float fireSpeed;
	public GameObject shot;
	public float shotDeviation = 5f;
	public bool equipped;
	public float energyRequirement;

	//modifier from parent
	float dmgModifier;

	void Start () {
		PlayerController p = GetComponentInParent<PlayerController>();
		fireRate = fireRate * p.rofModifier;
		dmgModifier = p.dmgAdder;
		rb = GetComponentInParent<Rigidbody>();
	}

	public void RefreshModifier(){
		PlayerController p = GetComponentInParent<PlayerController>();
		fireRate = fireRate * p.rofModifier;
		dmgModifier = p.dmgAdder;

	}

	void Update () {
		if(GameController.pause||!equipped)return;

		if (Input.GetButton("Fire1") && Time.time > nextFire) {											
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			Vector3 eulerAngle = transform.rotation.eulerAngles;
			//Fire deviation
			GameObject instantiated = Instantiate(shot, transform.position, 
			                                      Quaternion.Euler(new Vector3(
				eulerAngle.x, 
				eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
				eulerAngle.z))) as GameObject;
			//Initial velocity
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
			//Fire speed
			instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*fireSpeed);
			instantiated.GetComponent<BoltCollision>().ActivateBulletMod(new BulletMod(dmgModifier));
		};

	}

}
