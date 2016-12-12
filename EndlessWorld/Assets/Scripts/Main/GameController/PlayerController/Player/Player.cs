using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player {

	private string name;
	private int ID;
	private Color color;
	private int posX;
	private int posY;
	private Country country;
	private Dictionary<int,GameObject> spawnedObjects;
	private GameObject currentBox;

	public Player(string name, int ID, Color color,int posX,int posY, Country c) {
		this.name = name;
		this.ID = ID;
		this.color = color;
		this.posX = posX;
		this.posY = posY;
		this.country = c;

		this.spawnedObjects = new Dictionary<int, GameObject> ();
		this.currentBox = null;
	}


	public Color GetColor() {
		return this.color;
	}


	public int GetID() {
		return this.ID;
	}


	public GameObject GetCurrentBox() {
		return this.currentBox;
	}


	public Vector2 GetPos() {
		return new Vector2 (this.posX, this.posY);
	}


	public Country GetCountry() {
		return this.country;
	}

	public string GetName() {
		return this.name;
	}



	public void SetNextPos(Vector2 pos) {
		this.posX += (int)pos.x;
		this.posY += (int)pos.y;
	}


	public Dictionary<int, GameObject> GetSpawnedObjects() {
		return this.spawnedObjects;
	}


	public void SetSpawnedObjects(Dictionary<int, GameObject> spawnedObjects) {
		this.spawnedObjects = spawnedObjects;
	}


	public void SetCurrentBox(GameObject box) {
		this.currentBox = box;
	}


	public string toString() {
		return this.name + " id: " + this.ID + " position: " + posX + ", " + posY;
	}

}
