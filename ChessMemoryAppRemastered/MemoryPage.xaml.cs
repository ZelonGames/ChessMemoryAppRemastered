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
    private List<ChessBoardState> history = [];
    private ChessBoardState chessBoard;
    private UIChessBoard? uIChessBoard;
    private UIPieceIntegration pieceIntegration;
    private UIPieceMover pieceMover;
    private UISquareSelectionTracker squareSelectionTracker;
    public Course? Course {  get; set; }
    public Chapter? Chapter { get; set; }
    public Variation? Variation { get; set; }
    private int currentVariationMove = 0;
    private MnemonicsWordGenerator mnemonicsWordGenerator = new();

    public MemoryPage()
    {
        InitializeComponent();
        //var a = ChessBoardFenGenerator.Generate("1r1q4/bbp2kpp/p1np1n2/Pp1Pp3/4P3/2P1B2P/1P3PP1/RN1QR1K1 b  - 0 17");
        //var m = MoveNotationHelper.TryGetLegalMoveFromNotation(a, "Bxe3");

        SizeChanged += MainPage_SizeChanged;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        Variation = Course!.Chapters.Values.SelectMany(x => x.Variations.Where(x => x.Value.Name == "6...d6 7.O-O O-O 8.h3 Ba7 9.Re1 Nh5 #7")).First().Value;
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
        chessBoard = ChessBoardFenGenerator.Generate("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        history.Add(chessBoard);
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

    private async void btnNextMove_Clicked(object sender, EventArgs e)
    {
        if (currentVariationMove >= Variation!.Moves.Count)
            return;

        var variationMove = Variation!.Moves[currentVariationMove];
        LegalMove move = MoveNotationHelper.TryGetLegalMoveFromNotation(chessBoard, variationMove.MoveNotation);
        await mnemonicsWordGenerator.AddWordFromMove(move);
        UpdateMnemonicsText();
        ChessBoardState nextState = MoveHelper.GetNextStateFromMove(move);
        chessBoard = nextState;
        uIChessBoard!.ReloadPieces(nextState);
        currentVariationMove++;

        history.Add(nextState);
    }

    private void btnPreviousMove_Clicked(object sender, EventArgs e)
    {
        if (currentVariationMove <= 0)
            return;

        history.Remove(history.Last());
        chessBoard = history.Last();
        uIChessBoard!.ReloadPieces(chessBoard);
        mnemonicsWordGenerator.RemoveLastWord();
        UpdateMnemonicsText();
        currentVariationMove--;
    }

    private void btnCopy_Clicked(object sender, EventArgs e)
    {
        //Clipboard.SetTextAsync(lblMnemonics.Text);
    }

    private void btnReset_Clicked(object? sender, EventArgs? e)
    {
        currentVariationMove = 0;
        mnemonicsWordGenerator = new();
        history.Clear();
        UpdateMnemonicsText();
        LoadBoard();
    }

    private void btnToggleText_Clicked(object sender, EventArgs e)
    {
        lblWordMove.IsVisible = !lblWordMove.IsVisible;
        UpdateMnemonicsText();
    }

    private void UpdateMnemonicsText()
    {
        lblWordMove.Text = mnemonicsWordGenerator.Words.Count > 0 ? mnemonicsWordGenerator.Words.Last() : "";
    }
}