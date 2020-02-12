using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Router
{
    class Agent
    {
        NetworkNode nd;
        Form1 form;
        string ip;
        int a = 0;
        static string nodename;
        public IPAddress ManagementSystemAddress = IPAddress.Parse("127.0.1.1");
        int ManagementSystemPort = 123;
        public Agent(NetworkNode _nd, Form1 _form)
        {
            nd = _nd;
            LabelAction la = new LabelAction(_nd);
            form = _form;
        }
        private Socket connectingSocket = null;

        public void Start()
        {
            while (true)
            {
                ConnectToMS();

                if (connectingSocket == null)
                {
                    // connection failed
                    // start over
                    continue;
                }

                while (true)
                {
                    try
                    {
                        byte[] buffer = new byte[5120];
                        int bytes = connectingSocket.Receive(buffer);

                        var message = Encoding.ASCII.GetString(buffer, 0, bytes);

                        Task.Run(() => HandleMessage(message));
                    }
                    catch (SocketException e)
                    {
                        // ignore timeout exceptions
                        if (e.SocketErrorCode != SocketError.TimedOut)
                        {
                            if (e.SocketErrorCode == SocketError.Shutdown || e.SocketErrorCode == SocketError.ConnectionReset)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }


        private void ConnectToMS()
        {
            while (true)
            {
                Socket socket = new Socket(ManagementSystemAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socket.ReceiveTimeout = 20000;

                try
                {
                    var result = socket.BeginConnect(new IPEndPoint(ManagementSystemAddress, ManagementSystemPort), null, null);

                    bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                    if (success)
                    {
                        socket.EndConnect(result);
                    }
                    else
                    {
                        socket.Close();
                        continue;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Probuje");
                }

                try
                {
                    Console.WriteLine("wysylam");
                    socket.Send(Encoding.ASCII.GetBytes($"HELLO {nd.name}"));

                    byte[] buffer = new byte[256];
                    int bytes = socket.Receive(buffer);

                    var message = Encoding.ASCII.GetString(buffer, 0, bytes);

                    if (message.Contains("HELLO"))
                    {
                        
                        connectingSocket = socket;

                        break;
                    }
                }
                catch (Exception)
                {

                }
            }
            //Task.Run(async () =>
            //{
            //    while (true)
            //    {
            //        var connectionGood = connectingSocket != null && connectingSocket.Connected;
            //        if (connectionGood)
            //        {
            //            connectingSocket.Send(Encoding.ASCII.GetBytes("KEEPALIVE"));

            //            await Task.Delay(5000);
            //        }
            //        else
            //        {

            //            break;
            //        }
            //    }
            //});
        }
        private void HandleMessage(string message)
        {

            //string data = Encoding.Default.GetString(message);
            //data = data.Replace("\0", string.Empty);
            var data3 = message.Split(' ');
            nd.configs.Clear();
            nd.labelsActions.Clear();

            for (int i = 0; i < data3.Length - 2; i++)
            {
                int inPort = Int32.Parse(data3[i + 2]);
                string outPort = data3[i + 3];
                int inLabel = Int32.Parse(data3[i]);
                string routerName = data3[i + 4];
                int operationID = Int32.Parse(data3[i + 7]);
                string labelActionStr = data3[i + 6];

                Config config = new Config(inPort, outPort, inLabel, routerName, operationID, labelActionStr);
                LabelAction labelAction = new LabelAction(Int32.Parse(data3[i + 7]), data3[i + 1], data3[i + 5]);

                if (config.routerName.Equals(nd.name))
                {
                    nd.configs.Add(config);
                    nd.labelsActions.Add(labelAction);
                }
                i = i + 8;

            }
            Console.WriteLine("Tablica konfiguracja");
            foreach (var conf in nd.configs)
            {

                Console.WriteLine(conf.inLabel +/* " | " + i.outLabel + */" | " + conf.inPort + " | " + conf.outPort + " | " + conf.routerName);
            }

        }
    }
}