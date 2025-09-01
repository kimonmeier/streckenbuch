using Microsoft.Extensions.Logging;

namespace Streckenbuch.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSentry(options =>
                {
                    options.Dsn = "https://e7eb475600aa3160b6b8002766c6c25c@o4504980466499584.ingest.us.sentry.io/4509945005604864";
                    
                    options.AttachScreenshot = true;
                    
                    options.SendDefaultPii = true;
                    options.TracesSampleRate = 1.0;
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUGAPP
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
