using MauiTimesheet.Data;
using MauiTimesheet.Services;
using Microsoft.Extensions.Logging;

namespace MauiTimesheet
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<AuthService>()
                            .AddSingleton<ProjectsService>()
                            .AddSingleton<TimeLogService>();

            return builder.Build();
        }
    }
}
