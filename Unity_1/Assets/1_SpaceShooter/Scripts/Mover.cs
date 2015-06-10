using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public float speed = 800;
	void Start () {
		GetComponent<Rigidbody>().AddForce (transform.forward * speed);
	}
}