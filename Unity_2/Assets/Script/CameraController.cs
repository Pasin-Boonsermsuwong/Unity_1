using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject p1;
	GameObject p2;
	public float Scale;
	Camera c;
//	public static bool OnePlayerLeft = false;
	void Start () {
		p1 = GameObject.Find("Player1");
		p2 = GameObject.Find("Player2");
		c = GetComponent<Camera>();
	}

	void Update () {
			if(p1 == null){
				transform.position = new Vector3(p2.transform.position.x,p2.transform.position.y,-10);
				c.orthographicSize = 1.5f;
			}else if (p2 == null){
				transform.position = new Vector3(p1.transform.position.x,p1.transform.position.y,-10);
				c.orthographicSize = 1.5f;
		}else{
			Vector2 P1 = p1.transform.position;
			Vector2 P2 = p2.transform.position;
			transform.position = new Vector3((P1.x+P2.x)/2,(P1.y+P2.y)/2,-10);
			c.orthographicSize = Mathf.Max(1.5f,Vector2.Distance(P1,P2) * Scale);
		}
	}
}
