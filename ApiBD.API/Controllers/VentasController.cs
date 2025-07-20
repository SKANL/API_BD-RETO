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
    public class VentasController : ControllerBase
    {
        private readonly IGenericRepository<Venta> _repo;
        private readonly IMapper _mapper;

        public VentasController(IGenericRepository<Venta> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<VentaDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<VentaDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create(VentaDto dto)
        {
            var entity = _mapper.Map<Venta>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            var result = _mapper.Map<VentaDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.IdVenta }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, VentaDto dto)
        {
            if (id != dto.IdVenta) return BadRequest();
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
