using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
public class Gun : NetworkBehaviour {

	public int currentClass;
//	public enumClass classSelect;

	// 0 = fighter
	// 1 = healer
	// 2 = ranger
	// 3 = assassin
	// 4 = tank
	// 5 = juggernaut
	GameController gc;
	public Transform gunHardpoint;
	public Transform gunCheckpoint;

	Rigidbody rb;
	RbFPC_Custom fpc;
	string playerName;
	float fireRate;
	float nextFire;

	//WEAPON AMOUNT
	int weaponAmount = 5;
	Text[] weaponText;
	int activeWeapon;
	bool gunIsActive;

	//CHARGING
	bool isCharging;
	bool isChargeRelease;
	float chargeRate;
	float chargeCurrent;
	bool autoFireWhenFull;

	//Special ROF BUFF
	Transform effects;		//CHILD OF PLAYER OBJECT THAT HOLDS ALL THE BUFF PARTICLE EFFECT
	bool isRofBuff;
	GameObject buffEffect;
	string statusName = "rof";	//FOR UI TEXT

	//STUN
	public bool isStun;

	//DMG BUFF
	public bool isDmgBuff;
	int dmgBuffMultiplier = 2;

	//TANK SHIELD
	public Transform shield;
	[SyncVar (hook="SetShieldActivated")] bool shieldActivated;

	float[][] fireRateTable = {
		new float[]{0.2f,0.5f,1,3,3,0},//fighter
		new float[]{0.5f,0.33f,1,2,2,0},//healer
		new float[]{0.5f,2,7,4,14,0},//sniper
		new float[]{5,5,4,0.1f,10,0},//assassin
		new float[]{0.4f,0.4f,4,6,0,0},//tank
		new float[]{0.75f,0.2f,1f,1.5f,5,0},//juggernaut
	};
	string[][] weaponNameTable =  {
		new string[]{"Machine Gun","Shotgun","[C]Grenade","[C]Artillery","ROF Buff",""},//fighter
		new string[]{"Gun","Heal Gun","Area Heal","Bullet Eater","Armor Buff",""},//healer
		new string[]{"Long Gun","Sniper","[C]Artillery","[C]Superbomb","Air Strike",""},//sniper
		new string[]{"Burst Shotgun","Mine","[C]Sticky Bomb","[C]Dash","Instakill",""},//assassin
		new string[]{"Big Gun","Stun Gun","Area Stun","Blockade","Shield",""},//tank
		new string[]{"Heavy Shotgun","Devastator","Area Burst","[C]Cannon","DMG Buff",""},//juggernaut
	};


	GameObject[] shotTable;
	static string[] shotNameTable = {"B50","BB50","BG150","BS50","BA200","PErofCast",//0-5 FIGHT	ER
		"BB50LongLife","B50Heal","PEAreaHealCast","BP0","PEarmorCast",  //6-10 HEALER
		"B200","B500","BG450","BF50","BA100",		//11-15  SNIPER
		"BS100","BMine500","BG150Sticky","BS2000",	//16-19	ASSASSIN
		"BB100","B50Stun","PEstunCast","Blockade",	//20-23 TANK
	"B250ShortLife","PEareaBurst","BB400","PEdamageCast"};		//24-27 JUGGERNAUT		

	//Collider collider;

	void Start () {
		//Debug.Log ("GunStart");
		buffEffect = (GameObject) Resources.Load("PErof");
		effects = this.transform.Find("Effects");
		rb = GetComponentInParent<Rigidbody>();
		fpc = GetComponent<RbFPC_Custom>();
	//	Invoke("GetOwnerName", 1);
		shotTable = new GameObject[shotNameTable.Length];
		//LOAD BULLETS INTO ARRAY
		for(int i = 0;i<shotTable.Length;i++){
			shotTable[i] = (GameObject)Resources.Load(shotNameTable[i]);
		}
		if(isServer){
			//TODO: shottable server only?
		}
		if(isLocalPlayer){
			GetOwnerName();

			gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		//	currentClass = GetComponent<PlayerID>().currentClass;
			weaponText = new Text[weaponAmount];
			//SET UI WEAPON TEXT
			for(int i =0;i<weaponAmount;i++){
				weaponText[i] = gc.weaponPanel.transform.GetChild(i).GetComponent<Text>();
				weaponText[i].text = weaponNameTable[currentClass][i];
			}
			setActiveWeapon(0,true);
		}
		Invoke("CheckShieldActivated",1);

	}
	void GetOwnerName(){
		playerName = GetComponent<PlayerID>().displayName;
		if(playerName==""){
			Debug.Log("Player name is empty, reinvoking...");
			Invoke("GetOwnerName",0.5f);
		}
	}

