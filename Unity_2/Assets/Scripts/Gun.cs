using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

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
	AudioSource audio;
	string ownerName;
	float fireRate;
	float nextFire;

	int weaponAmount = 6;
	Text[] weaponText;
	int activeWeapon;

	float[][] fireRateTable = {
		new float[]{0.33f,0.5f,0,0,0,0},//fighter
		new float[]{0,0,0,0,0,0},//healer
		new float[]{0,0,0,0,0,0},//range
		new float[]{0,0,0,0,0,0},//scout
		new float[]{0,0,0,0,0,0},//tank
		new float[]{0,0,0,0,0,0},//spartan
		new float[]{0,0,0,0,0,0},//juggernaut
	};

	GameObject[] shotTable;
	string[] shotNameTable = {"B50","BB50"};

	Collider collider;

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
		}
		setActiveWeapon(0,true);
		rb = GetComponentInParent<Rigidbody>();
		audio = GetComponent<AudioSource>();
		ownerName = transform.root.name;
		collider = transform.root.GetComponent<Collider>();
	}

	void Update () {
		gc.localSliderReload.value = Mathf.Clamp(1.0f-(nextFire - Time.time)/fireRate,0,1);

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
		};
		if (base.isLocalPlayer && Input.GetButton("Fire1") && Time.time > nextFire) {
			switch (currentClass)
			{
			case 0:
				switch (activeWeapon)
				{
				case 0:
					//void CmdFireStandard(shotTable ID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
					CmdFireStandard(1,8000,8000,0,1);
					break;
				case 1:
					CmdFireStandard(0,9000,9000,0,1);
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
		//	if(audio!=null)audio.Play();

			nextFire = Time.time + fireRate;
		};
	}
	void setActiveWeapon(int n,bool skipCheck){
		if(n<0||n>=weaponAmount)Debug.LogError("setActiveWeapon out of range");
		if(activeWeapon==n&&!skipCheck)return;
		int lastActiveWeapon = activeWeapon;
		activeWeapon = n;
		weaponText[lastActiveWeapon].color = Color.black;
		weaponText[activeWeapon].color = Color.magenta;

		fireRate = fireRateTable[currentClass][n];
		nextFire = Time.time + fireRate;
	}
	void setActiveWeapon(int n){
		setActiveWeapon(n,false);
	}

	[Command]
	void CmdFireStandard(int shotID,float launchForceMin,float launchForceMax,float shotDeviation,float shotAmount){
	//	Debug.Log("CmdFire");
	//	if(audio!=null)audio.Play();
		for(int i = 0;i<shotAmount;i++){
			
		// CREATE SERVER SIDE INSTANCE
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
			Physics.IgnoreCollision(instantiated.GetComponent<Collider>(),collider);
		//SPAWN IN CLIENTS
			NetworkServer.Spawn(instantiated);
		}

	}
}
