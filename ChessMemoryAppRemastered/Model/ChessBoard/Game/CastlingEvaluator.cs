﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Game
{
    public static class CastlingEvaluator
    {
        public static bool CanCastleKingSide(ChessBoardState chessBoardState, Piece king)
        {
            var territoryState = new TerritoryState(chessBoardState, king.GetEnemyColor());
            int playerSide = king.color == ChessBoardState.PlayerColor.White ? 0 : 7;
            var kingSideTerritorySquares = new List<Coordinate>
            {
                new(5, playerSide),
                new(6, playerSide),
            };

            if (chessBoardState.Pieces.TryGetValue(new Coordinate(7, playerSide), out Piece? piece))
            {
                if (piece is not Rook || piece.color != king.color)
                    return false;
            }
            else
                return false;

            if (kingSideTerritorySquares.Any(territoryState.controlledSquares.ContainsKey))
                return false;
            if (kingSideTerritorySquares.Any(chessBoardState.Pieces.ContainsKey))
                return false;

            return true;
        }

        public static bool CanCastleQueenSide(ChessBoardState chessBoardState, Piece king)
        {
            var territoryState = new TerritoryState(chessBoardState, king.GetEnemyColor());
            int playerSide = king.color == ChessBoardState.PlayerColor.White ? 0 : 7;
            var queenSideTerritorySquares = new List<Coordinate>
            {
                new(3, playerSide),
                new(4, playerSide),
            };
            var queenSidePieceSquares = new List<Coordinate>
            {
                new(2, playerSide),
                new(3, playerSide),
                new(4, playerSide),
            };

            if (chessBoardState.Pieces.TryGetValue(new Coordinate(7, playerSide), out Piece? piece))
            {
                if (piece is not Rook || piece.color != king.color)
                    return false;
            }
            else
                return false;

            if (queenSideTerritorySquares.Any(territoryState.controlledSquares.ContainsKey))
                return false;
            if (queenSidePieceSquares.Any(chessBoardState.Pieces.ContainsKey))
                return false;

            return true;
        }
    }
}
