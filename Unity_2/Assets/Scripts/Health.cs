using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {

	public int maxHP;
	public int armor;
	
	[SyncVar (hook = "OnHealthChanged")]int curHP;

	Slider localSlider;
	public Slider slider;
	public GameObject playerCanvas;
	public GameObject deathExplosion;
	GameObject model;
	//CharacterController characterController;//NRB
	RbFPC_Custom characterController;
	Collider characterCollider;//RB
	Rigidbody rb;//RB
	Gun gun;

	string playerName;
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
		characterController = GetComponent<RbFPC_Custom>();
		characterCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		gun = GetComponent<Gun>();
		Invoke("GetOwnerName", 0.5f);
	}

	void GetOwnerName(){
		playerName = GetComponent<PlayerID>().displayName;
	}
	/*
	[Command]
	void CmdTellServerPlayerName(string playerName){
		if(!isServer)return;
		RpcTellPlayerName(playerName);
	}
	[ClientRpc]
	void RpcTellPlayerName(string playerName){
		if(!isLocalPlayer)this.playerName = playerName;
	}
	*/
	public void TakeDamage(int amount,string sourceName,string sourceWeapon){
		TakeDamage(amount, sourceName, sourceWeapon,"");
	}
	public void TakeDamage(int amount,string sourceName,string sourceWeapon,string specialTag){
		if(!isServer)return;
		Debug.Log("TakeDamage: "+amount+"-"+armor+"="+Mathf.Max(amount - armor,0));
		if(amount > 0)amount = Mathf.Max(amount - armor,0);
		curHP -= amount;
		if(curHP<=0){
			curHP = maxHP;
			StartCoroutine(ServerSideDeath(sourceName,sourceWeapon));
		}else{
			if(!string.IsNullOrEmpty(specialTag)){
				switch (specialTag)
				{
				case "rof":
					GetComponent<Gun>().RofBuffServer();
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}

			}
		}
	}



	[Command]
	void CmdSendKillMsg(string s){
		if(!isServer)return;
		RpcKillMsg(s);
	}
	[ClientRpc]
	void RpcKillMsg(string s){
		gc.AddKillMsg(s);
	}

	void OnHealthChanged(int h){
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

	IEnumerator ServerSideDeath(string sourceName,string sourceWeapon){
		RpcDeath(sourceName,sourceWeapon);
		yield return new WaitForSeconds(6);
		RpcRespawnPos();
		yield return new WaitForSeconds(0.4f);
		RpcRespawnVisible();
	}

	[ClientRpc]
	void RpcDeath(string sourceName,string sourceWeapon){
		//TODO: death sound
		Instantiate(deathExplosion, transform.position, transform.rotation);
		//characterController.enabled = false;//NRB
		rb.useGravity = false;
		rb.velocity = new Vector3(0,0,0);
		characterCollider.enabled = false;//RB
		if(isLocalPlayer){
	//		isDead = true;
			if(playerName=="")GetOwnerName();
			CmdSendKillMsg(sourceName+" > "+playerName+"("+sourceWeapon+")");
			StartCoroutine(gc.DeadScreen(this));
		//	curHP = maxHP;
			characterController.isDead = true;
			gun.DeathReset();
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
			characterController.isDead = false;
//			isDead = false;
		}
		myTransform.position = spawnPosition.position;
	}

	[ClientRpc]
	void RpcRespawnVisible(){
		OnHealthChanged(curHP);
		if(!isLocalPlayer){
			model.SetActive(true);
			playerCanvas.SetActive(true);
		}
		rb.useGravity = true;
		characterCollider.enabled = true;
		gun.enabled = true;
	}

}
