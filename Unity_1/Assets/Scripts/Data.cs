using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour {
	public static bool chance(float prop){
		return Random.value<=prop;
	}
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
			//ID , firerate, speed, bullet prefab name, shotDeviation, energyRequirement
	public static string[][] info = {
		new string[]{"1","0.3","800","shotBolt","5","30"}
	};
	public static string[] getItemInfo(int ID){		//When instantiate eqiupped weapon
		return itemInfo[ID];
	}
	public static string[] getInfo(int ID){			//When instantiate item
		return info[ID];
	}
}
/*
        Dictionary<string, BadGuy> badguys = new Dictionary<string, BadGuy>();
        
        BadGuy bg1 = new BadGuy("Harvey", 50);
        BadGuy bg2 = new BadGuy("Magneto", 100);
        
        //You can place variables into the Dictionary with the
        //Add() method.
        badguys.Add("gangster", bg1);
        badguys.Add("mutant", bg2);
        
        BadGuy magneto = badguys["mutant"];
 */
[System.Serializable]
public class ZoneData{
	//Type of enemies 
	//Zone ID, BG material name ,enemy1 (prefab name), enemy2, enemy3, enemy4, enemy5, (ABSOLUTE 0<x<1) freq1, freq2, freq3, freq4, freq5
	public static string[][] zoneEnemyInfo = {
		new string[]{"0","Asteroid",null,null,null,null,"0.02",null,null,null,null},
		
	};
	public static string[][] zoneSelectInfo = {

		//Zone ID, difficulty requirement, chance(relative)
		//Difficulty requirement must be sorted from small to large
		new string[]{"0","0","10"}
	//	new string[]{"2","10","8"},
	//	new string[]{"3","20","7"},
		//new string[]{"4","1000","1"},
	};
	/*
	public static Dictionary<string,Object> getZoneEnemyInfoDict(int ID){
		Dictionary<string,Object> temp = new Dictionary<string,Object>();
		temp.Add("id",zoneEnemyInfo[ID][0]);
		temp.Add("enemy1",zoneEnemyInfo[ID][1]);
		temp.Add("enemy2",zoneEnemyInfo[ID][2]);
		temp.Add("enemy3",zoneEnemyInfo[ID][3]);
		temp.Add("enemy4",zoneEnemyInfo[ID][4]);
		temp.Add("freq1",zoneEnemyInfo[ID][5]);
		temp.Add("freq2",zoneEnemyInfo[ID][6]);
		temp.Add("freq3",zoneEnemyInfo[ID][7]);
		temp.Add("freq4",zoneEnemyInfo[ID][8]);

		return temp;
	}
	*/
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