using UnityEngine;
using System.Collections;

public class Lifetime : MonoBehaviour {

	public float lifetime;

	void Start () {
		Destroy(gameObject,lifetime);
	}

}
