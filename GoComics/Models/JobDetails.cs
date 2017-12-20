using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoComics.Models
{
    class JobDetails
    {
        private Guid _jobId;
        private DateTime _startTime;
        private DateTime _endTime;
        private Guid guid;
        private DateTime now;

        public Guid JobId
        {
            get { return _jobId; }
            set { _jobId = value; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

       
        public JobDetails()
        {
        }

        public JobDetails(Guid jobId, DateTime startTime, DateTime endTime)
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;

        }

        public JobDetails(Guid guid, DateTime now)
        {
            this.guid = guid;
            this.now = now;
        }

        public override string ToString()
        {
            return $"Job: {JobId}, Started {StartTime}, Ended{EndTime}";
        }
    }
}
