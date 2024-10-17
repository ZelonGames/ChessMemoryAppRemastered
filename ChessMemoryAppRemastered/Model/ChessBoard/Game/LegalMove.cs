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
        public readonly Move.Type moveType;
        public readonly Move.Promotion promotionType;

        public LegalMove(
            ChessBoardState chessBoardState,
            Coordinate fromCoordinate,
            Coordinate toCoordinate,
            Move.Promotion promotionType = Move.Promotion.None)
        {
            this.chessBoardState = chessBoardState;
            this.fromCoordinate = fromCoordinate;
            this.toCoordinate = toCoordinate;
            Dictionary<Coordinate, Piece> pieces = chessBoardState.PiecesState.Pieces;

            if (fromCoordinate.Equals(toCoordinate))
                throw new SameSquareException();
            else if (!pieces.ContainsKey(fromCoordinate))
                throw new PieceNotFoundException();
            else
            {
                Piece movingPiece = pieces[fromCoordinate];
                if (chessBoardState.CurrentTurn != movingPiece.color)
                    throw new WrongPieceColorException();

                Dictionary<Coordinate, Move> legalMoves = movingPiece.GetLegalMoves(chessBoardState);
                if (!legalMoves.ContainsKey(toCoordinate))
                    throw new IllegalMoveException();
                else
                {
                    moveType = legalMoves[toCoordinate].type;
                    this.promotionType = promotionType;
                }
            }
        }

        public readonly Piece GetPieceToMove()
        {
            return chessBoardState.PiecesState.Pieces[fromCoordinate];
        }
    }

    public abstract class MoveException(string message) : Exception(message);

    public sealed class PieceNotFoundException : MoveException
    {
        public PieceNotFoundException() : base("Piece does not exist") { }
    }

    public sealed class IllegalMoveException : MoveException
    {
        public IllegalMoveException() : base("This piece can't move here") { }
    }

    public sealed class SameSquareException : MoveException
    {
        public SameSquareException() : base("You are trying to move to the same square!") { }
    }

    public sealed class WrongPieceColorException : MoveException
    {
        public WrongPieceColorException() : base("You are moving the wrong color!") { }
    }

    public sealed class NullCoordinateException : MoveException
    {
        public NullCoordinateException() : base("Coordinates can't be null.") { }
    }
}
