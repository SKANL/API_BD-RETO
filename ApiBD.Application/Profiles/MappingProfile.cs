using ApiBD.Application.Dtos;
using ApiBD.Core.Entities;
using AutoMapper;

namespace ApiBD.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Proveedor, ProveedorDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            CreateMap<ProductoCaducidad, ProductoCaducidadDto>().ReverseMap();
            CreateMap<Venta, VentaDto>().ReverseMap();
            CreateMap<DetalleVenta, DetalleVentaDto>().ReverseMap();
            // ... map other entities and DTOs
        }
    }
}
