using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	private GameController gameController;
	private Player currentPlayer;
	public GameObject basicGround;

	private Vector2 direction = new Vector2 (0, 0);
	//private float timer = 0;

	// Use this for initialization
	void Start () {
		this.gameController = new GameController (basicGround);

		string playerName = PlayerPrefs.GetString("player_name", "NoName");
		float r = PlayerPrefs.GetFloat ("player_r", 1);
		float g = PlayerPrefs.GetFloat ("player_g", 1);
		float b = PlayerPrefs.GetFloat ("player_b", 1);

		Color color = new Color (r, g, b);
		int playerID = 100;
		gameController.AddPlayer (playerName, color, playerID);
		currentPlayer = gameController.GetPlayerByID(playerID);
		if (currentPlayer == null) {
			Debug.LogError ("No player found with id: " + playerID);
		} else {
			gameController.SetCurrentPlayer(currentPlayer);
		}
		gameController.CreateMap();
	}

	void Update () {
		FindInput ();
		if (Input.GetMouseButtonUp (0)) {
			MusePressed (Input.mousePosition);
		}

		/*if (timer > 0) {
			timer -= Time.deltaTime;
		} else {
			gameController.PlayerClicked ((int) direction.x, (int) direction.y);
			timer = 0.5F;
		}*/
	}

	public void pauseGame() {
		this.gameController.pause ();
	}


	public void killPlayer() {
		this.gameController.killPlayer ();
	}


	private void MusePressed(Vector3 clickPos) {
		int x = 0;
		int y = 0;
		int screenW = Screen.width;
		int screenH = Screen.height;

		if (clickPos.y  > screenH / 3 && clickPos.y <= (screenH / 3) * 2 && clickPos.x < screenW / 3) {
			//vasen
			x = -1;
			gameController.PlayerClicked (x, y);
			direction.x = x;
			direction.y = y;
			return;
		} else if (clickPos.y  > screenH / 3 && clickPos.y <= (screenH / 3) * 2 && clickPos.x > (screenW / 3) * 2) {
			//oikee
			x = 1;
			gameController.PlayerClicked (x, y);
			direction.x = x;
			direction.y = y;
			return;
		}else if (clickPos.y < screenH / 3) {
			//alas
			y = -1;
			gameController.PlayerClicked (x, y);
			direction.y = y;
			direction.x = x;

			return;
		}  else if (clickPos.y > (screenH / 3 ) && (clickPos.y < (screenH - (screenH / 20)))) {
			//ylös
			y = 1;
			gameController.PlayerClicked (x, y);
			direction.y = y;
			direction.x = x;
			return;
		} else {
			return;
		}
			

	}

	private void FindInput() {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			gameController.PlayerClicked (0,1);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			gameController.PlayerClicked (0,-1);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			gameController.PlayerClicked (1,0);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			gameController.PlayerClicked (-1,0);
		}
		if (Input.GetKeyDown (KeyCode.I)) {
			DebugInfo();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			pauseGame ();
		}
	}

	void OnApplivationFocus(bool focusStatus) {
		if (focusStatus == false) {
			pauseGame ();
		}
	}

	private void DebugInfo() {
		Debug.Log ("Current player: " + currentPlayer.toString());
		Debug.Log ("Countries: " + gameController.GetPlayerController ().GetAllCountries ().Count);
		Debug.Log ("Players: " + gameController.GetPlayerController ().GetAllPlayers ().Count);
		Debug.Log ("Areas size: " + gameController.GetMap().GetAreasSize());
	}
}
