using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	PlayerController script;

	void Start () {
		script = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			script.Damage();
		}
	}
}
