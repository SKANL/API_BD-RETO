namespace ApiBD.Application.Dtos
{
    public class CategoriaDto
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
    }
}
