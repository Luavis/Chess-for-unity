using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;
using System;

public enum PieceType {
	WhiteKing = 1,
	WhiteQueen = 2,
	WhiteBishop = 3,
	WhiteKnight = 4,
	WhitePawn = 5,
	WhiteRock = 6,
	
	BlackKing = 11,
	BlackQueen = 12,
	BlackBishop = 13,
	BlackKnight = 14,
	BlackPawn = 15,
	BlackRock = 16,

	None = 0
};

enum GameTurn {
	White = 1,
	Black = 2
};

public class Game : MonoBehaviour {

	static string[] whitePieceNames = {
		"Chess_King_A",
		"Chess_Queen_A",
		"Chess_Bishop_A1",
		"Chess_Bishop_A2",
		"Chess_Knight_A1",
		"Chess_Knight_A2",
		"Chess_Rock_A1",
		"Chess_Rock_A2",
		"Chess_Pawn_A1",
		"Chess_Pawn_A2",
		"Chess_Pawn_A3",
		"Chess_Pawn_A4",
		"Chess_Pawn_A5",
		"Chess_Pawn_A6",
		"Chess_Pawn_A7",
		"Chess_Pawn_A8",
	};

	static string[] blackPieceNames = {
		"Chess_King_B",
		"Chess_Queen_B",
		"Chess_Bishop_B1",
		"Chess_Bishop_B2",
		"Chess_Knight_B1",
		"Chess_Knight_B2",
		"Chess_Rock_B1",
		"Chess_Rock_B2",
		"Chess_Pawn_B1",
		"Chess_Pawn_B2",
		"Chess_Pawn_B3",
		"Chess_Pawn_B4",
		"Chess_Pawn_B5",
		"Chess_Pawn_B6",
		"Chess_Pawn_B7",
		"Chess_Pawn_B8",
	};

	static public Game singleton;

	PieceType[][] chessBoard = new PieceType[][]{
		new PieceType[]{PieceType.BlackRock, PieceType.BlackKnight, PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackQueen, PieceType.BlackBishop, PieceType.BlackKnight, PieceType.BlackRock},
		new PieceType[]{PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn, PieceType.BlackPawn},
		new PieceType[]{PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, },
		new PieceType[]{PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, },
		new PieceType[]{PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, },
		new PieceType[]{PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, PieceType.None, },
		new PieceType[]{PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn, PieceType.WhitePawn},
		new PieceType[]{PieceType.WhiteRock, PieceType.WhiteKnight, PieceType.WhiteBishop, PieceType.WhiteKing, PieceType.WhiteQueen, PieceType.WhiteBishop, PieceType.WhiteKnight, PieceType.WhiteRock},
	};

	Dictionary<string, PieceObject> pieceObjects = new Dictionary<string, PieceObject>();

	GameTurn turn = GameTurn.Black;
	PieceObject selectedObject = null;
	Piece selectedPiece = null;

	public Text winMessage;

	static public Game GetInstance() {
		return singleton;
	}

