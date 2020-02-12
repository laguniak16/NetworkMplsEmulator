using Cloud;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cloud
{
    public partial class Form1 : Form
    {
        string data;
        string data2;
        int selected = 0;

        public Form1()
        {
            InitializeComponent();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           // listBox3.Items.RemoveAt(1);

        }

        private void listBox3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string a = listBox3.SelectedItem.ToString();
            string status = a.Split(' ').GetValue(74).ToString();
            string port2 = a.Split(' ').GetValue(59).ToString();
            string node2 = a.Split(' ').GetValue(42).ToString();
            string port1 = a.Split(' ').GetValue(25).ToString();
            string node1 = a.Split(' ').GetValue(6).ToString();
            
            
            
            selected = listBox3.SelectedIndex;
            // MessageBox.Show(port2);
            if (status == "RUNNING")
            {
                status = "DEAD";
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                listBox3.Items.Insert(listBox3.SelectedIndex + selected + 1, "      " + node1 + "                   " + port1 + "                 " + node2 + "                 " + port2 + "               " + status);
                CableCloudConfig.StatusOfCableBetweenNodes[node1 + ":" + port1 + "-" + node2 + ":" + port2] = "DEAD";
            }
            else
            {
                status = "RUNNING";
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                listBox3.Items.Insert(listBox3.SelectedIndex + selected + 1, "      " + node1 + "                   " + port1 + "                 " + node2 + "                 " + port2 + "               " + status);
                CableCloudConfig.StatusOfCableBetweenNodes[node1 + ":" + port1 + "-" + node2 + ":" + port2] = "RUNNING";
            }
        }
        public void Data(string data1)
        {
            listBox2.Invoke(new Action(delegate ()
            {
                listBox2.DrawMode = DrawMode.OwnerDrawVariable;
                listBox2.MeasureItem += lst_MeasureItem;
                listBox2.DrawItem += lst_DrawItem;
                listBox2.Items.Add(data1);
                
            }));
        }
     
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        
        }

        private void listBox2_MouseMove(object sender, MouseEventArgs e)
        {
        }
        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        { 


        }

        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(listBox2.Items[e.Index].ToString(), listBox2.Font, listBox2.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(listBox2.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        public void Connections()
        {
            // for(int i =0;i< Listener.connections.Count;i++)
            //   listBox3.Items.Add("      "+Listener.connections[i].node1 + "                   " + Listener.connections[i].port1 + "                 " + Listener.connections[i].node2 + "                 " + Listener.connections[i].port2 + "               " + Listener.connections[i].status);
            
            foreach (KeyValuePair<string, string> entry in CableCloudConfig.StatusOfCableBetweenNodes)
            {
                
                string items = entry.Key;
                var info = items.Split('-');
                string node1port1 = info[0];
                var tmp1 = node1port1.Split(':');
                string node1 = tmp1[0];
                string port1 = tmp1[1];
                string node2port2 = info[1];
                tmp1 = node2port2.Split(':');
                string node2 = tmp1[0];
                string port2 = tmp1[1];
                string status = entry.Value;
                
                
                    listBox3.Items.Add("      " + node1 + "                   " + port1 + "                 " + node2 + "                 " + port2 + "               " + status);
                    
               
            }
        }


    }
}
