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
    public class CategoriasController : ControllerBase
    {
        private readonly IGenericRepository<Categoria> _repo;
        private readonly IMapper _mapper;
        public CategoriasController(IGenericRepository<Categoria> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
        {
            var entities = await _repo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<CategoriaDto>>(entities);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDto>> Get(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return NotFound();
            var dto = _mapper.Map<CategoriaDto>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaDto>> Create(CategoriaDto dto)
        {
            var entity = _mapper.Map<Categoria>(dto);
            await _repo.AddAsync(entity);
            await _repo.SaveAsync();
            var resultDto = _mapper.Map<CategoriaDto>(entity);
            return CreatedAtAction(nameof(Get), new { id = resultDto.IdCategoria }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoriaDto dto)
        {
            if (id != dto.IdCategoria) return BadRequest();
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
