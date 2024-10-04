using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    public record ChessBoardState
    {
        public enum PlayerColor
        {
            White,
            Black
        }

        public ImmutableDictionary<Coordinate, Piece> Pieces { get; init; }

        public PlayerColor CurrentTurn { get; init; }

        public CastlingState CastlingState { get; init; }

        public Coordinate? EnpassantTarget { get; init; }

        public int FiftyMoveRuleCounter { get; init; }

        public int FullMoves { get; init; }

        public ChessBoardState(
            ImmutableDictionary<Coordinate, Piece> pieces,
            PlayerColor currentTurn,
            CastlingState castlingState,
            Coordinate? enpassantTarget,
            int fiftyMoveRuleCounter,
            int fullMoves)
        {
            this.Pieces = pieces;
            this.CurrentTurn = currentTurn;
            this.CastlingState = castlingState;
            this.EnpassantTarget = enpassantTarget;
            this.FiftyMoveRuleCounter = fiftyMoveRuleCounter;
            this.FullMoves = fullMoves;
        }

        public string ConvertToFenString()
        {
            string fen = "";

            for (int y = 7; y >= 0; y--)
            {
                int spaces = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (Pieces.TryGetValue(new Coordinate(x, y), out Piece? piece))
                    {
                        char pieceChar = piece.GetType().Name.First();
                        if (piece is Knight)
                            pieceChar = 'n';
                        pieceChar = piece.color == PlayerColor.White ?
                            char.ToUpper(pieceChar) : char.ToLower(pieceChar);

                        if (spaces > 0)
                        {
                            fen += spaces;
                            spaces = 0;
                        }
                        fen += pieceChar;
                    }
                    else
                    {
                        spaces++;
                    }
                }
                if (spaces > 0)
                    fen += spaces;
                if (y > 0)
                    fen += "/";
            }

            fen += " ";
            fen += CurrentTurn == PlayerColor.White ? "w" : "b";
            fen += " ";

            for (int i = 0; i < 4; i++)
            {
                if (!CastlingState.allowedKingCastlingMoves.TryGetValue(
                    (CastlingState.CastlingMove)i,
                    out CastlingState.CastlingMove castlingRight))
                    continue;

                switch (castlingRight)
                {
                    case CastlingState.CastlingMove.WhiteQueenSide:
                        fen += "Q";
                        break;
                    case CastlingState.CastlingMove.WhiteKingSide:
                        fen += "K";
                        break;
                    case CastlingState.CastlingMove.BlackQueenSide:
                        fen += "q";
                        break;
                    case CastlingState.CastlingMove.BlackKingSide:
                        fen += "k";
                        break;
                    default:
                        break;
                }
            }

            fen += " ";

            string enPassantTargetFen = "-";
            if (EnpassantTarget.HasValue)
                enPassantTargetFen = EnpassantTarget.Value.ToAlphabeticCoordinate();
            fen += enPassantTargetFen;

            fen += " ";
            fen += FiftyMoveRuleCounter;
            fen += " ";
            fen += FullMoves;

            return fen;
        }
    }
}
