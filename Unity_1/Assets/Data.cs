using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Data : MonoBehaviour {

}

[System.Serializable]
public class ShipData{
	public int ID;
	public string[] mountList;
	public Draggable.Slot[] dataList;
	public float strafeForce;
	public float RotationSpeed;
	public float power;
	public float thrust;
	public float maxSpeed;
	public float reverseFraction;

	public ShipData(){

	}


}

[System.Serializable]
public class ItemData{
	/*
	//VISUAL
	public Draggable.Slot type;
	public Sprite image;
	public string infoCard;

	//INFO Weapons
	public float fireRate;
	public float fireSpeed;
	public GameObject shot;
	public float shotDeviation;
*/

	public static string[][] itemInfo = {
		new string[]{"1","WEAPON1","item1","The basic weapon"}

	};
	public static string[][] info = {
		new string[]{"1","0.3","800","shotBolt","5"}
	};
	public static string[] getItemInfo(int ID){
		return itemInfo[ID];
	}
	public static string[] getInfo(int ID){
		return info[ID];
	}
}