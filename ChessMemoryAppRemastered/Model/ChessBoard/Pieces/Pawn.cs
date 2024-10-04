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

            moveToAdd = new Move(Move.MoveType.Movement, new Coordinate(coordinate.x, GetForwardYCoordinate(1)));
            if (!chessBoardState.Pieces.ContainsKey(moveToAdd.coordinate))
                moves.Add(moveToAdd.coordinate, moveToAdd);

            if (isPieceOnStartingSquare)
            {
                moveToAdd = new Move(Move.MoveType.DoublePawnMove, new Coordinate(coordinate.x, GetForwardYCoordinate(2)));
                if (!chessBoardState.Pieces.ContainsKey(moveToAdd.coordinate))
                    moves.Add(moveToAdd.coordinate, moveToAdd);
            }

            moveToAdd = new Move(Move.MoveType.Capture, new Coordinate(coordinate.x + 1, GetForwardYCoordinate(1)));
            if (chessBoardState.Pieces.ContainsKey(moveToAdd.coordinate) &&
                chessBoardState.Pieces[moveToAdd.coordinate].color != color)
                moves.Add(moveToAdd.coordinate, moveToAdd);

            moveToAdd = new Move(Move.MoveType.Capture, new Coordinate(coordinate.x - 1, GetForwardYCoordinate(1)));
            if (chessBoardState.Pieces.ContainsKey(moveToAdd.coordinate) &&
                chessBoardState.Pieces[moveToAdd.coordinate].color != color)
                moves.Add(moveToAdd.coordinate, moveToAdd);

            if (chessBoardState.EnpassantTarget != null)
                moves.Add(moveToAdd.coordinate, new Move(Move.MoveType.EnPassant, chessBoardState.EnpassantTarget.Value));

            return moves;
        }

        private bool IsPieceOnStartingSquare()
        {
            return color == ChessBoardState.PlayerColor.White ? coordinate.y == 1 : coordinate.y == 6;
        }

        private int GetForwardYCoordinate(int steps)
        {
            return coordinate.y + (color == ChessBoardState.PlayerColor.White ? steps : -steps);
        }
    }
}
