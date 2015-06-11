using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	GameObject p1;
	GameObject p2;
	public float Scale;
	Camera c;
	public static bool OnePlayerLeft = false;
	void Start () {
		p1 = GameObject.Find("Player1");
		p2 = GameObject.Find("Player2");
		c = GetComponent<Camera>();
	}

	void Update () {
		if(OnePlayerLeft){
			if(p1 == null){
				transform.position = p2.transform.position;
				c.orthographicSize = 3;
			}else{
				transform.position = p1.transform.position;
				c.orthographicSize = 3;
			}
		}else{
			Vector2 P1 = p1.transform.position;
			Vector2 P2 = p2.transform.position;
			transform.position = new Vector3((P1.x+P2.x)/2,(P1.y+P2.y)/2,-10);
			c.orthographicSize = Mathf.Max(1.5f,Vector2.Distance(P1,P2) * Scale);
		}
	}
}
