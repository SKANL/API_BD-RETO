namespace ApiBD.Core.Entities
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }
        public int? IdVenta { get; set; }
        public int? IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public Venta? Venta { get; set; }
        public Producto? Producto { get; set; }
    }
}
