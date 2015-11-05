using UnityEngine;
using System.Collections;

public class GamePositionButton : MonoBehaviour {

	void OnMouseDown () {
		string[] position = this.gameObject.name.Replace("Cube-", "").Split('-');

		Debug.Log (this.gameObject.name + " Clicked");
		Debug.Log (transform.localPosition.x);
		Debug.Log (transform.localPosition.y);
		Debug.Log (transform.localPosition.z);

		Game.GetInstance ().Move (int.Parse (position [0]), int.Parse (position [1]), transform.localPosition.x, transform.localPosition.z);
	}
}
