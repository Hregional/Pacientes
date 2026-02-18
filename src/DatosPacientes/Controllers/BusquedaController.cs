using DatosPacientes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using DatosPacientes.Models.SP;
using AutoMapper;
using DatosPacientes.DTOs;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Authorization;

namespace DatosPacientes.Controllers
{
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

        // ─── Método auxiliar: normaliza DPI y recorta espacios ───────────────────
        private static void NormalizarPacientes(List<PacienteCompletoDTO> pacientes)
        {
            pacientes.ForEach(p =>
            {
                p.CodigoRenap = string.IsNullOrEmpty(p.CodigoRenap)
                    ? null
                    : new string(p.CodigoRenap.Where(char.IsDigit).ToArray());

                p.NoHistoriaClinica = p.NoHistoriaClinica?.Trim();
            });
        }

        // ─── Método auxiliar: obtiene estado requerido desde parametros_generales ─
        private async Task<int> GetEstadoRequerido()
        {
            int? paramVerEliminados = null;
            var conn = _context.Database.GetDbConnection();

            // ✅ Solo abre si no está ya abierta
            if (conn.State != System.Data.ConnectionState.Open)
                await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT valor FROM parametros_generales WHERE nombre = 'VerPacientesEliminados'";
            var result = await cmd.ExecuteScalarAsync();
            if (result != null && result != DBNull.Value)
                paramVerEliminados = Convert.ToInt32(result);

            return (paramVerEliminados == 1) ? 1 : 0;
        }

