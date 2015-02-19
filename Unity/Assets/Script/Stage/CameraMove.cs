using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	Transform player;

	public float[] position = {0f, 2f};

	void Start () {
		player = GameObject.Find("Player").transform;
	}

	void Update () {
		Vector3 pos = player.position;
		pos.x += position[0];
		pos.y += position[1];
		pos.z = -10f;
		gameObject.transform.position = pos;
	}
}
