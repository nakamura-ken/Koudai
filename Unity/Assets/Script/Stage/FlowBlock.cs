using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowBlock : MonoBehaviour {
	
	public float Speed = 10f;			//移動スピード
	public float blockDistance = 10f;	//ブロック間距離

	Vector3 EndPos;		//目的地
	Vector3 movePos;	//objectのフレーム移動量
	Vector3 objScale;	//objectのScale

	float time = 0f;

	//PrefabBlockList
	List<GameObject> blockList = new List<GameObject>();
	List<GameObject> removeList = new List<GameObject>();

	GameObject player;

	void Start () {
		EndPos = transform.FindChild("FlowEnd").position;

		movePos = ObjectDistance(gameObject.transform.position, EndPos, Speed);
		objScale = gameObject.transform.localScale;	//Scale取得
		gameObject.renderer.enabled = false;

		player = GameObject.Find("Player");
	}

	void Update () {
		FlowBlockInstantiate();
		FlowBlockMove();
	}

	//ブロックを生成
	void FlowBlockInstantiate(){
		time += Time.deltaTime;
		if(time > blockDistance){
			time = 0f;
			GameObject _block = (GameObject)Instantiate(Resources.Load("Prefab/Stage/_FlowBlock"), gameObject.transform.position, gameObject.transform.rotation);
			_block.transform.localScale = objScale;
			blockList.Add(_block);
		}
	}

	//ブロックを移動・削除
	void FlowBlockMove(){
		foreach(GameObject g in blockList){
			g.transform.position += movePos;

			if(Vector3.Distance(EndPos, g.transform.position) <= 0.1f){
				removeList.Add(g);
				g.SetActive(false);
			}
		}

		//参照エラー発生
		foreach(GameObject rg in removeList){
			if(player.transform.parent == rg){
				player.transform.parent = null;
				blockList.Remove(rg);
				Destroy(rg);
			}else{
				blockList.Remove(rg);
				Destroy(rg);
			}
		}
	}


	//オブジェクトの２点間等速直線移動量算出メソッド
	Vector3 ObjectDistance(Vector3 start, Vector3 end, float frame){
		return new Vector3((end.x - start.x)/frame, (end.y - start.y)/frame, (end.z - start.z)/frame);
	}
}
