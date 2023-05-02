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

        private string dbName = "RecepcionV2T";
        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database={0};Trusted_Connection=True;MultipleActiveResultSets=true";

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(connectionString);


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
    
    }
}
