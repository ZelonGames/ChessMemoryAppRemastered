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
                new PiecesState() { Pieces = GetUpdatedPiecesAfterMove(legalMove) },
                GetUpdatedPlayerColor(legalMove),
                GetUpdatedCastlingState(legalMove),
                GetUpdatedEnPassantSquare(legalMove),
                GetUpdatedFiftyMoveRule(legalMove),
                GetUpdatedFullMove(legalMove)
            );
        }

        private static Dictionary<Coordinate, Piece> GetUpdatedPiecesAfterMove(LegalMove legalMove)
        {
            Piece piece = legalMove.GetPieceToMove();
            Dictionary<Coordinate, Piece> pieces = legalMove.chessBoardState.PiecesState.Pieces.ToDictionary();
            Piece movedPiece = piece with { coordinate = legalMove.toCoordinate };

            pieces.Remove(piece.coordinate);

            switch (legalMove.moveType)
            {
                case Move.Type.Movement:
                case Move.Type.DoublePawnMove:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    break;
                case Move.Type.Capture:
                    pieces.Remove(legalMove.toCoordinate);
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    break;
                case Move.Type.EnPassant:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    int direction = movedPiece.color == PlayerColor.White ? -1 : 1;
                    var enPassantedSquare = new Coordinate(legalMove.toCoordinate.X, legalMove.toCoordinate.Y + direction);
                    pieces.Remove(enPassantedSquare);
                    break;
                case Move.Type.WhiteKingSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(7, 0), out Piece? whiteKingSideRook);
                    whiteKingSideRook = whiteKingSideRook! with { coordinate = new Coordinate(5, 0) };
                    pieces.Add(whiteKingSideRook.coordinate, whiteKingSideRook!);
                    break;
                case Move.Type.WhiteQueenSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(0, 0), out Piece? whiteQueenSideRook);
                    whiteQueenSideRook = whiteQueenSideRook! with { coordinate = new Coordinate(3, 0) };
                    pieces.Add(whiteQueenSideRook.coordinate, whiteQueenSideRook!);
                    break;
                case Move.Type.BlackKingSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(7, 7), out Piece? blackKingSideRook);
                    blackKingSideRook = blackKingSideRook! with { coordinate = new Coordinate(5, 7) };
                    pieces.Add(blackKingSideRook.coordinate, blackKingSideRook!);
                    break;
                case Move.Type.BlackQueenSideCastle:
                    pieces.Add(legalMove.toCoordinate, movedPiece);
                    pieces.Remove(new Coordinate(0, 7), out Piece? blackQueenSideRook);
                    blackQueenSideRook = blackQueenSideRook! with { coordinate = new Coordinate(3, 7) };
                    pieces.Add(blackQueenSideRook.coordinate, blackQueenSideRook!);
                    break;
                default:
                    break;
            }

            // Remove the piece that you just moved because it will be replaced with the promoted piece
            if (legalMove.promotionType is not Move.Promotion.None)
                pieces.Remove(legalMove.toCoordinate);

            switch (legalMove.promotionType)
            {
                case Move.Promotion.Rook:
                    pieces.Add(legalMove.toCoordinate, new Rook()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.Promotion.Knight:
                    pieces.Add(legalMove.toCoordinate, new Knight()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.Promotion.Bishop:
                    pieces.Add(legalMove.toCoordinate, new Bishop()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.Promotion.Queen:
                    pieces.Add(legalMove.toCoordinate, new Queen()
                    {
                        color = movedPiece.color,
                        coordinate = movedPiece.coordinate
                    });
                    break;
                case Move.Promotion.None:
                    break;
                default:
                    break;
            }

            return pieces;
        }

        private static PlayerColor GetUpdatedPlayerColor(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();
            return movingPiece.GetEnemyColor();
        }

        private static CastlingState GetUpdatedCastlingState(LegalMove legalMove)
        {
            Piece movingPiece = legalMove.GetPieceToMove();
            var allowedKingCastlingMoves = new HashSet<CastlingMove>(legalMove.chessBoardState.CastlingState.AllowedKingCastlingMoves);
            if (movingPiece is Rook)
            {
                if (movingPiece.color == PlayerColor.White &&
                    movingPiece.coordinate.Y == 0)
                {
                    if (movingPiece.coordinate.X == 7)
                        allowedKingCastlingMoves.Remove(CastlingMove.WhiteKingSide);
                    else if (movingPiece.coordinate.X == 0)
                        allowedKingCastlingMoves.Remove(CastlingMove.WhiteQueenSide);
                }
                else if (movingPiece.color == PlayerColor.Black &&
                    movingPiece.coordinate.Y == 7)
                {
                    if (movingPiece.coordinate.X == 7)
                        allowedKingCastlingMoves.Remove(CastlingMove.BlackKingSide);
                    else if (movingPiece.coordinate.X == 0)
                        allowedKingCastlingMoves.Remove(CastlingMove.BlackQueenSide);
                }
            }
            else if (movingPiece is King)
            {
                if (movingPiece.color == PlayerColor.White)
                {
                    allowedKingCastlingMoves.Remove(CastlingMove.WhiteQueenSide);
                    allowedKingCastlingMoves.Remove(CastlingMove.WhiteKingSide);
                }
                else
                {
                    allowedKingCastlingMoves.Remove(CastlingMove.BlackQueenSide);
                    allowedKingCastlingMoves.Remove(CastlingMove.BlackKingSide);
                }
            }

            return new CastlingState() { AllowedKingCastlingMoves = allowedKingCastlingMoves };
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

            var enemyPawnNeighboor = legalMove.chessBoardState.PiecesState.Pieces
                .Where(x =>
                x.Value is Pawn &&
                x.Value.color == movingPiece.GetEnemyColor() &&
                x.Value.coordinate.Y == legalMove.toCoordinate.Y &&
                (x.Value.coordinate.X == legalMove.toCoordinate.X + 1 ||
                x.Value.coordinate.X == legalMove.toCoordinate.X - 1)).FirstOrDefault();

            if (movingPiece is Pawn && legalMove.moveType == Move.Type.DoublePawnMove)
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

            if (legalMove.moveType == Move.Type.Capture || movingPiece is Pawn)
                return 0;

            return legalMove.chessBoardState.FiftyMoveRuleCounter + 1;
        }
    }
}
