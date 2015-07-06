using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {

	public string ownerName;
	public float maxHP;


	[SyncVar]
	float curHP;

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
		myTransform = GetComponent<Transform>();
		spawnPosition = GameObject.FindWithTag("SpawnPosition").transform;
		spawnPositionScript = spawnPosition.GetComponent<SpawnPosition>();
		curHP = maxHP;

		model = myTransform.FindChild("Model").gameObject;
		characterController = GetComponent<CharacterController>();
		gun = GetComponent<Gun>();
	}

	public void TakeDamage(float amount){
		if(!isServer)return;
		curHP -= amount;
		slider.value = curHP/maxHP;
		if(curHP<=0)Death ();
	}

	void Death(){
		isDead = true;
		Instantiate(deathExplosion, transform.position, transform.rotation);
		StartCoroutine(gc.DeadScreen(this));
		curHP = maxHP;

		model.SetActive(false);
		characterController.enabled = false;
		gun.enabled = false;
	}

	[ClientRpc]
	public void RpcRespawn(){
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
