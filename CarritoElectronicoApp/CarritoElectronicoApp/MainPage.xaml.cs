using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;
using CarritoElectronicoApp.Views;

namespace CarritoElectronicoApp;

public partial class MainPage : ContentPage
{
    private readonly CarritoService _carritoService;
    private readonly ApiService _apiService;
    private readonly DatabaseService _database;
    private List<Producto> _todosLosProductos = new List<Producto>();

    public MainPage(CarritoService carritoService, ApiService apiService, DatabaseService database)
    {
        InitializeComponent();
        _carritoService = carritoService;
        _apiService = apiService;
        _database = database;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CargarProductosDesdeApi();
    }

    private async Task CargarProductosDesdeApi()
    {
        try
        {
            var productosApi = await _apiService.ObtenerProductosAsync();
            if (productosApi != null && productosApi.Count > 0)
            {
                _todosLosProductos = productosApi;
                ListaProductos.ItemsSource = _todosLosProductos;
                CargarCategorias();
            }
            else
            {
                await DisplayAlert("Aviso", "No se encontraron productos en el servidor.", "OK");
            }
        }
        catch (Exception)
        {
            await DisplayAlert("Error", "Fallo al conectar con la API. Verifica que el servidor Spring Boot esté corriendo.", "OK");
        }
    }

    private void CargarCategorias()
    {
        var categorias = _todosLosProductos.Select(p => p.Categoria).Distinct().ToList();
        categorias.Insert(0, "Todas");
        PickerCategoria.ItemsSource = categorias;
        PickerCategoria.SelectedIndex = 0;
    }

    private void OnCategoriaChanged(object sender, EventArgs e)
    {
        var selec = PickerCategoria.SelectedItem?.ToString();
        ListaProductos.ItemsSource = (selec == "Todas") ? _todosLosProductos : _todosLosProductos.Where(p => p.Categoria == selec).ToList();
    }

    private async void OnAgregarCarritoClicked(object sender, EventArgs e)
    {
        if (((Button)sender).BindingContext is Producto producto)
        {
            await _carritoService.AgregarProductoAsync(producto);
            await DisplayAlert("Carrito", $"{producto.Nombre} agregado correctamente", "OK");
        }
    }

    private async void OnVerCarritoClicked(object sender, EventArgs e)
    {
        // Se pasan los 3 servicios para evitar errores de constructor
        await Navigation.PushAsync(new CarritoPage(_carritoService, _database, _apiService));
    }
}