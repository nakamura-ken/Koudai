using UnityEngine;
using System.Collections;

public class BulletMove: MonoBehaviour {

	public float lifeDistance = 10f;

	public float speed = 0.5f;

	void Start () {

	}

	void Update () {
		gameObject.transform.localPosition += new Vector3(speed, 0f, 0f);

		if(lifeDistance <= gameObject.transform.localPosition.x)
			Destroy(gameObject);
	}
}
