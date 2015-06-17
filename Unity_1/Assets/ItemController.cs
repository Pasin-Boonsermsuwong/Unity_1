using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ItemController : MonoBehaviour {
	//{"WEAPON1","item1","The basic weapon"}
	public int ID;
	void Start () {
	
	}
	public void Setup (int ID) {
		string[] s = ItemData.getItemInfo(ID);
		GetComponent<Draggable>().type = (Draggable.Slot)System.Enum.Parse( typeof( Draggable.Slot ), s[1]);
		GetComponent<Image>().sprite = Resources.Load<Sprite>(s[2]);
		GetComponent<Draggable>().infoText = s[3];
	}
}
