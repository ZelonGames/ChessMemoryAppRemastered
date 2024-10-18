using Microsoft.Extensions.Logging;

namespace ChessMemoryAppRemastered
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            /*
            builder.Services.AddTransient<ChaptersPage>();
            builder.Services.AddTransient<VariationsPage>();
            builder.Services.AddTransient<MemoryPage>();*/

            return builder.Build();
        }
    }
}
