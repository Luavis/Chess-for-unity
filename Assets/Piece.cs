using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

	void Start() {

	}
	// Use this for initialization
	void OnMouseDown () {
		Behaviour halo = this.GetComponent("Halo") as Behaviour;
		halo.enabled = !halo.enabled;

		ParticleSystem a = GameObject.Find ("/light_1/a").GetComponentInChildren<ParticleSystem>();
		a.Play();

		if (halo.enabled)
			Game.getInstance().SelectPiece(this); 
		else
			Game.getInstance().DeselectPiece(this); 
	}
}
