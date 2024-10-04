using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Game
{
    public readonly struct LegalMove
    {
        public readonly ChessBoardState chessBoardState;
        private readonly Coordinate fromCoordinate;
        public readonly Coordinate toCoordinate;
        public readonly Pieces.Move.MoveType moveType;

        public LegalMove(ChessBoardState chessBoardState, Coordinate fromCoordinate, Coordinate toCoordinate)
        {
            this.chessBoardState = chessBoardState;
            this.fromCoordinate = fromCoordinate;
            this.toCoordinate = toCoordinate;
            Dictionary<Coordinate, Pieces.Move> legalMoves = chessBoardState.Pieces[fromCoordinate].GetLegalMoves(chessBoardState);

            if (!chessBoardState.Pieces.ContainsKey(fromCoordinate))
                throw new PieceNotFoundException();
            else
            {
                if (!legalMoves.ContainsKey(toCoordinate))
                    throw new IllegalMoveException();
                else
                    moveType = legalMoves[toCoordinate].moveType;
            }
        }

        public readonly Piece GetPieceToMove()
        {
            return chessBoardState.Pieces[fromCoordinate];
        }
    }

    public abstract class MoveException(string message) : Exception(message);

    public sealed class PieceNotFoundException : MoveException
    {
        public PieceNotFoundException() : base ("Piece does not exist") { }
    }

    public sealed class IllegalMoveException : MoveException
    {
        public IllegalMoveException() : base("This piece can't move here") { }
    }
}
