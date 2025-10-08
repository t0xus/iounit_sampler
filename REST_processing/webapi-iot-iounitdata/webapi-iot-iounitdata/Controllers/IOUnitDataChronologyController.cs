using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_iot_growdata5.Models;

namespace webapi_iot_growdata5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IOUnitDataChronologyController : ControllerBase
    {
        private ApplicationDbContext _context;

        public IOUnitDataChronologyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<iounit_data_chronology>>> GetAllData()
        {
            return await _context.iounit_data_chronology.ToListAsync();
        }

        // GET: api/IOUnitDataCurrently/specific
        [HttpGet("specific")]
        public async Task<ActionResult<IEnumerable<iounit_data_chronology>>> GetData(
            [FromQuery] int? id, [FromQuery] decimal? value_numerical, [FromQuery] DateTime? start_value, [FromQuery] DateTime? end_value)
        {
            
            if (start_value.HasValue && end_value.HasValue)
            {
                var result = await GetDataBetweenTimes(start_value.Value, end_value.Value);
                
                if (result.Value == null)
                {
                    return NotFound($"Keine Daten für start_value {start_value.Value} und end_value {end_value.Value} gefunden.");
                }
                
                return result;
            }

            if (id.HasValue)
            {
                var result = await GetDataById(id.Value);

                if (result.Value == null)
                {
                    return NotFound($"Keine Daten für start_value {id.Value} gefunden.");
                }

                return result;
            }

            if (value_numerical.HasValue)
            {
                var result = await GetDataByValueNumerical(value_numerical.Value);

                if (result.Value == null)
                {
                    return NotFound($"Keine Daten für start_value {value_numerical.Value} gefunden.");
                }

                return result;
            }

            // Wenn keiner der Parameter übergeben wurde, Fehler zurückgeben
            return BadRequest("Bitte einen Parameter (id oder value_numerical) angeben.");
        }

        [HttpPost("adddata")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddData([FromQuery] decimal value_numerical, [FromQuery] int id_mu, [FromQuery] int id_sdm)
        {
            var newRecord = new iounit_data_chronology
            {
                datetime = DateTime.Now,
                value_numeric = value_numerical,
                id_mu = (short)id_mu,
                id_sdm = (short)id_sdm
            };

            _context.iounit_data_chronology.Add(newRecord);

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Daten erfolgreich hinzugefügt.", Record = newRecord });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"Fehler beim Speichern in der Datenbank: {ex.Message}");
            }
        }


        private async Task<ActionResult<IEnumerable<iounit_data_chronology>>> GetDataById(int id)
        {
            var ret_data = await _context.iounit_data_chronology
                .Where(s => s.id == id)
                .ToListAsync();

            if (ret_data == null)
            {
                return NotFound();
            }

            return ret_data;
        }

        // Private Methode: Abruf von Daten nach value_numerical
        private async Task<ActionResult<IEnumerable<iounit_data_chronology>>> GetDataByValueNumerical(decimal value_numerical)
        {
            var ret_data = await _context.iounit_data_chronology
                .Where(s => s.value_numeric == value_numerical)
                .ToListAsync();

            if (ret_data == null)
            {
                return NotFound();
            }

            return ret_data;
        }

        private async Task<ActionResult<IEnumerable<iounit_data_chronology>>> GetDataBetweenTimes(DateTime start_value, DateTime end_value)
        {
            var ret_data = await _context.iounit_data_chronology
                .Where(s => s.datetime > start_value && s.datetime < end_value)
                .ToListAsync();

            if (ret_data == null || !ret_data.Any())
            {
                return NotFound();
            }

            return ret_data;
        }
    }

}
