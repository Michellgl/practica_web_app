using CarritoElectronicoApp.Models;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CarritoElectronicoApp.Services
{
    public class TicketService
    {
        // Genera ticket simulado en .txt
        public string GenerarTicket(List<Producto> items, decimal total, string metodoPago)
        {
            var sb = new StringBuilder();
            sb.AppendLine("********** TICKET DE COMPRA **********");
            sb.AppendLine($"Fecha: {DateTime.Now}");
            sb.AppendLine($"Método de Pago: {metodoPago}");
            sb.AppendLine("--------------------------------------");

            foreach (var item in items)
            {
                sb.AppendLine($"{item.Nombre} x{item.Cantidad} - ${item.Precio * item.Cantidad:F2}");
            }

            sb.AppendLine("--------------------------------------");
            sb.AppendLine($"TOTAL: ${total:F2}");
            sb.AppendLine("¡Gracias por su compra!");
            sb.AppendLine("**************************************");

            var ruta = Path.Combine(FileSystem.AppDataDirectory, $"Ticket_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
            File.WriteAllText(ruta, sb.ToString());

            return ruta;
        }

        // Abre el ticket en el dispositivo
        public async Task AbrirTicketAsync(string ruta)
        {
            if (File.Exists(ruta))
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(ruta)
                });
            }
        }
    }
}
