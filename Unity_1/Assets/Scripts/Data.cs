using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
	//Type of enemies 
	//Zone ID, BG material name ,enemy1 (prefab name), enemy2, enemy3, enemy4, (ABSOLUTE 0<x<1) freq1, freq2, freq3, freq4
	public static string[][] zoneEnemyInfo = {
		new string[]{"1","Asteroid",null,null,null,"0.12","0","0","0"},
		
	};
	public static string[][] zoneSelectInfo = {

		//Zone ID, difficulty requirement, chance(relative)
		//DIFFICULTY REQUIREMENT MUST BE ORDERED FROM SMALL TO BIG!
		new string[]{"1","0","10"},
		new string[]{"2","10","8"},
		new string[]{"3","20","7"},
		new string[]{"4","1000","1"},
	};
	public static string[] getZoneEnemyInfo(int ID){
		return zoneEnemyInfo[ID];
	}
	//Get random zone ID that doesn't exceed given difficulty
	public static int getRandomZone(float difficultyMin, float difficultyMax){
		if(difficultyMin>difficultyMax)Debug.LogError("difficultyMin is higher than Max!");
		List<int> list = new List<int>();
		int i = 0;
		for(;i<zoneSelectInfo.Length;i++){
			int Zonediff = int.Parse(zoneSelectInfo[i][1]);
			if(Zonediff>=difficultyMin)break;
		}
		for(;i<zoneSelectInfo.Length;i++){
			int Zonediff = int.Parse(zoneSelectInfo[i][1]);
			if(Zonediff>difficultyMax)break;
			list.Add(int.Parse(zoneSelectInfo[i][2]));
		}
		if(list.Count==0){
			Debug.LogError("No zone found within difficulty range");
		}
		//return ID of zone
		return int.Parse(
			zoneSelectInfo[Library.weightedRandom(list.ToArray())][0] 
			);

	}
}

public class Library{
	//return random index based on their weight
	public static int weightedRandom(int[] w){	// w = weight for each index

		int sum_of_weight = 0;
		for(int i=0; i<w.Length; i++) {
			sum_of_weight += w[i];
		}
		int rnd = Random.Range(0,sum_of_weight);
		for(int j=0; j<w.Length; j++) {
			if(rnd < w[j])return j;
			rnd -= w[j];
		}
		Debug.LogError("weightedRandom Error");
		return 0;
	}
	public static int weightedRandom(float[] w){	// w = weight for each index
		
		float sum_of_weight = 0;
		for(int i=0; i<w.Length; i++) {
			sum_of_weight += w[i];
		}
		float rnd = Random.Range(0,sum_of_weight);
		for(int j=0; j<w.Length; j++) {
			if(rnd < w[j])return j;
			rnd -= w[j];
		}
		Debug.LogError("weightedRandom Error");
		return 0;
	}
}