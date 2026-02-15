using System;
using System.Collections.Generic;
using System.Text;

namespace CarritoElectronicoApp.Models
{
    using SQLite;

    public class DetalleCompra
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int CompraId { get; set; }

        public string NombreProducto { get; set; }

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }
    }


}
