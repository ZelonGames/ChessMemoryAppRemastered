using ChessMemoryAppRemastered.Model;
using JChessLib;
using JChessLib.FEN;
using JChessLib.Courses;
using ChessMemoryAppRemastered.Model.Mnemomics;
using ChessMemoryAppRemastered.Model.UI_Components;
using ChessMemoryAppRemastered.Model.UI_Integration;

namespace ChessMemoryAppRemastered;

[QueryProperty(nameof(JChessLib.Courses.Course), "course")]
[QueryProperty(nameof(JChessLib.Courses.Chapter), "chapter")]
[QueryProperty(nameof(JChessLib.Courses.Variation), "variation")]
public partial class MemoryPage : ContentPage
{
    private ChessBoardState chessBoard;
    private UIChessBoard? uIChessBoard;
    private UIPieceIntegration? pieceIntegration;
    private UIPieceMover? pieceMover;
    private UISquareSelectionTracker? squareSelectionTracker;
    public Course? Course { get; set; }
    public Chapter? Chapter { get; set; }
    public Variation? Variation { get; set; }
    public VariationNavigator? variationNavigator;
    private MnemonicsWordGenerator mnemonicsWordGenerator = new();

    public MemoryPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        //Variation = Course!.Chapters.Values.SelectMany(x => x.Variations.Where(x => x.Value.Name == "Najdorf 6.Rg1 e5")).First().Value;
        variationNavigator = new VariationNavigator(Course!, Variation!);
        lblTitle.Text = Variation!.Name;
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
        chessBoard = variationNavigator!.GetStartState();
        uIChessBoard = new UIChessBoard(absoluteLayoutChessBoard, chessBoard);
        uIChessBoard.ReloadPieces(chessBoard);
    }

    private async void BtnNextMove_Clicked(object sender, EventArgs e)
    {
        ChessBoardState oldChessboard = chessBoard;
        chessBoard = variationNavigator!.GetNextState(chessBoard);
        if (oldChessboard == chessBoard)
            return;

        await mnemonicsWordGenerator.AddWordFromMove(variationNavigator.CurrentMove);
        UpdateMnemonicsText();
        uIChessBoard!.ReloadPieces(chessBoard);
    }

    private void BtnPreviousMove_Clicked(object sender, EventArgs e)
    {
        chessBoard = variationNavigator!.GetPreviousState(chessBoard);
        uIChessBoard!.ReloadPieces(chessBoard);
        mnemonicsWordGenerator.TryRemoveLastWord();
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
        chessBoard = variationNavigator!.GetStartState();
        uIChessBoard!.ReloadPieces(chessBoard);
    }

    private void BtnToggleText_Clicked(object sender, EventArgs e)
    {
        lblWordMove.Opacity = lblWordMove.Opacity == 1 ? 0 : 1;
        UpdateMnemonicsText();
    }

    private void UpdateMnemonicsText()
    {
        lblWordMove.Text = mnemonicsWordGenerator.LastWord;
    }
}