using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record King : Piece
    {
        public static Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState, Piece king)
        {
            var moves = new Dictionary<Coordinate, Move>();
            var coordinates = new List<Coordinate>()
            {
                new(king.coordinate.X + 1, king.coordinate.Y),
                new(king.coordinate.X - 1, king.coordinate.Y),
                new(king.coordinate.X, king.coordinate.Y + 1),
                new(king.coordinate.X, king.coordinate.Y - 1),
                new(king.coordinate.X + 1, king.coordinate.Y + 1),
                new(king.coordinate.X + 1, king.coordinate.Y - 1),
                new(king.coordinate.X - 1, king.coordinate.Y + 1),
                new(king.coordinate.X - 1, king.coordinate.Y - 1),
            };

            foreach (var coordinate in coordinates)
            {
                bool isWithinBounds = coordinate.X >= 0 && coordinate.Y >= 0 && coordinate.X < 8 && coordinate.Y < 8;
                if (!isWithinBounds)
                    continue;

                if (chessBoardState.PiecesState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != king.color)
                        moves.Add(coordinate, new Move(Move.Type.Capture, coordinate));
                }
                else
                {
                    PlayerColor enemyColor = king.GetEnemyColor();
                    var territory = new TerritoryState(chessBoardState, enemyColor);
                    if (!territory.controlledSquares.ContainsKey(coordinate))
                        moves.Add(coordinate, new Move(Move.Type.Movement, coordinate));
                }
            }

            var allowedCastlingMoves = chessBoardState.CastlingState.AllowedKingCastlingMoves;
            Coordinate castlingCoordinate;

            if (allowedCastlingMoves.Contains(CastlingMove.WhiteKingSide) &&
                king.color == PlayerColor.White)
            {
                castlingCoordinate = new Coordinate(6, 0);
                if (CastlingEvaluator.CanCastleKingSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.Type.WhiteKingSideCastle, castlingCoordinate));
            }
            if (allowedCastlingMoves.Contains(CastlingMove.WhiteQueenSide) &&
                king.color == PlayerColor.White)
            {
                castlingCoordinate = new Coordinate(2, 0);
                if (CastlingEvaluator.CanCastleQueenSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.Type.WhiteQueenSideCastle, castlingCoordinate));
            }
            if (allowedCastlingMoves.Contains(CastlingMove.BlackKingSide) &&
                king.color == PlayerColor.Black)
            {
                castlingCoordinate = new Coordinate(6, 7);
                if (CastlingEvaluator.CanCastleKingSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.Type.BlackKingSideCastle, castlingCoordinate));
            }
            if (allowedCastlingMoves.Contains(CastlingMove.BlackQueenSide) &&
                king.color == PlayerColor.Black)
            {
                castlingCoordinate = new Coordinate(2, 7);
                if (CastlingEvaluator.CanCastleQueenSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.Type.BlackQueenSideCastle, castlingCoordinate));
            }

            return moves;
        }

        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
