using UnityEngine;
using System.Collections;

public class CameraTrackPlayer : MonoBehaviour {

	public float y;
	GameObject player;
	void Start () {
		player = GameObject.FindWithTag("Player");
	}
	

	void Update () {
		if(player!=null)transform.position = new Vector3(player.transform.position.x,y,player.transform.position.z);
	}
}
