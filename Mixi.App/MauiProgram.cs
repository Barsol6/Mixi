using Microsoft.Extensions.Logging;
using Mixi.App.Services.Authentication;
using Mixi.Shared.Models.Account;
using Mixi.Shared.Models.UI;
using Mixi.SharedUI.Services.MusicPlayers;

namespace Mixi.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        builder.Services.AddSingleton<ISecureStorage>(SecureStorage.Default);
        builder.Services.AddScoped<IMusicPlayer, YoutubeMusicService>();
        builder.Services.AddScoped<IMusicPlayer, SpotifyMusicService>();
        builder.Services.AddScoped<IMusicPlayer, TidalMusicService>();
        builder.Services.AddScoped<LocalMusicService>();
        builder.Services.AddScoped<IMusicPlayer>(sp => sp.GetRequiredService<LocalMusicService>());

        builder.Services.AddScoped<PlaybackManager>();
        
        builder.Services.AddTransient<AuthTokenHandler>();
        
        builder.Services.AddHttpClient("MyApi", async (serviceProvider, client) =>
        {
            client.BaseAddress = new Uri("https://localhost:7079");
        }).AddHttpMessageHandler<AuthTokenHandler>();
        
        builder.Services.AddScoped(sp => 
            sp.GetRequiredService<IHttpClientFactory>().CreateClient("MyApi"));        

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddMauiBlazorWebView();


        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
        
        
        builder.Services.AddSingleton<SignUpPopup,SignUpPopup>();
        builder.Services.AddSingleton<MenuPopup,MenuPopup>();
        builder.Services.AddSingleton<NameGeneratorPopup,NameGeneratorPopup>();
        builder.Services.AddSingleton<PdfPopup,PdfPopup>();
        builder.Services.AddSingleton<Account, Account>();
        builder.Services.AddSingleton<NotePopup, NotePopup>();
        
        
        return builder.Build();
    }
}