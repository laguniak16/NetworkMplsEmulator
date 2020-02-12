using System;
using System.Collections.Generic;
using System.Text;

namespace Router
{
    
     class  Config
    {
        public int inPort;
        public string outPort;
        public int inLabel;       
        public string routerName;               
        public int operationID;
        public string labelAction;

        public Config(int _inPort, string _outPort, int _inLabel, string _routerName, int _operationID, string _labelAction)
        {
            inPort = _inPort;
            outPort = _outPort;
            inLabel = _inLabel;           
            routerName = _routerName;          
            operationID = _operationID;
            labelAction = _labelAction;
        }


    }
}
