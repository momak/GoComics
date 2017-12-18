using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GoComics
{
    internal enum ComicsImgQueries
    {
        SelectComicsImgs,
        InsertComicsImgs,
        UpdateComicsImgs,
        DeleteComicsImgs

    }
    class ComicsImgDb
    {
        public List<ComicsImg> Select(int? idImg, int? comicId, DateTime? forDate)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                //Create a list to store the result
                List<ComicsImg> list = new List<ComicsImg>();

                //Open connection
                if (dbConnect.OpenConnection())
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        Connection = dbConnect.SqlConnection,
                        CommandText = ComicsImgQueries.SelectComicsImgs.ToString(),
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@IdImg", idImg);
                    cmd.Parameters.AddWithValue("@ComicId", comicId);
                    cmd.Parameters.AddWithValue("@forDate", forDate);

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        ComicsImg comicsImg = new ComicsImg
                        {
                            IdImg = Convert.ToInt32(dataReader["IdImg"].ToString()),
                            JobId = Guid.Parse(dataReader["JobID"].ToString()),
                            ComicId = Convert.ToInt32(dataReader["ComicId"].ToString()),
                            ImgUrl = dataReader["ImgUrl"].ToString(),
                            ImagePath = dataReader["imagePath"].ToString(),
                            ComicUrl = dataReader["ComicUrl"].ToString(),
                            ForDate = Convert.ToDateTime(dataReader["forDate"].ToString()),
                            Visited = Convert.ToDateTime(dataReader["Visited"].ToString())
                        };

                        list.Add(comicsImg);
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
        public bool Insert(ComicsImg comicsImg)
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
                            CommandText = ComicsImgQueries.InsertComicsImgs.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@JID", comicsImg.JobId);
                        cmd.Parameters.AddWithValue("@CId", comicsImg.ComicId);
                        cmd.Parameters.AddWithValue("@ImgU", comicsImg.ImgUrl);
                        cmd.Parameters.AddWithValue("@imgPath", comicsImg.ImagePath);
                        cmd.Parameters.AddWithValue("@ComicU", comicsImg.ComicUrl);
                        cmd.Parameters.AddWithValue("@forD", comicsImg.ForDate);
                        cmd.Parameters.AddWithValue("@V", comicsImg.Visited);

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

        public bool Update(ComicsImg comicsImg)
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
                            CommandText = ComicsImgQueries.UpdateComicsImgs.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@IdImg", comicsImg.IdImg);
                        cmd.Parameters.AddWithValue("@JobID", comicsImg.JobId);
                        cmd.Parameters.AddWithValue("@ComicId", comicsImg.ComicId);
                        cmd.Parameters.AddWithValue("@ImgUrl", comicsImg.ImgUrl);
                        cmd.Parameters.AddWithValue("@imagePath", comicsImg.ImagePath);
                        cmd.Parameters.AddWithValue("@ComicUrl", comicsImg.ComicUrl);
                        cmd.Parameters.AddWithValue("@forDate", comicsImg.ForDate);
                        cmd.Parameters.AddWithValue("@Visited", comicsImg.Visited);

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

        public bool Delete(int IdImg)
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
                            CommandText = ComicsImgQueries.DeleteComicsImgs.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@IdImg", IdImg);

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
