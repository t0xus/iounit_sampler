using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_iot_growdata5.Models;



namespace webapi_iot_growdata5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IOUnitDataCurrentlyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public IOUnitDataCurrentlyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iounit_data_currently>>> GetAllData()
        {
            return await _context.iounit_data_currently.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<iounit_data_currently>> GetDataById(int id)
        {
            var ret_data = await _context.iounit_data_currently.FindAsync(id);

            if (ret_data == null)
            {
                return NotFound();
            }

            return ret_data;
        }
    }
}
