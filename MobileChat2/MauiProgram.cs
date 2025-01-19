using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MobileChat2.Data.External;
using MudBlazor.Services;

namespace MobileChat2
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

            builder.Services.AddMudServices();
            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            //Auth Config
            builder.Services.AddAuthorizationCore();
            builder.Services.TryAddScoped<AuthenticationStateProvider, ExternalAuthStateProvider>();
            builder.Services.AddScoped<ExternalAuthStateProvider>();

            return builder.Build();
        }
    }
}
