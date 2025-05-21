using Npgsql;
using System.Data;

namespace Ripple.Utility
{
    public class DatabaseConnectionProvider : IDisposable
    {
        private readonly NpgsqlConnection _connection;
        private NpgsqlTransaction? _transaction;

        public DatabaseConnectionProvider(IConfiguration configuration)
        {
            _connection = new NpgsqlConnection(GetSqlConnectionString(configuration));
            _connection.Open();
        }

        public IDbConnection GetConnection()
        {
            return _connection;
        }

        public void SetTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        public void CompleteTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Dispose()
        {
            //RollbackTransaction();
            _connection.Close();
            _connection.Dispose();
        }

        private static string GetSqlConnectionString(IConfiguration config)
        {
            return $"Host={config["DatabaseConnection_Server"]};Username={config["DatabaseConnection_Username"]};Password={config["DatabaseConnection_Password"]};Database={config["DatabaseConnection_Database"]}";
        }
    }
}
