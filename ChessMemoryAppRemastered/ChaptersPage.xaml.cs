using ChessMemoryAppRemastered.Model.Courses;

namespace ChessMemoryAppRemastered;

public partial class ChaptersPage : ContentPage
{
    private Course course;
    private Chapter chapter;

    public ChaptersPage()
    {
        InitializeComponent();
        Loaded += VariationsPage_Loaded;
    }

    private void VariationsPage_Loaded(object? sender, EventArgs e)
    {
        LoadChapters();
    }

    private async void LoadChapters()
    {
        course = await Course.CreateInstanceFromJson("The Grand Ruy Lopez");
        Title = course.Name;

        foreach (var chapter in course.Chapters)
        {
            var button = new Button()
            {
                Text = chapter.Value.Name,
            };
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
}