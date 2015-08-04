using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {
	bool reset;
	void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
	void OnLevelWasLoaded(int level) {
		if(level == 1){
			reset = true;
		}
		else if (level == 0 && reset){
			Cursor.lockState = CursorLockMode.None;
			Destroy(gameObject);
		}
	}
}
