using UnityEngine;
using System.Collections;

public class CameraTrackPlayer : MonoBehaviour {

	public float y;
	Transform player;
	Transform myTransform;
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		myTransform = GetComponent<Transform>();
	}

	void Update () {
		if(player!=null)myTransform.position = new Vector3(player.position.x,y,player.position.z);
	}
}