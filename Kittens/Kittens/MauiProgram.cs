using Kittens.Views;
using Kittens.Services;
using Kittens.ViewModel;
using CommunityToolkit.Maui;

namespace Kittens;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        }).UseMauiCommunityToolkit();
        builder.Services.AddTransient<GameConnect>();
        builder.Services.AddTransient<GameViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<RulesPage>();
        builder.Services.AddTransient<GamePage>();
        return builder.Build();
    }
}