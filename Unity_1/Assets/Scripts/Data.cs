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

		//ID, Draggable.type, sprite name, infocard, Name
	public static string[][] itemInfo = {
		new string[]{"0","WEAPON1","item1","The basic peashooter","Basic gun"},
		new string[]{"1","WEAPON1","itemShotgun","Classic multiple shot shotgun","Shotgun"},
		new string[]{"2","WEAPON1","itemSnipe","Shoot powerful high-velocity bullet","Sniper rifle"},
		new string[]{"3","WEAPON1","itemRocket","Standard explosive rocket","Rocket"}
	};
		//ID , firerate, speedMin, speedMax, bullet prefab name, shotDeviation, energyRequirement, shotAmount
	public static string[][] info = {
		new string[]{"0","0.3","800","800","shotBolt","5","30","1"},
		new string[]{"1","1","600","1000","shotBolt","13","120","6"},
		new string[]{"2","1","2000","2500","shotLarge","0","150","1"},
		new string[]{"3","1","400","400","shotRocket","1","200","1"}
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

	//Zone ID, BG material name ,(prefab name) enemy1, enemy2, enemy3, enemy4, enemy5, (ABSOLUTE 0<x<1) freq1, freq2, freq3, freq4, freq5
	 
	//textureName, tilingX, tilingY, Sector name
	public static string[][] zoneEnemyInfo = {
		new string[]{"0","Enemy1","AsteroidBig",null,null,null,"0.01","0.01",null,null,null,"tile_nebula_green_dff","8","4","TEST SECTOR"},
	//	new string[]{"0","Asteroid",null,null,null,null,"0.02",null,null,null,null,"tile_nebula_green_dff","8","4","Safe Space"},
		new string[]{"1","Asteroid",null,null,null,null,"0.06",null,null,null,null,"background_asteroid1","14","10.5","Asteroid Field"},
		new string[]{"2","Enemy1","Asteroid",null,null,null,"0.02","0.004",null,null,null,"tile_nebula_green_dff","8","4","Open Space"},
		new string[]{"3","Enemy1","EnemyRed",null,null,null,"0.01","0.001",null,null,null,"backgroundRed","6","6","Dangerous Space"},
		new string[]{"4","Asteroid","AsteroidBig",null,null,null,"0.03","0.001",null,null,null,"backgroundAsteroid2","6","6","Dense Asteroid Field"},
		new string[]{"5","EnemyRed",null,null,null,null,"0.02",null,null,null,null,"backgroundRed","6","6","RUN!"}

	};
	public static string[][] zoneSelectInfo = {


		//Difficulty requirement must be sorted from small to large

		//Zone ID, difficulty requirement, chance(relative)
		new string[]{"0","0","10"},
		new string[]{"1","1","10"},
		new string[]{"2","5","11"},
		new string[]{"3","7","11"},
		new string[]{"4","7","11"},
		new string[]{"5","15","3"}

	};
	public static string[] getZoneEnemyInfo(int ID){
		
		return zoneEnemyInfo[ID];
	}

	//Get random zone ID that is within difficulty range
	public static int getRandomZone(float difficultyMin, float difficultyMax){
		if(difficultyMin>difficultyMax)Debug.LogError("difficultyMin is higher than Max!");
		List<int> list = new List<int>();
		int i = 0;
		for(;i<zoneSelectInfo.Length;i++){
			int Zonediff = int.Parse(zoneSelectInfo[i][1]);
			if(Zonediff>=difficultyMin)break;
		}
		int startingIndex = i;	//Number of zone skipped in the array
		for(;i<zoneSelectInfo.Length;i++){
			int Zonediff = int.Parse(zoneSelectInfo[i][1]);
			if(Zonediff>difficultyMax)break;
			list.Add(int.Parse(zoneSelectInfo[i][2]));
		}
		if(list.Count==0){
			Debug.LogError("No zone found within difficulty range");
		}
		//return ID of zone
		return startingIndex + Library.weightedRandom(list.ToArray());


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
	public static bool chance(float chance){
		return Random.value<chance;
	}
}