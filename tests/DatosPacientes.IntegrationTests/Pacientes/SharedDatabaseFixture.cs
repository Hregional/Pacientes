using Bogus;
using DatosPacientes.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatosPacientes.IntegrationTests.Pacientes
{
    public class SharedDatabaseFixture : IDisposable
    {

        private static readonly object _lock = new object();
        private static bool _databaseInitialized;

        private string dbName = "RecepcionV2B";
        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database=RecepcionV2B;Trusted_Connection=True;MultipleActiveResultSets=true";

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(connectionString);

            Seed();

            Connection.Open();
        }
        public DbConnection Connection { get; }

        public void Dispose() => Connection.Dispose();

        public RecepcionV2Context CreateContext(DbTransaction? transaction = null)
        {
            var context = new RecepcionV2Context(new DbContextOptionsBuilder<RecepcionV2Context>().UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        SeedData(context);
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void SeedData(RecepcionV2Context context)
        {

            var personaFaker = new Faker<Persona>()
                .RuleFor(p => p.Codigo, f => f.Random.Number(1, 1000))
                .RuleFor(p => p.Nombre1, f => f.Person.FirstName)
                .RuleFor(p => p.Nombre2, f => f.Person.FirstName)
                .RuleFor(p => p.Apellido1, f => f.Person.LastName)
                .RuleFor(p => p.Apellido2, f => f.Person.LastName)
                .RuleFor(p => p.Apellido3, f => f.Person.LastName)
                .RuleFor(p => p.Telefono1, f => f.Person.Phone)
                .RuleFor(p => p.Telefono2, f => f.Person.Phone)
                .RuleFor(p => p.Movil, f => f.Person.Phone)
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.RazonSocial, f => f.Company.CompanyName())
                .RuleFor(p => p.Sexo, f => f.PickRandom("0", "1"))
                .RuleFor(p => p.FechaNacimiento, f => f.Person.DateOfBirth)
                //.RuleFor(p => p.EdadAnios, f => f.Person.DateOfBirth.HasValue ? f.Random.Number(1, 100) : null)
                //.RuleFor(p => p.EdadMeses, f => f.Person.DateOfBirth.HasValue ? f.Random.Number(1, 12) : null)
                //.RuleFor(p => p.EdadDias, f => f.Person.DateOfBirth.HasValue ? f.Random.Number(1, 31) : null)
                .RuleFor(p => p.EstadoCivil, f => f.Random.Byte(0, 1))
                .RuleFor(p => p.NombreCompleto, (f, p) => $"{p.Nombre1} {p.Nombre2} {p.Apellido1} {p.Apellido2} {p.Apellido3}".Trim())
                .RuleFor(p => p.Observaciones, f => f.Lorem.Sentence())
                .RuleFor(p => p.Estado, f => f.Random.Number(0, 1))
                .RuleFor(p => p.CodigoRenap, f => f.Random.AlphaNumeric(10))
                .RuleFor(p => p.PersonaJuridica, f => f.Random.Bool())
                .RuleFor(p => p.FechaDefuncion, f => f.Random.Bool() ? (DateTime?)f.Person.DateOfBirth.AddDays(f.Random.Number(1, 10000)) : null)
                .RuleFor(p => p.FechaRegistro, f => f.Date.Past());

            // Generar una instancia de persona con valores aleatorios
            var persona = personaFaker.Generate(1);

            

            context.SaveChanges();
        }
    }
}
