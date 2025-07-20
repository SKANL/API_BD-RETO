namespace ApiBD.Application.Dtos
{
    public class DetalleVentaDto
    {
        public int IdDetalle { get; set; }
        public int? IdVenta { get; set; }
        public int? IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
