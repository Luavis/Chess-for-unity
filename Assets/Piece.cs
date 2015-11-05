using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {

	private Vector3 targetVec; // do not move

	void Start() {
		targetVec = this.transform.localPosition;
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

	public void MoveInLocal(Vector3 vec) {
		targetVec = vec;
	}

	void Update() {
		if (targetVec == null)
			return;

		float step = (float) 0.5 * Time.deltaTime;
		this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition,  targetVec, step);
	}
}
