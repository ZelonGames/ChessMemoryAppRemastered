namespace ChessMemoryAppRemastered
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ChaptersPage), typeof(ChaptersPage));
            Routing.RegisterRoute(nameof(VariationsPage), typeof(VariationsPage));
            Routing.RegisterRoute(nameof(MemoryPage), typeof(MemoryPage));
            Routing.RegisterRoute(nameof(ChessBotPage), typeof(ChessBotPage));
        }
    }
}
