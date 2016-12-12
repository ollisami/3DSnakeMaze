using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapController {
	
	private Dictionary<int,Area> areas;

	private PlayerController playerController;
	private Dictionary<Player,Vector2> lastPlayerPositions;
	private CalculateCordinates calculator;

	private int spawnRadius = 40;
	private int areaMaxValue = 10000;

	private Country groundCountry;
	private int groundCounter = 0;

	public MapController() {
		this.areas = new Dictionary<int, Area> ();
		this.calculator = new CalculateCordinates ();
		lastPlayerPositions = new Dictionary<Player,Vector2> ();

		Color groundColor = new Color(0.44F, 0.9F, 0F);
		this.groundCountry = new Country ("Ground", 1000000, groundColor, -1000, -1000);
	}

//-------------------------------------------------------------Set area to player / country ----------------------------

	public void SetAreaToPlayer(Player p, int k) {
		if (areas.ContainsKey (k)) {
			areas [k].setOwner (p.GetCountry());
		} else {
			Vector2 pos = calculator.CalculateCordinatesFromKey(k);
			Area a = CreateArea((int)pos.x,(int)pos.y);
			a.setOwner (p.GetCountry ());
		}
		
	}

	public void SetAreaToPlayer(Player p, Area area) {
		area.setOwner (p.GetCountry ());
	}

	public void setAreaToCountry(Country c, int k) {
		if (areas.ContainsKey (k)) {
			areas [k].setOwner (c);
		} else {
			Vector2 pos = calculator.CalculateCordinatesFromKey(k);
			Area a = CreateArea((int)pos.x,(int)pos.y);
			a.setOwner (c);
		}
	}

//---------------------------------------------------------------DRAW THE MAP --------------------------------------------
	public void DrawMap() {
		List<Player> players = playerController.GetAllPlayers ();
		foreach (Player p in players) {
			Vector2 pos = p.GetPos();
			if(lastPlayerPositions.ContainsKey(p)) {
				if(lastPlayerPositions[p].Equals(pos)) {
					return;
				}
			}
			int posX = (int)pos.x;
			int posY = (int)pos.y;
				
			//key = calculator.CalculateKey (posX, posY);
			Area area = CreateArea (posX, posY);
			SetAreaToPlayer (p, area);
		}
	}



	private Area CreateArea(int x, int y) {
		if (x > 1 && y > 1 && x < areaMaxValue - 1 && y < areaMaxValue - 1) {
			int k = calculator.CalculateKey (x, y);
			if (!areas.ContainsKey (k)) {
				if (groundCounter < 50 && Random.value < 0.85F) {
					Area a = new Area (x, y, this.groundCountry);
					areas.Add (k, a);
					groundCounter++;
					return a;
				} else {	
					Area a = new Area (x, y);
					areas.Add (k, a);
					groundCounter++;
					if (groundCounter > 100) {
						groundCounter = 0;
					}
					return a;
				}
			}
			return areas [k];
		}
		if (x == 1 || y == 1 || x == areaMaxValue - 1 || y == areaMaxValue - 1) {
			int outk = calculator.CalculateKey (x, y);
			if (!areas.ContainsKey (outk)) {
				Area a = new Area (x, y, this.groundCountry);
				areas.Add (outk, a);
				return a;
			}
			return areas [outk];
		}
		return null;
	}

	public Area GetArea(int x, int y) {
		int k = calculator.CalculateKey (x, y);
		//Debug.Log ("here! " + x + " , " + y + " " + this.areas.Count);
		if(this.areas.ContainsKey(k)) {
			return areas[k];
		}
		return null;
	}
	public Area GetArea(int k) {
		if(areas.ContainsKey(k)) {
			return areas[k];
		}
		return null;
	}

//-----------------------------------------------------------SPAWN VISIBLE OBJECTS ---------------------------------	
	public Dictionary<int,Area> GetMapDataForPlayer(Player player) {

		Vector2 p = player.GetPos ();
		int centerX = (int)p.x;
		int centerY = (int)p.y;

		Dictionary<int,Area> spawnPositions = new Dictionary<int,Area> ();
		
		for (int x = -50 / 2; x <= 50 / 2; x++) {
			for (int y = -50 / 2; y <= 50 / 2; y++) {
				int k = calculator.CalculateKey(centerX+x,centerY+y);

				if(!spawnPositions.ContainsKey(k)) {
					if(!areas.ContainsKey(k)) {
						int areaX = centerX+x;
						int areaY =centerY+y;
						CreateArea(areaX, areaY);
					}

					if(x <= spawnRadius && y <= spawnRadius && y >= -15 && areas.ContainsKey(k)) {
						spawnPositions.Add(k, areas[k]);
					}
				}
			}
		}
		return spawnPositions;

	}
//-----------------------------------------------------------Helpers---------------------------------------------------------
	public void SetPlayerController (PlayerController pc) {
		this.playerController = pc;
	}

	public int CalculateKey (int x, int y) {
		return calculator.CalculateKey (x, y);
	}

	public int GetAreasSize() {
		return this.areas.Count;
	}

	public List<Area> CreateIsland(int centerx, int centery) {
		List<Area> island = new List<Area> ();
		List<Area> piece = new List<Area> ();
		int islandSize = Random.Range (1, 10);
		int counter = 0;
		int dir = 0;
		while (counter < islandSize) {
			for (int x = -2; x <= 2; x++) {
				for (int y = -2; y <= 2; y++) {
					int kX = (int)centerx + x + dir;
					int kY = (int)centery + y + dir;
					int k = CalculateKey(kX,kY);

					if (this.areas.ContainsKey (k)) {
						piece.Add (this.areas [k]);
					} else {
						Area a = CreateArea (kX, kY);
						piece.Add (a);
					}
				}
			}
			island.AddRange(piece);
			dir += Random.Range (-5, 5);
			counter++;
		}
		return island.Distinct ().ToList ();;
	}
//-------------------------------------------------------------Testing -----------------------------------------------------

	private void AddMaximalAmountOfArea () {
		for (int x = 0; x < areaMaxValue; x++) {
			for (int y = 0; y < areaMaxValue; y++) {
				CreateArea(x,y);
			}
		}
	}
}
