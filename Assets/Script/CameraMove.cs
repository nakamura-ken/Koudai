using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	Transform player;

	public float[] position = {0f, 2f, 10f};

	void Start () {
		player = GameObject.Find("Player").transform;
	}

	void Update () {
		Vector3 pos = player.position;
		pos.z = -position[2];
		pos.y += position[1];
		pos.x += position[0];
		gameObject.transform.position = pos;
	}
}
