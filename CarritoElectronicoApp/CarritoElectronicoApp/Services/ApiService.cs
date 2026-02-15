using System.Net.Http.Json;
using System.Text.Json;
using CarritoElectronicoApp.Models;

namespace CarritoElectronicoApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService()
        {
            // --- AQUÍ ESTÁ LA SOLUCIÓN DEL ERROR DE CONEXIÓN ---
            // Detectamos si está corriendo en Android (Emulador) o en Windows (PC)
            string baseUrl;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                // IP especial para el Emulador de Android
                baseUrl = "http://10.0.2.2:8080/api/";
            }
            else
            {
                // IP para Windows (localhost)
                baseUrl = "http://localhost:8080/api/";
            }

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(baseUrl);

            // Configuración para que no importen mayúsculas/minúsculas en el JSON
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        // 1. OBTENER PRODUCTOS (GET)
        public async Task<List<Producto>> ObtenerProductosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("articulos");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Producto>>(_jsonOptions);
                }
            }
            catch (Exception ex)
            {
                // Este Console.WriteLine aparecerá en la ventana de "Salida" de Visual Studio
                System.Diagnostics.Debug.WriteLine($"ERROR API: {ex.Message}");
            }

            // Si falla, regresamos lista vacía para que la app no se cierre
            return new List<Producto>();
        }

        // 2. ENVIAR COMPRA (POST)
        public async Task<bool> EnviarCompraAsync(CompraRequest venta)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("ventas", venta, _jsonOptions);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR API POST: {ex.Message}");
                return false;
            }
        }
    }

    // --- CLASES PARA ENVIAR DATOS A TU JAVA SPRING BOOT ---

    public class CompraRequest
    {
        public double total { get; set; }
        public string clienteNombre { get; set; }
        public List<DetalleRequest> detalles { get; set; } = new List<DetalleRequest>();
    }

    public class DetalleRequest
    {
        public int cantidad { get; set; }
        public decimal precioUnitario { get; set; }
        public decimal subtotal { get; set; }
        public ArticuloIdRequest articulo { get; set; }
    }

    public class ArticuloIdRequest
    {
        public int id { get; set; }
    }
}