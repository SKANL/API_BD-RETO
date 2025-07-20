using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBD.Application.Dtos;
using ApiBD.Core.Entities;
using ApiBD.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiBD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoCaducidadController : ControllerBase
    {
        private readonly IGenericRepository<ProductoCaducidad> _repo;
        private readonly IMapper _mapper;

        public ProductoCaducidadController(IGenericRepository<ProductoCaducidad> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoCaducidadDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProductoCaducidadDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoCaducidadDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProductoCaducidadDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProductoCaducidadDto>> Create(ProductoCaducidadDto dto)
        {
            var entity = _mapper.Map<ProductoCaducidad>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            var result = _mapper.Map<ProductoCaducidadDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.IdProducto }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductoCaducidadDto dto)
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
