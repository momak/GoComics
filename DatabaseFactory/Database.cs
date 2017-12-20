using System.Collections.Generic;
using System.Data;

namespace DatabaseFactory
{
    public abstract class Database
    {
        public string ConnectionString;
        #region Abstract Functions
        public abstract IDbConnection CreateConnection();
        public abstract IDbCommand CreateCommand();
        public abstract IDbConnection CreateOpenConnection();
        public abstract IDbCommand CreateCommand(string commandText, IDbConnection connection);
        public abstract IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection);
        public abstract IDataParameter CreateParameter(string parameterName, object parameterValue);
        public abstract IDataParameter AddWithValue<T>(IDbCommand command, string name, T value);
        public abstract IList<IDataParameter> AddWithValue<T>(IDbCommand command, Dictionary<string, object> dictionary);
        #endregion
    }
}
