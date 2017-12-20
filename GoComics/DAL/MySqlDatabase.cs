using System.Collections.Generic;
using System.Data;
using System.Linq;
using DatabaseFactory;
using MySql.Data.MySqlClient;

namespace GoComics.DAL
{
    class MySqlDatabase : Database
    {
        public override IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public override IDbCommand CreateCommand()
        {
            return new MySqlCommand();
        }

        public override IDbConnection CreateOpenConnection()
        {
            MySqlConnection connection = (MySqlConnection)CreateConnection();

            connection.Open();

            return connection;
        }

        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            MySqlCommand command = (MySqlCommand)CreateCommand();
            command.CommandText = commandText;
            command.Connection = (MySqlConnection)connection;
            command.CommandType = CommandType.Text;
            return command;
        }

        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            MySqlCommand command = (MySqlCommand)CreateCommand();
            command.CommandText = procName;
            command.Connection = (MySqlConnection)connection;
            command.CommandType = CommandType.StoredProcedure;
            return command;
        }

        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new MySqlParameter(parameterName, parameterValue);
        }
        public override IDataParameter AddWithValue<T>(IDbCommand command, string name, T value)
        {
            IDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            return parameter;
        }

        public override IList<IDataParameter> AddWithValue<T>(IDbCommand command, Dictionary<string, object> dictionary)
        {
            return dictionary.Select(d => AddWithValue(command, d.Key, d.Value)).ToList();
        }
    }
}
