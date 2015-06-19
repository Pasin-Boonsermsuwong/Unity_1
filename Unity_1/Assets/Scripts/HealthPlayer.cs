using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour {
	
	public float maxHP;
	public float curHP;
	public Slider slider;
	public GameObject explosion;
	GameController gc;
	
	void Start () {
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		curHP = maxHP;
	}
	
	public void TakeDamage(float amount){
		curHP -= amount;
		if(curHP<=0)Death ();
		slider.value = curHP/maxHP;
	}
	
	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
		gc.GameOver();
	}
}
