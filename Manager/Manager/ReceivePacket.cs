using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class ReceivePacket
    {
        private byte[] _buffer = new byte[4];
        private Socket _receiveSocket;
        Form1 form;
        Config config;
        int a = 0;
        
        public ReceivePacket(Socket receiveSocket,Form1 form1,Config config1)
        {
            
            config = config1;
            form = form1;
            _receiveSocket = receiveSocket;
        }

        public void StartReceiving()
        {
            try
            {
                _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, 0,new AsyncCallback(ReceiveCallback),null);
            }
            catch { }
        }
        private void ReceiveCallback(IAsyncResult AR)
        {           
            try
            {            
                    
                    _receiveSocket.EndReceive(AR);
                    _buffer = new byte[BitConverter.ToInt32(_buffer, 0)];
                    _receiveSocket.Receive(_buffer, _buffer.Length, SocketFlags.None);                 
                    string data = Encoding.Default.GetString(_buffer);

                    var data2 = data.Split(' ');                   
                    
                    byte[] bytes = new byte[1024];
                    StringBuilder sb = new StringBuilder();
                    string name = data2[4];

                    form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Connected with "+data2[4]);
                    

                  
                        for (int i = 0; i < config.configs.Count ; i++)
                            {
                               if (config.configs[i].routerName.Equals(name)){                            
                                sb.Append(config.configs[i].inLabel + " " + config.configs[i].outLabel + " " + config.configs[i].inPort + " " + config.configs[i].outPort + " " + config.configs[i].routerName + " " + config.configs[i].newLabel + " " + config.configs[i].labelAction + " " + config.configs[i].operationID + "  ");
                            }
                        }

                    bytes = Encoding.ASCII.GetBytes(sb.ToString());
                    _receiveSocket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), _receiveSocket);
                    form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Sending config to "+name);
                    
            }
            catch
            {              
            if (!_receiveSocket.Connected)
                {
                    Console.WriteLine("disconected");
                    Disconnect();
                }
                else
                {
                    Console.WriteLine("receive");
                    _buffer = new byte[4];
                    _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                }
            }
        }
        public void StartSending(string data)
        {
            try
            {
                _buffer = Encoding.ASCII.GetBytes(data);
                _receiveSocket.BeginSend(_buffer, 0, _buffer.Length, 0, new AsyncCallback(SendCallback), _receiveSocket);
            }
            catch { }
        }

        
        private void SendCallback(IAsyncResult ar)
        {   
            if(a==1)
            //_receiveSocket.EndSend(ar);

            a++;
            try
            { 
                Socket handler = (Socket)ar.AsyncState;
                handler.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        private void Disconnect()
        {      
            _receiveSocket.Disconnect(true);
        }
        
    }
}
