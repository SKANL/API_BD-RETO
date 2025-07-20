namespace ApiBD.Application.Dtos
{
    public class ProductoDto
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
    }
}
