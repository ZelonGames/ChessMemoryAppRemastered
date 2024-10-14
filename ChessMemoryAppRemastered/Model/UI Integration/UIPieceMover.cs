using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.UI_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Integration
{
    public class UIPieceMover : IntegrationBase
    {
        public event MadeLegalMoveDelegate? MadeLegalMove;
        public delegate void MadeLegalMoveDelegate(ChessBoardState nextChessBoardState);

        public UIPieceMover(UIChessBoard uIChessBoard)
            : base(uIChessBoard)
        {
            highlightType = UISquare.HighlightType.Red;
        }

        public void TryMovePiece(Coordinate firstSquare, Coordinate secondSquare)
        {
            try
            {
                var legalMove = new LegalMove(uIChessBoard.chessBoardState, firstSquare, secondSquare);
                ResetHighlightedSquares();
                ChessBoardState nextChessBoardState = MoveHelper.GetNextStateFromMove(legalMove);
                MadeLegalMove?.Invoke(nextChessBoardState);
            }
            catch (MoveException)
            {
                ResetHighlightedSquares();
            }
        }

        private void ResetHighlightedSquares()
        {
            var highlightedSquares = uIChessBoard.squares
                .Where(x => x.Value._HighlightType == highlightType);
            foreach (var square in highlightedSquares)
            {
                square.Value.SetHighlight(UISquare.HighlightType.None);
            }
        }

        public override void Dispose()
        {

        }
    }
}
