using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
	private Text scoreText;
	public GameObject scoreTextObject;
	public GameObject highScorePanel;
	public GameObject highScoreContentPanel;
	public GameObject scorePanelPrefab;
	public GameObject pauseMenu;
	public GameObject tutorial;

	public Font highscoreFont;

	private string secretKey = "194329"; // Edit this value and make sure it's the same as the one stored on the server
	private string sendScoreURL = "http://starsrent.fi/Area/addscore.php?"; //be sure to add a ? to your url
	private string getScoresURL = "http://starsrent.fi/Area/display.php";

	//LIsää jossai vaihees joku checki et mikä on pienin arvo highscoreissa ettei turhaan tukkiinnu
	private int tutorialClickCount = 0;

	void Start() {
		scoreText = scoreTextObject.GetComponent<Text>();

	}

	public void showPauseMenu(bool paused) {
		pauseMenu.SetActive (paused);
	}

	public void updateScoreText(int newScore) {
		if (scoreText != null) {
			scoreText.text = "" + newScore;
		}
	}

	public void playerDead(string name, int score) {
		//Show highscores
		StartCoroutine(SendScore (score, name));
		highScorePanel.SetActive(true);
	}

	public void restartGame() {
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene, LoadSceneMode.Single);
	}

	public void exitGame() {
		Debug.Log ("exit!");
		int scene = SceneManager.GetActiveScene ().buildIndex-1;
		SceneManager.LoadScene (scene, LoadSceneMode.Single);
	}

	public void hideTutorial() {
		tutorialClickCount++;
		if (tutorialClickCount >= 5) {
			tutorial.SetActive (false);
		}
	}


	IEnumerator SendScore (int score, string name)
	{
		//This connects to a server side php script that will add the name and score to a MySQL DB.
		// Supply it with a string representing the players name and the players score.
		string hash = Md5Sum(name + score + secretKey);

		string post_url = sendScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash;

		// Post the URL to the site and create a download object to get the result.
		WWW hs_post = new WWW(post_url);
		yield return hs_post; // Wait until the download is done
		if (hs_post.error != null) {
			Debug.Log ("There was an error, try again later: " + hs_post.error);
		} 
		yield return new WaitForSeconds (2);
		StartCoroutine(GetData());
	}

	IEnumerator GetData()
	{
		WWW hs_get = new WWW(getScoresURL);
		yield return hs_get;

		if (hs_get.error != null) {
			Debug.Log("There was an error, try again later: " + hs_get.error);
		} else {
			string serverText = hs_get.text;
			string[] strings = ExplodeMainString (serverText, '\n');
			int offsetY = 0;
			foreach (string itemString in strings) {
				yield return new WaitForSeconds (0.5F);
				string[] specs = ExplodeMainString (itemString, '\t');
				if (specs.Length == 2) {
					string name = specs [0];
					int score = IntParseFast (specs [1]);

					if (scorePanelPrefab != null) {
						GameObject go = (GameObject)Instantiate (scorePanelPrefab, new Vector3 (0, -100 - offsetY, 0), Quaternion.identity);
						go.transform.SetParent (highScoreContentPanel.transform, false);

						GameObject scoreGO = new GameObject ();
						GameObject nameGO = new GameObject ();
						scoreGO.transform.SetParent (go.transform, false);
						nameGO.transform.SetParent (go.transform, false);

						Vector3 scorePos = new Vector3 (-480, 0, 0);
						scoreGO.transform.localPosition = scorePos;

						Vector3 namePos = new Vector3 (480, -0, 0);
						nameGO.transform.localPosition = namePos;


						Text nameText = nameGO.AddComponent<Text>();
						Text scoreText = scoreGO.AddComponent<Text>();
						nameGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 60);
						scoreGO.GetComponent<RectTransform> ().sizeDelta = new Vector2 (400, 60);
						nameText.font = highscoreFont;
						scoreText.font = highscoreFont;

						nameText.alignment = TextAnchor.MiddleCenter;
						scoreText.alignment = TextAnchor.MiddleCenter;

						nameText.resizeTextForBestFit = true;
						scoreText.resizeTextForBestFit = true;

						nameText.resizeTextMaxSize = 300;
						scoreText.resizeTextMaxSize = 300;

						scoreText.text = "" + score;
						nameText.text = name;

						offsetY += 60;
					}
				}
			}
		}
	}

	public  string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}

	private string[] ExplodeMainString(string main, char splitChar)
	{
		return main.Split (splitChar);
	}

	public static int IntParseFast(string value)
	{
		int result = 0;
		for (int i = 0; i < value.Length; i++)
		{
			char letter = value[i];
			result = 10 * result + (letter - 48);
		}
		return result;
	}
}
