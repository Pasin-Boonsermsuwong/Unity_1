﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
public class Gun : NetworkBehaviour {

	int currentClass;
	public enumClass classSelect;
	public enum enumClass
	{
		Fighter = 0,
		Healer = 1,
		Ranger = 2,
		Assassin = 3,
		Tank = 4,
		Spartan = 5,
		Juggernaut = 6,
	}
	// 0 = fighter
	// 1 = healer
	// 2 = ranger
	// 3 = scout
	// 4 = tank
	// 5 = spartan
	// 6 = juggernaut
	GameController gc;
	public Transform gunHardpoint;
	Rigidbody rb;
//	AudioSource audio;
	string playerName;
//	NetworkInstanceId ownerID;		//OF ROOT TRANSOFRM

	float fireRate;
	float nextFire;

	int weaponAmount = 6;
	Text[] weaponText;
	int activeWeapon;
	bool gunIsActive;

	//CHARGING
	bool isCharging;
	bool isChargeRelease;
	float chargeRate;
	float chargeCurrent;

	//Special ROF BUFF
	Transform effects;

	bool isRofBuff;
	GameObject buffEffect;

	float[][] fireRateTable = {
		new float[]{0.2f,0.5f,1,2,3,0},//fighter
		new float[]{0,0,0,0,0,0},//healer
		new float[]{0,0,0,0,0,0},//range
		new float[]{0,0,0,0,0,0},//scout
		new float[]{0,0,0,0,0,0},//tank
		new float[]{0,0,0,0,0,0},//spartan
		new float[]{0,0,0,0,0,0},//juggernaut
	};
	string[][] weaponNameTable =  {
		new string[]{"Machine Gun","Shotgun","[C]Grenade","[C]Artillery","ROF Buff",""},//fighter
		new string[]{"","","","","",""},//healer
		new string[]{"","","","","",""},//range
		new string[]{"","","","","",""},//scout
		new string[]{"","","","","",""},//tank
		new string[]{"","","","","",""},//spartan
		new string[]{"","","","","",""},//juggernaut
	};


	GameObject[] shotTable;
	string[] shotNameTable = {"B50","BB50","BG150","BS50","BA200","PErofCast"};//0-5

	//Collider collider;

	void Start () {
		buffEffect = (GameObject) Resources.Load("PErof");
		effects = this.transform.Find("Effects");

		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		currentClass = (int)classSelect;


		shotTable = new GameObject[shotNameTable.Length];
		//LOAD BULLETS INTO ARRAY

		for(int i = 0;i<shotTable.Length;i++){
			shotTable[i] = (GameObject)Resources.Load(shotNameTable[i]);
		}


		weaponText = new Text[weaponAmount];
		//SET UI WEAPON TEXT
		for(int i =0;i<weaponAmount;i++){
			weaponText[i] = gc.weaponPanel.transform.GetChild(i).GetComponent<Text>();
			weaponText[i].text = weaponNameTable[currentClass][i];
		}
		rb = GetComponentInParent<Rigidbody>();
		Invoke("GetOwnerName", 1);
		if(!isLocalPlayer)return;
		setActiveWeapon(0,true);

	}
	void GetOwnerName(){
		playerName = GetComponent<PlayerID>().displayName;
	}

	void ActivateChargeUp(float chargeRate1){
		isCharging = true;
		chargeCurrent = 0;
		isChargeRelease = false;
		this.chargeRate = chargeRate1;
	}


