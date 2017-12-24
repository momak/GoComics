using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoComics.Models
{
    class ComicsJob : Comics
    {
        private bool _has;

        public bool Has
        {
            get { return _has; }
            set { _has = value; }
        }

        public ComicsJob() : base()
        {
        }
        public ComicsJob(bool has) : base()
        {
            Has = has;
        }

        public override string ToString()
        {
            return  $"{base.ToString()}, {Has}";
        }
    }
}
