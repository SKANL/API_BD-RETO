using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBD.Application.Dtos;
using ApiBD.Core.Entities;
using ApiBD.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IGenericRepository<Producto> _repo;
        private readonly IMapper _mapper;

        public ProductosController(IGenericRepository<Producto> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProductoDto>(entity));
        }

        [HttpGet("barcode/{codigo}")]
        public async Task<ActionResult<ProductoDto>> GetByBarcode(string codigo)
        {
            var entities = await _repo.GetAllAsync();
            var entity = entities.FirstOrDefault(p => p.CodigoDeBarra == codigo);
            if (entity == null) return NotFound(new { message = "Producto no encontrado con ese código de barras." });
            return Ok(_mapper.Map<ProductoDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create(ProductoDto dto)
        {
            var existingProducts = await _repo.GetAllAsync();
            
            // Validar si el nombre del producto ya existe
            if (existingProducts.Any(p => p.Nombre == dto.Nombre))
            {
                return Conflict(new { message = $"El producto '{dto.Nombre}' ya está registrado." });
            }

            // Validar si el código de barras ya existe
            if (!string.IsNullOrEmpty(dto.CodigoDeBarra))
            {
                if (existingProducts.Any(p => p.CodigoDeBarra == dto.CodigoDeBarra))
                {
                    return Conflict(new { message = $"El código de barras '{dto.CodigoDeBarra}' ya está registrado." });
                }
            }

            var entity = _mapper.Map<Producto>(dto);
            try
            {
                await _repo.AddAsync(entity);
                await _repo.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al crear el producto.", details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });
            }

            return CreatedAtAction(nameof(GetById), new { id = entity.IdProducto }, _mapper.Map<ProductoDto>(entity));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoDto dto)
        {
            if (id != dto.IdProducto) return BadRequest("El ID del producto no coincide.");
            
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound("Producto no encontrado.");

            // Validar si el nuevo nombre ya existe en otro producto
            var existingProducts = await _repo.GetAllAsync();
            if (existingProducts.Any(p => p.IdProducto != id && p.Nombre == dto.Nombre))
            {
                return Conflict(new { message = $"El nombre '{dto.Nombre}' ya está registrado en otro producto." });
            }

            // Validar si el nuevo código de barras ya existe en otro producto
            if (!string.IsNullOrEmpty(dto.CodigoDeBarra))
            {
                if (existingProducts.Any(p => p.IdProducto != id && p.CodigoDeBarra == dto.CodigoDeBarra))
                {
                    return Conflict(new { message = $"El código de barras '{dto.CodigoDeBarra}' ya está registrado en otro producto." });
                }
            }

            try
            {
                // Mapear los datos del DTO a la entidad existente
                entity.Nombre = dto.Nombre;
                entity.CodigoDeBarra = dto.CodigoDeBarra;
                entity.PrecioCosto = dto.PrecioCosto;
                entity.PrecioVenta = dto.PrecioVenta;
                entity.StockActual = dto.StockActual;
                entity.StockMinimo = dto.StockMinimo;
                entity.IdCategoria = dto.IdCategoria;
                entity.IdProveedor = dto.IdProveedor;

                _repo.Update(entity);
                await _repo.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el producto.", details = ex.InnerException?.Message ?? ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor.", details = ex.Message });
            }
            
            return NoContent();
        }
    }
}