using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpd = 10f;
	public float jumpForce;
	bool facingRight = true;
	bool grounded;
	Animator anim;

	Rigidbody2D rb;

	void Start () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	

	void Update () {
	//	grounded = !rb.velocity.y<double.e);
		float move = Input.GetAxis("Horizontal");	
		anim.SetFloat("Speed",Mathf.Abs(move));
		rb.velocity = new Vector2(move * moveSpd * Time.deltaTime,
		                                                 rb.velocity.y);
		if(Input.GetButton("Jump")){
			rb.AddForce(new Vector2(0,70));
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
