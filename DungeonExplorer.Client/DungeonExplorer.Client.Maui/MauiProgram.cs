using DungeonExplorer.Client.Maui.Dungeon;

using Microsoft.Extensions.Logging;

namespace DungeonExplorer.Client.Maui
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

            builder.Services.AddScoped<HttpClient>(svc => new HttpClient() { BaseAddress = new(@"http://localhost:8080") });
            builder.Services.AddScoped<Dungeon.Dungeon>();
            builder.Services.AddScoped<DungeonService>();
            builder.Services.AddScoped<DungeonViewModel>();
            builder.Services.AddScoped<DungeonGridViewModel>();
            builder.Services.AddScoped<DungeonPage>();
            builder.Services.AddScoped<DungeonGrid>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
