using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

	void Start() {

	}
	// Use this for initialization
	void OnMouseDown () {
		Behaviour halo = this.GetComponent("Halo") as Behaviour;
		halo.enabled = !halo.enabled;

		if (halo.enabled)
			Game.GetInstance().SelectPiece(this); 
		else
			Game.GetInstance().DeselectPiece(this); 
	}
}
