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
                new(king.coordinate.x + 1, king.coordinate.y),
                new(king.coordinate.x - 1, king.coordinate.y),
                new(king.coordinate.x, king.coordinate.y + 1),
                new(king.coordinate.x, king.coordinate.y - 1),
                new(king.coordinate.x + 1, king.coordinate.y + 1),
                new(king.coordinate.x + 1, king.coordinate.y - 1),
                new(king.coordinate.x - 1, king.coordinate.y + 1),
                new(king.coordinate.x - 1, king.coordinate.y - 1),
            };

            foreach (var coordinate in coordinates)
            {
                bool isWithinBounds = coordinate.x >= 0 && coordinate.y >= 0 && coordinate.x < 8 && coordinate.y < 8;
                if (!isWithinBounds)
                    continue;

                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != king.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));
                }
                else
                {
                    PlayerColor enemyColor = king.GetEnemyColor();
                    var territory = new TerritoryState(chessBoardState, enemyColor);
                    if (!territory.controlledSquares.ContainsKey(coordinate))
                        moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
                }
            }

            var allowedCastlingMoves = chessBoardState.CastlingState.allowedKingCastlingMoves;
            Coordinate castlingCoordinate;

            if (allowedCastlingMoves.Contains(CastlingState.CastlingMove.WhiteKingSide) &&
                king.color == PlayerColor.White)
            {
                castlingCoordinate = new Coordinate(6, 0);
                if (CastlingEvaluator.CanCastleKingSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.MoveType.WhiteKingSideCastle, castlingCoordinate));
            }
            else if (allowedCastlingMoves.Contains(CastlingState.CastlingMove.WhiteQueenSide) &&
                king.color == PlayerColor.White)
            {
                castlingCoordinate = new Coordinate(2, 0);
                if (CastlingEvaluator.CanCastleQueenSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.MoveType.WhiteQueenSideCastle, castlingCoordinate));
            }
            else if (allowedCastlingMoves.Contains(CastlingState.CastlingMove.BlackKingSide) &&
                king.color == PlayerColor.Black)
            {
                castlingCoordinate = new Coordinate(6, 7);
                if (CastlingEvaluator.CanCastleKingSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.MoveType.BlackKingSideCastle, castlingCoordinate));
            }
            else if (allowedCastlingMoves.Contains(CastlingState.CastlingMove.BlackQueenSide) &&
                king.color == PlayerColor.Black)
            {
                castlingCoordinate = new Coordinate(2, 7);
                if (CastlingEvaluator.CanCastleQueenSide(chessBoardState, king))
                    moves.Add(castlingCoordinate, new Move(Move.MoveType.BlackQueenSideCastle, castlingCoordinate));
            }

            return moves;
        }

        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
