using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using MySql.Data.MySqlClient;

namespace GoComics
{
    class DBconnect :IDisposable
    {
        bool _disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        private string _server;
        private string _database;
        private string _uid;
        private string _password;
        private MySqlConnection _sqlConnection;
        
        public MySqlConnection SqlConnection => _sqlConnection;
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string Uid { get; private set; }
        public string Password { get; private set; }

        //Constructor
        public DBconnect()
        {
            Initialize();
        }

        public DBconnect(string server, string database, string uid, string password)
        {
            Server = server;
            Database = database;
            Uid = uid;
            Password = password;

            Initialize();

        }

       //Initialize values
        private void Initialize()
        {
            if (string.IsNullOrEmpty(Server))
                Server = "localhost";

            if (string.IsNullOrEmpty(Database))
                Database = "gocomics";

            if (string.IsNullOrEmpty(Uid))
                Uid = "root";

            if (string.IsNullOrEmpty(Password))
                Password = "515667";

            string connectionString = $"SERVER={Server}; DATABASE={Database}; UID={Uid}; PASSWORD={Password};";

            _sqlConnection = new MySqlConnection(connectionString);

        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                SqlConnection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                SqlConnection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                handle?.Dispose();

                SqlConnection.Dispose();

            }

            _disposed = true;
        }
        ~DBconnect()
        {
            Dispose(false);
        }
    }
}
