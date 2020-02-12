using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Cloud;

namespace Cloud
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


    public class ReceivePacket
    {
        Form1 form1;
        private byte[] _buffer = new byte[4];
        private Socket _receiveSocket;
        public byte[] Buffer { get; set; } = new byte[1024];
        public ReceivePacket(Socket receiveSocket,Form1 form)
        {
            _receiveSocket = receiveSocket;
            form1 = form;
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
                    var content = data.Split(' ');
                // Console.WriteLine("Otrzymalem polaczenie od "+data);
                    
                    
                form1.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + " Received connection  " + data);

                if (content[2].Equals("R1"))
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "Ty jestes R1 to wysylam Ci twoje ustawienia:");
                    form1.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "Ty jestes R1 to wysylam Ci twoje ustawienia:");
                    _receiveSocket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), _receiveSocket);
                        _receiveSocket.EndSend(AR);       
                    }
                    if (content[2].Equals("R2"))
                    {


                    byte[] bytes = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "Ty jestes R2 to wysylam Ci twoje ustawienia:");
                    _receiveSocket.BeginSend(bytes, 0, bytes.Length, 0, new AsyncCallback(SendCallback), _receiveSocket);
                    _receiveSocket.EndSend(AR);
                    }
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
                    _receiveSocket.BeginReceive(_buffer, 0, _buffer.Length, 0, new AsyncCallback(ReceiveCallback), null);
                }
            }
        }
        private void SendCallback(IAsyncResult ar)
        {
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
