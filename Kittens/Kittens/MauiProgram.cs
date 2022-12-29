using Kittens.Views;
using Kittens.Services;
using Kittens.ViewModel;

namespace Kittens;

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

        builder.Services.AddSingleton<GameConnect>();
        builder.Services.AddSingleton<GameViewModel>();
        builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<RulesPage>();
		builder.Services.AddTransient<GamePage>();

        return builder.Build();
	}
}

