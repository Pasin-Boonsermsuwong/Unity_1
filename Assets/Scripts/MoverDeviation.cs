using UnityEngine;
using System.Collections;

public class MoverDeviation : MonoBehaviour {
	public float speed;
	public float deviation;
	void Start () {
		GetComponent<Rigidbody>().AddForce (transform.forward * (speed+Random.Range(-deviation,deviation)));
	}
}
