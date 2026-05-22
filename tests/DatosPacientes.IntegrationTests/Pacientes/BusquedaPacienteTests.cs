using MapsterMapper;
using Mapster;
using DatosPacientes.Controllers;
using DatosPacientes.DTOs;
using DatosPacientes.Models;
using DatosPacientes.Mappings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DatosPacientes.IntegrationTests.Pacientes
{
    public class BusquedaPacienteTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }

        public BusquedaPacienteTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;
            var config = new TypeAdapterConfig();
            new MapsterConfig().Register(config);
            _mapper = new Mapper(config);
        }

        [Fact]
        public async Task GetPersonas_ReturnsBadRequest_WhenNoHistoriaClinicaIsNullOrWhiteSpace()
        {

            using (var context = Fixture.CreateContext()) 
            { 
                // Arrange
                var controller = new BusquedaController(context, _mapper);

            string NoHistoriaClinica = null!;

                // Act
                var result = await controller.GetPersonas(NoHistoriaClinica);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("NoHistoriaClinica no puede ser nulo o vacío", badRequestResult.Value);
            }
            
        }

        [Fact]
        public async Task GetPersonas_ReturnsNotFound_WhenNoPacientesMatchNoHistoriaClinica()
        {
            using (var context = Fixture.CreateContext())
            {
                // Arrange
                var controller = new BusquedaController(context, _mapper);

                // Act
                var result = await controller.GetPersonas("123456789");

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("No se han encontrado pacientes para la Historia Clínica dada.", notFoundResult.Value);
            }
        }

        [Fact]
        public async Task GetPersonas_ReturnsOk_WhenPacientesMatchNoHistoriaClinica()
        {
            using (var context = Fixture.CreateContext())
            {
                // Arrange
                var controller = new BusquedaController(context, _mapper);

                // Act
                var result = await controller.GetPersonas("736523");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<PacienteCompletoDTO>>(okResult.Value);
                Assert.Single(returnValue);
            }
        }

        [Fact]
        public async Task GetPacientesByDPI_ReturnsOk_WhenPacientesMatchDPI()
        {
            using (var context = Fixture.CreateContext())
            {
                // Arrange
                var controller = new BusquedaController(context, _mapper);

                // Act
                var result = await controller.GetPacientesByDpi("3593745140801");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<PacienteCompletoDTO>>(okResult.Value);
                Assert.Single(returnValue);
            }
        }

        [Fact]
        public async Task GetPacientesByDPI_ReturnsNotFound_WhenNoPacientesMatchDPI()
        {
            using (var context = Fixture.CreateContext())
            {
                // Arrange
                var controller = new BusquedaController(context, _mapper);

                // Act
                var result = await controller.GetPacientesByDpi("1234567890123");

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("No se han encontrado pacientes con el DPI proporcionado.", notFoundResult.Value);
            }
        }
    }
}
