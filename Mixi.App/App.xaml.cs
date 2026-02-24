using Microsoft.Maui.Controls;

namespace Mixi.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new MainPage();
    }
}