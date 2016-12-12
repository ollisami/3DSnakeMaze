using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CountryNamesAndColors {
	private List<string> names;
	private List<Color> colors;

	public CountryNamesAndColors() {
		this.names = GetNameList ();
		this.colors = GetColorList (this.names.Count);
	}


	public int GetSize() {
		return this.names.Count;
	}

	public List<string> GetNames() {
		return this.names;
	}

	public List<Color> GetColors() {
		return this.colors;
	}


	private List<Color> GetColorList(int amount) {
		List<Color> colors = new List<Color> ();
		float r = 0.1F;
		float g = 0.1F;
		float b = 0.1F;
		for (int i = 0; i < amount; i++) {
			if(r + (0.05F) < 0.95F) {
				r += 0.05F;
			} else if(g + (0.05F) < 0.95F) {
				g += 0.05F;
			} else if(b + (0.05F) < 0.95F) {
				b += 0.05F;
			} else {
				r = Random.Range(1, 100) / 100;
				g = Random.Range(1, 100) / 100;
				b = Random.Range(1, 100) / 100;
				Debug.Log("Cant add anymore colors, creating new random values!");
			}
			Color c = new Color(r,g,b);
			colors.Add(c);
		}
		return colors;
	}

	private List<string> GetNameList() {
		List<string> names = new List<string> ();
		names.Add ("Russia");
		names.Add ("China");
		names.Add ("Canada");
		names.Add ("United States");
		names.Add ("Brazil");
		names.Add ("Australia");
		names.Add ("India");
		names.Add ("Argentina");
		names.Add ("Kazakhstan");
		names.Add ("Algeria");
		names.Add ("Sudan");
		names.Add ("Congo");
		names.Add ("Saudi Arabia");
		names.Add ("Mexico");
		names.Add ("Indonesia");
		names.Add ("Libya");
		names.Add ("Iran");
		names.Add ("Mongolia");
		names.Add ("Peru");
		names.Add ("Niger");
		names.Add ("Chad");
		names.Add ("Angola");
		names.Add ("South Africa");
		names.Add ("Mali");
		names.Add ("Ethiopia");
		names.Add ("Bolivia");
		names.Add ("Colombia");
		names.Add ("Mauritania");
		names.Add ("Egypt");
		names.Add ("Nigeria");
		names.Add ("Tanzania");
		names.Add ("Venezuela");
		names.Add ("Namibia");
		names.Add ("Mozambique");
		names.Add ("Pakistan");
		names.Add ("Turkey");
		names.Add ("Chile");
		names.Add ("Zambia");
		names.Add ("Burma");
		names.Add ("Afghanistan");
		names.Add ("Somalia");
		names.Add ("Ukraine");
		names.Add ("Botswana");
		names.Add ("Madagascar");
		names.Add ("Kenya");
		names.Add ("France");
		names.Add ("Yemen");
		names.Add ("Thailand");
		names.Add ("Spain");
		names.Add ("Turkmenistan");
		names.Add ("Cameroon");
		names.Add ("Papua New Guinea");
		names.Add ("Uzbekistan");
		names.Add ("Morocco");
		names.Add ("Iraq");
		names.Add ("Sweden");
		names.Add ("Paraguay");
		names.Add ("Japan");
		names.Add ("Zimbabwe");
		names.Add ("Germany");
		names.Add ("Greenland");
		names.Add ("Malaysia");
		names.Add ("Vietnam");
		names.Add ("Norway");
		names.Add ("Finland");
		names.Add ("Poland");
		names.Add ("Philippines");
		names.Add ("Italy");
		names.Add ("Ecuador");
		names.Add ("Burkina Faso");
		names.Add ("New Zealand");
		names.Add ("Western Sahara");
		names.Add ("Gabon");
		names.Add ("Guinea");
		names.Add ("United Kingdom");
		names.Add ("Laos");
		names.Add ("Romania");
		names.Add ("Ghana");
		names.Add ("Oman");
		names.Add ("Belarus");
		names.Add ("Uganda");
		names.Add ("Kyrgyzstan");
		names.Add ("Guyana");
		names.Add ("Senegal");
		names.Add ("Syria");
		names.Add ("Cambodia");
		names.Add ("Uruguay");
		names.Add ("Suriname");
		names.Add ("Tunisia");
		names.Add ("Tajikistan");
		names.Add ("Nepal");
		names.Add ("Bangladesh");
		names.Add ("Greece");
		names.Add ("Eritrea");
		names.Add ("Korea");
		names.Add ("Nicaragua");
		names.Add ("Honduras");
		names.Add ("Other");
		names.Sort ();
		return names;
	}
}
