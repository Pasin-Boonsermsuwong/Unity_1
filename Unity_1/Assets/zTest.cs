using UnityEngine;
using System.Collections;

public class zTest : MonoBehaviour {
	void Start(){
		Random r = new Random();
		Random.seed = 16;
		Debug.Log (Random.value);
	}
	// Update is called once per frame
	void Update () {
		Debug.Log(ZoneData.getRandomZone(40.0f,55.0f));
	}
}
