using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cloud
{
    class ReadConfig
    {
        Form1 form;
        public ReadConfig(Form1 form1)
        {
            form = form1;
        }
        public void ReadRouterConfig()
        {
            // var content = Environment.GetCommandLineArgs()[1];
            XmlTextReader reader = new XmlTextReader("..\\..\\..\\..\\Config\\Cloud.xml");
            // XmlTextReader reader = new XmlTextReader(content);
            while (reader.Read())
            {
                reader.Read();
                reader.Read();
                if (reader.Name.Equals("Config") && reader.NodeType == XmlNodeType.EndElement)
                    break;
                reader.Read();
                

                var split = reader.Value.ToString().Split(' ');
                    Connections con = new Connections(split[0].Split('=').GetValue(1).ToString(), Int32.Parse(split[1].Split('=').GetValue(1).ToString()), split[2].Split('=').GetValue(1).ToString(), Int32.Parse(split[3].Split('=').GetValue(1).ToString()), split[4].Split('=').GetValue(1).ToString());
                    Listener.connections.Add(con);
            }
            reader.Close();
        }

        
    }
}
