using System;
using System.Collections.Generic;

namespace DatosPacientes.Models
{
    public partial class Paciente
    {
        /// <summary>
        /// Almacena el identificador único de paciente
        /// </summary>
        public int Codigo { get; set; }
        /// <summary>
        /// Hacer referencia a la entidad Persona
        /// </summary>
        public int Persona { get; set; }
        /// <summary>
        /// Almacena el numero de registro de historia clínica de un paciente
        /// </summary>
        public string? NoHistoriaClinica { get; set; }
        /// <summary>
        /// Almacena el Número de admisión de un paciente (obsoleto)
        /// </summary>
        public string? NoAdmision { get; set; }
        /// <summary>
        /// Almacena la religión del paciente (Evangelico, Catalico, Mormon, Ateo, Testigo de Jehova, etc.)
        /// </summary>
        public int? Religion { get; set; }
        /// <summary>
        /// Almacena la ocupacion de un paciente (Agricultor, Amada de Casa, etc.)
        /// </summary>
        public int? Ocupacion { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Escolaridad
        /// </summary>
        public int? Escolaridad { get; set; }
        /// <summary>
        /// Almacena si un paciente tiene seguro social o no.
        /// </summary>
        public bool? SeguroSocial { get; set; }
        /// <summary>
        /// Almacena el número de Seguro Social del Paciente
        /// </summary>
        public string? NoSeguroSocial { get; set; }
        /// <summary>
        /// Almacena el nombre completo del paciente
        /// </summary>
        public string NombreCompleto { get; set; } = null!;
        /// <summary>
        /// Almacena el nombre completo de la esposa o esposo
        /// </summary>
        public string? NombreConyuge { get; set; }
        /// <summary>
        /// Almacena el lugar de trabajo del paciente
        /// </summary>
        public string? LugarTrabajo { get; set; }
        /// <summary>
        /// Almacena el nombre completo del padre del paciente
        /// </summary>
        public string? NombrePadre { get; set; }
        /// <summary>
        /// Almacena el estado de supervivencia del padre del paciente (Vivo o Muerto)
        /// </summary>
        public bool? SupervivenciaPadre { get; set; }
        /// <summary>
        /// Almacena el nombre completo de la madre del paciente
        /// </summary>
        public string? NombreMadre { get; set; }
        /// <summary>
        /// Almacena el estado de supervivencia de la madre del paciente (Viva o Muerta)
        /// </summary>
        public bool? SupervivenciaMadre { get; set; }
        /// <summary>
        /// Almacena el nombre completo del responsable del paciente
        /// </summary>
        public string? NombreResponsable { get; set; }
        /// <summary>
        /// Almacena la dirección completa del responsable del paciente
        /// </summary>
        public string? DireccionResponsable { get; set; }
        /// <summary>
        /// Almacena el numero de telefono del responsable del paciente
        /// </summary>
        public string? TelefonoResponsable { get; set; }
        /// <summary>
        /// Contiene la huella digital del Paciente
        /// </summary>
        public byte[]? Template { get; set; }
        /// <summary>
        /// Contiene el lugar de nacimiento del Paciente
        /// </summary>
        public string? LugarNacimiento { get; set; }
        /// <summary>
        /// Contiene la Nacionalidad del Paciente, hace referencia a la entidad Nacionalidad
        /// </summary>
        public int? Nacionalidad { get; set; }
        /// <summary>
        /// Contiene la fotografía del Paciente
        /// </summary>
        public byte[]? Fotografia { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Grupo_Etnico
        /// </summary>
        public int? GrupoEtnico { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Discapacidad
        /// </summary>
        public int? Discapacidad { get; set; }
        /// <summary>
        /// Hace referencia a la entidad Estado_Migratorio
        /// </summary>
        public int? EstadoMigratorio { get; set; }
        /// <summary>
        /// hace referencia a si el paciente sale vivo o muerto
        /// </summary>
        public bool? SupervivenciaPaciente { get; set; }
        /// <summary>
        /// Contiene el nombre de un cliente
        /// </summary>
        public int? Cliente { get; set; }
        /// <summary>
        /// Almacena el Grupo_Sanguineo del paciente
        /// </summary>
        public int? GrupoSanguineo { get; set; }
        /// <summary>
        /// Almacena el Registro Unico de Paciente
        /// </summary>
        public string? Rup { get; set; }
        /// <summary>
        /// Almacena la sub división de Grup_Etnico
        /// </summary>
        public int? EtniaMaya { get; set; }
        /// <summary>
        /// Almacena si el paciente es dondante de sangre
        /// </summary>
        public bool? DonanteSangre { get; set; }
        /// <summary>
        /// Almacena si el paciente es donante de Organos
        /// </summary>
        public bool? DonanteOrganos { get; set; }
        /// <summary>
        /// Fecha de Registro del paciente
        /// </summary>
        public DateTime? FechaRegistro { get; set; }
        /// <summary>
        /// Fuente de Registro del Paciente
        /// </summary>
        public int? FuenteRegistro { get; set; }
        /// <summary>
        /// Almacena Datos de Sigsa
        /// </summary>
        public string? Idas { get; set; }
        /// <summary>
        /// Almacena Datos de Sigsa
        /// </summary>
        public string? Idds { get; set; }
        /// <summary>
        /// Almacena Datos de Sigsa
        /// </summary>
        public string? Idts { get; set; }
        /// <summary>
        /// Almacena si el paciente es XXX
        /// </summary>
        public bool? NoIdentificado { get; set; }
        /// <summary>
        /// Almacena las caracteristicas de un paciente no identificado
        /// </summary>
        public string? CaracteristicasPacienteXxx { get; set; }
        /// <summary>
        /// Almacena si un paciente es un reo o no
        /// </summary>
        public bool? Reo { get; set; }
        /// <summary>
        /// referencia a la tabla de Orientacion sexual
        /// </summary>
        public int? OrientacionSexual { get; set; }
        /// <summary>
        /// referencia a la tabla de Profesion sexual
        /// </summary>
        public int? ProfesionSexual { get; set; }
        /// <summary>
        /// Describe si existe o no archivo físico del paciente
        /// </summary>
        public bool? ArchivoFisico { get; set; }
        /// <summary>
        /// Institucion o persona que la refirio al centro asistencial
        /// </summary>
        public string? Referente { get; set; }
        /// <summary>
        /// Medio por el cual se entero de los servicios prestados por el centro asistencial
        /// </summary>
        public int? Publicidad { get; set; }

        public virtual Discapacidad? DiscapacidadNavigation { get; set; }
        public virtual Escolaridad? EscolaridadNavigation { get; set; }
        public virtual EstadoMigratorio? EstadoMigratorioNavigation { get; set; }
        public virtual EtniaMaya? EtniaMayaNavigation { get; set; }
        public virtual FuenteRegistro? FuenteRegistroNavigation { get; set; }
        public virtual GrupoEtnico? GrupoEtnicoNavigation { get; set; }
        public virtual GrupoSanguineo? GrupoSanguineoNavigation { get; set; }
        public virtual Nacionalidad? NacionalidadNavigation { get; set; }
        public virtual Ocupacion? OcupacionNavigation { get; set; }
        public virtual Persona PersonaNavigation { get; set; } = null!;
        public virtual Religion? ReligionNavigation { get; set; }
    }
}
