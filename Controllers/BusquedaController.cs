using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using DatosPacientes.Models.SP;
using AutoMapper;
using DatosPacientes.DTOs;

namespace DatosPacientes.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    public class BusquedaController : ControllerBase
    {
        private readonly RecepcionV2Context _context;
        private readonly IMapper _mapper;

        public BusquedaController(RecepcionV2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        [HttpGet("api/[controller]/avanzado/{cui}")]
        public Task<List<PacienteSeleccionarCatalogo>>
            GetSeleccionarPorCui(string cui)
        {
            var resultado = _context.PacienteSeleccionarCatalogo.
                FromSqlRaw("EXEC dbo.PacienteSeleccionarCatalogoPorCodigoRENAP @RENAP = {0}",
                cui).ToListAsync();
            return resultado;
        }

        [HttpGet("api/[controller]/paciente")]
         public async Task<List<PacienteDTO>> GetPatients(string NoHistoriaClinica) {

            var pacientes = await _context.Pacientes.
                 Where(p => p.NoHistoriaClinica.Contains(NoHistoriaClinica))
                 .ToListAsync();

             var pacienteDto = _mapper.Map<List<PacienteDTO>> (pacientes);

             return pacienteDto;

         }
        [HttpGet("api/[controller]/pacientePaged")]
        public async Task<List<PacienteDTO>> GetPatients(string NoHistoriaClinica, int pageNumber = 1, int pageSize = 10)
        {
            var pacientes = await _context.Pacientes
                .Where(p => p.NoHistoriaClinica.Contains(NoHistoriaClinica))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pacientesDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            return pacientesDto;
        }

        [HttpGet("api/[controller]/pacientes")]
        public async Task<List<PacienteDTO>> GetAllPatients(int pageNumber = 1, int pageSize = 10)
        {
            var pacientes = await _context.Pacientes.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var PacienteDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            return PacienteDto;

        }
    }
}
