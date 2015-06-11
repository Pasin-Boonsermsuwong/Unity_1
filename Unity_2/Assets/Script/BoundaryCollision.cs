using UnityEngine;

public class BoundaryCollision : MonoBehaviour {

	void OnTriggerExit2D(Collider2D other){
//		Debug.Log("Exit Boundary");
		Destroy(other.gameObject);
	}
}
