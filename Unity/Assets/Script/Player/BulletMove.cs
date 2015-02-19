using UnityEngine;
using System.Collections;

public class BulletMove: MonoBehaviour {

	public float lifeDistance = 10f;

	public float speed = 0.5f;

	PlayerController p_script;

	void Start () {
		p_script = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	void Update () {
		gameObject.transform.localPosition += new Vector3(speed, 0f, 0f);

		//if(lifeDistance <= gameObject.transform.localPosition.x)
			//Destroy(gameObject);
	}

	void OnBecameInvisible(){
		Debug.Log("cam");
		p_script.shootCount ++;
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Enemy"){
			EnemyController e_script = other.GetComponent<EnemyController>();
			e_script.Damage();

		}

		if (other.tag == "Enemy_") {
			Enemy_piano e_script_piano = other.GetComponent<Enemy_piano> ();
			e_script_piano.Damage ();
		}

		if (other.tag == "Enemy__") {
			Enemy_pencil e_script_pencil = other.GetComponent<Enemy_pencil> ();
			e_script_pencil.Damage ();
		}

		Destroy(gameObject);
	}
}