	public void Move(int row, int col, float x, float z, bool ignore = false) {

		if (selectedObject == null || selectedPiece == null)
			return;

//		if (selectedObject == null)
//			return;

		int originRow = selectedObject.row;
		int originCol = selectedObject.col;

		if (selectedObject.Move (row, col)) {
			selectedPiece.MoveInLocal (new Vector3 (x, selectedPiece.transform.localPosition.y, z));
		} else {
			return;
		}

		if (this.chessBoard [row - 1] [col - 1] != PieceType.None) {
			string[] keys = (turn == GameTurn.Black) ? whitePieceNames : blackPieceNames;

			foreach(string key in keys) {
				if(pieceObjects[key].row == row && pieceObjects[key].col == col) {
					if(pieceObjects[key].IsKing()) {
						BlurOptimized blur = GameObject.Find ("Main Camera").GetComponent<BlurOptimized> ();
						Debug.Log (blur);
						blur.enabled = true;

						if(turn == GameTurn.White) {
							winMessage.text = "White user Win!";
						}
						else
							winMessage.text = "Black user Win!";
					}
					MeshRenderer renderer = pieceObjects[key].gameObject.GetComponent<MeshRenderer>();
					pieceObjects[key].isDead = true;
					renderer.enabled = false;
					pieceObjects[key].row = -1;
					pieceObjects[key].col = -1;
				}
			}
		}

		if (Application.platform == RuntimePlatform.Android && !ignore) {
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) {
					obj_Activity .CallStatic ("sendPointToPoint", originRow, originCol, row, col);
				}
			}
		}

		this.chessBoard [row - 1] [col - 1] = selectedObject.pieceType;
		this.chessBoard [originRow - 1] [originCol - 1] = PieceType.None;

		this.ChangeTurn ();

		this.DeselectPiece (selectedPiece); // deselect!
	}

	public PieceType[][] GetChessBoard() {
		return this.chessBoard;
	}

	public void ChangeTurn() {
		if (this.turn == GameTurn.Black)
			this.turn = GameTurn.White;
		else 
			this.turn = GameTurn.Black;

		if (selectedObject != null) {
			this.DeselectPiece (selectedPiece);
		}

		foreach (string name in whitePieceNames) {
			GameObject piece = pieceObjects[name].gameObject;
			BoxCollider collider = piece.GetComponent<BoxCollider>();
			if(pieceObjects[name].isDead) {
				collider.enabled = false;
			}
			else {
				collider.enabled = (GameTurn.Black != this.turn);
			}
		}

		foreach (string name in blackPieceNames) {
			GameObject piece = pieceObjects[name].gameObject;
			BoxCollider collider = piece.GetComponent<BoxCollider>();
			if(pieceObjects[name].isDead) {
				collider.enabled = false;
			}
			else {
				collider.enabled = (GameTurn.Black == this.turn);
			}
		}
	}

	public void SelectPiece(Piece piece) {
		this.StopParticlesAnimation();

		foreach (Transform child in transform) {
			Behaviour otherHalo = child.gameObject.GetComponent("Halo") as Behaviour;
			if(otherHalo == null)
				continue;

			otherHalo.enabled = false;
		}

		Behaviour halo = piece.GetComponent("Halo") as Behaviour;
		halo.enabled = true;
		
		selectedObject = pieceObjects [piece.name];
		selectedPiece = piece;

		int[][] availablePositionList = selectedObject.GetAvailableListToGo ();

		for(int i = 0; i < availablePositionList.Length; i++){
			ParticleSystem pSystem = GameObject.Find ("light_" + availablePositionList[i][0] + "/" + availablePositionList[i][1])
				.GetComponentInChildren<ParticleSystem>();
			if(pSystem == null) continue;

			pSystem.Play();
		}
	}
	
	public void DeselectPiece(Piece piece) {
		Behaviour halo = piece.GetComponent("Halo") as Behaviour;
		halo.enabled = false;

		selectedObject = null;
		selectedPiece = null;
		this.StopParticlesAnimation ();
 }

	private void StopParticlesAnimation() {
		for (int i = 0; i < chessBoard.Length; i++) {
			foreach (Transform child in GameObject.Find("light_" + (i + 1)).transform) {
				ParticleSystem pSystem = child.gameObject.GetComponentInChildren<ParticleSystem>();
				pSystem.Stop ();
			}
		}
	}

	// Use this for initialization

	void Awake() {
		singleton = this;
	}

	void Start () {

		foreach(string whitePieceName in whitePieceNames) {
			GameObject piece = GameObject.Find(whitePieceName);
			PieceObject obj = new PieceObject();
			obj.gameObject = piece;

			if(whitePieceName.StartsWith("Chess_King")) {
				obj.pieceType = PieceType.WhiteKing;
				obj.row = 8;
				obj.col = 4;
			}
			else if(whitePieceName.StartsWith("Chess_Queen")) {
				obj.pieceType = PieceType.WhiteQueen;
				obj.row = 8;
				obj.col = 5;
			}
			else if(whitePieceName.StartsWith("Chess_Bishop")) {
				obj.pieceType = PieceType.WhiteBishop;

				obj.row = 8;
				if(whitePieceName.EndsWith("1"))
					obj.col = 3;
				else
					obj.col = 6;
			}
			else if(whitePieceName.StartsWith("Chess_Knight")) {
				obj.pieceType = PieceType.WhiteKnight;

				obj.row = 8;
				if(whitePieceName.EndsWith("1"))
					obj.col = 2;
				else
					obj.col = 7;
			}
			else if(whitePieceName.StartsWith("Chess_Rock")) {
				obj.pieceType = PieceType.WhiteRock;

				obj.row = 8;
				if(whitePieceName.EndsWith("1"))
					obj.col = 1;
				else
					obj.col = 8;
			}
			else if(whitePieceName.StartsWith("Chess_Pawn")) {
				obj.pieceType = PieceType.WhitePawn;

				obj.row = 7;
				obj.col = int.Parse(whitePieceName[12].ToString());
			}

			pieceObjects.Add(whitePieceName, obj);
		}

		foreach(string blackPieceName in blackPieceNames) {
			GameObject piece = GameObject.Find(blackPieceName);
			PieceObject obj = new PieceObject();
			obj.gameObject = piece;
			
			if(blackPieceName.StartsWith("Chess_King")) {
				obj.pieceType = PieceType.BlackKing;

				obj.row = 1;
				obj.col = 4;
			}
			else if(blackPieceName.StartsWith("Chess_Queen")) {
				obj.pieceType = PieceType.BlackQueen;

				obj.row = 1;
				obj.col = 5;
			}
			else if(blackPieceName.StartsWith("Chess_Bishop")) {
				obj.pieceType = PieceType.BlackBishop;

				obj.row = 1;
				if(blackPieceName.EndsWith("1"))
					obj.col = 3;
				else
					obj.col = 6;
			}
			else if(blackPieceName.StartsWith("Chess_Knight")) {
				obj.pieceType = PieceType.BlackKnight;

				obj.row = 1;
				if(blackPieceName.EndsWith("1"))
					obj.col = 2;
				else
					obj.col = 7;
			}
			else if(blackPieceName.StartsWith("Chess_Rock")) {
				obj.pieceType = PieceType.BlackRock;

				obj.row = 1;
				if(blackPieceName.EndsWith("1"))
					obj.col = 1;
				else
					obj.col = 8;
			}
			else if(blackPieceName.StartsWith("Chess_Pawn")) {
				obj.pieceType = PieceType.BlackPawn;

				obj.row = 2;
				obj.col = int.Parse(blackPieceName[12].ToString());
			}
			
			pieceObjects.Add(blackPieceName, obj);
			
		}

		foreach (string name in whitePieceNames) {
			GameObject piece = pieceObjects[name].gameObject;
			BoxCollider collider = piece.GetComponent<BoxCollider>();
			collider.enabled = (GameTurn.Black != this.turn);
		}
		
		foreach (string name in blackPieceNames) {
			GameObject piece = pieceObjects[name].gameObject;
			BoxCollider collider = piece.GetComponent<BoxCollider>();
			collider.enabled = (GameTurn.Black == this.turn);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void recieveMsg(string msg) {
		string[] androidMessage = msg.Split (',');

		if (androidMessage.Length != 4)
			return;


		Vector3 targetPosition = GameObject.Find ("Cube-" + androidMessage [2] + "-" + androidMessage [3]).transform.localPosition;

		string[] keys = (turn != GameTurn.Black) ? whitePieceNames : blackPieceNames;
		int row = int.Parse (androidMessage [0]);
		int col = int.Parse (androidMessage [1]);

		foreach(string key in keys) {
			if(pieceObjects[key].row == row && pieceObjects[key].col == col) {
				selectedObject = pieceObjects[key];
				selectedPiece = selectedObject.gameObject.GetComponent<Piece> ();
			}
		}

		this.Move (int.Parse(androidMessage [2]), int.Parse (androidMessage [3]), targetPosition.x, targetPosition.z, true);
	}
}
