using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public string ownerName;
	public float maxHP;
	float curHP;
	public Slider slider;
	public GameObject explosion;
	public GameObject dmgText;
	
	void Start () {
		curHP = maxHP;
	}

	[PunRPC]
	public void TakeDamage(float amount){
		curHP -= amount;
		slider.value = curHP/maxHP;
		if(curHP<=0)Death ();
	}

	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
