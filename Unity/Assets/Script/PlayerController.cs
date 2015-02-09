using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float HP = 3f;
	float nowHP;

	public float invincibleTime = 1f;
	bool invincible = false;

	public float maxSpeed = 2f;
	bool facingRight = true;

	bool doubleJump = false;
	bool jump = false;
	bool grounded = false;
	Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;
	float jumpTime = 0.01f;

	public Color[] color;
	public float alpha = 0.3f;

	public float ShootTime = 0.1f;
	bool shoot = false;
	Transform shootParent;
	GameObject bullet;

	void Start () {
		groundCheck = transform.FindChild("groundCheck");
		shootParent = transform.FindChild("ShootingPosition");
		bullet = (GameObject)Resources.Load("Prefab/bullet");
		gameObject.renderer.material.color = color[0];

		nowHP = HP;
	}

	void Update(){
		PlayerMove();
		Shoot();
		if(Input.GetKeyDown(KeyCode.Space))
			Jump2();
	}


	void PlayerMove(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);

		float move = Input.GetAxisRaw("Horizontal");
		
		if(grounded || (!grounded && move != 0))
			gameObject.rigidbody2D.velocity = new Vector2(move * maxSpeed*10f, gameObject.rigidbody2D.velocity.y);
		
		if(move > 0 && !facingRight)
			Flip();
		else if(move < 0 && facingRight)
			Flip();
	}

	IEnumerator Jump(){
		if(grounded){
			if(Input.GetKey(KeyCode.Space) && jump){
				float time = 0f;
				while(time < jumpTime){
					time += Time.deltaTime;
					Debug.Log("a_jump");
					gameObject.rigidbody2D.AddForce(new Vector2(0f, jumpForce));
					yield return null;
				}
			}
			doubleJump = true;
			if(grounded && jump)
				jump = false;
		}
		if(Input.GetKeyDown(KeyCode.Space)){
			jump = true;
		}
		if(!grounded && doubleJump){
			if(Input.GetKey(KeyCode.Space)){
				float time = 0f;
				while(time < jumpTime){
					time += Time.deltaTime;
					Debug.Log("a_jump");
					gameObject.rigidbody2D.AddForce(new Vector2(0f, jumpForce));
					yield return null;
				}
			}
			doubleJump = false;
		}
	}

	void Jump2(){
		//Space Key down
			//Ground
			if(grounded){
				//Space Key 
				float time = 0f;
				if(Input.GetKey(KeyCode.Space)){
					time += Time.deltaTime;
					Debug.Log("a_jump");
					gameObject.rigidbody2D.AddForce(new Vector2(0f, jumpForce));
					//yield return null;

				}
			} 
	}


	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = gameObject.transform.localScale;
		scale.x *= -1;
		gameObject.transform.localScale = scale;
	}

	void Shoot(){
		if(Input.GetKeyDown(KeyCode.S)){
			Debug.Log("s");
			if(!shoot){
				shoot = true;
				GameObject _bullet = (GameObject)Instantiate(bullet);
				_bullet.transform.parent = shootParent.transform;
				float time = 0f;
				/*
				while(time <= ShootTime){
					Debug.Log("shoot");
					time += Time.deltaTime;
					yield return null;
				}
				*/
				shoot = false;
				Debug.Log("shoot_E");
			}
		}
	}

	public void Damage(){
		if(invincible) return;
		nowHP--;
		if(nowHP <= (HP/3)*2)
			gameObject.renderer.material.color = color[1];
		if(nowHP <= HP/3)
			gameObject.renderer.material.color = color[2];
		StartCoroutine(Invincible());
	}

	IEnumerator Invincible(){
		Debug.Log("inv");
		invincible = true;
		Color col = gameObject.renderer.material.color;
		col.a -= alpha;
		gameObject.renderer.material.color = col;
		float time = 0f;
		while(time < invincibleTime){
			time += Time.deltaTime;
			yield return null;
		}
		invincible = false;
		col = gameObject.renderer.material.color;
		col.a += alpha;
		gameObject.renderer.material.color = col;
	}
}
