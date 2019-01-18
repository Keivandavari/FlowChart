using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Block : ChartBaseObject
    {
        private List<object> _inputs;
        public List<object> Inputs
        {
            get
            {
                if (_inputs == null) return new List<object>();
                else return _inputs;
            }
            set
            {
                _inputs = value;
            }
        }
        public object Output { get; set; }
    }
}
