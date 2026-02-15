using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;

namespace CarritoElectronicoApp.Views;

public partial class CarritoPage : ContentPage
{
    private readonly CarritoService _carritoService;
    private readonly DatabaseService _database;
    private readonly ApiService _apiService;

    public CarritoPage(CarritoService carritoService, DatabaseService database, ApiService apiService)
    {
        InitializeComponent();
        _carritoService = carritoService;
        _database = database;
        _apiService = apiService;

        ListaCarrito.ItemsSource = _carritoService.Items;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _carritoService.InicializarAsync();
        ActualizarTotal();
    }

    private void ActualizarTotal()
    {
        LabelTotal.Text = $"Total: ${_carritoService.ObtenerTotal():F2}";
    }

    private async void OnAumentarClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is Producto p)
        {
            await _carritoService.AumentarCantidadAsync(p);
            ActualizarTotal();
        }
    }

    private async void OnDisminuirClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is Producto p)
        {
            await _carritoService.DisminuirCantidadAsync(p);
            ActualizarTotal();
        }
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is Producto p)
        {
            await _carritoService.EliminarProductoAsync(p);
            ActualizarTotal();
        }
    }

    private async void OnIrPagoClicked(object sender, EventArgs e)
    {
        if (_carritoService.Items.Count == 0)
        {
            await DisplayAlert("Aviso", "El carrito está vacío", "OK");
            return;
        }

        // Usar el nuevo constructor simplificado
        await Navigation.PushAsync(new PagoPage(_carritoService));
    }

}