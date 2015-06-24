using UnityEngine;
using System.Collections;

public class BoltCollision : MonoBehaviour {
	public ParticleSystem explosion;
	public float baseDamage = 200;
	//explosion
	public bool isExplode;
	public float radius;
	public GameObject dmgText;
	void Start(){
	}
	void OnTriggerEnter(Collider other){
		if(tag=="BulletPlayer" && other.tag=="Enemy"){
			other.GetComponent<Health>().TakeDamage(baseDamage);
			Hit (other);
		}
		else if(tag=="BulletEnemy" && other.tag =="Player"){
			other.GetComponent<HealthPlayer>().TakeDamage(baseDamage);
			Hit (other);
		}
	}
	public void ActivateBulletMod(BulletMod g){
		baseDamage += g.dmgAdder;
	//	GetComponent<Rigidbody>().AddForce (transform.forward * g.speed);
	}
	void Hit(Collider other){
		Instantiate(explosion, transform.position, transform.rotation);
		if(!isExplode){
			dmgText = Instantiate(dmgText, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
			dmgText.GetComponent<TextMesh>().text = baseDamage + "";
			if(other.tag=="Enemy")other.GetComponent<Health>().TakeDamage(baseDamage);
			else if(other.tag=="Player")other.GetComponent<HealthPlayer>().TakeDamage(baseDamage);
			Destroy (gameObject);
		}else{
			Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius); 
			foreach (Collider col in objectsInRange) {
				if(col.tag == "Enemy"||col.tag == "Player"){
					float proximity = (transform.position - col.transform.position).magnitude; 
					float effect = 1 - (proximity / radius);
					float calculatedDamage = baseDamage * effect;
					dmgText = Instantiate(dmgText, col.transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
					dmgText.GetComponent<TextMesh>().text = Mathf.Round(calculatedDamage) + "";
					if(col.tag=="Enemy")col.GetComponent<Health>().TakeDamage(calculatedDamage);
					else if(col.tag=="Player")col.GetComponent<HealthPlayer>().TakeDamage(calculatedDamage);
				}
			}
			Destroy (gameObject);
		}
	}
}
