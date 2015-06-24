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
	PlayerController playerController;
	Transform myTransform;
	Rigidbody rb;
	public int ID;
	public float fireRate;
	float nextFire;
	public float fireSpeedMin;
	public float fireSpeedMax;
	public GameObject shot;
	public float shotDeviation;
	public bool equipped;
	public float energyRequirement;
	public float shotAmount;

	//modifier from parent
	float dmgModifier;

	void Start () {
		myTransform = GetComponent<Transform>();
		playerController = GetComponentInParent<PlayerController>();
		fireRate = fireRate * playerController.rofModifier;
		dmgModifier = playerController.dmgAdder;
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
			if(energyRequirement>playerController.energy)return;
			playerController.energy -= energyRequirement;
			GetComponent<AudioSource>().Play();
			nextFire = Time.time + fireRate;
			for(int i = 0;i<shotAmount;i++){
				Vector3 eulerAngle = myTransform.rotation.eulerAngles;
				float shootSpeed;
				if(Mathf.Approximately(fireSpeedMax,fireSpeedMin)){
					shootSpeed = fireSpeedMin;
				}else{
					shootSpeed = Random.Range(fireSpeedMin,fireSpeedMax);
				}

				//Fire deviation
				GameObject instantiated = Instantiate(shot, myTransform.position, 
				                                      Quaternion.Euler(new Vector3(
					eulerAngle.x, 
					eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
					eulerAngle.z))) as GameObject;
				//Initial velocity
				instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
				//Fire speed
				instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*shootSpeed);
				instantiated.GetComponent<BoltCollision>().ActivateBulletMod(new BulletMod(dmgModifier));

				instantiated.tag = "BulletPlayer";
			}
		};
	}
}
