using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Block : ChartBaseObject
    {
        private List<Object> _inputs;
        public List<Object> Inputs
        {
            get
            {
                if (_inputs == null) return new List<Object>();
                else return _inputs;
            }
            set
            {
                _inputs = value;
            }
        }
        public Object Output { get; set; }
        
        // Other Function Class Objects
        // ****
    }
}
