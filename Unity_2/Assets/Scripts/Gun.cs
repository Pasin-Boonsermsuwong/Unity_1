﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
/*
[System.Serializable]
public class StandardFireData{
	public int shotID;
	public float launchForceMin;
	public float launchForceMax;
	public float shotDeviation;
	public float shotAmount;
	public StandardFireData(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
		this.shotID = shotID;
		this.launchForceMin = launchForceMin;
		this.launchForceMax = launchForceMax;
		this.shotDeviation = shotDeviation;
		this.shotAmount = shotAmount;
	}
}
*/
public class Gun : NetworkBehaviour {
	int currentClass;
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
	string ownerName;
//	NetworkInstanceId ownerID;		//OF ROOT TRANSOFRM

	float fireRate;
	float nextFire;

	int weaponAmount = 6;
	Text[] weaponText;
	int activeWeapon;
	bool gunIsActive;
//	bool shootSuccess;

	float[][] fireRateTable = {
		new float[]{0.33f,0.5f,0,0,0,0},//fighter
		new float[]{0,0,0,0,0,0},//healer
		new float[]{0,0,0,0,0,0},//range
		new float[]{0,0,0,0,0,0},//scout
		new float[]{0,0,0,0,0,0},//tank
		new float[]{0,0,0,0,0,0},//spartan
		new float[]{0,0,0,0,0,0},//juggernaut
	};
	string[][] weaponNameTable =  {
		new string[]{"Machine Gun","Shotgun","[C] Grenade","[C] Artillery","ROF Buff",""},//fighter
		new string[]{"","","","","",""},//healer
		new string[]{"","","","","",""},//range
		new string[]{"","","","","",""},//scout
		new string[]{"","","","","",""},//tank
		new string[]{"","","","","",""},//spartan
		new string[]{"","","","","",""},//juggernaut
	};


	GameObject[] shotTable;
	string[] shotNameTable = {"B50","BB50"};

	//Collider collider;

	void Start () {

		gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		currentClass = GameController.currentClass;


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
	//	audio = GetComponent<AudioSource>();
		if(!isLocalPlayer)return;
		setActiveWeapon(0,true);
		ownerName = GameObject.FindWithTag("PlayerData").GetComponent<PlayerData>().playerName;
	}

	void Update () {
		if(!isLocalPlayer)return;
		if(!gc.chatState){
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
		if (Input.GetButton("Fire1") && Time.time > nextFire && !gc.pause) {
			bool shootSuccess = false;
			switch (currentClass)
			{
			case 0:
				switch (activeWeapon)
				{
				case 0:
					shootSuccess = FireCheck(1,8000,8000,0,1);
					break;
				case 1:
					shootSuccess = FireCheck(0,9000,9000,0,1);
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
			Debug.Log("SpawnCollide");
			return false;
		}
		CmdFireStandard(shotID, launchForceMin, launchForceMax, shotDeviation, shotAmount);
		return true;
	}

	[Command]
	void CmdFireStandard(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
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
					eulerAngle.x, 
					eulerAngle.y+Random.Range(-shotDeviation,shotDeviation), 
					eulerAngle.z))) as GameObject;
			}
			instantiated.GetComponent<Rigidbody>().velocity = rb.velocity;
			instantiated.GetComponent<Rigidbody>().AddForce(instantiated.transform.forward*Random.Range(launchForceMin,launchForceMax));
			instantiated.GetComponent<Bullet>().ownerName = ownerName;
			//		instantiated.GetComponent<Bullet>().ownerID = netID;
			//		Debug.Log ("CmdFire setID: "+instantiated.GetComponent<Bullet>().ownerID);
			/*

			*/
			//	Physics.IgnoreCollision(instantiated.GetComponent<Collider>(),collider);
			//SPAWN IN CLIENTS
			NetworkServer.Spawn(instantiated);
		}
		//	shootSuccess = true;
	//	return;
	}

}
