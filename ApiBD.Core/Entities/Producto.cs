using System.Collections.Generic;

namespace ApiBD.Core.Entities
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = null!;
        public string? CodigoDeBarra { get; set; }
        public decimal PrecioCosto { get; set; }
        public decimal PrecioVenta { get; set; }
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int IdCategoria { get; set; }
        public int? IdProveedor { get; set; }

        public Categoria? Categoria { get; set; }
        public Proveedor? Proveedor { get; set; }
        public ProductoCaducidad? ProductoCaducidad { get; set; }
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}
