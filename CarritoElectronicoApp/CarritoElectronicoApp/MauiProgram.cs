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
            // Configuración de licencia de PDF (lo que ella ya tenía)
#if WINDOWS
            QuestPDF.Settings.License = LicenseType.Community;
#endif

            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // --- INYECCIÓN DE DEPENDENCIAS ---

            // 1. Servicios de Datos (Base de datos local y Carrito)
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<CarritoService>();

            // 2. NUEVO: Servicio de API (Para conectar con tu Spring Boot)
            // Sin esta línea, la app fallará al intentar enviar la compra
            builder.Services.AddSingleton<ApiService>();

            // 3. Páginas (Vistas)
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<CarritoPage>();
            builder.Services.AddTransient<PagoPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}