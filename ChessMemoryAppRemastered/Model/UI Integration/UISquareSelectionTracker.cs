using ChessMemoryAppRemastered.Model.UI_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Integration
{
    internal class UISquareSelectionTracker : IntegrationBase
    {
        public event SquareSelection? SquareSelectionCompleted;
        public delegate void SquareSelection(Coordinate firstSquare, Coordinate secondSquare);
        private readonly PointerGestureRecognizer pointerRecognizer;
        private Coordinate? firstCoordinate = null;

        public UISquareSelectionTracker(UIChessBoard uIChessBoard) : base(uIChessBoard)
        {
            highlightType = UISquare.HighlightType.Red;
            pointerRecognizer = new PointerGestureRecognizer();

            pointerRecognizer.PointerReleased += Square_PointerReleased;
            foreach (var square in uIChessBoard.squares.Values)
                square.contentView.GestureRecognizers.Add(pointerRecognizer);
        }

        public override void Dispose()
        {
        }

        private void Square_PointerReleased(object? sender, PointerEventArgs e)
        {
            if (sender is not ContentView)
                return;

            ContentView clickedSquare = (ContentView)sender;
            var clickedUISquare = uIChessBoard.squares
                .Where(x => x.Value.contentView == clickedSquare).First();
            Coordinate coordinateOfClickedSquare = clickedUISquare.Key;
            bool userClickedOnPiece = uIChessBoard.pieces.ContainsKey(coordinateOfClickedSquare);

            if (!firstCoordinate.HasValue)
            {
                if (!userClickedOnPiece)
                    return;
                firstCoordinate = coordinateOfClickedSquare;
                clickedUISquare.Value.SetHighlight(highlightType);
            }
            else if (firstCoordinate.HasValue && coordinateOfClickedSquare != firstCoordinate.Value)
            {
                SquareSelectionCompleted?.Invoke(firstCoordinate.Value, coordinateOfClickedSquare);
                firstCoordinate = null;
            }
        }
    }
}
