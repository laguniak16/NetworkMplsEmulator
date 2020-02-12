

using Manager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Manager
{
    public partial class Form1 : Form
    {
        Config config;
        ReceivePacket rc;
        Manager man;
        Dictionary<string, ListBox> namesToListBox;
        public Form1(Manager man1)
        {
            man = man1;
            InitializeComponent();
            button2.Enabled = false;

            namesToListBox = new Dictionary<string, ListBox>()
                    {
                        {"R1",listBox2},
                        {"R2",listBox3},
                        {"R3",listBox4},
                        {"R4",listBox5},
                        {"R5",listBox6}

                    };


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void Data(string data1)
        {

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
        public void getConfig(Config config1)
        {
            config = config1;
            for (int i = 0; i < config.configs.Count(); i++)
            {
                foreach (var item in namesToListBox)
                {
                    if (item.Key == config.configs[i].routerName)
                        namesToListBox[item.Key].Items.Add(config.cout(i));
                }
            }
        }



        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {//DELETE



            string str = namesToListBox[tabControl1.SelectedTab.Text].SelectedItem.ToString();
            str = str.Trim();
            var strArr = str.Replace("               ", " ").Split(' ');
            int tmp1 = namesToListBox[tabControl1.SelectedTab.Text].SelectedIndex;
            namesToListBox[tabControl1.SelectedTab.Text].Items.RemoveAt(tmp1);

            Config cnf = new Config(Int32.Parse(strArr[0]), strArr[2], Int32.Parse(strArr[1]), strArr[3], strArr[4], strArr[5], Int32.Parse(strArr[6]), tabControl1.SelectedTab.Text);
            config.delete(cnf);


            button2.Enabled = false;

            StringBuilder sb = new StringBuilder();

            string name = tabControl1.SelectedTab.Text;

            for (int i = 0; i < config.configs.Count; i++)
            {
                sb.Append(config.configs[i].inLabel + " " + config.configs[i].outLabel + " " + config.configs[i].inPort + " " + config.configs[i].outPort + " " + config.configs[i].routerName + " " + config.configs[i].newLabel + " " + config.configs[i].labelAction + " " + config.configs[i].operationID + "  ");
            }
            man.SendNewTable(name, sb.ToString());

        }
        private void button1_Click(object sender, EventArgs e)
        {//ADD
            int number = 0;
            if (int.TryParse(textBox1.Text.Trim(), out number) && int.TryParse(textBox2.Text.Trim(), out number) && int.TryParse(textBox3.Text.Trim(), out number) && int.TryParse(textBox4.Text.Trim(), out number))
            {
                int index = 0;

                //textBox value is a number; 3 cyfry 2 cyfry 3 cyfry 2 cyfry
                if (textBox1.TextLength > 2 && textBox2.TextLength > 1 && textBox3.TextLength > 2 && textBox4.TextLength > 1)
                {
                    if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage1"])//your specific tabname
                    {

                        listBox2.Items.Add("     " + textBox1.Text + "               " + textBox2.Text + "               " + textBox3.Text + "               " + textBox4.Text + "               " + textBox5.Text + "               " + textBox6.Text + "               " + textBox7.Text);
                        index = listBox2.Items.Count;
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage2"])//your specific tabname
                    {
                        listBox3.Items.Add("     " + textBox1.Text + "               " + textBox2.Text + "               " + textBox3.Text + "               " + textBox4.Text + "               " + textBox5.Text + "               " + textBox6.Text + "               " + textBox7.Text);
                        index = listBox2.Items.Count + listBox3.Items.Count;
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage3"])//your specific tabname
                    {
                        listBox4.Items.Add("     " + textBox1.Text + "               " + textBox2.Text + "               " + textBox3.Text + "               " + textBox4.Text + "               " + textBox5.Text + "               " + textBox6.Text + "               " + textBox7.Text);
                        index = listBox2.Items.Count + listBox3.Items.Count + listBox4.Items.Count;
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage4"])//your specific tabname
                    {
                        listBox5.Items.Add("     " + textBox1.Text + "               " + textBox2.Text + "               " + textBox3.Text + "               " + textBox4.Text + "               " + textBox5.Text + "               " + textBox6.Text + "               " + textBox7.Text);
                        index = listBox2.Items.Count + listBox3.Items.Count + listBox4.Items.Count + listBox5.Items.Count;
                    }
                    else if (tabControl1.SelectedTab == tabControl1.TabPages["tabPage5"])//your specific tabname
                    {
                        listBox6.Items.Add("     " + textBox1.Text + "               " + textBox2.Text + "               " + textBox3.Text + "               " + textBox4.Text + "               " + textBox5.Text + "               " + textBox6.Text + "               " + textBox7.Text);
                        index = listBox2.Items.Count + listBox3.Items.Count + listBox4.Items.Count + listBox5.Items.Count + listBox6.Items.Count;
                    }

                    Config config1 = new Config(Int32.Parse(textBox1.Text), textBox3.Text.ToString(), Int32.Parse(textBox2.Text.ToString()), textBox4.Text.ToString(), textBox5.Text, textBox6.Text, Int32.Parse(textBox7.Text.ToString()), tabControl1.SelectedTab.Text);
                    config.configs.Add(config1);
                }
            }
            else
            {
                MessageBox.Show("Wpisana wartosc musi byc liczba");
            }


            StringBuilder sb = new StringBuilder();
            string name = tabControl1.SelectedTab.Text;
            int howmany = config.configs.Count();
            //Config cfg = new Config(Int32.Parse(textBox1.Text), textBox3.Text, Int32.Parse(textBox2.Text), textBox4.Text, "-", "SWAP", howmany+1, name);
            //config.configs.Add(cfg);

            for (int i = 0; i < config.configs.Count; i++)
            {
                sb.Append(config.configs[i].inLabel + " " + config.configs[i].outLabel + " " + config.configs[i].inPort + " " + config.configs[i].outPort + " " + config.configs[i].routerName + " " + config.configs[i].newLabel + " " + config.configs[i].labelAction + " " + config.configs[i].operationID + "  ");
            }

            man.SendNewTable(name, sb.ToString());



        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            button2.Enabled = true;
        }

        public void getReceiver(ReceivePacket rc1)
        {
            rc = rc1;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }

}

