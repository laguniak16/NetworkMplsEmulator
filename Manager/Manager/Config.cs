using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Manager
{
    [Serializable]
    public class Config
    {
        public int inPort;
        public string outPort;
        public int inLabel;
        public string outLabel;
        public string routerName;
        public string newLabel;
        public string labelAction;
        public int operationID;

        public List<Config> configs = new List<Config>();
        public Config(int _inPort, string _outPort, int _inLabel, string _outLabel, string _newLabel, string _labelAction, int _operationID, string _routerName)
        {
            inPort = _inPort;
            outPort = _outPort;
            inLabel = _inLabel;
            outLabel = _outLabel;
            routerName = _routerName;
            newLabel = _newLabel;
            labelAction = _labelAction;
            operationID = _operationID;
        }
        public Config() { }
        
        public void ReadConfig()
        {

            XmlTextReader reader = null;
            try
            {
                var content = Environment.GetCommandLineArgs()[1];
                reader = new XmlTextReader(content);
            }
            catch
            {
                reader = new XmlTextReader("..\\..\\..\\..\\Config\\managerConfig.xml");
            }
            
            while (reader.Read())
            {
                for (int j = 1; j < 6; j++)
                {
                    string name = reader.Name;
                    bool boo = true;
                    if (name.Equals("R" + j))
                    {
                        reader.Read();
                        int i = 1;
                        do
                        {
                            reader.Read();
                            if (reader.Name.Equals("R" + j) && reader.NodeType == XmlNodeType.EndElement) boo = false;
                            if (reader.Name.Equals("route" + i))
                            {
                                reader.Read();
                                if (reader.ToString().Length == 3) boo = false;
                                var split = reader.Value.ToString().Split(' ');
                                Config config = new Config(Int32.Parse(split[0].Split('=').GetValue(1).ToString()), split[1].Split('=').GetValue(1).ToString(), Int32.Parse(split[2].Split('=').GetValue(1).ToString()), split[3].Split('=').GetValue(1).ToString(), split[4].Split('=').GetValue(1).ToString(), split[5].Split('=').GetValue(1).ToString(), Int32.Parse(split[6].Split('=').GetValue(1).ToString()), name);
                                configs.Add(config);
                                reader.Read();
                                reader.Read();
                            }
                            i++;
                        } while (boo);
                        reader.Read();
                        reader.Read();
                        //reader.Read();
                    }
                }
            }
            //reader.Close();
            foreach (var i in configs)
            {
                Console.WriteLine(i.routerName + " | " + i.inLabel + " | " + i.outLabel + " | " + i.inPort + " | " + i.outPort);
            }
        }

        public string cout(int i)
        {
            string tmp = "     " + configs[i].inPort + "               " + configs[i].inLabel + "               " + configs[i].outPort + "               " + configs[i].outLabel +"               " + configs[i].newLabel +"               " + configs[i].labelAction + "               " + configs[i].operationID;// jest po 15 spacji
            return tmp;
        }
        public void delete(Config config)
        {
            Config cnf_toBeRemoved = null;
            foreach (Config cnf in configs)
            {
                if (cnf.inPort == config.inPort && cnf.outPort == config.outPort && cnf.inLabel == config.inLabel && cnf.outLabel == config.outLabel && cnf.newLabel == config.newLabel && cnf.operationID == config.operationID && cnf.routerName == config.routerName)
                    cnf_toBeRemoved = cnf;

            }
            configs.Remove(cnf_toBeRemoved);
        }
    }
}
