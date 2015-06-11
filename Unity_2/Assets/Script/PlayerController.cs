using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpd = 10f;
	public float jumpForce;
	bool facingRight = true;
	bool grounded;
	bool shield;
	public Transform groundCheck;
	float groundedRadius = 0.15f; 
	public LayerMask groundMask;
	Animator anim;

	Rigidbody2D rb;

	void Start () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	

	void Update () {
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundedRadius,groundMask);

		anim.SetBool("Grounded",grounded);
		float move = Input.GetAxis("Horizontal");
		anim.SetFloat("Speed",Mathf.Abs(move));
		if(Input.GetAxis("Vertical")<0&&grounded&&move<double.Epsilon){
			anim.SetBool("Shield",true); 
			shield = true;
		}else{
			anim.SetBool("Shield",false);
			shield = false;
		}
		rb.velocity = new Vector2(move * moveSpd * Time.deltaTime, rb.velocity.y);
		if(Input.GetButton("Jump")&&grounded){
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
}
