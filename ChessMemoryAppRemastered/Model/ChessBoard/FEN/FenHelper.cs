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
        public const string STATRING_FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

        public static string ConvertFenToChessableUrl(string fen, string courseID)
        {
            if (fen == null)
                return "https://www.chessable.com";

            string urlFen = fen.Replace('/', ';').Replace(" ", "%20");
            return "https://www.chessable.com/course/" + courseID + "/fen/" + urlFen;
        }

        public static string ConvertFenToLichessUrl(string fen)
        {
            fen = fen.Split(' ')[0];
            return "https://lichess.org/analysis/" + fen;
        }

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

        public static CastlingMove GetStateFromFenChar(char c)
        {
            return c switch
            {
                'K' => CastlingMove.WhiteKingSide,
                'Q' => CastlingMove.WhiteQueenSide,
                'k' => CastlingMove.BlackKingSide,
                'q' => CastlingMove.BlackQueenSide,
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

            #region Position
            for (int y = 7; y >= 0; y--)
            {
                int spaces = 0;
                for (int x = 0; x < 8; x++)
                {
                    if (chessBoardState.PiecesState.Pieces.TryGetValue(new Coordinate(x, y), out Piece? piece))
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
            #endregion

            #region Current Turn
            fen += " ";
            fen += chessBoardState.CurrentTurn == PlayerColor.White ? "w" : "b";
            fen += " ";
            #endregion

            #region Castling
            for (int i = 0; i < 4; i++)
            {
                if (!chessBoardState.CastlingState.AllowedKingCastlingMoves.TryGetValue(
                    (CastlingMove)i,
                    out CastlingMove castlingRight))
                    continue;

                switch (castlingRight)
                {
                    case CastlingMove.WhiteQueenSide:
                        fen += "Q";
                        break;
                    case CastlingMove.WhiteKingSide:
                        fen += "K";
                        break;
                    case CastlingMove.BlackQueenSide:
                        fen += "q";
                        break;
                    case CastlingMove.BlackKingSide:
                        fen += "k";
                        break;
                    default:
                        fen += "-";
                        break;
                }
            }
            bool canCastle = chessBoardState.CastlingState.AllowedKingCastlingMoves.Count > 0;
            fen += canCastle ? " " : "- ";
            #endregion

            #region En Passant
            string enPassantTargetFen = "-";
            if (chessBoardState.EnpassantTarget.HasValue)
                enPassantTargetFen = chessBoardState.EnpassantTarget.Value.ToAlphabeticCoordinate();
            fen += enPassantTargetFen.ToLower();
            #endregion

            #region Move Counting
            fen += " ";
            fen += chessBoardState.FiftyMoveRuleCounter;
            fen += " ";
            fen += chessBoardState.FullMoves;
            #endregion

            return fen;
        }
    }
}
