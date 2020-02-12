using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Client;
using System.Threading.Tasks;


namespace Client
{
    public class ClientNode
    {
        public string[] hostsIP = new string[3];
        public string cloudIP;
        public int cloudPort;
        public string addressIP;
        public string name;
        public int outPort;
        public int label;
        public Form1 form;
        private Socket _connectingSocket;

        public ClientNode()
        {

        }

        public ClientNode(string _cloudIP, int _cloudPort, string _addressIP, string _name, int _outPort )
        {
            cloudIP = _addressIP;
            cloudPort = _cloudPort;
            addressIP = _addressIP;
            name = _name;
            outPort = _outPort;
       
        }
        

        public void fillTable(string[] tab)
        {
            hostsIP = tab;
        }

        public void start(Form1 form)
        {
            this.form = form;
            connectWithCloud();
            form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Connect With Cloud");
        }

       public void send(String message, string destination)
        {
            while (_connectingSocket == null || !_connectingSocket.Connected)
            {
                try
                {
                    connectWithCloud();
                }
                catch { }
            }
            chooseHostIp(destination);
            SendPacket SendPacket = new SendPacket(_connectingSocket, form);
            SendPacket.Send(Encoding.ASCII.GetBytes("LabelStack="+label+";Message="+message+";Source="+addressIP+";Destination="+destination+";Port="+outPort));
            
        }
       
         public void connectWithCloud()
        {
           
                while (_connectingSocket == null || !_connectingSocket.Connected)
                {
                    try
                    {
                        _connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _connectingSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
                        SendPacket SendPacket = new SendPacket(_connectingSocket, form);
                        SendPacket.Send(Encoding.ASCII.GetBytes($"HELLO {name}"));
                        //form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Message: HELLO SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45 ");
                        
                        Task.Run(() => { recive(); });
                }
                    catch { }
                    Console.WriteLine("nawalam");
                }
                
        }

        public void recive()
        {
            while (true)
            {
                while (_connectingSocket == null || !_connectingSocket.Connected)
                {
                   connectWithCloud();
                }
                try{
                                    var buffer = new byte[256];
                    _connectingSocket.Receive(buffer);
                    string data = Encoding.Default.GetString(buffer);
                    form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Receive: " + data);
                    Console.WriteLine(data);
                }catch{
                     Console.WriteLine("Listener exception");
                }
            }
        }

        public void chooseHostIp(string destination)
        {
            if (name.Equals("H1") && destination.Equals("172.13.222.33"))   //H1 to H2
            {
                label = 12;
                
            }
            else if (name.Equals("H1") && destination.Equals("188.88.141.12")) //H1 to H3
            {
                label = 13;
            }
            else if (name.Equals("H1") && destination.Equals("119.44.155.20"))  //H1 to H4
            {
                label = 11;
            }
            else if (name.Equals("H2") && destination.Equals("172.16.222.34"))  //H2 to H1
            {
                label = 15;
            }
            else if (name.Equals("H2") && destination.Equals("119.44.155.20"))  //H2 to H4
            {
                label = 16;
            }
            else if (name.Equals("H2") && destination.Equals("188.88.141.12"))  //H2 to H3
            {
                label = 19;
            }
            else if (name.Equals("H3") && destination.Equals("172.16.222.34"))  //H3 to H1
            {
                label = 17;
            }
            else if (name.Equals("H3") && destination.Equals("172.13.222.33"))  //H3 to H2
            {
                label = 20;
            }
            else if (name.Equals("H3") && destination.Equals("119.44.155.20"))  //H3 to H4
            {
                label = 18;
            }
            else if (name.Equals("H4") && destination.Equals("172.16.222.34"))  //H4 to H1
            {
                label = 13;
            }
            else if (name.Equals("H4") && destination.Equals("172.13.222.33"))  //H4 to H2
            {
                label = 12;
            }
            else if (name.Equals("H4") && destination.Equals("188.88.141.12"))  //H4 to H3
            {
                label = 11;
            }
        }

    }
}
