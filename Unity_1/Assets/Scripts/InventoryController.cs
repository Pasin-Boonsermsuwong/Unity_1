using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour {

	public GameObject itemPrefab;
	void Start () {

		//test
		InsertItem(0);
		InsertItem(0);
		InsertItem(1);
		InsertItem(1);
		InsertItem(2);
		InsertItem(2);
		InsertItem(3);
		InsertItem(3);
		InsertItem(4);
		InsertItem(4);
	}

	public void InsertItem (int ID){
		GameObject instantiated = Instantiate(itemPrefab) as GameObject;
		instantiated.transform.SetParent(transform);
		instantiated.GetComponent<ItemController>().Setup(ID);
	}
}
