using UnityEngine;
using System.Collections;

public class zTest : MonoBehaviour {
	void Start(){

	}
	// Update is called once per frame
	void Update () {
		Debug.Log(ZoneData.getRandomZone(40.0f,55.0f));
	}
}
