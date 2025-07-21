using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ApiBD.Application.Dtos;
using ApiBD.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiBD.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VistasController : ControllerBase
    {
        private readonly DbContext _context;

        public VistasController(DbContext context)
        {
            _context = context;
        }

        [HttpGet("productos-status")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetProductosStatus()
        {
            var result = await _context.Set<dynamic>().FromSqlRaw("SELECT * FROM v_productos_status").ToListAsync();
            return Ok(result);
        }

        [HttpGet("stock-bajo")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetStockBajo()
        {
            var result = await _context.Set<dynamic>().FromSqlRaw("SELECT * FROM v_stock_bajo").ToListAsync();
            return Ok(result);
        }
    }
}
