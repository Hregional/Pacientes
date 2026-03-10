using DatosPacientes.Controllers;
using DatosPacientes.DTOs;
using DatosPacientes.Models.SP;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatosPacientes.UnitTests
{
    public class NormalizacionTests
    {
        [Fact]
        public void NormalizarPacientes_DebeRecortarEspaciosEnBlanco()
        {
            // Arrange
            var pacientes = new List<PacienteCompletoDTO>
            {
                new PacienteCompletoDTO
                {
                    NoHistoriaClinica = "  12345  ",
                    NombrePadre = "Juan Perez  ",
                    NombreMadre = " Maria Garcia ",
                    Nombre_Resposable = "Jose Responsable   ",
                    Direccion_Responsable = "Calle 1, Zona 1  ",
                    DireccionPaciente = new DireccionPacienteDTO
                    {
                        Descripcion = "  Avenida Central  ",
                        Municipio = " Guatemala ",
                        Departamento = " Guatemala  "
                    }
                }
            };

            // Act
            BusquedaController.NormalizarPacientes(pacientes);

            // Assert
            var p = pacientes[0];
            Assert.Equal("12345", p.NoHistoriaClinica);
            Assert.Equal("Juan Perez", p.NombrePadre);
            Assert.Equal("Maria Garcia", p.NombreMadre);
            Assert.Equal("Jose Responsable", p.Nombre_Resposable);
            Assert.Equal("Calle 1, Zona 1", p.Direccion_Responsable);
            Assert.Equal("Avenida Central", p.DireccionPaciente.Descripcion);
            Assert.Equal("Guatemala", p.DireccionPaciente.Municipio);
            Assert.Equal("Guatemala", p.DireccionPaciente.Departamento);
        }

        [Fact]
        public void NormalizarPacienteDTOs_DebeRecortarEspaciosEnBlanco()
        {
            // Arrange
            var pacientes = new List<PacienteDTO>
            {
                new PacienteDTO
                {
                    NoHistoriaClinica = "  REG-123  ",
                    NombrePadre = "  Padre  ",
                    NombreMadre = "  Madre  ",
                    NombreCompleto = "  Completo  ",
                    LugarNacimiento = "  Lugar  "
                }
            };

            // Act
            BusquedaController.NormalizarPacienteDTOs(pacientes);

            // Assert
            var p = pacientes[0];
            Assert.Equal("REG-123", p.NoHistoriaClinica);
            Assert.Equal("Padre", p.NombrePadre);
            Assert.Equal("Madre", p.NombreMadre);
            Assert.Equal("Completo", p.NombreCompleto);
            Assert.Equal("Lugar", p.LugarNacimiento);
        }

        [Fact]
        public void NormalizarLegacy_DebeRecortarEspaciosEnBlanco()
        {
            // Arrange
            var pacientes = new List<PacienteSeleccionarCatalogo>
            {
                new PacienteSeleccionarCatalogo
                {
                    Padre = "  Padre  ",
                    Madre = "  Madre  ",
                    Nombres = "  Nombres  ",
                    Apellidos = "  Apellidos  ",
                    Historia_Clinica = "  HC  ",
                    Procedencia = "  Proc  "
                }
            };

            // Act
            BusquedaLegacyController.NormalizarLegacy(pacientes);

            // Assert
            var p = pacientes[0];
            Assert.Equal("Padre", p.Padre);
            Assert.Equal("Madre", p.Madre);
            Assert.Equal("Nombres", p.Nombres);
            Assert.Equal("Apellidos", p.Apellidos);
            Assert.Equal("HC", p.Historia_Clinica);
            Assert.Equal("Proc", p.Procedencia);
        }
    }
}
