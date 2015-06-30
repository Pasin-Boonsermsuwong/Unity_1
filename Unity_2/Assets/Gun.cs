using UnityEngine;
using System.Collections;

public class BulletMod{

	public float dmgAdder;
	public BulletMod(float dmgAdd){
		dmgAdder = dmgAdd;
	}
}
public class Gun : MonoBehaviour {
//Transform myTransform;
	public Transform gunHardpoint;
	Rigidbody rb;
	AudioSource audio;
	string ownerName;
	public float fireRate;
	float nextFire;
	public GameObject shot;
	public float launchSpeed;
	public float shotDeviation;
	public float shotAmount;
	
	//modifier from parent
	float dmgModifier;

	void Start () {
	//	myTransform = GetComponent<Transform>();
		rb = GetComponentInParent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		ownerName = GetComponentInParent<Health>().ownerName;
	}

	void Update () {
		if (Input.GetButton("Fire1") && Time.time > nextFire) {	
			if(audio!=null)audio.Play();
			nextFire = Time.time + fireRate;
			for(int i = 0;i<shotAmount;i++){


				GameObject instantiated;
				if(shotDeviation<double.Epsilon){
					instantiated = Instantiate(shot, gunHardpoint.position, 
					                           gunHardpoint.rotation) as GameObject;
				}else{
					Vector3 eulerAngle = gunHardpoint.rotation.eulerAngles;
					instantiated = Instantiate(shot, gunHardpoint.position, 
					                                      Quaternion.Euler(new Vector3(
						eulerAngle.x, 
						eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
						eulerAngle.z))) as GameObject;
				}

				instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
				instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*launchSpeed);
				instantiated.GetComponent<Bullet>().ownerName = ownerName;
			}
		};
	}
}
