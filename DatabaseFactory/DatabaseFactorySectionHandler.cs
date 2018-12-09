using System;
using System.Configuration;

namespace DatabaseFactory
{
    public sealed class DatabaseFactorySectionHandler : ConfigurationSection
    {
        [ConfigurationProperty("Name")]
        public string Name => (string)base["Name"];

        [ConfigurationProperty("ConnectionStringName")]
        private string ConnectionStringName => (string)base["ConnectionStringName"];

        public string ConnectionString
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
                }
                catch (Exception excep)
                {
                    throw new Exception("Connection string " + ConnectionStringName + " was not found in config file. " + excep.Message);
                }
            }
        }
    }
}
