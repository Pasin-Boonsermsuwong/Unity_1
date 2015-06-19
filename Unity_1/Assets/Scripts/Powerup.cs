using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
	public GameObject powerupText;
	public string s;
	PlayerController plr;
	int type;
			/*
			0 = improve firerate
			1 = improve damage
			 */

	void Start(){
		GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		//Determine powerup type
		type = Random.Range(0,2);
		switch(type){
			case 0:
			GetComponent<Renderer>().material.color = Color.red;
			s = "Firerate increased!";
			break;
			case 1:
			GetComponent<Renderer>().material.color = Color.yellow;
			s = "Damage increased!";
			break;
		}
		//Add initial speed
		GetComponent<Rigidbody>().AddForce (transform.forward * 300f);
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 5;
	}
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			powerupText = Instantiate(powerupText, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
			powerupText.GetComponent<TextMesh>().text = s;
			Destroy (gameObject);
			plr = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
			switch(type){
			case 0:	//firerate
				plr.editModifier(new ModifierChangeRequest(-0.015f,0));
				break;
			case 1:	//damage
				plr.editModifier(new ModifierChangeRequest(0,30));
				break;
			}

		}
	}
}
