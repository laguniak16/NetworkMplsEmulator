using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud
{
    class Connections
    {
        public string node1;
        public int port1;
        public string node2;
        public int port2;
        public string status;
        public Connections(string _node1,int _port1,string _node2,int _port2,string _status)
        {
            node1 = _node1;
            port1 = _port1;
            node2 = _node2;
            port2 = _port2;
            status = _status;
        }
    }
}
