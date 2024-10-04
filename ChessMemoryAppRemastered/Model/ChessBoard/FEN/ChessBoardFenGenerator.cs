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
            ImmutableDictionary<Coordinate, Piece> pieces = GeneratePiecesFromPieceFen(fenPieces);
            ChessBoardState.PlayerColor playerColor = GetPlayerColorFromFenComponents(fenComponents);
            var castlingStateFen = new CastlingStateFen(fen);
            CastlingState castlingState = castlingStateFen.castlingState;
            Coordinate? enPassantTarget = GetEnPassantTargetFromFenComponents(fenComponents);
            int fiftMoveRuleCounter = Convert.ToInt32(fenComponents[4]);
            int fullMoves = Convert.ToInt32(fenComponents[5]);

            return new ChessBoardState(pieces, playerColor, castlingState, enPassantTarget, fiftMoveRuleCounter, fullMoves);
        }

        private static Coordinate? GetEnPassantTargetFromFenComponents(string[] fenComponents)
        {
            string enPassantTarget = fenComponents[3];
            if (enPassantTarget.Length == 1 && enPassantTarget[0] == '-')
                return null;

            string letterCoords = "abcdefgh";

            // 0,0 = a1
            int y = letterCoords.IndexOf(enPassantTarget[0]);
            int x = (int)char.GetNumericValue(enPassantTarget[1]) - 1;

            return new Coordinate(x, y);
        }

        private static ChessBoardState.PlayerColor GetPlayerColorFromFenComponents(string[] fenComponents)
        {
            char fenColor = fenComponents[1][0];
            return fenColor == 'w' ? ChessBoardState.PlayerColor.White : ChessBoardState.PlayerColor.Black;
        }

        private static ImmutableDictionary<Coordinate, Piece> GeneratePiecesFromPieceFen(string[] pieceFen)
        {
            var pieces = new Dictionary<Coordinate, Piece>();
            // Start from top left corner and go down to bottom right
            var pieceCoordinate = new Coordinate(0, 7);
            
            foreach (var row in pieceFen)
            {
                pieceCoordinate.x = 0;
                foreach (char column in row)
                {
                    if (char.IsLetter(column))
                    {
                        var copiedCoordinate = new Coordinate(pieceCoordinate.x, pieceCoordinate.y);
                        pieces.Add(copiedCoordinate, InstantiatePieceFromFenChar(column, copiedCoordinate));
                        pieceCoordinate.x++;
                    }
                    else if (char.IsDigit(column))
                    {
                        pieceCoordinate.x += (int)char.GetNumericValue(column);
                    }
                }
                pieceCoordinate.y--;
            }

            return pieces.ToImmutableDictionary();
        }

        private static Piece InstantiatePieceFromFenChar(char fenChar, Coordinate coordinate)
        {
            ChessBoardState.PlayerColor color = char.IsUpper(fenChar) ? 
                ChessBoardState.PlayerColor.White : ChessBoardState.PlayerColor.Black;

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
    }
}
