using ChessMemoryAppRemastered.Model.ChessBoard;
using ChessMemoryAppRemastered.Model.ChessBoard.FEN;
using ChessMemoryAppRemastered.Model.ChessBoard.Game;
using ChessMemoryAppRemastered.Model.Courses;
using ChessMemoryAppRemastered.Model.Mnemomics;
using System.Text.RegularExpressions;

namespace ChessMemoryAppRemastered;

public partial class ChaptersPage : ContentPage
{
    private Course course;
    private Chapter chapter;

    public ChaptersPage()
    {
        InitializeComponent();
        //ChessBoardState chessBoardState = ChessBoardFenGenerator.Generate("r1q1r1k1/p2bbppp/2pp2n1/Pp6/2Q1PP2/2N1B3/1PP1B1PP/3R1RK1 w - b6 0 17");
        //MoveNotationHelper.TryGetLegalMoveFromNotation(chessBoardState, "axb6");
        Loaded += VariationsPage_Loaded;
    }

    private async void VariationsPage_Loaded(object? sender, EventArgs e)
    {
        await LoadChapters();
    }

    private async Task LoadChapters()
    {
        var elementsToKeep = new[] { "btnChessBot" };

        for (int i = variationsLayout.Children.Count - 1; i >= 0; i--)
        {
            var child = variationsLayout.Children[i];
            if (!elementsToKeep.Contains(((Button)child).StyleId))
                variationsLayout.Children.RemoveAt(i);
        }

        course = await Course.CreateInstanceFromJson("The Grand Ruy Lopez");
        Title = course.Name;
        //course.UpdateFens();

        var sordedList = course.Chapters.OrderBy(x => Regex.Replace(x.Value.Name.ToLower(), "[0-9]", ""));
        foreach (var chapter in course.Chapters)
        {
            var button = new Button()
            {
                Text = chapter.Value.Name,
                CornerRadius = 0,
                Padding = new Thickness(0, 20, 0, 20),
                FontSize = 20,
                Margin = 0,
            };
            if (variationsLayout.Children.Count % 2 == 1)
                button.BackgroundColor = Color.FromArgb("#31231b");
            button.Clicked += Chapter_Button_Clicked;
            variationsLayout.Add(button);
        }
    }

    private async void Chapter_Button_Clicked(object? sender, EventArgs e)
    {
        var clickedButton = (Button)sender;
        chapter = course!.GetChapterByName(clickedButton!.Text)!;
        var parameters = new Dictionary<string, object>()
                {
                    { "course", course! },
                    { "chapter", chapter! },
                };
        await Shell.Current.GoToAsync(nameof(VariationsPage), parameters);
    }

    private async void btnChessBot_Clicked(object sender, EventArgs e)
    {
        var parameters = new Dictionary<string, object>()
                {
                    { "course", course! },
                };
        await Shell.Current.GoToAsync(nameof(ChessBotPage), parameters);
    }
}