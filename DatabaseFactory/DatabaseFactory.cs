using System;
using System.Configuration;
using System.Reflection;

namespace DatabaseFactory
{
    public static class DatabaseFactory
    {
        private static readonly DatabaseFactorySectionHandler SectionHandler = (DatabaseFactorySectionHandler)ConfigurationManager.GetSection("DatabaseFactoryConfiguration");

        public static Database CreateDatabase()
        {
            // Verify a DatabaseFactoryConfiguration line exists in the web.config.
            if (SectionHandler.Name.Length == 0)
            {
                throw new Exception("Database name not defined in DatabaseFactoryConfiguration section of web.config.");
            }
            try
            {
                // Find the class
                Type database = Type.GetType(SectionHandler.Name);
                // Get it's constructor
                ConstructorInfo constructor = database.GetConstructor(new Type[] { });
                // Invoke it's constructor, which returns an instance.
                Database createdObject = (Database)constructor.Invoke(null);
                // Initialize the connection string property for the database.
                createdObject.ConnectionString = SectionHandler.ConnectionString;
                // Pass back the instance as a Database
                return createdObject;
            }
            catch (Exception excep)
            {
                throw new Exception("Error instantiating database " + SectionHandler.Name + ". " + excep.Message);
            }
        }
    }
}
