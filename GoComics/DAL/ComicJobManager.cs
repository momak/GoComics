using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseFactory;
using GoComics.Models;

namespace GoComics.DAL
{
    internal enum ComicsJobSP
    {
        GetComicsForDay

    }
    class ComicJobManager : DataWorker
    {
        public List<ComicsJob> Select(DateTime forDate)
        {
            //Create a list to store the result
            List<ComicsJob> list = new List<ComicsJob>();

            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(ComicsJobSP.GetComicsForDay.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("forDay", forDate));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ComicsJob comicsJob = new ComicsJob
                                {
                                    IdComic = Convert.ToInt32(reader["IdComic"].ToString()),
                                    Title = reader["Name"].ToString(),
                                    UrlComic = reader["UrlComic"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Has = Convert.ToBoolean(reader["Has"])
                                };

                                list.Add(comicsJob);
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
    }
}
