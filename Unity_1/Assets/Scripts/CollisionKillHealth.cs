using UnityEngine;
using System.Collections;

public class CollisionKillHealth : MonoBehaviour {
	public GameObject dmgText;
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			float dmg = Mathf.Min(GetComponent<Health>().curHP,other.GetComponent<HealthPlayer>().curHP);
			GetComponent<Health>().TakeDamage(dmg);
			other.GetComponent<HealthPlayer>().TakeDamage(dmg);
			dmgText = Instantiate(dmgText, other.transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
			dmgText.GetComponent<TextMesh>().text = Mathf.Round(dmg) + "";

		}
	}
}
