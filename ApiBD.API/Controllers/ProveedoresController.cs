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
    public class ProveedoresController : ControllerBase
    {
        private readonly IGenericRepository<Proveedor> _repo;
        private readonly IMapper _mapper;
        public ProveedoresController(IGenericRepository<Proveedor> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ProveedorDto>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            return Ok(_mapper.Map<ProveedorDto>(entity));
        }

        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> Create(ProveedorDto dto)
        {
            var entity = _mapper.Map<Proveedor>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            var result = _mapper.Map<ProveedorDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = result.IdProveedor }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProveedorDto dto)
        {
            if (id != dto.IdProveedor) return BadRequest();
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
