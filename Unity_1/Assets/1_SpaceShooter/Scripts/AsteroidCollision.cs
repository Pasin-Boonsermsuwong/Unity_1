using UnityEngine;
using System.Collections;

public class AsteroidCollision : MonoBehaviour {
	public GameObject explosion;
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			Instantiate(explosion, other.transform.position, other.transform.rotation);
			Destroy (other.gameObject);
			GameObject.FindWithTag("GameController").GetComponent<GameController>().GameOver();
		}
	}
}
