using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameobjectController {


	private GameObject mushroom_prefab;
	private GameObject basicBlock;

	public GameobjectController () {
		CreateList();
	}

	public GameObject InstantiateBoxObject(Vector3 pos) {
		GameObject temp = MonoBehaviour.Instantiate(basicBlock, pos, Quaternion.identity) as GameObject;
		return temp;
	}


	public GameObject InstantiateObject(GameObject obj, Vector3 pos) {
		if (obj == null) {
			return null;
		}
		GameObject temp = MonoBehaviour.Instantiate(obj, pos, Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
		return temp;
	}

	public AreaObject getRandomAreaObject() {
		int numb = Random.Range(0, 100);
		int index = 0;
		if (index == numb) {
			return new AreaObject (mushroom_prefab);
		}
		return new AreaObject();
	}


	private void CreateList() {
		this.basicBlock = (GameObject)Resources.Load("Prefabs/DefaultCube", typeof(GameObject));
		this.mushroom_prefab = (GameObject)Resources.Load("Prefabs/Nature_8", typeof(GameObject));
	}
}
