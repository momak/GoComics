using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using DatabaseFactory;
using GoComics.Models;
using MySql.Data.MySqlClient;

namespace GoComics.DAL
{
    internal enum ComicsImgSP
    {
        SelectComicsImgs,
        GetImageUrl,
        InsertComicsImgs,
        UpdateComicsImgs,
        DeleteComicsImgs

    }
    class ComicsImgManager : DataWorker
    {
        public List<ComicsImg> Select(int? idImg, int? comicId, DateTime? forDate)
        {
            //Create a list to store the result
            List<ComicsImg> list = new List<ComicsImg>();

            try
            {
                using (IDbConnection dbConnect = Database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        Database.CreateStoredProcCommand(ComicsImgSP.SelectComicsImgs.ToString(), dbConnect))
                    {
                        command.Parameters.Add(Database.CreateParameter("IdImg", idImg));
                        command.Parameters.Add(Database.CreateParameter("ComicId", comicId));
                        command.Parameters.Add(Database.CreateParameter("forDate", forDate));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComicsImg comicsImg = new ComicsImg
                                {
                                    IdImg = Convert.ToInt32(reader["IdImg"].ToString()),
                                    JobId = Guid.Parse(reader["JobID"].ToString()),
                                    ComicId = Convert.ToInt32(reader["ComicId"].ToString()),
                                    ImgUrl = reader["ImgUrl"].ToString(),
                                    ImagePath = reader["imagePath"].ToString(),
                                    ComicUrl = reader["ComicUrl"].ToString(),
                                    ForDate = Convert.ToDateTime(reader["forDate"].ToString()),
                                    Visited = Convert.ToDateTime(reader["Visited"].ToString())
                                };

                                list.Add(comicsImg);
                            }
                        }
                    }
                    dbConnect.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return list;
        }

        public bool CheckImageUrl(string imageUrl)
        {
            List<ComicsImg> list = new List<ComicsImg>();
            try
            {
                using (IDbConnection dbConnect = Database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        Database.CreateStoredProcCommand(ComicsImgSP.GetImageUrl.ToString(), dbConnect))
                    {
                        command.Parameters.Add(Database.CreateParameter("ImgU", imageUrl));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComicsImg comicsImg = new ComicsImg
                                {
                                    IdImg = Convert.ToInt32(reader["IdImg"].ToString()),
                                    JobId = Guid.Parse(reader["JobID"].ToString()),
                                    ComicId = Convert.ToInt32(reader["ComicId"].ToString()),
                                    ImgUrl = reader["ImgUrl"].ToString(),
                                    ImagePath = reader["imagePath"].ToString(),
                                    ComicUrl = reader["ComicUrl"].ToString(),
                                    ForDate = Convert.ToDateTime(reader["forDate"].ToString()),
                                    Visited = Convert.ToDateTime(reader["Visited"].ToString())
                                };

                                list.Add(comicsImg);
                            }
                        }
                    }
                    dbConnect.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return (list.Count > 0);
        }

        public bool Insert(ComicsImg comicsImg)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = Database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        Database.CreateStoredProcCommand(ComicsImgSP.InsertComicsImgs.ToString(), dbConnect))
                    {
                        command.Parameters.Add(Database.CreateParameter("@JID", comicsImg.JobId));
                        command.Parameters.Add(Database.CreateParameter("@CId", comicsImg.ComicId));
                        command.Parameters.Add(Database.CreateParameter("@ImgU", comicsImg.ImgUrl));
                        command.Parameters.Add(Database.CreateParameter("@imgPath", comicsImg.ImagePath));
                        command.Parameters.Add(Database.CreateParameter("@ComicU", comicsImg.ComicUrl));
                        command.Parameters.Add(Database.CreateParameter("@forD", comicsImg.ForDate));
                        command.Parameters.Add(Database.CreateParameter("@V", comicsImg.Visited));

                        retVal = (command.ExecuteNonQuery() > 0);
                    }
                    dbConnect.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retVal;
        }

        public bool Update(ComicsImg comicsImg)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = Database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        Database.CreateStoredProcCommand(ComicsImgSP.UpdateComicsImgs.ToString(), dbConnect))
                    {
                        command.Parameters.Add(Database.CreateParameter("@IdImg", comicsImg.IdImg));
                        command.Parameters.Add(Database.CreateParameter("@JobID", comicsImg.JobId));
                        command.Parameters.Add(Database.CreateParameter("@ComicId", comicsImg.ComicId));
                        command.Parameters.Add(Database.CreateParameter("@ImgUrl", comicsImg.ImgUrl));
                        command.Parameters.Add(Database.CreateParameter("@imagePath", comicsImg.ImagePath));
                        command.Parameters.Add(Database.CreateParameter("@ComicUrl", comicsImg.ComicUrl));
                        command.Parameters.Add(Database.CreateParameter("@forDate", comicsImg.ForDate));
                        command.Parameters.Add(Database.CreateParameter("@Visited", comicsImg.Visited));

                        retVal = (command.ExecuteNonQuery() > 0);
                    }
                    dbConnect.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retVal;
        }

        public bool Delete(int IdImg)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = Database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        Database.CreateStoredProcCommand(ComicsImgSP.UpdateComicsImgs.ToString(), dbConnect))
                    {
                        command.Parameters.Add(Database.CreateParameter("@IdImg", IdImg));

                        retVal = (command.ExecuteNonQuery() > 0);
                    }
                    dbConnect.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return retVal;
        }
    }
}
