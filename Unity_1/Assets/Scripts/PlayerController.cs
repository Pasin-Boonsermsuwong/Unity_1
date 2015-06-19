using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//Change player's modifier
public class ModifierChangeRequest{
	// ALL VALUE ARE ADDED
	public float rofModifier;
	public float dmgAdder;
	public ModifierChangeRequest(float rof,float dmg){
		rofModifier = rof;
		dmgAdder = dmg;
	}
}
[System.Serializable]
public class Boundary{
	public float xMin, xMax, zMin, zMax;
}
public class PlayerController : MonoBehaviour {

	GameController gameController;

	//Ship stats
	public string[] mountList = {"Weapon1","Weapon2"};
//	public Vector3[] mountPosition;
	public float strafeForce = 100f;
	public float RotationSpeed = 300f;
	public float power = 200f;
	public float thrust;
	public float maxSpeed;
	public Rigidbody rb;
	public Boundary boundary;
	public float reverseFraction = 0.3f;
	Vector3 mousePosition;
	public GameObject shipImage;	//Image of ship in inventory UI

	public float energyCapacity;
	public float energyRegen;
	public float energy;

	//WEAPONS MODIFIER
	public float rofModifier = 1;
	public float dmgAdder;

	//UI
	public Slider sliderEnergy;

	void OnStart(){
		rb = GetComponent<Rigidbody>();
		if(rb==null)Debug.Log("Rigidbody is null");

		updateMount();
	}
	void Start(){
		energy = energyCapacity;
		gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
		boundary = new Boundary();
		GameObject bg = GameObject.FindWithTag("Background");
		boundary.xMax = bg.transform.localScale.x/2;
		boundary.xMin = -bg.transform.localScale.x/2;
		boundary.zMax = bg.transform.localScale.y/2;
		boundary.zMin = -bg.transform.localScale.y/2;
	}
	void Update(){
		if(GameController.pause)return;
		Vector3 tf = transform.forward;
		Vector3 tp = transform.position;
		mousePosition = Input.mousePosition;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		mousePosition.y = 0;
		////////ROTATION

		Vector3 _direction = (mousePosition - tp).normalized;
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * RotationSpeed);

		////////THRUST
		rb.AddForce (tf * thrust *
		             Mathf.Clamp(Vector3.Distance (tp, mousePosition)-4.5f,0,maxSpeed)
		             );
		float backwardThrust = Input.GetAxis("Vertical");
		if(backwardThrust<0){
			rb.AddForce(tf * thrust * backwardThrust * reverseFraction);
		}

		////////STRAFE

		rb.AddForce(Quaternion.Euler(0, 90, 0) * tf
		            * Input.GetAxis("Horizontal") * strafeForce * Time.deltaTime);

		////////BOUNDARY
		if(tp.x>boundary.xMax){
			gameController.SwitchSector(1,0);
			transform.position = new Vector3(boundary.xMin,0.0f,transform.position.z);
		}
		else if(tp.x<boundary.xMin){
			gameController.SwitchSector(-1,0);
			transform.position = new Vector3(boundary.xMax,0.0f,transform.position.z);
		}
		if(tp.z>boundary.zMax){
			gameController.SwitchSector(0,1);
			transform.position = new Vector3(transform.position.x,0.0f,boundary.zMin);
		}
		else if(tp.z<boundary.zMin){
			gameController.SwitchSector(0,-1);
			transform.position = new Vector3(transform.position.x,0.0f,boundary.zMax);
		}

		//////ENERGY
		energy = Mathf.Clamp(energy+energyRegen*Time.deltaTime,0,energyCapacity);
		sliderEnergy.value = energy/energyCapacity;

	}
	//Edit player modifier and update modifier in all mounts
	public void editModifier(ModifierChangeRequest m){
		dmgAdder += m.dmgAdder;
		rofModifier = Mathf.Max(rofModifier + m.rofModifier,0.1f);
		applyModifier();
	}
	public void applyModifier(){
		Gun[] gArray = GetComponentsInChildren<Gun>();
		foreach(Gun g in gArray){
			g.RefreshModifier();
		}
	}

	//Guns only
	public void updateMount(){
		foreach(string s in mountList ){
	//		Debug.Log (s);
			Transform G = shipImage.transform.Find(s+"Mount");
			if(G.childCount==0){
	//			Debug.Log("Mount childcount = 0");
				Gun g1 = transform.Find(s).GetComponent<Gun>();
				g1.equipped = false;
				return;
			}
			ItemController i = G.GetChild(0).GetComponent<ItemController>();//get hardpoint from inventory canvas
			if(i==null){	//Deactivate gun since it is not equipped
				Gun g1 = transform.Find(s).GetComponent<Gun>();
				g1.equipped = false;
			}
			else {			//Equip a new gun
				string[] info = ItemData.getInfo(i.ID);
				Gun g = transform.Find(s).GetComponent<Gun>();
				g.equipped = true;
				g.ID = i.ID;
				g.fireRate = float.Parse(info[1]);
				g.fireSpeed = float.Parse(info[2]);
				g.shot = (GameObject)Resources.Load("Prefabs/"+info[3]);
				g.shotDeviation = float.Parse(info[4]);
				g.energyRequirement = float.Parse(info[5]);
			}
		}
		applyModifier();
	}
	
}
