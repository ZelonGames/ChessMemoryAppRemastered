using ChessMemoryAppRemastered.Model.Courses;

namespace ChessMemoryAppRemastered;

[QueryProperty(nameof(Model.Courses.Course), "course")]
[QueryProperty(nameof(Model.Courses.Chapter), "chapter")]
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
        base.OnNavigatedTo(args);
        Title = Chapter!.Name;

        variationsLayout.Clear();
        foreach (var variation in Chapter.Variations)
        {
            var button = new Button()
            {
                Text = variation.Value.Name,
            };
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