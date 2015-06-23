using UnityEngine;
using System.Collections;

public class RandomForce : MonoBehaviour {
	public float force;
	void Start () {
		GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * force);
	}
}
