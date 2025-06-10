using calculadora_sqia.Application.Ports.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System.Data;

namespace calculadora_sqia.SQLServer
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
