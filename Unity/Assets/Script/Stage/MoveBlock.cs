using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//移動タイプ
public enum MoveType{
	Line,
	Rotate,
	Flexible,
	Flow
}

public class MoveBlock : MonoBehaviour {

	public MoveType moveType;

	//移動量
	public float Speed = 10f;
	public float Move_x = 1f;
	public float Move_y = 1f;

	//移動方向反転
	public bool MoveReturn = false;
	float rMove = 1f;

	Vector3 firstPos;	//object初期位置

	//Flexible
	public float flexibleWait = 2f;	//端の待ち時間
	Vector3 targetPos;		//objectの移動先
	Vector3 movePos;		//objectのフレーム移動量
	Vector3 objScale;		//objectのScale
	float e_rMove = 1f;		//往復用
	Vector3[] nextPos;		//Prefab生成位置
	int blockIndex = 0;		//PrefabBlockIndex
	float time = 0f;

	//PrefabBlockList
	List<GameObject> blockList = new List<GameObject>();

	void Start () {
		if(MoveReturn)
			rMove = -1f;

		firstPos = gameObject.transform.position;	//初期位置取得

		switch(moveType){
		case MoveType.Flexible:
			objScale = gameObject.transform.localScale;	//Scale取得
			//Scaleと移動量から目的地を設定
			targetPos.x = firstPos.x + (objScale.x * Move_x * rMove);
			targetPos.y = firstPos.y + (objScale.y * Move_y * rMove);
			targetPos.z = firstPos.z;

			//目的地までの移動量を計算
			movePos = ObjectDistance(firstPos, targetPos, Speed);

			//配列の大きさを設定
			float max = Mathf.Max(Move_x, Move_y);
			nextPos = new Vector3[(int)max];

			//目的地までのブロックの位置を計算
			float next_x = (targetPos.x - firstPos.x)/max;
			float next_y = (targetPos.y - firstPos.y)/max;
			for(int i=0; i < max; i++){
				nextPos[i] = new Vector3(firstPos.x +(next_x * i), firstPos.y +(next_y * i), firstPos.z);
				Vector3 blockPos = new Vector3(nextPos[i].x, nextPos[i].y, nextPos[i].z + 0.1f);
				GameObject _block = (GameObject)Instantiate(Resources.Load("Prefab/Stage/_MoveBlock"), blockPos, gameObject.transform.rotation);
				_block.transform.localScale = objScale;
				blockList.Add(_block);
				_block.SetActive(false);
			}

			break;
		}
	}

	void Update () {
		switch(moveType){
		case MoveType.Line:
			LineMove();
			break;
		case MoveType.Rotate:
			RotateMove();
			break;
		case MoveType.Flexible:
			FlexibleMove();
			break;
		case MoveType.Flow:
			FlowMove();
			break;
		}
	}

	#region ブロック移動メソッド
	//線移動
	void LineMove(){
		float posX = firstPos.x + Move_x * Mathf.Cos(Time.time * Speed * rMove);
		float posY = firstPos.y + Move_y * Mathf.Sin(Time.time * Speed * rMove);
		
		gameObject.transform.position = new Vector3(posX, posY, gameObject.transform.position.z);
	}

	//回転
	void RotateMove(){
		transform.Rotate(new Vector3(0f, 0f, 45f * rMove) * Time.deltaTime * Speed);
	}

	//伸縮
	void FlexibleMove(){
		Vector3 pos = gameObject.transform.position;
		//反転
		if((e_rMove == 1f && Vector3.Distance(pos, targetPos) <= 0.1f)
		   || (e_rMove == -1f && Vector3.Distance(pos, firstPos) <= 0.1f))
		{
			time += Time.deltaTime;
			if(time < flexibleWait)
				return;
			else
				time = 0f;

			e_rMove *= -1f;
		}

		//ブロックの表示・非表示
		if(Vector3.Distance(pos, nextPos[blockIndex]) <= 0.1f){
			if(e_rMove == 1f){
				blockList[blockIndex].SetActive(true);
				blockIndex = (blockIndex < nextPos.Length - 1) ? blockIndex + 1 : blockIndex;
			}else if(e_rMove == -1f){
				blockList[blockIndex].SetActive(false);
				blockIndex = (blockIndex > 0) ? blockIndex - 1 : blockIndex + 1;
			}
		}

		//メインブロック移動
		gameObject.transform.position += movePos * e_rMove;
	}

	//流体運動
	void FlowMove(){

	}

	//オブジェクトの２点間等速直線移動量算出メソッド
	Vector3 ObjectDistance(Vector3 start, Vector3 end, float frame){
		return new Vector3((end.x - start.x)/frame, (end.y - start.y)/frame, (end.z - start.z)/frame);
	}
	#endregion
	
	void OnTriggerEnter2D(Collider2D other) {
		switch(moveType){
		//case MoveType.Flexible:
		case MoveType.Line:
			//Playerを子に入れる
			if(other.transform.parent == null && other.tag == "Player")
				other.transform.parent = gameObject.transform;
			break;
		case MoveType.Rotate:
			break;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		switch(moveType){
		//case MoveType.Flexible:
		case MoveType.Line:
			//Playerを子からはずす
			if(other.transform.parent != null && other.tag == "Player")
				other.transform.parent = null;
			break;
		case MoveType.Rotate:
			break;
		}

	}
}
