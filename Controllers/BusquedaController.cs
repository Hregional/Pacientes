using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using DatosPacientes.Models.SP;
using AutoMapper;
using DatosPacientes.DTOs;
using AutoMapper.QueryableExtensions;

namespace DatosPacientes.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/[controller]")]
    [ApiController]
    [Route("api/[controller]")]
    public class BusquedaController : ControllerBase
    {
        private readonly RecepcionV2Context _context;
        private readonly IMapper _mapper;

        public BusquedaController(RecepcionV2Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("paciente/{NoHistoriaClinica}")]
        public async Task<ActionResult<List<PacienteCompletoDTO>>> GetPersonas(string NoHistoriaClinica)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(NoHistoriaClinica))
                {
                    return BadRequest("NoHistoriaClinica no puede ser nolo o vacío");
                }

                var query = _context.Pacientes
                .Join(_context.Personas, p => p.Persona, per => per.Codigo, (p, per) => new { Paciente = p, Persona = per })
                .Where(a => (a.Paciente.NoHistoriaClinica.Contains(NoHistoriaClinica) || NoHistoriaClinica == "-1"))
                .OrderBy(a => a.Persona.Nombre1)
                .Select(a => new PacienteCompletoDTO()
                {
                    Codigo = a.Paciente.Codigo,
                    Persona = a.Paciente.Persona,
                    Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                    Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                    NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                    FechaNacimiento = a.Persona.FechaNacimiento,
                    Sexo = a.Persona.Sexo,
                    NombrePadre = a.Paciente.NombrePadre,
                    NombreMadre = a.Paciente.NombreMadre,
                    LugarNacimiento = a.Paciente.LugarNacimiento,
                    Archivo_Fisico = a.Paciente.ArchivoFisico,
                    Nombre_Resposable = a.Paciente.NombreResponsable,
                    Direccion_Responsable = a.Paciente.DireccionResponsable,
                    Telefono_Responsable = a.Paciente.TelefonoResponsable

                });

                var pacientes = await query.ToListAsync();

                if(pacientes.Count == 0)
                {
                    return NotFound("No se han encontrado pacientes para la NoHistoriaClinica dada.");
                }

                return pacientes;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ha ocurrido un error al procesar la solicitud");
            }
            

        }

        [HttpGet("pacientes")]
        public async Task<List<PacienteDTO>> GetAllPatients(int pageNumber = 1, int pageSize = 50)
        {
            var pacientes = await _context.Pacientes.OrderByDescending(p => p.Codigo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var PacienteDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            return PacienteDto;

        }
    }
}
