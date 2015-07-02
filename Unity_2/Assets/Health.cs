using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

	public string ownerName;
	public float HP;
	[SyncVar]
	float curHP;

	public Slider slider;
	public GameObject explosion;
	//public GameObject dmgText;
	
	void Start () {
		curHP = HP;
	}

	public void TakeDamage(float amount){
		if(!isServer)return;
		curHP -= amount;
//		slider.value = curHP/HP;
		if(curHP<=0)Death ();
	}

	void Death(){
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy (gameObject);
	}
}