        // ─── Búsqueda por Historia Clínica ────────────────────────────────────────
        [HttpGet("paciente/{NoHistoriaClinica}")]
        public async Task<ActionResult<List<PacienteCompletoDTO>>> GetPersonas(string NoHistoriaClinica)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NoHistoriaClinica))
                    return BadRequest("NoHistoriaClinica no puede ser nulo o vacío");

                int estadoRequerido = await GetEstadoRequerido();

                var query = _context.Pacientes
                    .Join(_context.Personas,
                        p => p.Persona,
                        per => per.Codigo,
                        (p, per) => new { Paciente = p, Persona = per })
                    .Where(a =>
                        a.Persona.Estado == estadoRequerido &&
                        (NoHistoriaClinica == "-1" ||
                         a.Paciente.NoHistoriaClinica.Contains(NoHistoriaClinica)))
                    .OrderBy(a => a.Persona.Nombre1)
                    .Select(a => new PacienteCompletoDTO()
                    {
                        Codigo = a.Paciente.Codigo,
                        Persona = a.Paciente.Persona,
                        Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                        Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                        NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                        CodigoRenap = a.Persona.CodigoRenap, // ✅ se normaliza después
                        FechaNacimiento = a.Persona.FechaNacimiento,
                        Edad = calculateAge(a.Persona.FechaNacimiento ?? DateTime.Now),
                        Sexo = a.Persona.Sexo,
                        NombrePadre = a.Paciente.NombrePadre,
                        NombreMadre = a.Paciente.NombreMadre,
                        LugarNacimiento = a.Paciente.LugarNacimiento,
                        Archivo_Fisico = a.Paciente.ArchivoFisico,
                        Nombre_Resposable = a.Paciente.NombreResponsable,
                        Direccion_Responsable = a.Paciente.DireccionResponsable,
                        Telefono_Responsable = a.Paciente.TelefonoResponsable,
                        Direccion_Paciente = a.Persona.DireccionNavigation != null ? a.Persona.DireccionNavigation.Descripcion : "",
                        Direccion_Paciente_Completa = a.Persona.DireccionNavigation != null
                            ? (a.Persona.DireccionNavigation.Descripcion ?? "") +
                              (a.Persona.DireccionNavigation.MunicipioNavigation != null ? ", " + a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "") +
                              (a.Persona.DireccionNavigation.DepartamentoNavigation != null ? ", " + a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : "")
                            : ""
                    });

                var pacientes = await query.ToListAsync();
                NormalizarPacientes(pacientes); // ✅ normaliza DPI

                if (pacientes.Count == 0)
                    return NotFound("No se han encontrado pacientes para la Historia Clínica dada.");

                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error al procesar la búsqueda por Historia Clínica: {ex.Message}");
            }
        }

        // ─── Búsqueda por Fecha de Nacimiento ─────────────────────────────────────
        [HttpGet("paciente/avanzado/{FechaNacimiento}")]
        public async Task<ActionResult<List<PacienteCompletoDTO>>> GetPacientesByBirthday(string FechaNacimiento)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FechaNacimiento))
                    return BadRequest("FechaNacimiento no puede ser nulo o vacío");

                // ✅ Parseo seguro
                if (!DateTime.TryParse(FechaNacimiento, out DateTime fechaParsed))
                    return BadRequest("Formato de fecha inválido. Use YYYY-MM-DD.");

                int estadoRequerido = await GetEstadoRequerido();

                var query = _context.Pacientes
                    .Join(_context.Personas,
                        p => p.Persona,
                        per => per.Codigo,
                        (p, per) => new { Paciente = p, Persona = per })
                    .Where(a =>
                        a.Persona.Estado == estadoRequerido &&
                        a.Persona.FechaNacimiento == fechaParsed)
                    .OrderBy(a => a.Persona.Nombre1)
                    .Select(a => new PacienteCompletoDTO()
                    {
                        Codigo = a.Paciente.Codigo,
                        Persona = a.Paciente.Persona,
                        Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                        Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                        NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                        CodigoRenap = a.Persona.CodigoRenap,
                        FechaNacimiento = a.Persona.FechaNacimiento,
                        Edad = calculateAge(a.Persona.FechaNacimiento ?? DateTime.Now),
                        Sexo = a.Persona.Sexo,
                        NombrePadre = a.Paciente.NombrePadre,
                        NombreMadre = a.Paciente.NombreMadre,
                        LugarNacimiento = a.Paciente.LugarNacimiento,
                        Archivo_Fisico = a.Paciente.ArchivoFisico,
                        Nombre_Resposable = a.Paciente.NombreResponsable,
                        Direccion_Responsable = a.Paciente.DireccionResponsable,
                        Telefono_Responsable = a.Paciente.TelefonoResponsable,
                        Direccion_Paciente = a.Persona.DireccionNavigation != null ? a.Persona.DireccionNavigation.Descripcion : "",
                        Direccion_Paciente_Completa = a.Persona.DireccionNavigation != null
                            ? (a.Persona.DireccionNavigation.Descripcion ?? "") +
                              (a.Persona.DireccionNavigation.MunicipioNavigation != null ? ", " + a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "") +
                              (a.Persona.DireccionNavigation.DepartamentoNavigation != null ? ", " + a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : "")
                            : ""
                    });

                var pacientes = await query.ToListAsync();
                NormalizarPacientes(pacientes);

                if (pacientes.Count == 0)
                    return NotFound("No se han encontrado pacientes para la fecha dada.");

                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error al procesar la búsqueda por fecha: {ex.Message}");
            }
        }

        // ─── Búsqueda por Nombre ──────────────────────────────────────────────────
        [HttpGet("paciente/nombre")]
        public async Task<ActionResult<List<PacienteCompletoDTO>>> GetPacientesByName(
            string PrimerNombre,
            string PrimerApellido,
            string? SegundoNombre = null,
            string? SegundoApellido = null,
            string? TercerApellido = null)
        {
            PrimerNombre = PrimerNombre.Trim().ToUpper();
            PrimerApellido = PrimerApellido.Trim().ToUpper();
            SegundoNombre = SegundoNombre?.Trim().ToUpper();
            SegundoApellido = SegundoApellido?.Trim().ToUpper();
            TercerApellido = TercerApellido?.Trim().ToUpper();

            try
            {
                int estadoRequerido = await GetEstadoRequerido();

                var query = _context.Pacientes
                    .Join(_context.Personas,
                        p => p.Persona,
                        per => per.Codigo,
                        (p, per) => new { Paciente = p, Persona = per })
                    .Where(a =>
                        a.Persona.Estado == estadoRequerido &&
                        a.Persona.Nombre1.Contains(PrimerNombre) &&
                        a.Persona.Apellido1.Contains(PrimerApellido) &&
                        // ✅ Protección contra null en Nombre2 y Apellido2
                        (SegundoNombre == null || (a.Persona.Nombre2 != null && a.Persona.Nombre2.Contains(SegundoNombre))) &&
                        (SegundoApellido == null || (a.Persona.Apellido2 != null && a.Persona.Apellido2.Contains(SegundoApellido)))
                    )
                    .OrderBy(a => a.Persona.Nombre1)
                    .Select(a => new PacienteCompletoDTO()
                    {
                        Codigo = a.Paciente.Codigo,
                        Persona = a.Paciente.Persona,
                        Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                        Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                        NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                        CodigoRenap = a.Persona.CodigoRenap, // ✅ faltaba en el original
                        FechaNacimiento = a.Persona.FechaNacimiento,
                        Edad = calculateAge(a.Persona.FechaNacimiento ?? DateTime.Now),
                        Sexo = a.Persona.Sexo,
                        NombrePadre = a.Paciente.NombrePadre,
                        NombreMadre = a.Paciente.NombreMadre,
                        LugarNacimiento = a.Paciente.LugarNacimiento,
                        Archivo_Fisico = a.Paciente.ArchivoFisico,
                        Nombre_Resposable = a.Paciente.NombreResponsable,
                        Direccion_Responsable = a.Paciente.DireccionResponsable,
                        Telefono_Responsable = a.Paciente.TelefonoResponsable,
                        Direccion_Paciente = a.Persona.DireccionNavigation != null ? a.Persona.DireccionNavigation.Descripcion : "",
                        Direccion_Paciente_Completa = a.Persona.DireccionNavigation != null
                            ? (a.Persona.DireccionNavigation.Descripcion ?? "") +
                              (a.Persona.DireccionNavigation.MunicipioNavigation != null ? ", " + a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "") +
                              (a.Persona.DireccionNavigation.DepartamentoNavigation != null ? ", " + a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : "")
                            : ""
                    });

                var pacientes = await query.ToListAsync();
                NormalizarPacientes(pacientes);

                if (pacientes.Count == 0)
                    return NotFound("No se han encontrado pacientes con el nombre dado.");

                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error al procesar la búsqueda por nombre: {ex.Message}");
            }
        }

        // ─── Búsqueda por DPI ─────────────────────────────────────────────────────
        [HttpGet("paciente/dpi/{cui}")]
        public async Task<ActionResult<List<PacienteCompletoDTO>>> GetPacientesByDpi(string cui)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cui))
                    return BadRequest("El DPI/CUI no puede estar vacío");

                string cuiLimpio = new string(cui.Where(char.IsDigit).ToArray());
                if (cui == "-1") cuiLimpio = "-1";

                int estadoRequerido = await GetEstadoRequerido(); // ✅ reutiliza método

                var query = _context.Pacientes
                    .Join(_context.Personas,
                        p => p.Persona,
                        per => per.Codigo,
                        (p, per) => new { Paciente = p, Persona = per })
                    .Where(a =>
                        a.Persona.Estado == estadoRequerido &&
                        (cuiLimpio == "-1" ||
                         (a.Persona.CodigoRenap != null && // ✅ protección null
                          a.Persona.CodigoRenap.Replace(" ", "").Replace("-", "").Replace("dpi:", "") == cuiLimpio))
                    )
                    .OrderBy(a => a.Persona.Nombre1)
                    .Select(a => new PacienteCompletoDTO()
                    {
                        Codigo = a.Paciente.Codigo,
                        Persona = a.Paciente.Persona,
                        CodigoRenap = a.Persona.CodigoRenap,
                        Nombres = a.Persona.Nombre1 + " " + (a.Persona.Nombre2 ?? ""),
                        Apellidos = a.Persona.Apellido1 + " " + (a.Persona.Apellido2 ?? ""),
                        NoHistoriaClinica = a.Paciente.NoHistoriaClinica,
                        FechaNacimiento = a.Persona.FechaNacimiento,
                        Edad = calculateAge(a.Persona.FechaNacimiento ?? DateTime.Now),
                        Sexo = a.Persona.Sexo,
                        NombrePadre = a.Paciente.NombrePadre,
                        NombreMadre = a.Paciente.NombreMadre,
                        LugarNacimiento = a.Paciente.LugarNacimiento,
                        Archivo_Fisico = a.Paciente.ArchivoFisico,
                        Nombre_Resposable = a.Paciente.NombreResponsable,
                        Direccion_Responsable = a.Paciente.DireccionResponsable,
                        Telefono_Responsable = a.Paciente.TelefonoResponsable,
                        Direccion_Paciente = a.Persona.DireccionNavigation != null ? a.Persona.DireccionNavigation.Descripcion : "",
                        Direccion_Paciente_Completa = a.Persona.DireccionNavigation != null
                            ? (a.Persona.DireccionNavigation.Descripcion ?? "") +
                              (a.Persona.DireccionNavigation.MunicipioNavigation != null ? ", " + a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "") +
                              (a.Persona.DireccionNavigation.DepartamentoNavigation != null ? ", " + a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : "")
                            : ""
                    });

                var pacientes = await query.ToListAsync();
                NormalizarPacientes(pacientes); // ✅ normaliza DPI

                if (pacientes.Count == 0)
                    return NotFound("No se han encontrado pacientes con el DPI proporcionado.");

                return Ok(pacientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error al procesar la búsqueda por DPI: {ex.Message}");
            }
        }

        // ─── Paginación ───────────────────────────────────────────────────────────
        [HttpGet]
        public async Task<List<PacienteDTO>> GetAllPatients(int pageNumber = 1, int pageSize = 50)
        {
            var pacientes = await _context.Pacientes
                .OrderByDescending(p => p.Codigo)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            var PacienteDto = _mapper.Map<List<PacienteDTO>>(pacientes);

            foreach (var paciente in PacienteDto)
            {
                paciente.PersonasLink = Url.Action("GetPersonas", "Busqueda",
                    new { paciente.NoHistoriaClinica });
            }

            return PacienteDto;
        }

        // ─── Calcular edad ────────────────────────────────────────────────────────
        public static string calculateAge(DateTime birthDate)
        {
            try
            {
                DateTime now = DateTime.Today;
                int ageYears = now.Year - birthDate.Year;
                if (birthDate > now.AddYears(-ageYears)) ageYears--;

                int ageMonths = now.Month - birthDate.Month;
                if (ageMonths < 0) { ageMonths += 12; ageYears--; }

                int ageDays = now.Day - birthDate.Day;
                if (ageDays < 0)
                {
                    ageDays += DateTime.DaysInMonth(now.Year, now.Month);
                    ageMonths--;
                }

                if (ageYears == 1) return $"{ageYears} Año";
                if (ageYears > 1) return $"{ageYears} Años";
                if (ageMonths == 1) return $"{ageMonths} Mes";
                if (ageMonths > 1) return $"{ageMonths} Meses";
                if (ageDays == 1) return $"{ageDays} Día";
                return $"{ageDays} Días";
            }
            catch
            {
                return "Error al calcular la edad";
            }
        }
    }
}