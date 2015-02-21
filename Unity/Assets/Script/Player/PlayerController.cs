using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Animator anim;

	//HP
	public float HitPoint = 3f;
	float nowHP;

	//無敵時間
	public float invincibleTime = 1f;
	public float Damage_x = 10f;
	public float Damage_y = 10f;
	bool invincible = false;
	bool damage = false;

	//移動速度
	public float Speed = 2f;
	bool facingRight = true;
	float move;

	//ジャンプ
	bool doubleJumpNG = false;
	bool doubleJump = false;
	bool jumping = false;
	bool jumpEnd = false;
	bool grounded = false;
	Transform groundCheck;
	float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;
	public float jumpTime = 0.01f;
	float time = 0f;

	//プレイヤーの色
	public Color[] color;
	public float alpha = 0.3f;

	//ショット
	//public float ShootTime = 1f;
	//bool shoot = false;
	[HideInInspector]
	public float shootCount = 3f;
	Transform shootParent;
	GameObject bullet;

	void Start () {
		anim = gameObject.GetComponent<Animator>();

		groundCheck = transform.FindChild("groundCheck");
		shootParent = transform.FindChild("ShootingPosition");
		bullet = (GameObject)Resources.Load("Prefab/Player/bullet");
		gameObject.renderer.material.color = color[0];

		nowHP = HitPoint;
	}

	void Update(){
		//接地判定
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Grounded", grounded);
		
		if(damage){
			if(anim.GetBool("Damage")){
				return;
			}else{
				damage = false;
			}
		}

		PlayerMove();
		Shoot();
		Jump();

		if(Input.GetKeyDown(KeyCode.A))
			gameObject.rigidbody2D.velocity = Vector2.zero;
	}

	void PlayerMove(){
		move = Input.GetAxisRaw("Horizontal");

		anim.SetFloat("Speed", Mathf.Abs(move));

		if(grounded || (!grounded && move != 0))
			gameObject.rigidbody2D.velocity = new Vector2(move * Speed*10f, gameObject.rigidbody2D.velocity.y);

		//顔の向きを反転
		if(move > 0 && !facingRight)
			Flip();
		else if(move < 0 && facingRight)
			Flip();
	}

	void Jump(){
		//接地判定
		if(grounded){
			doubleJump = false;
			doubleJumpNG =false;
		}

		//スペースDown
		if(Input.GetKeyDown(KeyCode.Space)){
			if(grounded){
				jumping = true;
			}else{
				if(!doubleJumpNG){
					doubleJump = true;
					doubleJumpNG = true;
				}
			}
			time = 0f;
		}
		//スペースUp
		if(Input.GetKeyUp(KeyCode.Space)){
			doubleJump = false;
			jumping = false;
			jumpEnd = false;
		}
		//ジャンプ
		if((jumping || doubleJump) && !jumpEnd){
			//Debug.Log("jump");
			time += Time.deltaTime;
			if(time > jumpTime){
				jumpEnd = true;
				//Debug.Log("End");
			}
			gameObject.rigidbody2D.AddForce(new Vector2(0f, jumpForce));
		}
	}

	//顔の向き
	void Flip(){
		facingRight = !facingRight;
		Vector3 scale = gameObject.transform.localScale;
		scale.x *= -1;
		gameObject.transform.localScale = scale;
	}

	//攻撃
	void Shoot(){
		if(Input.GetKeyDown(KeyCode.S)){
			//インスタンス作成
			GameObject _bullet = (GameObject)Instantiate(bullet, shootParent.position, shootParent.rotation);
			_bullet.transform.parent = shootParent.transform;
		}
	}

	//被ダメージ
	public void Damage(){
		if(invincible) return;
		damage = true;
		nowHP--;
		//float velo = gameObject.rigidbody2D.velocity.x;
		//gameObject.rigidbody2D.velocity = Vector2.zero;

		/*
		if(velo <= 0f)
			gameObject.rigidbody2D.AddForce(new Vector2(1f * Damage_x, Damage_y));
		else
			gameObject.rigidbody2D.AddForce(new Vector2((-1f) * Damage_x, Damage_y));
		*/
		gameObject.rigidbody2D.AddForce(new Vector2((-1f) * Damage_x, Damage_y));

		if(nowHP <= (HitPoint/3)*2)
			gameObject.renderer.material.color = color[1];
		if(nowHP <= HitPoint/3)
			gameObject.renderer.material.color = color[2];
		StartCoroutine(Invincible());
	}

	public void Damage2(Vector2 velo){
		if(invincible) return;
		damage = true;
		nowHP--;
		//float velo = gameObject.rigidbody2D.velocity.x;
		//gameObject.rigidbody2D.velocity = Vector2.zero;
		
		/*
		if(velo <= 0f)
			gameObject.rigidbody2D.AddForce(new Vector2(1f * Damage_x, Damage_y));
		else
			gameObject.rigidbody2D.AddForce(new Vector2((-1f) * Damage_x, Damage_y));
		*/
		Debug.Log(velo);
		gameObject.rigidbody2D.AddForce(new Vector2(Damage_x * velo.x, Damage_y * velo.y));
		
		if(nowHP <= (HitPoint/3)*2)
			gameObject.renderer.material.color = color[1];
		if(nowHP <= HitPoint/3)
			gameObject.renderer.material.color = color[2];
		StartCoroutine(Invincible());
	}

	//無敵判定
	IEnumerator Invincible(){
		Debug.Log("inv");
		invincible = true;
		anim.SetBool("Damage", true);
		Color col = gameObject.renderer.material.color;
		col.a -= alpha;
		gameObject.renderer.material.color = col;
		float time = 0f;
		while(time < invincibleTime){
			time += Time.deltaTime;
			yield return null;
		}
		anim.SetBool("Damage", false);
		invincible = false;
		col = gameObject.renderer.material.color;
		col.a += alpha;
		gameObject.renderer.material.color = col;
	}

}
