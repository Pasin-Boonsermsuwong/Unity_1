using UnityEngine;
using System.Collections;

public class zTest : MonoBehaviour {
	void Start(){
		/*
		Random.seed = 100;
		for(int i=0;i<10;i++){
Debug.Log(Random.value);
		}
*/
	}
	// Update is called once per frame
	void Update () {
		Debug.Log(ZoneData.getRandomZone(40.0f,55.0f));
	}
}
