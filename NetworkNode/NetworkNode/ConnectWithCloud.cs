using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Router;

namespace Router
{
    class ConnectWithCloud
    {
        NetworkNode nd;
        Form1 form;
        string ip;
        public ConnectWithCloud(NetworkNode _nd,Form1 form1,string ip1)
        {
            form = form1;
            nd = _nd;
            ip = ip1;           
        }

        private Socket _connectingSocket = null;

        public void connectWithCloud()
        {

            while (_connectingSocket == null || !_connectingSocket.Connected)
            {
                try
                {
                    _connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    _connectingSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
                    SendPacket SendPacket = new SendPacket(_connectingSocket, form);
                    logger(SendPacket, DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString());
                    Task.Run(() => { receive(); });
                }
                catch { }
                Console.WriteLine("nawalam");

            }

            // Thread.Sleep(5000);

        }
        public void receive()
        {
            while (true)
            {
                while (_connectingSocket == null || !_connectingSocket.Connected)
                {
                    connectWithCloud();
                }
                try
                {
                    
                    var buffer = new byte[256];
                    _connectingSocket.Receive(buffer);
                    string data = Encoding.Default.GetString(buffer);
                    form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Received: " + data);
                    Console.WriteLine(data);
                    LabelAction la = new LabelAction(nd);
                    // la.RouteConnection(data);
                    SendPacket SendPacket = new SendPacket(_connectingSocket,form);
                    SendPacket.SendToCloud(Encoding.ASCII.GetBytes(la.RouteConnection(data)));
                                 
                }
                catch
                {
                    Console.WriteLine("Listener exception");
                }
            }
        }
        //public void send()
        //{

        //    while (_connectingSocket == null || !_connectingSocket.Connected)
        //    {
        //        try
        //        {
        //            _connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //            _connectingSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
        //            SendPacket SendPacket = new SendPacket(_connectingSocket);
        //            SendPacket.Send(Encoding.ASCII.GetBytes($"Message: H1 SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45"));
        //            form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Message: " + nd.name + " SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45");
        //            Task.Run(() => { receive(); });
        //        }
        //        catch { }
        //        Console.WriteLine("nawalam");

        //    }

        //       // Thread.Sleep(5000);
            
        //}

        public void logger(SendPacket sp, string time)
        {
            sp.SendToCloud(Encoding.ASCII.GetBytes($"HELLO {nd.name}"));
            //sp.SendToManager(Encoding.ASCII.GetBytes($"HELLO {nd.name}"));
            form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Connect to cloud");

        }
    }
}
