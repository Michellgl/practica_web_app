using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;
namespace CarritoElectronicoApp.Views;

public partial class PagoPage : ContentPage
{
    private readonly CarritoService _carritoService;
    private readonly DatabaseService _database;



    public PagoPage(CarritoService carritoService, DatabaseService database)
    {
        InitializeComponent();

        _carritoService = carritoService;
        _database = database;
    }
        protected override void OnAppearing()
    {
        base.OnAppearing();

        ListaResumen.ItemsSource = _carritoService.Items;
        LabelTotal.Text = $"Total: ${_carritoService.ObtenerTotal()}";
    
    }
   


    private async void OnConfirmarPagoClicked(object sender, EventArgs e)
    {
        if (PickerMetodoPago.SelectedIndex == -1)
        {
            await DisplayAlert("Error", "Selecciona un método de pago", "OK");
            return;
        }

        var metodo = PickerMetodoPago.SelectedItem.ToString();

        var ticketService = new TicketService();

        var productos = _carritoService.Items.ToList();
        var total = _carritoService.ObtenerTotal();

        var ruta = ticketService.GenerarTicket(productos, total, metodo);

        await DisplayAlert("Pago Exitoso",
            "Tu compra fue realizada correctamente 🎉",
            "OK");

        await ticketService.AbrirTicketAsync(ruta);


        await _carritoService.VaciarCarritoAsync();

        await Navigation.PopToRootAsync();

        // 1. Guardar compra
        var compra = new Compra
        {
            Fecha = DateTime.Now,
            Total = total,
            MetodoPago = metodo
        };

        await _database.InsertAsync(compra);

        foreach (var item in productos)
        {
            var detalle = new DetalleCompra
            {
                CompraId = compra.Id,
                NombreProducto = item.Nombre,
                Cantidad = item.Cantidad,
                Precio = item.Precio
            };

            await _database.InsertAsync(detalle);
        }

        // 2. Vaciar carrito
        await _carritoService.VaciarCarritoAsync();

        // 3. Navegar
        await Navigation.PopToRootAsync();



    }





}
