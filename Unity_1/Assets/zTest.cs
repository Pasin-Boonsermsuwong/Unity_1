using UnityEngine;
using System.Collections;

public class zTest : MonoBehaviour {
	void Start(){
		Random r = new Random();
		Random.seed = 16*4*4;
		Debug.Log (Random.value);
		Random.seed = 999;
		Debug.Log (Random.value);
		Random.seed = 16*4*4;
		Debug.Log (Random.value);
	}
	// Update is called once per frame
	void Update () {
		Debug.Log(ZoneData.getRandomZone(40.0f,55.0f));
	}
}
