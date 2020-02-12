using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cloud;

namespace Cloud
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
            //ReadConfig rc = new ReadConfig(form1);
            //rc.ReadRouterConfig();

            CableCloud cc;
            try
            {
                var content = Environment.GetCommandLineArgs()[1];
                cc = new CableCloud(content, form1);
            }
            catch
            {
                cc = new CableCloud("../../../../Config/CloudConfig.xml", form1);
            }
             
            form1.Connections();
            form1.Text = "Cloud";
            //CableCloud cc = new CableCloud();
            //li.Send(form1);
            Application.Run(form1);
            //Application.Run(form1);
            
            


        }
    }
}
