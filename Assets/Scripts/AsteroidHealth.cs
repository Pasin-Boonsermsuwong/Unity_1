using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AsteroidHealth : MonoBehaviour {

	public float maxHP = 100;
	public float curHP;
	public float maxScale = 4;
	public Slider slider;
	public GameObject explosion;
	//private boolean isDead;

	void Start () {
		float scale = Random.Range(1.0f,maxScale);
		Debug.Log(scale);
		transform.localScale += new Vector3(scale*2,scale*2,scale*2);
		maxHP = maxHP * scale;
		curHP = maxHP;
		slider.value = 1;
	}

	public void TakeDamage(float amount){
		curHP -= amount;
		if(curHP<=0)Death ();
		slider.value = curHP/maxHP;
	}

	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
