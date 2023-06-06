using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace guimain
{
    public partial class LE : Form
    {
        public int le1ytext;
        public int le1xtext;
        public int le2xtext;
        public int le2ytext;
        public int le2xoffset;
        public int le2yoffset;
        public int le1xoffset;
        public int le1yoffset;
        public float le1res;
        public float le2res;
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public GuiMain f1 = new GuiMain();
        public LE()
        {
            InitializeComponent();
        }

        private void LE_Load(object sender, EventArgs e)
        {
           
            textBox1.Text = ini.IniReadValue("Section", "le1xoffset", filenameini);
            textBox2.Text = ini.IniReadValue("Section", "le1yoffset", filenameini);
            textBox3.Text = ini.IniReadValue("Section", "le2xoffset", filenameini);
            textBox4.Text = ini.IniReadValue("Section", "le2yoffset", filenameini);
            LE1Res.Text= ini.IniReadValue("Section", "le1res", filenameini);
            LE2Res.Text= ini.IniReadValue("Section", "le2res", filenameini);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                    bool success;
                    int result;
                    success = Int32.TryParse(textBox1.Text,out result);
                    if(success)
                    {
                      le1xtext = result;
                     
                    }
                   
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                bool success;
                int result;
                success = Int32.TryParse(textBox2.Text, out result);
                if (success)
                {
                    le1ytext = result;
                }
                

            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                bool success;
                int result;
                success = Int32.TryParse(textBox3.Text, out result);
                if (success)
                {
                    le2xtext = result;
                }

            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                bool success;
                int result;
                success = Int32.TryParse(textBox4.Text, out result);
                if (success)
                {
                    le2ytext = result;
                }
               
            }
        }

        private void LE1Res_TextChanged(object sender, EventArgs e)
        {
            if (LE1Res.Text != "")
            {
                bool success;
                float result;
                success = float.TryParse(LE1Res.Text, out result);
                if (success)
                {
                    le1res = result;
                }

            }
        }

        private void LE2Res_TextChanged(object sender, EventArgs e)
        {
            if (LE2Res.Text != "")
            {
                bool success;
                float result;
                success = float.TryParse(LE2Res.Text, out result);
                if (success)
                {
                    le2res = result;
                }

            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
