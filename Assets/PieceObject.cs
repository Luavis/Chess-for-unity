using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PieceObject {
	public GameObject gameObject;
	public PieceType pieceType;

	public int row;
	public int col;

	public bool isMoved = false;

	public bool IsBlackPiece() {
		return (this.pieceType == PieceType.BlackBishop || 
			this.pieceType == PieceType.BlackPawn || 
			this.pieceType == PieceType.BlackKing || 
			this.pieceType == PieceType.BlackKnight || 
	        this.pieceType == PieceType.BlackRock ||
			this.pieceType == PieceType.BlackQueen);
	}

	public bool IsPawn() {
		return (this.pieceType == PieceType.BlackPawn || this.pieceType == PieceType.WhitePawn);
	}

	public bool IsRock() {
		return (this.pieceType == PieceType.BlackRock || this.pieceType == PieceType.WhiteRock);
	}

	public bool IsKing() {
		return (this.pieceType == PieceType.BlackKing || this.pieceType == PieceType.WhiteKing);
	}

	public bool IsQueen() {
		return (this.pieceType == PieceType.BlackQueen || this.pieceType == PieceType.WhiteQueen);
	}

	public bool IsKnight() {
		return (this.pieceType == PieceType.BlackKnight || this.pieceType == PieceType.WhiteKnight);
	}

	public bool IsBishop() {
		return (this.pieceType == PieceType.BlackBishop || this.pieceType == PieceType.WhiteBishop);
	}

	public bool Move(int row, int col) {
		if (!this.IsAvaibleToGo (row, col, this.IsBlackPiece()))
			return false;

		bool found = false;

		foreach(int[] pos in this.GetAvailableListToGo()) {
			if(pos[0] == row && pos[1] == col) {
				found = true;
				break;
			}
		}

		if (!found)
			return false;

		this.row = row;
		this.col = col;
		this.isMoved = true;

		return true;
	}

	public int[][] GetAvailableListToGo() {
		bool isBlack = this.IsBlackPiece ();
		int[][] ret = new int[][] {};

		if (this.IsPawn ()) { // when pawn
			
			int delta = isBlack ? 1 : -1;

			if ((isBlack && this.row == 8) || (!isBlack && this.row == 1)) // is end to pawn
				return ret; //return empty

			if (this.isMoved)
				ret = new int[][] {
					new int[]{this.row + delta, this.col}
				};
			else
				ret = new int[][] {
					new int[]{this.row + delta, this.col},
					new int[]{this.row + delta * 2, this.col},
				};
		} else if (this.IsKnight ()) {
			List<int[]> candidate = new List<int[]> ();
			int[] rowDelta = new int[]{2, -2};
			int[] colDelta = new int[]{1, -1};

			foreach (int dRow in rowDelta) {
				foreach (int cRow in colDelta) {
					if (this.row + dRow >= 1 && this.row + dRow <= 8 &&
						this.col + cRow >= 1 && this.col + cRow <= 8
					   ) {
						if (IsAvaibleToGo (this.row + dRow, this.col + cRow, isBlack))
							candidate.Add (new int[] {this.row + dRow, this.col + cRow});
					}
				}
			}

			ret = candidate.ToArray ();
		} else if (this.IsBishop ()) {
			List<int[]> candidate = new List<int[]> ();

			int delta = 1;
			bool checkPP = true;
			bool checkPM = true;
			bool checkMP = true;
			bool checkMM = true;


			while (delta < 8) {

				if (checkPP && IsAvaibleToGo (this.row + delta, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col + delta});
				else
					checkPP = false;

				if (checkPM && IsAvaibleToGo (this.row + delta, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col - delta});
				else
					checkPM = false;

				if (checkMP && IsAvaibleToGo (this.row - delta, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col + delta});
				else
					checkMP = false;

				if (checkMM && IsAvaibleToGo (this.row - delta, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col - delta});
				else
					checkMM = false;

				delta += 1;
			}

			ret = candidate.ToArray ();
		} else if (this.IsRock ()) {
			List<int[]> candidate = new List<int[]> ();
			
			int delta = 1;
			bool checkPP = true;
			bool checkPM = true;
			bool checkMP = true;
			bool checkMM = true;
			
			
			while (delta < 8) {

				if (checkPP && IsAvaibleToGo (this.row + delta, this.col, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col});
				else
					checkPP = false;
				
				if (checkPM && IsAvaibleToGo (this.row - delta, this.col, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col});
				else
					checkPM = false;
				
				if (checkMP && IsAvaibleToGo (this.row, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row, this.col + delta});
				else
					checkMP = false;
				
				if (checkMM && IsAvaibleToGo (this.row, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row, this.col - delta});
				else
					checkMM = false;
				
				delta += 1;
			}
			
			ret = candidate.ToArray ();
		} else if (this.IsQueen ()) {
			List<int[]> candidate = new List<int[]> ();
			
			int delta = 1;
			bool checkPP = true;
			bool checkPM = true;
			bool checkMP = true;
			bool checkMM = true;

			bool checkPP2 = true;
			bool checkPM2 = true;
			bool checkMP2 = true;
			bool checkMM2 = true;
			
			
			while (delta < 8) {

				if (checkPP && IsAvaibleToGo (this.row + delta, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col + delta});
				else
					checkPP = false;
				
				if (checkPM && IsAvaibleToGo (this.row + delta, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col - delta});
				else
					checkPM = false;
				
				if (checkMP && IsAvaibleToGo (this.row - delta, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col + delta});
				else
					checkMP = false;
				
				if (checkMM && IsAvaibleToGo (this.row - delta, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col - delta});
				else
					checkMM = false;

				// check up down

				if (checkPP2 && IsAvaibleToGo (this.row + delta, this.col, isBlack))
					candidate.Add (new int[] {this.row + delta, this.col});
				else
					checkPP2 = false;
				
				if (checkPM2 && IsAvaibleToGo (this.row - delta, this.col, isBlack))
					candidate.Add (new int[] {this.row - delta, this.col});
				else
					checkPM2 = false;
				
				if (checkMP2 && IsAvaibleToGo (this.row, this.col + delta, isBlack))
					candidate.Add (new int[] {this.row, this.col + delta});
				else
					checkMP2 = false;
				
				if (checkMM2 && IsAvaibleToGo (this.row, this.col - delta, isBlack))
					candidate.Add (new int[] {this.row, this.col - delta});
				else
					checkMM2 = false;
				
				delta += 1;
			}
			
			ret = candidate.ToArray ();
		} else if (this.IsKing ()) {
			List<int[]> candidate = new List<int[]> ();
			
			int delta = 1;
			
			if (IsAvaibleToGo (this.row + delta, this.col + delta, isBlack))
				candidate.Add (new int[] {this.row + delta, this.col + delta});

			if (IsAvaibleToGo (this.row + delta, this.col - delta, isBlack))
				candidate.Add (new int[] {this.row + delta, this.col - delta});

			if (IsAvaibleToGo (this.row - delta, this.col + delta, isBlack))
				candidate.Add (new int[] {this.row - delta, this.col + delta});

			if (IsAvaibleToGo (this.row - delta, this.col - delta, isBlack))
				candidate.Add (new int[] {this.row - delta, this.col - delta});

			// check up down
			
			if (IsAvaibleToGo (this.row + delta, this.col, isBlack))
				candidate.Add (new int[] {this.row + delta, this.col});

			if (IsAvaibleToGo (this.row - delta, this.col, isBlack))
				candidate.Add (new int[] {this.row - delta, this.col});

			if (IsAvaibleToGo (this.row, this.col + delta, isBlack))
				candidate.Add (new int[] {this.row, this.col + delta});

			if (IsAvaibleToGo (this.row, this.col - delta, isBlack))
				candidate.Add (new int[] {this.row, this.col - delta});

			ret = candidate.ToArray ();
		}

		return ret;
	}

	private bool IsAvaibleToGo(int row, int col, bool isBlack) {
		PieceType[][] chessBoard = Game.GetInstance ().GetChessBoard ();

		row--;
		col--; // row and col is 1-base(note zero-base)

		if (row < 0 || row >= 8 || col < 0 || col >= 8)
			return false;

		if (chessBoard [row] [col] == PieceType.None) {
			return true;
		} else if(!isBlack && (
			chessBoard [row] [col] == PieceType.BlackKing ||
			chessBoard [row] [col] == PieceType.BlackQueen ||
			chessBoard [row] [col] == PieceType.BlackPawn ||
			chessBoard [row] [col] == PieceType.BlackKnight ||
			chessBoard [row] [col] == PieceType.BlackBishop ||
			chessBoard [row] [col] == PieceType.BlackRock)){
			return true;
		}
		else if(isBlack && (
			chessBoard [row] [col] == PieceType.WhiteKing ||
			chessBoard [row] [col] == PieceType.WhiteQueen ||
			chessBoard [row] [col] == PieceType.WhitePawn ||
			chessBoard [row] [col] == PieceType.WhiteKnight ||
			chessBoard [row] [col] == PieceType.WhiteBishop ||
			chessBoard [row] [col] == PieceType.WhiteRock
			)) {
			return true; // target is white and row-col is white
		}
		else 
			return false;
	}
}
