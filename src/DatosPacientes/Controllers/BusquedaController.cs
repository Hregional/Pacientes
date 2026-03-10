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
    [Authorize]
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
        public static void NormalizarPacientes(List<PacienteCompletoDTO> pacientes)
        {
            pacientes.ForEach(p =>
            {
                p.CodigoRenap = string.IsNullOrEmpty(p.CodigoRenap)
                    ? null
                    : new string(p.CodigoRenap.Where(char.IsDigit).ToArray());

                p.NoHistoriaClinica = p.NoHistoriaClinica?.Trim();
                p.NombrePadre = p.NombrePadre?.Trim();
                p.NombreMadre = p.NombreMadre?.Trim();
                p.Nombre_Resposable = p.Nombre_Resposable?.Trim();
                p.Direccion_Responsable = p.Direccion_Responsable?.Trim();

                if (p.DireccionPaciente != null)
                {
                    p.DireccionPaciente.Descripcion = p.DireccionPaciente.Descripcion?.Trim();
                    p.DireccionPaciente.Municipio = p.DireccionPaciente.Municipio?.Trim();
                    p.DireccionPaciente.Departamento = p.DireccionPaciente.Departamento?.Trim();
                }
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
                        Nombres = (a.Persona.Nombre1 ?? "").Trim() + " " + (a.Persona.Nombre2 ?? "").Trim(),
                        Apellidos = (a.Persona.Apellido1 ?? "").Trim() + " " + (a.Persona.Apellido2 ?? "").Trim(),
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

                        DireccionPaciente = a.Persona.DireccionNavigation != null ? new DireccionPacienteDTO
                        {
                            Descripcion = (a.Persona.DireccionNavigation.Descripcion ?? "").Trim() +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Colonia) ? "" : ", Colonia " + a.Persona.DireccionNavigation.Colonia.Trim()) +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Zona) ? "" : ", Zona " + a.Persona.DireccionNavigation.Zona.Trim()),
                            Municipio = a.Persona.DireccionNavigation.MunicipioNavigation != null ? a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "",
                            Departamento = a.Persona.DireccionNavigation.DepartamentoNavigation != null ? a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : ""
                        } : null
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
                        Nombres = (a.Persona.Nombre1 ?? "").Trim() + " " + (a.Persona.Nombre2 ?? "").Trim(),
                        Apellidos = (a.Persona.Apellido1 ?? "").Trim() + " " + (a.Persona.Apellido2 ?? "").Trim(),
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
                        DireccionPaciente = a.Persona.DireccionNavigation != null ? new DireccionPacienteDTO
                        {
                            Descripcion = (a.Persona.DireccionNavigation.Descripcion ?? "").Trim() +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Colonia) ? "" : ", Colonia " + a.Persona.DireccionNavigation.Colonia.Trim()) +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Zona) ? "" : ", Zona " + a.Persona.DireccionNavigation.Zona.Trim()),
                            Municipio = a.Persona.DireccionNavigation.MunicipioNavigation != null ? a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "",
                            Departamento = a.Persona.DireccionNavigation.DepartamentoNavigation != null ? a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : ""
                        } : null
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
            PrimerNombre = (PrimerNombre ?? "").Trim();
            PrimerApellido = (PrimerApellido ?? "").Trim();
            SegundoNombre = (SegundoNombre ?? "").Trim();
            SegundoApellido = (SegundoApellido ?? "").Trim();
            TercerApellido = (TercerApellido ?? "").Trim();

            try
            {
                var query = _context.Pacientes
                    .Join(_context.Personas,
                        p => p.Persona,
                        per => per.Codigo,
                        (p, per) => new { Paciente = p, Persona = per })
                    .Where(a =>
                        a.Persona.Estado == 0 &&

    // PrimerNombre - búsqueda sin acento (CI_AI)
    (EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Nombre1, "SQL_Latin1_General_CP1_CI_AI"),
        $"%{PrimerNombre}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Nombre2 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{PrimerNombre}%")) &&

    (SegundoNombre == "" ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Nombre1, "SQL_Latin1_General_CP1_CI_AI"),
        $"%{SegundoNombre}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Nombre2 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{SegundoNombre}%")) &&

    // PrimerApellido - búsqueda sin acento
    (EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido1, "SQL_Latin1_General_CP1_CI_AI"),
        $"%{PrimerApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido2 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{PrimerApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido3 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{PrimerApellido}%")) &&

    (SegundoApellido == "" ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido1, "SQL_Latin1_General_CP1_CI_AI"),
        $"%{SegundoApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido2 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{SegundoApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido3 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{SegundoApellido}%")) &&

    (TercerApellido == "" ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido1, "SQL_Latin1_General_CP1_CI_AI"),
        $"%{TercerApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido2 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{TercerApellido}%") ||
     EF.Functions.Like(
        EF.Functions.Collate(a.Persona.Apellido3 ?? "", "SQL_Latin1_General_CP1_CI_AI"),
        $"%{TercerApellido}%"))
                    )
                    .OrderBy(a => a.Persona.Nombre1)
                    .Select(a => new PacienteCompletoDTO()
                    {
                        Codigo = a.Paciente.Codigo,
                        Persona = a.Paciente.Persona,
                        Nombres = (a.Persona.Nombre1 ?? "").Trim() + " " + (a.Persona.Nombre2 ?? "").Trim(),
                        Apellidos = (a.Persona.Apellido1 ?? "").Trim() + " " + (a.Persona.Apellido2 ?? "").Trim(),
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
                        DireccionPaciente = a.Persona.DireccionNavigation != null ? new DireccionPacienteDTO
                        {
                            Descripcion = (a.Persona.DireccionNavigation.Descripcion ?? "").Trim() +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Colonia) ? "" : ", Colonia " + a.Persona.DireccionNavigation.Colonia.Trim()) +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Zona) ? "" : ", Zona " + a.Persona.DireccionNavigation.Zona.Trim()),
                            Municipio = a.Persona.DireccionNavigation.MunicipioNavigation != null ? a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "",
                            Departamento = a.Persona.DireccionNavigation.DepartamentoNavigation != null ? a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : ""
                        } : null
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
                        Nombres = (a.Persona.Nombre1 ?? "").Trim() + " " + (a.Persona.Nombre2 ?? "").Trim(),
                        Apellidos = (a.Persona.Apellido1 ?? "").Trim() + " " + (a.Persona.Apellido2 ?? "").Trim(),
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
                        DireccionPaciente = a.Persona.DireccionNavigation != null ? new DireccionPacienteDTO
                        {
                            Descripcion = (a.Persona.DireccionNavigation.Descripcion ?? "").Trim() +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Colonia) ? "" : ", Colonia " + a.Persona.DireccionNavigation.Colonia.Trim()) +
                                          (string.IsNullOrEmpty(a.Persona.DireccionNavigation.Zona) ? "" : ", Zona " + a.Persona.DireccionNavigation.Zona.Trim()),
                            Municipio = a.Persona.DireccionNavigation.MunicipioNavigation != null ? a.Persona.DireccionNavigation.MunicipioNavigation.Nombre : "",
                            Departamento = a.Persona.DireccionNavigation.DepartamentoNavigation != null ? a.Persona.DireccionNavigation.DepartamentoNavigation.Nombre : ""
                        } : null
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

            NormalizarPacienteDTOs(PacienteDto);

            foreach (var paciente in PacienteDto)
            {
                paciente.PersonasLink = Url.Action("GetPersonas", "Busqueda",
                    new { paciente.NoHistoriaClinica });
            }

            return PacienteDto;
        }

        public static void NormalizarPacienteDTOs(List<PacienteDTO> pacientes)
        {
            pacientes.ForEach(p =>
            {
                p.NoHistoriaClinica = p.NoHistoriaClinica?.Trim();
                p.NombrePadre = p.NombrePadre?.Trim();
                p.NombreMadre = p.NombreMadre?.Trim();
                p.NombreCompleto = p.NombreCompleto?.Trim();
                p.LugarNacimiento = p.LugarNacimiento?.Trim();
            });
        }

        // ─── Calcular edad ────────────────────────────────────────────────────────
        public static string calculateAge(DateTime birthDate)
        {
            try
            {
                DateTime now = DateTime.Today;

                int years = now.Year - birthDate.Year;
                int months = now.Month - birthDate.Month;
                int days = now.Day - birthDate.Day;

                if (days < 0)
                {
                    months--;
                    // Obtener los días del mes anterior para sumar al remanente de días
                    DateTime prevMonth = now.AddMonths(-1);
                    days += DateTime.DaysInMonth(prevMonth.Year, prevMonth.Month);
                }

                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                if (years > 0)
                {
                    return years == 1 ? "1 Año" : $"{years} Años";
                }

                if (months > 0)
                {
                    return months == 1 ? "1 Mes" : $"{months} Meses";
                }

                return days == 1 ? "1 Día" : $"{days} Días";
            }
            catch
            {
                return "Error al calcular la edad";
            }
        }
    }
}
