using UnityEngine;
using System.Collections;

public class SpawnPosition : MonoBehaviour {
	Transform myTransform;
	void Start(){
		myTransform = transform;
	}
	public void ChangeSpawnPosition(){
		Debug.Log("ChangeSpawnPosition");
		myTransform.position = new Vector3(Random.Range(-150,150),40,Random.Range(-150,150));
	}
}
