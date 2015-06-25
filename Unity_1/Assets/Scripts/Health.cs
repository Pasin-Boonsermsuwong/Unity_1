using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SpeedTumbleScale{
	public float speedMin,speedMax,tumble,scaleMin,scaleMax;
}
[System.Serializable]
public class Loot{
	public int[] ID;
	public float[] chance;
	public GameObject collectible;
}
public class Health : MonoBehaviour {

	public float maxHP;
	public float curHP;
	public Slider slider;
	public int score;
	public GameObject explosion;
	GameController gc;
	//Initial speed / rotation / scale. Only needed for asteroids or other unmanned object
	public bool speedTumbleScale;
	public SpeedTumbleScale STS;
	public Loot loot;
	public GameObject dmgText;
	public bool isDead;
	void Start () {
		isDead = false;
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		float scale = 1.0f;
		if(speedTumbleScale){
			GetComponent<Rigidbody>().AddForce (transform.forward * Random.Range(STS.speedMin,STS.speedMax));
			GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * STS.tumble;
			scale = Random.Range(STS.scaleMin,STS.scaleMax);
			transform.localScale += new Vector3(scale,scale,scale);
			maxHP = maxHP * scale  ;

		}
		curHP = maxHP;
		slider.value = 1;
		score = (int)Mathf.Round(score * scale);
	}

	public void TakeDamage(float amount){
		if(isDead)return;
		curHP -= amount;
		slider.value = curHP/maxHP;
		if(curHP<=0){
			isDead = true;
			Death ();
		}
	}
	public void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		//Spawn loot
		SpawnLoot();
		gc.AddScore(score);
		Destroy (gameObject);
	}
	void SpawnLoot(){
		for(int i = 0;i<loot.ID.Length;i++){
			Debug.Log("Lootcall: "+i);
			if(Library.chance(loot.chance[i])){
				GameObject instantiated = Instantiate(loot.collectible, transform.position, transform.rotation) as GameObject;
				instantiated.GetComponent<CollectibleItem>().Set(loot.ID[i]);
			}
		}
	}
}
