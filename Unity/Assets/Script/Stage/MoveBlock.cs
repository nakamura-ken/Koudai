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
	}

	//回転
	void RotateMove(){
		transform.Rotate(new Vector3(0f, 0f, 45f * rMove) * Time.deltaTime * Speed);
	}
	#endregion

	//線移動用Trigger
	void OnTriggerEnter2D(Collider2D other) {
		switch(moveType){
		case MoveType.Line:
			if(other.transform.parent == null &&other.tag == "Player")
				other.transform.parent = gameObject.transform;
			break;
		case MoveType.Rotate:
			break;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		switch(moveType){
		case MoveType.Line:
			if(other.transform.parent != null && other.tag == "Player")
				other.transform.parent = null;
			break;
		case MoveType.Rotate:
			break;
		}

	}
}
