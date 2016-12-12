using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController {
	private CountryNamesAndColors namesAndColors;
	private MapController mapcontroller;
	private List<Country> countries;

	public PlayerController(MapController mapcontroller) {
		this.mapcontroller = mapcontroller;
		this.namesAndColors = new CountryNamesAndColors ();
		this.countries = new List<Country> ();

		mapcontroller.SetPlayerController (this);
	}


//--------------------------------------------------------------------PLAYER CONFIG -------------------------------------
	public void AddPlayer(string name, Color color, int ID) {
		Country country = new Country("player_country", ID, color, 5000, 5000);
		this.countries.Add (country);
		country.AddPlayer (name, ID);
	}


	public Player GetPlayerByID(int ID) {
		Player p = null;
		foreach (Country c in this.countries) {
			p = c.GetPlayer(ID);
			if(p != null) {
				return p;
			}
		}
		return p;
	}

	public List<Player> GetAllPlayers() {
		List<Player> players = new List<Player> ();
		foreach (Country c in countries) {
			players.AddRange(c.GetAllPlayersInCountry());
		}
		return players;
	}


//---------------------------------------------------Create Countries ------------------------------------------------
	public void CreateCountries(int amount) {
		if (amount > this.namesAndColors.GetSize()) {
			amount = this.namesAndColors.GetSize();
		}
		List<string> names = this.namesAndColors.GetNames();
		List<Color> colors = this.namesAndColors.GetColors();
		List<Vector2> positions = CreatePositionList (names.Count);

		for (int i = 0; i < amount; i++) {
			Country c = new Country(names[i], i,colors[i], (int)positions[i].x, (int)positions[i].y);
			this.countries.Add(c);
		}
	}

	public List<Country> GetAllCountries () {
		return this.countries;
	}

//-----------------------------------------------Helpers ----------------------------------------------------------------
	private Country GetCountryByName(string name) {

		foreach (Country country in this.countries) {
			if(country.GetName().Equals(name)) {
				return country;
			}
		}
		return null;
	}

	
	
	private List<Vector2> CreatePositionList(int amount) {
		List<Vector2> positions = new List<Vector2> ();
		
		int posX = 5000;
		int posY = 5000;
		
		int offsetX = 200;
		
		while(positions.Count < amount) {
			for (int offsetY = 0; offsetY < 500; offsetY+= 100) {
				offsetX = 200;
				positions.Add(new Vector2(posX + offsetX, posY + offsetY));
				offsetX -= offsetY;
				offsetX = Mathf.Abs(offsetX);
			}
			posX += 100;
		}
		return positions;
	}
}
