using Microsoft.UI.Xaml.Controls;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Web.WebView2.Core;

namespace Mixi.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        
        blazorWebView.BlazorWebViewInitialized += (sender, args) =>
        {
#if WINDOWS
            var webView = args.WebView as WebView2;

            if (webView != null)
            {
                webView.EnsureCoreWebView2Async().AsTask().ContinueWith(task =>
                {
                    Dispatcher.Dispatch(() =>
                    {
                        if (webView.CoreWebView2 != null)
                        {
                            webView.CoreWebView2.Profile.PreferredTrackingPreventionLevel =
                                CoreWebView2TrackingPreventionLevel.None;

                            webView.CoreWebView2.Settings.IsScriptEnabled = true;
                            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = true;
                        }
                    });
                });
            }
#endif
        };


    }
}