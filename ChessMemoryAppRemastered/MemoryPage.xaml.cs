using ChessMemoryAppRemastered.Model;
using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.Courses;
using ChessMemoryAppRemastered.Model.Mnemomics;
using ChessMemoryAppRemastered.Model.UI_Components;
using ChessMemoryAppRemastered.Model.UI_Integration;
using Newtonsoft.Json;

namespace ChessMemoryAppRemastered;

public partial class MemoryPage : ContentPage
{
    private ChessBoardState chessBoard;
    private UIChessBoard? uIChessBoard;
    private UIPieceIntegration pieceIntegration;
    private UIPieceMover pieceMover;
    private UISquareSelectionTracker squareSelectionTracker;

    public MemoryPage()
	{
		InitializeComponent();
        SizeChanged += MainPage_SizeChanged;
        Loaded += MainPage_Loaded;

        var b = ChessBoardFenGenerator.Generate("rnbqkbnr/ppppppPp/8/8/8/8/PPPPPP1P/RNBQKBNR w KQkq - 0 1");
        var move = MoveNotationHelper.TryGetLegalMoveFromNotation(b, "gxh8=R");
        MoveHelper.GetNextStateFromMove(move);

        Test();
    }
    private async void Test()
    {
        var course = await Course.CreateInstanceFromJson("The Grand Ruy Lopez");
        Chapter? quickStarterGuide = course.GetChapterByName("Quickstarter Guide");
    }

    private void MainPage_Loaded(object? sender, EventArgs e)
    {
        chessBoard = ChessBoardFenGenerator.Generate("rnbqkb1r/ppp1ppPp/4n3/3p4/2P5/8/PP1P1PPP/RNBQKBNR w KQkq - 0 1");
        uIChessBoard = new UIChessBoard(absoluteLayoutChessBoard, chessBoard);

        UpdateChessBoardPosition(Width, Height);
        MainPage_SizeChanged(this, null);
        uIChessBoard.GeneratePieces(chessBoard);
        pieceIntegration = new UIPieceIntegration(uIChessBoard);
        pieceMover = new UIPieceMover(uIChessBoard);
        squareSelectionTracker = new UISquareSelectionTracker(uIChessBoard);
        squareSelectionTracker.SquareSelectionCompleted += pieceMover.TryMovePiece;
        pieceMover.MadeLegalMove += MadeLegalMove;
    }

    private void MadeLegalMove(ChessBoardState nextChessBoardState)
    {
        chessBoard = nextChessBoardState;
        uIChessBoard!.GeneratePieces(nextChessBoardState);
        pieceIntegration.Dispose();
        pieceIntegration = new UIPieceIntegration(uIChessBoard);
    }

    private void MainPage_SizeChanged(object? sender, EventArgs? e)
    {
        if (uIChessBoard == null)
            return;

        var page = sender as ContentPage;
        UpdateChessBoardPosition(page!.Content.Width, page.Content.Height);
    }

    private void UpdateChessBoardPosition(double pageWidth, double pageHeight)
    {
        absoluteLayoutChessBoard.TranslationX = pageWidth * 0.5f - uIChessBoard!.TotalSize * 0.5f;
        absoluteLayoutChessBoard.TranslationY = pageHeight * 0.5f - uIChessBoard.TotalSize * 0.5f;
    }
}