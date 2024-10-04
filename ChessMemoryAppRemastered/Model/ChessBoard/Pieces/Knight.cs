using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Knight : Piece
    {
        public static Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState, Piece knight)
        {
            var moves = new Dictionary<Coordinate, Move>();

            var coordinates = new List<Coordinate>(){
                new(knight.coordinate.x + 2, knight.coordinate.y + 1),
                new(knight.coordinate.x + 2, knight.coordinate.y - 1),
                new(knight.coordinate.x - 2, knight.coordinate.y + 1),
                new(knight.coordinate.x - 2, knight.coordinate.y - 1),
                new(knight.coordinate.x + 1, knight.coordinate.y + 2),
                new(knight.coordinate.x - 1, knight.coordinate.y + 2),
                new(knight.coordinate.x + 1, knight.coordinate.y - 2),
                new(knight.coordinate.x - 1, knight.coordinate.y - 2),
            };

            foreach (var coordinate in coordinates)
            {
                bool isWithinBounds = coordinate.x < 8 && coordinate.y < 8 && coordinate.x >= 0 && coordinate.y >= 0;
                if (!isWithinBounds)
                    continue;
                if (chessBoardState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != knight.color)
                        moves.Add(coordinate, new Move(Move.MoveType.Capture, coordinate));
                }
                else
                    moves.Add(coordinate, new Move(Move.MoveType.Movement, coordinate));
            }

            return moves;
        }

        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
