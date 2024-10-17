using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.ChessBoard
{
    internal class MoveNotationHelper
    {
        public static string GetCoordinatesFromMoveNotation(string moveNotation)
        {
            if (moveNotation is "O-O" or "O-O-O")
                return moveNotation;

            var regex = new Regex(@"([a-h][1-8])");
            Match match = regex.Match(moveNotation.ToLower());
            return match.Groups[0].Value;
        }

        public static Move.Promotion GetPromotionFromMoveNotation(string moveNotation)
        {
            var regex = new Regex(@"=([nbrq])");
            Match match = regex.Match(moveNotation.ToLower());
            char? promotionChar = match.Success ? match.Groups[1].Value.First() : null;

            if (!promotionChar.HasValue)
                return Move.Promotion.None;

            return promotionChar.Value switch
            {
                'n' => Move.Promotion.Knight,
                'b' => Move.Promotion.Bishop,
                'r' => Move.Promotion.Rook,
                'q' => Move.Promotion.Queen,
                _ => Move.Promotion.None
            };
        }

        public static char? GetSpecificationFromMoveNotation(string moveNotation)
        {
            if (moveNotation.Contains('='))
                moveNotation = moveNotation[..moveNotation.IndexOf('=')];
            moveNotation = Regex.Replace(moveNotation, "[#+]", "");
            bool isCaptureMove = moveNotation.Contains('x');
            if (isCaptureMove)
            {
                string leftSide = moveNotation.Split('x')[0];
                if (leftSide.Length == 2)
                    return leftSide[1];
                else if (leftSide.Length == 1 && char.IsUpper(leftSide[0]))
                    return null;
            }
            else
            {
                if (char.IsUpper(moveNotation[0]) && moveNotation.Length == 3 || 
                    moveNotation.Length == 2)
                    return null;
                if (char.IsLower(moveNotation[0]) && moveNotation.Length == 3)
                    return moveNotation[0];
            }

            var regex = new Regex(@"[nbrq]([a-h1-8])|([a-h1-8])x");
            Match match = regex.Match(moveNotation.ToLower()[..2]);

            if (match.Success)
            {
                return match.Groups[1].Value.Length == 0 ?
                     match.Groups[2].Value.First() : match.Groups[1].Value.First();
            }

            return null;
        }

        public static char GetPieceTypeCharFromMoveNotation(string moveNotation)
        {
            if (moveNotation is "O-O" or "O-O-O")
                return 'K';
            char firstChar = moveNotation[0];
            return char.IsUpper(firstChar) ? firstChar : 'P';
        }

        public static LegalMove TryGetLegalMoveFromNotation(ChessBoardState chessBoardState, string moveNotation)
        {
            PlayerColor color = chessBoardState.CurrentTurn;
            string toCoordinateString = GetCoordinatesFromMoveNotation(moveNotation);
            Coordinate toCoordinate = Coordinate.FromAlphabeticCoordinate(toCoordinateString, color);
            Type pieceType = GetPieceTypeFromNoveNotation(moveNotation);
            char? pieceSpecification = GetSpecificationFromMoveNotation(moveNotation);
            int? fromY = GetRowFromSpecification(pieceSpecification);
            int? fromX = GetColumnFromSpecification(pieceSpecification);

            var piecesOfColor = chessBoardState.PiecesState.Pieces
                .Where(x => x.Value.color == color && x.Value.GetType() == pieceType);

            Piece? movingPiece = null;
            foreach (var piece in piecesOfColor)
            {
                Dictionary<Coordinate, Move> legalMoves = piece.Value.GetLegalMoves(chessBoardState);
                bool canPieceMoveToSquare = legalMoves.ContainsKey(toCoordinate);
                if (!canPieceMoveToSquare)
                    continue;

                if (!pieceSpecification.HasValue ||
                    char.IsNumber(pieceSpecification.Value) && piece.Value.coordinate.Y == fromY!.Value ||
                    char.IsLetter(pieceSpecification.Value) && piece.Value.coordinate.X == fromX!.Value)
                {
                    movingPiece = piece.Value;
                    break;
                }
            }

            if (movingPiece == null)
                throw new PieceNotFoundException();

            Move.Promotion promotionType = GetPromotionFromMoveNotation(moveNotation);
            return new LegalMove(chessBoardState, movingPiece.coordinate, toCoordinate, promotionType);
        }

        private static int? GetColumnFromSpecification(char? specification)
        {
            return specification.HasValue ? Coordinate.rows.IndexOf(specification.Value) : null;
        }

        private static int? GetRowFromSpecification(char? specification)
        {
            return specification.HasValue ? (int)char.GetNumericValue(specification.Value) - 1 : null;
        }

        private static Type GetPieceTypeFromNoveNotation(string moveNotation)
        {
            char pieceType = GetPieceTypeCharFromMoveNotation(moveNotation);

            return pieceType switch
            {
                'P' => typeof(Pawn),
                'N' => typeof(Knight),
                'B' => typeof(Bishop),
                'R' => typeof(Rook),
                'Q' => typeof(Queen),
                'K' => typeof(King),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
