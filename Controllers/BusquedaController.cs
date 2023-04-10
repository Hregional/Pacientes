using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace DatosPacientes.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class BusquedaController : ControllerBase
    {
        private readonly RecepcionV2Context _context;

        public BusquedaController(RecepcionV2Context context)
        {
            _context = context;
        }
        
        // GET: api/Values
        [HttpGet("api/[controller]/{NohistoriaClinica}")]
        public Task<List<PacienteSeleccionarCatalogoPorNoHistoriaClinica>> GetSeleccionarPorNoHistoriaClinica(string NohistoriaClinica)
        {
            var resultado = _context.PacienteSeleccionarCatalogoPorNoHistoriaClinica.FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorNoHistoriaClinica @NoHistoriaClinica = {0}", NohistoriaClinica).ToListAsync();
            return resultado;
        }

    }
}
