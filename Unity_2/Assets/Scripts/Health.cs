using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {
	
	public string ownerName;
	public float maxHP;
	
	[SyncVar (hook = "OnHealthChanged")]float curHP;

	Slider localSlider;
	public Slider slider;
	public GameObject deathExplosion;
	GameObject model;
	CharacterController characterController;
	Gun gun;

	public bool isDead;
	Transform spawnPosition;
	SpawnPosition spawnPositionScript;
	Transform myTransform;
	GameController gc;
	void Start () {
		gc = GameObject.FindWithTag("GameController").transform.GetComponent<GameController>();
		localSlider = gc.localSliderHealth;

		myTransform = GetComponent<Transform>();
		spawnPosition = GameObject.FindWithTag("SpawnPosition").transform;
		spawnPositionScript = spawnPosition.GetComponent<SpawnPosition>();
		curHP = maxHP;

		model = myTransform.FindChild("Model").gameObject;
		characterController = GetComponent<CharacterController>();
		gun = GetComponent<Gun>();
	}

	public void TakeDamage(float amount){
		Debug.Log(ownerName+" TakeDamage");
		curHP -= amount;
		if(curHP<=0)Death ();
	}

	void OnHealthChanged(float h){
		Debug.Log("OnHealthChanged");
		curHP = h;
		if(isLocalPlayer){
			localSlider.value = curHP/maxHP;
		}else{
			slider.value = curHP/maxHP;
		}
	}


	void Death(){
		//TODO: death sound
		Instantiate(deathExplosion, transform.position, transform.rotation);
		model.SetActive(false);
		if(isLocalPlayer){
			isDead = true;
			StartCoroutine(gc.DeadScreen(this));
			curHP = maxHP;

			characterController.enabled = false;
			gun.enabled = false;
		}

	}

	[ClientRpc]
	public void RpcRespawn(){
		localSlider.value = curHP/maxHP;
		slider.value = curHP/maxHP;
		if(isLocalPlayer){
			spawnPositionScript.ChangeSpawnPosition();
			myTransform.position = spawnPosition.position;
			isDead = false;

			model.SetActive(true);
			characterController.enabled = true;
			gun.enabled = true;
		}
	}
}
