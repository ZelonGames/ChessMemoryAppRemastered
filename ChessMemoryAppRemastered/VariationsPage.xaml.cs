using JChessLib.Courses;

namespace ChessMemoryAppRemastered;

[QueryProperty(nameof(JChessLib.Courses.Course), "course")]
[QueryProperty(nameof(JChessLib.Courses.Chapter), "chapter")]
public partial class VariationsPage : ContentPage
{
    public Course? Course { get; set; }
    public Chapter? Chapter { get; set; }
    private Variation? variation;
    public VariationsPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        variationsLayout.Clear();
        base.OnNavigatedTo(args);
        Title = Chapter!.Name;

        variationsLayout.Clear();
        foreach (var variation in Chapter.Variations!)
        {
            var button = new Button()
            {
                Text = variation.Value.Name,
                CornerRadius = 0,
                Padding = new Thickness(0, 20, 0, 20),
                FontSize = 20,
                Margin = 0,
            };
            if (variationsLayout.Children.Count % 2 == 1)
                button.BackgroundColor = Color.FromArgb("#31231b");
            button.Clicked += Variation_Button_Clicked;
            variationsLayout.Add(button);
        }
    }

    private async void Variation_Button_Clicked(object? sender, EventArgs e)
    {
        var clickedButton = (Button)sender!;

        variation = Chapter!.GetVariationByName(clickedButton!.Text);
        var parameters = new Dictionary<string, object>()
                {
                    { "course", Course! },
                    { "chapter", Chapter },
                    { "variation", variation! },
                };
        await Shell.Current.GoToAsync(nameof(MemoryPage), parameters);
    }
}