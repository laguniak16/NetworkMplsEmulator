using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manager
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

            Config config = new Config();
            config.ReadConfig();
            
            Manager man = new Manager(config);
            Form1 form1 = new Form1(man);
            form1.getConfig(config);           
            form1.Text = "Manager";
            Task.Run(() => man.Start());


            Application.Run(form1);
        }
    }
}
