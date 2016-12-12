using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController {
	
	private UIController UIcontroller;
	private MapController mapControll;
	private PlayerController playerController;
	private CalculateCordinates calculator;
	private Player currentPlayer;

	private GameobjectController goController;

	private Material waterMaterial;
	private Material groundMaterial;
	private Material playerMaterial;
	private Country groundCountry;

	private int playerScore;
	private bool gamePaused;
	private bool playerDead;

	public GameController(GameObject ground) {

		this.UIcontroller = GameObject.FindGameObjectWithTag ("UIController").GetComponent<UIController> ();
		if (UIcontroller == null) {
			Debug.Log ("NO UIController found!");
		}
		this.playerScore = 0;
		this.gamePaused = false;
		this.playerDead = false;

		this.goController= new GameobjectController();

		this.mapControll = new MapController ();
		this.playerController = new PlayerController (mapControll);
		this.calculator = new CalculateCordinates();

		Color groundColor = new Color(0.44F, 0.9F, 0F);
		this.groundMaterial = new Material(Shader.Find("Standard"));
		this.groundMaterial.color = groundColor;
		this.groundCountry = new Country ("Ground", 1000000, groundColor, -1000, -1000);

		Color waterColor = new Color(0.07F, 0.53F, 0.53F);
		this.waterMaterial = new Material (Shader.Find ("Standard"));

		this.playerMaterial = new Material(Shader.Find("Standard"));
		waterMaterial.color = waterColor;

		playerController.CreateCountries (1);
	}

	public void CreateMap() {
		mapControll.DrawMap();
		SpawnMap();
	}

	//-----------------------------------------------------------Score -------------------------------------------------


	public void addScore(int amount) {
		playerScore += amount;
		UIcontroller.updateScoreText (playerScore);
	}

	public int getScore() {
		return this.playerScore;
	}

	//--------------------------------------------------------------Click Controll ---------------------------------------
	public void PlayerClicked(int areaX, int areaY) {
		if (!gamePaused) {
			Vector2 playerPos = currentPlayer.GetPos ();
			Area a = mapControll.GetArea ((int)playerPos.x + areaX, (int)playerPos.y + areaY);
			//yritetään hyökätä, jos hyökkäys onnistuu...
			if (a != null) {
				if (a.TryToInvade (currentPlayer.GetCountry ())) {
					Vector2 pos = a.GetVector2 () - currentPlayer.GetPos ();
					MovePlayer ((int)pos.x, (int)pos.y);
					addScore (1);
					UIcontroller.hideTutorial ();
					Vector2 newPlayerPos = currentPlayer.GetPos ();
					if (checkIfDead ((int)newPlayerPos.x, (int)newPlayerPos.y)) {
						Debug.Log ("Player is dead!");
						killPlayer ();
					}
				}
			}
		}
	}

	//--------------------------------------------------------------DEAD & PAUSE-------------------------------------------------

	public bool checkIfDead(int playerX, int playerY) {
		if (mapControll.GetArea (playerX - 1, playerY) == null || mapControll.GetArea (playerX - 1, playerY).GetOwner () == null) {
			return false;
		}
		if (mapControll.GetArea (playerX + 1, playerY) == null || mapControll.GetArea (playerX + 1, playerY).GetOwner () == null) {
			return false;
		}
		if (mapControll.GetArea (playerX , playerY- 1) == null || mapControll.GetArea (playerX, playerY-1).GetOwner () == null) {
			return false;
		}
		if (mapControll.GetArea (playerX, playerY + 1) == null || mapControll.GetArea (playerX, playerY+1).GetOwner () == null) {
			return false;
		}
		return true;
	}

	public void killPlayer() {
		this.playerDead = true;
		this.gamePaused = true;
		UIcontroller.playerDead (currentPlayer.GetName(), playerScore);
	}

	public void pause() {
		if (!this.playerDead) {
			this.gamePaused = !this.gamePaused;
			UIcontroller.showPauseMenu (gamePaused);
		}
	}
	//-----------------------------------------------------------------Player config ---------------------------------------

	public void AddPlayer(string name, Color color, int id) {
		playerController.AddPlayer (name, color, id);
		this.playerMaterial.color = color;
	}

	public Player GetPlayerByID(int id) {
		return playerController.GetPlayerByID (id);
	}

	public void SetCurrentPlayer (Player p) {
		this.currentPlayer = p;
	}

	//------------------------------------------------------------------- FIND MOVEMENT ----------------------------------

	public void MovePlayer(int x, int y) {
		currentPlayer.SetNextPos(new Vector2(x, y));
		CreateMap ();
	}

	private void MoveBots() {
		foreach (Player p in playerController.GetAllPlayers()) {
			if(p != currentPlayer) {
				p.SetNextPos(new Vector2(Random.Range(-1,2), Random.Range(-1,2)));
			}
		}
	}
//--------------------------------------------------------------------SPAWN MAP ------------------------------------------------
	private void SpawnMap() {
		Player p = currentPlayer;
		Dictionary<int,Area>  spawnPositions = mapControll.GetMapDataForPlayer(p);
		Dictionary<int, GameObject> spawnedObjects = p.GetSpawnedObjects();

		List<int> objectKeys = new List<int> (spawnedObjects.Keys);

		int currentKey = mapControll.CalculateKey ((int)currentPlayer.GetPos ().x, (int)currentPlayer.GetPos ().y);
		Area currentPlayerArea = mapControll.GetArea (currentKey);
		if (currentPlayerArea.isMushroom ()) {
			//WE FOUND A MUSHROOM!
			addScore(50);
			Debug.Log("WE FOUND A MUSHROOM!");
			Vector2 pos = currentPlayerArea.GetVector2();
			GameObject temp = goController.InstantiateBoxObject(new Vector3(pos.x, pos.y, 0));
			temp.GetComponent<MeshRenderer> ().material = playerMaterial;
			currentPlayerArea.SetAreaObject (new AreaObject ());
			if(spawnedObjects.ContainsKey(currentKey)) {
				GameObject old = spawnedObjects [currentKey];
				spawnedObjects [currentKey] = temp;
				MonoBehaviour.Destroy (old);
			}
		}

		foreach (int objKey in objectKeys) {
			if (!spawnPositions.ContainsKey (objKey) && spawnedObjects.ContainsKey (objKey)) {
				MonoBehaviour.Destroy (spawnedObjects [objKey]);
				spawnedObjects.Remove (objKey);
			}
		}
			foreach(int spawnKey in spawnPositions.Keys) {
				if(!spawnedObjects.ContainsKey(spawnKey)) {
					Area a = spawnPositions [spawnKey];
					Vector2 pos = a.GetVector2();
					GameObject temp = null;
					if (a.GetOwner () != null) {
						temp = goController.InstantiateBoxObject(new Vector3(pos.x, pos.y, 0));
						if (a.GetOwner ().GetID () == groundCountry.GetID ()) {
							temp.GetComponent<MeshRenderer> ().material = groundMaterial;
						} else {
						temp.GetComponent<MeshRenderer> ().material = playerMaterial;
						}
					} else {
						if (a.getAreaObject () == null) {
							a.SetAreaObject (goController.getRandomAreaObject ());
						}
						temp = goController.InstantiateObject (a.getAreaObject().getGameObject(), new Vector3(pos.x, pos.y, 0.5F));
						a.getAreaObject ().SetGameObject (temp);
					}
				if (temp != null) {
					spawnedObjects.Add (spawnKey, temp);
				}
				}
			}
			p.SetSpawnedObjects(spawnedObjects);

		foreach (Player b in playerController.GetAllPlayers()) {
			Vector2 pos = b.GetPos();
			if(spawnPositions.ContainsKey(calculator.CalculateKey((int)pos.x,(int)pos.y))) {
				setCurrentBox (b);
			} else if (b.GetCurrentBox() != null) {
				MonoBehaviour.Destroy(b.GetCurrentBox());
			}
		}
	}

	private void setCurrentBox(Player p) {
		Vector2 pos = p.GetPos ();
		GameObject currentBox = p.GetCurrentBox ();

		if (currentBox != null) {
			MonoBehaviour.Destroy(currentBox);
		}
		currentBox = goController.InstantiateBoxObject(new Vector3(pos.x, pos.y, 0));
		currentBox.GetComponent<MeshRenderer> ().material = playerMaterial;
		
		if (p == currentPlayer) {
			currentBox.gameObject.tag = "Player";
		}
		p.SetCurrentBox (currentBox);
	}
//------------------------------------------------------------------OTHERS ----------------------------------------------------
	public MapController GetMap() {
		return mapControll;
	}

	public PlayerController GetPlayerController() {
		return playerController;
	}
}
