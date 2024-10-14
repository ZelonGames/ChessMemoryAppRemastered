using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.Mnemomics
{
    public class MnemonicsHelper
    {
        public static string ConvertMoveToMnemonics(LegalMove move)
        {
            Piece movingPiece = move.GetPieceToMove();
            ChessBoardState chessBoard = move.chessBoardState;
            var pieces = chessBoard.PiecesState.Pieces
                //.OrderBy(x => x.Value.coordinate.X)
                .Where(x => x.Value.color == movingPiece.color);

            int amountOfCandidatePieces = 0;

            foreach (var piece in pieces)
            {
                bool canPieceMakeTheSameMove = piece.Value.GetLegalMoves(chessBoard).ContainsKey(move.toCoordinate);
                if (canPieceMakeTheSameMove)
                    amountOfCandidatePieces++;
            }

            return "";
        }
    }
}
