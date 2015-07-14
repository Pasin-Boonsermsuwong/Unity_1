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
	CharacterController characterController;//NRB
	Collider characterCollider;//RB
	Rigidbody rb;//RB
	Gun gun;

//	public bool isDead;
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
		characterCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		gun = GetComponent<Gun>();
	}

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
		RpcRespawnPos();
		yield return new WaitForSeconds(0.4f);
		RpcRespawnServer();
	}
	[ClientRpc]
	void RpcDeath(){
		//TODO: death sound
		Instantiate(deathExplosion, transform.position, transform.rotation);
		//characterController.enabled = false;//NRB
		rb.useGravity = false;
		rb.velocity = new Vector3(0,0,0);
		characterCollider.enabled = false;//RB
		if(isLocalPlayer){
	//		isDead = true;
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
	void RpcRespawnPos(){
		if(isLocalPlayer){
			spawnPositionScript.ChangeSpawnPosition();
			myTransform.position = spawnPosition.position;
//			isDead = false;
		}
	}

	[ClientRpc]
	void RpcRespawnServer(){
		OnHealthChanged(curHP);
		if(!isLocalPlayer){
			model.SetActive(true);
			playerCanvas.SetActive(true);
		}
	//	characterController.enabled = true;//NRB
		rb.useGravity = true;
		characterCollider.enabled = true;
		gun.enabled = true;
	}

}
