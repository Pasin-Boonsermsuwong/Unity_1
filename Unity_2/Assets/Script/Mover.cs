using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {
	public bool facingRight;
	public float speed = 800;
	void Start () {
		if(facingRight){
			Flip();
		}
//		Debug.Log("Start");
		GetComponent<Rigidbody2D>().AddForce((facingRight)?transform.right * speed:-transform.right * speed);
	}
	void Flip(){
		Vector3 v = transform.localScale;
		v.x = v.x*-1;
		transform.localScale = v;
	}
}