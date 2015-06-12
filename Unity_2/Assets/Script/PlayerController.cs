using UnityEngine;
using UnityEngine.UI;
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
	float delaySinceFirstJump;
	public bool doubleJump;
	public string MoveFire;
	public ParticleSystem jumpEffect;
	//Stats
	public float maxHP;
	float HP;

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

	//Slider
	public Slider healthSlider;
	public Slider shieldSlider;
	public Slider fireSlider;

	void Start () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		fire = fireMax;
		HP = maxHP;
		shield = shieldMax;
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
		if(grounded){
			delaySinceFirstJump = Time.time;
			doubleJump = true;
		}
		anim.SetBool("Grounded",grounded);
		float move = 0;
		if(!IsShield){																			//Movement
			move = Input.GetAxis(moveHor);															
			anim.SetFloat("Speed",Mathf.Abs(move));
			rb.velocity = new Vector2(move * moveSpd * Time.deltaTime, rb.velocity.y);
		}
		if(Input.GetButton(jump)&&!IsShield){														//Jump TODO: time.deltatime
	//		Debug.Log(Time.time-delaySinceFirstJump);
			if(grounded){
				rb.AddForce(new Vector2(0,jumpForce*Time.deltaTime));
			}
			else if(doubleJump&&Time.time-delaySinceFirstJump > 0.5f){
	//			Debug.Log("Doublejump");
				rb.AddForce(new Vector2(0,jumpForce*Time.deltaTime*5));
				Instantiate(jumpEffect, new Vector2(transform.position.x,transform.position.y-0.7f),  Quaternion.Euler(new Vector3(
					90, 
					0, 
					0)));
				doubleJump = false;
			}
		}

		if(Input.GetButtonDown(moveShield)&&shield>=shieldCost){									//Shield 	&&move<double.Epsilon&&move>-double.Epsilon
			anim.SetBool("Shield",true); 
			IsShield = true;
			transform.GetChild (2).gameObject.GetComponent<CircleCollider2D>().enabled = true;
		}
		if(Input.GetButtonUp(moveShield)||shield<shieldCost){
			IsShield = false;
			transform.GetChild (2).gameObject.GetComponent<CircleCollider2D>().enabled = false;
			anim.SetBool("Shield",false);
		}
		if(IsShield){
			shield -= shieldCost;

		}else{
			shield += shieldRegen;
			shield = Mathf.Min(shield,shieldMax);
		}

		if(move > 0 && !facingRight){
			Flip();
		}else if(move < 0 && facingRight){
			Flip();
		}
		healthSlider.value = HP/maxHP;
		shieldSlider.value = shield/shieldMax;
		fireSlider.value = fire/fireMax;

	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 v = transform.localScale;
		v.x = v.x*-1;
		transform.localScale = v;
	}

	public void TakeDamage(float damage){
		HP -= (IsShield)?damage*0.2f:damage;
		if(HP<0){
			healthSlider.value = 0;
			Destroy(gameObject);
		}
	}
}
