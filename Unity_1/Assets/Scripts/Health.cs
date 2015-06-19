using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class SpeedTumbleScale{
	public float speedMin,speedMax,tumble,scaleMin,scaleMax;
}
public class Health : MonoBehaviour {

	public float maxHP = 100;
	public float curHP;
	public Slider slider;
	public int score = 100;
	public GameObject explosion;

	//Initial speed / rotation / scale. Only needed for asteroids or other unmanned object
	public bool speedTumbleScale;
	public SpeedTumbleScale STS;

	void Start () {
	//	isDead = false;
		GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		float scale = 1.0f;
		if(speedTumbleScale){
			GetComponent<Rigidbody>().AddForce (transform.forward * Random.Range(STS.speedMin,STS.speedMax));
			GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * STS.tumble;
			scale = Random.Range(STS.scaleMin,STS.scaleMax);
			transform.localScale += new Vector3(scale*1.5f,scale*1.5f,scale*1.5f);
			maxHP = maxHP * scale  ;
			curHP = maxHP;
		}

		slider.value = 1;
		score = (int)Mathf.Round(score * scale);
	}

	public void TakeDamage(float amount){
		curHP -= amount;
		if(curHP<=0)Death ();
		slider.value = curHP/maxHP;
	}

	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		//GameObject explosionClone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		//explosionClone.transform.localScale = (new Vector3(scale*1.5f,scale*1.5f,scale*1.5f));
		GameObject.FindWithTag("GameController").GetComponent<GameController>().AddScore(score);
		Destroy (gameObject);
	}
}
