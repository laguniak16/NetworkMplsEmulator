using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Cloud
{
    public partial class CableCloud
    {
        private ManualResetEvent AllDone = new ManualResetEvent(false);
        /// <summary>
        /// How many messages can be stored in server's buffer
        /// </summary>
        private const int Backlog = 100;
        private IPAddress CloudAddress;
        
        private int CloudPort =1234;
        public CableCloudConfig Config { get; set; }
        private Socket Server;
        // for instance: socket, H1
        private ConcurrentDictionary<Socket, string> SocketToNodeName;
        // for instance: H1, socket
        private ConcurrentDictionary<string, Socket> NodeNameToSocket;
        // public CableCloudGUI window { get; set; }
        Form1 _form;

        public CableCloud(string filePath, Form1 form)
        {
            _form = form;
            ReadConfig(filePath); 
            SocketToNodeName = new ConcurrentDictionary<Socket, string>();
            NodeNameToSocket = new ConcurrentDictionary<string, Socket>();
       
            Task.Run(() => StartServer());
        }
        private void ReadConfig(string filePath)
        {
            XmlDocument xDoc = new XmlDocument();  
            xDoc.Load(filePath);  
            XmlNodeList address = xDoc.GetElementsByTagName("address");  
            XmlNodeList port = xDoc.GetElementsByTagName("port");  
            Console.WriteLine(address[0].InnerText);
            Console.WriteLine(port[0].InnerText);

            CloudAddress = IPAddress.Parse(address[0].InnerText);
            CloudPort = int.Parse(port[0].InnerText);

 /*           StreamReader file = new StreamReader(filePath);
            string line = file.ReadLine();
            var parts = line.Split(' ');
            CloudAddress = IPAddress.Parse(parts[1]);
            line = file.ReadLine();
            parts = line.Split(' ');
            CloudPort = int.Parse(parts[1]);
            file.ReadLine(); file.ReadLine(); file.ReadLine(); // skip 3 lines
   */         Config = new CableCloudConfig(xDoc);
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

            //Thread.Sleep(1000);
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
              
                        break;
                    
                    Thread.Sleep(100);
                }
                // e.g. H1, socket
                while (true)
                {
                    var success = NodeNameToSocket.TryAdd(content[1], handler);
                    
                        break;
                    
                    Thread.Sleep(100);
                }
                AddLog($"Estabilished connection with {content[1]}");
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
            Packet receivedPackage = Packet.FromBytes(state.Buffer);
            string node = SocketToNodeName[handler];
            //string node = Config.NodeNameToIp.FirstOrDefault(x => x.Value == receivedPackage.SourceAddress).Key[handler];
            string IP = Config.GetIP(node);
            string port = Convert.ToString(receivedPackage.Port);
            AddLog($"Received package from {IP}:{port}");
            // Received package from node X, port Y -> check to which port and node send it now
            try
            {
                string nextNode = Config.GetNextNode(node, port);
                string nextPort = Config.GetNextPort(node, port);
                string cableStatus = Config.GetCableStatus(node, port, nextNode, nextPort);
                string nextIP = Config.GetIP(nextNode);
                if (cableStatus.Equals("DEAD"))
                {
                    AddLog($"The cable connecting {IP}:{port} to {nextIP}:{nextPort} is corrupt. " +
                        $"Package discarded!");
                }
                else if (cableStatus.Equals("RUNNING"))
                {
                    receivedPackage.Port = ushort.Parse(nextPort);
                    Socket socket = NodeNameToSocket[nextNode];
                    SendPackage(socket, receivedPackage.ToBytes());
                    AddLog($"Sent package to {nextIP}:{nextPort}");
                }
            }
            catch (Exception)
            {
                var address = Config.GetIP(node);
                AddLog($"{address}:{port} is not connected. " +
                    $"Package discarded!");
            }
        }

        private void AddLog(string log)
        {
            _form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " " + log);
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
}