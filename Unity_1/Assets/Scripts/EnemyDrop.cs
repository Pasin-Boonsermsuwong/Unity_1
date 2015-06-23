using UnityEngine;
using System.Collections;

public class EnemyDrop : MonoBehaviour {

	public int[] ID;
	public float[] chance;
	public GameObject collectible;
	public void SpawnLoot(){
		for(int i = 0;i<ID.Length;i++){
			if(Library.chance(chance[i])){
				GameObject instantiated = Instantiate(collectible, transform.position, transform.rotation) as GameObject;
				instantiated.GetComponent<CollectibleItem>().Set(ID[i]);
			}
		}
	}
}
