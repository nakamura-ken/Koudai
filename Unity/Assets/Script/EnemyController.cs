using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public float HitPoint = 2f;

	bool grounded = false;
	Transform groundCheck;
	float groundRadius = 0.1f;
	public LayerMask whatIsGround;

	PlayerController script;

	void Start () {
		script = GameObject.Find("Player").GetComponent<PlayerController>();
		groundCheck = transform.FindChild("groundCheck");
	}

	void Update () {
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		if(!grounded)
			gameObject.transform.Translate(Vector3.down * 0.5f);
	}

	public void Damage(){
		HitPoint --;

		if(HitPoint <= 0f){
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			script.Damage();
		}
	}
}
