﻿using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.UI_Integration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessMemoryAppRemastered.Model.UI_Components
{
    public class UIChessBoard
    {
        private readonly List<IntegrationBase> integrations = [];
        private readonly AbsoluteLayout chessBoardLayout;
        public readonly Dictionary<Coordinate, UISquare> squares = [];
        public readonly Dictionary<Coordinate, UIPiece> pieces = [];
        public ChessBoardState chessBoardState { get; private set; }

        public int TotalSize { get; private set; }

        public UIChessBoard(AbsoluteLayout chessBoardLayout, ChessBoardState chessBoardState)
        {
            this.chessBoardState = chessBoardState;
            this.chessBoardLayout = chessBoardLayout;
            GenerateSquares();
            chessBoardLayout.SizeChanged += Layout_SizeChanged;
            Layout_SizeChanged(chessBoardLayout, null);
        }

        public void AddIntegration(IntegrationBase integration)
        {
            integrations.Add(integration);
        }

        private void Layout_SizeChanged(object? sender, EventArgs? e)
        {
            var contentView = sender as AbsoluteLayout;

            double minSize = Math.Min(contentView!.Width, contentView.Height);
            double squareSize = (int)(minSize / 8);

            foreach (var square in squares)
            {
                square.Value.contentView.WidthRequest = squareSize;
                square.Value.contentView.HeightRequest = squareSize;
                square.Value.contentView.TranslationX = square.Key.X * squareSize;
                square.Value.contentView.TranslationY = squareSize * 7 - squareSize * square.Key.Y;
            }

            TotalSize = (int)squareSize * 8;
        }

        private void GenerateSquares()
        {
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var content = new ContentView();
                    Color color = (x + y) % 2 == 1 ? UISquare.white : UISquare.black;
                    var square = new UISquare(content, color);
                    squares.Add(new Coordinate(x, y), square);
                    chessBoardLayout.Add(square.contentView);
                }
            }
        }

        public void GeneratePieces(ChessBoardState chessBoardState)
        {
            this.chessBoardState = chessBoardState;
            pieces.Clear();

            foreach (var square in squares.Values)
            {
                square.contentView.Content = null;
            }

            foreach (var piece in chessBoardState.PiecesState.Pieces.Values)
            {
                var uiPiece = new UIPiece(piece);

                pieces.Add(piece.coordinate, uiPiece);
                squares[piece.coordinate].contentView.Content = uiPiece.image;
            }
        }
    }
}