	void ActivateChargeUp(float chargeRate1){
		ActivateChargeUp(chargeRate1, true);
	}

	void ActivateChargeUp(float chargeRate1, bool autoFireWhenFull){
		isCharging = true;
		chargeCurrent = 0;
		isChargeRelease = false;
		this.autoFireWhenFull = autoFireWhenFull;
		chargeRate = chargeRate1;
		if(isRofBuff)chargeRate = chargeRate * 2;
	}


	void Update () {
		if(!isLocalPlayer||isStun)return;
		float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
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
		//		setActiveWeapon(5);
			}else if(Input.GetButtonDown("WeaponNext")||scrollWheelInput<0){
				setActiveWeapon(Mathf.Clamp(activeWeapon+1,0,weaponAmount-1));
			}else if(Input.GetButtonDown("WeaponPrevious")||scrollWheelInput>0){
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
				if(chargeCurrent>1&&autoFireWhenFull){
					isCharging = false;
					isChargeRelease = true;
					gc.localSliderCharge.value = 0;
				}
			}else{
				if(chargeCurrent>1)chargeCurrent = 1;
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
			case 0:			//FIGHTER
				switch (activeWeapon)
				{//shotID,launchForceMin,launchForceMax,shotDeviation, shotAmount)
				case 0:
					shootSuccess = FireCheck(1,8000,8000,300f,1);
					break;
				case 1:
					shootSuccess = FireCheck(3,9000,12500,1000f,6);
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
					CmdAoEStandard(0,20,5,"rof",0,true,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 1:		//HEALER
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(6,6500,7800,200f,1);
					break;
				case 1:
					shootSuccess = FireCheck(7,5500,6500,0,1);
					break;
				case 2:
					CmdAoEStandard(-60,20,8,null,0,true,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
					break;
				case 3:
					shootSuccess = FireIgnoreCheck(9,35000,35000,0,1);
					break;
				case 4:
					CmdAoEStandard(0,20,10,"armor",0,true,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 2:		//SNIPER
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(1,15000,15000,0,1);
					break;
				case 1:
					shootSuccess = FireCheck(11,15000,15000,0,1);
					break;
				case 2:
					if(!isChargeRelease){
						ActivateChargeUp(0.5f);
					}else{
						shootSuccess = FireCheck(4,96000*chargeCurrent,96000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 3:
					if(!isChargeRelease){
						ActivateChargeUp(0.4f);
					}else{
						shootSuccess = FireCheck(13,70000*chargeCurrent,70000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 4:
					shootSuccess = RaycastCheck();
					break;
				case 5:

					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 3:		//ASSASSIN
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(16,25000,40000,1000f,6);
					break;
				case 1:
					CmdLayobject(17,playerName,weaponNameTable[currentClass][activeWeapon],10,0);
					shootSuccess = true;
					break;
				case 2:
					if(!isChargeRelease){
						ActivateChargeUp(0.5f);
					}else{
						shootSuccess = FireCheck(18,32000*chargeCurrent,32000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 3:
					if(!isChargeRelease){
						ActivateChargeUp(0.65f,false);
					}else{
						fpc.Dash();
						rb.AddForce(gunHardpoint.forward * 1000000000 * chargeCurrent);
						shootSuccess = true;
						isChargeRelease = false;
					}
					break;
				case 4:
					shootSuccess = FireCheck(19,100,100,0,1);
					break;
				case 5:
					
					break;
				default:
					Debug.LogError("WTF!?");
					break;
				}
				break;
			case 4:		//TANK
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(20,15000,20000,750f,1);
					break;
				case 1:
					shootSuccess = FireCheck(21,7250,8000,300f,1);
					break;
				case 2:
					CmdAoEStandard(0,20,22,"stun",3f,false,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
					break;
				case 3:
					CmdLayobject(23,"","",5,2.1f);
					shootSuccess = true;
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
			case 5:		//JUGGERNAUT
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(16,18000,55000,5000f,7);
					//shootSuccess = FireCheck(16,33000,33000,10f,7);
					break;
				case 1:
					shootSuccess = FireCheck(24,45000,55000,3000f,1);
					break;
				case 2:
					CmdAoEStandard(200,35,25,null,0,false,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
					break;
				case 3:
					if(!isChargeRelease){
						ActivateChargeUp(0.40f);
					}else{
						shootSuccess = FireCheck(26,250000*chargeCurrent,250000*chargeCurrent,0,1);
						isChargeRelease = false;
					}
					break;
				case 4:
					CmdAoEStandard(0,20,27,"dmg",0,true,playerName,weaponNameTable[currentClass][activeWeapon]);
					shootSuccess = true;
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

		//SPECIAL TANK SHIELD (DISABLE)
		if(currentClass == 4 && activeWeapon == 4){
			CmdServerSetShield(false);
		}

		int lastActiveWeapon = activeWeapon;
		activeWeapon = n;

		//SPECIAL TANK SHIELD (ENABLE)
		if(currentClass == 4 && activeWeapon == 4){
			CmdServerSetShield(true);
		}

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

	[Command]
	void CmdServerSetShield(bool active){
		shieldActivated = active;
	}
	void CheckShieldActivated(){
		if(shieldActivated){
			SetShieldActivated(true);
		}
	}
	void SetShieldActivated(bool b){
		shieldActivated = b;
		shield.gameObject.SetActive(shieldActivated);
	}

	bool FireCheck(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
		SphereCollider sph = shotTable[shotID].GetComponent<SphereCollider>();
		if(sph == null){
			Debug.Log("Bullet collider is not sphere");
			return false;
		}
		float radius = sph.transform.localScale.x/2;
	//	Debug.Log(" FireCheck: "+sph.name);
	//	Debug.Log(" FireCheck radius: "+ radius);
		Collider[] colliders = Physics.OverlapSphere(gunCheckpoint.position,radius);
		if(colliders.Length>1){
			return false;
		}if(colliders.Length>0){	//ENABLES POINT-BLANK FIRING INTO ANOTHER PLAYER
			if(colliders[0].tag=="Player"&&colliders[0]!=this.GetComponent<Collider>()){
				CmdFireStandard(shotID, launchForceMin, launchForceMax, shotDeviation, shotAmount,playerName,weaponNameTable[currentClass][activeWeapon]);
				return true;
			}
			return false;
		}
		CmdFireStandard(shotID, launchForceMin, launchForceMax, shotDeviation, shotAmount,playerName,weaponNameTable[currentClass][activeWeapon]);
		return true;
	}

	bool FireIgnoreCheck(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
		CmdFireStandard(shotID, launchForceMin, launchForceMax, shotDeviation, shotAmount,playerName,weaponNameTable[currentClass][activeWeapon]);
		return true;
	}
	[Command]
	void CmdFireStandard(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount,string ownerName1,string ownerGun1){
	//	shotDeviation = shotDeviation * 300f;
		for(int i = 0;i<shotAmount;i++){

			GameObject instantiated;
			//SET UP BULLET PROPERTIES
			if(shotDeviation<double.Epsilon){
				instantiated = Instantiate(shotTable[shotID], gunHardpoint.position, 
				                           gunHardpoint.rotation) as GameObject; 
			}else{
//				Vector3 eulerAngle = gunHardpoint.rotation.eulerAngles;
				instantiated = Instantiate(shotTable[shotID], gunHardpoint.position, 
				                          gunHardpoint.rotation
			//	                           Quaternion.Euler(new Vector3(
			//		eulerAngle.x+Random.Range(-shotDeviation,shotDeviation), 
			//		eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
			//		eulerAngle.z+Random.Range(-shotDeviation,shotDeviation)))
				                           ) as GameObject;
			}

			//Vector3 deviation = Random.insideUnitCircle * shotDeviation;
			//deviation.z = gunHardpoint.position.z; // circle is at Z units 
			//deviation = transform.TransformDirection( deviation.normalized );
//			Vector3 forward = instantiated.transform.forward;//
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
			instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward
			                                                * Random.Range(launchForceMin,launchForceMax) +
			                                                instantiated.transform.up * Random.Range(-shotDeviation,shotDeviation) +
			                                                instantiated.transform.right * Random.Range(-shotDeviation,shotDeviation));
			Bullet bullet = instantiated.GetComponent<Bullet>();
			if(bullet != null){
				bullet.ownerName = ownerName1;
				bullet.ownerGun = ownerGun1;
				if(isDmgBuff)bullet.damage = bullet.damage*dmgBuffMultiplier;
			}
			NetworkServer.Spawn(instantiated);
		}
	}

	[Command]
	void CmdAoEStandard(int damage, float explodeRadius, int effectID,string specialTag, float specialTagParam,bool canHitCaster,string ownerName1, string ownerGun1){
		RpcAoEEffect(effectID);
		Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explodeRadius); 
		foreach (Collider col in objectsInRange) {
			if(col.tag == "Player"){
			//	Debug.Log("AoE hit something");
				if(!canHitCaster&&col.gameObject == this.gameObject)continue;
				float proximity = (transform.position - col.transform.position).magnitude; 
				float effect = 1 - (proximity / explodeRadius);
				int calculatedDamage = Mathf.RoundToInt(damage * effect);
				if(isDmgBuff)calculatedDamage = calculatedDamage * dmgBuffMultiplier;
				col.GetComponent<Health>().TakeDamage(calculatedDamage,ownerName1,ownerGun1,specialTag,specialTagParam);
			}
		}
	}
	bool RaycastCheck(){
		RaycastHit hitPos;
		if(!Physics.Raycast(gunHardpoint.position,gunHardpoint.forward,out hitPos)){
	//		Debug.Log("AirStrikeCheck false");
			return false;
		}
		Instantiate(shotTable[5],hitPos.point,Quaternion.identity);
		CmdAirStrike(hitPos.point,playerName,weaponNameTable[currentClass][activeWeapon]);
//		Debug.Log("AirStrikeCheck true:"+hitPos.distance+hitPos.collider.gameObject);
		return true;
	}

	[Command]
	void CmdAirStrike(Vector3 pos,string ownerName1,string ownerGun1){	//SNIPER 6TH WEAPON
		int boxRadius = 20;
		int downForce = -20;
		int hForce = 6;	//Horizontal force

		Vector3 spawnPosition = new Vector3(pos.x,pos.y + 150,pos.z);
		for(int i = 0;i<8;i++){
			GameObject instantiated;
			instantiated = Instantiate(shotTable[15],
			    new Vector3(spawnPosition.x+Random.Range(-boxRadius,boxRadius),spawnPosition.y,spawnPosition.z+Random.Range(-boxRadius,boxRadius)),
			    Quaternion.identity ) as GameObject;
			
			instantiated.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-hForce,hForce),Random.Range(downForce,0),Random.Range(-hForce,hForce));
			Bullet bullet = instantiated.GetComponent<Bullet>();
			if(bullet != null){
				bullet.ownerName = ownerName1;
				bullet.ownerGun = ownerGun1;
				if(isDmgBuff)bullet.damage = bullet.damage*dmgBuffMultiplier;
			}
			NetworkServer.Spawn(instantiated);
		}
	}

	[Command]
	void CmdLayobject(int shotID,string ownerName1,string ownerGun1,float range,float y){
		RaycastHit hitPos;
		Vector3 spawnPoint;
		if(Physics.Raycast(gunHardpoint.position,gunHardpoint.forward,out hitPos,range)){
			spawnPoint = hitPos.point + new Vector3(0,y,0);;
		}else{
			spawnPoint = gunHardpoint.position + gunHardpoint.forward * range + new Vector3(0,y,0);
		}
		GameObject instantiated;

		instantiated = Instantiate(shotTable[shotID],
		                           spawnPoint, //+ new Vector3(0,1,0),
		                          transform.rotation) as GameObject;

		Bullet bullet = instantiated.GetComponent<Bullet>();
		if(bullet != null){
			bullet.ownerName = ownerName1;
			bullet.ownerGun = ownerGun1;
			if(isDmgBuff)bullet.damage = bullet.damage*dmgBuffMultiplier;
		}
		NetworkServer.Spawn(instantiated);
	}


	[ClientRpc]
	void RpcAoEEffect(int effectID){	//FROM IN SHOTID
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
			gc.ActivateStatus(statusName);
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
			gc.DeactivateStatus(statusName);
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
