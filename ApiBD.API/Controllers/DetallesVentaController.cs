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
    public class DetallesVentaController : ControllerBase
    {
        private readonly IGenericRepository<DetalleVenta> _repo;
        private readonly IMapper _mapper;

        public DetallesVentaController(IGenericRepository<DetalleVenta> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleVentaDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<DetalleVentaDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleVentaDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<DetalleVentaDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<DetalleVentaDto>> Create(DetalleVentaDto dto)
        {
            var entity = _mapper.Map<DetalleVenta>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            var result = _mapper.Map<DetalleVentaDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.IdDetalle }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DetalleVentaDto dto)
        {
            if (id != dto.IdDetalle) return BadRequest();
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
