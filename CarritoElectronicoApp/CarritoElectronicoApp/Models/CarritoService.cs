using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using CarritoElectronicoApp.Models;
using CarritoElectronicoApp.Services;

namespace CarritoElectronicoApp.Models
{
    public class CarritoService
    {
        private readonly DatabaseService _database;

        public ObservableCollection<Producto> Items { get; set; }
            = new ObservableCollection<Producto>();

        public CarritoService(DatabaseService database)
        {
            _database = database;
        }

        public async Task InicializarAsync()
        {
            var productos = await _database.GetProductosAsync();

            Items.Clear();

            foreach (var producto in productos)
                Items.Add(producto);
        }

        public async Task AgregarProductoAsync(Producto producto)
        {
            var existente = Items.FirstOrDefault(p => p.Id == producto.Id);

            if (existente != null)
            {
                existente.Cantidad++;
                await _database.UpdateProductoAsync(existente);
            }
            else
            {
                producto.Cantidad = 1;
                Items.Add(producto);
                await _database.AddProductoAsync(producto);
            }
        }

        public async Task DisminuirCantidadAsync(Producto producto)
        {
            var item = Items.FirstOrDefault(p => p.Id == producto.Id);

            if (item != null)
            {
                if (item.Cantidad > 1)
                {
                    item.Cantidad--;
                    await _database.UpdateProductoAsync(item);
                }
                else
                {
                    Items.Remove(item);
                    await _database.DeleteProductoAsync(item);
                }
            }
        }

        public async Task AumentarCantidadAsync(Producto producto)
        {
            var item = Items.FirstOrDefault(p => p.Id == producto.Id);

            if (item != null)
            {
                item.Cantidad++;
                await _database.UpdateProductoAsync(item);
            }
        }



        public async Task EliminarProductoAsync(Producto producto)
        {
            Items.Remove(producto);
            await _database.DeleteProductoAsync(producto);
        }

        public decimal ObtenerTotal()
        {
            return Items.Sum(p => p.Precio * p.Cantidad);
        }

        public async Task VaciarCarritoAsync()
        {
            Items.Clear();
            await _database.ClearAsync();
        }
    }
}
