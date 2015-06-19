using UnityEngine;
using System.Collections;

public class BoltCollision : MonoBehaviour {
	public ParticleSystem explosion;
	public GameObject dmgText;
	public float baseDamage = 200;
	//public AsteroidHealth sc;
	void Start(){
//		PlayerController p = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
//		damage = p.gunDamage;
	}
	void OnTriggerEnter(Collider other){
		if(other.tag=="Enemy"){
			Instantiate(explosion, transform.position, transform.rotation);
			Destroy (gameObject);
			other.GetComponent<Health>().TakeDamage(baseDamage);
			dmgText = Instantiate(dmgText, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
			dmgText.GetComponent<TextMesh>().text = baseDamage + "";
		}
	}
	public void ActivateBulletMod(BulletMod g){
		baseDamage += g.dmgAdder;
	//	GetComponent<Rigidbody>().AddForce (transform.forward * g.speed);
	}
}
