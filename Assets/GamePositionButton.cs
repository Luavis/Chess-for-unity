using UnityEngine;
using System.Collections;

public class GamePositionButton : MonoBehaviour {

	void OnMouseDown () {
		string[] position = this.gameObject.name.Replace("Cube-", "").Split('-');

		Game.GetInstance ().Move (int.Parse (position [0]), int.Parse (position [1]), transform.localPosition.x, transform.localPosition.z);
	}
}
