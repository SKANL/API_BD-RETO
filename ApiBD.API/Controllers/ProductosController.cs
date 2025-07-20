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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductoDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProductoDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create(ProductoDto dto)
        {
            // Check if the product name already exists
            var existingProduct = await _repo.GetAllAsync();
            if (existingProduct.Any(p => p.Nombre == dto.Nombre))
            {
                return Conflict(new { message = $"El producto '{dto.Nombre}' ya está registrado." });
            }

            var entity = _mapper.Map<Producto>(dto);
            try
            {
                await _repo.AddAsync(entity);
                await _repo.SaveAsync();
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Error al guardar el producto.", details = ex.Message });
            }

            var result = _mapper.Map<ProductoDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.IdProducto }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoDto dto)
        {
            if (id != dto.IdProducto) return BadRequest();
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            _mapper.Map(dto, entity);
            _repo.Update(entity);
            await _repo.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            _repo.Delete(entity);
            await _repo.SaveAsync();
            return NoContent();
        }
    }
}
