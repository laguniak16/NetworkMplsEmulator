using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Router;

namespace Router
{
   
    class NetworkNode
    {
        private Socket _connectingSocket = null;

        public string name;
        public string managerIP;
        public int managerPort;
        public string cloudIP;
        public int cloudPort;

        public List<Config> configs = new List<Config>();                               //lista do przechowywania configu odebranego od menadzera jako obiekty klasy config
        public List<Config> configs2 = new List<Config>();
        public List<LabelAction> labelsActions = new List<LabelAction>();
        public NetworkNode()
        {
        }
        public NetworkNode(string _name, string _managerIP, int _managerPort, string _cloudIP, int _cloudPort)
        {
            name = _name;
            managerIP = _managerIP;
            managerPort = _managerPort;
            cloudIP = _cloudIP;
            cloudPort = _cloudPort;
        }
        public void connect(NetworkNode nd,Form1 form1,string ip)
        {
            ConnectWithCloud connect = new ConnectWithCloud(nd,form1,ip);
            connect.connectWithCloud();
        }

        public void connectWithManager(NetworkNode nd,Form1 form1)
        {
            Agent agent = new Agent(nd,form1);
            form1.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "Connect With Manager");
            agent.Start();

        }

        public void sendPacket(NetworkNode nd, Form1 form1, string ip)
        {
           // ConnectWithCloud connect = new ConnectWithCloud(nd, form1, ip);
          //  connect.send();
        }
    }
}
