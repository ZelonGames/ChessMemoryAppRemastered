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
                new(knight.coordinate.X + 2, knight.coordinate.Y + 1),
                new(knight.coordinate.X + 2, knight.coordinate.Y - 1),
                new(knight.coordinate.X - 2, knight.coordinate.Y + 1),
                new(knight.coordinate.X - 2, knight.coordinate.Y - 1),
                new(knight.coordinate.X + 1, knight.coordinate.Y + 2),
                new(knight.coordinate.X - 1, knight.coordinate.Y + 2),
                new(knight.coordinate.X + 1, knight.coordinate.Y - 2),
                new(knight.coordinate.X - 1, knight.coordinate.Y - 2),
            };

            foreach (var coordinate in coordinates)
            {
                bool isWithinBounds = coordinate.X < 8 && coordinate.Y < 8 && coordinate.X >= 0 && coordinate.Y >= 0;
                if (!isWithinBounds)
                    continue;
                if (chessBoardState.PiecesState.Pieces.TryGetValue(coordinate, out Piece? piece))
                {
                    if (piece.color != knight.color)
                        moves.Add(coordinate, new Move(Move.Type.Capture, coordinate));
                }
                else
                    moves.Add(coordinate, new Move(Move.Type.Movement, coordinate));
            }

            return moves;
        }

        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            return GetLegalMoves(chessBoardState, this);
        }
    }
}
