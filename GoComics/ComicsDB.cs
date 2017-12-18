using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GoComics
{
    internal enum ComicsQueries
    {
        SelectComics,
        InsertComic,
        UpdateComic,
        DeleteComic
    }

    class ComicsDb
    {
        //Select statement
        public List<Comics> Select(int? idComic)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                //Create a list to store the result
                List<Comics> list = new List<Comics>();

                //Open connection
                if (dbConnect.OpenConnection())
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        Connection = dbConnect.SqlConnection,
                        CommandText = ComicsQueries.SelectComics.ToString(),
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("idC", idComic);

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        Comics comics = new Comics
                        {
                            IdComic = Convert.ToInt32(dataReader["IdComic"].ToString()),
                            Name = dataReader["Name"].ToString(),
                            UrlComic = dataReader["UrlComic"].ToString(),
                            Description = dataReader["Description"].ToString()
                        };

                        list.Add(comics);
                    }

                    //close Data Reader
                    dataReader.Close();

                    //close Connection
                    dbConnect.CloseConnection();

                }
                return list;
            }
          

           
        }

        //Insert statement
        public bool Insert(Comics comics)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                try
                {
                    //open connection
                    if (dbConnect.OpenConnection())
                    {
                        //create command and assign the query and connection from the constructor
                        MySqlCommand cmd = new MySqlCommand()
                        {
                            Connection =  dbConnect.SqlConnection,
                            CommandText = ComicsQueries.InsertComic.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@urlC", comics.UrlComic);
                        cmd.Parameters.AddWithValue("@n", comics.Name);
                        cmd.Parameters.AddWithValue("@D", comics.Description);

                        //Execute command
                        return cmd.ExecuteNonQuery()>0;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    //close connection
                    dbConnect.CloseConnection();
                }
            }
            return false;
        }

        public bool Update(Comics comics)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                try
                {
                    //open connection
                    if (dbConnect.OpenConnection())
                    {
                        //create command and assign the query and connection from the constructor
                        MySqlCommand cmd = new MySqlCommand()
                        {
                            Connection = dbConnect.SqlConnection,
                            CommandText = ComicsQueries.UpdateComic.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@IdC", comics.IdComic);
                        cmd.Parameters.AddWithValue("@url", comics.UrlComic);
                        cmd.Parameters.AddWithValue("@N", comics.Name);
                        cmd.Parameters.AddWithValue("@D", comics.Description);

                        //Execute command
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    //close connection
                    dbConnect.CloseConnection();
                }
            }
            return false;
        }

        public bool Delete(int IdC)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                try
                {
                    //open connection
                    if (dbConnect.OpenConnection())
                    {
                        //create command and assign the query and connection from the constructor
                        MySqlCommand cmd = new MySqlCommand()
                        {
                            Connection = dbConnect.SqlConnection,
                            CommandText = ComicsQueries.DeleteComic.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@IdC", IdC);

                        //Execute command
                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    //close connection
                    dbConnect.CloseConnection();
                }
            }
            return false;
        }
    }
}
