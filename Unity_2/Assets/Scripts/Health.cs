using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {
	
	public string ownerName;
	public int maxHP;
	
	[SyncVar (hook = "OnHealthChanged")]int curHP;

	Slider localSlider;
	public Slider slider;
	public GameObject playerCanvas;
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

//	public void TakeDamage(float amount){
	public void TakeDamage(int amount){
		if(!isServer)return;

		curHP -= amount;
		if(curHP<=0){
			curHP = maxHP;
			StartCoroutine("ServerSideDeath");
		}
	}

	void OnHealthChanged(int h){
		Debug.Log("OnHealthChanged");
		curHP = h;
		if(isLocalPlayer){
			localSlider.value = curHP/(float)maxHP;
		}else{
			slider.value = curHP/(float)maxHP;
		}
	}

	IEnumerator ServerSideDeath(){
		RpcDeath();
		yield return new WaitForSeconds(6);
		RpcRespawn();
	}
	[ClientRpc]
	void RpcDeath(){
		//TODO: death sound
		Instantiate(deathExplosion, transform.position, transform.rotation);
		if(isLocalPlayer){
			isDead = true;
			StartCoroutine(gc.DeadScreen(this));
		//	curHP = maxHP;
			characterController.enabled = false;
			gun.enabled = false;
		}else{
			//SERVER ONLY
			model.SetActive(false);
			playerCanvas.SetActive(false);
		}
	}
	[ClientRpc]
	void RpcRespawn(){
		OnHealthChanged(curHP);
		if(isLocalPlayer){
			spawnPositionScript.ChangeSpawnPosition();
			myTransform.position = spawnPosition.position;
			isDead = false;
			characterController.enabled = true;
			gun.enabled = true;
		}else{
			model.SetActive(true);
			playerCanvas.SetActive(true);
		}
	}
}
