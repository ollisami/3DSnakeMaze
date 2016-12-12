using UnityEngine;
using System.Collections;

public class AreaObject {

	private GameObject prefab;

	public AreaObject() {
		this.prefab = null;
	}

	public AreaObject (GameObject prefab) {
		this.prefab = prefab;
	}

	public GameObject getGameObject() {
		return this.prefab;
	}

	public void SetGameObject(GameObject go) {
		this.prefab = go;
	}
}
