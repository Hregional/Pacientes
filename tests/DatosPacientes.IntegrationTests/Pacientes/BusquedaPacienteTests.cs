using AutoMapper;
using DatosPacientes.Controllers;
using DatosPacientes.DTOs;
using DatosPacientes.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosPacientes.IntegrationTests.Pacientes
{
    public class BusquedaPacienteTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }

        public BusquedaPacienteTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }).CreateMapper();
        }

        [Fact]
        public async Task GetPersonas_ReturnsBadRequest_WhenNoHistoriaClinicaIsNullOrWhiteSpace()
        {

            using (var context = Fixture.CreateContext()) 
            { 
                // Arrange
                var controller = new BusquedaController(context, _mapper);

            string NoHistoriaClinica = null;

                // Act
                var result = await controller.GetPersonas(NoHistoriaClinica);

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
                Assert.Equal("NoHistoriaClinica no puede ser nolo o vacío", badRequestResult.Value);
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
                var result = await controller.GetPersonas("123");

                // Assert
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
                Assert.Equal("No se han encontrado pacientes para la NoHistoriaClinica dada.", notFoundResult.Value);
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
                var result = await controller.GetPersonas("1");

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result.Result);
                var returnValue = Assert.IsType<List<PacienteCompletoDTO>>(okResult.Value);
                Assert.Equal(1, returnValue.Count);
            }
        }



    }
}
