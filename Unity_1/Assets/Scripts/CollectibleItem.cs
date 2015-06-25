using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CollectibleItem : MonoBehaviour {
	public GameObject powerupText;
	public string s;
	PlayerController plr;
	int ID;


	void Start(){

	}
	public void Set(int ID){
		this.ID = ID;
		s = ItemData.getItemInfo(ID)[4];
	}
	void OnTriggerEnter(Collider other){
		if(other.tag=="Player"){
			powerupText = Instantiate(powerupText, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
			powerupText.GetComponent<TextMesh>().text = s;
			Destroy (gameObject);

			//gamecontroller insert item to inventory
			GameController g = GameObject.FindWithTag("GameController").GetComponent<GameController>();
			g.insertItem(ID);
		}
	}
}