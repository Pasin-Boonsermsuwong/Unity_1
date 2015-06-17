using UnityEngine;
using System.Collections;

public class mountPlayerCaller : MonoBehaviour {
	PlayerController player;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
	}
	public void UpdateMount(){
		player.updateMount();
	}
}
