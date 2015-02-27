using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//移動タイプ
public enum MoveType{
	Line,
	Rotate
}

public class MoveBlock : MonoBehaviour {

	public MoveType moveType;

	//移動量
	public float Speed = 10f;
	public float Move_x = 1f;
	public float Move_y = 1f;

	public bool MoveReturn = false;	//逆移動

	Vector3 pos;
	float rMove = 1f;

	List<GameObject> ride = new List<GameObject>();

	void Start () {
		pos = gameObject.transform.position;
		if(MoveReturn)
			rMove = -1f;
	}

	void Update () {
		switch(moveType){
		case MoveType.Line:
			LineMove();
			break;
		case MoveType.Rotate:
			RotateMove();
			break;
		}
	}

	#region 移動メソッド
	//線移動
	void LineMove(){
		float posX = pos.x + Move_x * Mathf.Cos(Time.time * Speed * rMove);
		float posY = pos.y + Move_y * Mathf.Sin(Time.time * Speed * rMove);
		
		gameObject.transform.position = new Vector3(posX, posY, gameObject.transform.position.z);
		foreach (GameObject g in ride) {
			Vector3 v = g.transform.position;
			//g.transform.position = new Vector3(v.x + posX, v.y + posY, v.z);
		}
	}

	//回転
	void RotateMove(){
		transform.Rotate(new Vector3(0f, 0f, 45f * rMove) * Time.deltaTime * Speed);
	}
	#endregion

	void OnTriggerEnter2D(Collider2D other) {
		//床の上に乗ったオブジェクトを保存
		ride.Add(other.gameObject);
	}
	
	void OnTriggerExit2D(Collider2D other) {
		//床から離れたので削除
		ride.Remove(other.gameObject);
	}
}
