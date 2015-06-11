using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpd = 300f;
	public float jumpForce = 3000f;
	bool facingRight = true;
	bool grounded;
	bool IsShield;
	public Transform groundCheck;
	float groundedRadius = 0.15f; 
	public LayerMask groundMask;
	Animator anim;

	Rigidbody2D rb;
	//Reference to input
	public string moveHor;
	public string moveShield;
	public string jump;
	public string MoveFire;

	//Stats
	public float maxHP;
	public float HP;
	public float maxShield;
	public float shieldMax;
	float shield;
	public float shieldRegen;
	public float shieldCost;

	//weapons
	public Transform gunpoint;
	public GameObject bullet;
	public float fireRate = 1f;
	private float nextFire = 0.0f;
	public float fireMax;
	float fire;
	public float fireRegen;
	public float fireCost;


	void Start () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		fire = fireMax;
		HP = maxHP;
		shield = maxShield;
		transform.GetChild (2).gameObject.GetComponent<CircleCollider2D>().enabled = false;
	}
	

	void Update () {
		//Debug.Log(fire);
		if(Input.GetButton(MoveFire)&& Time.time > nextFire && fire >= fireCost&&!IsShield){			//Weapon fire
			fire -= fireCost;
			nextFire = Time.time + fireRate;
			GameObject clone =  Instantiate(bullet, gunpoint.position, transform.rotation) as GameObject;
			clone.GetComponent<Mover>().facingRight = facingRight;
			anim.SetBool("Fire",true);
		}else{
			fire += fireRegen;
			fire = Mathf.Min(fire,fireMax);
			anim.SetBool("Fire",false);
		}
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundedRadius,groundMask);
		anim.SetBool("Grounded",grounded);
		float move = Input.GetAxis(moveHor);															//Movement
		anim.SetFloat("Speed",Mathf.Abs(move));
		if(Input.GetButtonDown(moveShield)&&move<double.Epsilon&&move>-double.Epsilon&&shield>=shieldCost){		//Shield							//Shield
			anim.SetBool("Shield",true); 
			IsShield = true;
			transform.GetChild (2).gameObject.GetComponent<CircleCollider2D>().enabled = true;
		}else if(Input.GetButtonUp(moveShield)){
			IsShield = false;
			transform.GetChild (2).gameObject.GetComponent<CircleCollider2D>().enabled = false;
			anim.SetBool("Shield",false);
		}
		if(IsShield){
			shield -= shieldCost;

		}else{
			shield += shieldRegen;
			shield = Mathf.Min(shield,shieldRegen);
		}
		rb.velocity = new Vector2(move * moveSpd * Time.deltaTime, rb.velocity.y);
			if(Input.GetButton(jump)&&grounded){														//Jump TODO: time.deltatime
			rb.AddForce(new Vector2(0,jumpForce*Time.deltaTime));
		}
		if(move > 0 && !facingRight){
			Flip();
		}else if(move < 0 && facingRight){
			Flip();
		}
	

	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 v = transform.localScale;
		v.x = v.x*-1;
		transform.localScale = v;
	}

	public void TakeDamage(float damage){
		HP -= (IsShield)?damage*0.2f:damage;
	}
}
