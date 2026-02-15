using CarritoElectronicoApp.Views;

namespace CarritoElectronicoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CarritoPage), typeof(CarritoPage));
        }
    }
}
