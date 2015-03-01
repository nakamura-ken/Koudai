using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Animator anim;

	//HP
	public float HitPoint = 3f;
	float nowHP;

	//ダメージ
	public float invincibleTime = 1f;
	bool invincible = false;
	public float DamageDistance = 10f;
	public float DamageSpeed = 2f;
	bool damage = false;

	//プレイヤーの色
	public Color[] color;
	public float DamageAlpha = 0.3f;

	//移動速度
	public float Speed = 2f;
	bool facingRight = true;

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

	//ショット
	public float ShootAnimTime = 0.5f;
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

		if(damage)
			return;

		PlayerMove();
		Jump();

		if(Input.GetKeyDown(KeyCode.S))
			StartCoroutine(Shoot());
	}

	#region Player移動
	void PlayerMove(){
		float move = Input.GetAxisRaw("Horizontal");

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
	#endregion

	//攻撃
	IEnumerator Shoot(){
		if(!anim.GetBool("Shoot"))
			anim.SetBool("Shoot", true);

		//インスタンス作成
		GameObject _bullet = (GameObject)Instantiate(bullet, shootParent.position, shootParent.rotation);
		_bullet.transform.parent = shootParent.transform;
		yield return new WaitForSeconds(ShootAnimTime);
		anim.SetBool("Shoot", false);
	}

	#region Playerダメージ
	//被ダメージ
	public void Damage(){
		if(invincible)
			return;

		damage = true;
		nowHP--;

		if(nowHP <= (HitPoint/3)*2)
			gameObject.renderer.material.color = color[1];
		if(nowHP <= HitPoint/3)
			gameObject.renderer.material.color = color[2];
		StartCoroutine(Invincible());
		StartCoroutine(DamageBack());
	}

	//のけぞり
	IEnumerator DamageBack(){
		anim.SetBool("Damage", true);
		float time = 0f;
		Vector3 pos = gameObject.transform.position;
		float right = 0f;
		if(facingRight)
			right = -1f;
		else
			right = 1f;
		while(Vector3.Distance(gameObject.transform.position, pos) <= DamageDistance){
			time += Time.deltaTime;
			gameObject.rigidbody2D.velocity = new Vector2(right * DamageSpeed * 10f, gameObject.rigidbody2D.velocity.y);
			yield return null;
		}
		anim.SetBool("Damage", false);
		damage = false;
	}

	//無敵判定
	IEnumerator Invincible(){
		Debug.Log("inv");
		invincible = true;
		Color col = gameObject.renderer.material.color;
		col.a -= DamageAlpha;
		gameObject.renderer.material.color = col;
		yield return new WaitForSeconds(invincibleTime);
		invincible = false;
		col = gameObject.renderer.material.color;
		col.a += DamageAlpha;
		gameObject.renderer.material.color = col;
	}
	#endregion

}
