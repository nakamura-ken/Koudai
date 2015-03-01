using UnityEngine;
using System.Collections;

public class _FlowBlock : MonoBehaviour {

	void Start () {
	
	}

	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		//Playerを子に入れる
		if(other.transform.parent == null && other.tag == "Player")
			other.transform.parent = gameObject.transform;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		//Playerを子からはずす
		if(other.transform.parent != null && other.tag == "Player")
			other.transform.parent = null;
	}
}
