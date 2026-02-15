using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;

namespace CarritoElectronicoApp.Views;

public partial class CarritoPage : ContentPage
{
    private readonly CarritoService _carritoService;
    private readonly DatabaseService _database;


    public CarritoPage(CarritoService carritoService, DatabaseService database)
    {
        InitializeComponent();

        _carritoService = carritoService;
        _database = database;

        ListaCarrito.ItemsSource = _carritoService.Items;

        CargarCarrito();
    }



    private async void CargarCarrito()
    {
        await _carritoService.InicializarAsync();
        ActualizarTotal();
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var producto = button?.BindingContext as Producto;

        if (producto != null)
        {
            await _carritoService.EliminarProductoAsync(producto);
            ActualizarTotal();
        }
    }

    private void ActualizarTotal()
    {
        LabelTotal.Text = $"Total: ${_carritoService.ObtenerTotal()}";
    }

    private async void OnAumentarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var producto = button?.BindingContext as Producto;

        if (producto != null)
        {
            await _carritoService.AumentarCantidadAsync(producto);
            ActualizarTotal();
        }
    }

    private async void OnDisminuirClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var producto = button?.BindingContext as Producto;

        if (producto != null)
        {
            await _carritoService.DisminuirCantidadAsync(producto);
            ActualizarTotal();
        }
    }
    private async void OnIrPagoClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PagoPage(_carritoService, _database));

    }
}
