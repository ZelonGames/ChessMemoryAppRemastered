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

[QueryProperty(nameof(Model.Courses.Course), "course")]
[QueryProperty(nameof(Model.Courses.Chapter), "chapter")]
[QueryProperty(nameof(Model.Courses.Variation), "variation")]
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
        SizeChanged += MainPage_SizeChanged;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        //Variation = Course!.Chapters.Values.SelectMany(x => x.Variations.Where(x => x.Value.Name == "6...d6 7.O-O O-O 8.h3 Ba7 9.Re1 Nh5 #7")).First().Value;
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

        UpdateChessBoardPosition(Width, Height);
        MainPage_SizeChanged(this, null);
        uIChessBoard.ReloadPieces(chessBoard);
    }

    private void MainPage_SizeChanged(object? sender, EventArgs? e)
    {
        if (uIChessBoard == null)
            return;

        var page = sender as ContentPage;
        UpdateChessBoardPosition(page!.Width, page.Content.Height);
    }

    private void UpdateChessBoardPosition(double pageWidth, double pageHeight)
    {
        absoluteLayoutChessBoard.TranslationX = 0;
        absoluteLayoutChessBoard.TranslationY = 0;// pageHeight * 0.5f - uIChessBoard.TotalSize * 0.5f;
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
        mnemonicsWordGenerator.RemoveLastWord();
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
        lblWordMove.IsVisible = !lblWordMove.IsVisible;
        UpdateMnemonicsText();
    }

    private void UpdateMnemonicsText()
    {
        lblWordMove.Text = mnemonicsWordGenerator.LastWord;
    }
}