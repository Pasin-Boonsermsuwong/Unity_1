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

				//ID, Draggable.type, sprite name, infocard
	public static string[][] itemInfo = {
		new string[]{"1","WEAPON1","item1","The basic weapon"}

	};
			//ID , firerate, speed, bullet prefab name, shotDeviation
	public static string[][] info = {
		new string[]{"1","0.3","800","shotBolt","5"}
	};
	public static string[] getItemInfo(int ID){		//When instantiate eqiupped weapon
		return itemInfo[ID];
	}
	public static string[] getInfo(int ID){			//When instantiate item
		return info[ID];
	}
}

[System.Serializable]
public class ZoneData{
	//Zone ID, difficulty requirement, chance, enemy1 (prefab name), enemy2, enemy3, enemy4, (relative) freq1, freq2, freq3, freq4
	public static string[][] info = {
		new string[]{"1","0","10","Asteroid",null,null,null,"1","0","0","0"},
		
	};
	public static string[] getInfo(int ID){
		return info[ID];
	}
}

public class Library{
	//return
	public static int weightedRandom(float[] weight){

	}
}




