using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
public class Health : NetworkBehaviour {


	[SyncVar (hook = "OnMaxHealthChanged")]public int maxHP;
	[SyncVar]public int armor;
	
	[SyncVar (hook = "OnHealthChanged")]int curHP;

	bool isDead;
	bool isInvincible;

	Slider localSlider;
	public Slider slider;
	public GameObject playerCanvas;
	public GameObject deathExplosion;
	GameObject model;
	//CharacterController characterController;//NRB
	RbFPC_Custom fpc;
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
	GameObject armorBuffEffect;
	int originalArmor;

	//STUN
	bool isStun;
	int stunTimeLimit = 5;
	GameObject stunEffect;
	float stunEndTime;

	//DMG BUFF
	GameObject dmgBuffEffect;

	void Start () {

		gc = GameObject.FindWithTag("GameController").transform.GetComponent<GameController>();
		localSlider = gc.localSliderHealth;
		spawnPosition = GameObject.FindWithTag("SpawnPosition").transform;
		spawnPositionScript = spawnPosition.GetComponent<SpawnPosition>();
		model = transform.FindChild("Model").gameObject;
		fpc = GetComponent<RbFPC_Custom>();
		characterCollider = GetComponent<Collider>();
		rb = GetComponent<Rigidbody>();
		gun = GetComponent<Gun>();
		playerName = GetComponent<PlayerID>().displayName;
		UpdateSlider();
		curHP = maxHP;

		originalArmor = armor;
		armorBuffEffect = (GameObject) Resources.Load("PEarmor");
		stunEffect = (GameObject) Resources.Load("PEstun");
		dmgBuffEffect = (GameObject) Resources.Load("PEdamage");
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
		TakeDamage(amount, sourceName, sourceWeapon,specialTag,0);
	}
	public void TakeDamage(int amount,string sourceName,string sourceWeapon,string specialTag,float specialTagParam){
		if(!isServer||isDead||isInvincible)return;
	//	Debug.Log("TakeDamageRAW: "+amount);
		if(amount > 0){
			//ARMOR MAXES AT 90% DAMAGE REDUCTION
			amount = Mathf.Max(amount - armor,(int)Mathf.Round(amount * 0.1f));
		}
		Debug.Log("TakeDamage: "+amount);
		curHP = Mathf.Min (curHP - amount,maxHP);
		if(curHP<=0){
			isDead = true;
			StartCoroutine(ServerSideDeath(sourceName,sourceWeapon));
		}else{
			if(!string.IsNullOrEmpty(specialTag)){
				switch (specialTag)
				{
				case "rof":
					gun.RofBuffServer();
					break;
				case "armor":
					ArmorBuffServer();
					break;
				case "stun":
					StunServer(specialTagParam);
					break;
				case "dmg":
					DmgBuffServer();
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
		yield return new WaitForSeconds(5.5f);
		RpcRespawnPos();
		yield return new WaitForSeconds(0.5f);
		isInvincible = true;
		curHP = maxHP;
		RpcRespawnVisible();
		yield return new WaitForSeconds(3);
		isInvincible = false;
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
			CmdSendKillMsg(sourceName+" > "+playerName+" ("+sourceWeapon+")");
			StartCoroutine(gc.DeadScreen(this));
		//	curHP = maxHP;
			fpc.isDead = true;
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
			transform.position = spawnPosition.position;
		}
	}

	[ClientRpc]
	void RpcRespawnVisible(){
		OnHealthChanged(curHP);
		if(!isLocalPlayer){
			model.SetActive(true);
			playerCanvas.SetActive(true);
		}
		rb.useGravity = true;
		fpc.isDead = false;
		characterCollider.enabled = true;
		gun.enabled = true;
		isDead = false;
	}

	public void ArmorBuffServer(){
		StopCoroutine("ArmorBuffRoutine");
		StartCoroutine("ArmorBuffRoutine");
	}
	[Server]
	IEnumerator ArmorBuffRoutine(){
		if(!isArmorBuff){		//DO IF BUFF IS NOT ACTIVATED YET
			RpcArmorBuff();
			ArmorBuffApply(true);
		}
		yield return new WaitForSeconds(10);
		RpcArmorBuffEnd();
		ArmorBuffApply(false);
	}
	[ClientRpc]
	void RpcArmorBuff(){
		if(isLocalPlayer){
			if(isArmorBuff)return;
			gc.ActivateStatus("armor");
			isArmorBuff = true;
		}else{
			if(effects.FindChild("PEarmor(Clone)")==null){
				GameObject PErof = (GameObject)Instantiate(armorBuffEffect,transform.position,Quaternion.identity);
				PErof.transform.SetParent(effects);
			}
		}
	}
	void ArmorBuffApply(bool buffActivated){
		if(buffActivated){
			armor = armor + 30;
		}
		else{
			armor = originalArmor ;
		}
	}
	[ClientRpc]
	void RpcArmorBuffEnd(){
		if(isLocalPlayer){
			isArmorBuff = false;
			gc.DeactivateStatus("armor");
		}else{

			Transform t = effects.FindChild("PEarmor(Clone)");
			if(t!=null)Destroy(t.gameObject);
		}
	}

	[ServerCallback]
	void Update(){
		if(isStun){
			if(Time.time > stunEndTime){
				RpcStunEnd();
				isStun = false;
			}

		}
	}

	public void StunServer(float stunTime){
		Debug.Log( "StunServer: "+stunTime+" "+isStun );
		if(isStun){		//ALREADY STUNNED, ADD MORE TIME
			stunEndTime = Mathf.Min(stunEndTime + stunTime,Time.time + stunTimeLimit);
		}else{			//ACTIVATE STUN
			stunEndTime = Time.time + stunTime;
			isStun = true;
			RpcStun();
		}
	}

	[ClientRpc]
	void RpcStun(){
	//	Debug.Log("RpcStun: "+isLocalPlayer+" "+isStun);
		if(isLocalPlayer){
			isStun = true;
			StunApply(isStun);
		}else{
			if(effects.FindChild("PEstun(Clone)")==null){
				GameObject PEarmor = (GameObject)Instantiate(stunEffect,transform.position,Quaternion.identity);
				PEarmor.transform.SetParent(effects);
			}
		}
	}
	[Client]
	void StunApply(bool isActivate){
	//	Debug.Log("StunApply: "+isActivate);
		if(isActivate){
			gc.ActivateStatus("stun");
			gun.isStun = true;
			fpc.isStun = true;
		}
		else{
			gc.DeactivateStatus("stun");
			gun.isStun = false;
			fpc.isStun = false;
		}
	}
	[ClientRpc]
	void RpcStunEnd(){
	//	Debug.Log("RpcStunEnd");
		if(isLocalPlayer){
			isStun = false;
			StunApply(isStun);
		}else{
			Transform t = effects.FindChild("PEstun(Clone)");
			if(t!=null)Destroy(t.gameObject);
		}
	}

	public void DmgBuffServer(){
		StopCoroutine("DmgBuffRoutine");
		StartCoroutine("DmgBuffRoutine");
	}
	[Server]
	IEnumerator DmgBuffRoutine(){
		Debug.Log("DmgBuffRoutine");
		if(!gun.isDmgBuff){		// DO IF BUFF NOT ACTIVATED YET
			RpcDmgBuff();
			DmgBuffApply(true);
		}
		yield return new WaitForSeconds(5);
		RpcDmgBuffEnd();
		DmgBuffApply(false);
	}
	[Server]
	void DmgBuffApply(bool buffActivated){
		if(buffActivated){
			gun.isDmgBuff = true;
		}
		else{
			gun.isDmgBuff = false;
		}
	}
	[ClientRpc]
	void RpcDmgBuff(){
		if(isLocalPlayer){
			gc.ActivateStatus("dmg");
		}else{
			if(effects.FindChild("PEdamage(Clone)")==null){
				GameObject PEdmg = (GameObject)Instantiate(dmgBuffEffect,transform.position,Quaternion.identity);
				PEdmg.transform.SetParent(effects);
			}
		}
	}
	[ClientRpc]
	void RpcDmgBuffEnd(){
		if(isLocalPlayer){
			gc.DeactivateStatus("dmg");
		}else{
			Transform t = effects.FindChild("PEdamage(Clone)");
			if(t!=null)Destroy(t.gameObject);
		}
	}
}
