using UnityEngine;
using System.Collections;

public class CalculateCordinates {

	public CalculateCordinates() {
		///// MAX VAlUE = 10 000 x 10 000!!!!
	}

	//----------------------------------------------------------ADDITIONAL FUNCTIONS -------------------------------------
	
	public int CalculateKey(int x, int y) {
		long sum1 = (long)x + (long)y;
		long sum2 = sum1 + 1;
		long finalSum = ((sum1 * sum2) / 2) + y;
		return (int)finalSum;
	}
	
	public Vector2 CalculateCordinatesFromKey(int key) {
		float sum1 = Mathf.Sqrt((8 * key) + 1);
		long w = (long)(sum1 - 1) / 2;
		int t = (int)(w * (w + 1)) / 2;
		int y = key - t;
		int x = (int)w - y;
		return new Vector2(x,y); 
	}
}
