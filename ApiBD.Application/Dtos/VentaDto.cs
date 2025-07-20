using System;
using System.Collections.Generic;

namespace ApiBD.Application.Dtos
{
    public class VentaDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public ICollection<DetalleVentaDto>? DetallesVenta { get; set; }
    }
}
