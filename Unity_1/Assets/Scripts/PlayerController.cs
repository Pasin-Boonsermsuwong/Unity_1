using UnityEngine;
using System.Collections;

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

	//WEAPONS MODIFIER
	public float rofModifier = 1;
	public float dmgAdder;


	void OnStart(){
		rb = GetComponent<Rigidbody>();
		if(rb==null)Debug.Log("Rigidbody is null");
		boundary = new Boundary();
		updateMount();
		
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

		////////BORDER
		rb.position = new Vector3(
			Mathf.Clamp(rb.position.x,boundary.xMin,boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z,boundary.zMin,boundary.zMax)
		);
		if(tp.x>boundary.xMax)rb.AddForce(new Vector3(-power/5,0,0));
		else if(tp.x<boundary.xMin)rb.AddForce(new Vector3(power/5,0,0));
		if(tp.z>boundary.zMax)rb.AddForce(new Vector3(0,0,-power/5));
		else if(tp.z<boundary.zMin)rb.AddForce(new Vector3(0,0,power/5));
	}
	//Edit player modifier and update modifier in all mounts
	public void editModifier(ModifierChangeRequest m){
		dmgAdder += m.dmgAdder;
		rofModifier = Mathf.Max(rofModifier + m.rofModifier,0.1f);
		Gun[] gArray = GetComponentsInChildren<Gun>();
		foreach(Gun g in gArray){
			g.RefreshModifier();
		}
	}
	//new string[]{"1","0.3","800","shotBolt","5"}
	//Guns only
	public void updateMount(){
		foreach(string s in mountList ){
			Debug.Log (s);
			Transform G = shipImage.transform.Find(s+"Mount");
			if(G.childCount==0){
				Debug.Log("Mount childcount = 0");
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
			}
		}
	}
	
}
