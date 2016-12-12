using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Area {

	private int posX;
	private int posY;
	private Country owner;

	private AreaObject areaObject;

	public Area(int x, int y) {
		this.posX = x;
		this.posY = y;
		this.areaObject = null;
		this.owner = null;
	}

	public Area(int x, int y, Country country) {
		this.posX = x;
		this.posY = y;
		this.areaObject = null;
		this.owner = country;

	}

	public int GetX() {
		return this.posX;
	}
	public int GetY() {
		return this.posY;
	}
	public Vector2 GetVector2() {
		return new Vector2 (this.posX, this.posY);
	}
	public Country GetOwner() {
		return this.owner;
	}

	public AreaObject getAreaObject() {
		return this.areaObject;
	}

	public void setOwner(Country owner) {
		this.owner = owner;
	}

	public void SetAreaObject(AreaObject aobject) {
		this.areaObject = aobject;
	}
	public bool TryToInvade(Country other) {
		if (owner != null || other == owner) {
			return false;
		} else {
			this.owner = other;
			return true;
		}
	}

	public bool isMushroom() {
		if (this.areaObject == null) {
			return false;
		}
		if (this.areaObject.getGameObject () == null) {
			return false;
		}
		return true;
	}

}
