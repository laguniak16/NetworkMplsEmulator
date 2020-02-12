using Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace Client
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer t;

        string tmp;
        string tmp1;
        bool whatIf = false;
        //string[] tmp3 = null;
        List<String> list = new List<String>();
        ItemCheckEventArgs itemcheck1;
        ItemCheckEventArgs itemcheck2;

        //ConnectWithCloud connectWithCloud = new ConnectWithCloud();
        public ClientNode _client;

        public Form1(ClientNode client)
        {
          
            _client = client;
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void Data(string data1)
        {

            //textBox6.AppendText(data1);
            //listBox2.BeginUpdate();

            listBox1.Invoke(new Action(delegate ()
            {
                listBox1.DrawMode = DrawMode.OwnerDrawVariable;
                listBox1.MeasureItem += lst_MeasureItem;
                listBox1.DrawItem += lst_DrawItem;
                listBox1.Items.Add(data1);

            }));
        }
        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(listBox1.Items[e.Index].ToString(), listBox1.Font, listBox1.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        private void button1_Click(object sender, EventArgs e)
        {


            t = new System.Windows.Forms.Timer();
            //button1.Enabled = false;
            button2.Enabled = true;
            //checkedListBox1.Enabled = false;
            //checkedListBox2.Enabled = false;
            if (textBox1.Text == null)
                textBox1.Enabled = false;
            if (textBox1.Text != null)
            {
                tmp = textBox1.Text;
                textBox1.Enabled = true;
            }
            // zeby startowalo z zaznaczonym
            tmp1 = checkedListBox1.SelectedItem.ToString();
            
            string[] tmp2 = tmp1.Split(' ');
            float time = Convert.ToSingle(tmp2[0]);
            float time2 = time * 1000;
            tmp1 = checkedListBox2.SelectedItem.ToString();

            if (whatIf)
            {
                _client.send(tmp.ToString(), list[0]);
                _client.send(tmp.ToString(), list[1]);
            }else _client.send(tmp.ToString(), tmp1);
           
            t.Interval = (int)Math.Round(time2);

            t.Tick += new EventHandler(timer_Tick);
            t.Start();





            //MessageBox.Show(time.ToString(),tmp1);




            //_client.send(tmp.ToString());





        }

        private void button2_Click(object sender, EventArgs e)
        {
            //button2.Enabled = false;
            //button1.Enabled = true;
            checkedListBox1.Enabled = true;
            checkedListBox2.Enabled = true;
            t.Stop();
            t.Dispose();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck_1(object sender, ItemCheckEventArgs e)
        {

            if (e.NewValue == CheckState.Checked)
                for (int ix = 0; ix < checkedListBox1.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox1.SetItemChecked(ix, false);

            itemcheck1 = e;
            allChecked();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void hostsIp(string[] hostip)
        {
            checkedListBox2.Items.Clear();
            for(int i=0;i<hostip.Length;i++)
                {
                if(hostip[i]!=null)
                checkedListBox2.Items.Add(hostip[i]);
                }

        }

        private void checkedListBox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                for (int ix = 0; ix < checkedListBox2.Items.Count; ++ix)
                    if (e.Index != ix) checkedListBox2.SetItemChecked(ix, false);
                
            }

            itemcheck2 = e;
            allChecked();
        }

       private void allChecked()
        {
            if (itemcheck1 != null && itemcheck2 != null)
            {
                if (itemcheck1.NewValue == CheckState.Checked && itemcheck2.NewValue == CheckState.Checked)
                {
                    if (button2.Enabled == false)
                        button1.Enabled = true;
                }
                else
                {
                    //button1.Enabled = false;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public string messageContent(String message)
        {

            return message;
        }
  

        void timer_Tick(object sender, EventArgs e)
        {
            _client.send(tmp.ToString(),tmp1);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            list.Add(checkedListBox2.SelectedItem.ToString());
            whatIf = true;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            list.Clear();
            whatIf = false;
        }
    }
}
