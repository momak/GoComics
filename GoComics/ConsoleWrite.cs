using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoComics.Models;

namespace GoComics
{
    class ConsoleWrite : ILog, IDisposable
    {
        private LogMode _lm;
        private LogDetail _ld;
        private readonly string _outputFile;
        private JobDetails _jobDetails;
        private readonly object locker = new object();

        public LogMode Lmode
        {
            get => _lm;
            set => _lm = value;
        }
        public LogDetail Ldetail
        {
            get => _ld;
            set => _ld = value;
        }

        public ConsoleWrite()
        {
        }

        public ConsoleWrite(LogMode lm, LogDetail ld, string outputFile)
        {
            _lm = lm;
            _ld = ld;
            _outputFile = outputFile;
        }

        public ConsoleWrite(LogMode lm, LogDetail ld, string outputFile, JobDetails jobDetails)
        {
            _lm = lm;
            _ld = ld;
            _outputFile = outputFile;
            _jobDetails = jobDetails;
            lock (locker)
            {
                WriteHeaderToFile();
            }
        }

        public void WriteLine(string message)
        {
            try
            {
                lock (locker)
                {
                    switch (_lm)
                    {
                        case LogMode.Console:
                            {
                                WriteToConsole(message);
                                break;
                            }
                        case LogMode.File:
                            {
                                WriteToFile(message);
                                break;
                            }
                        case LogMode.Both:
                            {
                                WriteToConsole(message);
                                WriteToFile(message);
                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void WriteHeaderToFile()
        {
            using (StreamWriter writer = new StreamWriter(_outputFile, true))
            {
                writer.WriteLine($"**********************************************************");
                writer.WriteLine($"    Job Id: {_jobDetails.JobId}");
                writer.WriteLine($"    Start Date: {_jobDetails.StartTime:d} {_jobDetails.StartTime:HH:mm:ss}");
                writer.WriteLine($"**********************************************************");
            }
        }

        private void WriteFooterToFile()
        {
            using (StreamWriter writer = new StreamWriter(_outputFile, true))
            {
                writer.WriteLine($"**********************************************************");
                writer.WriteLine($"    Job Id: {_jobDetails.JobId}");
                writer.WriteLine($"    End Date: {_jobDetails.EndTime:d} {_jobDetails.EndTime:HH:mm:ss}");
                writer.WriteLine($"**********************************************************");
            }
        }

        private void WriteToFile(string message)
        {
            using (StreamWriter writer = new StreamWriter(_outputFile, true))
            {
                writer.WriteLine(message);
            }
        }

        private void WriteToConsole(string message)
        {
            switch (_ld)
            {
                case LogDetail.None:
                    {
                        Console.ResetColor();
                        break;
                    }
                case LogDetail.Information:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    }
                case LogDetail.Error:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    }
                case LogDetail.Debug:
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                }
                case LogDetail.Success:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    }
            }
            Console.WriteLine(message);
        }

        public void Dispose()
        {
            lock (locker)
            {
                WriteFooterToFile();
            }
        }

    }
}

