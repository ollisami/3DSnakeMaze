using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Country {

	private string name;
	private int ID;
	private Color color;
	private List<Player> players;
	private int posX;
	private int posY;
	private List<Area> coveredArea;

	public Country (string name, int ID, Color color, int x, int y) {
		this.players = new List<Player> ();
		this.coveredArea = new List<Area> ();
		this.name = name;
		this.ID = ID;
		this.color = color;
		this.posX = x;
		this.posY = y;
	}


	public string GetName() {
		return this.name;
	}


	public Vector2 GetPos() {
		return new Vector2 (this.posX, this.posY);
	}


	public Player GetPlayer(int ID) {
		foreach (Player p in this.players) {
			if(p.GetID() == ID) {
				return p;
			}
		}
		return null;
	}


	public List<Player> GetAllPlayersInCountry() {
		return this.players;
	}


	public List<Area> GetCoveredArea() {
		return this.coveredArea;
	}


	public Color GetColor() {
		return this.color;
	}

	public int GetID() {
		return this.ID;
	}


	public void SetCoveredArea(List<Area> areas) {
		this.coveredArea = areas;
	}


	public void SetPos(Vector2 pos) {
		this.posX = (int)pos.x;
		this.posY = (int)pos.y;
	}

	public void SetPos(int x, int y) {
		this.posX = x;
		this.posY = y;
	}

	public void AddPlayer(string name, int id) {
		Player p = new Player (name, id, this.color, this.posX, this.posY, this);
		this.players.Add (p);
	}
}
