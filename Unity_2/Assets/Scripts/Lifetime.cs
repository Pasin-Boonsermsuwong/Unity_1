using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Lifetime : NetworkBehaviour {

	public float lifetime;

	void Start () {
		Destroy(gameObject,lifetime);
	}

}
