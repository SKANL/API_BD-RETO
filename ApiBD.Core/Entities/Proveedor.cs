using System.Collections.Generic;

namespace ApiBD.Core.Entities
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? DiaRecarga { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
