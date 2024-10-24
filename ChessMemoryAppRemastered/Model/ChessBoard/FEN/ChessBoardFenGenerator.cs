using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.FEN
{
    public class ChessBoardFenGenerator
    {
        public static ChessBoardState Generate(string fen)
        {
            string[] fenComponents = fen.Split(' ');
            string[] fenPieces = fenComponents[0].Split('/');
            Dictionary<Coordinate, Piece> pieces = GeneratePiecesFromPieceFen(fenPieces);
            PlayerColor playerColor = GetPlayerColorFromFenComponents(fenComponents);
            var castlingStateFen = new CastlingStateFen(fen);
            CastlingState castlingState = castlingStateFen.castlingState;
            Coordinate? enPassantTarget = GetEnPassantTargetFromFenComponents(fenComponents);
            int fiftMoveRuleCounter = Convert.ToInt32(fenComponents[4]);
            int fullMoves = Convert.ToInt32(fenComponents[5]);

            return new ChessBoardState
            (
                new PiecesState() { Pieces = pieces}, 
                playerColor, 
                castlingState, 
                enPassantTarget, 
                fiftMoveRuleCounter, 
                fullMoves 
            );
        }

        private static Coordinate? GetEnPassantTargetFromFenComponents(string[] fenComponents)
        {
            string enPassantTarget = fenComponents[3];
            if (enPassantTarget.Length == 1 && enPassantTarget[0] == '-')
                return null;

            string letterCoords = "abcdefgh";

            // 0,0 = a1
            int x = letterCoords.IndexOf(enPassantTarget[0]);
            int y = (int)char.GetNumericValue(enPassantTarget[1]) - 1;

            return new Coordinate(x, y);
        }

        private static PlayerColor GetPlayerColorFromFenComponents(string[] fenComponents)
        {
            char fenColor = fenComponents[1][0];
            return fenColor == 'w' ? PlayerColor.White : PlayerColor.Black;
        }

        private static Dictionary<Coordinate, Piece> GeneratePiecesFromPieceFen(string[] pieceFen)
        {
            var pieces = new Dictionary<Coordinate, Piece>();
            // Start from top left corner and go down to bottom right
            var pieceCoordinate = new Coordinate(0, 7);
            
            foreach (var row in pieceFen)
            {
                pieceCoordinate.X = 0;
                foreach (char column in row)
                {
                    if (char.IsLetter(column))
                    {
                        var copiedCoordinate = new Coordinate(pieceCoordinate.X, pieceCoordinate.Y);
                        pieces.Add(copiedCoordinate, FenHelper.InstantiatePieceFromFenChar(column, copiedCoordinate));
                        pieceCoordinate.X++;
                    }
                    else if (char.IsDigit(column))
                    {
                        pieceCoordinate.X += (int)char.GetNumericValue(column);
                    }
                }
                pieceCoordinate.Y--;
            }

            return pieces;
        }
    }
}
