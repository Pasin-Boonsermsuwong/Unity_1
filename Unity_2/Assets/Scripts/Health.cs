using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {


	[SyncVar (hook = "OnMaxHealthChanged")]public int maxHP;
	[SyncVar]public int armor;
	
	[SyncVar (hook = "OnHealthChanged")]int curHP;

	bool isDead = false;
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
	GameController gc;

	//ARMOR BUFF
	bool isArmorBuff;
	Transform effects;		//CHILD OF PLAYER OBJECT THAT HOLDS ALL THE BUFF PARTICLE EFFECT
	GameObject buffEffect;
	int originalArmor;

	void Start () {

		gc = GameObject.FindWithTag("GameController").transform.GetComponent<GameController>();
		localSlider = gc.localSliderHealth;
		spawnPosition = GameObject.FindWithTag("SpawnPosition").transform;
		spawnPositionScript = spawnPosition.GetComponent<SpawnPosition>();
		model = transform.FindChild("Model").gameObject;
		characterController = GetComponent<RbFPC_Custom>();
		characterCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		gun = GetComponent<Gun>();
		playerName = GetComponent<PlayerID>().displayName;
		UpdateSlider();
		curHP = maxHP;

		buffEffect = (GameObject) Resources.Load("PEarmor");
		effects = this.transform.Find("Effects");
	}

	void GetOwnerName(){
		playerName = GetComponent<PlayerID>().displayName;
	}

	[Command]
	void CmdSuicide(){
		TakeDamage(int.MaxValue,playerName,"Suicide");
	}

	public void TakeDamage(int amount,string sourceName,string sourceWeapon){
		TakeDamage(amount, sourceName, sourceWeapon,"");
	}
	public void TakeDamage(int amount,string sourceName,string sourceWeapon,string specialTag){
		if(!isServer||isDead)return;
		Debug.Log("TakeDamage: "+amount+"-"+armor+"="+Mathf.Max(amount - armor,0));
		if(amount > 0)amount = Mathf.Max(amount - armor,0);
		curHP = Mathf.Min (curHP - amount,maxHP);
		if(curHP<=0){
			isDead = true;
			curHP = maxHP;
			StartCoroutine(ServerSideDeath(sourceName,sourceWeapon));
		}else{
			if(!string.IsNullOrEmpty(specialTag)){
				switch (specialTag)
				{
				case "rof":
					GetComponent<Gun>().RofBuffServer();
					break;
				case "armor":
					ArmorBuffServer();
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
	void OnMaxHealthChanged(int h){
		maxHP = h;
		UpdateSlider();
	}
	void OnHealthChanged(int h){
		curHP = h;
		UpdateSlider();
	}
	void UpdateSlider(){
		if(isLocalPlayer){
			if(localSlider==null){
				gc = GameObject.FindWithTag("GameController").transform.GetComponent<GameController>();
				localSlider = gc.localSliderHealth;
				Debug.Log("OnHealthChanged: localSlider is null");
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
	//	rb.velocity = new Vector3(0,0,0);
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
			Transform effects = transform.FindChild("Effects");
			foreach (Transform child in effects) {
				GameObject.Destroy(child.gameObject);
			}
		}
		rb.velocity = new Vector3(0,0,0);
	}

	[ClientRpc]
	void RpcRespawnPos(){
		if(isLocalPlayer){
			rb.velocity = new Vector3(0,0,0);
			spawnPositionScript.ChangeSpawnPosition();
			characterController.isDead = false;
//			isDead = false;
		}
		transform.position = spawnPosition.position;
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
		isDead = false;
	}

	public void ArmorBuffServer(){
		StopCoroutine("ArmorBuffRoutine");
		StartCoroutine("ArmorBuffRoutine");
	}
	IEnumerator ArmorBuffRoutine(){
		RpcArmorBuff();
		yield return new WaitForSeconds(10);
		RpcArmorBuffEnd();
	}
	[ClientRpc]
	void RpcArmorBuff(){
		if(isLocalPlayer){
			if(isArmorBuff)return;	//BUFF ALREADY ACTIVATED
			isArmorBuff = true;
			ArmorBuffApply(isArmorBuff);
		}else{
			if(effects.FindChild("PEarmor(Clone)")==null){
				GameObject PErof = (GameObject)Instantiate(buffEffect,transform.position,Quaternion.identity);
				PErof.transform.SetParent(effects);
			}
		}
	}
	void ArmorBuffApply(bool buffActivated){
		if(buffActivated){
			originalArmor = armor;
			armor = Mathf.Max(armor + 10, armor*2);
		}
		else{
			armor = originalArmor ;
		}
	}
	[ClientRpc]
	void RpcArmorBuffEnd(){
		if(isLocalPlayer){
			isArmorBuff = false;
			ArmorBuffApply(isArmorBuff);
		}else{
			Transform t = effects.FindChild("PEarmor(Clone)");
			if(t!=null)Destroy(t.gameObject);
		}
	}




}
