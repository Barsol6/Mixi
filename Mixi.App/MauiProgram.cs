using Microsoft.Extensions.Logging;
using Mixi.Shared.Models.Account;
using Mixi.Shared.Models.UI;

namespace Mixi.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();


        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();

        builder.Services.AddScoped(sp => new HttpClient
        {
           BaseAddress= new Uri("https://localhost:7079")
        });
        
        builder.Services.AddSingleton<SignUpPopup,SignUpPopup>();
        builder.Services.AddSingleton<MenuPopup,MenuPopup>();
        builder.Services.AddSingleton<NameGeneratorPopup,NameGeneratorPopup>();
        builder.Services.AddSingleton<PdfPopup,PdfPopup>();
        builder.Services.AddSingleton<Account, Account>();
        
        
        return builder.Build();
    }
}