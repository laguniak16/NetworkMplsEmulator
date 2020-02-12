using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Client
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            ClientNode client = new ClientNode();
            
            ReadConfig rd = new ReadConfig();
            client = rd.ReadHostConfig();

            Form1 form1 = new Form1(client);
            form1.Text = client.name;
            form1.hostsIp(client.hostsIP);
            Task.Run(() => client.start(form1));

            Application.Run(form1);

        }
    }
}
