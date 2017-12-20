using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Remoting.Messaging;
using DatabaseFactory;
using GoComics.Models;
using MySql.Data.MySqlClient;

namespace GoComics.DAL
{
    internal enum JobDetailsSP
    {
        SelectJob,
        InsertJob,
        UpdateJob,
        DeleteJob
    }
    class JobDetailsManager : DataWorker
    {
        //Select statement
        public List<JobDetails> Select(Guid? jobId)
        {
            //Create a list to store the result
            List<JobDetails> list = new List<JobDetails>();

            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(JobDetailsSP.SelectJob.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("@IdJob", jobId));

                        using (IDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                JobDetails jobDetails = new JobDetails
                                {
                                    JobId = Guid.Parse(reader["IdJob"].ToString()),
                                    StartTime = Convert.ToDateTime(reader["StartTime"].ToString()),
                                    EndTime = Convert.ToDateTime(reader["EndTime"].ToString())
                                };

                                list.Add(jobDetails);
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

        //Insert statement
        public bool Insert(JobDetails jobDetails)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(JobDetailsSP.InsertJob.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("jId", jobDetails.JobId));
                        command.Parameters.Add(database.CreateParameter("@StartD", jobDetails.StartTime));
                        command.Parameters.Add(database.CreateParameter("@EndD", jobDetails.EndTime));

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

        public bool Update(JobDetails jobDetails)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(JobDetailsSP.UpdateJob.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("jId", jobDetails.JobId));
                        command.Parameters.Add(database.CreateParameter("@StartD", jobDetails.StartTime));
                        command.Parameters.Add(database.CreateParameter("@EndD", jobDetails.EndTime));

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

        public bool Delete(int IdJob)
        {
            bool retVal = false;
            try
            {
                using (IDbConnection dbConnect = database.CreateOpenConnection())
                {
                    using (IDbCommand command =
                        database.CreateStoredProcCommand(JobDetailsSP.DeleteJob.ToString(), dbConnect))
                    {
                        command.Parameters.Add(database.CreateParameter("IdJob", IdJob));

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
