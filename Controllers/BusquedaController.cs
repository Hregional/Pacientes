using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using DatosPacientes.Models.SP;

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
        
        [HttpGet("api/[controller]/{NohistoriaClinica}")]
        public Task<List<PacienteSeleccionarCatalogo>> 
            GetSeleccionarPorNoHistoriaClinica(string NohistoriaClinica)
        {
            var resultado = _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorNoHistoriaClinica @NoHistoriaClinica = {0}",
                NohistoriaClinica).ToListAsync();
            return resultado;
        }

        [HttpGet("api/[controller]/nombre")]
        public Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorNombre(

            string PrimerNombre,
            string PrimerApellido,
            string? SegundoNombre = null,
            string? SegundoApellido = null,
            string? TercerApellido= null
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

        


    }
}
