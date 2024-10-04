using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Rook : Piece
    {
        public static Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState, Piece rook)
        {
            var moves = new Dictionary<Coordinate, Move>();

            for (int y = rook.coordinate.y + 1; y < 8; y++)
            {
                var nextCoordinate = new Coordinate(rook.coordinate.x, y);
                if (chessBoardState.Pieces.TryGetValue(nextCoordinate, out Piece? piece))
                {
                    if (piece.color != rook.color)
                        moves.Add(nextCoordinate, new Move(Move.MoveType.Capture, nextCoordinate));
                    break;
                }
                else
                    moves.Add(nextCoordinate, new Move(Move.MoveType.Movement, nextCoordinate));
            }

            for (int y = rook.coordinate.y - 1; y > 0; y--)
            {
                var nextCoordinate = new Coordinate(rook.coordinate.x, y);
                if (chessBoardState.Pieces.TryGetValue(nextCoordinate, out Piece? piece))
                {
                    if (piece.color != rook.color)
                        moves.Add(nextCoordinate, new Move(Move.MoveType.Capture, nextCoordinate));
                    break;
                }
                else
                    moves.Add(nextCoordinate, new Move(Move.MoveType.Movement, nextCoordinate));
            }

            for (int x = rook.coordinate.x + 1; x < 8; x++)
            {
                var nextCoordinate = new Coordinate(x, rook.coordinate.y);
                if (chessBoardState.Pieces.ContainsKey(nextCoordinate))
                {
                    if (chessBoardState.Pieces[nextCoordinate].color != rook.color)
                        moves.Add(nextCoordinate, new Move(Move.MoveType.Capture, nextCoordinate));
                    break;
                }
                else
                    moves.Add(nextCoordinate, new Move(Move.MoveType.Movement, nextCoordinate));
            }

            for (int x = rook.coordinate.x - 1; x > 0; x--)
            {
                var nextCoordinate = new Coordinate(x, rook.coordinate.y);
                if (chessBoardState.Pieces.ContainsKey(nextCoordinate))
                {
                    if (chessBoardState.Pieces[nextCoordinate].color != rook.color)
                        moves.Add(nextCoordinate, new Move(Move.MoveType.Capture, nextCoordinate));
                    break;
                }
                else
                    moves.Add(nextCoordinate, new Move(Move.MoveType.Movement, nextCoordinate));
            }

            return moves;
        }
        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
