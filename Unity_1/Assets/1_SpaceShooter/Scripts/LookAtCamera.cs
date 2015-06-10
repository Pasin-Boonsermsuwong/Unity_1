using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LookAtCamera : MonoBehaviour {

	public Image Fill;
	public Transform parent;
	void Start(){
		transform.position = new Vector3(parent.position.x,parent.position.y+5,parent.position.z+5);
		transform.rotation = Quaternion.Euler(90, 0, 0); 
	}
	void Update() {
		transform.position = new Vector3(parent.position.x,parent.position.y+5,parent.position.z+5);
		transform.rotation = Quaternion.Euler(90, 0, 0); 

	}
}
