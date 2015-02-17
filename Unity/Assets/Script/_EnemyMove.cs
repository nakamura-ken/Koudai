using UnityEngine;
using System.Collections;

public class _EnemyMove : MonoBehaviour {

	public float HP = 3f;
	float nowHP;

	//public float invincibleTime = 1f;
	//bool invincible = false;

	public float maxSpeed = 2f;
	bool facingRight = true;

	bool grounded = false;
	Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	//public float jumpForce = 700f;
	float jumpTime = 0.01f;
	
	public float alpha = 0.3f;

	public float ShootTime = 0.1f;
	bool shoot = false;
	Transform shootParent;
	GameObject bullet;
	//


	PlayerController script;

	void Start () {
		script = GameObject.Find("Player").GetComponent<PlayerController>();

		//
		groundCheck = transform.FindChild("groundCheck");
		shootParent = transform.FindChild("ShootingPosition");
		bullet = (GameObject)Resources.Load("Prefab/bullet");

		nowHP = HP;
		//

	}

	void Update () {


		PlayerMove();
		//Shoot();

	}
	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player"){
			script.Damage();
		}
	}

	/*
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			script.Damage();
		}
	}
*/


	public float move = -0.1f;

	void PlayerMove(){

		//float move = Input.GetAxisRaw("Horizontal");


		//if(grounded || (!grounded && move != 0))
			gameObject.rigidbody2D.velocity = new Vector2(move * maxSpeed*10f, gameObject.rigidbody2D.velocity.y);

		if(move < 0 && !facingRight)
			Flip();
		else if(move > 0 && facingRight)
			Flip();
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
				//				_bullet.transform.parent = shootParent.transform;
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




}
	

		




