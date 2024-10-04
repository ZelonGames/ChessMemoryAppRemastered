using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.FEN
{
    public static class FenHelper
    {
        public static char GetFenPieceChar(Piece piece)
        {
            char pieceChar = piece switch
            {
                Pawn => 'p',
                Knight => 'n',
                Bishop => 'b',
                Rook => 'r',
                Queen => 'q',
                King => 'k',
                _ => throw new ArgumentException("Unknown piece type")
            };

            if (piece.color == PlayerColor.White)
                pieceChar = char.ToUpper(pieceChar);

            return pieceChar;
        }

        public static CastlingState.CastlingMove GetStateFromFenChar(char c)
        {
            return c switch
            {
                'K' => CastlingState.CastlingMove.WhiteKingSide,
                'Q' => CastlingState.CastlingMove.WhiteQueenSide,
                'k' => CastlingState.CastlingMove.BlackKingSide,
                'q' => CastlingState.CastlingMove.BlackQueenSide,
                _ => throw new NotImplementedException(),
            };
        }

        public static Piece InstantiatePieceFromFenChar(char fenChar, Coordinate coordinate)
        {
            PlayerColor color = char.IsUpper(fenChar) ?
                PlayerColor.White : PlayerColor.Black;

            fenChar = char.ToLower(fenChar);

            return fenChar switch
            {
                'p' => new Pawn() { color = color, coordinate = coordinate },
                'r' => new Rook() { color = color, coordinate = coordinate },
                'n' => new Knight() { color = color, coordinate = coordinate },
                'b' => new Bishop() { color = color, coordinate = coordinate },
                'q' => new Queen() { color = color, coordinate = coordinate },
                'k' => new King() { color = color, coordinate = coordinate },
                _ => throw new NotImplementedException(),
            };
        }

        public static string ConvertToFenString(ChessBoardState chessBoardState)
        {
            string fen = "";

            for (int y = 7; y >= 0; y--)
            {
                int spaces = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (chessBoardState.Pieces.TryGetValue(new Coordinate(x, y), out Piece? piece))
                    {
                        char pieceChar = GetFenPieceChar(piece);
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
                        spaces++;
                }
                if (spaces > 0)
                    fen += spaces;
                if (y > 0)
                    fen += "/";
            }

            fen += " ";
            fen += chessBoardState.CurrentTurn == PlayerColor.White ? "w" : "b";
            fen += " ";

            for (int i = 0; i < 4; i++)
            {
                if (!chessBoardState.CastlingState.allowedKingCastlingMoves.TryGetValue(
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
            if (chessBoardState.EnpassantTarget.HasValue)
                enPassantTargetFen = chessBoardState.EnpassantTarget.Value.ToAlphabeticCoordinate();
            fen += enPassantTargetFen;

            fen += " ";
            fen += chessBoardState.FiftyMoveRuleCounter;
            fen += " ";
            fen += chessBoardState.FullMoves;

            return fen;
        }
    }
}
