using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard.Game
{
    public class MoveHelper
    {
        public static ChessBoardState GetNextStateFromMove(LegalMove legalMove)
        {
            return new ChessBoardState
            (
                GetUpdatedPieces(legalMove),
                GetUpdatedPlayerColor(legalMove),
                GetUpdatedCastlingState(legalMove),
                GetUpdatedEnPassantSquare(legalMove),
                GetUpdatedFiftyMoveRule(legalMove),
                GetUpdatedFullMove(legalMove)
            );
        }

        private static ImmutableDictionary<Coordinate, Piece> GetUpdatedPieces(LegalMove legalMove)
        {
            Piece piece = legalMove.GetPieceToMove();
            Dictionary<Coordinate, Piece> pieces = legalMove.chessBoardState.Pieces.ToDictionary();
            Piece movedPiece = piece with { coordinate = legalMove.toCoordinate };

            pieces.Remove(piece.coordinate);

            switch (legalMove.moveType)
            {
                case Move.MoveType.Movement:
                case Move.MoveType.DoublePawnMove:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    break;
                case Move.MoveType.Capture:
                    pieces.Remove(legalMove.toCoordinate);
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    break;
                case Move.MoveType.EnPassant:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    int direction = movedPiece.color == PlayerColor.White ? -1 : 1;
                    var enPassantedSquare = new Coordinate(legalMove.toCoordinate.X, legalMove.toCoordinate.Y + direction);
                    pieces.Remove(enPassantedSquare);
                    break;
                case Move.MoveType.WhiteKingSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(7, 0), out Piece? whiteKingSideRook);
                    pieces.Add(new Coordinate(5, 0), whiteKingSideRook!);
                    break;
                case Move.MoveType.WhiteQueenSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(0, 0), out Piece? whiteQueenSideRook);
                    pieces.Add(new Coordinate(3, 0), whiteQueenSideRook!);
                    break;
                case Move.MoveType.BlackKingSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(7, 7), out Piece? blackKingSideRook);
                    pieces.Add(new Coordinate(5, 7), blackKingSideRook!);
                    break;
                case Move.MoveType.BlackQueenSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(0, 7), out Piece? blackQueenSideRook);
                    pieces.Add(new Coordinate(3, 7), blackQueenSideRook!);
                    break;
                case Move.MoveType.PromotionRook:
                    pieces.Add(legalMove.toCoordinate, new Rook() 
                    { 
                        color = movedPiece.color, 
                        coordinate = movedPiece.coordinate 
                    });
                    break;
                case Move.MoveType.PromotionKnight:
                    pieces.Add(legalMove.toCoordinate, new Knight()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.MoveType.PromotionBishop:
                    pieces.Add(legalMove.toCoordinate, new Bishop()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.MoveType.PromotionQueen:
                    pieces.Add(legalMove.toCoordinate, new Queen()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                default:
                    break;
            }

            return pieces.ToImmutableDictionary();
        }

        private static PlayerColor GetUpdatedPlayerColor(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();
            return movingPiece.GetEnemyColor();
        }

        private static CastlingState GetUpdatedCastlingState(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();
            var allowedKingCastlingMoves = new HashSet<CastlingState.CastlingMove>(legalMove.chessBoardState.CastlingState.allowedKingCastlingMoves);
            if (movingPiece is Rook)
            {
                if (movingPiece.color == PlayerColor.White &&
                    movingPiece.coordinate.Y == 0)
                {
                    if (movingPiece.coordinate.X == 7)
                        allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.WhiteKingSide);
                    else if (movingPiece.coordinate.X == 0)
                        allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.WhiteQueenSide);
                }
                else if (movingPiece.color == PlayerColor.Black &&
                    movingPiece.coordinate.Y == 7)
                {
                    if (movingPiece.coordinate.X == 7)
                        allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.BlackKingSide);
                    else if (movingPiece.coordinate.X == 0)
                        allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.BlackQueenSide);
                }
            }
            else if (movingPiece is King)
            {
                if (movingPiece.color == PlayerColor.White)
                {
                    allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.WhiteQueenSide);
                    allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.WhiteKingSide);
                }
                else
                {
                    allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.BlackQueenSide);
                    allowedKingCastlingMoves.Remove(CastlingState.CastlingMove.BlackKingSide);
                }
            }

            return new CastlingState(allowedKingCastlingMoves.ToImmutableHashSet());
        }

        private static int GetUpdatedFullMove(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();
            if (movingPiece.color == PlayerColor.Black)
                return legalMove.chessBoardState.FullMoves + 1;
            return legalMove.chessBoardState.FullMoves;
        }

        private static Coordinate? GetUpdatedEnPassantSquare(LegalMove legalMove)
        {
            if (legalMove.chessBoardState.EnpassantTarget != null)
                return null;

            Piece movingPiece = legalMove.GetPieceToMove();

            var enemyPawnNeighboor = legalMove.chessBoardState.Pieces
                .Where(x => 
                x.Value is Pawn &&
                x.Value.color == movingPiece.GetEnemyColor() &&
                x.Value.coordinate.Y == legalMove.toCoordinate.Y &&
                (x.Value.coordinate.X == legalMove.toCoordinate.X + 1 ||
                x.Value.coordinate.X == legalMove.toCoordinate.X - 1)).FirstOrDefault();

            if (movingPiece is Pawn && legalMove.moveType == Move.MoveType.DoublePawnMove)
            {
                if (enemyPawnNeighboor.Value != null)
                {
                    int direction = movingPiece.color == PlayerColor.White ? 1 : -1;
                    return new Coordinate(movingPiece.coordinate.X, movingPiece.coordinate.Y + direction);
                }
            }

            return null;
        }

        private static int GetUpdatedFiftyMoveRule(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();

            if (legalMove.moveType == Move.MoveType.Capture || movingPiece is Pawn)
                return 0;

            return legalMove.chessBoardState.FiftyMoveRuleCounter + 1;
        }
    }
}
