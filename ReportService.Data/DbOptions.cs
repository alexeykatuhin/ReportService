using Microsoft.Data.SqlClient;
using System.Data;

namespace ReportService.Data
{
    public class DbOptions
    {
        private readonly string _connectionString;

        public DbOptions(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}