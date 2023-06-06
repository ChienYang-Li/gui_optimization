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
    public partial class Parameter : Form
    {
        public GuiMain f1 = new GuiMain();
        public double maxsize;
        public uint winrow;
        public uint wincol;
        public uint omaxiter;
        public bool success;
        public double propotion;
        public int Constantiteration;
        public int Epsilon;
        public double tabuproportion;
        public int lossweight;
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public Parameter()
        {
            InitializeComponent();
        }

        private void Parameter_Load(object sender, EventArgs e)
        {

            TxtMaxSize.Text=ini.IniReadValue("Section", "MaxSize", filenameini);
            TxtOrgWinRow.Text=ini.IniReadValue("Section", "OrgWinRow", filenameini);
            TxtOrgWinCol.Text=ini.IniReadValue("Section", "OrgWinCol", filenameini);
            TxtOmaxiter.Text=ini.IniReadValue("Section", "Omaxiter", filenameini);
            TxtPropotion.Text=ini.IniReadValue("Section", "Propotion", filenameini);
            TxtTabu.Text= ini.IniReadValue("Section", "tabuproportion", filenameini);
            //Txtepsilon.Text= ini.IniReadValue("Section", "Epsilon", filenameini);
            textBox1.Text= ini.IniReadValue("Section", "Constantiteration", filenameini);
            Txtlossweight.Text = ini.IniReadValue("Section", "LossWeight", filenameini);
            //Txtepsilon.Visible = false;
            //Txtepsilon.Visible = false;
            //label9.Visible = false;
            //ini.IniReadValue("Section", "Propotion", filenameini);

        }

        private void TxtMaxSize_TextChanged(object sender, EventArgs e)
        {

            success = double.TryParse(TxtMaxSize.Text,out double result);
            if(success==true)
            {
                maxsize = result;
            }
            else
            {

            }
                

        }

        private void TxtOrgWinRow_TextChanged(object sender, EventArgs e)
        {
            success = uint.TryParse(TxtOrgWinRow.Text, out uint result);
            if (success == true)
            {
                winrow = result;
            }
            else
            {

            }
        }

        private void TxtWinCol_TextChanged(object sender, EventArgs e)
        {
            success = uint.TryParse(TxtOrgWinCol.Text, out uint result);
            if (success == true)
            {
                wincol = result;
            }
            else
            {

            }
        }

        private void TxtOmaxiter_TextChanged(object sender, EventArgs e)
        {

            success = uint.TryParse(TxtOmaxiter.Text, out uint result);
            if (success == true)
            {
                omaxiter = result;
            }
            else
            {

            }
        }

        private void TxtPropotion_TextChanged(object sender, EventArgs e)
        {
            success = double.TryParse(TxtPropotion.Text, out double result);
            if (success == true)
            {
                if(propotion>=0&& propotion<=1)
                {
                    propotion = result;
                }
                else
                {
                    MessageBox.Show("請輸入0~1的數字","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
              
            }
            else
            {

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            success = int.TryParse(textBox1.Text, out int result);
            if (success == true)
            {
                Constantiteration = result;
            }
            else
            {

            }
        }

        private void Txtepsilon_TextChanged(object sender, EventArgs e)
        {
            //success = int.TryParse(Txtepsilon.Text, out int result);
            //if (success == true)
            //{
            //    Epsilon = result;
            //}
            //else
            //{

            //}
        }

        private void TxtTabu_TextChanged(object sender, EventArgs e)
        {
            success = double.TryParse(TxtTabu.Text, out double result);
            if (success == true)
            {
                if (tabuproportion >= 0 && tabuproportion <= 1)
                {
                    tabuproportion = result;
                }
                else
                {
                    MessageBox.Show("請輸入0~1的數字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            else
            {

            }
        }

        private void Txtlossweight_TextChanged(object sender, EventArgs e)
        {
            success = int.TryParse(Txtlossweight.Text, out int result);
            if (success == true)
            {
                lossweight = result;
            }
            else
            {

            }
        }
    }
}
