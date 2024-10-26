using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Game;

public readonly record struct LegalMove
{
    public ChessBoardState ChessBoardStateBeforeMoveMade { get; init; }
    private Coordinate FromCoordinate { get; init; }
    public Coordinate ToCoordinate { get; init; }
    public Move.Type MoveType { get; init; }
    public Move.Promotion PromotionType { get; init; }

    public LegalMove(
        ChessBoardState chessBoardStateBeforeMoveMade,
        Coordinate fromCoordinate,
        Coordinate toCoordinate,
        Move.Promotion promotionType = Move.Promotion.None)
    {
        ChessBoardStateBeforeMoveMade = chessBoardStateBeforeMoveMade;
        FromCoordinate = fromCoordinate;
        ToCoordinate = toCoordinate;
        Dictionary<Coordinate, Piece> pieces = chessBoardStateBeforeMoveMade.PiecesState.Pieces;

        if (fromCoordinate.Equals(toCoordinate))
            throw new SameSquareException();
        else if (!pieces.ContainsKey(fromCoordinate))
            throw new PieceNotFoundException();
        else
        {
            Piece movingPiece = pieces[fromCoordinate];
            if (chessBoardStateBeforeMoveMade.CurrentTurn != movingPiece.color)
                throw new WrongPieceColorException();

            Dictionary<Coordinate, Move> legalMoves = movingPiece.GetLegalMoves(chessBoardStateBeforeMoveMade);
            if (!legalMoves.ContainsKey(toCoordinate))
                throw new IllegalMoveException();
            else
            {
                MoveType = legalMoves[toCoordinate].type;
                PromotionType = promotionType;
            }
        }
    }

    public readonly Piece GetPieceToMove()
    {
        return ChessBoardStateBeforeMoveMade.PiecesState.Pieces[FromCoordinate];
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
