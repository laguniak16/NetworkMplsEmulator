using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Router;

namespace Router
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
            Form1 form1 = new Form1();

            NetworkNode nd = new NetworkNode();
            ReadConfig rd = new ReadConfig();
            nd = rd.ReadRouterConfig();
            form1.Text = nd.name;
            Task responseTask = Task.Run(() => nd.connect(nd, form1,"127.0.0.1"));
            Task responseTask2 = Task.Run(() => nd.connectWithManager(nd,form1));

            //responseTask.ContinueWith(t => Console.WriteLine("In ContinueWith"));
            //responseTask.Wait();

           // Task.Run(() => nd.sendPacket(nd, form1, "127.0.0.1"));

            Application.Run(form1);
        }
    }
}
