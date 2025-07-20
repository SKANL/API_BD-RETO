using System;

namespace ApiBD.Core.Entities
{
    public class ProductoCaducidad
    {
        public int IdProducto { get; set; }
        public DateTime FechaCaducidad { get; set; }

        public Producto Producto { get; set; } = null!;
    }
}
