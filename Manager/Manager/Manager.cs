

using Manager;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manager
{
    class StateObject
    {
        // Client  socket
        public Socket WorkSocket { get; set; } = null;
        // Size of receive buffer
        public const int BufferSize = 1024;
        // Receive buffer
        public byte[] Buffer { get; set; } = new byte[BufferSize];
        // Received data string
        public StringBuilder sb { get; set; } = new StringBuilder();
    }
    public class Manager
    {
        Config config;
        private ManualResetEvent AllDone = new ManualResetEvent(false);
        private const int Backlog = 100;
        private IPAddress ManagementSystemAddress = IPAddress.Parse("127.0.1.1");
        private Socket Server;
        private const int ManagementSystemPort = 123;
        private ConcurrentDictionary<string, Socket> RouterNameToSocket;
        public ConcurrentDictionary<Socket, string> SocketToRouterName { get; set; }
        // public Dictionary<string, List<FtnTableRow>> RouterNameToFTN_Table { get; set; } = new Dictionary<string, List<FtnTableRow>>();
        // public Dictionary<string, List<MplsFibTableRow>> RouterNameToMPLS_FIB_Table { get; set; } = new Dictionary<string, List<MplsFibTableRow>>();
        //  public Dictionary<string, List<IpFibTableRow>> RouterNameToIP_FIB_Table { get; set; } = new Dictionary<string, List<IpFibTableRow>>();
        // public Dictionary<string, List<IlmTableRow>> RouterNameToILMTable { get; set; } = new Dictionary<string, List<IlmTableRow>>();
        //  public Dictionary<string, List<NHLFETableRow>> RouterNameToNHLFE_Table { get; set; } = new Dictionary<string, List<NHLFETableRow>>();

        public Manager()
        {
            RouterNameToSocket = new ConcurrentDictionary<string, Socket>();
            SocketToRouterName = new ConcurrentDictionary<Socket, string>();
        }
        public Manager(Config cof)
        {
            RouterNameToSocket = new ConcurrentDictionary<string, Socket>();
            SocketToRouterName = new ConcurrentDictionary<Socket, string>();
            config = cof;

        }

        public void Start()
        {
            Task.Run(action: () => StartServer());
        }

        private void StartServer()
        {
            Server = new Socket(ManagementSystemAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Server.Bind(new IPEndPoint(ManagementSystemAddress, ManagementSystemPort));
                Server.Listen(Backlog);
                while (true)
                {
                    AllDone.Reset();
                    Server.BeginAccept(new AsyncCallback(AcceptCallback), Server);
                    AllDone.WaitOne();
                }
            }
            catch (Exception e)
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
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
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
                Socket outSocket;
                string outString;

                var routerName = SocketToRouterName[handler];
                RouterNameToSocket.TryRemove(routerName, out outSocket);
                SocketToRouterName.TryRemove(handler, out outString);
                // if the client has been shutdown, then close the connection
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                return;
            }
            state.sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, bytesRead));
            var content = state.sb.ToString().Split(' ');
            if (content[0].Equals("HELLO"))
            {
                var routerName = content[1];

                while (true)
                {
                    var success = RouterNameToSocket.TryAdd(routerName, handler);
                    if (success)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }

                while (true)
                {
                    var success = SocketToRouterName.TryAdd(handler, routerName);
                    if (success)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                }
                SendResponse(routerName, handler);
            }
            else
            {

                return;
            }
            state.sb.Clear();
            handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        private void SendResponse(string routerName, Socket handler)
        {
            byte[] responseMessage = Encoding.ASCII.GetBytes("HELLO");
            handler.Send(responseMessage);

            SendTable(routerName);

        }
        public void SendTable(string routerName)
        {
            var socket = RouterNameToSocket[routerName];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < config.configs.Count; i++)
            {
                if (config.configs[i].routerName.Equals(routerName))
                {
                    sb.Append(config.configs[i].inLabel + " " + config.configs[i].outLabel + " " + config.configs[i].inPort + " " + config.configs[i].outPort + " " + config.configs[i].routerName + " " + config.configs[i].newLabel + " " + config.configs[i].labelAction + " " + config.configs[i].operationID + "  ");
                }
            }
            byte[] data = Encoding.ASCII.GetBytes(sb.ToString());
            socket.Send(data);
        }
        public void SendNewTable(string routerName, string mess)
        {
            var socket = RouterNameToSocket[routerName];
            byte[] data = Encoding.ASCII.GetBytes(mess);
            socket.Send(data);
        }
    }
}

