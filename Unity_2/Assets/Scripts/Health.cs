using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {
	
//	public string ownerName;
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
		Debug.Log("TakeDamage: "+amount);
		curHP -= amount;
		if(curHP<=0){
			curHP = maxHP;
			StartCoroutine("ServerSideDeath");
		}
	}

	void OnHealthChanged(int h){
	//	Debug.Log("OnHealthChanged");
		curHP = h;
		if(isLocalPlayer){
			if(localSlider==null){
				Debug.Log("OnHealthChanged: localSlider is null");
				return;
			}
			localSlider.value = curHP/(float)maxHP;
		}else{
			slider.value = curHP/(float)maxHP;
		}
	}

	IEnumerator ServerSideDeath(){
		RpcDeath();
		yield return new WaitForSeconds(6);
		RpcRespawn();
		yield return new WaitForSeconds(0.5f);
		RpcRespawnServer();
	}
	[ClientRpc]
	void RpcDeath(){
		//TODO: death sound
		Instantiate(deathExplosion, transform.position, transform.rotation);
		characterController.enabled = false;
		if(isLocalPlayer){
			isDead = true;
			StartCoroutine(gc.DeadScreen(this));
		//	curHP = maxHP;

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
		characterController.enabled = true;
		if(isLocalPlayer){
			spawnPositionScript.ChangeSpawnPosition();
			myTransform.position = spawnPosition.position;
			isDead = false;
			gun.enabled = true;
		}
	}
	[ClientRpc]
	void RpcRespawnServer(){
		if(!isLocalPlayer){
			model.SetActive(true);
			playerCanvas.SetActive(true);
		}
	}
}
