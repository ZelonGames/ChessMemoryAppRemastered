using ChessMemoryAppRemastered.Model;
using JChessLib;
using JChessLib.FEN;
using ChessMemoryAppRemastered.Model.ChessBot;
using JChessLib.Courses;
using ChessMemoryAppRemastered.Model.Mnemomics;
using ChessMemoryAppRemastered.Model.UI_Components;
using ChessMemoryAppRemastered.Model.UI_Integration;

namespace ChessMemoryAppRemastered;

[QueryProperty(nameof(JChessLib.Courses.Course), "course")]
public partial class ChessBotPage : ContentPage
{
    private ChessBoardState chessBoard;
    private UIChessBoard? uIChessBoard;
    private UIPieceIntegration? pieceIntegration;
    private UIPieceMover? pieceMover;
    private UISquareSelectionTracker? squareSelectionTracker;
    public Course? Course { get; set; }
    private MnemonicsWordGenerator? mnemonicsWordGenerator = new();
    private VariationMoveSelector? variationMoveSelector;

    public ChessBotPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        variationMoveSelector = new VariationMoveSelector(Course!);
        
        var clickRecognizer = new TapGestureRecognizer();
        clickRecognizer.Tapped += ClickRecognizer_Tapped;
        lblTitle.GestureRecognizers.Add(clickRecognizer);
        LoadBoard();
    }

    private void ClickRecognizer_Tapped(object? sender, TappedEventArgs e)
    {
        string fen = FenHelper.ConvertToFenString(chessBoard);
        string url = FenHelper.ConvertFenToChessableUrl(fen, Course!.ChessableCourseID.ToString());
        Launcher.OpenAsync(url);
    }

    private void LoadBoard()
    {
        chessBoard = ChessBoardFenGenerator.Generate(Course!.PreviewFen!);
        uIChessBoard = new UIChessBoard(absoluteLayoutChessBoard, chessBoard);

        UpdateChessBoardPosition(Width, Height);
        uIChessBoard.ReloadPieces(chessBoard);
    }

    private void UpdateChessBoardPosition(double pageWidth, double pageHeight)
    {
        absoluteLayoutChessBoard.TranslationX = 0;
        absoluteLayoutChessBoard.TranslationY = 0;// pageHeight * 0.5f - uIChessBoard.TotalSize * 0.5f;
    }

    private async void BtnNextMove_Clicked(object sender, EventArgs e)
    {
        ChessBoardState oldChessboard = chessBoard;
        chessBoard = variationMoveSelector!.GetNextStateFromRandomMove(chessBoard);
        if (oldChessboard == chessBoard)
            return;

        await mnemonicsWordGenerator!.AddWordFromMove(variationMoveSelector.LastMovePlayed!.Value);
        UpdateMnemonicsText();
        uIChessBoard!.ReloadPieces(chessBoard);
    }

    private void BtnPreviousMove_Clicked(object sender, EventArgs e)
    {
        if (variationMoveSelector!.CurrentVariation == null)
            return;

        var variationNavigator = new VariationNavigator(Course!, variationMoveSelector!.CurrentVariation);
        chessBoard = variationNavigator.GetPreviousState(chessBoard);

        uIChessBoard!.ReloadPieces(chessBoard);
        mnemonicsWordGenerator!.TryRemoveLastWord();
        UpdateMnemonicsText();
    }

    private void BtnCopy_Clicked(object sender, EventArgs e)
    {
        //Clipboard.SetTextAsync(lblMnemonics.Text);
    }

    private void BtnReset_Clicked(object? sender, EventArgs? e)
    {
        mnemonicsWordGenerator = new();
        UpdateMnemonicsText();
        LoadBoard();
    }

    private void BtnToggleText_Clicked(object sender, EventArgs e)
    {
        lblWordMove.Opacity = lblWordMove.Opacity == 1 ? 0 : 1;
        UpdateMnemonicsText();
    }

    private void UpdateMnemonicsText()
    {
        lblWordMove.Text = mnemonicsWordGenerator!.LastWord;
    }
}