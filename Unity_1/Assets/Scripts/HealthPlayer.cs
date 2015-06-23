using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour {
	
	public float maxHP;
	public float curHP;
	public Slider slider;
	public GameObject explosion;
	GameController gc;
	public GameObject dmgText;
	
	void Start () {
		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		curHP = maxHP;
	}
	
	public void TakeDamage(float amount){
		curHP -= amount;
		slider.value = curHP/maxHP;
		if(curHP<=0)Death ();

	}
	
	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
		gc.GameOver();
	}
}
