using Mapster;
using MapsterMapper;
using DatosPacientes.Mappings;
using DatosPacientes.Models;
using DatosPacientes.DTOs;
using System;
using Xunit;

namespace DatosPacientes.UnitTests.Mappings
{
    public class MappingTests
    {
        private readonly TypeAdapterConfig _config;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _config = new TypeAdapterConfig();
            new MapsterConfig().Register(_config);
            _mapper = new Mapper(_config);
        }

        [Fact]
        public void ShouldMapPacienteToPacienteDTO()
        {
            var source = new Paciente
            {
                Codigo = 1,
                NombreCompleto = "Test User",
                NoHistoriaClinica = "12345"
            };

            var result = _mapper.Map<PacienteDTO>(source);

            Assert.Equal(source.Codigo, result.Codigo);
            Assert.Equal(source.NombreCompleto, result.NombreCompleto);
            Assert.Equal(source.NoHistoriaClinica, result.NoHistoriaClinica);
        }
    }
}
