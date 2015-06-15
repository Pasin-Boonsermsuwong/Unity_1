using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AsteroidHealth : MonoBehaviour {

	public float maxHP = 100;
	public float curHP;
	public Slider slider;
	//public Image Fill;
	public int score = 100;
	public GameObject explosion;
	public float deviation = 200;
//	public GameObject explosionClone;
	//private boolean isDead;

	void Start () {
		GameController gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();

		GetComponent<Rigidbody>().AddForce (transform.forward * Mathf.Max(50,(gc.speed+Random.Range(-deviation,deviation))));
		GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * 5;
		float scale = Random.Range(1.0f,gc.scale);
	//	Debug.Log(scale);
		transform.localScale += new Vector3(scale*1.5f,scale*1.5f,scale*1.5f);
		maxHP = maxHP * scale  ;
		curHP = maxHP;
		slider.value = 1;
		score = (int)Mathf.Round(score * scale);
	}

	public void TakeDamage(float amount){
		curHP -= amount;
		if(curHP<=0)Death ();
		slider.value = curHP/maxHP;
	//	Fill.material.color = Color.Lerp(Color.red,Color.green,curHP/maxHP);
	}

	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		//GameObject explosionClone = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
		//explosionClone.transform.localScale = (new Vector3(scale*1.5f,scale*1.5f,scale*1.5f));
		GameObject.FindWithTag("GameController").GetComponent<GameController>().AddScore(score);
		Destroy (gameObject);
	}
}
