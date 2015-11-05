using UnityEngine;
using System.Collections;

public class GamePositionButton : MonoBehaviour {

	void OnMouseDown () {
		Debug.Log (this.gameObject.name + " Clicked");
	}
}
