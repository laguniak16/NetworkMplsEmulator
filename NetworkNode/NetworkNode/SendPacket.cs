using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Router
{
    public class SendPacket
    {
        private Socket _sendSocked;
        Form1 _form;
        public SendPacket(Socket sendSocket, Form1 form)
        {
            _form = form;
            _sendSocked = sendSocket;
        }
        public void SendToCloud(byte[] data)
        {
            try
            {

                var fullPacket = new List<byte>();
                //fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                fullPacket.AddRange(data);
                Console.WriteLine(fullPacket.ToString());
                Console.WriteLine(Encoding.Default.GetString(data));
                _form.Data(DateTime.Now.ToLongTimeString() + ":" + DateTime.Now.Millisecond.ToString() + "  Send: " + Encoding.Default.GetString(data));
                _sendSocked.Send(fullPacket.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void SendToManager(byte[] data)
        {
            try
            {

                var fullPacket = new List<byte>();
               // fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                fullPacket.AddRange(data);
                _sendSocked.Send(fullPacket.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void SendCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;            
            handler.EndSend(ar);

        }
    }
}