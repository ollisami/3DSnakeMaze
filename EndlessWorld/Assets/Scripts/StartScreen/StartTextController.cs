using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartTextController : MonoBehaviour {
	public List<GameObject> textObjects;
	float timer = 1.0F;
	float speed = 15.0F;
	private List<GameObject> droppingObject = new List<GameObject>();

	public Image previewImage;
	public GameObject settingWindow;
	public Text playerNameText;

	private float player_r;
	private float player_g;
	private float player_b;
	private string playerName;

	// Use this for initialization
	void Start () {
		player_r = PlayerPrefs.GetFloat ("player_r", 1);
		player_g = PlayerPrefs.GetFloat ("player_g", 1);
		player_b = PlayerPrefs.GetFloat ("player_b", 1);

		playerName = PlayerPrefs.GetString ("player_name", "NoName");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (timer >= 0) {
			timer -= Time.deltaTime;
		} else if (textObjects.Count > 0) {
			dropABlock ();
			timer += 0.1F;
		}

		if (droppingObject.Count != 0) {
			for (int i = 0; i < droppingObject.Count; i++) {
				Vector3 pos = droppingObject[i].transform.position;
				if (pos.z < -0.5F) {
					pos.z += speed * Time.deltaTime;
					droppingObject[i].transform.position = pos;
				} else {
					droppingObject.Remove(droppingObject[i]);
				}
			}
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

	private void dropABlock() {
		int i = Random.Range (0, textObjects.Count - 1);
		droppingObject.Add(textObjects [i]);
		textObjects.Remove (textObjects [i]);
	}

	public void startGame() {
		saveValues ();
		int scene = SceneManager.GetActiveScene ().buildIndex+1;
		SceneManager.LoadScene (scene, LoadSceneMode.Single);
	}

	public void showSettings() {
		if (textObjects.Count < 40) {
			settingWindow.SetActive (true);
			playerNameText.text = playerName;
			setPreviewImageColor ();
		}
	}

	public void setColorR(float amount) {
		player_r = amount;
		setPreviewImageColor ();

	}

	public void setColorG(float amount) {
		player_g = amount;
		setPreviewImageColor ();

	}

	public void setColorB(float amount) {
		player_b = amount;
		setPreviewImageColor ();

	}

	public void setPlayerName(string name) {
		playerName = name;
	}

	private void setPreviewImageColor() {
		previewImage.color = new Color (player_r, player_g, player_b);
	}

	private void saveValues() {
		PlayerPrefs.SetString ("player_name", playerName);
		PlayerPrefs.SetFloat ("player_r", player_r);
		PlayerPrefs.SetFloat ("player_g", player_g);
		PlayerPrefs.SetFloat ("player_b", player_b);
	}
}
