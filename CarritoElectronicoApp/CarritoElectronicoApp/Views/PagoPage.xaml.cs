using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;

namespace CarritoElectronicoApp.Views;

public partial class PagoPage : ContentPage
{
    private readonly CarritoService _carritoService;

    public PagoPage(CarritoService carritoService)
    {
        InitializeComponent();
        _carritoService = carritoService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Mostrar productos en resumen
        ListaResumen.ItemsSource = _carritoService.Items;

        // Mostrar total
        var total = _carritoService.ObtenerTotal();
        LabelTotal.Text = $"Total: ${total:F2}";
    }

    private async void OnConfirmarPagoClicked(object sender, EventArgs e)
    {
        if (PickerMetodoPago.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Selecciona un método de pago", "OK");
            return;
        }

        LoadingOverlay.IsVisible = true;
        ((Button)sender).IsEnabled = false;

        try
        {
            // Total y método de pago
            var total = _carritoService.ObtenerTotal();
            var metodo = PickerMetodoPago.SelectedItem?.ToString() ?? "Desconocido";

            // Generar ticket simulado en texto
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("********** TICKET DE COMPRA **********");
            sb.AppendLine($"Fecha: {DateTime.Now}");
            sb.AppendLine($"Método de Pago: {metodo}");
            sb.AppendLine("--------------------------------------");

            foreach (var item in _carritoService.Items)
            {
                sb.AppendLine($"{item.Nombre} x{item.Cantidad} - ${item.Precio * item.Cantidad:F2}");
            }

            sb.AppendLine("--------------------------------------");
            sb.AppendLine($"TOTAL: ${total:F2}");
            sb.AppendLine("¡Gracias por su compra!");
            sb.AppendLine("**************************************");

            // Mostrar ticket en un mensaje
            await DisplayAlert("Ticket de Compra", sb.ToString(), "OK");

            // Vaciar carrito y volver al inicio
            await _carritoService.VaciarCarritoAsync();
            await Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al procesar el pago: {ex.Message}", "OK");
        }
        finally
        {
            LoadingOverlay.IsVisible = false;
            ((Button)sender).IsEnabled = true;
        }
    }
}
