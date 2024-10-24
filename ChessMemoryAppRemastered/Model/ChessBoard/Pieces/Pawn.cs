using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Pieces
{
    public record Pawn : Piece
    {
        public override Dictionary<Coordinate, Move> GetLegalMoves(ChessBoardState chessBoardState)
        {
            var moves = new Dictionary<Coordinate, Move>();
            bool isPieceOnStartingSquare = IsPieceOnStartingSquare();
            Move moveToAdd;

            moveToAdd = new Move(Move.Type.Movement, new Coordinate(coordinate.X, GetForwardYCoordinate(1)));

            if (!chessBoardState.PiecesState.Pieces.ContainsKey(moveToAdd.coordinate))
            {
                moves.Add(moveToAdd.coordinate, moveToAdd);

                if (isPieceOnStartingSquare)
                {
                    moveToAdd = new Move(Move.Type.DoublePawnMove, new Coordinate(coordinate.X, GetForwardYCoordinate(2)));
                    if (!chessBoardState.PiecesState.Pieces.ContainsKey(moveToAdd.coordinate))
                        moves.Add(moveToAdd.coordinate, moveToAdd);
                }
            }

            moveToAdd = new Move(Move.Type.Capture, new Coordinate(coordinate.X + 1, GetForwardYCoordinate(1)));
            if (chessBoardState.PiecesState.Pieces.ContainsKey(moveToAdd.coordinate) &&
                chessBoardState.PiecesState.Pieces[moveToAdd.coordinate].color != color)
                moves.Add(moveToAdd.coordinate, moveToAdd);

            moveToAdd = new Move(Move.Type.Capture, new Coordinate(coordinate.X - 1, GetForwardYCoordinate(1)));
            if (chessBoardState.PiecesState.Pieces.ContainsKey(moveToAdd.coordinate) &&
                chessBoardState.PiecesState.Pieces[moveToAdd.coordinate].color != color)
                moves.Add(moveToAdd.coordinate, moveToAdd);

            if (chessBoardState.EnpassantTarget is not null)
            {
                bool isPawnNextToTarget =
                    GetForwardYCoordinate(1) == chessBoardState.EnpassantTarget.Value.Y &&
                    (coordinate.X == chessBoardState.EnpassantTarget.Value.X + 1 ||
                    coordinate.X == chessBoardState.EnpassantTarget.Value.X - 1);
                if (isPawnNextToTarget)
                    moves.Add(chessBoardState.EnpassantTarget.Value, new Move(Move.Type.EnPassant, chessBoardState.EnpassantTarget.Value));
            }

            return moves;
        }

        private bool IsPieceOnStartingSquare()
        {
            return color == PlayerColor.White ? coordinate.Y == 1 : coordinate.Y == 6;
        }

        private int GetForwardYCoordinate(int steps)
        {
            return coordinate.Y + (color == PlayerColor.White ? steps : -steps);
        }
    }
}
