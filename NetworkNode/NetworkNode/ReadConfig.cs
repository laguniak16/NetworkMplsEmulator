using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Router
{
    class ReadConfig
    {
        int[] table = new int[4];
        string[] table2 = new string[3];
        string[] table3 = new string[4];
        string[] table4 = new string[3];


        public NetworkNode ReadRouterConfig()
        {
            XmlTextReader reader = null;
            try
            {
                var content = Environment.GetCommandLineArgs()[1];
                reader = new XmlTextReader(content);
            }
            catch
            {
                reader = new XmlTextReader("..\\..\\..\\..\\Config\\config3.xml");
            }
            //var content = Environment.GetCommandLineArgs()[1];
            //XmlTextReader reader = new XmlTextReader("..\\..\\..\\..\\Config\\config1.xml");
            //XmlTextReader reader = new XmlTextReader(content);

            //var content = Environment.GetCommandLineArgs()[1];
           // XmlTextReader reader = new XmlTextReader("..\\..\\..\\..\\Config\\config1.xml");
            //XmlTextReader reader = new XmlTextReader(content);

            while (reader.Read())
            {

                if (reader.Name.Equals("managerIP"))
                {
                    reader.Read();
                    table2[0] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("managerPort"))
                {
                    reader.Read();
                    Int32.TryParse(reader.Value, out table[0]);
                    reader.Read();

                }
                else if (reader.Name.Equals("nodeName"))
                {
                    reader.Read();
                    table2[1] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("cloudIP"))
                {
                    reader.Read();
                    table2[2] = reader.Value;
                    reader.Read();
                }
                else if (reader.Name.Equals("cloudPort"))
                {
                    reader.Read();
                    Int32.TryParse(reader.Value, out table[1]);
                    reader.Read();
                }

            }
            //reader.Close();
            NetworkNode nd = new NetworkNode(table2[1], table2[0], table[0], table2[2], table[1]);
            return nd;
        }

       
    }
}
