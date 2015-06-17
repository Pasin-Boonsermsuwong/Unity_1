using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {

	public GameObject itemPrefab;
	// Use this for initialization
	void Start () {
		InsertItem(0);
		InsertItem(0);
		InsertItem(0);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void InsertItem (int ID){
		GameObject instantiated = Instantiate(itemPrefab) as GameObject;
		instantiated.transform.SetParent(transform);
		instantiated.GetComponent<ItemController>().Setup(ID);
	}
}
