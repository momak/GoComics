using System;

namespace DatabaseFactory
{
    public class DataWorker
    {
        private static readonly Database _database = null;
        static DataWorker()
        {
            try
            {
                _database = DatabaseFactory.CreateDatabase();
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }
        public static Database database => _database;
    }
}
