using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GoComics
{
    internal enum JobDetailsQueries
    {
        SelectJob,
        InsertJob,
        UpdateJob,
        DeleteJob
    }
    class JobDetailsDB
    {
        //Select statement
        public List<JobDetails> Select(Guid? jobId)
        {
            using (DBconnect dbConnect = new DBconnect())
            {
                //Create a list to store the result
                List<JobDetails> list = new List<JobDetails>();

                //Open connection
                if (dbConnect.OpenConnection())
                {
                    //Create Command
                    MySqlCommand cmd = new MySqlCommand()
                    {
                        Connection = dbConnect.SqlConnection,
                        CommandText = JobDetailsQueries.SelectJob.ToString(),
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@IdJob", jobId);

                    //Create a data reader and Execute the command
                    MySqlDataReader dataReader = cmd.ExecuteReader();

                    //Read the data and store them in the list
                    while (dataReader.Read())
                    {
                        JobDetails jobDetails = new JobDetails
                        {
                            JobId = Guid.Parse(dataReader["IdJob"].ToString()),
                            StartTime = Convert.ToDateTime(dataReader["StartTime"].ToString()),
                            EndTime = Convert.ToDateTime(dataReader["EndTime"].ToString())
                        };

                        list.Add(jobDetails);
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
        public bool Insert(JobDetails jobDetails)
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
                            CommandText = JobDetailsQueries.InsertJob.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("jId", jobDetails.JobId);
                        cmd.Parameters.AddWithValue("StartD", jobDetails.StartTime);
                        cmd.Parameters.AddWithValue("EndD", jobDetails.EndTime);
                        
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

        public bool Update(JobDetails jobDetails)
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
                            CommandText = JobDetailsQueries.UpdateJob.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };
                        
                        cmd.Parameters.AddWithValue("jId", jobDetails.JobId);
                        cmd.Parameters.AddWithValue("startD", jobDetails.StartTime);
                        cmd.Parameters.AddWithValue("endD", jobDetails.EndTime);

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

        public bool Delete(int IdJob)
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
                            CommandText = JobDetailsQueries.DeleteJob.ToString(),
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@IdJob", IdJob);

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
