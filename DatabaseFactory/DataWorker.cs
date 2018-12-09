using System;

namespace DatabaseFactory
{
    public class DataWorker
    {
        static DataWorker()
        {
            try
            {
                Database = DatabaseFactory.CreateDatabase();
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }

        protected static Database Database { get; } = null;
    }
}
