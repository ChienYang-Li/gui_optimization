using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace guimain
{
    public partial class Optimization : Form
    {
        public Optimization()
        {
            InitializeComponent();
        }
        public GuiMain f1 = new GuiMain();
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public string outputpath;
        public string opt_dpath, opt_ldpath;
        public string orgpath, org_dpath, org_ldpath;
        public imgclass patleft;
        public imgclass patright;
        public int ptbw;
        public bool leftselected = false;
        public bool rightselected = false;
        public uint realpercentage;
        public float rationew;
        public float rationewleft;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        public float realres = 0.125f;
        public float realresld = 1.25f;
        public bool cb1first = true;
        public bool cb2first = true;
        public bool cb3first = true;
        public int mousewheelvalue;
        public int resolutionnow;
        public bool patleftflag = false;
        public bool patrightflag = false;
        public string inputpath;
        public string outputpathxor;
        public string orgpath_xor;
        public string optpath_xor;
        public string inputpath1;
        public string inputpath2;
        public string orgpath_thd;
        public string optpath_thd;
        public string nullimage;
        public bool first = true;
        public int ptbh;
        public string threshold_XOR;
        public string xor_xor;
        public string Dosage_xor;
        public string ldfile_xor;
        public bool RB1check = false;
        public bool RB2check = false;
        public bool RB3check = false;
        public bool firstshow1 = false;
        public bool firstshow2 = false;
        public float ratiold;
        public float ratioldleft;
        public float ration1;
        public ulong mouseroix;
        public ulong mouseroiy;
        public int mousevaluex;
        public int mousevaluey;
        public bool RBrun1check=false;
        public bool RBrun2check=false;
        public bool RBrun3check=false;
        public int orgwidth = 0, orgheight = 0;
        public int rectx, recty;
        public Rectangle rectshoworg = new Rectangle();

        private void PBleft_Paint(object sender, PaintEventArgs e)
        {

            int pos0 = panel1.HorizontalScroll.Value;
            int pos1 = panel1.VerticalScroll.Value;
            int[] result = new int[4];
            //SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
             Pen DarkVioletpen = new Pen(Color.FromArgb(205, 148, 0, 211),4);
             //Pen redpen = new Pen(Color.DarkTurquoise, 4);
            if (leftselected == true)
            {
                //PBleft.Image = patleft.img;
                Tuple<int, int> showrectopt;



                string beamfile1path = GuiMain.beamfile + "spotarrays100le1_0125.h5";
                showrectopt = patleft.showoptrect(beamfile1path);



                //if (comboBox2.Text == "Dosage" || comboBox2.Text == "Dosage_opt" || comboBox2.Text == "Dosage_XOR")
                //{

                //    //result = patleft.showopt(patleft.roiarray);
                //    rectshoworg.Height = patleft.img.Height - (4 * showrectopt.Item2);
                //    rectshoworg.Width = patleft.img.Width - (4 * showrectopt.Item1);
                //    orgwidth = rectshoworg.Width;
                //    orgheight = rectshoworg.Height;
                   
                //    rectshoworg.Location = new Point(showrectopt.Item1*2, showrectopt.Item2*2);
                //    rectx = rectshoworg.X;
                //    recty = rectshoworg.Y;

                //}
                //else
                {
                    //result = patleft.showopt(patleft.roibytearray);
                    rectshoworg.Height = patleft.img.Height - (4 * showrectopt.Item2);
                    rectshoworg.Width = patleft.img.Width - (4 * showrectopt.Item1);
                    orgwidth = rectshoworg.Width;
                    orgheight = rectshoworg.Height;
                    rectshoworg.Location = new Point(showrectopt.Item1*2, showrectopt.Item2*2);
                    rectx = rectshoworg.X;
                    recty = rectshoworg.Y;
                }
                //rectshoworg.Location=









                leftselected = false;
            }

            if (PBleft.Image != null)
            {
                int[] size = new int[2];
                int[] xy = new int[2];
                size = patleft.rect_updateimg(orgwidth, orgheight, rationew);
                xy = patleft.rectlocation_update(rectx, recty, orgwidth, orgheight, rationew);
                rectshoworg.Location = new Point(xy[0], xy[1]);
                rectshoworg.Width = size[0];
                rectshoworg.Height = size[1];
                e.Graphics.DrawRectangle(DarkVioletpen, rectshoworg);
            }
            panel2.AutoScrollPosition = new Point(pos0, pos1);
        }



        private void Txtratio_TextChanged(object sender, EventArgs e)
        {
            if (Txtratio.Text != "")
            {


                float result;
                bool number = float.TryParse(Txtratio.Text, out result);
                if (number == false)
                {
                    return;
                }
                else
                {
                    float a = float.Parse(Txtratio.Text);
                    if (result < 0)
                    {
                        return;
                    }
                    int length = Math.Abs(a).ToString().Length;

                    aTimer.Interval = 500;

                    aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

                    aTimer.Enabled = true;
                    this.aTimer.SynchronizingObject = this;



                    if (length >= 2)
                    {
                        if (a > 1000)
                        {

                            a = 1000;
                            Txtratio.Text = a.ToString();
                        }
                        else if (a == 0)
                        {
                            Txtratio.Text = "";
                        }
                        else if (a > 0 && a < 10)
                        {

                            a = 10;
                            Txtratio.Text = a.ToString();
                        }
                        if (a >= 10 && a <= 1000)
                        {


                            ratio(a, realres);

                        }
                        else
                        {

                            MessageBox.Show("請輸入10~1000的數字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                    }
                    if (patleftflag == true && patleft.img != null)
                    {
                        PBleft.Height = patleft.img.Height;
                        PBleft.Width = patleft.img.Width;
                        var t = PBleft.Size;

                        t.Width = (int)(t.Width * rationew);
                        t.Height = (int)(t.Height * rationew);



                        PBleft.Size = t;

                    }
                    if (patrightflag == true && patright.img != null)
                    {
                        PBright.Height = patright.img.Height;
                        PBright.Width = patright.img.Width;
                        var t1 = PBright.Size;

                        t1.Width = (int)(t1.Width * rationew);
                        t1.Height = (int)(t1.Height * rationew);


                        PBright.Size = t1;

                    }

                }





            }
            else
            {
                Txtratio.Text = "";
            }
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            float result;
            bool success;
            success = float.TryParse(Txtratio.Text, out result);
            float a = 0;
            if (success == true)
            {
                a = result;
                int length = Math.Abs(a).ToString().Length;
                if (length == 1)
                {
                    a = 10;
                    Txtratio.Text = a.ToString();
                    //ratio(a, realres);
                }

            }


            aTimer.Enabled = false;
        }
        public void ratio(float ratio, float realres)
        {
            rationew = ratio / 100;
            if (patleftflag == true)
            {
                patleft.ratio = rationew;
                patleft.rect_updateimg(rectshoworg.Width, rectshoworg.Height, rationew);
                //PBleft.Invalidate();



            }
            if (patrightflag == true)
            {
                patright.ratio = rationew;
                //patright.rect_updateimg(rectshoworg.Width, rectshoworg.Height, rationew);
                //PBright.Invalidate();
            }

            float resnew = realres / rationew;
            Txtresolution.Text = Convert.ToString(resnew);
            PBleft.Invalidate();
            PBright.Invalidate();




        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                return;
            }
            if (!(RB1check == true || RB2check == true || RB3check == true))
            {


                MessageBox.Show("請先選擇一階、二階或三階", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);



                comboBox1.Text = "";
                //return;
            }
            patrightflag = true;
            if (RB1.Checked || RB2.Checked || RB3.Checked)
            {
                if (comboBox1.Text == "Original")
                {
                    rightselected = true;
                    //orgpath += "targetroi_1stage.h5";
                    patright = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }

                }
                else if (comboBox1.Text == "Dosage")
                {
                    rightselected = true;
                    //org_dpath += "orgd_1stage.h5";
                    patright = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if(patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }


                }
                else if (comboBox1.Text == "Dosage_opt")
                {
                    rightselected = true;
                    //opt_dpath += "optd_1stage.h5";
                    patright = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    // else
                    {
                        //PBright.Height = 0;
                        // PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }
                }
                else if (comboBox1.Text == "LDFile")
                {
                    rightselected = true;
                    //org_ldpath += "orgld_1stage.h5";

                    patright = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //Txtresolution.Text = Convert.ToString(realresld);
                    // Txtratio.Text = Convert.ToString(10);
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                      

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }
                }
                else if (comboBox1.Text == "LDFile_opt")
                {
                    rightselected = true;
                    //opt_ldpath += "optld_1stage.h5";
                    Txtresolution.Text = Convert.ToString(realresld);
                    //Txtratio.Text = Convert.ToString(10);
                    patright = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    // if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }
                }
                else if (comboBox1.Text == "Threshold_Org")
                {
                    rightselected = true;
                    patright = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }
                    //PBleft.Height = patleft.img.Height;
                    //PBleft.Width = patleft.img.Width;
                    //PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                }
                else if (comboBox1.Text == "Threshold_Opt")
                {
                    rightselected = true;
                    patright = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        

                    }
                    //else
                    {
                        //PBright.Height = 0;
                        //PBright.Width = 0;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        //PBright.Invalidate();


                    }
                }
                else if (comboBox1.Text == "XOR_Org")
                {
                    rightselected = true;
                    patright = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                    //ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                }
                else if (comboBox1.Text == "XOR_Opt")
                {
                    rightselected = true;
                    patright = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {

                        

                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                    //ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                }
                else if (comboBox1.Text == "Threshold_XOR")
                {
                    rightselected = true;
                    patright = new imgclass(threshold_XOR, "Threshold_XOR", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                        

                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                }
                else if (comboBox1.Text == "XOR_XOR")
                {
                    rightselected = true;
                    patright = new imgclass(xor_xor, "XOR_XOR", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                      
                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                }
                else if (comboBox1.Text == "Dosage_XOR")
                {
                    rightselected = true;
                    patright = new imgclass(Dosage_xor, "Dosage_XOR", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                }
                else if (comboBox1.Text == "LD_XOR")
                {
                    rightselected = true;
                    //Txtresolution.Text = Convert.ToString(realresld);
                    //Txtratio.Text = Convert.ToString(10);
                    patright = new imgclass(ldfile_xor, "LD_XOR", ptbh, ptbw, 1, 2);
                    if (patright.img != null)
                    {
                        PBright.Image = patright.img;
                        PBright.Invalidate();
                    }
                    //if (patright.img != null)
                    {
                        //PBright.Height = patright.img.Height;
                        //PBright.Width = patright.img.Width;
                        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                       

                    }
                    //else
                    //{
                    //    PBright.Height = 0;
                    //    PBright.Width = 0;
                    //    PBright.SizeMode = PictureBoxSizeMode.Zoom;
                    //    PBright.Invalidate();


                    //}
                }



                if (patright.img == null)
                {
                    return;
                }
                //if (comboBox1.Text == "LDFile" || comboBox1.Text == "LDFile_opt" || comboBox1.Text == "LD_XOR")
                {

                }
                //else
                {
                    if (patrightflag == true)
                    {
                        PBright.Height = patright.img.Height;
                        PBright.Width = patright.img.Width;
                        var t1 = PBright.Size;

                        t1.Width = (int)(t1.Width * rationew);
                        t1.Height = (int)(t1.Height * rationew);


                        PBright.Size = t1;

                    }

                }

            }
            Txtresolution.Focus();
        }

        private void PBright_Paint(object sender, PaintEventArgs e)
        {
            int pos0 = panel2.HorizontalScroll.Value;
            int pos1 = panel2.VerticalScroll.Value;
            int[] result = new int[4];
            Pen DarkVioletpen = new Pen(Color.FromArgb(205, 148, 0, 211), 4);
            if (rightselected == true)
            {
                //PBright.Image = patright.img;
                Tuple<int, int> showrectopt;



                string beamfile1path = GuiMain.beamfile + "spotarrays100le1_0125.h5";
                showrectopt = patright.showoptrect(beamfile1path);



                //if (comboBox2.Text == "Dosage" || comboBox2.Text == "Dosage_opt" || comboBox2.Text == "Dosage_XOR")
                //{

                //    rectshoworg.Height = patright.img.Height - (2 * showrectopt.Item2);
                //    rectshoworg.Width = patright.img.Width - (2 * showrectopt.Item1);
                //    orgwidth = rectshoworg.Width;
                //    orgheight = rectshoworg.Height;

                //    rectshoworg.Location = new Point(showrectopt.Item1, showrectopt.Item2);
                //    rectx = rectshoworg.X;
                //    recty = rectshoworg.Y;

                //}
                //else
                {
                    rectshoworg.Height = patright.img.Height - (4 * showrectopt.Item2);
                    rectshoworg.Width = patright.img.Width - (4 * showrectopt.Item1);
                    orgwidth = rectshoworg.Width;
                    orgheight = rectshoworg.Height;
                    rectshoworg.Location = new Point(showrectopt.Item1*2, showrectopt.Item2*2);
                    rectx = rectshoworg.X;
                    recty = rectshoworg.Y;
                }
                //rectshoworg.Location=

                rightselected = false;
            }
            if (PBright.Image != null)
            {
                int[] size = new int[2];
                int[] xy = new int[2];
                xy = patright.rectlocation_update(rectx, recty, orgwidth, orgheight, rationew);
                rectshoworg.Location = new Point(xy[0], xy[1]);
                size = patright.rect_updateimg(orgwidth, orgheight, rationew);
                rectshoworg.Width = size[0];
                rectshoworg.Height = size[1];

                e.Graphics.DrawRectangle(DarkVioletpen, rectshoworg);
            }
            panel1.AutoScrollPosition = new Point(pos0, pos1);
        }

        private void CB1_CheckedChanged(object sender, EventArgs e)
        {
            //if (CB1.Checked)
            //{

            //    orgpath = outputpath + "optimization\\targetroi_1stage.h5";
            //    opt_dpath = outputpath + "optimization\\optd_1stage.h5";
            //    opt_ldpath = outputpath + "optimization\\optld_1stage.h5";
            //    org_dpath = outputpath + "optimization\\orgd_1stage.h5";
            //    org_ldpath = outputpath + "optimization\\orgld_1stage.h5";

            //    //orgpath += "targetroi_1stage.h5";
            //    //org_dpath += "orgd_1stage.h5";
            //    //opt_dpath += "optd_1stage.h5";
            //    //org_ldpath += "orgld_1stage.h5";
            //    //opt_ldpath += "optld_1stage.h5";
            //    //    cb1first = false;
            //    //}

            //}


        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CB2_CheckedChanged(object sender, EventArgs e)
        {
            //if (CB2.Checked)
            //{
            //    //if (cb2first == true)
            //    //{
            //    //    orgpath += "targetroi_2stage.h5";
            //    //    org_dpath += "orgd_2stage.h5";
            //    //    opt_dpath += "optd_2stage.h5";
            //    //    org_ldpath += "orgld_2stage.h5";
            //    //    opt_ldpath += "optld_2stage.h5";
            //    //    cb2first = false;
            //    //}
            //    opt_dpath = outputpath + "optimization\\optd_2stage.h5";
            //    opt_ldpath = outputpath + "optimization\\optld_2stage.h5";
            //    orgpath = outputpath + "optimization\\targetroi_2stage.h5";
            //    org_dpath = outputpath + "optimization\\orgd_2stage.h5";
            //    org_ldpath = outputpath + "optimization\\orgld_2stage.h5";
            //}
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void CB3_CheckedChanged(object sender, EventArgs e)
        {
            //if (CB3.Checked)
            //{

            //    opt_dpath = outputpath + "optimization\\optd_3stage.h5";
            //    opt_ldpath = outputpath + "optimization\\optld_3stage.h5";
            //    orgpath = outputpath + "optimization\\targetroi_3stage.h5";
            //    org_dpath = outputpath + "optimization\\orgd_3stage.h5";
            //    org_ldpath = outputpath + "optimization\\orgld_3stage.h5";
            //}
        }

        private void RB1_CheckedChanged(object sender, EventArgs e)
        {
            if (RB1.Checked)
            {
                RB1check = true;

                orgpath = outputpath + "optimization\\targetroi_1stage.h5";
                opt_dpath = outputpath + "optimization\\optd_1stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_1stage.h5";
                org_dpath = outputpath + "optimization\\orgd_1stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_1stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_1stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_1stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_1stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_1stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_1stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_1stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_1stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_1stage.h5";
                PBleft.Refresh();
                PBright.Refresh();

                //if (comboBox2.Text != "")
                //{
                //    patleftflag = true;
                //    if (comboBox2.Text == "Original")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "orgpath", orgpath.ToString(), filenameini);


                //    }
                //    else if (comboBox2.Text == "Dosage")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "org_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Dosage_opt")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "opt_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);

                //        //ini.IniWriteValue("Section", "org_ldpath", org_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile_opt")
                //    {
                //        leftselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patleft = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);

                //        //ini.IniWriteValue("Section", "opt_ldpath", opt_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "orgpath_thd", orgpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "optpath_thd", optpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        //ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    else if(comboBox2.Text == "Threshold_XOR")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(threshold_XOR, "XOR_Opt", ptbh, ptbw, 1, 2);
                //    }
                //    else if (comboBox2.Text == "XOR_XOR")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(xor_xor, "XOR_XOR", ptbh, ptbw, 1, 2);
                //        //PBright.Invalidate();
                //    }
                //    else if(comboBox2.Text == "Dosage_XOR")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(Dosage_xor, "Dosage_XOR", ptbh, ptbw, 1, 2);
                //    }
                //    else if(comboBox2.Text == "LD_XOR")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(ldfile_xor, "LD_XOR", ptbh, ptbw, 1, 2);
                //    }


                //    if (patleft.img == null)
                //    {
                //        return;
                //    }
                //    if (patleftflag == true)
                //    {
                //        PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBleft.Height = patleft.img.Height;
                //        PBleft.Width = patleft.img.Width;
                //        var t = PBleft.Size;

                //        t.Width = (int)(t.Width * rationew);
                //        t.Height = (int)(t.Height * rationew);



                //        PBleft.Size = t;
                //        PBleft.Invalidate();
                //    }

                //}
                //if (comboBox1.Text != "")
                //{
                //    patrightflag = true;
                //    if (comboBox1.Text == "Original")
                //    {
                //        rightselected = true;
                //        //orgpath += "targetroi_1stage.h5";
                //        patright = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();

                //    }
                //    else if (comboBox1.Text == "Dosage")
                //    {
                //        rightselected = true;
                //        //org_dpath += "orgd_1stage.h5";
                //        patright = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Dosage_opt")
                //    {
                //        rightselected = true;
                //        //opt_dpath += "optd_1stage.h5";
                //        patright = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile")
                //    {
                //        rightselected = true;
                //        //org_ldpath += "orgld_1stage.h5";
                //        patright = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile_opt")
                //    {
                //        rightselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patright = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Threshold_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //        //PBleft.Height = patleft.img.Height;
                //        //PBleft.Width = patleft.img.Width;
                //        //PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //    }
                //    else if (comboBox1.Text == "Threshold_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "XOR_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //        //ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox1.Text == "XOR_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //        //ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox1.Text == "Threshold_XOR")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(threshold_XOR, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        //PBright.Height = patright.img.Height;
                //        //PBright.Width = patright.img.Width;
                //        //PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "XOR_XOR")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(xor_xor, "XOR_XOR", ptbh, ptbw, 1, 2);
                //        //PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Dosage_XOR")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(Dosage_xor, "Dosage_XOR", ptbh, ptbw, 1, 2);
                //    }
                //    else if (comboBox1.Text == "LD_XOR")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(ldfile_xor, "LD_XOR", ptbh, ptbw, 1, 2);
                //    }


                //    if (PBright.Image == null)
                //    {
                //        return;
                //    }
                //    if (patrightflag == true)
                //    {
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        var t1 = PBright.Size;

                //        t1.Width = (int)(t1.Width * rationew);
                //        t1.Height = (int)(t1.Height * rationew);


                //        PBright.Size = t1;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();

                //    }
                //}
            }
            else
            {
                RB1check = false;
            }

        }

        private void RB2_CheckedChanged(object sender, EventArgs e)
        {
            if (RB2.Checked)
            {

                RB2check = true;
                //if (cb2first == true)
                //{
                //    orgpath += "targetroi_2stage.h5";
                //    org_dpath += "orgd_2stage.h5";
                //    opt_dpath += "optd_2stage.h5";
                //    org_ldpath += "orgld_2stage.h5";
                //    opt_ldpath += "optld_2stage.h5";
                //    cb2first = false;
                //}
                opt_dpath = outputpath + "optimization\\optd_2stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_2stage.h5";
                orgpath = outputpath + "optimization\\targetroi_2stage.h5";
                org_dpath = outputpath + "optimization\\orgd_2stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_2stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_2stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_2stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_2stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_2stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_2stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_2stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_2stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_2stage.h5";
                PBleft.Refresh();
                PBright.Refresh();
                //if (comboBox2.Text != "")
                //{
                //    patleftflag = true;
                //    if (comboBox2.Text == "Original")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "orgpath", orgpath.ToString(), filenameini);


                //    }
                //    else if (comboBox2.Text == "Dosage")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "org_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Dosage_opt")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "opt_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile")
                //    {
                //        leftselected = true;
                //        //org_ldpath += "orgld_1stage.h5";
                //        patleft = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);

                //        ini.IniWriteValue("Section", "org_ldpath", org_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile_opt")
                //    {
                //        leftselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patleft = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);

                //        ini.IniWriteValue("Section", "opt_ldpath", opt_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "orgpath_thd", orgpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "optpath_thd", optpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    if (patleft.img == null)
                //    {
                //        return;
                //    }
                //    if (patleftflag == true)
                //    {
                //        PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBleft.Height = patleft.img.Height;
                //        PBleft.Width = patleft.img.Width;
                //        var t = PBleft.Size;

                //        t.Width = (int)(t.Width * rationew);
                //        t.Height = (int)(t.Height * rationew);



                //        PBleft.Size = t;
                //        PBleft.Invalidate();
                //    }

                //}
                //if (comboBox1.Text != "")
                //{
                //    patrightflag = true;
                //    if (comboBox1.Text == "Original")
                //    {
                //        rightselected = true;
                //        //orgpath += "targetroi_1stage.h5";
                //        patright = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();

                //    }
                //    else if (comboBox1.Text == "Dosage")
                //    {
                //        rightselected = true;
                //        //org_dpath += "orgd_1stage.h5";
                //        patright = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Dosage_opt")
                //    {
                //        rightselected = true;
                //        //opt_dpath += "optd_1stage.h5";
                //        patright = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile")
                //    {
                //        rightselected = true;
                //        //org_ldpath += "orgld_1stage.h5";
                //        patright = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile_opt")
                //    {
                //        rightselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patright = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Threshold_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        //PBleft.Height = patleft.img.Height;
                //        //PBleft.Width = patleft.img.Width;
                //        //PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //    }
                //    else if (comboBox1.Text == "Threshold_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //    }
                //    else if (comboBox1.Text == "XOR_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox1.Text == "XOR_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    if (PBright.Image == null)
                //    {
                //        return;
                //    }
                //    if (patrightflag == true)
                //    {
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        var t1 = PBright.Size;

                //        t1.Width = (int)(t1.Width * rationew);
                //        t1.Height = (int)(t1.Height * rationew);


                //        PBright.Size = t1;

                //    }
                //}
            }
            else
            {
                RB2check = false;



            }

        }

        private void RB3_CheckedChanged(object sender, EventArgs e)
        {
            if (RB3.Checked)
            {

                RB3check = true;

                opt_dpath = outputpath + "optimization\\optd_3stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_3stage.h5";
                orgpath = outputpath + "optimization\\targetroi_3stage.h5";
                org_dpath = outputpath + "optimization\\orgd_3stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_3stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_3stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_3stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_3stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_3stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_3stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_3stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_3stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_3stage.h5";
                PBleft.Refresh();
                PBright.Refresh();
                //if (comboBox2.Text != "")
                //{
                //    patleftflag = true;
                //    if (comboBox2.Text == "Original")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "orgpath", orgpath.ToString(), filenameini);


                //    }
                //    else if (comboBox2.Text == "Dosage")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "org_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Dosage_opt")
                //    {
                //        leftselected = true;

                //        patleft = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "opt_dpath", opt_dpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile")
                //    {
                //        leftselected = true;
                //        //org_ldpath += "orgld_1stage.h5";
                //        patleft = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);

                //        ini.IniWriteValue("Section", "org_ldpath", org_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "LDFile_opt")
                //    {
                //        leftselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patleft = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);

                //        ini.IniWriteValue("Section", "opt_ldpath", opt_ldpath.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "orgpath_thd", orgpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "Threshold_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "optpath_thd", optpath_thd.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Org")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox2.Text == "XOR_Opt")
                //    {
                //        leftselected = true;
                //        patleft = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    if (patleft.img == null)
                //    {
                //        return;
                //    }
                //    if (patleftflag == true)
                //    {
                //        PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBleft.Height = patleft.img.Height;
                //        PBleft.Width = patleft.img.Width;
                //        var t = PBleft.Size;

                //        t.Width = (int)(t.Width * rationew);
                //        t.Height = (int)(t.Height * rationew);



                //        PBleft.Size = t;
                //        PBleft.Invalidate();
                //    }

                //}
                //if (comboBox1.Text != "")
                //{
                //    patrightflag = true;
                //    if (comboBox1.Text == "Original")
                //    {
                //        rightselected = true;
                //        //orgpath += "targetroi_1stage.h5";
                //        patright = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();

                //    }
                //    else if (comboBox1.Text == "Dosage")
                //    {
                //        rightselected = true;
                //        //org_dpath += "orgd_1stage.h5";
                //        patright = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Dosage_opt")
                //    {
                //        rightselected = true;
                //        //opt_dpath += "optd_1stage.h5";
                //        patright = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile")
                //    {
                //        rightselected = true;
                //        //org_ldpath += "orgld_1stage.h5";
                //        patright = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "LDFile_opt")
                //    {
                //        rightselected = true;
                //        //opt_ldpath += "optld_1stage.h5";
                //        patright = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        PBright.SizeMode = PictureBoxSizeMode.Zoom;
                //        PBright.Invalidate();
                //    }
                //    else if (comboBox1.Text == "Threshold_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                //        //PBleft.Height = patleft.img.Height;
                //        //PBleft.Width = patleft.img.Width;
                //        //PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                //    }
                //    else if (comboBox1.Text == "Threshold_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                //    }
                //    else if (comboBox1.Text == "XOR_Org")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                //    }
                //    else if (comboBox1.Text == "XOR_Opt")
                //    {
                //        rightselected = true;
                //        patright = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                //        ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                //    }
                //    if (PBright.Image == null)
                //    {
                //        return;
                //    }
                //    if (patrightflag == true)
                //    {
                //        PBright.Height = patright.img.Height;
                //        PBright.Width = patright.img.Width;
                //        var t1 = PBright.Size;

                //        t1.Width = (int)(t1.Width * rationew);
                //        t1.Height = (int)(t1.Height * rationew);


                //        PBright.Size = t1;

                //    }
                //}
            }
            else
            {
                RB3check = false;
            }

        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (!(RBrun1.Checked || RBrun2.Checked || RBrun3.Checked))
            {
                MessageBox.Show("請先選擇Run Stage中的一階、二階或三階", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            guimainCode mainfunction = new guimainCode();
            if (RBrun1.Checked)
            {
                if (ThresholdCB.Checked)
                {
                    opt_dpath = outputpath + "optimization\\optd_1stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_1stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_1stage.h5";
                    mainfunction.Threshold(org_dpath, orgpath_thd, GuiMain.threshold, GuiMain.time);
                    optpath_thd = outputpath + "optimization\\optpath_thd_1stage.h5";
                    mainfunction.Threshold(opt_dpath, optpath_thd, GuiMain.threshold, GuiMain.time);

                }
                if (XORCB.Checked)
                {
                    orgpath = outputpath + "optimization\\targetroi_1stage.h5";
                    optpath_thd = outputpath + "optimization\\optpath_thd_1stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_1stage.h5";
                    optpath_xor = outputpath + "optimization\\optpatg_xor_1stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_1stage.h5";
                    mainfunction.xor_new(orgpath, orgpath_thd, orgpath_xor);
                    mainfunction.xor_new(orgpath, optpath_thd, optpath_xor);

                }
                if (Dosage_XOR.Checked)
                {
                    opt_dpath = outputpath + "optimization\\optd_1stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_1stage.h5";
                    Dosage_xor = outputpath + "optimization\\dosage_xor_1stage.h5";
                    mainfunction.xor_new_float(opt_dpath, org_dpath, Dosage_xor);
                }
                if (Threshold_XOR.Checked)
                {
                    optpath_thd = outputpath + "optimization\\optpath_thd_1stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_1stage.h5";
                    threshold_XOR = outputpath + "optimization\\threshold_xor_1stage.h5";
                    mainfunction.xor_new(orgpath_thd, optpath_thd, threshold_XOR);

                }
                if (XOR_XOR.Checked)
                {
                    optpath_xor = outputpath + "optimization\\optpatg_xor_1stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_1stage.h5";
                    xor_xor = outputpath + "optimization\\xor_xor_1stage.h5";
                    mainfunction.xor_xor(orgpath_xor, optpath_xor, xor_xor);
                }
                if (LDFile_XOR.Checked)
                {
                    opt_ldpath = outputpath + "optimization\\optld_1stage.h5";
                    org_ldpath = outputpath + "optimization\\orgld_1stage.h5";
                    ldfile_xor = outputpath + "optimization\\ldfile_xor_1stage.h5";
                    mainfunction.xor_new(org_ldpath, opt_ldpath, ldfile_xor);

                }
            }
            else if (RBrun2.Checked)
            {
                if (ThresholdCB.Checked)
                {
                    opt_dpath = outputpath + "optimization\\optd_2stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_2stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_2stage.h5";
                    mainfunction.Threshold(org_dpath, orgpath_thd, GuiMain.threshold, GuiMain.time);
                    optpath_thd = outputpath + "optimization\\optpath_thd_2stage.h5";
                    mainfunction.Threshold(opt_dpath, optpath_thd, GuiMain.threshold, GuiMain.time);
                }
                if (XORCB.Checked)
                {
                    orgpath = outputpath + "optimization\\targetroi_2stage.h5";
                    optpath_thd = outputpath + "optimization\\optpath_thd_2stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_2stage.h5";
                    optpath_xor = outputpath + "optimization\\optpatg_xor_2stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_2stage.h5";
                    mainfunction.xor_new(orgpath, orgpath_thd, orgpath_xor);
                    mainfunction.xor_new(orgpath, optpath_thd, optpath_xor);

                }
                if (Dosage_XOR.Checked)
                {
                    opt_dpath = outputpath + "optimization\\optd_2stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_2stage.h5";
                    Dosage_xor = outputpath + "optimization\\dosage_xor_2stage.h5";
                    mainfunction.xor_new_float(opt_dpath, org_dpath, Dosage_xor);
                }
                if (Threshold_XOR.Checked)
                {
                    optpath_thd = outputpath + "optimization\\optpath_thd_2stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_2stage.h5";
                    threshold_XOR = outputpath + "optimization\\threshold_xor_2stage.h5";
                    mainfunction.xor_new(orgpath_thd, optpath_thd, threshold_XOR);

                }
                if (XOR_XOR.Checked)
                {
                    optpath_xor = outputpath + "optimization\\optpatg_xor_2stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_2stage.h5";
                    xor_xor = outputpath + "optimization\\xor_xor_1stage.h5";
                    mainfunction.xor_xor(orgpath_xor, optpath_xor, xor_xor);
                }
                if (LDFile_XOR.Checked)
                {
                    opt_ldpath = outputpath + "optimization\\optld_2stage.h5";
                    org_ldpath = outputpath + "optimization\\orgld_2stage.h5";
                    ldfile_xor = outputpath + "optimization\\ldfile_xor_2stage.h5";
                    mainfunction.xor_new(org_ldpath, opt_ldpath, ldfile_xor);

                }
            }
            else if (RBrun3.Checked)
            {
                if (ThresholdCB.Checked)
                {


                    orgpath_thd = outputpath + "optimization\\orgpath_thd_3stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_3stage.h5";
                    opt_dpath = outputpath + "optimization\\optd_3stage.h5";
                    mainfunction.Threshold(org_dpath, orgpath_thd, GuiMain.threshold, GuiMain.time);
                    optpath_thd = outputpath + "optimization\\optpath_thd_3stage.h5";
                    mainfunction.Threshold(opt_dpath, optpath_thd, GuiMain.threshold, GuiMain.time);
                }
                if (XORCB.Checked)
                {
                    orgpath = outputpath + "optimization\\targetroi_3stage.h5";
                    optpath_thd = outputpath + "optimization\\optpath_thd_3stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_3stage.h5";
                    optpath_xor = outputpath + "optimization\\optpatg_xor_3stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_3stage.h5";
                    mainfunction.xor_new(orgpath, orgpath_thd, orgpath_xor);
                    mainfunction.xor_new(orgpath, optpath_thd, optpath_xor);

                }
                if (Dosage_XOR.Checked)
                {
                    opt_dpath = outputpath + "optimization\\optd_3stage.h5";
                    org_dpath = outputpath + "optimization\\orgd_3stage.h5";
                    Dosage_xor = outputpath + "optimization\\dosage_xor_3stage.h5";
                    mainfunction.xor_new_float(opt_dpath, org_dpath, Dosage_xor);
                }
                if (Threshold_XOR.Checked)
                {
                    optpath_thd = outputpath + "optimization\\optpath_thd_3stage.h5";
                    orgpath_thd = outputpath + "optimization\\orgpath_thd_3stage.h5";
                    threshold_XOR = outputpath + "optimization\\threshold_xor_3stage.h5";
                    mainfunction.xor_new(orgpath_thd, optpath_thd, threshold_XOR);

                }
                if (XOR_XOR.Checked)
                {
                    optpath_xor = outputpath + "optimization\\optpatg_xor_3stage.h5";
                    orgpath_xor = outputpath + "optimization\\orgpath_xor_3stage.h5";
                    xor_xor = outputpath + "optimization\\xor_xor_3stage.h5";
                    mainfunction.xor_xor(orgpath_xor, optpath_xor, xor_xor);
                }
                if (LDFile_XOR.Checked)
                {
                    opt_ldpath = outputpath + "optimization\\optld_3stage.h5";
                    org_ldpath = outputpath + "optimization\\orgld_3stage.h5";
                    ldfile_xor = outputpath + "optimization\\ldfile_xor_3stage.h5";
                    mainfunction.xor_new(org_ldpath, opt_ldpath, ldfile_xor);

                }
            }

            MessageBox.Show("程式執行完成","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //ini.IniWriteValue("Section", "orgpath_thd", orgpath_thd.ToString(), filenameini);
            //ini.IniWriteValue("Section", "optpath_thd", optpath_thd.ToString(), filenameini);
            //ini.IniWriteValue("Section", "orgpath_xor", orgpath_thd.ToString(), filenameini);
            //ini.IniWriteValue("Section", "optpath_xor", optpath_thd.ToString(), filenameini);
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void PBright_Move(object sender, EventArgs e)
        {

        }

        private void PBright_MouseMove(object sender, MouseEventArgs e)
        {
            mouseroix = (ulong)e.X & 0xffff;//40000
            mouseroiy = (ulong)e.Y & 0xffff;//40000
            double ratio = Convert.ToDouble(Txtratio.Text)/100;
            int mousex = (int)(mouseroix / ratio);
            int mousey = (int)(mouseroiy / ratio);
            Txtmousex.Text = mousex.ToString();
            Txtmousey.Text = mousey.ToString();
            float value;
            if (comboBox1.Text == "Dosage" || comboBox1.Text == "Dosage_opt" || comboBox1.Text == "Dosage_XOR")
            {
                if (patright != null)
                {
                    double mouseratiox = Convert.ToDouble(Txtratio.Text);
                    mouseratiox = mouseratiox / 100;
                    mousevaluex = (int)mouseroix;//40000
                    mousevaluey = (int)mouseroiy;//40000


                    if (patright.roiarray != null)
                    {
                        mousevaluex = (int)(mousevaluex / mouseratiox);
                        mousevaluey = (int)(mousevaluey / mouseratiox);
                        if (mousevaluex < 0)
                            mousevaluex = 0;
                        if (mousevaluey < 0)
                            mousevaluey = 0;
                        if (mousevaluex > patright.roiarray.GetLength(1) - 1)
                            mousevaluex = patright.roiarray.GetLength(1) - 1;
                        if (mousevaluey > patright.roiarray.GetLength(0) - 1)
                            mousevaluey = patright.roiarray.GetLength(0) - 1;
                        value = patright.roiarray[mousevaluey, mousevaluex];
                        value = (float)(value * (GuiMain.time));
                        Txtmatrixvalue.Text = Convert.ToString(value);

                    }
                }
            }

        }

        private void PBleft_MouseMove(object sender, MouseEventArgs e)
        {
            mouseroix = (ulong)e.X & 0xffff;//40000
            mouseroiy = (ulong)e.Y & 0xffff;//40000
            double ratio = Convert.ToDouble(Txtratio.Text)/100;
            int mousex = (int)(mouseroix / ratio);
            int mousey = (int)(mouseroiy / ratio);
            Txtmousex.Text = mousex.ToString();
            Txtmousey.Text = mousey.ToString();
          
            float value;
            if (comboBox2.Text == "Dosage" || comboBox2.Text == "Dosage_opt" || comboBox2.Text == "Dosage_XOR")
            {
                if (patleft != null)
                {
                    double mouseratiox = Convert.ToDouble(Txtratio.Text);
                    mouseratiox = mouseratiox / 100;
                    mousevaluex = (int)mouseroix;//40000
                    mousevaluey = (int)mouseroiy;//40000


                    if (patleft.roiarray != null)
                    {
                        mousevaluex = (int)(mousevaluex / mouseratiox);
                        mousevaluey = (int)(mousevaluey / mouseratiox);
                        if (mousevaluex < 0)
                            mousevaluex = 0;
                        if (mousevaluey < 0)
                            mousevaluey = 0;
                        if (mousevaluex > patleft.roiarray.GetLength(1) - 1)
                            mousevaluex = patleft.roiarray.GetLength(1) - 1;
                        if (mousevaluey > patleft.roiarray.GetLength(0) - 1)
                            mousevaluey = patleft.roiarray.GetLength(0) - 1;
                        value = patleft.roiarray[mousevaluey, mousevaluex];
                        value = (float)(value * (GuiMain.time));
                        Txtmatrixvalue.Text = Convert.ToString(value);

                    }
                }
            }
        }

        private void RBrun1_CheckedChanged(object sender, EventArgs e)
        {
            if (RBrun1.Checked)
            {
                RBrun1check = true;

                orgpath = outputpath + "optimization\\targetroi_1stage.h5";
                opt_dpath = outputpath + "optimization\\optd_1stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_1stage.h5";
                org_dpath = outputpath + "optimization\\orgd_1stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_1stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_1stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_1stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_1stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_1stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_1stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_1stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_1stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_1stage.h5";
                PBleft.Refresh();
                PBright.Refresh();

      
            }
            else
            {
                RBrun1check = false;
            }
        }

        private void RBrun2_CheckedChanged(object sender, EventArgs e)
        {
            if (RBrun2.Checked)
            {

                RBrun2check = true;
                //if (cb2first == true)
                //{
                //    orgpath += "targetroi_2stage.h5";
                //    org_dpath += "orgd_2stage.h5";
                //    opt_dpath += "optd_2stage.h5";
                //    org_ldpath += "orgld_2stage.h5";
                //    opt_ldpath += "optld_2stage.h5";
                //    cb2first = false;
                //}
                opt_dpath = outputpath + "optimization\\optd_2stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_2stage.h5";
                orgpath = outputpath + "optimization\\targetroi_2stage.h5";
                org_dpath = outputpath + "optimization\\orgd_2stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_2stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_2stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_2stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_2stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_2stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_2stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_2stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_2stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_2stage.h5";
                PBleft.Refresh();
                PBright.Refresh();
              
              
            }
            else
            {
                RBrun2check = false;



            }
        }

        private void RBrun3_CheckedChanged(object sender, EventArgs e)
        {
            if (RBrun3.Checked)
            {

                RBrun3check = true;

                opt_dpath = outputpath + "optimization\\optd_3stage.h5";
                opt_ldpath = outputpath + "optimization\\optld_3stage.h5";
                orgpath = outputpath + "optimization\\targetroi_3stage.h5";
                org_dpath = outputpath + "optimization\\orgd_3stage.h5";
                org_ldpath = outputpath + "optimization\\orgld_3stage.h5";
                orgpath_thd = outputpath + "optimization\\orgpath_thd_3stage.h5";
                optpath_thd = outputpath + "optimization\\optpath_thd_3stage.h5";
                optpath_xor = outputpath + "optimization\\optpatg_xor_3stage.h5";
                orgpath_xor = outputpath + "optimization\\orgpath_xor_3stage.h5";
                Dosage_xor = outputpath + "optimization\\dosage_xor_3stage.h5";
                threshold_XOR = outputpath + "optimization\\threshold_xor_3stage.h5";
                xor_xor = outputpath + "optimization\\xor_xor_3stage.h5";
                ldfile_xor = outputpath + "optimization\\ldfile_xor_3stage.h5";
                PBleft.Refresh();
                PBright.Refresh();
              
            }
            else
            {
                RBrun3check = false;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            //leftselected = true;
            //PBleft.Invalidate();
        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
            rightselected = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text == "")
            {
                return;
            }
            if (!(RB1check == true || RB2check == true || RB3check == true))
            {
                MessageBox.Show("請先選擇一階、二階或三階", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Text = "";
                //return;
            }

            patleftflag = true;

            if (RB1.Checked || RB2.Checked || RB3.Checked)
            {
                if (comboBox2.Text == "Original")
                {
                    leftselected = true;
                    patleft = new imgclass(orgpath, "Original", ptbh, ptbw, 1, 2);
                    if(patleft.img!=null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                   
                    //ini.IniWriteValue("Section", "orgpath", orgpath.ToString(), filenameini);


                }
                else if (comboBox2.Text == "Dosage")
                {
                    leftselected = true;

                    patleft = new imgclass(org_dpath, "Dosage", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "org_dpath", opt_dpath.ToString(), filenameini);
                }
                else if (comboBox2.Text == "Dosage_opt")
                {
                    leftselected = true;

                    patleft = new imgclass(opt_dpath, "Dosage_opt", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "opt_dpath", opt_dpath.ToString(), filenameini);
                }
                else if (comboBox2.Text == "LDFile")
                {
                    leftselected = true;
                    //org_ldpath += "orgld_1stage.h5";
                    patleft = new imgclass(org_ldpath, "LDFile", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }

                    //Txtresleft.Text = Convert.ToString(realresld);
                    //Txtratioleft.Text = Convert.ToString(10);
                    //PBleft.Invalidate();
                    //ini.IniWriteValue("Section", "org_ldpath", org_ldpath.ToString(), filenameini);
                }
                else if (comboBox2.Text == "LDFile_opt")
                {
                    leftselected = true;
                    //opt_ldpath += "optld_1stage.h5";
                    //Txtresleft.Text = Convert.ToString(realresld);
                    //Txtratioleft.Text = Convert.ToString(10);
                    patleft = new imgclass(opt_ldpath, "LDFile_opt", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "opt_ldpath", opt_ldpath.ToString(), filenameini);
                }
                else if (comboBox2.Text == "Threshold_Org")
                {
                    leftselected = true;
                    patleft = new imgclass(orgpath_thd, "Threshold_Org", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "orgpath_thd", orgpath_thd.ToString(), filenameini);
                }
                else if (comboBox2.Text == "Threshold_Opt")
                {
                    leftselected = true;
                    patleft = new imgclass(optpath_thd, "Threshold_Opt", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "optpath_thd", optpath_thd.ToString(), filenameini);
                }
                else if (comboBox2.Text == "XOR_Org")
                {
                    leftselected = true;
                    patleft = new imgclass(orgpath_xor, "XOR_Org", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "XOR_Org", orgpath_xor.ToString(), filenameini);
                }
                else if (comboBox2.Text == "XOR_Opt")
                {
                    leftselected = true;
                    patleft = new imgclass(optpath_xor, "XOR_Opt", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                    //ini.IniWriteValue("Section", "XOR_Opt", optpath_xor.ToString(), filenameini);
                }
                else if (comboBox2.Text == "Threshold_XOR")
                {
                    leftselected = true;
                    patleft = new imgclass(threshold_XOR, "XOR_Opt", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                }
                else if (comboBox2.Text == "XOR_XOR")
                {
                    leftselected = true;
                    patleft = new imgclass(xor_xor, "XOR_XOR", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                }
                else if (comboBox2.Text == "Dosage_XOR")
                {
                    leftselected = true;
                    patleft = new imgclass(Dosage_xor, "Dosage_XOR", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                }
                else if (comboBox2.Text == "LD_XOR")
                {
                    leftselected = true;

                    patleft = new imgclass(ldfile_xor, "LD_XOR", ptbh, ptbw, 1, 2);
                    if (patleft.img != null)
                    {
                        PBleft.Image = patleft.img;
                        PBleft.Invalidate();
                    }
                }



                if (patleft.img == null)
                {
                    return;
                }
                if (patleftflag == true)
                {
                    PBleft.SizeMode = PictureBoxSizeMode.Zoom;
                    PBleft.Height = patleft.img.Height;
                    PBleft.Width = patleft.img.Width;
                    var t = PBleft.Size;

                    t.Width = (int)(t.Width * rationew);
                    t.Height = (int)(t.Height * rationew);



                    PBleft.Size = t;
                    PBleft.Invalidate();
                }

            }
            Txtresolution.Focus();
        }


        //public string[] list;
        private void Optimization_Load(object sender, EventArgs e)
        {
            MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            ptbw = PBleft.Width;
            ptbh = PBleft.Height;
            if (first == true)
            {
                string[] list = new string[14] { "Original", "LDFile", "LDFile_opt", "LD_XOR", "Dosage", "Dosage_opt", "Dosage_XOR", "Threshold_Org", "Threshold_Opt", "Threshold_XOR", "XOR_Org", "XOR_Opt", "XOR_XOR", "" };
                comboBox1.Items.AddRange(list);
                comboBox2.Items.AddRange(list);
                first = false;
            }

            outputpath = ini.IniReadValue("Section", "TempFolder", filenameini);
            //optpath = outputpath + "optimization\\";

            realpercentage = 100;



            Txtresolution.Text = Convert.ToString(realres);
            Txtratio.Text = Convert.ToString(realpercentage);
            Txtres.Text = Convert.ToString(0.125);



            if (GuiMain.opt1start == true && GuiMain.opt2start == false && GuiMain.opt3start == false)
            {
                if(GuiMain.optst1result.Length==0)
                {
                    TxtCN.Text = "";//Connection Number
                    TxtTN.Text = "";// Total Number
                    TxtAfterCN.Text ="";
                    TxtAfterTN.Text = "";
                    Txtloss.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst1result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst1result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst1result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst1result[1]);
                    Txtloss.Text = Convert.ToString(GuiMain.optst1result[4]);
                }
                
                GuiMain.opt1start = false;
            }
            else if (GuiMain.opt1start == false && GuiMain.opt2start == true && GuiMain.opt3start == false)
            {
                if(GuiMain.optst2result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst2result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst2result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst2result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst2result[1]);
                    Txtloss.Text = Convert.ToString(GuiMain.optst2result[4]);
                }
                
                GuiMain.opt2start = false;
            }
            else if (GuiMain.opt1start == false && GuiMain.opt2start == false && GuiMain.opt3start == true)
            {
                if(GuiMain.optst3result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst3result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst3result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst3result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst3result[1]);
                    Txtloss.Text = Convert.ToString(GuiMain.optst3result[4]);
                }
               
                GuiMain.opt3start = false;
            }
            else if(GuiMain.opt1start == true && GuiMain.opt2start == true && GuiMain.opt3start == false)
            {
                if(GuiMain.optst2result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst2result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst2result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst2result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst2result[1]);
                }
                
                GuiMain.opt1start = false;
                GuiMain.opt2start = false;
            }
            else if(GuiMain.opt1start == true && GuiMain.opt2start == false && GuiMain.opt3start == true)
            {
                if(GuiMain.optst3result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst3result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst3result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst3result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst3result[1]);
                }
               
                GuiMain.opt1start = false;

                GuiMain.opt3start = false;
            }
            else if(GuiMain.opt1start == false && GuiMain.opt2start == true && GuiMain.opt3start == true)
            {
                if(GuiMain.optst3result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst3result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst3result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst3result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst3result[1]);
                }
                
                GuiMain.opt2start = false;

                GuiMain.opt3start = false;
            }
            else if(GuiMain.opt1start == true && GuiMain.opt2start == true && GuiMain.opt3start == true)
            {
                if(GuiMain.optst3result.Length==0)
                {
                    TxtCN.Text = "";
                    TxtTN.Text = "";
                    TxtAfterCN.Text = "";
                    TxtAfterTN.Text = "";
                }
                else
                {
                    TxtCN.Text = Convert.ToString(GuiMain.optst3result[2]);//Connection Number
                    TxtTN.Text = Convert.ToString(GuiMain.optst3result[0]);// Total Number
                    TxtAfterCN.Text = Convert.ToString(GuiMain.optst3result[3]);
                    TxtAfterTN.Text = Convert.ToString(GuiMain.optst3result[1]);
                }
               
                GuiMain.opt1start = false;
                GuiMain.opt2start = false;
                GuiMain.opt3start = false;
            }
            else if (GuiMain.opt1start == false && GuiMain.opt2start == false && GuiMain.opt3start == false)
            {
                TxtCN.Text = "0";
                TxtTN.Text = "0";
                TxtAfterCN.Text = "0";
                TxtAfterTN.Text = "0";
            }
            
            if(GuiMain.result2!="")
            {
                Txttime.Text = GuiMain.result2;
            }
            else
            {
                Txttime.Text ="";
            }
            ThresholdCB.Checked = true;
            XORCB.Checked = true;
            Dosage_XOR.Checked = true;
            LDFile_XOR.Checked = true;
            Threshold_XOR.Checked = true;
            XOR_XOR.Checked = true;
            //TxtCN.Text = Convert.ToString(GuiMain.optst1result[2]);
            //TxtTN.Text = Convert.ToString(GuiMain.optresult[0]);
            //TxtAfterCN.Text = Convert.ToString(GuiMain.optresult[3]);
            //TxtAfterTN.Text = Convert.ToString(GuiMain.optresult[1]);
            //TxtTN.Text = ini.IniReadValue("Section", "TN", filenameini);
            //TxtAfterTN.Text= ini.IniReadValue("Section", "OptTN", filenameini);
            //TxtCN.Text = ini.IniReadValue("Section", "CN", filenameini);
            //TxtAfterCN.Text = ini.IniReadValue("Section", "OptCN", filenameini);



        }
        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            //comboBox2.Focus();
            //comboBox1.Focus();
            mousewheelvalue = e.Delta;
            resolutionnow += e.Delta;
            if (Txtratio.Text != "")
            {
                if (e.Delta > 0)
                {

                    double a = double.Parse(Txtratio.Text);

                    a *= 2;
                    Txtratio.Text = a.ToString();

                }
                else if (e.Delta == 0)
                {
                    Txtratio.Text = "100";
                }
                else
                {
                    double a = double.Parse(Txtratio.Text);
                    a *= 0.5;
                    Txtratio.Text = a.ToString();
                }
            }
            if (RB1.Checked || RB2.Checked || RB3.Checked)
            {
                if (patleftflag == true && patleft.img != null)
                {
                    PBleft.Height = patleft.img.Height;
                    PBleft.Width = patleft.img.Width;
                    var t = PBleft.Size;
                    if (e.Delta > 0)
                    {
                        t.Width = (int)(t.Width * rationew);
                        t.Height = (int)(t.Height * rationew);

                    }
                    else if (e.Delta < 0)
                    {
                        t.Width = (int)(t.Width * rationew);
                        t.Height = (int)(t.Height * rationew);
                    }
                    else
                    {
                        t.Width *= 1;
                        t.Height *= 1;

                    }
                    //if (e.Delta > 0)
                    //{
                    //    rectshoworg.Height = (int)(rectshoworg.Height * rationew);
                    //    rectshoworg.Width = (int)(rectshoworg.Width * rationew);
                    //}
                    //else if (e.Delta < 0)
                    //{
                    //    rectshoworg.Height = (int)(rectshoworg.Height * rationew);
                    //    rectshoworg.Width = (int)(rectshoworg.Width * rationew);
                    //}
                    //else
                    //{
                    //    rectshoworg.Height = (rectshoworg.Height * 1);
                    //    rectshoworg.Width = (rectshoworg.Width * 1);
                    //}
                    //t.Width += e.Delta;
                    //t.Height += e.Delta;
                    PBleft.Size = t;
                    //PBleft.Invalidate();
                    //patleftflag = false;
                }
                if (patrightflag == true && patright.img != null)
                {
                    PBright.Height = patright.img.Height;
                    PBright.Width = patright.img.Width;
                    var t1 = PBright.Size;
                    if (e.Delta > 0)
                    {
                        t1.Width = (int)(t1.Width * rationew);
                        t1.Height = (int)(t1.Height * rationew);

                    }
                    else if (e.Delta < 0)
                    {
                        t1.Width = (int)(t1.Width * rationew);
                        t1.Height = (int)(t1.Height * rationew);
                    }
                    else
                    {
                        t1.Width *= 1;
                        t1.Height *= 1;

                    }
                    //if (e.Delta > 0)
                    //{
                    //    rectshoworg.Height = (int)(rectshoworg.Height * rationew);
                    //    rectshoworg.Width = (int)(rectshoworg.Width * rationew);
                    //}
                    //else if (e.Delta < 0)
                    //{
                    //    rectshoworg.Height = (int)(rectshoworg.Height * rationew);
                    //    rectshoworg.Width = (int)(rectshoworg.Width * rationew);
                    //}
                    //else
                    //{
                    //    rectshoworg.Height = (rectshoworg.Height * 1);
                    //    rectshoworg.Width = (rectshoworg.Width * 1);
                    //}

                    //t.Width += e.Delta;
                    //t.Height += e.Delta;
                    PBright.Size = t1;
                    //patrightflag = false;
                    //PBright.Invalidate();
                }


            }

        }
    }
}
