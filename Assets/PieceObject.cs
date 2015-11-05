using UnityEngine;
using System.Collections;

public class PieceObject {
	public GameObject gameObject;
	public PieceType pieceType;

	public int row;
	public int col;

	public bool isMoved = false;

	public bool isBlackPiece() {
		return (this.pieceType == PieceType.BlackBishop || 
			this.pieceType == PieceType.BlackPawn || 
			this.pieceType == PieceType.BlackKing || 
			this.pieceType == PieceType.BlackKnight || 
	        this.pieceType == PieceType.BlackRock ||
			this.pieceType == PieceType.BlackQueen);
	}

	public bool isPawn() {
		return (this.pieceType == PieceType.BlackPawn || this.pieceType == PieceType.WhitePawn);
	}

	public bool isRock() {
		return (this.pieceType == PieceType.BlackRock || this.pieceType == PieceType.WhiteRock);
	}

	public bool isKing() {
		return (this.pieceType == PieceType.BlackKing || this.pieceType == PieceType.WhiteKing);
	}

	public bool isQeen() {
		return (this.pieceType == PieceType.BlackQueen || this.pieceType == PieceType.WhiteQueen);
	}

	public bool isKnight() {
		return (this.pieceType == PieceType.BlackKnight || this.pieceType == PieceType.WhiteKnight);
	}

	public bool isBishop() {
		return (this.pieceType == PieceType.BlackBishop || this.pieceType == PieceType.WhiteBishop);
	}
}
