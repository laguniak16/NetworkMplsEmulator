using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Client;

namespace Client
{
    class ConnectWithCloud
    {
        //private Socket _connectingSocket = null;
        //private Socket connectingSocket = null;
        //Form1 form;
        //string port = null;
        //string ipSource = null;
        //int label = 0;
        
        //public ConnectWithCloud(Form1 form1)
        //{
        //    form = form1;
        //}

        //public ConnectWithCloud()
        //{

        //}
        //public void connectWithCloud()
        //{
           
        //        while (_connectingSocket == null || !_connectingSocket.Connected)
        //        {
        //            try
        //            {
        //                _connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //                _connectingSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
        //                SendPacket SendPacket = new SendPacket(_connectingSocket);
        //                //SendPacket.Send(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Message: " + NetworkNodeConfig.name + " SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45");
        //                //SendPacket.Send(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Message: " + client.name +" SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45 ");
        //                SendPacket.Send(Encoding.ASCII.GetBytes($"HELLO H1"));
        //                form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Message: HELLO SourceIP: 178.199.23.23  DestinationIP: 178.123.45.45 ");
        //            }
        //            catch { }
        //            Console.WriteLine("nawalam");
        //        }
                
        //}
        //public void recive()
        //{
        //    if (_connectingSocket.Connected)
        //    {
        //        var buffer = new byte[256];
        //        _connectingSocket.Receive(buffer);
        //        string data = Encoding.Default.GetString(buffer);
        //        Console.WriteLine(data);
        //    }
        //}

        //public void send(String message,float time,string destination,string hostname)
        //{
        //    while (_connectingSocket == null || !_connectingSocket.Connected)
        //    {
        //        try
        //        {

        //            connectWithCloud();
        //        }
        //        catch { }
        //    }               
            
        //    SendPacket SendPacket = new SendPacket(_connectingSocket);
        //    SendPacket.Send(Encoding.ASCII.GetBytes(message));
        //}
        

        //            try
        //            {
        //                chooseHostIp(hostname);
        //                connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //                connectingSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1234));
        //                SendPacket SendPacket = new SendPacket(connectingSocket);
                        
        //                SendPacket.Send(Encoding.ASCII.GetBytes("LabelStack="+label+";Message="+message+";Source="+ipSource+";Destination="+destination+";Port=1234"));

        //            }
        //            catch { }
        //            //form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Trying to connect with manager ");
        //        }               
        //        Thread.Sleep(5000);
        //    }
        //}

        //public void chooseHostIp(string name)
        //{
        //    if (name.Equals("H1"))
        //    {
        //        ipSource = "172.16.222.31";
        //        port = "";
        //        label = 10;
        //            ;
        //    } else if (name.Equals("H2")){

        //        ipSource="172.10.245.32";
        //        port = "";
        //        label = 10;
        //    }
        //    else if (name.Equals("H3")){

        //        ipSource = "172.13.67.33";
        //        port = "";
        //        label = 10;
        //    }
        //    else if (name.Equals("H4")){

        //        ipSource = "172.32.126.34";
        //        port = "";
        //        label = 10;
        //    }
        //}
    }
}
