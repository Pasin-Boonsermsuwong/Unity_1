using UnityEngine;
using System.Collections;

public class SpawnPosition : MonoBehaviour {
	Transform myTransform;
	Vector3 rayDirection;
	void Start(){
		rayDirection = transform.FindChild("RayDir").transform.forward;
		myTransform = transform;
//		StartCoroutine("SpawnTest");
	}
	public void ChangeSpawnPosition(){
		RaycastHit hitPos;
		Vector3 randomPos =  new Vector3(Random.Range(-150,150),200,Random.Range(-150,150));
		Physics.Raycast(randomPos,rayDirection,out hitPos,400);
	//	Debug.Log(hitPos.point);
//		Debug.Log("ChangeSpawnPosition");
		myTransform.position = hitPos.point + new Vector3(0,10,0);
	}

	IEnumerator SpawnTest(){
		while(true){
			yield return new WaitForSeconds(3);
			ChangeSpawnPosition();
		}
	}
}
