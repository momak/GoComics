using System;
using System.Collections.Generic;
using System.Data;
using DatabaseFactory;
using GoComics.Models;

namespace GoComics.DAL
{
    internal enum ComicsSP
    {
        SelectComics,
        InsertComic,
        UpdateComic,
        DeleteComic
    }

    class ComicsManager : DataWorker
    {
        public List<Comics> Select(int? idComic)
        {
            //Create a list to store the result
            List<Comics> list = new List<Comics>();

            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(ComicsSP.SelectComics.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("idC", idComic));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Comics comics = new Comics
                                {
                                    IdComic = Convert.ToInt32(reader["IdComic"].ToString()),
                                    Title = reader["Name"].ToString(),
                                    UrlComic = reader["UrlComic"].ToString(),
                                    Description = reader["Description"].ToString()
                                };

                                list.Add(comics);
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

        public bool Insert(Comics comics)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(ComicsSP.InsertComic.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("@urlC", comics.UrlComic));
                        command.Parameters.Add(database.CreateParameter("@n", comics.Title));
                        command.Parameters.Add(database.CreateParameter("@D", comics.Description));

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

        public bool Update(Comics comics)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(ComicsSP.UpdateComic.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("@IdC", comics.IdComic));
                        command.Parameters.Add(database.CreateParameter("@url", comics.UrlComic));
                        command.Parameters.Add(database.CreateParameter("@N", comics.Title));
                        command.Parameters.Add(database.CreateParameter("@D", comics.Description));

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

        public bool Delete(Comics comics)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(ComicsSP.DeleteComic.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("@IdC", comics.IdComic));

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
