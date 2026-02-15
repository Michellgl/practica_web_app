using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;
using CarritoElectronicoApp.Views;
using Microsoft.Extensions.Logging;
using QuestPDF.Infrastructure;




namespace CarritoElectronicoApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            #if WINDOWS
            QuestPDF.Settings.License = LicenseType.Community;
            #endif


            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<CarritoService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CarritoPage>();
            builder.Services.AddTransient<PagoPage>();
            




            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
