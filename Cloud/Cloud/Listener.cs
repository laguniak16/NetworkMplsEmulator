using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cloud;
using System.IO;
using System.Xml;

namespace Cloud
{
    class Listener
    {
        public static List<Connections> connections = new List<Connections>();
        public Socket ListenerSocket; //This is the socket that will listen to any incoming connections
        public short Port = 1234; // on this port we will listen
        private ManualResetEvent AllDone = new ManualResetEvent(false);

        /// <summary>
        /// How many messages can be stored in server's buffer
        /// </summary>
        private const int Backlog = 100;
        private IPAddress CloudAddress= IPAddress.Parse("127.0.0.1");
        private int CloudPort = 1234;
        public CableCloudConfig Config { get; set; }
        private Socket Server;
        // for instance: socket, H1
        private ConcurrentDictionary<Socket, string> SocketToNodeName;
        // for instance: H1, socket
        private ConcurrentDictionary<string, Socket> NodeNameToSocket;
        

        Form1 form1;

        public Listener(Form1 form)
        {
            form1 = form;
           // Task.Run(() => StartListening());
        }


        public void StartServer()
        {
            Server = new Socket(CloudAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Server.Bind(new IPEndPoint(CloudAddress, CloudPort));
                Server.Listen(Backlog);
                while (true)
                {
                    AllDone.Reset();
                    Server.BeginAccept(new AsyncCallback(AcceptCallback), Server);
                    AllDone.WaitOne();
                }
            }
            catch (Exception)
            {
            }
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.WorkSocket = handler;
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        private void ReadCallback(IAsyncResult ar)
        {
            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.WorkSocket;
            // Read data from the client socket.
            int bytesRead;
            try
            {
                bytesRead = handler.EndReceive(ar);
            }
            catch (Exception)
            {
                string outString;
                Socket outSocket;

                // if the client has been shutdown, then close the connection
                var nodeName = SocketToNodeName[handler];
                SocketToNodeName.TryRemove(handler, out outString);
                NodeNameToSocket.TryRemove(nodeName, out outSocket);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return;
            }
            state.sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
            var content = state.sb.ToString().Split(' ');
            // This will be the first message sent by client
            if (content[0].Equals("HELLO"))
            {
                // if the message will be received as: HELLO H1KEEPALIVEKEEPALIVE...
                int index = content[1].IndexOf("K");
                if (index > 0)
                {
                    content[1] = content[1].Substring(0, index);
                }
                // e.g. socket, H1
                while (true)
                {
                    var success = SocketToNodeName.TryAdd(handler, content[1]);
                    if (success)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                // e.g. H1, socket
                while (true)
                {
                    var success = NodeNameToSocket.TryAdd(content[1], handler);
                    if (success)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                //AddLog($"Estabilished connection with {content[1]}", LogType.Connected);
            }
            // keep connection alive - should receive it every 5s
            else if (content[0].Equals("KEEPALIVE"))
            {
                // do nothing
            }
            // receive and forward a package
            else
            {
                ProcessPackage(state, handler, ar);
            }
            state.sb.Clear();
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void ProcessPackage(StateObject state, Socket handler, IAsyncResult ar)
        {
            var receivedPackage = Packet.FromBytes(state.Buffer);
            string node = SocketToNodeName[handler];
            string IP = Config.GetIP(node);
            string port = Convert.ToString(receivedPackage.Port);
            //($"Received package from {IP}:{port}", LogType.Received);
            // Received package from node X, port Y -> check to which port and node send it now
            try
            {
                string nextNode = Config.GetNextNode(node, port);
                string nextPort = Config.GetNextPort(node, port);
                string cableStatus = Config.GetCableStatus(node, port, nextNode, nextPort);
                string nextIP = Config.GetIP(nextNode);
                if (cableStatus.Equals("DEAD"))
                {
                //    AddLog($"The cable connecting {IP}:{port} to {nextIP}:{nextPort} is out of order. " +
                //        $"Package discarded!", LogType.discarded);
                }
                else if (cableStatus.Equals("RUNNING"))
                {
                    receivedPackage.Port = ushort.Parse(nextPort);
                    Socket socket = NodeNameToSocket[nextNode];
                    SendPackage(socket, receivedPackage.ToBytes());
                 //   AddLog($"Sent package to {nextIP}:{nextPort}", LogType.Sent);
                }
            }
            catch (Exception)
            {
                var address = Config.GetIP(node);
               // AddLog($"{address}:{port} is not connected. " +
               //     $"Package discarded!", LogType.discarded);
            }
        }
        private void SendPackage(Socket handler, byte[] byteData)
        {
            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }

     public class CableCloudConfig
    {
        // e.g. H1, 1.1.1.1
        private Dictionary<string, string> NodeNameToIP;
        // e.g. H1, R1
        private Dictionary<string, string> PairsOfConnectedNodes;
        // e.g. H1:1111-R2:2222, WORKING
        public static Dictionary<string, string> StatusOfCableBetweenNodes { get; set; }
        public CableCloudConfig(XmlDocument xDoc)
        {
            NodeNameToIP = new Dictionary<string, string>(); 
           
            //XML    
            XmlNodeList nodes = xDoc.GetElementsByTagName("node"); 
            foreach(XmlNode node in nodes)
                NodeNameToIP.Add(node["name"].InnerText, node["address"].InnerText);
            XmlNodeList connections = xDoc.GetElementsByTagName("connection"); 
            PairsOfConnectedNodes = new Dictionary<string, string>();
            StatusOfCableBetweenNodes = new Dictionary<string, string>();
            foreach(XmlNode connection in connections){
               String address1 =connection["node1"].InnerText + ":" + connection["port1"].InnerText;
               String address2 =connection["node2"].InnerText + ":" + connection["port2"].InnerText;
               PairsOfConnectedNodes.Add(address1, address2);
               PairsOfConnectedNodes.Add(address2, address1);

               StatusOfCableBetweenNodes.Add(address1 + "-" + address2, connection["status"].InnerText);
               StatusOfCableBetweenNodes.Add(address2 + "-" + address1, connection["status"].InnerText);
            }

            //XML

  /*          string line;
   
            while (!(line = file.ReadLine()).Equals(""))
            {
                // e.g. H1 1.1.1.1
                var parts = line.Split(' ');
                NodeNameToIP.Add(parts[0], parts[1]);
            }
            file.ReadLine(); file.ReadLine(); // skip 2 lines

            PairsOfConnectedNodes = new Dictionary<string, string>();
            StatusOfCableBetweenNodes = new Dictionary<string, string>();
            while ((line = file.ReadLine()) != null)
            {
                // e.g. R1 20300 H1 10100 

                var parts = line.Split(' ');
                string node1 = parts[0], port1 = parts[1], node2 = parts[2], port2 = parts[3], status = parts[4];
                PairsOfConnectedNodes.Add(node1 + ":" + port1, node2 + ":" + port2);
                // in config there is only R1 20300 H1 10100, but we need H1 10100 R1 20300 as well
                PairsOfConnectedNodes.Add(node2 + ":" + port2, node1 + ":" + port1);
                var address1 = node1 + ":" + port1;
                var address2 = node2 + ":" + port2;
                StatusOfCableBetweenNodes.Add(address1 + "-" + address2, status);
                StatusOfCableBetweenNodes.Add(address2 + "-" + address1, status);
            }
    */    }

        public string GetIP(string node)
        {
            return NodeNameToIP[node];
        }

        public string GetCableStatus(string node1, string port1, string node2, string port2)
        {
            var connectedNodes = node1 + ":" + port1 + "-" + node2 + ":" + port2;
            return StatusOfCableBetweenNodes[connectedNodes];
        }

        public string GetNextNode(string node, string port)
        {
            var parts = PairsOfConnectedNodes[node + ":" + port].Split(':');
            return parts[0];
        }

        public string GetNextPort(string node, string port)
        {
            var parts = PairsOfConnectedNodes[node + ":" + port].Split(':');
            return parts[1];
        }
    }
}
