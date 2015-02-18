using UnityEngine;
using System.Collections;

public class Enemy_piano: MonoBehaviour {

	public float HitPoint = 2f;
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
	//


	//ショット
	//public float ShootTime = 0.1f;
	bool shoot = false;
	[HideInInspector]
	public float shootCount = 3f;
	Transform shootParent;
	GameObject bullet_piano;
	GameObject bullet_piano2;


	PlayerController script;

	void Start () {
		script = GameObject.Find("Player").GetComponent<PlayerController>();

		//
		//groundCheck = transform.FindChild("groundCheck");
		shootParent = transform.FindChild("Enemyattack");
		bullet_piano = (GameObject)Resources.Load("Prefab/enemybullet1");
		bullet_piano2 = (GameObject)Resources.Load("Prefab/enemybullet2");
		nowHP = HitPoint;
		//
	

	}
	public float bulletspan = 10f;

	void Update () {


		EnemyMove();
	
		bulletspan = bulletspan + Time.deltaTime;

		if(bulletspan >= 1f){
		Shoot();
		}
	}
		
	void OnCollisionEnter2D(Collision2D col){
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
	void EnemyMove(){

		//float move = Input.GetAxisRaw("Horizontal");
		//if(grounded || (!grounded && move != 0))
		//gameObject.rigidbody2D.velocity = new Vector2(move * maxSpeed*10f, gameObject.rigidbody2D.velocity.y);

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
		
	public int bulletcount = 0;
	//攻撃
	void Shoot(){
		//if(Input.GetKeyDown(KeyCode.X)){
		//if(bulletspan >= 10f){
			//インスタンス作成
			if(bulletcount <= 2){
			GameObject _bullet_piano = (GameObject)Instantiate(bullet_piano, shootParent.position, shootParent.rotation);
			_bullet_piano.transform.parent = shootParent.transform;
				bulletcount++;
				bulletspan = 0f;
			}
			else{
				GameObject _bullet_piano2 = (GameObject)Instantiate(bullet_piano2, shootParent.position, shootParent.rotation);
				_bullet_piano2.transform.parent = shootParent.transform;
				bulletcount = 0;
				bulletspan = 0f;

			}
		//}
	}


	public void Damage(){
		HitPoint --;

		if(HitPoint <= 0f){
			Destroy(gameObject);
		}
	}

}
	




