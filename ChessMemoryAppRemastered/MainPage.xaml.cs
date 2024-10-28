using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model;
using ChessMemoryAppRemastered.Model.ChessBoard;
using System.Security.Cryptography.X509Certificates;
using ChessMemoryAppRemastered.Model.UI_Components;
using ChessMemoryAppRemastered.Model.UI_Integration;
using ChessMemoryAppRemastered.Model.ChessBoard.Pieces;
using System.Collections.Immutable;
using System.IO;

namespace ChessMemoryAppRemastered
{
    public partial class MainPage : ContentPage
    {
        private ChessBoardState? previousChessBoardState = null;
        private ChessBoardState chessBoard;
        private UIChessBoard? uIChessBoard;
        private UIPieceIntegration pieceIntegration;
        private UIPieceMover pieceMover;

        private int currentMove = 0;

        private Dictionary<ChessBoardState, MoveTree<ChessBoardState>> addedMoves = [];
        private MoveTree<ChessBoardState>? moveTree = null;
        private MoveTree<ChessBoardState>? firstMove = null;

        public MainPage()
        {
            InitializeComponent();
            btnNextMove.Clicked += BtnNextMove_Clicked;
            btnPreviousMove.Clicked += BtnPreviousMove_Clicked;
            btnAddMove.Clicked += BtnAddMove_Clicked;
            btnFirstMove.Clicked += BtnFirstMove_Clicked;
            Loaded += MainPage_Loaded;
        }

        private void BtnFirstMove_Clicked(object? sender, EventArgs e)
        {
            if (moveTree == null)
                return;

            currentMove = 1;
            ChessBoardState current = moveTree!.GetFromDepth(currentMove);
            MadeLegalMove(current);
        }

        private void BtnAddMove_Clicked(object? sender, EventArgs e)
        {
            var move = new MoveTree<ChessBoardState>(chessBoard);
            addedMoves.TryAdd(chessBoard, move);

            if (moveTree == null)
                moveTree = move;

            if (previousChessBoardState.HasValue)
                moveTree.AddFrom(addedMoves[previousChessBoardState.Value], move);
        }

        private void BtnPreviousMove_Clicked(object? sender, EventArgs e)
        {
            if (moveTree == null)
                return;

            if (currentMove > 0)
                currentMove--;
            ChessBoardState current = moveTree.GetFromDepth(currentMove);
            MadeLegalMove(current);
        }

        private void BtnNextMove_Clicked(object? sender, EventArgs e)
        {
            if (moveTree == null)
                return;

            if (currentMove < moveTree.Count)
                currentMove++;
            ChessBoardState current = moveTree.GetFromDepth(currentMove);
            MadeLegalMove(current);
        }

        private void MainPage_Loaded(object? sender, EventArgs e)
        {
            chessBoard = ChessBoardFenGenerator.Generate("r1bqkbnr/1ppp1ppp/p1n5/1B2p3/4P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 4");
            uIChessBoard = new UIChessBoard(absoluteLayoutChessBoard, chessBoard);

            uIChessBoard.ReloadPieces(chessBoard);
            pieceIntegration = new UIPieceIntegration(uIChessBoard);
            pieceMover = new UIPieceMover(uIChessBoard);
            pieceMover.MadeLegalMove += MadeLegalMove;
            pieceMover.MadeLegalMove += (ChessBoardState nextChessBoardState) => { currentMove++; };
        }

        private void MadeLegalMove(ChessBoardState nextChessBoardState)
        {
            previousChessBoardState = chessBoard;
            chessBoard = nextChessBoardState;
            uIChessBoard!.ReloadPieces(nextChessBoardState);
            pieceIntegration.Dispose();
            pieceIntegration = new UIPieceIntegration(uIChessBoard);
        }
    }
}

