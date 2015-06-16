using UnityEngine;
using System.Collections;

public class BulletMod{

	public float dmgAdder;

	public BulletMod(float dmgAdd){
		dmgAdder = dmgAdd;
	}
}
public class Gun : MonoBehaviour {

	Rigidbody rb;
	//Ship weapons
	public float fireRate = 0.5f;
	public float nextFire = 0.0f;
	public GameObject shot;
	public float shotDeviation = 5f;


	//modifier from parent
//	float rofModifier;
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
		if(GameController.pause)return;
		//////////WEAPON		
		
		if (Input.GetButton("Fire1") && Time.time > nextFire) {											
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			Vector3 eulerAngle = transform.rotation.eulerAngles;
			GameObject instantiated = Instantiate(shot, transform.position, 
			                                      Quaternion.Euler(new Vector3(
				eulerAngle.x, 
				eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
				eulerAngle.z))) as GameObject;
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
			instantiated.GetComponent<BoltCollision>().ActivateBulletMod(new BulletMod(dmgModifier));
		};

	}
}
