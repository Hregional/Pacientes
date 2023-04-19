using DatosPacientes.Models;
using DatosPacientes.Models.SP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatosPacientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaLegacyController : ControllerBase
    {

        private readonly RecepcionV2Context _context;

        public BusquedaLegacyController(RecepcionV2Context context)
        {
            _context = context;
        }
        [HttpGet("{NohistoriaClinica}")]
        public Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorNoHistoriaClinica(string NohistoriaClinica)
        {
            var resultado = _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorNoHistoriaClinica @NoHistoriaClinica = {0}",
                NohistoriaClinica).ToListAsync();
            return resultado;
        }

        [HttpGet("nombre")]
        public Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorNombre(

            string PrimerNombre,
            string PrimerApellido,
            string? SegundoNombre = null,
            string? SegundoApellido = null,
            string? TercerApellido = null
            )
        {

            // Check if the parameters are null, and if so, set them to DBNull.Value

            SegundoNombre = SegundoNombre ?? DBNull.Value.ToString();
            SegundoApellido = SegundoApellido ?? DBNull.Value.ToString();
            TercerApellido = TercerApellido ?? DBNull.Value.ToString();

            var resultado = _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarPorNombre @PrimerNombre = {0}," +
                " @SegundoNombre = {1}, @PrimerApellido = {2}, @SegundoApellido = {3}, @TercerApellido = {4}",
                PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, TercerApellido).ToListAsync();
            return resultado;
        }

        [HttpGet("avanzado/{cui}")]
        public Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorCui(string cui)
        {
            var resultado = _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorCodigoRENAP @RENAP = {0}",
                cui).ToListAsync();
            return resultado;
        }
    }
}
