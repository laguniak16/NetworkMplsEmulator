using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Router
{
    class LabelAction
    {

        public int operationId;
        public string outLabel;
        public string newLabel;
        public Socket connectingSocket = null;
        NetworkNode nd;

        public LabelAction(int _operationId, string _outLabel, string _newLabel)
        {
            operationId = _operationId;
            outLabel = _outLabel;
            newLabel = _newLabel;
        }

        public LabelAction(NetworkNode network)
        {
            nd = network;
        }

        public string RouteConnection(string packet)
        {
            List<string> labelsList = new List<string>();
            //connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var input = packet.Replace("\0", string.Empty);
            string[] pakiet = input.Split(';');

            string labels = pakiet[0].Split('=').GetValue(1).ToString();
            string[] label = labels.Split(',');

            for (int i = 0; i < label.Count(); i++)
            {
                labelsList.Add(label[i]);
            }
            //jezeli przyjdzie tylko z jedna etykieta to nie bedzie miało jak splitowac po ','
            //if (label == null) label[0] = labels;


            labelsList.Reverse();


            string port = pakiet[4].Split('=').GetValue(1).ToString();
            string newLabel = null;
            int operationID = 0;
            string operation = null;
            string newPacket = null;
            string help = null;


            for (int i = 0; i < nd.configs.Count(); i++)
            {
                if (Int32.Parse(labelsList[labelsList.Count() - 1]).Equals(nd.configs[i].inLabel) && Int32.Parse(port).Equals(nd.configs[i].inPort))
                {
                    operation = nd.configs[i].labelAction;
                    operationID = nd.configs[i].operationID;
                }
            }


            if (operation.Equals("SWAP"))
            {
                labelsList[labelsList.Count() - 1] = nd.labelsActions[operationID - 1].outLabel.ToString();
                port = nd.configs[operationID - 1].outPort.ToString();
                Console.WriteLine(port + " SW");
            }
            else if (operation.Equals("PUSH"))
            {
                labelsList[labelsList.Count() - 1] = nd.labelsActions[operationID - 1].outLabel.ToString();
                port = nd.configs[operationID - 1].outPort.ToString();
                newLabel = nd.labelsActions[operationID - 1].newLabel.ToString();
                labelsList.Add(newLabel);
                Console.WriteLine(port + " PUSH");
            }
            else if (operation.Equals("POP"))
            {
                if(labelsList.Count()>1)
                labelsList.RemoveAt(labelsList.Count() - 1);

                for (int i = 0; i < nd.configs.Count(); i++)
                {
                    if (Int32.Parse(labelsList[labelsList.Count() - 1]).Equals(nd.configs[i].inLabel) && Int32.Parse(port).Equals(nd.configs[i].inPort))
                    {
                        operation = nd.configs[i].labelAction;
                        operationID = nd.configs[i].operationID;
                    }
                }

                labelsList[labelsList.Count() - 1] = nd.labelsActions[operationID - 1].outLabel.ToString();
                port = nd.configs[operationID - 1].outPort.ToString();
                Console.WriteLine(port+" POP");
            }

            labelsList.Reverse();

            for (int i = 0; i < labelsList.Count(); i++)
            {

                if (i == labelsList.Count() - 1)
                    help += labelsList[i].ToString();
                else
                    help += labelsList[i].ToString() + ",";    
            }
            newPacket = "LabelStack=" + help + ";Message=" + pakiet[1].Split('=').GetValue(1).ToString() + ";Source=" + pakiet[2].Split('=').GetValue(1).ToString() + ";Destination=" + pakiet[3].Split('=').GetValue(1).ToString() + ";Port=" + port;
            Console.WriteLine("koniec "+newPacket);
            return newPacket;

        }
    }
}
