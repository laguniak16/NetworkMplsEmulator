using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Client
{
    class ReadConfig
    {
        int[] table = new int[4];
        string[] table2 = new string[3];
        string[] table3 = new string[4];
        string[] table4 = new string[3];


        public ClientNode ReadHostConfig()
        {
            XmlTextReader reader = null;
            try{
                var content = Environment.GetCommandLineArgs()[1];            
                reader = new XmlTextReader(content);
            }catch{
                reader = new XmlTextReader("..\\..\\..\\..\\Config\\host1.xml");
            }
            
           //    var content = Environment.GetCommandLineArgs()[1];            
            //   XmlTextReader reader = new XmlTextReader(content);
           //XmlTextReader reader = new XmlTextReader("..\\..\\..\\..\\Config\\host2.xml");

                while (reader.Read())
            {
                if (reader.Name.Equals("Host1"))
                {
                    reader.Read();
                    table3[0] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("Host2"))
                {
                    reader.Read();
                    table3[1] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("Host3"))
                {
                    reader.Read();
                    table3[2] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("Host4"))
                {
                    reader.Read();
                    table3[3] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("cloudPort"))
                {
                    reader.Read();
                    Int32.TryParse(reader.Value, out table[2]);
                    reader.Read();
                }
                else if (reader.Name.Equals("hostName"))
                {
                    reader.Read();
                    table4[0] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("addressIP"))
                {
                    reader.Read();
                    Console.WriteLine(reader.Value);
                    table4[1] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("outPort"))
                {
                    reader.Read();
                    Int32.TryParse(reader.Value, out table[3]);
                    reader.Read();
                }
                else if (reader.Name.Equals("cloudIP"))
                {
                    reader.Read();
                    table4[2] = reader.Value;
                    reader.Read();
                }
            }
            ClientNode cn = new ClientNode(table4[2], table[2], table4[1], table4[0], table[3]);
            cn.fillTable(table3);
            reader.Close();
            return cn;
            
        }
    }
}
