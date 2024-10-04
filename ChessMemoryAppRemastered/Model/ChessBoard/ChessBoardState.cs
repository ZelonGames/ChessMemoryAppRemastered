﻿using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public enum PlayerColor
    {
        White,
        Black
    }
    public record ChessBoardState(
        ImmutableDictionary<Coordinate, Piece> Pieces,
        PlayerColor CurrentTurn,
        CastlingState CastlingState,
        Coordinate? EnpassantTarget,
        int FiftyMoveRuleCounter,
        int FullMoves);
}
