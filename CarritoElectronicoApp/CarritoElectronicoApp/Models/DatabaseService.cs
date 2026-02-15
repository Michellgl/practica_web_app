using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using CarritoElectronicoApp.Models;

namespace CarritoElectronicoApp.Services
{
    public class DatabaseService

    {
        private SQLiteAsyncConnection _database;

        public async Task Init()
        {
            if (_database != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "carrito.db");

            _database = new SQLiteAsyncConnection(databasePath);

            await _database.CreateTableAsync<Producto>();
            await _database.CreateTableAsync<Compra>();
            await _database.CreateTableAsync<DetalleCompra>();

        }

        public async Task<List<Producto>> GetProductosAsync()
        {
            await Init();
            return await _database.Table<Producto>().ToListAsync();
        }

        public async Task AddProductoAsync(Producto producto)
        {
            await Init();
            await _database.InsertAsync(producto);
        }

        public async Task DeleteProductoAsync(Producto producto)
        {
            await Init();
            await _database.DeleteAsync(producto);
        }

        public async Task ClearAsync()
        {
            await Init();
            await _database.DeleteAllAsync<Producto>();
        }
        public Task<int> UpdateProductoAsync(Producto producto)
        {
            return _database.UpdateAsync(producto);
        }
        public Task<int> InsertAsync<T>(T entity)
        {
            return _database.InsertAsync(entity);
        }

        public Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return _database.Table<T>().ToListAsync();
        }


    }
}

