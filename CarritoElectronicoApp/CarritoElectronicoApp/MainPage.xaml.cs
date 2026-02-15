using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Views;

namespace CarritoElectronicoApp
{
    public partial class MainPage : ContentPage
    {
        List<Producto> productos = new List<Producto>();
        private readonly CarritoService _carritoService;
        public MainPage(CarritoService carritoService)
        {
            InitializeComponent();
            _carritoService = carritoService;
         

            CargarProductos();
            CargarCategorias();
        }

        void CargarProductos()
        {
            productos = new List<Producto>
    {
        new Producto { Nombre="Laptop Gamer", Descripcion="16GB RAM, RTX 4060", Precio=1500, Imagen="laptop.png", Categoria="Computadoras"},
        new Producto { Nombre="Smartphone 5G", Descripcion="128GB, Cámara 108MP", Precio=800, Imagen="phone1.jpg", Categoria="Celulares"},
        new Producto { Nombre="Tablet Pro", Descripcion="Pantalla 12 pulgadas", Precio=600, Imagen="tablet.png", Categoria="Tablets"},
        new Producto { Nombre="Audífonos Bluetooth", Descripcion="Cancelación de ruido", Precio=200, Imagen="headphones1.jpg", Categoria="Accesorios"},
        new Producto { Nombre="Smartwatch", Descripcion="Monitoreo cardíaco", Precio=250, Imagen="watch.jpg", Categoria="Accesorios"},

        new Producto { Nombre="Laptop Ultrabook", Descripcion="8GB RAM, 512GB SSD", Precio=1100, Imagen="ultrabook.jpg", Categoria="Computadoras"},
        new Producto { Nombre="PC de Escritorio", Descripcion="Ryzen 7, 32GB RAM", Precio=1800, Imagen="desktop.jpg", Categoria="Computadoras"},
        new Producto { Nombre="Monitor 4K", Descripcion="27 pulgadas UHD", Precio=400, Imagen="monitor.jpg", Categoria="Computadoras"},

        new Producto { Nombre="iPhone Pro", Descripcion="256GB, Chip A17", Precio=1200, Imagen="iphone.jpg", Categoria="Celulares"},
        new Producto { Nombre="Samsung Galaxy S", Descripcion="256GB, AMOLED 120Hz", Precio=1000, Imagen="galaxy.jpg", Categoria="Celulares"},
        new Producto { Nombre="Xiaomi Redmi Note", Descripcion="128GB, 64MP", Precio=350, Imagen="xiaomi.jpg", Categoria="Celulares"},

        new Producto { Nombre="iPad Air", Descripcion="10.9 pulgadas, 256GB", Precio=750, Imagen="ipadair.jpg", Categoria="Tablets"},
        new Producto { Nombre="Tablet Samsung Tab S", Descripcion="Pantalla AMOLED 11 pulgadas", Precio=650, Imagen="tabs.jpg", Categoria="Tablets"},

        new Producto { Nombre="Teclado Mecánico RGB", Descripcion="Switch Blue, Retroiluminado", Precio=120, Imagen="keyboard.jpeg", Categoria="Accesorios"},
        new Producto { Nombre="Mouse Gamer", Descripcion="16000 DPI, RGB", Precio=80, Imagen="mouse.jpg", Categoria="Accesorios"},
        new Producto { Nombre="Bocina Bluetooth", Descripcion="Resistente al agua", Precio=150, Imagen="speaker.jpg", Categoria="Accesorios"},
        new Producto { Nombre="Cámara Web HD", Descripcion="1080p con micrófono", Precio=90, Imagen="webcam.jpg", Categoria="Accesorios"},
        new Producto { Nombre="Disco Duro Externo", Descripcion="2TB USB 3.0", Precio=130, Imagen="hdd.jpg", Categoria="Accesorios"}

    };

            ListaProductos.ItemsSource = productos;
        }

        void CargarCategorias()
        {
            var categorias = productos
                .Select(p => p.Categoria)
                .Distinct()
                .ToList();

            categorias.Insert(0, "Todas");

            PickerCategoria.ItemsSource = categorias;
            PickerCategoria.SelectedIndex = 0;
        }


        private void OnCategoriaChanged(object sender, EventArgs e)
        {
            var categoriaSeleccionada = PickerCategoria.SelectedItem?.ToString();

            if (categoriaSeleccionada == "Todas")
            {
                ListaProductos.ItemsSource = productos;
            }
            else
            {
                ListaProductos.ItemsSource = productos
                    .Where(p => p.Categoria == categoriaSeleccionada)
                    .ToList();
            }
        }



        private async void OnAgregarCarritoClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var producto = button?.BindingContext as Producto;

            if (producto != null)
            {
                await _carritoService.AgregarProductoAsync(producto);
                await DisplayAlert("Carrito", "Producto agregado correctamente", "OK");
            }
        }


        private async void OnVerCarritoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CarritoPage));
        }



    }
}
