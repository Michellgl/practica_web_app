using System;
using System.Collections.Generic;
using System.Text;

namespace CarritoElectronicoApp.Models
{
    using SQLite;

    public class Compra
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public decimal Total { get; set; }

        public string MetodoPago { get; set; }
    }

}
