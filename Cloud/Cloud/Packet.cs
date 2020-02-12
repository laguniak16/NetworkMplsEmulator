using System;
using System.Net;
using System.Text;

public class Packet
{
    public int ID { get; set; }
    public string LabelStack { get; set; } 
    public IPAddress SourceAddress { get; set; }
    public IPAddress DestAddress { get; set; }
    public ushort Port { get; set; }
    public string Message { get; set; }

    public byte[] ToBytes()
    {
        return Encoding.ASCII.GetBytes(this.ToString());
    }

    public static Packet FromBytes(byte[] bytes)
    {
        try
        {
            Packet packet = new Packet();
            string str =  Encoding.ASCII.GetString(bytes);
            string[] parts = str.Split(';');

           // Console.WriteLine(str);

            packet.LabelStack = parts[0].Split('=')[1];
            packet.Message = parts[1].Split('=')[1];
            packet.SourceAddress = IPAddress.Parse(parts[2].Split('=')[1]);
            packet.DestAddress = IPAddress.Parse(parts[3].Split('=')[1]);
            packet.Port = Convert.ToUInt16(parts[4].Split('=')[1].Substring(0,3));

            return packet;
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    public override string ToString()
    {
        return $"LabelStack={LabelStack};Message={Message};Source={SourceAddress.ToString()};Destination={DestAddress.ToString()};Port={Port}";
    }

}
