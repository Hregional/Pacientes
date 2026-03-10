using DatosPacientes.Models;
using DatosPacientes.Models.SP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatosPacientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BusquedaLegacyController : ControllerBase
    {

        private readonly RecepcionV2Context _context;

        public BusquedaLegacyController(RecepcionV2Context context)
        {
            _context = context;
        }
        [HttpGet("{NohistoriaClinica}")]
        public async Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorNoHistoriaClinica(string NohistoriaClinica)
        {
            var resultado = await _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorNoHistoriaClinica @NoHistoriaClinica = {0}",
                NohistoriaClinica).ToListAsync();

            NormalizarLegacy(resultado);
            return resultado;
        }

        [HttpGet("nombre")]
        public async Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorNombre(

            string PrimerNombre,
            string PrimerApellido,
            string? SegundoNombre = null,
            string? SegundoApellido = null,
            string? TercerApellido = null
            )
        {

            // Check if the parameters are null, and if so, set them to DBNull.Value

            SegundoNombre ??= DBNull.Value.ToString();
            SegundoApellido ??= DBNull.Value.ToString();
            TercerApellido ??= DBNull.Value.ToString();

            var resultado = await _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarPorNombre @PrimerNombre = {0}," +
                " @SegundoNombre = {1}, @PrimerApellido = {2}, @SegundoApellido = {3}, @TercerApellido = {4}",
                PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, TercerApellido).ToListAsync();

            NormalizarLegacy(resultado);
            return resultado;
        }

        [HttpGet("avanzado/{cui}")]
        public async Task<List<PacienteSeleccionarCatalogo>> GetSeleccionarPorCui(string cui)
        {
            // Esto elimina cualquier carácter que no sea un número (espacios, guiones, letras)
            string cuiSoloNumeros = new string(cui.Where(char.IsDigit).ToArray());

            // Si después de limpiar queda vacío (o era "-1"), mantenemos el comportamiento original del SP
            if (string.IsNullOrEmpty(cuiSoloNumeros) && cui != "-1") cuiSoloNumeros = "0";
            else if (cui == "-1") cuiSoloNumeros = "-1";

            var resultado = await _context.PacienteSeleccionarCatalogo
                .FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorCodigoRENAP @RENAP = {0}", cuiSoloNumeros)
                .ToListAsync();

            NormalizarLegacy(resultado);
            return resultado;
        }

        public static void NormalizarLegacy(List<PacienteSeleccionarCatalogo> pacientes)
        {
            pacientes.ForEach(p =>
            {
                p.Padre = p.Padre?.Trim();
                p.Madre = p.Madre?.Trim();
                p.Nombres = p.Nombres?.Trim();
                p.Apellidos = p.Apellidos?.Trim();
                p.Historia_Clinica = p.Historia_Clinica?.Trim();
                p.Procedencia = p.Procedencia?.Trim();
            });
        }
    }
}