	void Update () {
		if(!isLocalPlayer)return;
		if(!gc.chatState&&!isCharging){
			if(Input.GetButtonDown("Weapon0")){
				setActiveWeapon(0);
			}else if(Input.GetButtonDown("Weapon1")){
				setActiveWeapon(1);
			}else if(Input.GetButtonDown("Weapon2")){
				setActiveWeapon(2);
			}else if(Input.GetButtonDown("Weapon3")){
				setActiveWeapon(3);
			}else if(Input.GetButtonDown("Weapon4")){
				setActiveWeapon(4);
			}else if(Input.GetButtonDown("Weapon5")){
				setActiveWeapon(5);
			}else if(Input.GetButtonDown("WeaponNext")){
				setActiveWeapon(Mathf.Clamp(activeWeapon+1,0,weaponAmount-1));
			}else if(Input.GetButtonDown("WeaponPrevious")){
				setActiveWeapon(Mathf.Clamp(activeWeapon-1,0,weaponAmount-1));
		}
		
		};
		if(!gunIsActive)return;
		gc.localSliderReload.value = Mathf.Clamp(1.0f-(nextFire - Time.time)/fireRate,0,1);

		//WEAPON CHARGING
		if(isCharging){
		//	Debug.Log("Charging");
			if(Input.GetButton("Fire1")){
				chargeCurrent += chargeRate*Time.deltaTime;
				gc.localSliderCharge.value = chargeCurrent;
				if(chargeCurrent>1){
		//			Debug.Log("Charging end >1 ");
					isCharging = false;
					isChargeRelease = true;
					gc.localSliderCharge.value = 0;
				}
			}else{
		//		Debug.Log("Charging interrupted");
				isCharging = false;
				isChargeRelease = true;
				gc.localSliderCharge.value = 0;
			}
		}




		//WEAPON FIRE
		if ((Input.GetButton("Fire1") && Time.time > nextFire && !gc.pause && !isCharging)
		    || isChargeRelease) {
			bool shootSuccess = false;
			switch (currentClass)
			{
			case 0:
				switch (activeWeapon)
				{//shotID,launchForceMin,launchForceMax,shotDeviation, shotAmount)
				case 0:
					shootSuccess = FireCheck(1,8000,8000,0.5f,1);
					break;
				case 1:
					shootSuccess = FireCheck(3,1800,2500,4,6);
					break;
				case 2:
					if(!isChargeRelease){
						ActivateChargeUp(0.5f);
					}else{
						shootSuccess = FireCheck(2,27000*chargeCurrent,27000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 3:
					if(!isChargeRelease){
						ActivateChargeUp(0.25f);
					}else{
						shootSuccess = FireCheck(4,55000*chargeCurrent,55000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 4:
					CmdAoEStandard(0,20,5,"rof");
					shootSuccess = true;
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 1:
				switch (activeWeapon)
				{
				case 0:
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 2:
				switch (activeWeapon)
				{
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 3:
				switch (activeWeapon)
				{
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 4:
				switch (activeWeapon)
				{
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 5:
				switch (activeWeapon)
				{
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 6:
				switch (activeWeapon)
				{
				case 0:
					
					break;
				case 1:
					
					break;
				case 2:
					
					break;
				case 3:
					
					break;
				case 4:
					
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			default:
				Debug.LogError("WTF!?");
				break;
			}
			if(shootSuccess)nextFire = Time.time + fireRate;
		//	nextFire = Time.time + fireRate;
		};

	}
	void setActiveWeapon(int n,bool skipCheck){
		if(n<0||n>=weaponAmount)Debug.LogError("setActiveWeapon out of range");
		if(activeWeapon==n&&!skipCheck)return;
		int lastActiveWeapon = activeWeapon;
		activeWeapon = n;
		weaponText[lastActiveWeapon].color = Color.black;
		weaponText[activeWeapon].color = Color.magenta;

		if(weaponText[activeWeapon].text==""){
			gunIsActive = false;
			gc.localSliderReload.value = 0;
		}else{
			gunIsActive = true;
			fireRate = fireRateTable[currentClass][n];
			RofBuffApply(isRofBuff);
			nextFire = Time.time + fireRate;

		}
	}
	void setActiveWeapon(int n){
		setActiveWeapon(n,false);
	}

	bool FireCheck(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
		SphereCollider sph = shotTable[shotID].GetComponent<SphereCollider>();
		if(sph == null){
			Debug.Log("Bullet collider is not sphere");
			return false;
		}
		float radius = sph.radius;
	//	Debug.Log("Bullet start overlapLength: "+Physics.OverlapSphere(transform.position,radius).Length);
		//~(1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("PlayerLocal"))
		if(Physics.OverlapSphere(transform.position,radius).Length>1){
			//BY DEFAULT WILL HIT TURRETCOLLIDER , SO LENGTH = 1;
	//		Debug.Log("SpawnCollide");
			return false;
		}
		CmdFireStandard(shotID, launchForceMin, launchForceMax, shotDeviation, shotAmount,playerName,weaponNameTable[currentClass][activeWeapon]);
		return true;
	}

	[Command]
	void CmdFireStandard(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount,string ownerName1,string ownerGun1){
		for(int i = 0;i<shotAmount;i++){

			GameObject instantiated;

			//SET UP BULLET PROPERTIES
			if(shotDeviation<double.Epsilon){
				instantiated = Instantiate(shotTable[shotID], gunHardpoint.position, 
				                           gunHardpoint.rotation) as GameObject;
			}else{
				Vector3 eulerAngle = gunHardpoint.rotation.eulerAngles;
				instantiated = Instantiate(shotTable[shotID], gunHardpoint.position, 
				                           Quaternion.Euler(new Vector3(
					eulerAngle.x+Random.Range(-shotDeviation,shotDeviation), 
					eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
					eulerAngle.z))) as GameObject;
			}
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
			instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*Random.Range(launchForceMin,launchForceMax));
			instantiated.GetComponent<Bullet>().ownerName = ownerName1;
			instantiated.GetComponent<Bullet>().ownerGun = ownerGun1;
			NetworkServer.Spawn(instantiated);
		}
	}
	[Command]
	void CmdAoEStandard(int damage, float explodeRadius, int effectID,string specialTag){
		RpcAoEEffect(effectID);
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
		foreach (Collider col in objectsInRange) {
			if(col.tag == "Player"){
				float proximity = (transform.position - col.transform.position).magnitude; 
				float effect = 1 - (proximity / explodeRadius);
				int calculatedDamage = Mathf.RoundToInt(damage * effect);
				col.GetComponent<Health>().TakeDamage(calculatedDamage,playerName,weaponNameTable[currentClass][activeWeapon],specialTag);
			}
		}
	}
	[ClientRpc]
	void RpcAoEEffect(int effectID){
		Instantiate(shotTable[effectID], transform.position, Quaternion.identity);
	}
	public void DeathReset(){
		isCharging = false;
		isChargeRelease = false;
		chargeCurrent = 0;
		nextFire = Time.time + fireRate;
		gc.localSliderReload.value = 0;
		gc.localSliderCharge.value = 0;
	}

	public void RofBuffServer(){
		StopCoroutine("RofBuffRoutine");
		StartCoroutine("RofBuffRoutine");
	}
	IEnumerator RofBuffRoutine(){
		RpcRofBuff();
		yield return new WaitForSeconds(6);
		RpcRofBuffEnd();
	}
	[ClientRpc]
	void RpcRofBuff(){
		if(isLocalPlayer){
			isRofBuff = true;
			RofBuffApply(isRofBuff);
		}else{
			if(effects.FindChild("PErof(Clone)")==null){
				GameObject PErof = (GameObject)Instantiate(buffEffect,transform.position,Quaternion.identity);
				PErof.transform.SetParent(effects);
			}

		}
	}
	[ClientRpc]
	void RpcRofBuffEnd(){
		if(isLocalPlayer){
			isRofBuff = false;
			RofBuffApply(isRofBuff);
		}else{
			Transform t = effects.FindChild("PErof(Clone)");
			if(t!=null)Destroy(t.gameObject);
		}
	}
	void RofBuffApply(bool buffActivated){
		if(buffActivated)fireRate = fireRateTable[currentClass][activeWeapon]*0.5f;
		else fireRate = fireRateTable[currentClass][activeWeapon];
	}

}
