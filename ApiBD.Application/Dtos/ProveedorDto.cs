namespace ApiBD.Application.Dtos
{
    public class ProveedorDto
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? DiaRecarga { get; set; }
    }
}
