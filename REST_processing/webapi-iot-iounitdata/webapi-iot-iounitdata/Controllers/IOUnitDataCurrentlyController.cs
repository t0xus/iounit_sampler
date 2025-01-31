using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_iot_growdata5.Models;

namespace webapi_iot_growdata5.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IOUnitDataCurrentlyController : ControllerBase
    {
        private ApplicationDbContext _context;

        public IOUnitDataCurrentlyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/IOUnitDataCurrently
        [HttpGet]
        public async Task<ActionResult<IEnumerable<iounit_data_currently>>> GetAllData()
        {
            return await _context.iounit_data_currently.ToListAsync();
        }

        // GET: api/IOUnitDataCurrently/retrieve_data
        [HttpGet("retrieve_data")]
        public async Task<ActionResult<IEnumerable<iounit_data_currently>>> RetrieveDataById([FromQuery] int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Bitte eine ID angeben.");
            }

            // Den passenden Datensatz anhand der ID holen
            var result = await _context.iounit_data_currently.FindAsync(id.Value);
            
            if (result == null)
            {
                return NotFound($"Kein Eintrag mit id_sdm={id.Value} gefunden.");
            }

            // direction_stamp_a auf das aktuelle Datum/Zeit setzen
            result.direction_stamp_a = DateTime.Now;

            // Änderungen an der DB speichern
            await _context.SaveChangesAsync();

            // Die gesamte Entität zurückgeben (bzw. nur item.value_numerical, falls gewünscht)
            return new List<iounit_data_currently> { result }; ;
        }

        // GET: api/IOUnitDataCurrently/specific
        [HttpGet("specific")]
        public async Task<ActionResult<iounit_data_currently>> GetData(
            [FromQuery] int? id, [FromQuery] int? value_numerical)
        {
            // Falls der Parameter "id" übergeben wurde, rufe GetDataById auf
            if (id.HasValue)
            {
                var result = await GetDataById(id.Value);
                if (result.Value == null)
                {
                    return NotFound($"Keine Daten für ID {id.Value} gefunden.");
                }
                return result;
            }

            // Falls der Parameter "value_numerical" übergeben wurde, rufe GetDataByValueNumerical auf
            if (value_numerical.HasValue)
            {
                var result = await GetDataByValueNumerical(value_numerical.Value);
                if (result.Value == null)
                {
                    return NotFound($"Keine Daten für value_numerical {value_numerical.Value} gefunden.");
                }
                return result;
            }

            // Wenn keiner der Parameter übergeben wurde, Fehler zurückgeben
            return BadRequest("Bitte einen Parameter (id oder value_numerical) angeben.");
        }

        // Neuen PUT-Endpoint zum Ändern des value_numerical hinzufügen:
        [HttpPut("updateValue")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateValueNumericalById([FromQuery] int id, [FromQuery] decimal newValue)
        {
            // Datensatz anhand der ID holen
            var record = await _context.iounit_data_currently.FindAsync(id);
            if (record == null)
            {
                return NotFound($"Keine Daten für ID {id} gefunden.");
            }

            // Wert anpassen
            record.value_numerical = newValue;

            // Änderungen speichern
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  $"Fehler beim Speichern in der Datenbank: {ex.Message}");
            }

            return Ok($"Der Wert value_numerical für ID {id} wurde erfolgreich auf {newValue} gesetzt.");
        }

        // Private Methode: Abruf von Daten nach ID
        private async Task<ActionResult<iounit_data_currently>> GetDataById(int id)
        {
            var ret_data = await _context.iounit_data_currently.FindAsync(id);

            if (ret_data == null)
            {
                return NotFound();
            }

            return ret_data;
        }

        // Private Methode: Abruf von Daten nach value_numerical
        private async Task<ActionResult<iounit_data_currently>> GetDataByValueNumerical(int value_numerical)
        {
            var ret_data = await _context.iounit_data_currently
                .FirstOrDefaultAsync(s => s.value_numerical == value_numerical);

            if (ret_data == null)
            {
                return NotFound();
            }

            return ret_data;
        }
    }
}