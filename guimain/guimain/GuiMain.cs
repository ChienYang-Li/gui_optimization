using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using HDF5DotNet;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;
using System.Timers;

namespace guimain
{

    public partial class GuiMain : Form
    {
        public hdf5 orgh5image = new hdf5();
        public hdf5 ldh5image = new hdf5();
        public hdf5 dosageh5image = new hdf5();
        public static tiffimage tiffimg = new tiffimage();
        public hdf5 patternimg = new hdf5();
        public Point realposlefttop;
        public Point realposrightbottom;
        public int ptbw;
        public int ptbh;
        public int bufferrow = 3000;
        public int buffercol = 3000;
        bool boundaryflag0 = false, boundaryflag1 = false;
        bool boundaryflaglefttop0 = true, boundaryflaglefttop1 = true;
        bool boundaryflag0right = false, boundaryflag1right = false;
        bool boundaryflaglefttop0right = true, boundaryflaglefttop1right = true;
        Point currentlefttoppos;
        Point currentrightbottompos;
        Bitmap bmp;
        bool down = true;
        bool mouseevent = false;
        bool Originalflag = false;
        public Point RectStartPoint;
        public Point ROIlefttop;
        public Point ROIrightbottom;
        public Rectangle RectOriginal = new Rectangle();
        public Rectangle RectPattern = new Rectangle();
        public int curheight, curwidth, currow, curcol;
        public static string filename;
        public int orgcollen;
        public int mousex;
        public int mousey;
        public int mousevaluex;
        public int mousevaluey;
        public Image patternld;
        public Image patterndosage;
        public string tiffimagename = "img.h5";
        public string outputfolderpath;
        public string tempfolderpath;
        public string dosageimgpath;
        public string Originalpath;
        public string LDconfig1path;
        public string LDconfig2path;
        public float[,] orgarray;
        public byte[,] ldarray;
        public string ldimgpath;
        public float[,] dosagearray;
        public bool Orgimageflag = false;
        public bool Patternimageflag = false;
        public bool patternrectflag = false;
        public string filenamepath;
        public bool mousewheelflag = false;
        public int resolutionnow;
        public bool patterndosagechangeflag = true;
        public bool patternldchangeflag = true;
        public bool Orifirstflag = false;
        public int mousewheelvalue;
        public bool first = true;
        public int fowardratio;
        public bool Orgbuttonclick = false;
        public bool dosageflag = false;
        public bool ldflag = false;
        public Image orgimage;
        public Image ldimage;
        public Image dosageimage;
        public bool buttonorgflag;
        public bool buttonldflag;
        public bool buttondosageflag;
        private float x = 0;
        private float y = 0;
        private Graphics backGraphics;
        public float realres;
        public float realpercentage;
        private Bitmap backBmp;
        public bool orgflag = false;
        public bool ldflagfirst = false;
        public bool dosageflagfirst = false;
        public bool ratiochangeflag = false;
        public bool ratiochangefirstflag = true;
        public float oldratio = 1;
        public float rationew = 1;
        public bool ratiochangescendflag = false;
        public float orgheight;
        public float orgwidth;
        public float patheight;
        public float patwidth;
        public Size t;
        public Size t1;
        public int ratiocnt;
        public string ldfilename1;
        public string ldfilename2;
        public int ptbmax = 40000;
        public int magnifyratioheight;
        public int magnifyratiowidth;
        public bool ptbmaxsizeall = false;
        public bool ptbmaxsizewidth = false;
        public bool ptbmaxsizeheight = false;
        public bool ptbminsizeall = false;
        public bool ptbminsizewidth = false;
        public bool ptbminsizeheight = false;
        public ldconfig ldfile1 = new ldconfig();
        public ldconfig ldfile2 = new ldconfig();
        public float imgps = 1f;
        public static string beamfile;
        public static LE le1 = new LE();
        public int le2xoffset;
        public int le2yoffset;
        public int le1xoffset;
        public int le1yoffset;
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public int cnt = 0;
        public float ptbfloatw = 0;
        public float ptbfloath = 0;
        public List<int> ldcnt = new List<int>();
        DateTime EndTime;
        TimeSpan RemainderTime;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        public int le1xnew = 0;
        public int le1ynew = 0;
        public int le2xnew = 0;
        public int le2ynew = 0;
        public ulong mousexvalue;
        public ulong mouseyvalue;
        public int orgpos0now, orgpos1now;
        public int patpos0now, patpos1now;
        public bool orgfresh = false;
        public bool patfresh = false;
        public imgclass orgimg;
        public imgclass patimg;
        public imgclass patroi_img;
        public imgclass orgroi_img;
        public string dosagename_2levelpath, dosagename_4levelpath, ldimgname_2levelpath, ldimgname_4levelpath, Threshold_2levelpath, Threshold_4levelpath, xor_path;
        public string ldimg_2levelROI, ldimg_4levelROI, dosage_2levelROI, dosage_4levelROI, Threshold_2levelROI, Threshold_4levelROI, xor_roi_2level_path, xor_4level_path, xor_roi_4level_path, roi_path;
        public bool comboflag = false;
        public float dosageres = 1.25f;
        public bool roiflag = false;
        public bool checkroiflag = false;
        public bool checkroiflag2 = false;
        public bool mw_flag_ROI = false;//mousewheelflag&&ROI
        public Size rectoptsize = new Size(300, 300);
        public Rectangle rectopt = new Rectangle();
        public Size rectnewsize;
        public Size rectnewsize40000;
        public Point optlefttop = new Point();
        public bool rectoptflag = false;
        public static Optimization opt;
        public static Parameter par = new Parameter();
        public bool orgmeenterflag = false;//originalmouseenterflag
        public bool patmeenterflag = true;//patternmouseenterflag;
        public bool ratiofirst = true;
        public bool ratiosecond = true;
        public float ratiotmp = 0;
        public ulong mouseratiox;
        public ulong mouseratioy;
        public ulong mousexvaluept;
        public ulong mouseyvaluept;
        public string ldfilepath_2level_roi1, ldfilepath_2level_roi2;
        public string ldfilepath1_2level, ldfilepath2_2level;
        public string ldfilepath_4level_roi1, ldfilepath_4level_roi2, ldfilepath1_4level, ldfilepath2_4level;
        public string ldfilepath1, ldfilepath2;
        public int newrectx;
        public int newrecty;
        public bool modifybymouse = false;
        public static float threshold = 0;
        public static float time = 0;
        public static int[] optst1result;
        public static int[] optst2result;
        public static int[] optst3result;
        public static bool opt1start = false;
        public static bool opt2start = false;
        public static bool opt3start = false;
        public bool mouseright = false;
        public bool optflag = false;
        public bool guimainflag = false;
        public string roi_4level_path;
        public string roimousexy;
        public bool roicheckbool = false;
        public string oldpattern;
        public bool firstpattern = true;
        public bool patternclear = false;
        public int oldxscroll = 0;
        public int oldyscroll = 0;
        public bool roichange = false;
        public bool combochange = false;
        public bool roiscroll = false;
        public bool ldconfig1fail = false;
        public bool ldconfig2fail = false;
        public bool beamfilefail = false;
        public bool outputfolderfail = false;
        public bool tempfolderfail = false;
        public static bool patfail = false;
        public ulong mouseroix;
        public ulong mouseroiy;
        public float displacement;
        //public float time;
        //public bool openfilefirstflag = false;
        //public string openfilefirst;
        //public bool openfilemodfirstflag = false;
        //public string openfilemodfirst;
        public string OriginalPath;
        public string modpath;
        public string overwriteld1;
        public string overwriteld2;
        public static  string result2;
        //public int mousex40082;
        //public int mousey40082;
        public GuiMain()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.backBmp = new Bitmap(this.DisplayRectangle.Width, this.DisplayRectangle.Height);
            this.backGraphics = Graphics.FromImage(backBmp);

            this.timer1.Enabled = true;
            this.timer1.Interval = 50;

        }
        private void ROIcheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ROIcheckBox.Checked && RectoptCB.Checked)
            {
                MessageBox.Show("ROIcheckbox和Rectoptcheckbox不能同時勾選", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //mouseevent = true;
                mouseevent = true;
                ROIcheckBox.Checked = false;
                RectoptCB.Checked = false;
              
                return;
            }
            if (mouseevent == true)
            {
                mouseevent = false;
            }
            else
            {
                mouseevent = true;
            }
            this.ROIcheckBox.Focus();


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.x += 1;

            //重新填充背景色，否則會殘留之前的圖形
            this.backGraphics.FillRectangle(Brushes.White, 0, 0, this.DisplayRectangle.Width, this.DisplayRectangle.Height);

            this.backGraphics.DrawEllipse(Pens.Blue, this.x, this.y, 50, 50);
            this.CreateGraphics().DrawImageUnscaled(this.backBmp, 0, 0);
        }
        private void ScanningRadiobutton_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void TxtPattern_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void lEToolStripMenuItem_Click(object sender, EventArgs e)
        {

            le1.f1 = this;
            le1.ShowDialog();

            ldfile1.readldconfig(ldfilename1,imgps);

            ldfile2.readldconfig(ldfilename2, imgps);
            ldfile1.addxyoffset(le1.le1xtext, le1.le1ytext);

            ldfile2.addxyoffset(le1.le1xtext + le1.le2xtext, le1.le1ytext + le1.le2ytext);
            if (le1.le1res != 0 && le1.le2res != 0)
            {
                ldfile1.Multires(le1.le1res);
                ldfile2.Multires(le1.le2res);
                ldfile1.lightspotrange();
                ldfile2.lightspotrange();
                ini.IniWriteValue("Section", "le1res", Convert.ToString(le1.le1res), filenameini);
                ini.IniWriteValue("Section", "le2res", Convert.ToString(le1.le2res), filenameini);

            }
            else
            {


            }
            //ldfile1.Multires(le1.le1res);
            //ldfile2.Multires(le1.le2res);
            le1xnew = le1.le1xtext;
            le1ynew = le1.le1ytext;
            le2xnew = le1.le2xtext;
            le2ynew = le1.le2ytext;
            ini.IniWriteValue("Section", "le1xoffset", Convert.ToString(le1xnew), filenameini);
            ini.IniWriteValue("Section", "le1yoffset", Convert.ToString(le1ynew), filenameini);
            ini.IniWriteValue("Section", "le2xoffset", Convert.ToString(le2xnew), filenameini);
            ini.IniWriteValue("Section", "le2yoffset", Convert.ToString(le2ynew), filenameini);

            PatternPB.Invalidate();
        }

        private void GuiMain_Load(object sender, EventArgs e)
        {
            ptbw = OriginalPB.Width;
            ptbh = OriginalPB.Height;
            ptbfloatw = OriginalPB.Width;
            ptbfloath = OriginalPB.Height;
            this.IsMdiContainer = true;
            string[] list = new string[] { "LDFile_2level", "LDFile_4level", "Dosage_2level", "Dosage_4level", "Threshold_2level", "Threshold_4level", /*"Threshold_2levelROI", "Threshold_4levelROI"*/ "XOR_2level", "XOR_4level", "" };
            comboBox1.Items.AddRange(list);
            bool success;
            bool success1;
            //ini.IniWriteValue("Section", "LDconfig1", "D:\\h5img\\MainGuicode\\LD1.25.txt", filenameini);
            //ini.IniWriteValue("Section", "LDconfig2", "D:\\h5img\\MainGuicode\\LD2.txt", filenameini);
            //ini.IniWriteValue("Section", "Beamfile", "D:\\h5img\\MainGuicode\\", filenameini);
            //ini.IniWriteValue("Section", "OutputFolder", "D:\\h5img\\", filenameini);
            //ini.IniWriteValue("Section", "le1xoffset", "0", filenameini);
            //ini.IniWriteValue("Section", "le1yoffset", "0", filenameini);
            //ini.IniWriteValue("Section", "le2xoffset", "0", filenameini);
            //ini.IniWriteValue("Section", "le2yoffset", "0", filenameini);
            //ini.IniWriteValue("Section", "OriginalPath", OriginalPath, filenameini);
            textBox5.Text = ini.IniReadValue("Section", "Displacement", filenameini);
            ldfilename1 = ini.IniReadValue("Section", "LDconfig1", filenameini);
            ldfilename2 = ini.IniReadValue("Section", "LDconfig2", filenameini);
            beamfile = ini.IniReadValue("Section", "Beamfile", filenameini);
            outputfolderpath = ini.IniReadValue("Section", "OutputFolder", filenameini);
            tempfolderpath = ini.IniReadValue("Section", "TempFolder", filenameini);
            //if (File.Exists(ldfilename1))
            ldfile1.readldconfig(ldfilename1,imgps);
            //if (File.Exists(ldfilename2))
            ldfile2.readldconfig(ldfilename2, imgps);
            le2xoffset = int.Parse(ini.IniReadValue("Section", "le2xoffset", filenameini));
            le2yoffset = int.Parse(ini.IniReadValue("Section", "le2yoffset", filenameini));
            le1xoffset = int.Parse(ini.IniReadValue("Section", "le1xoffset", filenameini));
            le1yoffset = int.Parse(ini.IniReadValue("Section", "le1yoffset", filenameini));
            ldfile2.addxyoffset(le2xoffset, le2yoffset);

            ldfile1.addxyoffset(le1xoffset, le1yoffset);
            ldfile1.lightspotrange();
            ldfile2.lightspotrange();
            ldconfig1txt.Text = ini.IniReadValue("Section", "LDconfig1", filenameini);
            ldconfig2txt.Text = ini.IniReadValue("Section", "LDconfig2", filenameini);
            beamfilepathtxt.Text = ini.IniReadValue("Section", "Beamfile", filenameini);
            outputpathtxt.Text = ini.IniReadValue("Section", "OutputFolder", filenameini);
            tempfoldertxt.Text = ini.IniReadValue("Section", "TempFolder", filenameini);
            //DeleteSrcFolder(tempfoldertxt.Text);
            Txttime.Text = ini.IniReadValue("Section", "Time", filenameini);
            TxtThreshold.Text = ini.IniReadValue("Section", "Threshold", filenameini);

            TxtLDFile1.Text = ini.IniReadValue("Section", "ldfilepath1", filenameini);
            TxtLDFile2.Text = ini.IniReadValue("Section", "ldfilepath2", filenameini);
            Txtorg.Text = ini.IniReadValue("Section", "OriginalPath", filenameini);
            ldfilepath1 = TxtLDFile1.Text;
            ldfilepath2 = TxtLDFile2.Text;
            //success = float.TryParse(TxtThreshold.Text, out float result);
            //success1 = float.TryParse(Txttime.Text, out float result1);
            //if (success1 == true)
            //{
            //    time = result1;

            //}
            //if (success == true)
            //{
            //    threshold = result;
            //}
            //else
            //{

            //}

            //LE2offsetxfirst.Text= Convert.ToString(ini.IniReadValue("Section", "le2xoffset", filenameini));
            //LE2offsetyfirst.Text= Convert.ToString(ini.IniReadValue("Section", "le2yoffset", filenameini));
            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
            timer1.Interval = 10000;
            TxtRectX.Text = Convert.ToString(300);
            TxtRectY.Text = Convert.ToString(300);
            Txtresopt.Text = Convert.ToString(0.125);
            //if()
            //{

            //}
        }
        public void DeleteSrcFolder(string file)
        {
            //去除資料夾和子檔案的只讀屬性
            //去除資料夾的只讀屬性
            System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
            fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            //去除檔案的只讀屬性
            System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
            //判斷資料夾是否還存在
            if (Directory.Exists(file))
            {
                foreach (string f in Directory.GetFileSystemEntries(file))
                {
                    if (File.Exists(f))
                    {
                        //如果有子檔案刪除檔案
                        File.Delete(f);
                    }
                    else
                    {
                        //迴圈遞迴刪除子資料夾 
                        //DeleteSrcFolder1(f);
                    }
                }
                //刪除空資料夾
                //Directory.Delete(file);
            }
        }

        private void LDFileBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = "D:\\";
            dialog.Filter = "Text Files(*.h5)|*.h5";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ldimgpath = dialog.FileName;
                ldh5image.openfile(dialog.FileName);
                //int height = 20000;
                //int width = 20000;
                if (ldh5image.rownum <= realposrightbottom.Y)
                {
                    realposrightbottom.Y = ldh5image.rownum - 1;
                }
                else if (ldh5image.colnum <= realposrightbottom.X)
                {
                    realposrightbottom.X = ldh5image.colnum - 1;
                }

                //PatternPB.Height = ldh5image.rownum;
                //PatternPB.Width = ldh5image.colnum;
                ldh5image.closefile();
                buttonldflag = true;

                PatternPB.Invalidate();







            }
        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void OriginalBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            //dialog.InitialDirectory = "D:\\";
            dialog.Filter = " TIFF | *.tif; *.tiff";
            //if(openfilefirstflag==true)
            //{
            //    openfilefirstflag = false;
            //    dialog.InitialDirectory = openfilefirst;
            //}
            //else
            //{

            //}
            dialog.InitialDirectory = Txtorg.Text;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //openfilefirstflag = true;
                //openfilefirst = Path.GetDirectoryName(dialog.FileName);  //得到路徑  

                if(ROIshowCB.Checked)
                {
                    MessageBox.Show("請將ROI_Image勾選紐取消","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }



                OriginalPath = Path.GetDirectoryName(dialog.FileName);
                ini.IniWriteValue("Section", "OriginalPath", OriginalPath, filenameini);
                filename = Path.GetFileName(dialog.FileName);

                Txtinputname.Text = filename;
                string h5filename = tempfolderpath + filename + "_img.h5";
                //if (File.Exists(h5filename))
                //{
                //tiffimg.opentiff(dialog.FileName);
                //Originalpath = outputfolderpath + filename + "_img.h5";
                //    tiffimg.closetiff();
                //}
                //else
                {
                    tiffimg.opentiff(dialog.FileName);
                    tiffimg.readtiffproperty();
                    Originalpath = tempfolderpath + tiffimg.imgfilename + "_img.h5";
                    if (!File.Exists(Originalpath))
                    {
                        tiffimg.readpattern(tempfolderpath);
                    }
                    tiffimg.closetiff();




                }

                orgimg = new imgclass(Originalpath, "Originalimg", ptbh, ptbw, 1);
                if (firstpattern == true)
                {
                    oldpattern = filename;
                    firstpattern = false;
                }
                if (oldpattern != filename)
                {
                    combochange = true;
                    panel1.AutoScrollPosition = new Point(0, 0);
                    panel2.AutoScrollPosition = new Point(0, 0);

                    //comboBox1.Items.Clear();
                    comboBox1.Text = "";

                    //OriginalPB.Image = null;
                    oldpattern = filename;
                    orgheight = orgimg.h5img.rownum;
                    orgwidth = orgimg.h5img.colnum;
                    //int orgheightscale = (int)(orgheight * 100/ 100);
                    //int orgwidthscale = (int)(orgwidth * 100 / 100);
                    magnifyratioheight = (int)orgheight;
                    magnifyratiowidth = (int)orgwidth;
                }

                //ptbmax = orgimg.h5img.rownum;//改成自己定義的變數
                OriginalPB.Height = ptbmax;//initialize OriginalPB
                OriginalPB.Width = ptbmax;//initialize OriginalPB




                PatternPB.Height = OriginalPB.Height;
                PatternPB.Width = OriginalPB.Width;
                ldimgname_2levelpath = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level.h5";
                ldimgname_4levelpath = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level.h5";
                dosagename_2levelpath = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level.h5";
                dosagename_4levelpath = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level.h5";
                Threshold_2levelpath = tempfolderpath + tiffimg.imgfilename + "_threshold_2level.h5";
                Threshold_4levelpath = tempfolderpath + tiffimg.imgfilename + "_threshold_4level.h5";
                xor_path = tempfolderpath + tiffimg.imgfilename + "_xor_2level.h5";
                xor_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_4level.h5";
                ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi.h5";
                ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi.h5";
                dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi.h5";
                dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi.h5";
                Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi.h5";
                Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi.h5";
                xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level.h5";
                xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level.h5";
                roi_path = tempfolderpath + tiffimg.imgfilename + "_roi.h5";
                roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level.h5";
                ldfilepath_2level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_2level_roi.LD";
                ldfilepath_2level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_2level_roi.LD";
                ldfilepath1_2level = outputfolderpath + tiffimg.imgfilename + "_LE1_2level.LD";
                ldfilepath2_2level = outputfolderpath + tiffimg.imgfilename + "_LE2_2level.LD";
                ldfilepath_4level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_4level_roi.LD";
                ldfilepath_4level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_4level_roi.LD";
                ldfilepath1_4level = outputfolderpath + tiffimg.imgfilename + "_LE1_4level.LD";
                ldfilepath2_4level = outputfolderpath + tiffimg.imgfilename + "_LE2_4level.LD";
                //Tuple<int[], byte[,]> currentarraytuple;

                //if (ldflagfirst == false && dosageflagfirst == false)
                //{
                //    if (rationew >= 1)
                //    {
                //        currentarraytuple = calculateposorg(Originalpath, 0, 0);
                //        if (orgh5image.rownum <= realposrightbottom.Y)
                //        {
                //            realposrightbottom.Y = orgh5image.rownum - 1;
                //        }
                //        else if (orgh5image.colnum <= realposrightbottom.X)
                //        {
                //            realposrightbottom.X = orgh5image.colnum - 1;
                //        }
                //        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
                //        realposlefttop.X = currentarraytuple.Item1[0];
                //        realposlefttop.Y = currentarraytuple.Item1[1];
                //        realposrightbottom.X = currentarraytuple.Item1[2];
                //        realposrightbottom.Y = currentarraytuple.Item1[3];

                //    }
                //    else
                //    {
                //        currentarraytuple = calculateposorgfloatzoomout(Originalpath, 0, 0, rationew);
                //        if (orgh5image.rownum <= realposrightbottom.Y)
                //        {
                //            realposrightbottom.Y = orgh5image.rownum - 1;
                //        }
                //        else if (orgh5image.colnum <= realposrightbottom.X)
                //        {
                //            realposrightbottom.X = orgh5image.colnum - 1;
                //        }
                //        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
                //        realposlefttop.X = currentarraytuple.Item1[0];
                //        realposlefttop.Y = currentarraytuple.Item1[1];
                //        realposrightbottom.X = currentarraytuple.Item1[2];
                //        realposrightbottom.Y = currentarraytuple.Item1[3];
                //    }

                //}
                //else
                //{

                //    orgh5image.openfile(outputfolderpath + tiffimagename);
                //    if (orgh5image.rownum <= realposrightbottom.Y)
                //    {
                //        realposrightbottom.Y = orgh5image.rownum - 1;
                //    }
                //    else if (orgh5image.colnum <= realposrightbottom.X)
                //    {
                //        realposrightbottom.X = orgh5image.colnum - 1;
                //    }
                //    byte[,] currentarray;
                //    currentarray = orgh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
                //    orgimage = BufferTobinaryImage(currentarray);
                //    orgh5image.closefile();



                //}



                guimainflag = true;
                //tiffimg.imgps = 2.5f;
                realres = tiffimg.imgps;
                imgps = tiffimg.imgps;

                if(Math.Abs(ldfile1.ldps - imgps) > 0.00001)
                {
                    ldfile1.modifyres(imgps);
                    ldfile2.modifyres(imgps);
                    ldfile1.lightspotrange();
                    ldfile2.lightspotrange();
                }

                realpercentage = 100;
                Txtratio.Text = Convert.ToString(realpercentage);
                //XORGB.Visible = false;
                //OriginalPB.Height = orgh5image.rownum;
                //OriginalPB.Width = orgh5image.colnum;
                TxtInputheight.Text = Convert.ToString(orgimg.h5img.rownum);
                Txtinputwidth.Text = Convert.ToString(orgimg.h5img.colnum);
                Txtinputresolution.Text = Convert.ToString(tiffimg.imgps);
                OriginalPB.Invalidate();
                PatternPB.Invalidate();
                buttonorgflag = true;
                orgflag = true;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ldconfig ldfile1 = new ldconfig();
            OpenFileDialog dialog = new OpenFileDialog();


            dialog.Title = "Select file";
            dialog.InitialDirectory = "D:\\";
            dialog.Filter = "Text Files(*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                if (!File.Exists(dialog.FileName))
                {
                    MessageBox.Show("路徑下不存在LDconfig檔,請重新選擇新的LDconfig檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ldconfig1fail = true;

                    return;
                }
                else
                {
                    ldconfig1fail = false;
                    ldfilename1 = dialog.FileName;
                    ldfile1.readldconfig(ldfilename1,imgps);
                    
                    ldfile1.showfacet();
                    ini.IniWriteValue("Section", "LDconfig1", ldfilename1, filenameini);
                    ldconfig1txt.Text = ldfilename1;
                    ldfile1.lightspotrange();
                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            //ldconfig ldfile2 = new ldconfig();
            dialog.Title = "Select file";
            dialog.InitialDirectory = "D:\\";
            dialog.Filter = "Text Files(*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {

                if (!File.Exists(dialog.FileName))
                {
                    MessageBox.Show("路徑下不存在LDconfig檔,請重新選擇新的LDconfig檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ldconfig2fail = true;
                    return;
                }
                else
                {
                    ldconfig2fail = false;
                    ldfilename2 = dialog.FileName;
                    ldfile2.readldconfig(ldfilename2,imgps);
                    
                    ini.IniWriteValue("Section", "LDconfig2", ldfilename2, filenameini);
                    ldconfig2txt.Text = ldfilename2;
                    ldfile2.lightspotrange();
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog FolderPath = new FolderBrowserDialog();
            string spotarray = "spotarrays";
            string energy100 = "100";
            string energy66 = "66";
            string energy33 = "33";
            string res0125 = "0125";
            string res125 = "125";
            string le1 = "le1";
            string le2 = "le2";
            string h5 = ".h5";
            StringBuilder path = new StringBuilder(spotarray), path1, path2, path3;
            //FolderPath.ShowDialog();
            if (FolderPath.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(FolderPath.SelectedPath))
                {
                    MessageBox.Show("路徑下的資料夾不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le1 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le1 + "_" + res125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le2 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le2 + "_" + res125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le1 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le1 + "_" + res125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le2 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le2 + "_" + res125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le1 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le1 + "_" + res125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le2 + "_" + res0125 + h5) && !File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le2 + "_" + res125 + h5))
                {
                    MessageBox.Show("所選路徑不存在12個光點能量陣列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string[] energy = new string[3] { "100", "66", "33" };
                    string[] res = new string[2] { "0125", "125" };
                    string[] le = new string[2] { "1", "2" };
                    foreach (string energynum in energy)
                    {
                        path1 = new StringBuilder(path.ToString());
                        path1.Append(energynum);
                        foreach (string lenum in le)
                        {
                            path2 = new StringBuilder(path1.ToString());
                            path2.Append("le" + lenum + "_");
                            foreach (string resnum in res)
                            {
                                path3 = new StringBuilder(path2.ToString());
                                path3.Append(resnum + h5);
                                if (!File.Exists(FolderPath.SelectedPath + "\\" + path3.ToString()))
                                {
                                    MessageBox.Show(path3.ToString() + "光點檔案不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    beamfilefail = true;
                                }
                                else
                                {
                                    beamfilefail = false;
                                    beamfile = FolderPath.SelectedPath + "\\";
                                    ini.IniWriteValue("Section", "beamfile", beamfile, filenameini);
                                    beamfilepathtxt.Text = beamfile;
                                }
                            }
                        }
                    }

                    //if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le1 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le1 + "_" + res125 + h5))
                    //{
                    //    if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le2 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy33 + le2 + "_" + res125 + h5))
                    //    {
                    //        if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le1 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le1 + "_" + res125 + h5))
                    //        {
                    //            if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le2 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy66 + le2 + "_" + res125 + h5))
                    //            {
                    //                if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le1 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le1 + "_" + res125 + h5))
                    //                {
                    //                    if (File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le2 + "_" + res0125 + h5) && File.Exists(FolderPath.SelectedPath + "\\" + spotarray + energy100 + le2 + "_" + res125 + h5))
                    //                    {
                    //                        beamfilefail = false;
                    //                        beamfile = FolderPath.SelectedPath + "\\";
                    //                        ini.IniWriteValue("Section", "beamfile", beamfile, filenameini);
                    //                        beamfilepathtxt.Text = beamfile;
                    //                    }
                    //                    else
                    //                    {
                    //                        MessageBox.Show("不存在le2下能量100%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //                        beamfilefail = true;
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    MessageBox.Show("不存在le1下能量100%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //                    beamfilefail = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                MessageBox.Show("不存在le2下能量66%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //                beamfilefail = true;
                    //            }
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("不存在le1下能量66%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //            beamfilefail = true;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("不存在le2下能量33%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        beamfilefail = true;
                    //    }
                    //}
                    //else
                    //{

                    //    MessageBox.Show("不存在le1下能量33%的h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    beamfilefail = true;

                    //}
                }



            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderPath = new FolderBrowserDialog();
            //FolderPath.ShowDialog();
            if (FolderPath.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(FolderPath.SelectedPath))
                {
                    MessageBox.Show("路徑下的資料夾不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    outputfolderfail = true;
                    return;
                }
                else
                {
                    outputfolderfail = false;
                    outputfolderpath = FolderPath.SelectedPath + "\\";
                    ini.IniWriteValue("Section", "OutputFolder", outputfolderpath, filenameini);
                    outputpathtxt.Text = outputfolderpath;
                }

            }




        }

        private void DosageBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            dialog.InitialDirectory = "D:\\";
            dialog.Filter = "Text Files(*.h5)|*.h5";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //StreamReader sr = new StreamReader(dialog.FileName);
                dosageh5image.openfile(dialog.FileName);
                dosageimgpath = dialog.FileName;
                //int height = 20000;
                //int width = 20000;
                if (dosageh5image.rownum <= realposrightbottom.Y)
                {
                    realposrightbottom.Y = dosageh5image.rownum - 1;
                }
                else if (dosageh5image.colnum <= realposrightbottom.X)
                {
                    realposrightbottom.X = dosageh5image.colnum - 1;
                }
                //dosagearray = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y- realposlefttop.Y+1, realposrightbottom.X- realposlefttop.X+1);
                //PatternPB.Height = dosageh5image.rownum;
                //PatternPB.Width = dosageh5image.colnum;
                dosageh5image.closefile();
                //int m = dosagearray.GetLength(0);
                //int n = dosagearray.GetLength(1);
                //byte[,] dosagetarraybyte = new byte[m, n];
                //float min1 = float.MaxValue, max1 = 0;
                //for (int i = 0; i < m; ++i)
                //    for (int j = 0; j < n; ++j)
                //    {
                //        if (max1 < dosagearray[i, j])
                //        { max1 = dosagearray[i, j]; }
                //        if (min1 > dosagearray[i, j])
                //        { min1 = dosagearray[i, j]; }
                //    }
                //for (int i = 0; i < m; ++i)
                //{
                //    for (int j = 0; j < n; ++j)
                //    {
                //        dosagearray[i, j] = (dosagearray[i, j] - min1) / (max1 - min1);
                //        dosagetarraybyte[i, j] = (byte)(dosagearray[i, j] * 255);
                //    }
                //}
                //Tuple<int[], byte[,]> currentarraytupledosage;
                //int pos0 = panel1.HorizontalScroll.Value;
                //int pos1 = panel1.VerticalScroll.Value;
                //currentarraytupledosage = calculateposfloat(dosageimgpath, pos0, pos1);
                //dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                //realposlefttop.X = currentarraytupledosage.Item1[0];
                //realposlefttop.Y = currentarraytupledosage.Item1[1];
                //realposrightbottom.X = currentarraytupledosage.Item1[2];
                //realposrightbottom.Y = currentarraytupledosage.Item1[3];
                //dosageimage = BufferToImage(dosagetarraybyte);

                // patterndosage = BufferTobinaryImage(orgarray);
                ////Bitmap imageout = new Bitmap(orgimage);
                //PatternPB.Image = patterndosage;
                //PatternPB.Height = patterndosage.Height;
                //PatternPB.Width = patterndosage.Width;
                buttondosageflag = true;
                PatternPB.Invalidate();



            }

        }
        public Image BufferTobinaryImage(byte[,] myMatrix)
        {
            {
                //do a fake image processing for a grayscale bitmap
                //note that I use 1 byte for one pixel, 

                //set up an array (x must be multiple of 4)

                int height = myMatrix.GetLength(0);
                int width = myMatrix.GetLength(1);
                //fill with random (color-) values

                //now we have to convert the 2 dimensional array into a one dimensional byte-array for use with 8bpp bitmaps

                int Stride = 4 * ((width * 8 + 31) / 32);
                byte[] pixels = new byte[height * Stride];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pixels[y * Stride + x] = myMatrix[y, x];
                    }
                }
                // Buffer.BlockCopy(pixels, 0, myMatrix, 0, pixels.Length);

                //create a new Bitmap
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

                //create grayscale palette

                pal.Entries[0] = Color.Black;
                pal.Entries[1] = Color.White;
                //for (int i = 1; i < 256; i++)
                //{
                //    pal.Entries[i] = Color.FromArgb(i, i, i);//all the values exclude 0 are white 
                //}


                //assign to bmp
                bmp.Palette = pal;

                //lock it to get the BitmapData Object
                System.Drawing.Imaging.BitmapData bmData =
                    bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                //copy the bytes
                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmData.Scan0, bmData.Stride * bmData.Height);

                //never forget to unlock the bitmap
                bmp.UnlockBits(bmData);

                //display
                return bmp;
            }
        }
        public Image BufferToImage(byte[,] myMatrix)
        {
            {
                //do a fake image processing for a grayscale bitmap
                //note that I use 1 byte for one pixel, 

                //set up an array (x must be multiple of 4)

                int height = myMatrix.GetLength(0);
                int width = myMatrix.GetLength(1);
                //fill with random (color-) values

                //now we have to convert the 2 dimensional array into a one dimensional byte-array for use with 8bpp bitmaps

                int Stride = 4 * ((width * 8 + 31) / 32);
                byte[] pixels = new byte[height * Stride];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pixels[y * Stride + x] = myMatrix[y, x];
                    }
                }
                // Buffer.BlockCopy(pixels, 0, myMatrix, 0, pixels.Length);

                //create a new Bitmap
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

                //create grayscale palette

                pal.Entries[0] = Color.Black;
                //for (int i = 1; i < 86; ++i)
                //{

                //    pal.Entries[i] = Color.FromArgb(0, 0, i + 170);
                //    //all the values exclude 0 are white


                //}
                float r = 53;
                float g = 42;
                float b = 135;
                bool first = true;
                bool first1 = true;
                bool first2 = true;
                bool first3 = true;
                bool first4 = true;
                bool first5 = true;
                for (int i = 1; i < 256; ++i)
                {
                    if (i >= 2 && i <= 40)
                    {
                        r -= 52f / 39f;
                        //if(r<=1)
                        //{
                        //    r = 1;
                        //}

                    }
                    if (i >= 41 && i <= 68)
                    {
                        if (first == true)
                        {
                            r = 4;
                            first = false;
                        }
                        else
                        {
                            r += 16f / 27f;
                        }


                        //if (r <= 1)
                        //{
                        //    r = 1;
                        //}
                    }
                    if (i >= 69 && i <= 84)
                    {
                        if (first1 == true)
                        {
                            r = 19;
                            first1 = false;
                        }
                        else
                        {
                            r -= 10f / 15f;
                        }


                        //if (r <= 1)
                        //{
                        //    r = 1;
                        //}
                    }
                    if (i >= 85 && i <= 104)
                    {
                        r = 7;

                    }
                    if (i >= 105 && i <= 220)
                    {
                        if (first2 == true)
                        {
                            r = 10;
                            first2 = false;
                        }
                        else
                        {
                            r += 245f / 115f;
                        }



                    }
                    if (i >= 221 && i <= 248)
                    {
                        if (first3 == true)
                        {
                            r = 253;
                            first3 = false;
                        }
                        else
                        {
                            r -= 8f / 27f;
                        }


                        //if (r <= 1)
                        //{
                        //    r = 1;
                        //}
                    }
                    if (i >= 249 && i <= 255)
                    {
                        if (first4 == true)
                        {
                            r = 247;
                            first4 = false;
                        }
                        else
                        {
                            r -= 2f / 6f;
                        }


                        //if (r <= 1)
                        //{
                        //    r = 1;
                        //}
                    }
                    if (i >= 2 && i <= 255)
                    {
                        g += 209f / 254f;
                    }
                    if (i >= 2 && i <= 40)
                    {
                        b += 90f / 39f;
                    }
                    if (i >= 41 && i <= 255)
                    {
                        if (first5 == true)
                        {
                            b = 224;
                            first5 = false;
                        }
                        else
                        {
                            b -= 210f / 214f;
                        }
                    }


                    pal.Entries[i] = Color.FromArgb((int)r, (int)g, (int)b);
                }

                //for (int j = 86; j < 172; ++j)
                //{


                //    pal.Entries[j] = Color.FromArgb(128, 128, j);//all the values exclude 0 are white 

                //}





                //for (int k = 172; k < 256; ++k)
                //{


                //    pal.Entries[k] = Color.FromArgb(255, k, 255);//all the values exclude 0 are white 

                //}

                //for (int i = 1; i < 256; ++i)
                //{
                //    pal.Entries[i] = Color.FromArgb(255, i, 255);
                //}




                //assign to bmp
                bmp.Palette = pal;

                //lock it to get the BitmapData Object
                System.Drawing.Imaging.BitmapData bmData =
                    bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                //copy the bytes
                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmData.Scan0, bmData.Stride * bmData.Height);

                //never forget to unlock the bitmap
                bmp.UnlockBits(bmData);

                //display
                return bmp;
            }
        }
        public Image BufferToImage15(byte[,] myMatrix)
        {
            {
                //do a fake image processing for a grayscale bitmap
                //note that I use 1 byte for one pixel, 

                //set up an array (x must be multiple of 4)

                int height = myMatrix.GetLength(0);
                int width = myMatrix.GetLength(1);
                //fill with random (color-) values

                //now we have to convert the 2 dimensional array into a one dimensional byte-array for use with 8bpp bitmaps

                int Stride = 4 * ((width * 8 + 31) / 32);
                byte[] pixels = new byte[height * Stride];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        pixels[y * Stride + x] = myMatrix[y, x];
                    }
                }
                // Buffer.BlockCopy(pixels, 0, myMatrix, 0, pixels.Length);

                //create a new Bitmap
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

                //create grayscale palette

                pal.Entries[0] = Color.Black;
                //for (int i = 1; i < 86; ++i)
                //{

                //    pal.Entries[i] = Color.FromArgb(0, 0, i + 170);
                //    //all the values exclude 0 are white


                //}
                int r = 0;
                int g = 0;
                int b = 0;
                for (int i = 1; i < 256; ++i)
                {
                    if (i >= 1 && i <= 17)//1
                    {
                        r = 0;
                        g = 0;
                        b = 143;
                    }
                    if (i >= 18 && i <= 34)//2
                    {
                        r = 0;
                        g = 0;
                        b = 191;
                    }
                    if (i >= 35 && i <= 51)//3
                    {
                        r = 0;
                        g = 0;
                        b = 255;
                    }
                    if (i >= 52 && i <= 68)//4
                    {
                        r = 0;
                        g = 64;
                        b = 255;
                    }
                    if (i >= 69 && i <= 85)//5
                    {
                        r = 0;
                        g = 128;
                        b = 255;
                    }
                    if (i >= 86 && i <= 102)//6
                    {
                        r = 0;
                        g = 191;
                        b = 255;
                    }
                    if (i >= 103 && i <= 119)//7
                    {
                        r = 0;
                        g = 255;
                        b = 255;
                    }
                    if (i >= 120 && i <= 136)//8
                    {
                        r = 64;
                        g = 255;
                        b = 191;

                    }
                    if (i >= 137 && i <= 153)//9
                    {
                        r = 128;
                        g = 255;
                        b = 128;
                    }
                    if (i >= 154 && i <= 170)//10
                    {
                        r = 191;
                        g = 255;
                        b = 64;
                    }
                    if (i >= 171 && i <= 187)//11
                    {
                        r = 255;
                        g = 255;
                        b = 0;
                    }
                    if (i >= 188 && i <= 204)//12
                    {
                        r = 255;
                        g = 191;
                        b = 0;
                    }
                    if (i >= 205 && i <= 221)//13
                    {
                        r = 255;
                        g = 128;
                        b = 0;

                    }
                    if (i >= 222 && i <= 238)//14
                    {
                        r = 255;
                        g = 64;
                        b = 0;
                    }
                    if (i >= 239 && i <= 255)//15
                    {
                        r = 255;
                        g = 0;
                        b = 0;
                    }
                    pal.Entries[i] = Color.FromArgb(r, g, b);
                }

                //for (int j = 86; j < 172; ++j)
                //{


                //    pal.Entries[j] = Color.FromArgb(128, 128, j);//all the values exclude 0 are white 

                //}





                //for (int k = 172; k < 256; ++k)
                //{


                //    pal.Entries[k] = Color.FromArgb(255, k, 255);//all the values exclude 0 are white 

                //}

                //for (int i = 1; i < 256; ++i)
                //{
                //    pal.Entries[i] = Color.FromArgb(255, i, 255);
                //}




                //assign to bmp
                bmp.Palette = pal;

                //lock it to get the BitmapData Object
                System.Drawing.Imaging.BitmapData bmData =
                    bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                //copy the bytes
                System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmData.Scan0, bmData.Stride * bmData.Height);

                //never forget to unlock the bitmap
                bmp.UnlockBits(bmData);

                //display
                return bmp;
            }
        }
        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            ROIcheckBox.Focus();
            mousewheelvalue = e.Delta;
            resolutionnow += e.Delta;
            if (mouseevent == false && optflag == false)
            {



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


            }
            if (ROIshowCB.Checked)
            {

                PatternPB.Height = patroi_img.img.Height;
                PatternPB.Width = patroi_img.img.Width;
                OriginalPB.Height = patroi_img.img.Height;
                OriginalPB.Width = patroi_img.img.Width;
                var t = PatternPB.Size;
                var t1 = OriginalPB.Size;
                if (e.Delta > 0)
                {
                    t.Width = (int)(t.Width * rationew);
                    t.Height = (int)(t.Height * rationew);
                    t1.Width = (int)(t1.Width * rationew);
                    t1.Height = (int)(t1.Height * rationew);
                }
                else if (e.Delta < 0)
                {
                    t.Width = (int)(t.Width * rationew);
                    t.Height = (int)(t.Height * rationew);
                    t1.Width = (int)(t1.Width * rationew);
                    t1.Height = (int)(t1.Height * rationew);
                }
                else
                {
                    t.Width *= 1;
                    t.Height *= 1;
                    t1.Width *= 1;
                    t1.Height *= 1;
                }
                //t.Width += e.Delta;
                //t.Height += e.Delta;
                PatternPB.Size = t;
                OriginalPB.Size = t1;
            }
            //if (RectoptCB.Checked&&mouseright!=true)
            //{
            //    rectopt.Size = rectoptsize;
            //    rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
            //    optlefttop.X = (int)mousexvalue;
            //    optlefttop.Y = (int)mouseyvalue;
            //    //optlefttop = orgimg.rectopt_point_updating((int)mousexvalue, (int)mouseyvalue, rationew);
            //    rectopt.Location = optlefttop;
            //    rectopt.Size = rectnewsize;
            //    OriginalPB.Invalidate();
            //    PatternPB.Invalidate();
            //}
            //if(RectoptCB.Checked && mouseright == true)
            //{
            //    rectopt.Size = rectoptsize;
            //    rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
            //    optlefttop.X = (int)mousexvalue;
            //    optlefttop.Y = (int)mouseyvalue;
            //    //optlefttop = orgimg.rectopt_point_updating((int)mousexvalue, (int)mouseyvalue, rationew);
            //    rectopt.Location = optlefttop;
            //    rectopt.Size = rectnewsize;
            //    OriginalPB.Invalidate();
            //    PatternPB.Invalidate();
            //}
            //if(ROIshowCB.Checked)
            //{
            //    if (comboBox1.Text == "Dosage_2level")
            //    {
            //        patroi_img = new imgclass(dosage_2levelROI, "Dosage_2level_ROI", ptbh, ptbw, 1, 2);
            //        PatternPB.Height = ptbh;
            //        PatternPB.Width = ptbw;
            //        PatternPB.Image = null;
            //        PatternPB.SizeMode = PictureBoxSizeMode.CenterImage;
            //        //roiflag = true;
            //        PatternPB.Invalidate();
            //        OriginalPB.Invalidate();
            //    }
            //}


        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {
            mousewheelflag = false;
            if (ROIshowCB.Checked)
            {
                roiscroll = true;
            }
            else
            {
                roiscroll = false;
            }
            //if(patimg.img==null)
            //{
            //    patternclear = true;
            //    PatternPB.Invalidate();
            //}

        }
        private void ScrollableControl1_Scroll(Object sender, ScrollEventArgs e)
        {


        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

            //int pos0 = panel1.HorizontalScroll.Value;
            //int pos1 = panel1.VerticalScroll.Value;
        }

        private void OriginalPB_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void OriginalPB_Paint(object sender, PaintEventArgs e)
        {
            #region newversion
            if (buttonorgflag == false)
            {
                return;
            }
            if (!ROIshowCB.Checked)
            {

                if (oldxscroll != 0 && oldyscroll != 0)
                {

                    panel1.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                else if (oldxscroll == 0 && oldyscroll != 0)
                {
                    panel1.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                else if (oldxscroll != 0 && oldyscroll == 0)
                {
                    panel1.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                if (OriginalPB.Image != null)
                {
                    OriginalPB.Image = null;
                    OriginalPB.Refresh();
                }
            }
            int ptbw40082 = 0;
            int ptbh40082 = 0;
            int pos0 = panel1.HorizontalScroll.Value;
            int pos1 = panel1.VerticalScroll.Value;

            int posratio0 = pos0;//40000
            int posratio1 = pos1;//40000
            if (!ROIshowCB.Checked)
            {
                OriginalPB.SizeMode = PictureBoxSizeMode.Zoom;
                OriginalPB.Width = ptbmax;
                OriginalPB.Height = ptbmax;

                if (orgmeenterflag == true && rationew > oldratio && !ROIcheckBox.Checked)
                {
                    int newmousex;
                    int newmousey;


                    ptbw40082 = (int)(ptbw / rationew / 2);
                    ptbh40082 = (int)(ptbh / rationew / 2);
                    newmousex = (mousex - ptbw40082);//40082
                    newmousey = (mousey - ptbh40082);//40082
                    if (newmousex < 0)
                        newmousex = 0;
                    if (newmousey < 0)
                        newmousey = 0;
                    if (newmousex > tiffimg.width - ptbw)
                        newmousex = (tiffimg.width - ptbw);
                    if (newmousey > tiffimg.height - ptbh)
                        newmousey = (tiffimg.height - ptbh);
                    pos0 = newmousex;//40082
                    pos1 = newmousey;//40082
                    optlefttop.X += pos0;
                    optlefttop.Y += pos1;
                    posratio0 = (int)(Math.Round((((pos0) * ((ptbmax - ptbw) / (double)(magnifyratiowidth - 1 - ptbw)))) * orgimg.ratio));//40000
                    posratio1 = (int)(Math.Round((((pos1) * ((ptbmax - ptbh) / (double)(magnifyratioheight - 1 - ptbh)))) * orgimg.ratio));//40000

                    rectopt.Location = optlefttop;
                    panel1.AutoScrollPosition = new Point(posratio0, posratio1);
                    oldratio = rationew;
                    modifybymouse = true;
                }
                else
                {
                    modifybymouse = false;
                }

                int epsilon = 2000;//需更動
                                   //pos0 = (int)(Math.Round(((pos0 * ((double)(orgimg.h5img.colnum - 1 - ptbw) / (ptbmax - ptbw))))));

                //pos1 = (int)(Math.Round(((pos1 * ((double)(orgimg.h5img.rownum - 1 - ptbh) / (ptbmax - ptbh))))));
                if (modifybymouse == false)
                {
                    pos0 = (int)(Math.Round(((pos0 * ((double)(magnifyratiowidth - 1 - ptbw) / (ptbmax - ptbw)))) / orgimg.ratio));//40082
                    pos1 = (int)(Math.Round(((pos1 * ((double)(magnifyratioheight - 1 - ptbh) / (ptbmax - ptbh)))) / orgimg.ratio));//40082
                }

                //optlefttop.X += posratio0;
                //optlefttop.Y += posratio1;

                currentlefttoppos.X = pos0;
                currentlefttoppos.Y = pos1;
                currentrightbottompos.X = currentlefttoppos.X + (int)(ptbw / orgimg.ratio);
                currentrightbottompos.Y = currentlefttoppos.Y + (int)(ptbh / orgimg.ratio);
                TxtleftopxOri.Text = currentlefttoppos.X.ToString();
                TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
                TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
                TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();

                if (pos0 != orgpos0now || pos1 != orgpos1now)
                {
                    orgfresh = true;
                }
                else
                {
                    orgfresh = false;
                }
                orgpos0now = pos0;
                orgpos1now = pos1;

                if (orgimg.checkifupdate(pos0, pos1, epsilon))
                {
                    orgimg.updateimg(pos0, pos1);
                    if (comboflag == true)
                    {
                        if (patimg.img != null)
                        {
                            patimg.updateimg(pos0, pos1);
                        }
                        else
                        {
                            patternclear = true;
                        }
                        //else
                        //{

                        //}

                    }
                    else
                    {

                    }

                }
                //Rectangle Rectshowptb = new Rectangle(orgimg.realposlefttop.X, orgimg.realposlefttop.Y, ptbw, ptbh);
                //ImageAttributes imageAttr = new ImageAttributes();
                //Graphics.DrawImageAbort imageCallback
                //  = new Graphics.DrawImageAbort(DrawImageCallback8);
                //IntPtr imageCallbackData = new IntPtr(1);
                //GraphicsUnit units = GraphicsUnit.Pixel;
                ////e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
                //Rectangle Rectimage = new Rectangle(pos0, pos1, ptbw, ptbh);
                //e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw, ptbh, units);
                //panel2.AutoScrollPosition = new Point(pos0, pos1);

                //Rectangle Rectshowptb = new Rectangle(orgimg.realposlefttop.X, orgimg.realposlefttop.Y, ptbw, ptbh);
                ImageAttributes imageAttr = new ImageAttributes();
                Graphics.DrawImageAbort imageCallback
                  = new Graphics.DrawImageAbort(DrawImageCallback8);
                IntPtr imageCallbackData = new IntPtr(1);
                GraphicsUnit units = GraphicsUnit.Pixel;
                //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
                //int newptbw = (int)(ptbw * rationew);
                //int newptbh = (int)(ptbh * rationew);
                Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
                Rectangle Rectmouseimg = new Rectangle(posratio0, posratio1, ptbw, ptbh);
                if (orgfresh == true)
                {
                    OriginalPB.Refresh();
                    orgfresh = false;
                }
                //if (modifybymouse == true)
                //{
                //    e.Graphics.DrawImage(orgimg.img, Rectimage, pos0 - orgimg.realposlefttop.X, pos1 - orgimg.realposlefttop.Y, ptbw40000*2, ptbh40000 * 2, units);
                //}
                //else
                {
                    e.Graphics.DrawImage(orgimg.img, Rectimage, pos0 - orgimg.realposlefttop.X, pos1 - orgimg.realposlefttop.Y, ptbw / orgimg.ratio, ptbh / orgimg.ratio, units);
                }
               




                panel2.AutoScrollPosition = new Point(posratio0, posratio1);
                if (mouseevent == true)
                {
                    if (RectOriginal != null && RectOriginal.Width > 0 && RectOriginal.Height > 0)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.Red, 2), RectOriginal);

                    }
                }
                if (RectoptCB.Checked && rectoptflag == true && optflag == true)
                {
                    e.Graphics.DrawRectangle(new Pen(Color.Red, 2), rectopt);

                }


            }
            else
            {
                if (checkroiflag == true)
                {
                    if (orgroi_img.img == null)
                    {
                        return;
                    }

                    OriginalPB.Image = orgroi_img.img;
                    checkroiflag = false;

                }
                if (roiscroll == true)
                {
                    pos0 = panel1.HorizontalScroll.Value;
                    pos1 = panel1.VerticalScroll.Value;

                    panel2.AutoScrollPosition = new Point(pos0, pos1);
                }



            }


            //ROIcheckBox.Focus();
            //panel1.Focus();
            //panel2.Focus();


            #endregion newversion



            #region oldversion
            //bool oldflag = false;
            //if (oldflag)
            //{
            //    if (Txtratio.Text == "100")
            //    {
            //        int pos0 = panel1.HorizontalScroll.Value;
            //        int pos1 = panel1.VerticalScroll.Value;
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 + ptbw;
            //        currentrightbottompos.Y = pos1 + ptbh;
            //        TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //        //int currentarrayx2 = pos0 + ptbw + buffercol;
            //        //int currentarrayy2 = pos1 + ptbh + bufferrow;

            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;
            //        //mousewheelflag = false;
            //        OriginalPB.Height = orgh5image.rownum - 1;
            //        OriginalPB.Width = orgh5image.colnum - 1;

            //        if (comboBox1.Text == "Dosage")
            //        {
            //            PatternPB.Height = dosageh5image.rownum - 1;
            //            PatternPB.Width = dosageh5image.colnum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            PatternPB.Height = ldh5image.rownum - 1;
            //            PatternPB.Width = ldh5image.colnum - 1;
            //        }
            //        //PatternPB.Height=40082;
            //        //PatternPB.Width = 40082;
            //        orgheight = OriginalPB.Height;
            //        orgwidth = OriginalPB.Width;
            //        patheight = PatternPB.Height;
            //        patwidth = PatternPB.Width;
            //        int epsilon = 2000;

            //        int maxheightsize = orgh5image.rownum - 1;
            //        int maxwidthsize = orgh5image.colnum - 1;
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {
            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }



            //        }

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(pos0, pos1, ptbw, ptbh);
            //        e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw, ptbh, units);
            //        panel2.AutoScrollPosition = new Point(pos0, pos1);
            //    }
            //    else //抓ptb放大後的位置
            //    {
            //        if (ptbmaxsizeall == true)
            //        {

            //            int h = OriginalPB.Height;
            //            int w = OriginalPB.Width;
            //            int pos0 = panel1.HorizontalScroll.Value;
            //            int pos1 = panel1.VerticalScroll.Value;
            //            int posratio0 = pos0;
            //            int posratio1 = pos1;

            //            pos0 = (int)(Math.Round(((pos0 * ((double)(magnifyratiowidth - 1 - ptbw) / (ptbmax - ptbw)))) / rationew));
            //            pos1 = (int)(Math.Round(((pos1 * ((double)(magnifyratioheight - 1 - ptbh) / (ptbmax - ptbh)))) / rationew));
            //            //pos0 = (int)Math.Round(pos0 *(float)(orgh5image.colnum - 1 ) / (ptbmax));
            //            //pos1 = (int)Math.Round(pos1 * (float)(orgh5image.rownum - 1) / (ptbmax));
            //            //int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //            //int orgratiopatterny=(int)((pos1 - ptbh * rationew) / rationew);
            //            if (pos0 != pos0now || pos1 != pos1now)
            //            {
            //                orgfresh = true;
            //            }
            //            else
            //            {
            //                orgfresh = false;
            //            }
            //            pos0now = pos0;
            //            pos1now = pos1;
            //            //pos0 = (int)(pos0 / rationew);
            //            //pos1 = (int)(pos1 / rationew);
            //            int ptbwrealpos = 0;
            //            int ptbhrealpos = 0;
            //            int ptbw40082 = 0;
            //            int ptbh40082 = 0;


            //            //ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbh40082 = (int)(ptbh40082 / rationew);
            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 = (int)(ptbw40082 / rationew);
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //            currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //            TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);


            //            //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //            //{

            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //            //{
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //            //{
            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}

            //            //int currentarrayx2 = pos0 + ptbw + buffercol;
            //            //int currentarrayy2 = pos1 + ptbh + bufferrow;

            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxheightsize = orgh5image.rownum - 1;
            //            int maxwidthsize = orgh5image.colnum - 1;
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {
            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }



            //            }

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            int newptbw = (int)(ptbw * rationew);
            //            int newptbh = (int)(ptbh * rationew);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (orgfresh == true)
            //            {
            //                OriginalPB.Refresh();
            //                orgfresh = false;
            //            }

            //            e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel2.AutoScrollPosition = new Point(posratio0, posratio1);

            //            ROIcheckBox.Focus();
            //            panel1.Focus();
            //            panel2.Focus();

            //        }
            //        else if (ptbmaxsizeheight == true)
            //        {

            //            int h = OriginalPB.Height;
            //            int w = OriginalPB.Width;
            //            int pos0 = panel1.HorizontalScroll.Value;
            //            int pos1 = panel1.VerticalScroll.Value;
            //            int posratio0 = pos0;
            //            int posratio1 = pos1;
            //            //pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            pos1 = (int)(Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh)))) / rationew);
            //            int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //            int orgratiopatterny = (int)((pos1 - ptbh * rationew) / rationew);
            //            pos0 = (int)(pos0 / rationew);
            //            //pos1 = (int)(pos1 / rationew);
            //            //int ptbw40082 = 0;
            //            //int ptbh40082 = 0;
            //            //ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbh40082 = (int)(ptbh40082 / rationew);
            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 = (int)(ptbw40082 / rationew);
            //            if (pos0 != pos0now || pos1 != pos1now)
            //            {
            //                orgfresh = true;
            //            }
            //            else
            //            {
            //                orgfresh = false;
            //            }
            //            pos0now = pos0;
            //            pos1now = pos1;
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //            currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //            TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);


            //            //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //            //{

            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //            //{
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //            //{
            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}


            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxheightsize = orgh5image.rownum - 1;
            //            int maxwidthsize = orgh5image.colnum - 1;
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {
            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }



            //            }

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            int newptbw = (int)(ptbw * rationew);
            //            int newptbh = (int)(ptbh * rationew);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (orgfresh == true)
            //            {
            //                OriginalPB.Refresh();
            //                orgfresh = false;
            //            }
            //            e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel2.AutoScrollPosition = new Point(posratio0, posratio1);


            //        }
            //        else if (ptbmaxsizewidth == true)
            //        {

            //            int h = OriginalPB.Height;
            //            int w = OriginalPB.Width;
            //            int pos0 = panel1.HorizontalScroll.Value;
            //            int pos1 = panel1.VerticalScroll.Value;
            //            int posratio0 = pos0;
            //            int posratio1 = pos1;
            //            pos0 = (int)(Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw)))) / rationew);
            //            //pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //            //int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //            //int orgratiopatterny = (int)((pos1 - ptbh * rationew) / rationew);
            //            //pos0 = (int)(pos0 / rationew);
            //            pos1 = (int)(pos1 / rationew);
            //            int ptbw40082 = 0;
            //            if (pos0 != pos0now || pos1 != pos1now)
            //            {
            //                orgfresh = true;
            //            }
            //            else
            //            {
            //                orgfresh = false;
            //            }
            //            pos0now = pos0;
            //            pos1now = pos1;
            //            //int ptbh40082 = 0;
            //            //ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbh40082 = (int)(ptbh40082 / rationew);
            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 = (int)(ptbw40082 / rationew);
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //            currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //            TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);


            //            //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //            //{

            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //            //{
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //            //{
            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}

            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxheightsize = orgh5image.rownum - 1;
            //            int maxwidthsize = orgh5image.colnum - 1;
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {
            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }



            //            }

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            int newptbw = (int)(ptbw * rationew);
            //            int newptbh = (int)(ptbh * rationew);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (orgfresh == true)
            //            {
            //                OriginalPB.Refresh();
            //                orgfresh = false;
            //            }
            //            e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel2.AutoScrollPosition = new Point(posratio0, posratio1);

            //        }
            //        else if (ptbminsizeall == true)
            //        {
            //            int h = OriginalPB.Height;
            //            int w = OriginalPB.Width;
            //            int pos0 = panel1.HorizontalScroll.Value;
            //            int pos1 = panel1.VerticalScroll.Value;
            //            int posratio0 = pos0;
            //            int posratio1 = pos1;



            //            pos0 = (int)(pos0 / rationew);
            //            pos1 = (int)(pos1 / rationew);
            //            if (pos0 != pos0now || pos1 != pos1now)
            //            {
            //                orgfresh = true;
            //            }
            //            else
            //            {
            //                orgfresh = false;
            //            }
            //            pos0now = pos0;
            //            pos1now = pos1;
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //            currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //            TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);


            //            int currentarrayx2 = pos0 + ptbw + buffercol;
            //            int currentarrayy2 = pos1 + ptbh + bufferrow;

            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = (int)((orgh5image.rownum) * 0.13);

            //            int maxheightsize = orgh5image.rownum - 1;
            //            int maxwidthsize = orgh5image.colnum - 1;
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {
            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                    currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, pos0, pos1, rationew);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                    currentarraytupleld = calculateposldfloatzoomout(ldimgpath, pos0, pos1, rationew);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }



            //            }

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            int newptbw = (int)(ptbw * rationew);
            //            int newptbh = (int)(ptbh * rationew);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (orgfresh == true)
            //            {
            //                OriginalPB.Refresh();
            //                orgfresh = false;
            //            }
            //            e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel2.AutoScrollPosition = new Point(posratio0, posratio1);

            //        }
            //        else if (ptbminsizeheight == true)//不需要
            //        {

            //            {

            //                int h = OriginalPB.Height;
            //                int w = OriginalPB.Width;
            //                int pos0 = panel1.HorizontalScroll.Value;
            //                int pos1 = panel1.VerticalScroll.Value;
            //                int posratio0 = pos0;
            //                int posratio1 = pos1;
            //                //pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //                pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //                int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //                int orgratiopatterny = (int)((pos1 - ptbh * rationew) / rationew);
            //                pos0 = (int)(pos0 / rationew);
            //                pos1 = (int)(pos1 / rationew);
            //                currentlefttoppos.X = pos0;
            //                currentlefttoppos.Y = pos1;
            //                currentrightbottompos.X = pos0 + ptbw;
            //                currentrightbottompos.Y = pos1 + ptbh;
            //                TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //                TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //                TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //                TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //                int ptbhratio = (int)(ptbh * rationew);
            //                int ptbwratio = (int)(ptbw * rationew);


            //                //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //                //{

            //                //    pos1 = (int)((h - ptbhratio) / rationew);
            //                //}
            //                //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //                //{
            //                //    pos0 = (int)((w - ptbwratio) / rationew);
            //                //}
            //                //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //                //{
            //                //    pos1 = (int)((h - ptbhratio) / rationew);
            //                //    pos0 = (int)((w - ptbwratio) / rationew);
            //                //}


            //                Tuple<int[], byte[,]> currentarraytuple;
            //                Tuple<int[], byte[,]> currentarraytupledosage;
            //                Tuple<int[], byte[,]> currentarraytupleld;


            //                int epsilon = 2000;
            //                int maxheightsize = orgh5image.rownum - 1;
            //                int maxwidthsize = orgh5image.colnum - 1;
            //                if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //                {

            //                }
            //                else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //                {

            //                }
            //                else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //                {

            //                }
            //                else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //                {
            //                    if (comboBox1.Text == "Dosage")
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }
            //                    else if (comboBox1.Text == "LDFile")
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }
            //                    else
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }



            //                }

            //                Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //                ImageAttributes imageAttr = new ImageAttributes();
            //                Graphics.DrawImageAbort imageCallback
            //                  = new Graphics.DrawImageAbort(DrawImageCallback8);
            //                IntPtr imageCallbackData = new IntPtr(1);
            //                GraphicsUnit units = GraphicsUnit.Pixel;
            //                //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //                int newptbw = (int)(ptbw * rationew);
            //                int newptbh = (int)(ptbh * rationew);
            //                Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //                e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel2.AutoScrollPosition = new Point(posratio0, posratio1);


            //            }
            //        }
            //        else if (ptbminsizewidth == true)//不需要
            //        {

            //            {

            //                int h = OriginalPB.Height;
            //                int w = OriginalPB.Width;
            //                int pos0 = panel1.HorizontalScroll.Value;
            //                int pos1 = panel1.VerticalScroll.Value;
            //                int posratio0 = pos0;
            //                int posratio1 = pos1;
            //                pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //                //pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //                //int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //                //int orgratiopatterny = (int)((pos1 - ptbh * rationew) / rationew);
            //                pos0 = (int)(pos0 / rationew);
            //                pos1 = (int)(pos1 / rationew);
            //                currentlefttoppos.X = pos0;
            //                currentlefttoppos.Y = pos1;
            //                currentrightbottompos.X = pos0 + ptbw;
            //                currentrightbottompos.Y = pos1 + ptbh;
            //                TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //                TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //                TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //                TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //                int ptbhratio = (int)(ptbh * rationew);
            //                int ptbwratio = (int)(ptbw * rationew);


            //                //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //                //{

            //                //    pos1 = (int)((h - ptbhratio) / rationew);
            //                //}
            //                //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //                //{
            //                //    pos0 = (int)((w - ptbwratio) / rationew);
            //                //}
            //                //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //                //{
            //                //    pos1 = (int)((h - ptbhratio) / rationew);
            //                //    pos0 = (int)((w - ptbwratio) / rationew);
            //                //}

            //                Tuple<int[], byte[,]> currentarraytuple;
            //                Tuple<int[], byte[,]> currentarraytupledosage;
            //                Tuple<int[], byte[,]> currentarraytupleld;


            //                int epsilon = 2000;
            //                int maxheightsize = orgh5image.rownum - 1;
            //                int maxwidthsize = orgh5image.colnum - 1;
            //                if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //                {

            //                }
            //                else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //                {

            //                }
            //                else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //                {

            //                }
            //                else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //                {

            //                }
            //                else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //                {
            //                    if (comboBox1.Text == "Dosage")
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }
            //                    else if (comboBox1.Text == "LDFile")
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }
            //                    else
            //                    {
            //                        currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                        orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                        realposlefttop.X = currentarraytuple.Item1[0];
            //                        realposlefttop.Y = currentarraytuple.Item1[1];
            //                        realposrightbottom.X = currentarraytuple.Item1[2];
            //                        realposrightbottom.Y = currentarraytuple.Item1[3];
            //                        //OriginalPB.Invalidate();
            //                        //PatternPB.Invalidate();
            //                    }



            //                }

            //                Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //                ImageAttributes imageAttr = new ImageAttributes();
            //                Graphics.DrawImageAbort imageCallback
            //                  = new Graphics.DrawImageAbort(DrawImageCallback8);
            //                IntPtr imageCallbackData = new IntPtr(1);
            //                GraphicsUnit units = GraphicsUnit.Pixel;
            //                //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //                int newptbw = (int)(ptbw * rationew);
            //                int newptbh = (int)(ptbh * rationew);
            //                Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //                e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel2.AutoScrollPosition = new Point(posratio0, posratio1);


            //            }
            //        }
            //        else
            //        {
            //            //int h = OriginalPB.Height;
            //            //int w = OriginalPB.Width;
            //            int pos0 = panel1.HorizontalScroll.Value;
            //            int pos1 = panel1.VerticalScroll.Value;
            //            pos0 = (int)(pos0 / rationew);
            //            pos1 = (int)(pos1 / rationew);
            //            if (pos0 != pos0now || pos1 != pos1now)
            //            {
            //                orgfresh = true;
            //            }
            //            else
            //            {
            //                orgfresh = false;
            //            }
            //            pos0now = pos0;
            //            pos1now = pos1;
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = currentlefttoppos.X + (int)(ptbw / rationew);
            //            currentrightbottompos.Y = currentlefttoppos.Y + (int)(ptbh / rationew);
            //            TxtleftopxOri.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyOri.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxOri.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyOri.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);
            //            int posratio0 = panel1.HorizontalScroll.Value;
            //            int posratio1 = panel1.VerticalScroll.Value;
            //            //if (pos1 >= 40082 - ptbh&&pos0<= 40082 - ptbw)
            //            //{

            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw&& pos1<= 40082 - ptbh)
            //            //{
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}
            //            //else if (pos0 >= 40082 - ptbw && pos1 > 40082 - ptbh)
            //            //{
            //            //    pos1 = (int)((h - ptbhratio) / rationew);
            //            //    pos0 = (int)((w - ptbwratio) / rationew);
            //            //}

            //            //int currentarrayx2 = pos0 + ptbw + buffercol;
            //            //int currentarrayy2 = pos1 + ptbh + bufferrow;

            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxheightsize = orgh5image.rownum - 1;
            //            int maxwidthsize = orgh5image.colnum - 1;
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {
            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosagefloat(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposldfloat(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorgfloat(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }



            //            }

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            int newptbw = (int)(ptbw * rationew);
            //            int newptbh = (int)(ptbh * rationew);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (orgfresh == true)
            //            {
            //                OriginalPB.Refresh();
            //                orgfresh = false;
            //            }
            //            e.Graphics.DrawImage(orgimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel2.AutoScrollPosition = new Point(posratio0, posratio1);

            //        }
            //    }
            //}

            #endregion oldversion
        }
        private Tuple<int[], byte[,]> calculateposorg(string path, int pos0, int pos1)
        {
            orgh5image.openfile(path);

            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = pos0 - buffercol;
            int currentarrayy1 = pos1 - bufferrow;
            int currentarrayx2 = pos0 + ptbw + buffercol;
            int currentarrayy2 = pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= orgh5image.colnum)
            {
                currentarrayx2 = orgh5image.colnum - 1;
            }
            if (currentarrayy2 >= orgh5image.rownum)
            {
                currentarrayy2 = orgh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = orgh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            orgh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposld(string path, int pos0, int pos1)
        {
            ldh5image.openfile(path);
            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = pos0 - buffercol;
            int currentarrayy1 = pos1 - bufferrow;
            int currentarrayx2 = pos0 + ptbw + buffercol;
            int currentarrayy2 = pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= ldh5image.colnum)
            {
                currentarrayx2 = ldh5image.colnum - 1;
            }
            if (currentarrayy2 >= ldh5image.rownum)
            {
                currentarrayy2 = ldh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = ldh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            ldh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposdosage(string path, int pos0, int pos1)
        {
            dosageh5image.openfile(path);
            float[,] currentarrayfloat;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = pos0 - buffercol;
            int currentarrayy1 = pos1 - bufferrow;
            int currentarrayx2 = pos0 + ptbw + buffercol;
            int currentarrayy2 = pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= dosageh5image.colnum)
            {
                currentarrayx2 = dosageh5image.colnum - 1;
            }
            if (currentarrayy2 >= dosageh5image.rownum)
            {
                currentarrayy2 = dosageh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarrayfloat = dosageh5image.readfloatdata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            dosageh5image.closefile();
            int m = currentarrayfloat.GetLength(0);
            int n = currentarrayfloat.GetLength(1);
            byte[,] currentarray = new byte[m, n];
            float min1 = float.MaxValue, max1 = 0;
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                {
                    if (max1 < currentarrayfloat[i, j])
                    { max1 = currentarrayfloat[i, j]; }
                    if (min1 > currentarrayfloat[i, j])
                    { min1 = currentarrayfloat[i, j]; }
                }
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    currentarrayfloat[i, j] = (currentarrayfloat[i, j] - min1) / (max1 - min1);
                    currentarray[i, j] = (byte)(currentarrayfloat[i, j] * 255);
                }
            }
            //OriginalPB.Invalidate();

            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposorgfloat(string path, float pos0, float pos1)
        {
            orgh5image.openfile(path);
            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = (int)(pos0 - buffercol);
            int currentarrayy1 = (int)(pos1 - bufferrow);
            int currentarrayx2 = (int)(pos0 + ptbw + buffercol);
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= orgh5image.colnum)
            {
                currentarrayx2 = orgh5image.colnum - 1;
            }
            if (currentarrayy2 >= orgh5image.rownum)
            {
                currentarrayy2 = orgh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = orgh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            orgh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposdosagefloat(string path, float pos0, float pos1)
        {
            dosageh5image.openfile(path);
            float[,] currentarrayfloat;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = (int)pos0 - buffercol;
            int currentarrayy1 = (int)pos1 - bufferrow;
            int currentarrayx2 = (int)pos0 + ptbw + buffercol;
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= dosageh5image.colnum)
            {
                currentarrayx2 = dosageh5image.colnum - 1;
            }
            if (currentarrayy2 >= dosageh5image.rownum)
            {
                currentarrayy2 = dosageh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarrayfloat = dosageh5image.readfloatdata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            dosageh5image.closefile();
            int m = currentarrayfloat.GetLength(0);
            int n = currentarrayfloat.GetLength(1);
            byte[,] currentarray = new byte[m, n];
            float min1 = float.MaxValue, max1 = 0;
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                {
                    if (max1 < currentarrayfloat[i, j])
                    { max1 = currentarrayfloat[i, j]; }
                    if (min1 > currentarrayfloat[i, j])
                    { min1 = currentarrayfloat[i, j]; }
                }
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    currentarrayfloat[i, j] = (currentarrayfloat[i, j] - min1) / (max1 - min1);
                    currentarray[i, j] = (byte)(currentarrayfloat[i, j] * 255);
                }
            }
            //OriginalPB.Invalidate();

            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposldfloat(string path, float pos0, float pos1)
        {
            ldh5image.openfile(path);
            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];


            int currentarrayx1 = (int)pos0 - buffercol;
            int currentarrayy1 = (int)pos1 - bufferrow;
            int currentarrayx2 = (int)pos0 + ptbw + buffercol;
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= ldh5image.colnum)
            {
                currentarrayx2 = ldh5image.colnum - 1;
            }
            if (currentarrayy2 >= ldh5image.rownum)
            {
                currentarrayy2 = ldh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = ldh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            ldh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposorgfloatzoomout(string path, float pos0, float pos1, float ratio)
        {
            orgh5image.openfile(path);
            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];
            buffercol = (int)(ptbw / ratio);
            bufferrow = (int)(ptbh / ratio);


            int currentarrayx1 = (int)(pos0 - buffercol);
            int currentarrayy1 = (int)(pos1 - bufferrow);
            int currentarrayx2 = (int)(pos0 + ptbw + buffercol);
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= orgh5image.colnum)
            {
                currentarrayx2 = orgh5image.colnum - 1;
            }
            if (currentarrayy2 >= orgh5image.rownum)
            {
                currentarrayy2 = orgh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = orgh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            orgh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposdosagefloatzoomout(string path, float pos0, float pos1, float ratio)
        {
            dosageh5image.openfile(path);
            float[,] currentarrayfloat;
            int[] currentarraymatrix = new int[4];
            buffercol = (int)(ptbfloatw / ratio);
            bufferrow = (int)(ptbfloath / ratio);

            int currentarrayx1 = (int)pos0 - buffercol;
            int currentarrayy1 = (int)pos1 - bufferrow;
            int currentarrayx2 = (int)pos0 + ptbw + buffercol;
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= dosageh5image.colnum)
            {
                currentarrayx2 = dosageh5image.colnum - 1;
            }
            if (currentarrayy2 >= dosageh5image.rownum)
            {
                currentarrayy2 = dosageh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarrayfloat = dosageh5image.readfloatdata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            dosageh5image.closefile();
            int m = currentarrayfloat.GetLength(0);
            int n = currentarrayfloat.GetLength(1);
            byte[,] currentarray = new byte[m, n];
            float min1 = float.MaxValue, max1 = 0;
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                {
                    if (max1 < currentarrayfloat[i, j])
                    { max1 = currentarrayfloat[i, j]; }
                    if (min1 > currentarrayfloat[i, j])
                    { min1 = currentarrayfloat[i, j]; }
                }
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    currentarrayfloat[i, j] = (currentarrayfloat[i, j] - min1) / (max1 - min1);
                    currentarray[i, j] = (byte)(currentarrayfloat[i, j] * 255);
                }
            }
            //OriginalPB.Invalidate();

            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private Tuple<int[], byte[,]> calculateposldfloatzoomout(string path, float pos0, float pos1, float ratio)
        {
            ldh5image.openfile(path);
            byte[,] currentarray;
            int[] currentarraymatrix = new int[4];
            buffercol = (int)(ptbfloatw / ratio);
            bufferrow = (int)(ptbfloath / ratio);

            int currentarrayx1 = (int)pos0 - buffercol;
            int currentarrayy1 = (int)pos1 - bufferrow;
            int currentarrayx2 = (int)pos0 + ptbw + buffercol;
            int currentarrayy2 = (int)pos1 + ptbh + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;

            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= ldh5image.colnum)
            {
                currentarrayx2 = ldh5image.colnum - 1;
            }
            if (currentarrayy2 >= ldh5image.rownum)
            {
                currentarrayy2 = ldh5image.rownum - 1;

            }
            currentarraymatrix[0] = currentarrayx1;
            currentarraymatrix[1] = currentarrayy1;
            currentarraymatrix[2] = currentarrayx2;
            currentarraymatrix[3] = currentarrayy2;
            currentarray = ldh5image.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            ldh5image.closefile();
            return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);

        }
        private void OriginalPB_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (ROIshowCB.Checked)
                {
                    return;
                }
                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;
                RectStartPoint = e.Location;
                ulong rectstartponitx = (ulong)RectStartPoint.X & 0xffff;
                ulong rectstartponity = (ulong)RectStartPoint.Y & 0xffff;

                ROIlefttop.X = mousescalex(rectstartponitx, rationew, pos0, currentlefttoppos.X);//40082
                ROIlefttop.Y = mousescaley(rectstartponity, rationew, pos1, currentlefttoppos.Y);//40082
                RectStartPoint.X = (int)rectstartponitx;//40000
                RectStartPoint.Y = (int)rectstartponity;//40000

                TxtlefttopxROI.Text = Convert.ToString(ROIlefttop.X);
                TxtlefttopyROI.Text = Convert.ToString(ROIlefttop.Y);


                down = true;

            }
            else if (e.Button == MouseButtons.Right)
            {
                //bool success;
                //bool success1;
                //if (TxtRectX.Text != "" && TxtRectY.Text != "")
                //{
                //    success = int.TryParse(TxtRectX.Text, out int result);
                //    if (success == true)
                //    {
                //        newrectx = result;
                //    }
                //    success1 = int.TryParse(TxtRectY.Text, out int result1);
                //    if (success1 == true)
                //    {
                //        newrecty = result1;
                //    }
                //    rectopt.Size = new Size(newrectx, newrecty);

                //}
                //else if (TxtRectX.Text != "" && TxtRectY.Text == "")
                //{
                //    success = int.TryParse(TxtRectX.Text, out int result);
                //    if (success == true)
                //    {
                //        newrectx = result;
                //    }
                //    rectopt.Size = new Size(newrectx, 300);
                //}
                //else if (TxtRectX.Text == "" && TxtRectY.Text != "")
                //{
                //    success1 = int.TryParse(TxtRectY.Text, out int result1);
                //    if (success1 == true)
                //    {
                //        newrecty = result1;
                //    }
                //    rectopt.Size = new Size(300, newrecty);
                //}
                //else
                //{
                //    rectopt.Size = rectoptsize;
                //}
                RectOriginal.Size = new Size(0, 0);
                RectPattern.Size = new Size(0, 0);
                optlefttop.X = (int)mousexvalue;
                optlefttop.Y = (int)mouseyvalue;
                int mousex40082;
                int mousey40082;
                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;
                mousex40082 = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);//40082
                mousey40082 = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);//40082
                TxtlefttopxROI.Text = Convert.ToString(mousex40082);
                TxtlefttopyROI.Text = Convert.ToString(mousey40082);

                //optlefttop = orgimg.rectopt_point_updating((int)mousexvalue, (int)mouseyvalue, rationew);
                rectoptflag = true;
                //rectnewsize40000= orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                rectnewsize = orgimg.rectopt_updating_1(rectopt.Width, rectopt.Height, rationew);
                //rectopt.Size = rectnewsize40000;
                TxtrightbottomxROI.Text = Convert.ToString(mousex40082 + rectnewsize.Width);
                TxtrightbottomyROI.Text = Convert.ToString(mousey40082 + rectnewsize.Height);
                if (TxtlefttopxROI.Text != "" && TxtlefttopyROI.Text != "" && TxtrightbottomxROI.Text != "" && TxtrightbottomyROI.Text != "")
                {
                    int roiheight = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roiwidth = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                    TxtroiHeight.Text = Convert.ToString(roiheight);
                    TxtroiWidth.Text = Convert.ToString(roiwidth);
                }
                rectopt.Location = optlefttop;
                mouseright = true;
                if (!RectoptCB.Checked)
                {
                    TxtlefttopyROI.Text = "";
                    TxtlefttopxROI.Text = "";
                    TxtrightbottomyROI.Text = "";
                    TxtrightbottomxROI.Text = "";
                }
                else
                {

                }

                OriginalPB.Invalidate();
                PatternPB.Invalidate();
            }
            else
            {
                return;
            }



        }
        public int mousescalex(ulong x, float ratio, int pos0, int currentx)
        {

            float x1 = (int)x - pos0;//計算40000座標系
            float xr = x1 / (ptbw);//計算40000座標系
            int scalex = (int)(ptbw / ratio * xr);//計算40082座標系

            return (scalex + currentx);
        }
        public int mousescaley(ulong y, float ratio, int pos1, int currenty)
        {
            float y1 = (int)y - pos1;//計算40000座標系
            float yr = y1 / (ptbh);//計算40000座標系
            int scaley = (int)(ptbh / ratio * yr);//計算40082座標系
            return (scaley + currenty);
        }


        private void OriginalPB_MouseMove(object sender, MouseEventArgs e)
        {

            if (!RectoptCB.Checked)
            {

                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;

                mousex = e.X;
                mousey = e.Y;
                mousexvalue = (ulong)e.X & 0xffff;//40000
                mouseyvalue = (ulong)e.Y & 0xffff;//40000
                mouseratiox = (ulong)e.X & 0xffff;//40000
                mouseratioy = (ulong)e.Y & 0xffff;//40000

                mousex = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);//40082
                mousey = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);//40082

                Txtmousex.Text = Convert.ToString(mousex);
                Txtmousey.Text = Convert.ToString(mousey);
                if (e.Button != MouseButtons.Left)
                    return;
                Point tempEndPoint = e.Location;
                int tempEndPointx = tempEndPoint.X;
                int tempEndPointy = tempEndPoint.Y;
                ulong tempEndPointxul = (ulong)tempEndPoint.X & 0xffff;//40000
                ulong tempEndPointyul = (ulong)tempEndPoint.Y & 0xffff;//40000

                tempEndPoint.X = (int)tempEndPointxul;//40000
                tempEndPoint.Y = (int)tempEndPointyul;//40000
                RectOriginal.Location = new Point(Math.Min(RectStartPoint.X, tempEndPoint.X), Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                RectPattern.Location = new Point(Math.Min(RectStartPoint.X, tempEndPoint.X), Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                RectOriginal.Size = new Size(
                    Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
                RectPattern.Size = new Size(
                    Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            }
            else
            {
                //    if (OriginalPB.Image == null)
                //    {
                //        //MessageBox.Show("請由下拉式選單選擇想要觀看的Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }

                if (e.Button != MouseButtons.Left && RectoptCB.Checked && mouseright != true)
                {
                    if (orgflag == false)
                    {
                        return;
                    }
                    int pos0 = panel1.HorizontalScroll.Value;
                    int pos1 = panel1.VerticalScroll.Value;
                    mousex = e.X;
                    mousey = e.Y;
                    mousexvalue = (ulong)e.X & 0xffff;//40000
                    mouseyvalue = (ulong)e.Y & 0xffff;//40000

                    mousex = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);
                    mousey = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);

                    Txtmousex.Text = Convert.ToString(mousex);
                    Txtmousey.Text = Convert.ToString(mousey);
                    rectoptflag = true;
                    bool success;
                    bool success1;
                    if (TxtRectX.Text != "" && TxtRectY.Text != "")
                    {
                        success = int.TryParse(TxtRectX.Text, out int result);
                        if (success == true)
                        {
                            newrectx = result;
                        }
                        success1 = int.TryParse(TxtRectY.Text, out int result1);
                        if (success1 == true)
                        {
                            newrecty = result1;
                        }
                        rectopt.Size = new Size(newrectx, newrecty);

                    }
                    else if (TxtRectX.Text != "" && TxtRectY.Text == "")
                    {
                        success = int.TryParse(TxtRectX.Text, out int result);
                        if (success == true)
                        {
                            newrectx = result;
                        }
                        rectopt.Size = new Size(newrectx, 300);
                    }
                    else if (TxtRectX.Text == "" && TxtRectY.Text != "")
                    {
                        success1 = int.TryParse(TxtRectY.Text, out int result1);
                        if (success1 == true)
                        {
                            newrecty = result1;
                        }
                        rectopt.Size = new Size(300, newrecty);
                    }
                    else
                    {
                        rectopt.Size = rectoptsize;
                    }
                    rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                    optlefttop.X = (int)mousexvalue;
                    optlefttop.Y = (int)mouseyvalue;
                    rectopt.Location = optlefttop;
                    rectopt.Size = rectnewsize;
                    OriginalPB.Invalidate();
                    PatternPB.Invalidate();
                    mouseright = false;
                }
                if (e.Button == MouseButtons.Right && RectoptCB.Checked)
                {
                    //rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                    //rectoptflag = true;
                    //rectopt.Size = rectoptsize;
                    //rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                    //rectopt.Size = rectnewsize;
                    //rectopt.Location = optlefttop;
                    //mouseright = true;
                }
            }



        }

        private void OriginalPB_MouseClick(object sender, MouseEventArgs e)
        {
            if (mouseevent == true)
            {
                // Originalflag = true;
                if (e.Button == MouseButtons.Left)
                {
                    return;
                }
                else if (e.Button == MouseButtons.Right)
                {
                    RectOriginal.Size = new Size(0, 0);
                    RectPattern.Size = new Size(0, 0);
                }
                else
                {
                    return;
                }

            }

        }

        private void OriginalPB_MouseEnter(object sender, EventArgs e)
        {
            orgmeenterflag = true;


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guimainflag == false)
            {

                MessageBox.Show("請先按下Original按鈕選擇原始Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (combochange == true)
            {
                combochange = false;
            }
            else
            {

                comboflag = true;

            }

            int[] size = new int[4];
            //roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
            //roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
            //roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level.h5";
            //ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi"+ roimousexy+".h5";
            //ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi"+ roimousexy+".h5";
            //dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi"+ roimousexy+".h5";
            //dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi"+ roimousexy+".h5";
            //Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi"+ roimousexy+".h5";
            //Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi"+ roimousexy+".h5";
            //xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level"+ roimousexy+".h5";
            //xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level"+ roimousexy+".h5";
            if (ROIshowCB.Checked)
            {
                oldxscroll = panel1.HorizontalScroll.Value;
                oldyscroll = panel1.VerticalScroll.Value;
                //roi_path = Originalpath + "_roi.h5";
                //roi_4level_path = Originalpath + "_roi_4level.h5";
                if (comboBox1.Text == "Dosage_2level" || comboBox1.Text == "LDFile_2level" || comboBox1.Text == "Threshold_2level" || comboBox1.Text == "XOR_2level")
                {
                    orgroi_img = new imgclass(roi_path, "roi.h5", ptbh, ptbw, 1, 2);
                }
                else if (comboBox1.Text == "Dosage_4level" || comboBox1.Text == "LDFile_4level" || comboBox1.Text == "Threshold_4level" || comboBox1.Text == "XOR_4level")
                {
                    orgroi_img = new imgclass(roi_4level_path, "roi_4level.h5", ptbh, ptbw, 1, 2);
                }

                if (orgroi_img.img == null)
                {
                    OriginalPB.Image = null;
                    PatternPB.Image = null;
                    return;
                }
                OriginalPB.Height = orgroi_img.img.Height;
                OriginalPB.Width = orgroi_img.img.Width;
                OriginalPB.Image = null;
                OriginalPB.Invalidate();
                OriginalPB.SizeMode = PictureBoxSizeMode.Zoom;
                var s3 = PatternPB.Size;
                var s4 = OriginalPB.Size;
                int Patwidth = s3.Width;
                int Patheight = s3.Height;
                int Oriwidth = s4.Width;
                int Oriheight = s4.Height;
                size = orgroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                s3.Width = size[2];
                s3.Height = size[3];
                s4.Width = size[0];
                s4.Height = size[1];
                PatternPB.Size = s3;
                OriginalPB.Size = s4;

                checkroiflag = true;
                checkroiflag2 = true;
                colorbarGB.Visible = false;
                XORGB.Visible = false;
            }




            if (comboBox1.Text == "Dosage_2level")
            {
                patternclear = false;
                Lablevel.Text = "2";
                if (ROIshowCB.Checked)
                {

                    patroi_img = new imgclass(dosage_2levelROI, "Dosage_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = true;
                    XORGB.Visible = false;
                }
                else
                {

                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(dosagename_2levelpath, "Dosage_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);

                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = orgimg.ratio;
                    //if(patimg.img!=null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = true;
                        XORGB.Visible = false;
                    }



                }






            }
            else if (comboBox1.Text == "LDFile_2level")
            {
                patternclear = false;
                Lablevel.Text = "2";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(ldimg_2levelROI, "LDFile_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(ldimgname_2levelpath, "LDFile_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    //if(patimg.img==null)
                    //{
                    //    comboBox1.Text = "";
                    //}
                    patimg.ratio = orgimg.ratio;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }

                }


            }
            else if (comboBox1.Text == "LDFile_4level")
            {
                patternclear = false;
                Lablevel.Text = "4";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(ldimg_4levelROI, "LDFile_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(ldimgname_4levelpath, "LDFile_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    patimg.ratio = orgimg.ratio;
                    //if (patimg.img != null)
                    //{
                    //    PatternPB.Invalidate();
                    //    colorbarGB.Visible = false;
                    //}
                    //else
                    //{

                    //}
                    PatternPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;

                }


            }
            else if (comboBox1.Text == "Dosage_4level")
            {
                patternclear = false;
                Lablevel.Text = "4";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(dosage_4levelROI, "Dosage_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    roiflag = true;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = true;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(dosagename_4levelpath, "Dosage_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = orgimg.ratio;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = true;
                        XORGB.Visible = false;
                    }
                }





            }
            else if (comboBox1.Text == "Threshold_2level")
            {
                patternclear = false;
                Lablevel.Text = "2";
                if (ROIshowCB.Checked)
                {

                    patroi_img = new imgclass(Threshold_2levelROI, "Threshold_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;

                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(Threshold_2levelpath, "Threshold_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }

                }





            }
            else if (comboBox1.Text == "Threshold_4level")
            {
                patternclear = false;
                Lablevel.Text = "4";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(Threshold_4levelROI, "Threshold_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(Threshold_4levelpath, "Threshold_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }
                }

            }
            else if (comboBox1.Text == "XOR_2level")
            {
                patternclear = false;
                Lablevel.Text = "2";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(xor_roi_2level_path, "XOR_ROI_2level", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = true;
                }
                else
                {
                    patimg = new imgclass(xor_path, "xor", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    PatternPB.Invalidate();
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = true;
                    }

                }


            }
            else if (comboBox1.Text == "XOR_4level")
            {
                patternclear = false;
                Lablevel.Text = "4";
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(xor_roi_4level_path, "XOR_ROI_4level", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = true;
                }
                else
                {
                    patimg = new imgclass(xor_4level_path, "xor_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    PatternPB.Invalidate();
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = true;
                    }
                }
            }
            else if (comboBox1.Text == "")
            {
                Lablevel.Text = "";
                patternclear = true;
                PatternPB.Invalidate();
            }
            //else if (comboBox1.Text == "LDFile_2levelROI")
            //{
            //    patroi_img = new imgclass(ldimg_2levelROI, "LDFile_2level_ROI", ptbh, ptbw, 1, 2);
            //    PatternPB.Height = ptbh;
            //    PatternPB.Width = ptbw;
            //    PatternPB.Image = null;
            //    PatternPB.SizeMode = PictureBoxSizeMode.CenterImage;
            //    roiflag = true;
            //    PatternPB.Invalidate();
            //    OriginalPB.Invalidate();
            //}
            //else if (comboBox1.Text == "LDFile_4levelROI")
            //{
            //    patroi_img = new imgclass(ldimg_4levelROI, "LDFile_4level_ROI", ptbh, ptbw, 1, 2);
            //    PatternPB.Height = ptbh;
            //    PatternPB.Width = ptbw;
            //    PatternPB.Image = null;
            //    PatternPB.SizeMode = PictureBoxSizeMode.CenterImage;
            //    roiflag = true;
            //    PatternPB.Invalidate();
            //    OriginalPB.Invalidate();
            //}
            //else if (comboBox1.Text == "Dosage_2levelROI")
            //{
            //    patroi_img = new imgclass(dosage_2levelROI, "Dosage_2level_ROI", ptbh, ptbw, 1, 2);
            //    PatternPB.Height = ptbh;
            //    PatternPB.Width = ptbw;
            //    PatternPB.Image = null;
            //    PatternPB.SizeMode = PictureBoxSizeMode.CenterImage;
            //    roiflag = true;
            //    PatternPB.Invalidate();
            //    OriginalPB.Invalidate();
            //}
            //else if (comboBox1.Text == "Dosage_4levelROI")
            //{
            //    patroi_img = new imgclass(dosage_4levelROI, "Dosage_4level_ROI", ptbh, ptbw, 1, 2);
            //    PatternPB.Height = ptbh;
            //    PatternPB.Width = ptbw;
            //    PatternPB.Image = null;
            //    PatternPB.SizeMode = PictureBoxSizeMode.CenterImage;
            //    roiflag = true;
            //    PatternPB.Invalidate();
            //    OriginalPB.Invalidate();
            //}

            Txtratio.Focus();
        }

        private void OriginalPB_ParentChanged(object sender, EventArgs e)
        {

        }

        private void PatternPB_ParentChanged(object sender, EventArgs e)
        {

        }

        private void TxtleftopyROI_TextChanged(object sender, EventArgs e)
        {

        }

        private void OptimizationCB_CheckedChanged(object sender, EventArgs e)
        {

        }



        private void PatternPB_Paint(object sender, PaintEventArgs e)
        {
            #region newversion
            if (comboflag == false)
            {
                return;
            }
            if (patternclear == true)
            {

                Rectangle Rectimage = new Rectangle(0, 0, ptbw, ptbh);//指定繪製影像的位置和大小。 縮放影像來符合矩形。
                Pen newpen = new Pen(Color.Silver);                                                                    //創建bmp的Graphics對象

                //將畫布填充成與PictureBox背景顏色相同的顏色
                e.Graphics.DrawRectangle(newpen, Rectimage);
                //將畫布畫到與picCar相同的位置

                return;
            }
            if (!ROIshowCB.Checked)
            {

                if (oldxscroll != 0 && oldyscroll != 0)
                {

                    panel2.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                else if (oldxscroll == 0 && oldyscroll != 0)
                {
                    panel2.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                else if (oldxscroll != 0 && oldyscroll == 0)
                {
                    panel2.AutoScrollPosition = new Point(oldxscroll, oldyscroll);
                    oldxscroll = 0;
                    oldyscroll = 0;
                }
                if (PatternPB.Image != null)
                {
                    PatternPB.Image = null;
                    PatternPB.Refresh();
                }
            }
            int ptbwnew40082 = 0;
            int ptbhnew40082 = 0;
            int pos0 = panel2.HorizontalScroll.Value;
            int pos1 = panel2.VerticalScroll.Value;
            int posratio0 = pos0;
            int posratio1 = pos1;
            if (!ROIshowCB.Checked)
            {

                PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                PatternPB.Width = ptbmax;
                PatternPB.Height = ptbmax;


                if (patmeenterflag == true && rationew > oldratio && !ROIcheckBox.Checked)
                {
                    int newmousex;
                    int newmousey;


                    ptbwnew40082 = (int)(ptbw / rationew / 2);
                    ptbhnew40082 = (int)(ptbh / rationew / 2);
                    newmousex = (mousex - ptbwnew40082);//40082
                    newmousey = (mousey - ptbhnew40082);//40082
                    if (newmousex < 0)
                        newmousex = 0;
                    if (newmousey < 0)
                        newmousey = 0;
                    if (newmousex > tiffimg.width - ptbw)
                        newmousex = (tiffimg.width - ptbw);
                    if (newmousey > tiffimg.height - ptbh)
                        newmousey = (tiffimg.height - ptbh);
                    pos0 = newmousex;//40082
                    pos1 = newmousey;//40082
                    optlefttop.X += pos0;
                    optlefttop.Y += pos1;
                    posratio0 = (int)(Math.Round((((pos0) * ((ptbmax - ptbw) / (double)(magnifyratiowidth - 1 - ptbw)))) * orgimg.ratio));//40000
                    posratio1 = (int)(Math.Round((((pos1) * ((ptbmax - ptbh) / (double)(magnifyratioheight - 1 - ptbh)))) * orgimg.ratio));//40000

                    rectopt.Location = optlefttop;
                    panel2.AutoScrollPosition = new Point(posratio0, posratio1);
                    oldratio = rationew;
                    modifybymouse = true;
                }
                else
                {
                    modifybymouse = false;
                }


                int epsilon = 2000;
                if (modifybymouse == false)
                {
                    if (patimg == null)
                    {

                        return;
                    }
                    //if (patimg.img == null)
                    //{
                    //    return;
                    //}
                    pos0 = (int)(Math.Round(((pos0 * ((double)(magnifyratiowidth - 1 - ptbw) / (ptbmax - ptbw)))) / patimg.ratio));//轉成40000
                    pos1 = (int)(Math.Round(((pos1 * ((double)(magnifyratioheight - 1 - ptbh) / (ptbmax - ptbh)))) / patimg.ratio));//轉成40000
                }

                if (ptbminsizeall == true)
                {
                    epsilon = (int)((orgimg.h5img.rownum) * 0.13);

                }

                currentlefttoppos.X = pos0;
                currentlefttoppos.Y = pos1;
                currentrightbottompos.X = currentlefttoppos.X + (int)(ptbw / patimg.ratio);
                currentrightbottompos.Y = currentlefttoppos.Y + (int)(ptbh / patimg.ratio);
                TxtleftopxPat.Text = currentlefttoppos.X.ToString();
                TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
                TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
                TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();



                if (pos0 != patpos0now || pos1 != patpos1now)
                {
                    patfresh = true;
                }
                else
                {
                    patfresh = false;
                }
                patpos0now = pos0;
                patpos1now = pos1;



                if (orgimg.checkifupdate(pos0, pos1, epsilon))
                {
                    orgimg.updateimg(pos0, pos1);
                    if (comboflag == true && patimg.img != null)
                        patimg.updateimg(pos0, pos1);
                }


                if (comboflag == true && patimg.img != null)
                {
                    ImageAttributes imageAttr = new ImageAttributes();
                    Graphics.DrawImageAbort imageCallback
                      = new Graphics.DrawImageAbort(DrawImageCallback8);
                    IntPtr imageCallbackData = new IntPtr(1);
                    GraphicsUnit units = GraphicsUnit.Pixel;
                    //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
                    //int newptbw = (int)(ptbw * rationew);
                    //int newptbh = (int)(ptbh * rationew);
                    Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);//指定繪製影像的位置和大小。 縮放影像來符合矩形。

                    if (patfresh == true)
                    {
                        PatternPB.Refresh();
                        patfresh = false;
                    }
                    if (patimg.img != null)
                    {
                        //if (comboBox1.Text == "LDFile_2level"|| comboBox1.Text == "LDFile_4level")
                        //{
                        //    e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        //    e.Graphics.DrawImage(patimg.img, Rectimage, pos0 - patimg.realposlefttop.X, pos1 - patimg.realposlefttop.Y, ptbw / patimg.ratio, ptbh / patimg.ratio, units);
                        //}
                        //else
                        {
                            e.Graphics.DrawImage(patimg.img, Rectimage, pos0 - patimg.realposlefttop.X, pos1 - patimg.realposlefttop.Y, ptbw / patimg.ratio, ptbh / patimg.ratio, units);
                        }
                        

                    }
                    else
                    {
                        return;
                    }

                    /*
                     要繪製之來源影像部分左上角的 X 座標:pos0 - patimg.realposlefttop.X
                     要繪製之來源影像部分左上角的 Y 座標:pos1 - patimg.realposlefttop.Y
                     要繪製之來源影像部分的寬度:ptbw / patimg.ratio
                     要繪製之來源影像部分的長度:ptbh / patimg.ratio
                    */

                    panel1.AutoScrollPosition = new Point(posratio0, posratio1);





                    if (mouseevent == true)
                    {
                        if (RectOriginal != null && RectOriginal.Width > 0 && RectOriginal.Height > 0)
                        {
                            e.Graphics.DrawRectangle(new Pen(Color.Red, 2), RectOriginal);

                        }
                    }
                    if (RectoptCB.Checked && rectoptflag == true)
                    {
                        e.Graphics.DrawRectangle(new Pen(Color.Red, 2), rectopt);

                    }

                    #region drawing
                    //ldfile1.readldconfig(ldfilename1);
                    //ldfile1.lightspotrange();
                    //ldfile2.readldconfig(ldfilename2);
                    //ldfile2.lightspotrange();
                    float[] ldxmax;
                    float[] ldxmin;
                    float[] le2ldxmax;
                    float[] le2ldxmin;
                    float le1ldmax;
                    float le1ldmin;
                    float le2ldmax;
                    float le2ldmin;
                    int ldfile1ldmax = 0;
                    int ldfile1ldmin = Int32.MaxValue;
                    int ldfile2ldmax = 0;
                    int ldfile2ldmin = Int32.MaxValue;
                    int dxmin;
                    int dxmax;
                    int le1ldmin65535;
                    int le1ldmax65535;
                    int le2ldmin65535;
                    int le2ldmax65535;
                    bool le1flag = false;
                    bool le2flag = false;
                    Rectangle withoutexposurearea = new Rectangle();
                    Point withoutexposurearealefttop = new Point();
                    //int cnt=0;
                    
                    ldxmax = ldfile1.ldxmax;
                    ldxmin = ldfile1.ldxmin;
                    le2ldxmax = ldfile2.ldxmax;
                    le2ldxmin = ldfile2.ldxmin;

                    le1ldmax = ldfile1.xmax;
                    le1ldmin = ldfile1.xmin;
                    le2ldmax = ldfile2.xmax;
                    le2ldmin = ldfile2.xmin;
                    int ptbw40082, ldxscale, ptbhscale;
                    Scale_del scale_del;
                    //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))) / patimg.ratio);
                    ptbw40082 = (int)Math.Round(ptbw / patimg.ratio);
                    scaleclass scalex = new scaleclass(ptbw, ptbmax, patimg.ratio, magnifyratiowidth, pos0, ptbw40082, posratio0);


                    scale_del = scalex.scaleptbmax;//新的找座標方法
                    //if (comboBox1.Text == "LDFile")
                    {
                        //ldfile2.lightspotrange();
                        //ldfile1.lightspotrange();
                        Pen drawRedPen = new Pen(Color.Red, 5);
                        Pen drawBluePen = new Pen(Color.Blue, 5);
                        Pen drawGreenPen = new Pen(Color.Green, 5);
                        Pen drawPurplePen = new Pen(Color.Purple, 5);
                        Pen drawOrangePen = new Pen(Color.DarkOrange, 5);
                        Pen drawYellowPen = new Pen(Color.Gold, 5);
                        Pen drawWhitePen = new Pen(Color.AntiqueWhite, 5);
                        ldcnt.Clear();
                        //int diffh = h - ldh5image.rownum - +1;
                        //int diffw = w - ldh5image.colnum - 1;
                        int xepsilon = 100;
                        //int[] ldxmax;
                        //int[] ldxmin;
                        int ldx65535 = 0;
                        int ldy65535 = 0;
                        int ptbw65535 = 0;

                        if (pos0 + ptbw40082 >= le1ldmin && pos0 <= le1ldmax)
                        {
                            le1flag = true;
                        }
                        if (pos0 + ptbw40082 >= le2ldmin && pos0 <= le2ldmax)
                        {
                            le2flag = true;
                        }

                        if (le1flag == true && le2flag == false)
                        {
                            LElabel.Text = "LE1";
                        }
                        else if (le2flag == true && le1flag == false)
                        {
                            LElabel.Text = "LE2";
                        }
                        else if (le1flag == true && le2flag == true)
                        {
                            LElabel.Text = "LE1+LE2";
                        }
                        else if (le1flag == false && le2flag == false)
                        {
                            LElabel.Text = "";
                        }



                        //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
                        //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
                        //int cnt=0;
                        //ldxmax = ldfile1.ldxmax;
                        //ldxmin = ldfile1.ldxmin;

                        for (int i = 0; i < 20; ++i)
                        {

                            if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw40082)//判斷式都是用原始座標系去判斷
                            {

                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
                                if (cnt % 2 == 0)
                                {
                                    /*作圖都要在max座標系上操作*/

                                    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(ldxmin[i]);//利用委派叫出max的function

                                    e.Graphics.DrawLine(drawOrangePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);

                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(ldxmin[i]);
                                    e.Graphics.DrawLine(drawPurplePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
                                }
                            }
                            if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw40082)
                            {

                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
                                if (cnt % 2 == 0)
                                {
                                    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(ldxmax[i]);
                                    e.Graphics.DrawLine(drawOrangePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(ldxmax[i]);
                                    e.Graphics.DrawLine(drawPurplePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
                                }
                            }

                            if (ldxmax[i] >= pos0 && pos0 + ptbw40082 >= ldxmin[i])
                            {
                                if (ldxmin[i] < pos0)
                                {
                                    dxmin = posratio0;
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

                                    dxmin = scale_del(ldxmin[i]);
                                }
                                if (ldxmax[i] > pos0 + ptbw40082)
                                {
                                    dxmax = posratio0 + ptbw;
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    dxmax = scale_del(ldxmax[i]);
                                }
                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

                                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, posratio1);
                            }
                            if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw40082)
                            {

                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
                                if (cnt % 2 == 0)
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(le2ldxmin[i]);
                                    e.Graphics.DrawLine(drawWhitePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(le2ldxmin[i]);
                                    e.Graphics.DrawLine(drawYellowPen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
                                }
                            }
                            if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw40082)
                            {
                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
                                if (cnt % 2 == 0)
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(le2ldxmax[i]);
                                    e.Graphics.DrawLine(drawWhitePen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    ldxscale = scale_del(le2ldxmax[i]);
                                    e.Graphics.DrawLine(drawYellowPen, ldxscale, posratio1, ldxscale, posratio1 + ptbh);
                                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
                                }
                            }
                            if (le2ldxmax[i] >= pos0 && pos0 + ptbw40082 >= le2ldxmin[i])
                            {
                                if (le2ldxmin[i] < pos0)
                                {
                                    dxmin = posratio0;
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    dxmin = scale_del(le2ldxmin[i]);
                                }
                                if (le2ldxmax[i] > pos0 + ptbw40082)
                                {
                                    dxmax = posratio0 + ptbw;
                                }
                                else
                                {
                                    //ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                                    dxmax = scale_del(le2ldxmax[i]);
                                }
                                cnt = i + 1;
                                Font drawFont = new Font("Arial", 10);
                                SolidBrush drawbrush = new SolidBrush(Color.Yellow);

                                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, posratio1 + 20);
                            }
                        }

                        int dpos, dpos0;
                        int ptbh65535;
                        ptbh65535 = (int)Math.Round(((ptbh * patimg.ratio) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                        if (le1ldmin > pos0)
                        {
                            //int le1ldminfor65535;
                            //le1ldminfor65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系


                            if (le1ldmin > pos0 + ptbw40082)
                                dpos = posratio0 + ptbw;
                            else
                                dpos = scale_del(le1ldmin);

                            withoutexposurearealefttop.X = posratio0;
                            withoutexposurearealefttop.Y = posratio1;
                            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                            //SolidBrush mybrush = new SolidBrush(Color.Orange);
                            withoutexposurearea.Location = withoutexposurearealefttop;
                            withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbh);
                            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        }
                        else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw40082 > le1ldmax && le1ldmax < pos0 && le2ldmin < pos0 + ptbw40082)
                        {
                            int le1ldminfor65535;
                            int le2ldminfor65535;
                            int le1ldmaxfor65535;
                            int le2ldmaxfor65535;
                            //le1ldmaxfor65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                            //le2ldminfor65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

                            dpos0 = posratio0;


                            dpos = scale_del(le2ldmin);

                            withoutexposurearealefttop.X = posratio0;
                            withoutexposurearealefttop.Y = dpos;
                            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                            withoutexposurearea.Location = withoutexposurearealefttop;
                            withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
                            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        }
                        else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw40082 > le1ldmax && le1ldmax > pos0 && le2ldmin > pos0 + ptbw40082)
                        {
                            int le1ldminfor65535;
                            int le2ldminfor65535;
                            int le1ldmaxfor65535;
                            int le2ldmaxfor65535;
                            //le1ldmaxfor65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                            //le2ldminfor65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

                            dpos0 = scale_del(le1ldmax);



                            dpos = posratio0 + ptbw;


                            withoutexposurearealefttop.X = dpos0;
                            withoutexposurearealefttop.Y = posratio1;
                            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                            withoutexposurearea.Location = withoutexposurearealefttop;
                            withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
                            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        }
                        else if(le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw40082 > le1ldmax && le1ldmax <pos0 && le2ldmin > pos0 + ptbw40082)
                        {
                            int le1ldminfor65535;
                            int le2ldminfor65535;
                            int le1ldmaxfor65535;
                            int le2ldmaxfor65535;
                            //le1ldmaxfor65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                            //le2ldminfor65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

                            dpos0 = posratio0;



                            dpos = posratio0 + ptbw;


                            withoutexposurearealefttop.X = dpos0;
                            withoutexposurearealefttop.Y = posratio1;
                            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                            withoutexposurearea.Location = withoutexposurearealefttop;
                            withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
                            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        }
                        else if (le2ldmax < pos0 + ptbw40082)
                        {
                            //int le1ldminfor65535;
                            //int le2ldminfor65535;
                            //int le1ldmaxfor65535;
                            //int le2ldmaxfor65535;
                            //le2ldmaxfor65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
                            if (le2ldmax < pos0)
                                dpos = posratio0;
                            else
                                dpos = scale_del(le2ldmax);

                            withoutexposurearealefttop.X = dpos;
                            withoutexposurearealefttop.Y = posratio1;
                            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                            withoutexposurearea.Location = withoutexposurearealefttop;
                            withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw - dpos), ptbh);
                            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        }
                        //else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw40082 > le1ldmax && le1ldmax < pos0 && le2ldmin > pos0 + ptbw40082)
                        //{



                        //    withoutexposurearealefttop.X = posratio0;
                        //    withoutexposurearealefttop.Y = posratio1;
                        //    SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
                        //    withoutexposurearea.Location = withoutexposurearealefttop;
                        //    withoutexposurearea.Size = new Size(Math.Abs(posratio1 - posratio0), ptbh);
                        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
                        //}



                    }

                    #endregion drawing
                }

            }
            else
            {

                if (checkroiflag2 == true)
                {
                    if (patroi_img.img == null)
                        return;

                    PatternPB.Image = patroi_img.img;
                    //Rectangle Rectimage = new Rectangle(0, 0, patroi_img.img.Width, patroi_img.img.Height);
                    //GraphicsUnit units = GraphicsUnit.Pixel;
                    //e.Graphics.DrawImage(patroi_img.img, Rectimage, 0, 0, patroi_img.img.Width, patroi_img.img.Height, units);


                    checkroiflag2 = false;

                }
                if (roiscroll == true)
                {
                    pos0 = panel2.HorizontalScroll.Value;
                    pos1 = panel2.VerticalScroll.Value;
                    panel1.AutoScrollPosition = new Point(pos0, pos1);
                }

                //PatternPB.Image = patroi_img.img;
                //OriginalPB.Image = orgroi_img.img;

            }

            #endregion newversion


            #region oldversion
            //if (buttonorgflag == false)
            //{
            //    return;
            //}
            ////if (buttonldflag == false)      //|| buttondosageflag == false))
            ////{
            ////    return;
            ////}

            //if (Txtratio.Text == "100")
            //{
            //   // PatternPB.Refresh();
            //    int pos0 = panel2.HorizontalScroll.Value;
            //    int pos1 = panel2.VerticalScroll.Value;
            //    currentlefttoppos.X = pos0;
            //    currentlefttoppos.Y = pos1;
            //    currentrightbottompos.X = pos0 + ptbw;
            //    currentrightbottompos.Y = pos1 + ptbh;
            //    TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //    TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //    TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //    TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //    int currentarrayx2 = pos0 + ptbw + buffercol;
            //    int currentarrayy2 = pos1 + ptbh + bufferrow;
            //    if (pos0 != pos0now || pos1 != pos1now)
            //    {
            //        patfresh = true;
            //    }
            //    else
            //    {
            //        patfresh = false;
            //    }
            //    pos0now = pos0;
            //    pos1now = pos1;

            //    Tuple<int[], byte[,]> currentarraytuple;
            //    Tuple<int[], byte[,]> currentarraytupledosage;
            //    Tuple<int[], byte[,]> currentarraytupleld;

            //    OriginalPB.Height = orgh5image.rownum - 1;
            //    OriginalPB.Width = orgh5image.colnum - 1;
            //    if (comboBox1.Text == "Dosage")
            //    {
            //        PatternPB.Height = dosageh5image.rownum - 1;
            //        PatternPB.Width = dosageh5image.colnum - 1;
            //    }
            //    else if (comboBox1.Text == "LDFile")
            //    {

            //        PatternPB.Height = ldh5image.rownum - 1;
            //        PatternPB.Width = ldh5image.colnum - 1;
            //    }
            //    int epsilon = 2000;
            //    int maxsize = 40081;
            //    int maxwidthsize = 0;
            //    int maxheightsize = 0;
            //    if (comboBox1.Text == "Dosage")
            //    {

            //        maxwidthsize = dosageh5image.colnum - 1;
            //        maxheightsize = dosageh5image.rownum - 1;
            //    }
            //    else if (comboBox1.Text == "LDFile")
            //    {

            //        maxwidthsize = ldh5image.colnum - 1;
            //        maxheightsize = ldh5image.rownum - 1;
            //    }
            //    if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //    {

            //    }
            //    else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //    {

            //    }
            //    else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //    {

            //    }
            //    else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //    {

            //    }
            //    else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //    {

            //    }
            //    else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //    {

            //    }
            //    else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //    {

            //        if (comboBox1.Text == "Dosage")
            //        {
            //            currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //            currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //            orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //            dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //            realposlefttop.X = currentarraytuple.Item1[0];
            //            realposlefttop.Y = currentarraytuple.Item1[1];
            //            realposrightbottom.X = currentarraytuple.Item1[2];
            //            realposrightbottom.Y = currentarraytuple.Item1[3];
            //            // OriginalPB.Invalidate();
            //            //PatternPB.Invalidate();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //            currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //            orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //            ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //            realposlefttop.X = currentarraytuple.Item1[0];
            //            realposlefttop.Y = currentarraytuple.Item1[1];
            //            realposrightbottom.X = currentarraytuple.Item1[2];
            //            realposrightbottom.Y = currentarraytuple.Item1[3];
            //            //OriginalPB.Invalidate();
            //            //PatternPB.Invalidate();
            //        }
            //        else
            //        {
            //            currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //            orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //            realposlefttop.X = currentarraytuple.Item1[0];
            //            realposlefttop.Y = currentarraytuple.Item1[1];
            //            realposrightbottom.X = currentarraytuple.Item1[2];
            //            realposrightbottom.Y = currentarraytuple.Item1[3];
            //            //OriginalPB.Invalidate();
            //            // PatternPB.Invalidate();
            //        }



            //    }
            //    //if (dosageflag == true)
            //    //{
            //    //    //currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //    //    float[,] currentarraydosage;
            //    //    dosageh5image.openfile(dosageimgpath);
            //    //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //    //    int m = currentarraydosage.GetLength(0);
            //    //    int n = currentarraydosage.GetLength(1);
            //    //    byte[,] currentarray = new byte[m, n];
            //    //    float min1 = float.MaxValue, max1 = 0;
            //    //    for (int i = 0; i < m; ++i)
            //    //        for (int j = 0; j < n; ++j)
            //    //        {
            //    //            if (max1 < currentarraydosage[i, j])
            //    //            { max1 = currentarraydosage[i, j]; }
            //    //            if (min1 > currentarraydosage[i, j])
            //    //            { min1 = currentarraydosage[i, j]; }
            //    //        }
            //    //    for (int i = 0; i < m; ++i)
            //    //    {
            //    //        for (int j = 0; j < n; ++j)
            //    //        {
            //    //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //    //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //    //        }
            //    //    }
            //    //    dosageimage = BufferToImage(currentarray);

            //    //    //currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //    //    //dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //    //    //dosageflag = false;


            //    //}

            //    //if (ldflag == true)
            //    //{
            //    //    byte[,] currentarrayld;
            //    //    ldh5image.openfile(ldimgpath);
            //    //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //    //    ldimage = BufferTobinaryImage(currentarrayld);

            //    //    //ldflag = false;

            //    //}

            //    Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //    ImageAttributes imageAttr = new ImageAttributes();
            //    Graphics.DrawImageAbort imageCallback
            //      = new Graphics.DrawImageAbort(DrawImageCallback8);
            //    IntPtr imageCallbackData = new IntPtr(1);
            //    GraphicsUnit units = GraphicsUnit.Pixel;
            //    //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //    Rectangle Rectimage = new Rectangle(pos0, pos1, ptbw, ptbh);
            //    if (comboBox1.Text == "Dosage")
            //    {

            //        if (patfresh == true)
            //        {
            //            PatternPB.Refresh();
            //            patfresh = false;
            //        }
            //        e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);


            //        panel1.AutoScrollPosition = new Point(pos0, pos1);


            //        //dosageh5image.closefile();
            //    }
            //    else if (comboBox1.Text == "LDFile")
            //    {

            //        if (patfresh == true)
            //        {
            //            PatternPB.Refresh();
            //            patfresh = false;
            //        }
            //        e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //        panel1.AutoScrollPosition = new Point(pos0, pos1);

            //        //ldh5image.closefile();
            //    }

            //    if (comboBox1.Text == "LDFile")
            //    {


            //        //ldfile1.lightspotrange();
            //        //ldfile2.lightspotrange();
            //        Pen drawRedPen = new Pen(Color.Red, 5);
            //        Pen drawBluePen = new Pen(Color.Blue, 5);
            //        Pen drawGreenPen = new Pen(Color.Green, 5);
            //        Pen drawYellowPen = new Pen(Color.Yellow,5);
            //        Pen drawPurplePen = new Pen(Color.Purple, 5);
            //        Pen drawOrangePen = new Pen(Color.Orange, 5);
            //        Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //        Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed,5);

            //        //ldcnt.Clear();
            //        int xepsilon = 100;

            //        //if (le2ldmin > le1ldmin && le2ldmin > le1ldmax)
            //        //{
            //        //    if (pos0 <= le1ldmax && pos0 >= le1ldmin)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else if (pos0 <= le2ldmax && pos0 >= le2ldmin)
            //        //    {


            //        //        label30.Text = "LE2";

            //        //    }
            //        //    else if ((pos0 + ptbw) > le1ldmax && (pos0 + ptbw) < le2ldmin)
            //        //    {
            //        //        label30.Text = "";
            //        //    }

            //        //}
            //        //else if (le1ldmax > le2ldmin && le2ldmin > le1ldmin)
            //        //{

            //        //    if (pos0 >= le1ldmin && pos0 < le2ldmin)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else if (pos0 > le1ldmax && pos0 <= le2ldmax)
            //        //    {
            //        //        label30.Text = "LE2";
            //        //    }
            //        //    else if (pos0 >= le2ldmin && pos0 <= le1ldmax)
            //        //    {
            //        //        label30.Text = "LE1+LE2";
            //        //    }
            //        //    else
            //        //    {
            //        //        label30.Text = "";
            //        //    }

            //        //}
            //        //else if (le2ldmax > le1ldmin && le1ldmax > le2ldmax)
            //        //{
            //        //    if (pos0 >= le1ldmin && pos0 <= le2ldmax)
            //        //    {
            //        //        label30.Text = "LE1+LE2";
            //        //    }
            //        //    else if (pos0 > le2ldmin && pos0 < le1ldmin)
            //        //    {
            //        //        label30.Text = "LE2";
            //        //    }
            //        //    else if (pos0 >= le2ldmax && pos0 <= le1ldmax)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else
            //        //    {
            //        //        label30.Text = "";
            //        //    }
            //        //}
            //        if(pos0+ptbw>=le1ldmin&&pos0<=le1ldmax)
            //        {
            //            le1flag = true;
            //        }

            //        if(pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //        {
            //            le2flag = true;
            //        }

            //        if(le1flag==true&&le2flag==false)
            //        {
            //            label30.Text = "LE1";
            //        }
            //        else if(le2flag == true && le1flag == false)
            //        {
            //            label30.Text = "LE2";
            //        }
            //        else if(le1flag == true && le2flag == true)
            //        {
            //            label30.Text = "LE1+LE2";
            //        }
            //        else if(le1flag ==false&& le2flag == false)
            //        {
            //            label30.Text = "";
            //        }


            //        for (int i = 0; i < 20; ++i)
            //        {
            //            if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //            {

            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {
            //                    e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);

            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);
            //                }
            //            }

            //            if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //            {
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {

            //                    e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }



            //            }

            //            if(ldxmax[i]>=pos0&&pos0+ptbw>=ldxmin[i])
            //            {
            //                if(ldxmin[i]<pos0)
            //                {
            //                    dxmin = pos0;
            //                }
            //                else
            //                {
            //                    dxmin = ldxmin[i];
            //                }
            //                if(ldxmax[i]>pos0+ptbw)
            //                {
            //                    dxmax = pos0 + ptbw;
            //                }
            //                else
            //                {
            //                    dxmax = ldxmax[i];
            //                }
            //                cnt = i+1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush,(  dxmax-dxmin) / 2+dxmin, pos1);
            //            }
            //            if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw)
            //            {

            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {
            //                    e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmin[i], 0, le2ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);
            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawCyanPen, le2ldxmin[i], 0, le2ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);
            //                }
            //            }
            //            if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw)
            //            {
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {
            //                    e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmax[i], 0, le2ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawCyanPen, le2ldxmax[i], 0, le2ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }



            //            }
            //            if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //            {
            //                if (le2ldxmin[i] < pos0)
            //                {
            //                    dxmin = pos0;
            //                }
            //                else
            //                {
            //                    dxmin = le2ldxmin[i];
            //                }
            //                if (le2ldxmax[i] > pos0 + ptbw)
            //                {
            //                    dxmax = pos0 + ptbw;
            //                }
            //                else
            //                {
            //                    dxmax = le2ldxmax[i];
            //                }
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1+20);
            //            }
            //        }
            //        int dpos,dpos0;

            //        if(le1ldmin> pos0)
            //        {
            //            if (le1ldmin > pos0 + ptbw)
            //                dpos = pos0 + ptbw;
            //            else
            //                dpos = le1ldmin;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(dpos - pos0), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }
            //        else if(le2ldmin> le1ldmax&&pos0< le2ldmin&& pos0+ptbw> le1ldmax)
            //        {
            //            if (le1ldmax > pos0)
            //                dpos0 = le1ldmax;
            //            else
            //                dpos0 = pos0;

            //            if (le2ldmin < pos0+ptbw)
            //                dpos = le2ldmin;
            //            else
            //                dpos = pos0 + ptbw;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }
            //        else if (le2ldmax < pos0 + ptbw)
            //        {
            //            if (le2ldmax < pos0)
            //                dpos = pos0;
            //            else
            //                dpos = le2ldmax;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(pos0+ptbw-dpos), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }


            //        //if (le1ldmin > pos0)
            //        //{
            //        //    withoutexposurearealefttop.X = pos0;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin - pos0), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //}
            //        //if (pos0 + ptbw > le1ldmax)
            //        //{
            //        //    withoutexposurearealefttop.X = le1ldmax;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(pos0 + ptbw - le1ldmax), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //}
            //        //if(label30.Text=="LE2")
            //        //{
            //        //   if (le2ldmin > pos0 )
            //        //   {
            //        //    withoutexposurearealefttop.X = pos0;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(le2ldmin - pos0), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //   }
            //        //   if (pos0 + ptbw > le2ldmax)
            //        //   {
            //        //    withoutexposurearealefttop.X = le2ldmax;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(pos0 + ptbw - le2ldmax), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //   }

            //        //}









            //    }

            //    if (comboBox1.Text == "Dosage")
            //    {

            //        //ldfile1.lightspotrange();
            //        //ldfile2.lightspotrange();
            //        Pen drawRedPen = new Pen(Color.Red, 5);
            //        Pen drawBluePen = new Pen(Color.Blue, 5);
            //        Pen drawGreenPen = new Pen(Color.Green, 5);
            //        Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //        Pen drawPurplePen = new Pen(Color.Purple, 5);
            //        Pen drawOrangePen = new Pen(Color.Orange, 5);
            //        //ldcnt.Clear();
            //        int xepsilon = 100;

            //        //if (le2ldmin > le1ldmin && le2ldmin > le1ldmax)
            //        //{
            //        //    if (pos0 <= le1ldmax && pos0 >= le1ldmin)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else if (pos0 <= le2ldmax && pos0 >= le2ldmin)
            //        //    {


            //        //        label30.Text = "LE2";

            //        //    }
            //        //    else if ((pos0 + ptbw) > le1ldmax && (pos0 + ptbw) < le2ldmin)
            //        //    {
            //        //        label30.Text = "";
            //        //    }

            //        //}
            //        //else if (le1ldmax > le2ldmin && le2ldmin > le1ldmin)
            //        //{

            //        //    if (pos0 >= le1ldmin && pos0 < le2ldmin)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else if (pos0 > le1ldmax && pos0 <= le2ldmax)
            //        //    {
            //        //        label30.Text = "LE2";
            //        //    }
            //        //    else if (pos0 >= le2ldmin && pos0 <= le1ldmax)
            //        //    {
            //        //        label30.Text = "LE1+LE2";
            //        //    }
            //        //    else
            //        //    {
            //        //        label30.Text = "";
            //        //    }

            //        //}
            //        //else if (le2ldmax > le1ldmin && le1ldmax > le2ldmax)
            //        //{
            //        //    if (pos0 >= le1ldmin && pos0 <= le2ldmax)
            //        //    {
            //        //        label30.Text = "LE1+LE2";
            //        //    }
            //        //    else if (pos0 > le2ldmin && pos0 < le1ldmin)
            //        //    {
            //        //        label30.Text = "LE2";
            //        //    }
            //        //    else if (pos0 >= le2ldmax && pos0 <= le1ldmax)
            //        //    {
            //        //        label30.Text = "LE1";
            //        //    }
            //        //    else
            //        //    {
            //        //        label30.Text = "";
            //        //    }
            //        //}
            //        if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //        {

            //            le1flag = true;
            //        }



            //        if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //        {
            //            le2flag = true;
            //        }

            //        if (le1flag == true && le2flag == false)
            //        {
            //            label30.Text = "LE1";
            //        }
            //        else if (le2flag == true && le1flag == false)
            //        {
            //            label30.Text = "LE2";
            //        }
            //        else if (le1flag == true && le2flag == true)
            //        {
            //            label30.Text = "LE1+LE2";
            //        }
            //        else if (le1flag == false && le2flag == false)
            //        {
            //            label30.Text = "";
            //        }


            //        for (int i = 0; i < 20; ++i)
            //        {
            //            if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //            {

            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {
            //                    e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);

            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);
            //                }
            //            }

            //            if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //            {
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {

            //                    e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }



            //            }

            //            if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //            {
            //                if (ldxmin[i] < pos0)
            //                {
            //                    dxmin = pos0;
            //                }
            //                else
            //                {
            //                    dxmin = ldxmin[i];
            //                }
            //                if (ldxmax[i] > pos0 + ptbw)
            //                {
            //                    dxmax = pos0 + ptbw;
            //                }
            //                else
            //                {
            //                    dxmax = ldxmax[i];
            //                }
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //            }
            //            if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw)
            //            {

            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {
            //                    e.Graphics.DrawLine(drawGreenPen, le2ldxmin[i], 0, le2ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);

            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawYellowPen, le2ldxmin[i], 0, le2ldxmin[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], pos1);
            //                }








            //            }
            //            if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw)
            //            {
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                if (cnt % 2 == 0)
            //                {

            //                    e.Graphics.DrawLine(drawGreenPen, le2ldxmax[i], 0, le2ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }
            //                else
            //                {
            //                    e.Graphics.DrawLine(drawYellowPen, le2ldxmax[i], 0, le2ldxmax[i], ldh5image.rownum - 1);
            //                    //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], pos1 + 20);
            //                }



            //            }
            //            if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //            {
            //                if (le2ldxmin[i] < pos0)
            //                {
            //                    dxmin = pos0;
            //                }
            //                else
            //                {
            //                    dxmin = le2ldxmin[i];
            //                }
            //                if (le2ldxmax[i] > pos0 + ptbw)
            //                {
            //                    dxmax = pos0 + ptbw;
            //                }
            //                else
            //                {
            //                    dxmax = le2ldxmax[i];
            //                }
            //                cnt = i + 1;
            //                Font drawFont = new Font("Arial", 10);
            //                SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1+20);
            //            }
            //        }
            //        int dpos, dpos0;

            //        if (le1ldmin > pos0)
            //        {
            //            if (le1ldmin > pos0 + ptbw)
            //                dpos = pos0 + ptbw;
            //            else
            //                dpos = le1ldmin;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(dpos - pos0), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }
            //        else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //        {
            //            if (le1ldmax > pos0)
            //                dpos0 = le1ldmax;
            //            else
            //                dpos0 = pos0;

            //            if (le2ldmin < pos0 + ptbw)
            //                dpos = le2ldmin;
            //            else
            //                dpos = pos0 + ptbw;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }
            //        else if (le2ldmax < pos0 + ptbw)
            //        {
            //            if (le2ldmax < pos0)
            //                dpos = pos0;
            //            else
            //                dpos = le2ldmax;

            //            withoutexposurearealefttop.X = pos0;
            //            withoutexposurearealefttop.Y = pos1;
            //            SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //            withoutexposurearea.Location = withoutexposurearealefttop;
            //            withoutexposurearea.Size = new Size(Math.Abs(pos0 + ptbw - dpos), ptbh);
            //            e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        }
            //        //if (le1ldmin > pos0)
            //        //{
            //        //    withoutexposurearealefttop.X = pos0;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin - pos0), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //}
            //        //if (pos0 + ptbw > le1ldmax)
            //        //{
            //        //    withoutexposurearealefttop.X = le1ldmax;
            //        //    withoutexposurearealefttop.Y = pos1;
            //        //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //    withoutexposurearea.Location = withoutexposurearealefttop;
            //        //    withoutexposurearea.Size = new Size(Math.Abs(pos0 + ptbw - le1ldmax), ldh5image.rownum - 1);
            //        //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //}
            //        //if (label30.Text == "LE2")
            //        //{
            //        //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //        //    {
            //        //        withoutexposurearealefttop.X = pos0;
            //        //        withoutexposurearealefttop.Y = pos1;
            //        //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //        withoutexposurearea.Location = withoutexposurearealefttop;
            //        //        withoutexposurearea.Size = new Size(Math.Abs(le2ldmin - pos0), ldh5image.rownum - 1);
            //        //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //    }
            //        //    if (pos0 + ptbw > le2ldmax)
            //        //    {
            //        //        withoutexposurearealefttop.X = le2ldmax;
            //        //        withoutexposurearealefttop.Y = pos1;
            //        //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //        //        withoutexposurearea.Location = withoutexposurearealefttop;
            //        //        withoutexposurearea.Size = new Size(Math.Abs(pos0 + ptbw - le2ldmax), ldh5image.rownum - 1);
            //        //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //        //    }

            //        //}







            //    }
            //    panel1.AutoScrollPosition = new Point(pos0, pos1);
            //}
            //else //抓放大縮小後的圖形
            //{
            //    if (ptbmaxsizeall == true)//width and length exceed 65535
            //    {
            //        int h = PatternPB.Height;
            //        int w = PatternPB.Width;
            //        int pos0 = panel2.HorizontalScroll.Value;
            //        int pos1 = panel2.VerticalScroll.Value;
            //        int posratio0 = panel2.HorizontalScroll.Value;
            //        int posratio1 = panel2.VerticalScroll.Value;

            //        pos0 = (int)(Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw)))) / rationew);//由65535座標系轉成原始圖檔座標系
            //        pos1 = (int)(Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh)))) / rationew);//由65535座標系轉成原始圖檔座標系
            //        if (pos0 != pos0now || pos1 != pos1now)
            //        {
            //            patfresh = true;
            //        }
            //        else
            //        {
            //            patfresh = false;
            //        }
            //        pos0now = pos0;
            //        pos1now = pos1;

            //        int orgratiopatternx = (int)((pos0 - ptbw * rationew) / rationew);
            //        int orgratiopatterny = (int)((pos1 - ptbh * rationew) / rationew);
            //        //pos0 = (int)(pos0 / rationew);//實際圖檔座標系
            //        //pos1 = (int)(pos1 / rationew);//實際圖檔座標系
            //        int ptbw40082 = 0;
            //        int ptbh40082 = 0;
            //        ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw)))/ rationew);
            //        //ptbh40082 = (int)(ptbh40082 / rationew);
            //        ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))) / rationew);
            //        //ptbw40082 = (int)(ptbw40082 / rationew);
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 +(int)(ptbw / rationew);
            //        currentrightbottompos.Y = pos1 +(int)(ptbh / rationew);
            //        TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //        int ptbhratio = (int)(ptbh * rationew);
            //        int ptbwratio = (int)(ptbw * rationew);



            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;


            //        int epsilon = 2000;
            //        int maxsize = 40081;
            //        int maxwidthsize = 0;
            //        int maxheightsize = 0;
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            maxwidthsize = dosageh5image.colnum - 1;
            //            maxheightsize = dosageh5image.rownum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            maxwidthsize = ldh5image.colnum - 1;
            //            maxheightsize = ldh5image.rownum - 1;
            //        }
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                // PatternPB.Invalidate();
            //            }



            //        }
            //        //if (dosageflag == true)
            //        //{
            //        //    //currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //        //    float[,] currentarraydosage;
            //        //    dosageh5image.openfile(dosageimgpath);
            //        //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //        //    int m = currentarraydosage.GetLength(0);
            //        //    int n = currentarraydosage.GetLength(1);
            //        //    byte[,] currentarray = new byte[m, n];
            //        //    float min1 = float.MaxValue, max1 = 0;
            //        //    for (int i = 0; i < m; ++i)
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            if (max1 < currentarraydosage[i, j])
            //        //            { max1 = currentarraydosage[i, j]; }
            //        //            if (min1 > currentarraydosage[i, j])
            //        //            { min1 = currentarraydosage[i, j]; }
            //        //        }
            //        //    for (int i = 0; i < m; ++i)
            //        //    {
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //        //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //        //        }
            //        //    }
            //        //    dosageimage = BufferToImage(currentarray);

            //        //    //currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //        //    //dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //        //    //dosageflag = false;


            //        //}

            //        //if (ldflag == true)
            //        //{
            //        //    byte[,] currentarrayld;
            //        //    ldh5image.openfile(ldimgpath);
            //        //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //        //    ldimage = BufferTobinaryImage(currentarrayld);

            //        //    //ldflag = false;

            //        //}

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);


            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);


            //            //dosageh5image.closefile();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //ldh5image.closefile();
            //        }

            //        if (comboBox1.Text == "LDFile")
            //        {
            //            //ldfile2.lightspotrange();
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ldy65535 = 0;
            //            int ptbw65535 = 0;

            //            if (pos0 + ptbw40082 >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw40082 >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }



            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            for (int i = 0; i < 20; ++i)
            //            {

            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw40082)//判斷式都是用原始座標系去判斷
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                       /*作圖都要在65535座標系上操作*/



            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }





            //                }
            //                //if (pos0 >= ldxmin[i] && ldxmax[i] >= pos0 || pos0 + ptbw40082 >= ldxmin[i] && ldxmax[i] >= pos0 + ptbw40082)
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                //    ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 +(ptbw65535 / 2), posratio1);
            //                //}
            //                ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw40082 <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw40082 >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw65535 / 2)-225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw40082 > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, ( ldx65535- posratio0) / 2 + posratio0, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw40082 > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0+ ptbw65535 - ldx65535 ) / 2 + posratio0 , posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw40082 > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (  ldx65535- posratio0) / 2 + posratio0, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw40082 > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ptbw65535- ldx65535) / 2 + posratio0 , posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin-175, posratio1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //                        e.Graphics.DrawLine(drawGreenPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawYellowPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawGreenPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawYellowPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }





            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw40082 >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw40082)
            //                    {
            //                        dxmax = posratio0 + ptbw;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1+20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbh65535;
            //            ptbh65535 = (int)Math.Round(((ptbh * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            if (le1ldmin > pos0)
            //            {
            //                int le1ldminfor65535;
            //                le1ldminfor65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbw;
            //                else
            //                    dpos = le1ldminfor65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbh65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                int le1ldminfor65535;
            //                int le2ldminfor65535;
            //                int le1ldmaxfor65535;
            //                int le2ldmaxfor65535;
            //                le1ldmaxfor65535= (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                le2ldminfor65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxfor65535;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminfor65535;
            //                else
            //                    dpos = posratio0 + ptbw65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                int le1ldminfor65535;
            //                int le2ldminfor65535;
            //                int le1ldmaxfor65535;
            //                int le2ldmaxfor65535;
            //                le2ldmaxfor65535= (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxfor65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - dpos), ptbh);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    le1ldmin65535= (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535= (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le1ldmax65535), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(le2ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le2ldmax65535), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}

            //        }

            //        if (comboBox1.Text == "Dosage")
            //        {
            //            //ldfile2.lightspotrange();
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ldy65535 = 0;
            //            int ptbw65535 = 0;

            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }



            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            for (int i = 0; i < 20; ++i)
            //            {

            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }





            //                }
            //                //if (pos0 >= ldxmin[i] && ldxmax[i] >= pos0 || pos0 + ptbw40082 >= ldxmin[i] && ldxmax[i] >= pos0 + ptbw40082)
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                //    ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 +(ptbw65535 / 2), posratio1);
            //                //}
            //                ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw40082 <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw40082 >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw65535 / 2)-225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw40082 > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, ( ldx65535- posratio0) / 2 + posratio0, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw40082 > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0+ ptbw65535 - ldx65535 ) / 2 + posratio0 , posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw40082 > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (  ldx65535- posratio0) / 2 + posratio0, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw40082 > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ptbw65535- ldx65535) / 2 + posratio0 , posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //                        e.Graphics.DrawLine(drawGreenPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawYellowPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawGreenPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawYellowPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, posratio1 + 20);
            //                    }





            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1+20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbh65535;
            //            ptbh65535 = (int)Math.Round(((ptbh * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            if (le1ldmin > pos0)
            //            {
            //                int le1ldminfor65535;
            //                le1ldminfor65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbw65535;
            //                else
            //                    dpos = le1ldminfor65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbh65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                int le1ldminfor65535;
            //                int le2ldminfor65535;
            //                int le1ldmaxfor65535;
            //                int le2ldmaxfor65535;
            //                le1ldmaxfor65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                le2ldminfor65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxfor65535;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminfor65535;
            //                else
            //                    dpos = posratio0 + ptbw65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbh65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                int le1ldminfor65535;
            //                int le2ldminfor65535;
            //                int le1ldmaxfor65535;
            //                int le2ldmaxfor65535;
            //                le2ldmaxfor65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxfor65535;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - dpos), ptbh65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    le1ldmin65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le1ldmax65535), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(le2ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le2ldmax65535), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}

            //        }
            //        panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //        ROIcheckBox.Focus();

            //    }
            //    else if (ptbmaxsizeheight == true)// when height exceed 65535
            //    {
            //        int h = PatternPB.Height;
            //        int w = PatternPB.Width;
            //        int pos0 = panel2.HorizontalScroll.Value;
            //        int pos1 = panel2.VerticalScroll.Value;
            //        int posratio0 = panel2.HorizontalScroll.Value;
            //        int posratio1 = panel2.VerticalScroll.Value;
            //        //pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //        pos1 = (int)(Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh)))) / rationew);
            //        pos0 = (int)(pos0 / rationew);
            //        //pos1 = (int)(pos1 / rationew);
            //        if (pos0 != pos0now || pos1 != pos1now)
            //        {
            //            patfresh = true;
            //        }
            //        else
            //        {
            //            patfresh = false;
            //        }
            //        pos0now = pos0;
            //        pos1now = pos1;
            //        int ptbw40082 = 0;
            //        int ptbh40082 = 0;
            //        ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //        ptbh40082 = (int)(ptbh40082 / rationew);
            //        //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //        //ptbw40082 = (int)(ptbw40082 / rationew);
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //        currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //        TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //        int ptbhratio = (int)(ptbh * rationew);
            //        int ptbwratio = (int)(ptbw * rationew);



            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;


            //        int epsilon = 2000;
            //        int maxsize = 40081;
            //        int maxwidthsize = 0;
            //        int maxheightsize = 0;
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            maxwidthsize = dosageh5image.colnum - 1;
            //            maxheightsize = dosageh5image.rownum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            maxwidthsize = ldh5image.colnum - 1;
            //            maxheightsize = ldh5image.rownum - 1;
            //        }
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                // OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                // PatternPB.Invalidate();
            //            }



            //        }
            //        //if (dosageflag == true)
            //        //{
            //        //    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //        //    float[,] currentarraydosage;
            //        //    dosageh5image.openfile(dosageimgpath);
            //        //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //        //    int m = currentarraydosage.GetLength(0);
            //        //    int n = currentarraydosage.GetLength(1);
            //        //    byte[,] currentarray = new byte[m, n];
            //        //    float min1 = float.MaxValue, max1 = 0;
            //        //    for (int i = 0; i < m; ++i)
            //        //        for (int j = 0; j < n; ++j)
            //        //        { < currentarraydosage[i, j])
            //        //            { max1 = currentarraydosage[i, j]; }
            //        //            if (min1 > currentarraydosage[i, j])
            //        //            { min1 = currentarraydosage[i, j]; }
            //        //        }
            //        //    for (int i = 0; i < m; ++i)
            //        //    {
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //        //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //        //        }
            //        //    }
            //        //    dosageimage = BufferToImage(currentarray);

            //        //    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //        //    dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //        //     //dosageflag = false;


            //        //}

            //        //if (ldflag == true)
            //        //{
            //        //    byte[,] currentarrayld;
            //        //    ldh5image.openfile(ldimgpath);
            //        //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //        //    ldimage = BufferTobinaryImage(currentarrayld);

            //        //    //ldflag = false;

            //        //}

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //        if (comboBox1.Text == "Dosage")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //dosageh5image.closefile();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //ldh5image.closefile();
            //        }
            //        if (comboBox1.Text == "LDFile")
            //        {

            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);

            //            //ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ptbw65535 = 0;

            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            for (int i = 0; i < 20; ++i)
            //            {

            //                //ptbw65535= (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)//判斷一樣用原始圖檔的判斷式去判斷
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        /*作圖需要轉換成height超過65535座標的座標系(代表只做一次轉換,*rationew即可)*/


            //                        // ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        // ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw / rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2) - 225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew + ptbw * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2 - 225, posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i]*rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw*rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax =(int)(ldxmax[i]*rationew);
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        // ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        // ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = (int)(le2ldxmax[i] * rationew);
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1+20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhfor65535;
            //            ptbhfor65535 = ldx65535 = (int)Math.Round(((ptbh * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            int ptbwnew;
            //            int le1ldminforhwight;
            //            int le1ldmaxforhwight;
            //            int le2ldminforhwight;
            //            int le2ldmaxforhwight;
            //            le1ldminforhwight = (int)(le1ldmin * rationew);
            //            le1ldmaxforhwight = (int)(le1ldmax * rationew);
            //            le2ldminforhwight = (int)(le2ldmin * rationew);
            //            le2ldmaxforhwight = (int)(le2ldmax * rationew);
            //            ptbwnew = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {
            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwnew;
            //                else
            //                    dpos = le1ldminforhwight;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxforhwight;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminforhwight;
            //                else
            //                    dpos = posratio0 + ptbwnew;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxforhwight;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwnew - dpos), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    //le1ldmin65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin * rationew )- posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - (int)(le1ldmax*rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin*rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - (int)(le2ldmax*rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}


            //        }
            //        if(comboBox1.Text=="Dosage")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);

            //            //ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ptbw65535 = 0;

            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            for (int i = 0; i < 20; ++i)
            //            {

            //                //ptbw65535= (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        // ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        // ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw / rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2) - 225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew + ptbw * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew) / 2 - 225, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2 - 225, posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = (int)(ldxmax[i] * rationew);
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        // ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        // ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        //ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = (int)(le2ldxmax[i] * rationew);
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1 + 20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhfor65535;
            //            ptbhfor65535 = ldx65535 = (int)Math.Round(((ptbh * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            int ptbwnew;
            //            int le1ldminforhwight;
            //            int le1ldmaxforhwight;
            //            int le2ldminforhwight;
            //            int le2ldmaxforhwight;
            //            le1ldminforhwight = (int)(le1ldmin * rationew);
            //            le1ldmaxforhwight = (int)(le1ldmax * rationew);
            //            le2ldminforhwight = (int)(le2ldmin * rationew);
            //            le2ldmaxforhwight = (int)(le2ldmax * rationew);
            //            ptbwnew = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {
            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwnew;
            //                else
            //                    dpos = le1ldminforhwight;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxforhwight;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminforhwight;
            //                else
            //                    dpos = posratio0 + ptbwnew;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxforhwight;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwnew - dpos), ptbhfor65535);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    //le1ldmin65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - (int)(le1ldmax * rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - (int)(le2ldmax * rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}


            //        }

            //        panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //    }
            //    else if (ptbmaxsizewidth == true)//when width exceed 65535
            //    {
            //        int h = PatternPB.Height;
            //        int w = PatternPB.Width;
            //        int pos0 = panel2.HorizontalScroll.Value;
            //        int pos1 = panel2.VerticalScroll.Value;
            //        int posratio0 = panel2.HorizontalScroll.Value;
            //        int posratio1 = panel2.VerticalScroll.Value;
            //        pos0 = (int)(Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw)))) / rationew);
            //        //pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //        //pos0 = (int)(pos0 / rationew);
            //        pos1 = (int)(pos1 / rationew);
            //        if (pos0 != pos0now || pos1 != pos1now)
            //        {
            //            patfresh = true;
            //        }
            //        else
            //        {
            //            patfresh = false;
            //        }
            //        pos0now = pos0;
            //        pos1now = pos1;
            //        int ptbw40082 = 0;
            //        int ptbh40082 = 0;
            //        //ptbh40082 = (int)Math.Round((ptbh * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //        //ptbh40082 = (int)(ptbh40082 / rationew);
            //        ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //        ptbw40082 = (int)(ptbw40082 / rationew);
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //        currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //        TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //        int ptbhratio = (int)(ptbh * rationew);
            //        int ptbwratio = (int)(ptbw * rationew);


            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;


            //        int epsilon = 2000;
            //        int maxsize = 40081;
            //        int maxwidthsize = 0;
            //        int maxheightsize = 0;
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            maxwidthsize = dosageh5image.colnum - 1;
            //            maxheightsize = dosageh5image.rownum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            maxwidthsize = ldh5image.colnum - 1;
            //            maxheightsize = ldh5image.rownum - 1;
            //        }
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                // OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                // PatternPB.Invalidate();
            //            }



            //        }
            //        //if (dosageflag == true)
            //        //{
            //        //    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //        //    float[,] currentarraydosage;
            //        //    dosageh5image.openfile(dosageimgpath);
            //        //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //        //    int m = currentarraydosage.GetLength(0);
            //        //    int n = currentarraydosage.GetLength(1);
            //        //    byte[,] currentarray = new byte[m, n];
            //        //    float min1 = float.MaxValue, max1 = 0;
            //        //    for (int i = 0; i < m; ++i)
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            if (max1 < currentarraydosage[i, j])
            //        //            { max1 = currentarraydosage[i, j]; }
            //        //            if (min1 > currentarraydosage[i, j])
            //        //            { min1 = currentarraydosage[i, j]; }
            //        //        }
            //        //    for (int i = 0; i < m; ++i)
            //        //    {
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //        //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //        //        }
            //        //    }
            //        //    dosageimage = BufferToImage(currentarray);

            //        //    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //        //    dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //        //     //dosageflag = false;


            //        //}

            //        //if (ldflag == true)
            //        //{
            //        //    byte[,] currentarrayld;
            //        //    ldh5image.openfile(ldimgpath);
            //        //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //        //    ldimage = BufferTobinaryImage(currentarrayld);

            //        //    //ldflag = false;

            //        //}

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //        if (comboBox1.Text == "Dosage")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //dosageh5image.closefile();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //ldh5image.closefile();
            //        }
            //        if (comboBox1.Text == "LDFile")
            //        {

            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);
            //            ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ptbw65535 = 0;

            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            for (int i = 0; i < 20; ++i)
            //            {

            //                //ptbw65535= (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin-175, posratio1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1+20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbwforwidth;
            //            int ptbhforwidth;
            //            int le1ldminforwidth;
            //            int le1ldmaxforwidth;
            //            int le2ldminforwidth;
            //            int le2ldmaxforwidth;
            //            ptbwforwidth= (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            ptbhforwidth = (int)(ptbh * rationew);
            //            le1ldminforwidth= (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le1ldmaxforwidth = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le2ldminforwidth = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le2ldmaxforwidth = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            if (le1ldmin > pos0)
            //            {
            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbhforwidth;
            //                else
            //                    dpos = le1ldminforwidth;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - pos0), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxforwidth;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminforwidth;
            //                else
            //                    dpos = posratio0 + ptbwforwidth;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le1ldmaxforwidth;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwforwidth - dpos), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    le1ldmin65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le1ldmax65535), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(le2ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le2ldmax65535), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}


            //        }

            //        if(comboBox1.Text=="Dosage")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);
            //            ldcnt.Clear();
            //            int diffh = h - ldh5image.rownum - 1;
            //            int diffw = w - ldh5image.colnum - 1;
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            int ldx65535 = 0;
            //            int ptbw65535 = 0;

            //            //ptbw40082 = (int)Math.Round((ptbw * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //ptbw40082 =(int)(ptbw40082 / rationew);//實際圖檔座標系
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            for (int i = 0; i < 20; ++i)
            //            {

            //                //ptbw65535= (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawRedPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawBluePen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                ptbw65535 = (int)Math.Round((ptbw * rationew * ((double)(ptbmax) / (magnifyratiowidth))));
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {




            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);

            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, ldx65535, 0, ldx65535, ptbmax);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldx65535, 0);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw40082)
            //                {

            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        e.Graphics.DrawLine(drawCyanPen, ldx65535, 0, ldx65535, maxheightsize);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldx65535, 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmin = ldx65535;
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = posratio0 + ptbw65535;
            //                    }
            //                    else
            //                    {
            //                        ldx65535 = (int)Math.Round(((le2ldxmax[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                        dxmax = ldx65535;
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin - 175, posratio1 + 20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbwforwidth;
            //            int ptbhforwidth;
            //            int le1ldminforwidth;
            //            int le1ldmaxforwidth;
            //            int le2ldminforwidth;
            //            int le2ldmaxforwidth;
            //            ptbwforwidth = (int)Math.Round(((ptbw * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            ptbhforwidth = (int)(ptbh * rationew);
            //            le1ldminforwidth = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le1ldmaxforwidth = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le2ldminforwidth = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            le2ldmaxforwidth = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            if (le1ldmin > pos0)
            //            {
            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbhforwidth;
            //                else
            //                    dpos = le1ldminforwidth;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - pos0), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxforwidth;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminforwidth;
            //                else
            //                    dpos = posratio0 + ptbwforwidth;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le1ldmaxforwidth;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwforwidth - dpos), ptbhforwidth);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    le1ldmin65535 = (int)Math.Round(((le1ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //    withoutexposurearealefttop.X = posratio0;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(le1ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    le1ldmax65535 = (int)Math.Round(((le1ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系

            //            //    withoutexposurearealefttop.X = le1ldmax65535;
            //            //    withoutexposurearealefttop.Y = posratio1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le1ldmax65535), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        le2ldmin65535 = (int)Math.Round(((le2ldmin * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.Y = posratio1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(le2ldmin65535 - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        le2ldmax65535 = (int)Math.Round(((le2ldmax * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //            //        withoutexposurearealefttop.X = posratio0;
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbw65535 - le2ldmax65535), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}


            //        }
            //        panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //    }
            //    else if (ptbminsizeall == true)//when ratio<100
            //    {
            //        int h = PatternPB.Height;
            //        int w = PatternPB.Width;
            //        int pos0 = panel2.HorizontalScroll.Value;
            //        int pos1 = panel2.VerticalScroll.Value;
            //        int posratio0 = panel2.HorizontalScroll.Value;
            //        int posratio1 = panel2.VerticalScroll.Value;
            //        pos0 = (int)(pos0 / rationew);
            //        pos1 = (int)(pos1 / rationew);
            //        if (pos0 != pos0now || pos1 != pos1now)
            //        {
            //            patfresh = true;
            //        }
            //        else
            //        {
            //            patfresh = false;
            //        }
            //        pos0now = pos0;
            //        pos1now = pos1;
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //        currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //        TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //        int ptbhratio = (int)(ptbh * rationew);
            //        int ptbwratio = (int)(ptbw * rationew);



            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;


            //        int epsilon = (int)((orgh5image.rownum) * 0.13); 
            //        int maxsize = 40081;
            //        int maxwidthsize = 0;
            //        int maxheightsize = 0;
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            maxwidthsize = dosageh5image.colnum - 1;
            //            maxheightsize = dosageh5image.rownum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            maxwidthsize = ldh5image.colnum - 1;
            //            maxheightsize = ldh5image.rownum - 1;
            //        }
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, pos0, pos1, rationew);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                // OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                currentarraytupleld = calculateposldfloatzoomout(ldimgpath, pos0, pos1, rationew);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1, rationew);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                // PatternPB.Invalidate();
            //            }



            //        }
            //        //if (dosageflag == true)
            //        //{
            //        //    currentarraytuple = calculateposorgfloatzoomout(Originalpath, pos0, pos1,rationew);
            //        //    float[,] currentarraydosage;
            //        //    dosageh5image.openfile(dosageimgpath);
            //        //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //        //    int m = currentarraydosage.GetLength(0);
            //        //    int n = currentarraydosage.GetLength(1);
            //        //    byte[,] currentarray = new byte[m, n];
            //        //    float min1 = float.MaxValue, max1 = 0;
            //        //    for (int i = 0; i < m; ++i)
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            if (max1 < currentarraydosage[i, j])
            //        //            { max1 = currentarraydosage[i, j]; }
            //        //            if (min1 > currentarraydosage[i, j])
            //        //            { min1 = currentarraydosage[i, j]; }
            //        //        }
            //        //    for (int i = 0; i < m; ++i)
            //        //    {
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //        //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //        //        }
            //        //    }
            //        //    dosageimage = BufferToImage(currentarray);

            //        //    //currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //        //    //dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //        //    //dosageflag = false;


            //        //}

            //        //if (ldflag == true)
            //        //{
            //        //    byte[,] currentarrayld;
            //        //    ldh5image.openfile(ldimgpath);
            //        //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //        //    ldimage = BufferTobinaryImage(currentarrayld);

            //        //    //ldflag = false;

            //        //}

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //        if (comboBox1.Text == "Dosage")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //dosageh5image.closefile();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //ldh5image.closefile();
            //        }
            //        if (comboBox1.Text == "LDFile")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            List<int> ldcnt = new List<int>();
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            for (int i = 0; i < 20; ++i)
            //            {
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }

            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw/rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2)+225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i]*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw/rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i]*rationew + ptbw*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw/ rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i]*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw/rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2, posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawGreenPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawYellowPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawGreenPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawYellowPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = le2ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Purple);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhformin;
            //            int ptbwformin;
            //            int le1ldminformin;
            //            int le1ldmaxformin;
            //            int le2ldminformin;
            //            int le2ldmaxformin;
            //            le1ldminformin = (int)(le1ldmin * rationew);
            //            le1ldmaxformin = (int)(le1ldmax * rationew);
            //            le2ldminformin = (int)(le2ldmin * rationew);
            //            le2ldmaxformin = (int)(le2ldmax * rationew);
            //            ptbhformin =(int)(ptbh * rationew);
            //            ptbwformin = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwformin;
            //                else
            //                    dpos = le1ldminformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxformin;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminformin;
            //                else
            //                    dpos = posratio0 + ptbwformin;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwformin - dpos), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    withoutexposurearealefttop.X = pos0;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin*rationew) - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    withoutexposurearealefttop.X = le1ldmax;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 +(int)( ptbw*rationew )- (int)(le1ldmax * rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = pos0;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le2ldmax * rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}
            //        }
            //        if(comboBox1.Text=="Dosage")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            List<int> ldcnt = new List<int>();
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }

            //            for (int i = 0; i < 20; ++i)
            //            {
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }

            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw/rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2)+225, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i]*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw/rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i]*rationew + ptbw*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw/ rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i]*rationew) / 2, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw/rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2, posratio1);
            //                //}
            //                if (ldxmax[i] >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawGreenPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawYellowPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawGreenPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawYellowPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = le2ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Purple);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhformin;
            //            int ptbwformin;
            //            int le1ldminformin;
            //            int le1ldmaxformin;
            //            int le2ldminformin;
            //            int le2ldmaxformin;
            //            le1ldminformin = (int)(le1ldmin * rationew);
            //            le1ldmaxformin = (int)(le1ldmax * rationew);
            //            le2ldminformin = (int)(le2ldmin * rationew);
            //            le2ldmaxformin = (int)(le2ldmax * rationew);
            //            ptbhformin = (int)(ptbh * rationew);
            //            ptbwformin = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwformin;
            //                else
            //                    dpos = le1ldminformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxformin;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminformin;
            //                else
            //                    dpos = posratio0 + ptbwformin;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwformin - dpos), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    withoutexposurearealefttop.X = pos0;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    withoutexposurearealefttop.X = le1ldmax;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le1ldmax * rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = pos0;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le2ldmax * rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}
            //        }

            //        panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //    }
            //    else if (ptbminsizeheight == true)//沒用到
            //    {

            //        {
            //            int h = PatternPB.Height;
            //            int w = PatternPB.Width;
            //            int pos0 = panel2.HorizontalScroll.Value;
            //            int pos1 = panel2.VerticalScroll.Value;
            //            int posratio0 = panel2.HorizontalScroll.Value;
            //            int posratio1 = panel2.VerticalScroll.Value;
            //            //pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //            pos0 = (int)(pos0 / rationew);
            //            pos1 = (int)(pos1 / rationew);
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + ptbw;
            //            currentrightbottompos.Y = pos1 + ptbh;
            //            TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);



            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxsize = 40081;
            //            int maxwidthsize = 0;
            //            int maxheightsize = 0;
            //            if (comboBox1.Text == "Dosage")
            //            {

            //                maxwidthsize = dosageh5image.colnum - 1;
            //                maxheightsize = dosageh5image.rownum - 1;
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {

            //                maxwidthsize = ldh5image.colnum - 1;
            //                maxheightsize = ldh5image.rownum - 1;
            //            }
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {

            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    // OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    // PatternPB.Invalidate();
            //                }



            //            }
            //            //if (dosageflag == true)
            //            //{
            //            //    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //            //    float[,] currentarraydosage;
            //            //    dosageh5image.openfile(dosageimgpath);
            //            //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //            //    int m = currentarraydosage.GetLength(0);
            //            //    int n = currentarraydosage.GetLength(1);
            //            //    byte[,] currentarray = new byte[m, n];
            //            //    float min1 = float.MaxValue, max1 = 0;
            //            //    for (int i = 0; i < m; ++i)
            //            //        for (int j = 0; j < n; ++j)
            //            //        {
            //            //            if (max1 < currentarraydosage[i, j])
            //            //            { max1 = currentarraydosage[i, j]; }
            //            //            if (min1 > currentarraydosage[i, j])
            //            //            { min1 = currentarraydosage[i, j]; }
            //            //        }
            //            //    for (int i = 0; i < m; ++i)
            //            //    {
            //            //        for (int j = 0; j < n; ++j)
            //            //        {
            //            //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //            //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //            //        }
            //            //    }
            //            //    dosageimage = BufferToImage(currentarray);

            //            //    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //            //    dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //            //    //dosageflag = false;


            //            //}

            //            //if (ldflag == true)
            //            //{
            //            //    byte[,] currentarrayld;
            //            //    ldh5image.openfile(ldimgpath);
            //            //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //            //    ldimage = BufferTobinaryImage(currentarrayld);

            //            //    //ldflag = false;

            //            //}

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (comboBox1.Text == "Dosage")
            //            {

            //                e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //                //dosageh5image.closefile();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {

            //                e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //                //ldh5image.closefile();
            //            }
            //            //if (comboBox1.Text == "LDFile")
            //            //{
            //            //    ldfile1.lightspotrange();
            //            //    Pen drawRedPen = new Pen(Color.Red, 5);
            //            //    Pen drawBluePen = new Pen(Color.Blue, 5);
            //            //    Pen drawGreenPen = new Pen(Color.Green, 5);
            //            //    Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            //    Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            //    List<int> ldcnt = new List<int>();
            //            //    int xepsilon = 100;
            //            //    int[] ldxmax;
            //            //    int[] ldxmin;
            //            //    //int cnt=0;
            //            //    ldxmax = ldfile1.ldxmax;
            //            //    ldxmin = ldfile1.ldxmin;
            //            //    for (int i = 0; i < 20; ++i)
            //            //    {
            //            //        if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //            //        {
            //            //            cnt = i + 1;
            //            //            Font drawFont = new Font("Arial", 15);
            //            //            SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //            //            if (cnt % 2 == 0)
            //            //            {
            //            //                e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);

            //            //            }
            //            //            else
            //            //            {
            //            //                e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);
            //            //            }







            //            //        }
            //            //        if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //            //        {
            //            //            cnt = i + 1;
            //            //            Font drawFont = new Font("Arial", 15);
            //            //            SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //            //            if (cnt % 2 == 0)
            //            //            {

            //            //                e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //            //            }
            //            //            else
            //            //            {
            //            //                e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //            //            }




            //            //        }


            //            //    }
            //            //    for (int j = 0; j < 20; ++j)
            //            //    {
            //            //        if (pos0 >= ldxmin[j] && ldxmax[j] >= pos0 || pos0 + ptbw >= ldxmin[j] && ldxmax[j] >= pos0 + ptbw)
            //            //        {
            //            //            ldcnt.Add(j + 1);
            //            //        }
            //            //        //if(pos0+ptbw >= ldxmin[j] && ldxmax[j] >= pos0+ ptbw)
            //            //        //{
            //            //        //ldcnt.Add(j+1);
            //            //        // }
            //            //    }
            //            //    if (ldcnt.Count == 1)
            //            //    {
            //            //        label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]);
            //            //    }
            //            //    else
            //            //    {
            //            //        label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]) + "~" + Convert.ToString(ldcnt[ldcnt.Count - 1]);
            //            //    }
            //            //    ldcnt.Clear();

            //            //}

            //            //if (comboBox1.Text == "Dosage")
            //            //{
            //            //    ldfile1.lightspotrange();
            //            //    Pen drawRedPen = new Pen(Color.Red, 5);
            //            //    Pen drawBluePen = new Pen(Color.Blue, 5);
            //            //    Pen drawGreenPen = new Pen(Color.Green, 5);
            //            //    Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            //    Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            //    List<int> ldcnt = new List<int>();
            //            //    int xepsilon = 100;
            //            //    int[] ldxmax;
            //            //    int[] ldxmin;
            //            //    //int cnt=0;
            //            //    ldxmax = ldfile1.ldxmax;
            //            //    ldxmin = ldfile1.ldxmin;
            //            //    for (int i = 0; i < 20; ++i)
            //            //    {
            //            //        if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //            //        {
            //            //            cnt = i + 1;
            //            //            Font drawFont = new Font("Arial", 15);
            //            //            SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //            //            if (cnt % 2 == 0)
            //            //            {
            //            //                e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], dosageh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);

            //            //            }
            //            //            else
            //            //            {
            //            //                e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], dosageh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);
            //            //            }







            //            //        }
            //            //        if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //            //        {
            //            //            cnt = i + 1;
            //            //            Font drawFont = new Font("Arial", 15);
            //            //            SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //            //            if (cnt % 2 == 0)
            //            //            {

            //            //                e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], dosageh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //            //            }
            //            //            else
            //            //            {
            //            //                e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], dosageh5image.rownum - 1);
            //            //                e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //            //            }




            //            //        }


            //            //    }
            //            //    for (int j = 0; j < 20; ++j)
            //            //    {
            //            //        if (pos0 >= ldxmin[j] && ldxmax[j] >= pos0 || pos0 + ptbw >= ldxmin[j] && ldxmax[j] >= pos0 + ptbw)
            //            //        {
            //            //            ldcnt.Add(j + 1);
            //            //        }
            //            //        //if(pos0+ptbw >= ldxmin[j] && ldxmax[j] >= pos0+ ptbw)
            //            //        //{
            //            //        //ldcnt.Add(j+1);
            //            //        // }
            //            //    }
            //            //    if (ldcnt.Count == 1)
            //            //    {
            //            //        label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]);
            //            //    }
            //            //    else
            //            //    {
            //            //        label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]) + "~" + Convert.ToString(ldcnt[ldcnt.Count - 1]);
            //            //    }
            //            //    ldcnt.Clear();
            //            //}

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //        }
            //    }
            //    else if (ptbminsizewidth == true)//沒用到
            //    {

            //        {
            //            int h = PatternPB.Height;
            //            int w = PatternPB.Width;
            //            int pos0 = panel2.HorizontalScroll.Value;
            //            int pos1 = panel2.VerticalScroll.Value;
            //            int posratio0 = panel2.HorizontalScroll.Value;
            //            int posratio1 = panel2.VerticalScroll.Value;
            //            pos0 = (int)Math.Round((pos0 * ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))));
            //            //pos1 = (int)Math.Round((pos1 * ((double)(magnifyratioheight - ptbh) / (ptbmax - ptbh))));
            //            pos0 = (int)(pos0 / rationew);
            //            pos1 = (int)(pos1 / rationew);
            //            currentlefttoppos.X = pos0;
            //            currentlefttoppos.Y = pos1;
            //            currentrightbottompos.X = pos0 + ptbw;
            //            currentrightbottompos.Y = pos1 + ptbh;
            //            TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //            TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //            TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //            TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //            int ptbhratio = (int)(ptbh * rationew);
            //            int ptbwratio = (int)(ptbw * rationew);


            //            Tuple<int[], byte[,]> currentarraytuple;
            //            Tuple<int[], byte[,]> currentarraytupledosage;
            //            Tuple<int[], byte[,]> currentarraytupleld;


            //            int epsilon = 2000;
            //            int maxsize = 40081;
            //            int maxwidthsize = 0;
            //            int maxheightsize = 0;
            //            if (comboBox1.Text == "Dosage")
            //            {

            //                maxwidthsize = dosageh5image.colnum - 1;
            //                maxheightsize = dosageh5image.rownum - 1;
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {

            //                maxwidthsize = ldh5image.colnum - 1;
            //                maxheightsize = ldh5image.rownum - 1;
            //            }
            //            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //            {

            //            }
            //            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //            {

            //            }
            //            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //            {

            //            }
            //            else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //            {

            //                if (comboBox1.Text == "Dosage")
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    dosageimage = BufferToImage(currentarraytupledosage.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    // OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else if (comboBox1.Text == "LDFile")
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    //PatternPB.Invalidate();
            //                }
            //                else
            //                {
            //                    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                    orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                    realposlefttop.X = currentarraytuple.Item1[0];
            //                    realposlefttop.Y = currentarraytuple.Item1[1];
            //                    realposrightbottom.X = currentarraytuple.Item1[2];
            //                    realposrightbottom.Y = currentarraytuple.Item1[3];
            //                    //OriginalPB.Invalidate();
            //                    // PatternPB.Invalidate();
            //                }



            //            }
            //            //if (dosageflag == true)
            //            //{
            //            //    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //            //    float[,] currentarraydosage;
            //            //    dosageh5image.openfile(dosageimgpath);
            //            //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //            //    int m = currentarraydosage.GetLength(0);
            //            //    int n = currentarraydosage.GetLength(1);
            //            //    byte[,] currentarray = new byte[m, n];
            //            //    float min1 = float.MaxValue, max1 = 0;
            //            //    for (int i = 0; i < m; ++i)
            //            //        for (int j = 0; j < n; ++j)
            //            //        {
            //            //            if (max1 < currentarraydosage[i, j])
            //            //            { max1 = currentarraydosage[i, j]; }
            //            //            if (min1 > currentarraydosage[i, j])
            //            //            { min1 = currentarraydosage[i, j]; }
            //            //        }
            //            //    for (int i = 0; i < m; ++i)
            //            //    {
            //            //        for (int j = 0; j < n; ++j)
            //            //        {
            //            //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //            //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //            //        }
            //            //    }
            //            //    dosageimage = BufferToImage(currentarray);

            //            //    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //            //    dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //            //    //dosageflag = false;


            //            //}

            //            //if (ldflag == true)
            //            //{
            //            //    byte[,] currentarrayld;
            //            //    ldh5image.openfile(ldimgpath);
            //            //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //            //    ldimage = BufferTobinaryImage(currentarrayld);

            //            //    //ldflag = false;

            //            //}

            //            Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //            ImageAttributes imageAttr = new ImageAttributes();
            //            Graphics.DrawImageAbort imageCallback
            //              = new Graphics.DrawImageAbort(DrawImageCallback8);
            //            IntPtr imageCallbackData = new IntPtr(1);
            //            GraphicsUnit units = GraphicsUnit.Pixel;
            //            //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //            Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //            if (comboBox1.Text == "Dosage")
            //            {

            //                e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //                //dosageh5image.closefile();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {

            //                e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //                panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //                //ldh5image.closefile();
            //            }
            //            if (comboBox1.Text == "LDFile")
            //            {
            //                ldfile1.lightspotrange();
            //                Pen drawRedPen = new Pen(Color.Red, 5);
            //                Pen drawBluePen = new Pen(Color.Blue, 5);
            //                Pen drawGreenPen = new Pen(Color.Green, 5);
            //                Pen drawPurplePen = new Pen(Color.Purple, 5);
            //                Pen drawOrangePen = new Pen(Color.Orange, 5);
            //                List<int> ldcnt = new List<int>();
            //                int xepsilon = 100;
            //                //int[] ldxmax;
            //                //int[] ldxmin;
            //                //int cnt=0;
            //                //ldxmax = ldfile1.ldxmax;
            //                //ldxmin = ldfile1.ldxmin;
            //                for (int i = 0; i < 20; ++i)
            //                {
            //                    if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //                    {
            //                        cnt = i + 1;
            //                        Font drawFont = new Font("Arial", 15);
            //                        SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                        if (cnt % 2 == 0)
            //                        {
            //                            e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);

            //                        }
            //                        else
            //                        {
            //                            e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], ldh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);
            //                        }







            //                    }
            //                    if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //                    {
            //                        cnt = i + 1;
            //                        Font drawFont = new Font("Arial", 15);
            //                        SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //                        if (cnt % 2 == 0)
            //                        {

            //                            e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //                        }
            //                        else
            //                        {
            //                            e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], ldh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //                        }




            //                    }


            //                }
            //                for (int j = 0; j < 20; ++j)
            //                {
            //                    if (pos0 >= ldxmin[j] && ldxmax[j] >= pos0 || pos0 + ptbw >= ldxmin[j] && ldxmax[j] >= pos0 + ptbw)
            //                    {
            //                        ldcnt.Add(j + 1);
            //                    }
            //                    //if(pos0+ptbw >= ldxmin[j] && ldxmax[j] >= pos0+ ptbw)
            //                    //{
            //                    //ldcnt.Add(j+1);
            //                    // }
            //                }
            //                if (ldcnt.Count == 1)
            //                {
            //                    label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]);
            //                }
            //                else
            //                {
            //                    label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]) + "~" + Convert.ToString(ldcnt[ldcnt.Count - 1]);
            //                }
            //                ldcnt.Clear();

            //            }

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                ldfile1.lightspotrange();
            //                Pen drawRedPen = new Pen(Color.Red, 5);
            //                Pen drawBluePen = new Pen(Color.Blue, 5);
            //                Pen drawGreenPen = new Pen(Color.Green, 5);
            //                Pen drawPurplePen = new Pen(Color.Purple, 5);
            //                Pen drawOrangePen = new Pen(Color.Orange, 5);
            //                List<int> ldcnt = new List<int>();
            //                int xepsilon = 100;
            //                //int[] ldxmax;
            //                //int[] ldxmin;
            //                //int cnt=0;
            //                //ldxmax = ldfile1.ldxmax;
            //                //ldxmin = ldfile1.ldxmin;
            //                for (int i = 0; i < 20; ++i)
            //                {
            //                    if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw)
            //                    {
            //                        cnt = i + 1;
            //                        Font drawFont = new Font("Arial", 15);
            //                        SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                        if (cnt % 2 == 0)
            //                        {
            //                            e.Graphics.DrawLine(drawRedPen, ldxmin[i], 0, ldxmin[i], dosageh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);

            //                        }
            //                        else
            //                        {
            //                            e.Graphics.DrawLine(drawBluePen, ldxmin[i], 0, ldxmin[i], dosageh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i], 0);
            //                        }







            //                    }
            //                    if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw)
            //                    {
            //                        cnt = i + 1;
            //                        Font drawFont = new Font("Arial", 15);
            //                        SolidBrush drawbrush = new SolidBrush(Color.Yellow);
            //                        if (cnt % 2 == 0)
            //                        {

            //                            e.Graphics.DrawLine(drawRedPen, ldxmax[i], 0, ldxmax[i], dosageh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //                        }
            //                        else
            //                        {
            //                            e.Graphics.DrawLine(drawBluePen, ldxmax[i], 0, ldxmax[i], dosageh5image.rownum - 1);
            //                            e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i], 20);
            //                        }




            //                    }


            //                }
            //                for (int j = 0; j < 20; ++j)
            //                {
            //                    if (pos0 >= ldxmin[j] && ldxmax[j] >= pos0 || pos0 + ptbw >= ldxmin[j] && ldxmax[j] >= pos0 + ptbw)
            //                    {
            //                        ldcnt.Add(j + 1);
            //                    }
            //                    //if(pos0+ptbw >= ldxmin[j] && ldxmax[j] >= pos0+ ptbw)
            //                    //{
            //                    //ldcnt.Add(j+1);
            //                    // }
            //                }
            //                if (ldcnt.Count == 1)
            //                {
            //                    label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]);
            //                }
            //                else
            //                {
            //                    label30.Text = "LD範圍:" + Convert.ToString(ldcnt[0]) + "~" + Convert.ToString(ldcnt[ldcnt.Count - 1]);
            //                }
            //                ldcnt.Clear();
            //            }
            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);
            //        }
            //    }
            //    else
            //    {
            //        int h = PatternPB.Height;
            //        int w = PatternPB.Width;
            //        int pos0 = panel2.HorizontalScroll.Value;
            //        int pos1 = panel2.VerticalScroll.Value;
            //        pos0 = (int)(pos0 / rationew);
            //        pos1 = (int)(pos1 / rationew);
            //        if (pos0 != pos0now || pos1 != pos1now)
            //        {
            //            patfresh = true;
            //        }
            //        else
            //        {
            //            patfresh = false;
            //        }
            //        pos0now = pos0;
            //        pos1now = pos1;
            //        currentlefttoppos.X = pos0;
            //        currentlefttoppos.Y = pos1;
            //        currentrightbottompos.X = pos0 + (int)(ptbw / rationew);
            //        currentrightbottompos.Y = pos1 + (int)(ptbh / rationew);
            //        TxtleftopxPat.Text = currentlefttoppos.X.ToString();
            //        TxtleftopyPat.Text = currentlefttoppos.Y.ToString();
            //        TxtrightbottomxPat.Text = currentrightbottompos.X.ToString();
            //        TxtrightbottomyPat.Text = currentrightbottompos.Y.ToString();
            //        int ptbhratio = (int)(ptbh * rationew);
            //        int ptbwratio = (int)(ptbw * rationew);
            //        int posratio0 = panel2.HorizontalScroll.Value;
            //        int posratio1 = panel2.VerticalScroll.Value;

            //        int currentarrayx2 = pos0 + ptbw + buffercol;
            //        int currentarrayy2 = pos1 + ptbh + bufferrow;

            //        Tuple<int[], byte[,]> currentarraytuple;
            //        Tuple<int[], byte[,]> currentarraytupledosage;
            //        Tuple<int[], byte[,]> currentarraytupleld;


            //        int epsilon = 2000;
            //        int maxsize = 40081;
            //        int maxwidthsize = 0;
            //        int maxheightsize = 0;
            //        if (comboBox1.Text == "Dosage")
            //        {

            //            maxwidthsize = dosageh5image.colnum - 1;
            //            maxheightsize = dosageh5image.rownum - 1;
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {

            //            maxwidthsize = ldh5image.colnum - 1;
            //            maxheightsize = ldh5image.rownum - 1;
            //        }
            //        if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            //        {

            //        }
            //        else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            //        {

            //        }
            //        else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            //        {

            //        }
            //        else if (pos0 - realposlefttop.X < epsilon || pos0 - realposrightbottom.X < epsilon || pos1 - realposlefttop.Y < epsilon || pos1 - realposrightbottom.Y < epsilon || realposlefttop.X - pos0 < epsilon || realposrightbottom.X - pos0 < epsilon || realposlefttop.Y - pos1 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            //        {

            //            if (comboBox1.Text == "Dosage")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                dosageimage = BufferToImage15(currentarraytupledosage.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                // OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else if (comboBox1.Text == "LDFile")
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                currentarraytupleld = calculateposld(ldimgpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                //PatternPB.Invalidate();
            //            }
            //            else
            //            {
            //                currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //                orgimage = BufferTobinaryImage(currentarraytuple.Item2);
            //                realposlefttop.X = currentarraytuple.Item1[0];
            //                realposlefttop.Y = currentarraytuple.Item1[1];
            //                realposrightbottom.X = currentarraytuple.Item1[2];
            //                realposrightbottom.Y = currentarraytuple.Item1[3];
            //                //OriginalPB.Invalidate();
            //                // PatternPB.Invalidate();
            //            }



            //        }
            //        //if (dosageflag == true)
            //        //{
            //        //    currentarraytuple = calculateposorg(Originalpath, pos0, pos1);
            //        //    float[,] currentarraydosage;
            //        //    dosageh5image.openfile(dosageimgpath);
            //        //    currentarraydosage = dosageh5image.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
            //        //    int m = currentarraydosage.GetLength(0);
            //        //    int n = currentarraydosage.GetLength(1);
            //        //    byte[,] currentarray = new byte[m, n];
            //        //    float min1 = float.MaxValue, max1 = 0;
            //        //    for (int i = 0; i < m; ++i)
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            if (max1 < currentarraydosage[i, j])
            //        //            { max1 = currentarraydosage[i, j]; }
            //        //            if (min1 > currentarraydosage[i, j])
            //        //            { min1 = currentarraydosage[i, j]; }
            //        //        }
            //        //    for (int i = 0; i < m; ++i)
            //        //    {
            //        //        for (int j = 0; j < n; ++j)
            //        //        {
            //        //            currentarraydosage[i, j] = (currentarraydosage[i, j] - min1) / (max1 - min1);
            //        //            currentarray[i, j] = (byte)(currentarraydosage[i, j] * 255);
            //        //        }
            //        //    }
            //        //    dosageimage = BufferToImage(currentarray);

            //        //    currentarraytupledosage = calculateposdosage(dosageimgpath, pos0, pos1);

            //        //    dosageimage = BufferToImage(currentarraytupledosage.Item2);

            //        //    // dosageflag = false;


            //        //}

            //        //if (ldflag == true)
            //        //{
            //        //    byte[,] currentarrayld;
            //        //    ldh5image.openfile(ldimgpath);
            //        //    currentarrayld = ldh5image.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

            //        //    ldimage = BufferTobinaryImage(currentarrayld);

            //        //    //ldflag = false;

            //        //}

            //        Rectangle Rectshowptb = new Rectangle(realposlefttop.X, realposlefttop.Y, ptbw, ptbh);
            //        ImageAttributes imageAttr = new ImageAttributes();
            //        Graphics.DrawImageAbort imageCallback
            //          = new Graphics.DrawImageAbort(DrawImageCallback8);
            //        IntPtr imageCallbackData = new IntPtr(1);
            //        GraphicsUnit units = GraphicsUnit.Pixel;
            //        //e.Graphics.DrawImage(orgimage, Rectshowptb, realposlefttop.X, realposlefttop.Y, ptbw, ptbh, units);
            //        Rectangle Rectimage = new Rectangle(posratio0, posratio1, ptbw, ptbh);
            //        if (comboBox1.Text == "Dosage")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(dosageimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //dosageh5image.closefile();
            //        }
            //        else if (comboBox1.Text == "LDFile")
            //        {
            //            if (patfresh == true)
            //            {
            //                PatternPB.Refresh();
            //                patfresh = false;
            //            }
            //            e.Graphics.DrawImage(ldimage, Rectimage, pos0 - realposlefttop.X, pos1 - realposlefttop.Y, ptbw / rationew, ptbh / rationew, units);

            //            panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //            //ldh5image.closefile();
            //        }
            //        if (comboBox1.Text == "LDFile")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);
            //            List<int> ldcnt = new List<int>();
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }
            //            for (int i = 0; i < 20; ++i)
            //            {
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] / rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] / rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (ldxmax[i]/rationew >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw / rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2)-100, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew + ptbw * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2 - 100, posratio1);
            //                //}
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] / rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 &&le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] / rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] / rationew >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = le2ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1+20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhformin;
            //            int ptbwformin;
            //            int le1ldminformin;
            //            int le1ldmaxformin;
            //            int le2ldminformin;
            //            int le2ldmaxformin;
            //            le1ldminformin = (int)(le1ldmin * rationew);
            //            le1ldmaxformin = (int)(le1ldmax * rationew);
            //            le2ldminformin = (int)(le2ldmin * rationew);
            //            le2ldmaxformin = (int)(le2ldmax * rationew);
            //            ptbhformin = (int)(ptbh * rationew);
            //            ptbwformin = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwformin;
            //                else
            //                    dpos = le1ldminformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxformin;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminformin;
            //                else
            //                    dpos = posratio0 + ptbwformin;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwformin - dpos), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    withoutexposurearealefttop.X = pos0;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    withoutexposurearealefttop.X = le1ldmax;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le1ldmax * rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = pos0;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le2ldmax * rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}

            //        }
            //        if(comboBox1.Text=="Dosage")
            //        {
            //            //ldfile1.lightspotrange();
            //            Pen drawRedPen = new Pen(Color.Red, 5);
            //            Pen drawBluePen = new Pen(Color.Blue, 5);
            //            Pen drawGreenPen = new Pen(Color.Green, 5);
            //            Pen drawYellowPen = new Pen(Color.Yellow, 5);
            //            Pen drawPurplePen = new Pen(Color.Purple, 5);
            //            Pen drawOrangePen = new Pen(Color.Orange, 5);
            //            Pen drawCyanPen = new Pen(Color.Cyan, 5);
            //            Pen drawPaleVioletRedPen = new Pen(Color.PaleVioletRed, 5);
            //            List<int> ldcnt = new List<int>();
            //            int xepsilon = 100;
            //            //int[] ldxmax;
            //            //int[] ldxmin;
            //            //int cnt=0;
            //            //ldxmax = ldfile1.ldxmax;
            //            //ldxmin = ldfile1.ldxmin;
            //            if (pos0 + ptbw >= le1ldmin && pos0 <= le1ldmax)
            //            {

            //                le1flag = true;
            //            }



            //            if (pos0 + ptbw >= le2ldmin && pos0 <= le2ldmax)
            //            {
            //                le2flag = true;
            //            }

            //            if (le1flag == true && le2flag == false)
            //            {
            //                label30.Text = "LE1";
            //            }
            //            else if (le2flag == true && le1flag == false)
            //            {
            //                label30.Text = "LE2";
            //            }
            //            else if (le1flag == true && le2flag == true)
            //            {
            //                label30.Text = "LE1+LE2";
            //            }
            //            else if (le1flag == false && le2flag == false)
            //            {
            //                label30.Text = "";
            //            }
            //            for (int i = 0; i < 20; ++i)
            //            {
            //                if (ldxmin[i] >= pos0 && ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawRedPen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] / rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmin[i] * rationew, 0, ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (ldxmax[i] >= pos0 && ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawRedPen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] / rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawBluePen, ldxmax[i] * rationew, 0, ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (ldxmax[i] / rationew >= pos0 && pos0 + ptbw >= ldxmin[i])
            //                {
            //                    if (ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(ldxmin[i] * rationew);
            //                    }
            //                    if (ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1);
            //                }
            //                //if (pos0 >= ldxmin[i] && pos0 + ptbw / rationew <= ldxmax[i] && pos0 <= ldxmax[i] && pos0 + ptbw / rationew >= ldxmin[i])
            //                //{
            //                //    cnt = i + 1;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, posratio0 + (ptbw * rationew / 2)-100, posratio1);
            //                //}

            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] > ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] > ldxmin[i])
            //                //{
            //                //    //ldx65535 = (int)Math.Round(((ldxmin[i] * rationew) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))));//65535座標系
            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew + ptbw * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmax[i] > pos0 && pos0 + ptbw / rationew > ldxmax[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i + 2;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmax[i] * rationew) / 2 - 100, posratio1);
            //                //}
            //                //if (ldxmin[i] > pos0 && pos0 + ptbw / rationew > ldxmin[i] && ldxmax[i] < ldxmin[i])
            //                //{

            //                //    cnt = i;
            //                //    Font drawFont = new Font("Arial", 10);
            //                //    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);

            //                //    e.Graphics.DrawString("LD" + cnt.ToString() + "範圍", drawFont, drawbrush, (posratio0 + ldxmin[i] * rationew + ptbw * rationew) / 2 - 100, posratio1);
            //                //}
            //                if (le2ldxmin[i] >= pos0 && le2ldxmin[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {
            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] / rationew, posratio1);

            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmin[i] * rationew, 0, le2ldxmin[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Left", drawFont, drawbrush, ldxmin[i] * rationew, posratio1);
            //                    }







            //                }
            //                if (le2ldxmax[i] >= pos0 && le2ldxmax[i] <= pos0 + ptbw / rationew)
            //                {
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Chartreuse);
            //                    if (cnt % 2 == 0)
            //                    {

            //                        e.Graphics.DrawLine(drawPaleVioletRedPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] / rationew, posratio1 + 20);
            //                    }
            //                    else
            //                    {
            //                        e.Graphics.DrawLine(drawCyanPen, le2ldxmax[i] * rationew, 0, le2ldxmax[i] * rationew, ldh5image.rownum - 1);
            //                        //e.Graphics.DrawString("LD" + cnt.ToString() + "Right", drawFont, drawbrush, ldxmax[i] * rationew, posratio1 + 20);
            //                    }




            //                }
            //                if (le2ldxmax[i] / rationew >= pos0 && pos0 + ptbw >= le2ldxmin[i])
            //                {
            //                    if (le2ldxmin[i] < pos0)
            //                    {
            //                        dxmin = posratio0;
            //                    }
            //                    else
            //                    {
            //                        dxmin = (int)(le2ldxmin[i] * rationew);
            //                    }
            //                    if (le2ldxmax[i] > pos0 + ptbw)
            //                    {
            //                        dxmax = (int)(posratio0 + ptbw * rationew);
            //                    }
            //                    else
            //                    {
            //                        dxmax = le2ldxmax[i];
            //                    }
            //                    cnt = i + 1;
            //                    Font drawFont = new Font("Arial", 10);
            //                    SolidBrush drawbrush = new SolidBrush(Color.Yellow);

            //                    e.Graphics.DrawString("LD" + cnt.ToString(), drawFont, drawbrush, (dxmax - dxmin) / 2 + dxmin, pos1 + 20);
            //                }
            //            }
            //            int dpos, dpos0;
            //            int ptbhformin;
            //            int ptbwformin;
            //            int le1ldminformin;
            //            int le1ldmaxformin;
            //            int le2ldminformin;
            //            int le2ldmaxformin;
            //            le1ldminformin = (int)(le1ldmin * rationew);
            //            le1ldmaxformin = (int)(le1ldmax * rationew);
            //            le2ldminformin = (int)(le2ldmin * rationew);
            //            le2ldmaxformin = (int)(le2ldmax * rationew);
            //            ptbhformin = (int)(ptbh * rationew);
            //            ptbwformin = (int)(ptbw * rationew);
            //            if (le1ldmin > pos0)
            //            {


            //                if (le1ldmin > pos0 + ptbw)
            //                    dpos = posratio0 + ptbwformin;
            //                else
            //                    dpos = le1ldminformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                //SolidBrush mybrush = new SolidBrush(Color.Orange);
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - posratio0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmin > le1ldmax && pos0 < le2ldmin && pos0 + ptbw > le1ldmax)
            //            {
            //                if (le1ldmax > pos0)
            //                    dpos0 = le1ldmaxformin;
            //                else
            //                    dpos0 = posratio0;

            //                if (le2ldmin < pos0 + ptbw)
            //                    dpos = le2ldminformin;
            //                else
            //                    dpos = posratio0 + ptbwformin;

            //                withoutexposurearealefttop.X = pos0;
            //                withoutexposurearealefttop.Y = pos1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(dpos - dpos0), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            else if (le2ldmax < pos0 + ptbw)
            //            {
            //                if (le2ldmax < pos0)
            //                    dpos = posratio0;
            //                else
            //                    dpos = le2ldmaxformin;

            //                withoutexposurearealefttop.X = posratio0;
            //                withoutexposurearealefttop.Y = posratio1;
            //                SolidBrush mybrush = new SolidBrush(Color.FromArgb(100, 255, 255, 0));
            //                withoutexposurearea.Location = withoutexposurearealefttop;
            //                withoutexposurearea.Size = new Size(Math.Abs(posratio0 + ptbwformin - dpos), ptbhformin);
            //                e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            }
            //            //if (le1ldmin > pos0)
            //            //{
            //            //    withoutexposurearealefttop.X = pos0;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs((int)(le1ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (pos0 + ptbw > le1ldmax)
            //            //{
            //            //    withoutexposurearealefttop.X = le1ldmax;
            //            //    withoutexposurearealefttop.Y = pos1;
            //            //    SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //    withoutexposurearea.Location = withoutexposurearealefttop;
            //            //    withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le1ldmax * rationew)), ldh5image.rownum - 1);
            //            //    e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //}
            //            //if (label30.Text == "LE2")
            //            //{
            //            //    if (le2ldmin > pos0 && pos0 > le1ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = pos0;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs((int)(le2ldmin * rationew) - posratio0), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }
            //            //    if (pos0 + ptbw > le2ldmax)
            //            //    {
            //            //        withoutexposurearealefttop.X = le2ldmax;
            //            //        withoutexposurearealefttop.Y = pos1;
            //            //        SolidBrush mybrush = new SolidBrush(Color.Orange);
            //            //        withoutexposurearea.Location = withoutexposurearealefttop;
            //            //        withoutexposurearea.Size = new Size(Math.Abs(posratio0 + (int)(ptbw * rationew) - (int)(le2ldmax * rationew)), ldh5image.rownum - 1);
            //            //        e.Graphics.FillRectangle(mybrush, withoutexposurearea);
            //            //    }

            //            //}

            //        }

            //        panel1.AutoScrollPosition = new Point(posratio0, posratio1);

            //    }




            //}

            //if (mouseevent == true)
            //{
            //    //if (patternrectflag == true)
            //    //{
            //    //if (PatternPB.Image != null)
            //    //{

            //    if (RectPattern != null && RectPattern.Width > 0 && RectPattern.Height > 0) //&& down == true)
            //    {
            //        e.Graphics.DrawRectangle(new Pen(Color.Red, 2), RectPattern);
            //        OriginalPB.Invalidate();
            //    }
            //    //}
            //    //    patternrectflag = false;
            //    //}

            //}
            #endregion oldversion
        }

        private void Txtratio_TextChanged(object sender, EventArgs e)
        {
            int[] size = new int[4];
            int[] size1 = new int[4];
            if (Txtratio.Text != "")
            {
                if (guimainflag == false)
                {
                    //MessageBox.Show("請先按下Original按鈕選擇原始Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

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

                            //if (patimg != null)
                            {
                                ratio(a, realres);
                            }
                            //ratio(a, realres);// a is the percentage,realres is the oldres;
                            //mouseevent = true;
                            if (ROIshowCB.Checked && patroi_img.img != null)
                            {

                                PatternPB.Height = patroi_img.img.Height;
                                PatternPB.Width = patroi_img.img.Width;
                                OriginalPB.Height = patroi_img.img.Height;
                                OriginalPB.Width = patroi_img.img.Width;
                                var s3 = PatternPB.Size;
                                var s4 = OriginalPB.Size;
                                int Patwidth = s3.Width;
                                int Patheight = s3.Height;
                                int Oriwidth = s4.Width;
                                int Oriheight = s4.Height;
                                size = orgimg.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                                s3.Width = size[0];
                                s3.Height = size[1];
                                s4.Width = size[2];
                                s4.Height = size[3];
                                PatternPB.Size = s3;
                                OriginalPB.Size = s4;
                                PatternPB.Invalidate();
                                OriginalPB.Invalidate();

                            }




                        }
                        else
                        {

                            MessageBox.Show("請輸入10~1000的數字", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

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
                    ratio(a, realres);
                }

            }


            aTimer.Enabled = false;
        }


        private void PatternPB_MouseUp(object sender, MouseEventArgs e)
        {
            if (mouseevent == true)
            {
                //Originalflag = true;
                if (orgflag == false)
                {
                    return;
                }
                if (e.Button != MouseButtons.Left)
                    return;

                int Rectwidth = RectOriginal.Width;
                int Rectheight = RectOriginal.Height;

                ROIrightbottom.X = ROIlefttop.X + (int)(Rectwidth / orgimg.ratio);
                ROIrightbottom.Y = ROIlefttop.Y + (int)(Rectheight / orgimg.ratio);

                TxtrightbottomxROI.Text = Convert.ToString(ROIrightbottom.X);
                TxtrightbottomyROI.Text = Convert.ToString(ROIrightbottom.Y);
                if (TxtlefttopxROI.Text != "" && TxtlefttopyROI.Text != "" && TxtrightbottomxROI.Text != "" && TxtrightbottomyROI.Text != "")
                {
                    int roiheight = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roiwidth = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                    TxtroiHeight.Text = Convert.ToString(roiheight);
                    TxtroiWidth.Text = Convert.ToString(roiwidth);
                }
                down = false;
                OriginalPB.Invalidate();
                PatternPB.Invalidate();

            }
        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            mousewheelflag = false;

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            mousewheelflag = false;
        }

        private void PatternPB_Click(object sender, EventArgs e)
        {

        }

        private void OriginalPB_MouseHover(object sender, EventArgs e)
        {

        }

        private void TxtLevel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RectoptCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ROIcheckBox.Checked && RectoptCB.Checked)
            {
                MessageBox.Show("ROIcheckbox和Rectoptcheckbox不能同時勾選", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                optflag = true;
                ROIcheckBox.Checked = false;
                RectoptCB.Checked = false;
               
                return;
            }
            if (optflag == false)
            {
                optflag = true;
            }
            else
            {
                optflag = false;
            }
            if (!RectoptCB.Checked)
            {
                rectopt.Size = new Size(0, 0);
                TxtlefttopyROI.Text = "";
                TxtlefttopxROI.Text = "";
                TxtrightbottomyROI.Text = "";
                TxtrightbottomxROI.Text = "";
                OriginalPB.Invalidate();
                PatternPB.Invalidate();
                mouseright = false;
            }
        }

        private void optimizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            opt = new Optimization();
            //if (optst1result.Length == 0|| optst2result.Length == 0|| optst3result.Length == 0)
            //{


            //}
            //else
            {
                opt.f1 = this;
                opt.ShowDialog();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ldfilepath1 = TxtLDFile1.Text;
            //ldfilepath2 = TxtLDFile2.Text;
            OpenFileDialog LDFileopen = new OpenFileDialog();
            LDFileopen.Title = "Select file";
            //dialog.InitialDirectory = "D:\\";
            LDFileopen.Filter = " LD | *.ld; *.ld";
            if (LDFileopen.ShowDialog() == DialogResult.OK)
            {
                int findle;
                TxtLDFile1.Text = LDFileopen.FileName;
                findle = TxtLDFile1.Text.IndexOf("LE");

                TxtLDFile2.Text = TxtLDFile1.Text.Remove(findle + 2, 1);
                TxtLDFile2.Text = TxtLDFile2.Text.Insert(findle + 2, "2");


                ini.IniWriteValue("Section", "ldfilepath1", TxtLDFile1.Text, filenameini);
                ini.IniWriteValue("Section", "ldfilepath2", TxtLDFile2.Text, filenameini);


            }




        }

        private void parameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            par.f1 = this;
            par.ShowDialog();
            if (par.maxsize > 0.5 && par.maxsize < 5)
            {
                ini.IniWriteValue("Section", "MaxSize", Convert.ToString(par.maxsize), filenameini);
            }
            else
            {
                MessageBox.Show("Maxsize輸入大小必須為2~10GB", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (par.winrow > 0)
            {
                ini.IniWriteValue("Section", "OrgWinRow", Convert.ToString(par.winrow), filenameini);
            }
            else
            {
                MessageBox.Show("OrgWinRow輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (par.wincol > 0)
            {
                ini.IniWriteValue("Section", "OrgWinCol", Convert.ToString(par.wincol), filenameini);
            }
            else
            {
                MessageBox.Show("OrgWinCol輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            if (par.omaxiter > 0)
            {
                ini.IniWriteValue("Section", "Omaxiter", Convert.ToString(par.omaxiter), filenameini);

            }
            else
            {
                MessageBox.Show("Omaxiter輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
            if (par.propotion > 0 && par.propotion < 1)
            {
                ini.IniWriteValue("Section", "Propotion", Convert.ToString(par.propotion), filenameini);



            }
            else
            {
                MessageBox.Show("Propotion輸入大小必需在0~1之間", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (par.Constantiteration > 0)
            {
                ini.IniWriteValue("Section", "Constantiteration", Convert.ToString(par.Constantiteration), filenameini);
            }
            else
            {
                MessageBox.Show("Constantiteration輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(par.tabuproportion > 0 && par.tabuproportion < 1)
            {
                ini.IniWriteValue("Section", "tabuproportion", Convert.ToString(par.tabuproportion), filenameini);
            }
            else
            {
                MessageBox.Show("tabuproportion輸入大小必需在0~1之間", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(par.lossweight > 0)
            {
                ini.IniWriteValue("Section", "LossWeight", Convert.ToString(par.lossweight), filenameini);
            }
            else
            {
                MessageBox.Show("LossWeight輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (par.Epsilon > 0)
            //{
            //    ini.IniWriteValue("Section", "Epsilon", Convert.ToString(par.Epsilon), filenameini);
            //}
            //else
            //{
            //    MessageBox.Show("Epsilon輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //ini.IniWriteValue("Section", "MaxSize", Convert.ToString(par.maxsize), filenameini);
            //ini.IniWriteValue("Section", "OrgWinRow", Convert.ToString(par.winrow), filenameini);
            //ini.IniWriteValue("Section", "OrgWinCol", Convert.ToString(par.wincol), filenameini);
            //ini.IniWriteValue("Section", "Omaxiter", Convert.ToString(par.omaxiter), filenameini);
            //ini.IniWriteValue("Section", "Propotion", Convert.ToString(par.propotion), filenameini);


        }

        private void OriginalPB_QueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {

        }

        private void BtnTmp_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FolderPath = new FolderBrowserDialog();
            if (FolderPath.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(FolderPath.SelectedPath))
                {
                    MessageBox.Show("路徑下的資料夾不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tempfolderfail = true;
                    return;

                }
                else
                {
                    tempfolderfail = false;
                    tempfolderpath = FolderPath.SelectedPath + "\\";
                    ini.IniWriteValue("Section", "TempFolder", tempfolderpath, filenameini);
                    tempfoldertxt.Text = tempfolderpath;
                }

            }
        }

        private void BtnROI_MOD_Click(object sender, EventArgs e)
        {
            if (guimainflag == false)
            {
                MessageBox.Show("請先按下Original按鈕選擇原始Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select file";
            //dialog.InitialDirectory = "D:\\";
            dialog.Filter = " HDF5 | *.h5";
            dialog.InitialDirectory = tempfoldertxt.Text;
            //if (openfilemodfirstflag == true)
            //{
            //    openfilemodfirstflag = false;
            //    dialog.InitialDirectory = openfilemodfirst;
            //}
            //else
            //{

            //}
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string h5filename;
                string h5temp;
                string lefttopx;
                string lefttopy;
                string rightx;
                string righty;
                h5filename = Path.GetFileName(dialog.FileName);
                //modpath = Path.GetDirectoryName(dialog.FileName);

                int findroi;
                int findh5;
                int find_1;
                int find_2;
                int find_3;
                int find_4;

                findroi = h5filename.IndexOf("roi");
                if (findroi == -1)
                {
                    MessageBox.Show("請選擇ROI檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                findh5 = h5filename.IndexOf(".h5");

                h5temp = h5filename.Remove(0, findroi + 3);
                findh5 = h5temp.IndexOf(".h5");
                h5temp = h5temp.Remove(findh5, 3);
                roimousexy = h5temp;
                find_1 = roimousexy.IndexOf("_");
                find_2 = roimousexy.IndexOf("_", find_1 + 1);
                find_3 = roimousexy.IndexOf("_", find_2 + 1);
                find_4 = roimousexy.IndexOf("_", find_3 + 1);
                lefttopx = roimousexy.Remove(find_2);
                lefttopx = lefttopx.Remove(0, 1);
                lefttopy = roimousexy.Remove(find_3);
                lefttopy = lefttopy.Remove(find_1, find_2 + 1);
                rightx = roimousexy.Remove(find_4);
                rightx = rightx.Remove(find_1, find_3 + 1);
                righty = roimousexy.Remove(find_1, find_4 + 1);
                Txtltx.Text = lefttopx;
                Txtlty.Text = lefttopy;
                Txtrbx.Text = rightx;
                Txtrby.Text = righty;
                roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
                roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level" + roimousexy + ".h5";
                ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi" + roimousexy + ".h5";
                ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi" + roimousexy + ".h5";
                dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5";
                dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5";
                Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";
                Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
                xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level" + roimousexy + ".h5";
                xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level" + roimousexy + ".h5";




            }


        }

        private void panel2_MouseEnter(object sender, EventArgs e)
        {
            //roiscroll = true;

        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {
            //roiscroll = false;
        }

        private void RectoptCB_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            //roiscroll = false;
        }

        private void panel2_Scroll(object sender, ScrollEventArgs e)
        {
            if (ROIshowCB.Checked)
            {
                roiscroll = true;
            }
            else
            {
                roiscroll = false;
            }

        }

        private void Txtinputname_TextChanged(object sender, EventArgs e)
        {

        }

        private void ldconfig1txt_TextChanged(object sender, EventArgs e)
        {

        }

        private void ROIgb_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DeleteSrcFolder(tempfoldertxt.Text);
        }

        private void Btnor_Click(object sender, EventArgs e)
        {

            {

                //roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                roimousexy = "";
                string ld_stage1_opt1, ld_stage1_opt2, ld_stage2_opt1, ld_stage2_opt2, ld_stage4_opt1, ld_stage4_opt2;
                int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //if (!ROIcheckBox.Checked && !RectoptCB.Checked && CB1.Checked)
                //{
                //    ld_stage1_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                //    ld_stage1_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                //    ld_stage2_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_2stage_2level_opt.LD";
                //    ld_stage2_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_2stage_2level_opt.LD";
                //    if (File.Exists(ldfilepath1_2level) && File.Exists(ldfilepath2_2level))
                //    {
                //        if (File.Exists(ld_stage1_opt1))
                //        {
                //            string ldfile_2level_roi1 = tiffimg.imgfilename + "_LE1_2level_roi" + roimousexy + ".LD";
                //            string ldfile_2level_roi1_opt = tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                //            File.Replace(ldfile_2level_roi1, ldfile_2level_roi1_opt, ldfile_2level_roi1_opt);
                //            //string ldfile_2level_roi2 = tiffimg.imgfilename + "_LE2_2level_roi.LD";
                //            //string ldfile_2level_roi2_opt = tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                //            //File.Replace(ldfile_2level_roi2, ldfile_2level_roi2_opt, ldfile_2level_roi2_opt);
                //        }
                //        else
                //        {
                //            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        }
                //        if (File.Exists(ld_stage1_opt2))
                //        {
                //            string ldfile_2level_roi2 = tiffimg.imgfilename + "_LE2_2level_roi.LD";
                //            string ldfile_2level_roi2_opt = tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                //            File.Replace(ldfile_2level_roi2, ldfile_2level_roi2_opt, ldfile_2level_roi2_opt);
                //        }
                //        else
                //        {
                //            MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        }

                //    }
                //    else
                //    {
                //        MessageBox.Show("Temp資料夾不存在roi的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    }



                //}
                if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB3.Checked)
                {
                    if (ScanningCB.Checked)
                    {
                        MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    roimousexy = "";
                    ld_stage4_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_3stage_4level_opt.LD";
                    ld_stage4_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_3stage_4level_opt.LD";
                    string add4="";
                    if (File.Exists(overwriteld1))
                    {
                        if (File.Exists(ld_stage4_opt1))
                        {
                            string ldfile1_4level = tiffimg.imgfilename + "_LE1_4level.LD";
                            string ldfile1_4level_opt = tiffimg.imgfilename + "_LE1_3stage_4level_opt.LD";
                            //File.Replace(ldfile1_4level, ldfile1_4level_opt, ldfile1_4level_opt);
                            
                            int find2level;
                            find2level = overwriteld1.IndexOf("2level");
                            if (find2level == -1)
                            {
                                File.Delete(overwriteld1);
                                File.Move(ld_stage4_opt1, overwriteld1);
                            }
                            else
                            {
                                string remove2 = overwriteld1.Remove(find2level,6);
                                 add4 = remove2.Insert(find2level, "4level");
                                if (File.Exists(add4))
                                    File.Delete(add4);
                                
                                 File.Move(ld_stage4_opt1, add4);
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (File.Exists(overwriteld2))
                    {
                        if (File.Exists(ld_stage4_opt2))
                        {
                            string ldfile2_4level = tiffimg.imgfilename + "_LE2_2level.LD";
                            string ldfile2_4level_opt = tiffimg.imgfilename + "_LE2_2stage_2level_opt.LD";
                            File.Delete(overwriteld2);
                            int find2level;
                            find2level = overwriteld2.IndexOf("2level");
                            if (find2level == -1)
                            {
                                File.Move(ld_stage4_opt2, overwriteld2);
                            }
                            else
                            {
                                string remove2 = overwriteld2.Remove(find2level, 6);
                                 add4 = remove2.Insert(find2level, "4level");
                                if (File.Exists(add4))
                                    File.Delete(add4);
                                File.Move(ld_stage4_opt2, add4);
                            }
                          
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB2.Checked)
                {
                    if (ScanningCB.Checked)
                    {
                        MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    roimousexy = "";
                    ld_stage2_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_2stage_4level_opt.LD";
                    ld_stage2_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_2stage_4level_opt.LD";
                    string add4 = "";
                    if (File.Exists(overwriteld1))
                    {
                        if (File.Exists(ld_stage2_opt1))
                        {
                            string ldfile1_2level = tiffimg.imgfilename + "_LE1_2level.LD";
                            string ldfile1_2level_opt = tiffimg.imgfilename + "_LE1_2stage_4level_opt.LD";
                           
                            int find2level;
                            find2level = overwriteld1.IndexOf("2level");
                            if (find2level == -1)
                            {
                                File.Delete(overwriteld1);
                                File.Move(ld_stage2_opt1, overwriteld1);
                            }
                            else
                            {
                                string remove2 = overwriteld1.Remove(find2level, 6);
                                add4 = remove2.Insert(find2level, "4level");
                                if (File.Exists(add4))
                                    File.Delete(add4);
                                File.Move(ld_stage2_opt1, add4);
                            }
                           
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (File.Exists(overwriteld2))
                    {
                        if (File.Exists(ld_stage2_opt2))
                        {
                            string ldfile2_2level = tiffimg.imgfilename + "_LE2_2level.LD";
                            string ldfile2_2level_opt = tiffimg.imgfilename + "_LE2_2stage_4level_opt.LD";
                            File.Delete(overwriteld2);
                            int find2level;
                            find2level = overwriteld2.IndexOf("2level");
                            if (find2level == -1)
                            {
                                File.Move(ld_stage2_opt2, overwriteld2);
                            }
                            else
                            {
                                string remove2 = overwriteld2.Remove(find2level, 6);
                                add4 = remove2.Insert(find2level, "4level");
                                if (File.Exists(add4))
                                    File.Delete(add4);
                                File.Move(ld_stage2_opt2, add4);
                            }
                            
                            //File.Replace(ldfile2_2level, ldfile2_2level_opt, ldfile2_2level_opt);
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB1.Checked)
                {
                    if (ScanningCB.Checked)
                    {
                        MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    roimousexy = "";
                    ld_stage1_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                    //string ld_stage1_opt1_backup= tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt_backup.LD";
                    //string ld_stage1_opt2_backup = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt_backup.LD";
                    ld_stage1_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";

                    if (File.Exists(overwriteld1))
                    {
                        if (File.Exists(ld_stage1_opt1))
                        {
                            //string ldfile1_2level = tiffimg.imgfilename + "_LE1_2level.LD";
                            //string ldfile1_2level_opt = tiffimg.imgfilename + "_LE1_1stage_2level_opt.LD";
                            //File.Replace(ldfilepath1_2level, ld_stage1_opt1, ld_stage1_opt1_backup);
                            File.Delete(overwriteld1);
                            File.Move(ld_stage1_opt1, overwriteld1);
                            guimainCode update = new guimainCode();
                            update.updateldimg(ldimgname_2levelpath, tempfolderpath,beamfile + "spotarrays100le1_0125.h5", roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    if (File.Exists(overwriteld2))
                    {
                        if (File.Exists(ld_stage1_opt2))
                        {
                            //string ldfile2_2level = tiffimg.imgfilename + "_LE2_2level.LD";
                            //string ldfile2_2level_opt = tiffimg.imgfilename + "_LE2_1stage_2level_opt.LD";
                            File.Delete(overwriteld2);
                            File.Move(ld_stage1_opt2, overwriteld2);
                        }
                        else
                        {
                            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }




                }
                else
                {

                }




            }
        }

        private void overwriteCB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void beamfilepathtxt_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bool success;
            //float threshold = 0;
            success = float.TryParse(textBox5.Text, out float result);
            if (success == true)
            {
                displacement = result;
                //threshold= threshold / time;
                ini.IniWriteValue("Section", "Displacement", displacement.ToString(), filenameini);
                
            }
            else
            {

            }
        }

        private void DosageCB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void OriginalPB_MouseLeave(object sender, EventArgs e)
        {
            orgmeenterflag = false;
        }

        private void PatternPB_MouseEnter(object sender, EventArgs e)
        {
            patmeenterflag = true;
        }

        private void PatternPB_MouseLeave(object sender, EventArgs e)
        {
            patmeenterflag = false;
        }

        private void Txtmousex_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void ScanningCB_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Txttime_TextChanged(object sender, EventArgs e)
        {
            bool success;
            //time = 0;
            success = float.TryParse(Txttime.Text, out float result);

            if (success == true)
            {
                time = result;
                //time = (float)(time * Math.Pow(10, -6));
                ini.IniWriteValue("Section", "Time", time.ToString(), filenameini);
                time = (float)(time * Math.Pow(10, -6));
            }
            else
            {

            }
        }

        private void RunBtn_Click(object sender, EventArgs e)
        {
            if (guimainflag == false)
            {
                MessageBox.Show("請先按下Original按鈕選擇原始Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            guimainCode mainfunction = new guimainCode();
            optimization_1stage opcode;
            optimization_2stage op2code;
            optimization_3stage op3code;
            //guimainCode dosage = new guimainCode();
            string inputpath;
            string outputpath;
            string inputpath1;
            string inputpath2;
            bool success;
            bool success1;
            bool success2;
            bool success3;
            bool first = true;
            //float threshold = 0;
            //float time = 0;
            int maxcc = 0;
            int totalnum = 0;
            if (!twolevelRB.Checked && !fourlevelRB.Checked)
            {
                MessageBox.Show("請勾選Two level或者Four level", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ROIcheckBox.Checked && RectoptCB.Checked)
            {
                MessageBox.Show("ROI及OptRect只能選一個勾選", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ldconfig1fail == true)
            {
                MessageBox.Show("請選擇正確的ldconfig1檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (ldconfig2fail == true)
            {
                MessageBox.Show("請選擇正確的ldconfig2檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (beamfilefail == true)
            {
                MessageBox.Show("請確定beamfile所選資料夾下是否存在12個光點h5檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (outputfolderfail == true)
            {
                MessageBox.Show("請確定outputfolder選擇的資料夾是否正確", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (tempfolderfail == true)
            {
                MessageBox.Show("請確定tempfolder選擇的資料夾是否正確", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //if (patimg != null)
            //{
            //    //patimg = new imgclass();
            //    //patimg.img = null;

            //    //patimg = null;

            //    //comboBox1.Text = "";
            //    //GC.Collect();
            //    //MessageBox.Show("將下拉式圖片清空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    //PatternPB.Invalidate();
            //    //return;

            //    //PatternPB.Invalidate();


            //}
            //if (par.winrow <= 0)
            //{
            //    MessageBox.Show("OrgWinRow輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
            //if (par.wincol <= 0)
            //{
            //    MessageBox.Show("OrgWinCol輸入大小必須大於0", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}


            //ldfile1.readldconfig(ldfilename1);
            ldfile1.lightspotrange();
            //ldfile2.readldconfig(ldfilename2);
            ldfile2.lightspotrange();

            if (ScanningCB.Checked)
            {
                if (twolevelRB.Checked && !fourlevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    mainfunction.scanning2level(ldfile1, ldfile2, tiffimg, outputfolderpath, tempfolderpath);
                    ldimgpath = ldimgname_2levelpath;
                    ldfilepath1 = ldfilepath1_2level;
                    ldfilepath2 = ldfilepath2_2level;
                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    mainfunction.scanning4level(ldfile1, ldfile2, tiffimg, outputfolderpath, tempfolderpath);
                    ldimgpath = ldimgname_4levelpath;
                    ldfilepath1 = ldfilepath1_4level;
                    ldfilepath2 = ldfilepath2_4level;

                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && twolevelRB.Checked && !fourlevelRB.Checked)
                {
                    if (RectoptCB.Checked)
                    {
                        if (mouseright == false)
                        {
                            MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (TxtlefttopyROI.Text == "" && TxtlefttopxROI.Text == "")
                    {
                        MessageBox.Show("請圈出ROI大小區域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int roirowoffset = Convert.ToInt32(TxtlefttopyROI.Text);
                    int roicoloffset = Convert.ToInt32(TxtlefttopxROI.Text);
                    int roirownum = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roicolnum = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);


                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";



                    mainfunction.org_roi(roirowoffset, roicoloffset, roirownum, roicolnum, Originalpath, beamfile, tiffimg.imgps, roi_path);
                    mainfunction.scanning2levelroi(roirowoffset, roicoloffset, roirownum, roicolnum, ldfile1, ldfile2, tiffimg, outputfolderpath, beamfile, tempfolderpath, roimousexy);
                    ldfilepath_2level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_2level_roi" + roimousexy + ".LD";
                    ldfilepath_2level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_2level_roi" + roimousexy + ".LD";
                    ldfilepath1 = ldfilepath_2level_roi1;
                    ldfilepath2 = ldfilepath_2level_roi2;
                    //roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
                    roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level" + roimousexy + ".h5";
                    ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi" + roimousexy + ".h5";
                    ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi" + roimousexy + ".h5";
                    dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5";
                    dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5";
                    Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";
                    Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
                    xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level" + roimousexy + ".h5";
                    xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level" + roimousexy + ".h5";
                    Txtltx.Text = TxtlefttopxROI.Text;
                    Txtlty.Text = TxtlefttopyROI.Text;
                    Txtrbx.Text = TxtrightbottomxROI.Text;
                    Txtrby.Text = TxtrightbottomyROI.Text;


                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && !twolevelRB.Checked && fourlevelRB.Checked)
                {
                    //int roirowoffset = Convert.ToInt16(TxtlefttopyROI.Text);
                    //int roicoloffset = Convert.ToInt16(TxtlefttopxROI.Text);
                    //int roirownum = Convert.ToInt16(TxtrightbottomyROI.Text) - Convert.ToUInt16(TxtlefttopyROI.Text);
                    //int roicolnum = Convert.ToInt16(TxtrightbottomxROI.Text) - Convert.ToUInt16(TxtlefttopxROI.Text);
                    if (RectoptCB.Checked)
                    {
                        if (mouseright == false)
                        {
                            MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (TxtlefttopyROI.Text == "" && TxtlefttopxROI.Text == "")
                    {
                        MessageBox.Show("請圈出ROI大小區域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    int roirowoffset = Convert.ToInt32(TxtlefttopyROI.Text);
                    int roicoloffset = Convert.ToInt32(TxtlefttopxROI.Text);
                    int roirownum = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roicolnum = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level" + roimousexy + ".h5";
                    mainfunction.org_roi(roirowoffset, roicoloffset, roirownum, roicolnum, Originalpath, beamfile, tiffimg.imgps, roi_4level_path);
                    mainfunction.scanning4levelroi(roirowoffset, roicoloffset, roirownum, roicolnum, ldfile1, ldfile2, tiffimg, outputfolderpath, beamfile, tempfolderpath, roimousexy);
                    ldfilepath_4level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_4level_roi" + roimousexy + ".LD";
                    ldfilepath_4level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_4level_roi" + roimousexy + ".LD";
                    ldfilepath1 = ldfilepath_4level_roi1;
                    ldfilepath2 = ldfilepath_4level_roi2;

                    roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
                    //roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level"+ roimousexy + ".h5";
                    ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi" + roimousexy + ".h5";
                    ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi" + roimousexy + ".h5";
                    dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5";
                    dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5";
                    Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";
                    Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
                    xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level" + roimousexy + ".h5";
                    xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level" + roimousexy + ".h5";
                    Txtltx.Text = TxtlefttopxROI.Text;
                    Txtlty.Text = TxtlefttopyROI.Text;
                    Txtrbx.Text = TxtrightbottomxROI.Text;
                    Txtrby.Text = TxtrightbottomyROI.Text;
                }

                ini.IniWriteValue("Section", "ldfilepath1", ldfilepath1.ToString(), filenameini);
                ini.IniWriteValue("Section", "ldfilepath2", ldfilepath2.ToString(), filenameini);
                TxtLDFile1.Text = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                TxtLDFile2.Text = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //if(ScanningCB.Checked&&!DosageCB.Checked&&!ThresholdCB.Checked&&!XORCB.Checked&& !OptimizationCB.Checked&&!ROIcheckBox.Checked)
                //{
                //    MessageBox.Show("Scanning程式執行結束","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                //}
                //else if(ScanningCB.Checked && !DosageCB.Checked && !ThresholdCB.Checked && !XORCB.Checked && !OptimizationCB.Checked && ROIcheckBox.Checked)
                //{
                //    MessageBox.Show("Scanning_ROI程式執行結束", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}


            }
            if (DosageCB.Checked)
            {
                if (twolevelRB.Checked && !fourlevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    if (File.Exists(ldfilepath1_2level) && File.Exists(ldfilepath2_2level))
                    {
                        mainfunction.dosage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1_2level, ldfilepath2_2level);
                        ldfilepath1 = ldfilepath1_2level;
                        ldfilepath2 = ldfilepath2_2level;
                    }
                    else
                    {
                        MessageBox.Show("輸出路徑下不存在LD檔,請先執行Two level的Scanning程式產生LD檔", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    if (File.Exists(ldfilepath1_4level) && File.Exists(ldfilepath2_4level))
                    {
                        mainfunction.dosage4level(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1_4level, ldfilepath2_4level);
                        ldfilepath1 = ldfilepath1_4level;
                        ldfilepath2 = ldfilepath2_4level;
                    }
                    else
                    {
                        MessageBox.Show("輸出路徑下不存在LD檔,請先執行Four levle的Scanning程式產生LD檔", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && twolevelRB.Checked && !fourlevelRB.Checked)
                {

                    //if (!ScanningCB.Checked)
                    //{
                    //    MessageBox.Show("請按下Scanning的CheckBox", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    ldfilepath_2level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_2level_roi" + roimousexy + ".LD";
                    ldfilepath_2level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_2level_roi" + roimousexy + ".LD";
                    if (File.Exists(ldfilepath_2level_roi1) && File.Exists(ldfilepath_2level_roi2))
                    {
                        if (RectoptCB.Checked)
                        {
                            if (mouseright == false)
                            {
                                MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        if (TxtlefttopyROI.Text == "" && TxtlefttopxROI.Text == "")
                        {
                            MessageBox.Show("請圈出ROI大小區域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        int roirowoffset = Convert.ToInt32(TxtlefttopyROI.Text);
                        int roicoloffset = Convert.ToInt32(TxtlefttopxROI.Text);
                        int roirownum = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                        int roicolnum = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);



                        mainfunction.dosageroi(tiffimg, ldfile1, ldfile2, roirowoffset, roicoloffset, roirownum, roicolnum, tempfolderpath, beamfile, ldfilepath_2level_roi1, ldfilepath_2level_roi2, roimousexy);

                        //dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5";

                        ldfilepath1 = ldfilepath_2level_roi1;
                        ldfilepath2 = ldfilepath_2level_roi2;
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;
                    }
                    else
                    {
                        MessageBox.Show("暫存檔路徑下不存在LD檔,無法執行Dosage程式,請先執行Two level的Scanning_ROI程式", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                else if ((ROIcheckBox.Checked || RectoptCB.Checked) && !twolevelRB.Checked && fourlevelRB.Checked)
                {
                    //if (!ScanningCB.Checked)
                    //{
                    //    MessageBox.Show("請按下Scanning的CheckBox", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    ldfilepath_4level_roi1 = tempfolderpath + tiffimg.imgfilename + "_LE1_4level_roi" + roimousexy + ".LD";
                    ldfilepath_4level_roi2 = tempfolderpath + tiffimg.imgfilename + "_LE2_4level_roi" + roimousexy + ".LD";
                    if (File.Exists(ldfilepath_4level_roi1) && File.Exists(ldfilepath_4level_roi2))
                    {
                        if (RectoptCB.Checked)
                        {
                            if (mouseright == false)
                            {
                                MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        if (TxtlefttopyROI.Text == "" && TxtlefttopxROI.Text == "")
                        {
                            MessageBox.Show("請圈出ROI大小區域", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        int roirowoffset = Convert.ToInt32(TxtlefttopyROI.Text);
                        int roicoloffset = Convert.ToInt32(TxtlefttopxROI.Text);
                        int roirownum = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                        int roicolnum = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                        //roimousexy = "_"+TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                        mainfunction.dosage4levelroi(tiffimg, ldfile1, ldfile2, roirowoffset, roicoloffset, roirownum, roicolnum, tempfolderpath, beamfile, ldfilepath_4level_roi1, ldfilepath_4level_roi2, roimousexy);
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;

                        //dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5";
                        ldfilepath1 = ldfilepath_4level_roi1;
                        ldfilepath2 = ldfilepath_4level_roi2;
                    }
                    else
                    {
                        MessageBox.Show("暫存檔路徑下不存在LD檔,無法執行Dosage程式,請先執行Four level的Scanning_ROI程式", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }


                }
                ini.IniWriteValue("Section", "ldfilepath1", ldfilepath1.ToString(), filenameini);
                ini.IniWriteValue("Section", "ldfilepath2", ldfilepath2.ToString(), filenameini);
                TxtLDFile1.Text = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                TxtLDFile2.Text = ini.IniReadValue("Section", "ldfilepath2", filenameini);


            }
            if (ThresholdCB.Checked)
            {
                if (twolevelRB.Checked && !fourlevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    inputpath = tempfolderpath + filename + "_dosageimg_2level.h5";
                    outputpath = tempfolderpath + filename + "_threshold_2level.h5";
                    if (!File.Exists(inputpath))
                    {
                        MessageBox.Show("不存在dosage_2level檔案，Thrshold程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    success = float.TryParse(TxtThreshold.Text, out float result);
                    success1 = float.TryParse(Txttime.Text, out float result1);
                    if (success == true && success1 == true)
                    {
                        //threshold = result;
                        //time = result1;
                        mainfunction.Threshold(inputpath, outputpath, threshold, time);
                    }
                    else
                    {

                    }


                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    inputpath = tempfolderpath + filename + "_dosageimg_4level.h5";
                    outputpath = tempfolderpath + filename + "_threshold_4level.h5";
                    if (!File.Exists(inputpath))
                    {
                        MessageBox.Show("不存在dosage_4level檔案，Thrshold程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    success = float.TryParse(TxtThreshold.Text, out float result);
                    success1 = float.TryParse(Txttime.Text, out float result1);
                    if (success == true)
                    {
                        //threshold = result;
                        //time = result1;
                        mainfunction.Threshold(inputpath, outputpath, threshold, time);
                    }
                    else
                    {

                    }
                }
                else if (twolevelRB.Checked && !fourlevelRB.Checked && (ROIcheckBox.Checked || RectoptCB.Checked))
                {
                    //if (ScanningCB.Checked && DosageCB.Checked)
                    //{

                    //}
                    //else
                    //{
                    //    MessageBox.Show("請先執行Scanning及Dosage程式,Threshold執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;

                    Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";

                    inputpath = dosage_2levelROI;
                    outputpath = Threshold_2levelROI;
                    if (!File.Exists(inputpath))
                    {
                        MessageBox.Show("不存在dosage_2levelROI檔案，Thrshold程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    success = float.TryParse(TxtThreshold.Text, out float result);
                    success1 = float.TryParse(Txttime.Text, out float result1);
                    if (success == true && success1 == true)
                    {
                        //threshold = result;
                        //time = result1;
                        mainfunction.Threshold_ROI(inputpath, outputpath, threshold, time);
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;
                    }
                    else
                    {

                    }
                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && (ROIcheckBox.Checked || RectoptCB.Checked))
                {
                    //if (ScanningCB.Checked && DosageCB.Checked)
                    //{

                    //}
                    //else
                    //{
                    //    MessageBox.Show("請先執行Scanning及Dosage程式,Threshold執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}
                    inputpath = dosage_4levelROI;
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    //Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
                    outputpath = Threshold_4levelROI;
                    if (!File.Exists(inputpath))
                    {
                        MessageBox.Show("不存在dosage_4levelROI檔案，Thrshold程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    success = float.TryParse(TxtThreshold.Text, out float result);
                    success1 = float.TryParse(Txttime.Text, out float result1);
                    if (success == true)
                    {
                        //threshold = result;
                        //time = result1;
                        mainfunction.Threshold_ROI(inputpath, outputpath, threshold, time);
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;
                    }
                    else
                    {

                    }
                }


            }
            if (XORCB.Checked)
            {

                if (twolevelRB.Checked && !fourlevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {


                    inputpath1 = Originalpath;
                    inputpath2 = Threshold_2levelpath;
                    if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                    {
                        MessageBox.Show("不存在原始Pattern及Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!File.Exists(inputpath1))
                    {
                        MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!File.Exists(inputpath2))
                    {
                        MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    outputpath = xor_path;
                    mainfunction.xor_new(inputpath1, inputpath2, outputpath);
                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && !ROIcheckBox.Checked && !RectoptCB.Checked)
                {
                    inputpath1 = Originalpath;
                    inputpath2 = Threshold_4levelpath;
                    if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                    {
                        MessageBox.Show("不存在原始Pattern及Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!File.Exists(inputpath1))
                    {
                        MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else if (!File.Exists(inputpath2))
                    {
                        MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    outputpath = xor_4level_path;
                    mainfunction.xor_new(inputpath1, inputpath2, outputpath);
                }
                else if (twolevelRB.Checked && !fourlevelRB.Checked && (ROIcheckBox.Checked || RectoptCB.Checked))
                {

                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;

                    //roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
                    //roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level" + roimousexy + ".h5";
                    roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
                    Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";
                    xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level" + roimousexy + ".h5";

                    //if (ScanningCB.Checked && DosageCB.Checked && ThresholdCB.Checked)
                    {
                        inputpath1 = roi_path;
                        inputpath2 = Threshold_2levelROI;
                        if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                        {
                            MessageBox.Show("不存在原始Pattern_ROI及Threshold_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (!File.Exists(inputpath1))
                        {
                            MessageBox.Show("不存在原始Pattern_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (!File.Exists(inputpath2))
                        {
                            MessageBox.Show("不存在Threshold_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename  + "_xor_roi_2level"+ roimousexy+".h5";
                        outputpath = xor_roi_2level_path;
                        mainfunction.xor_ROI_new(inputpath1, inputpath2, outputpath);
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;

                    }
                    //else
                    //{
                    //    MessageBox.Show("請先執行Scanning、Dosage程式及Threshold程式,Threshold執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}


                }
                else if (fourlevelRB.Checked && !twolevelRB.Checked && (ROIcheckBox.Checked || RectoptCB.Checked))
                {
                    roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    //if (ScanningCB.Checked && DosageCB.Checked && ThresholdCB.Checked)
                    roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level" + roimousexy + ".h5";
                    Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
                    xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level" + roimousexy + ".h5";
                    {

                        inputpath1 = roi_4level_path;
                        inputpath2 = Threshold_4levelROI;
                        if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                        {
                            MessageBox.Show("不存在原始Pattern_ROI及Threshold_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (!File.Exists(inputpath1))
                        {
                            MessageBox.Show("不存在原始Pattern_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else if (!File.Exists(inputpath2))
                        {
                            MessageBox.Show("不存在Threshold_ROI檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //roimousexy = "_"+TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                        //xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename  + "_xor_roi_4level"+ roimousexy+".h5";
                        outputpath = xor_roi_4level_path;
                        mainfunction.xor_ROI_new(inputpath1, inputpath2, outputpath);
                        Txtltx.Text = TxtlefttopxROI.Text;
                        Txtlty.Text = TxtlefttopyROI.Text;
                        Txtrbx.Text = TxtrightbottomxROI.Text;
                        Txtrby.Text = TxtrightbottomyROI.Text;
                    }
                    //else
                    //{
                    //    MessageBox.Show("請先執行Scanning、Dosage程式及Threshold程式,Threshold執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    return;
                    //}

                }




            }
            if (OptimizationCB.Checked)
            {
                DateTime time_start = DateTime.Now;//計時開始 取得目前時間
                string newtarget1, newtarget2;
                string newtarget_roi1 = ini.IniReadValue("Section", "ldfilepath1", filenameini) , newtarget_roi2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                if (!CB1.Checked && !CB2.Checked && !CB3.Checked)
                {
                    MessageBox.Show("執行最佳化程式時需要選擇1階、2階或3階", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB1.Checked)
                {
                   
                    
                    ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                    ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                   
                    int findroi;
                   
                    findroi = ldfilepath1.IndexOf("roi");
                    int findtiffimage;
                    findtiffimage = ldfilepath1.IndexOf(tiffimg.imgfilename);
                    if (!ScanningCB.Checked)
                    {
                        roimousexy = "";
                        if (findroi != -1)
                        {
                            MessageBox.Show("請使用整張圖的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if(findtiffimage==-1)
                        {
                            MessageBox.Show("請使用正確的的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    else
                    {
                        roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                        if(findroi==-1)
                        {
                            MessageBox.Show("請使用ROI的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (first == true)
                    {
                        overwriteld1 = ldfilepath1;
                        overwriteld2 = ldfilepath2;
                        first = false;
                    }


                    opt1start = true;
                   
                    if (twolevelRB.Checked && !fourlevelRB.Checked)
                    {
                       
                        if (TxtCN.Text != "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success1 = int.TryParse(TxtTN.Text, out int result1);
                            if (success1 == true)
                            {
                                totalnum = result1;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            //ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                            //ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            int find2level;
                            find2level = ldfilepath1.IndexOf("2level");
                            if (find2level == -1)
                            {
                                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }

                            opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold,displacement, maxcc, totalnum);
                            //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);

                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);

                            optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text != "" && TxtTN.Text == "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            int find2level;
                            find2level = ldfilepath1.IndexOf("2level");
                            if (find2level == -1)
                            {
                                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold , maxcc, totalnum: int.MaxValue);
                            //    optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}

                            {

                                opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: int.MaxValue);
                                optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);

                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text == "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtTN.Text, out int result);
                            if (success == true)
                            {
                                totalnum = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            maxcc = int.MaxValue;
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                           
                            int find2level;
                            find2level = ldfilepath1.IndexOf("2level");
                            if (find2level == -1)
                            {
                                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold , maxcc, totalnum: totalnum);
                            //    optst1result = opcode.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: totalnum);
                                optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);

                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", "", filenameini);
                            //ini.IniWriteValue("Section", "OptCN", "", filenameini);

                        }
                        newtarget_roi1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                        newtarget_roi2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                    }


                }
                if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB2.Checked)
                {

                    ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                    ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                    
                    int findroi;
                    findroi = ldfilepath1.IndexOf("roi");
                    int findtiffimage;
                    findtiffimage = ldfilepath1.IndexOf(tiffimg.imgfilename);
                    if (!ScanningCB.Checked)
                    {
                        roimousexy = "";
                        if (findroi != -1)
                        {
                            MessageBox.Show("請使用整張圖的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (findtiffimage == -1)
                        {
                            MessageBox.Show("請使用正確的的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //Show("請執行Scanning_ROI程式產生LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                    else
                    {
                        roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                        if (findroi == -1)
                        {
                            MessageBox.Show("請使用ROI的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    int find2level;
                    find2level = ldfilepath1.IndexOf("2level");
                    if (find2level == -1)
                    {
                        MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    opt2start = true;
                    if (first == true)
                    {
                        overwriteld1 = ldfilepath1;
                        overwriteld2 = ldfilepath2;
                        first = false;
                    }
                    if (twolevelRB.Checked ||fourlevelRB.Checked)
                    {
                        
                        if (newtarget_roi1 != ldfilepath1)
                        {
                            ldfilepath1 = newtarget_roi1;
                            ldfilepath2 = newtarget_roi2;
                        }
                        else
                        {
                            ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                            ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                        }

                        if (TxtCN.Text != "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success1 = int.TryParse(TxtTN.Text, out int result1);
                            if (success1 == true)
                            {
                                totalnum = result1;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                           

                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold , maxcc, totalnum);
                            //    //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                            //    optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum);
                                //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                                optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text != "" && TxtTN.Text == "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            //int find2level;
                            //find2level = ldfilepath1.IndexOf("2level");
                            //if (find2level == -1)
                            //{
                            //    MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    return;
                            //}
                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy,threshold, maxcc, totalnum: int.MaxValue);
                            //    optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: int.MaxValue);
                                optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text == "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtTN.Text, out int result);
                            if (success == true)
                            {
                                totalnum = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            maxcc = int.MaxValue;
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            //int find2level;
                            //find2level = ldfilepath1.IndexOf("2level");
                            //if (find2level == -1)
                            //{
                            //    MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //    return;
                            //}
                            //if (findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                            //    optst2result = op2code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: totalnum);
                                optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }
                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", "", filenameini);
                            //ini.IniWriteValue("Section", "OptCN", "", filenameini);


                        }

                    }
                    newtarget_roi1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_2stage_4level_opt.LD";
                    newtarget_roi2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_2stage_4level_opt.LD";
                }
                if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB3.Checked)
                {

                    ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                    ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                   
                    int findroi;
                    findroi = ldfilepath1.IndexOf("roi");
                    int findtiffimage;
                    findtiffimage = ldfilepath1.IndexOf(tiffimg.imgfilename);
                    if (!ScanningCB.Checked)
                    {
                        roimousexy = "";
                        if (findroi != -1)
                        {
                            MessageBox.Show("請使用整張圖的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (findtiffimage == -1)
                        {
                            MessageBox.Show("請使用正確的的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        //MessageBox.Show("請執行Scanning_ROI程式產生LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //return;
                    }
                    else
                    {
                        roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                        if (findroi == -1)
                        {
                            MessageBox.Show("請使用ROI的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                    if (first == true)
                    {
                        overwriteld1 = ldfilepath1;
                        overwriteld2 = ldfilepath2;
                        first = false;
                    }
                    opt3start = true;
                    if ((twolevelRB.Checked || fourlevelRB.Checked) && (ROIcheckBox.Checked || RectoptCB.Checked))
                    {
                       
                        if (newtarget_roi1 != ldfilepath1)
                        {
                            ldfilepath1 = newtarget_roi1;
                            ldfilepath2 = newtarget_roi2;
                        }
                        else
                        {
                            ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                            ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                        }
                        if (TxtCN.Text != "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success1 = int.TryParse(TxtTN.Text, out int result1);
                            if (success1 == true)
                            {
                                totalnum = result1;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);


                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold , maxcc, totalnum);
                            //    //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                            //    optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum);
                                //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                                optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text != "" && TxtTN.Text == "")
                        {
                            success = int.TryParse(TxtCN.Text, out int result);
                            if (success == true)
                            {
                                maxcc = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, (float)(threshold / time * Math.Pow(10, -6)), maxcc, totalnum: int.MaxValue);
                            //    optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: int.MaxValue);
                                optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }


                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                        }
                        else if (TxtCN.Text == "" && TxtTN.Text != "")
                        {
                            success = int.TryParse(TxtTN.Text, out int result);
                            if (success == true)
                            {
                                totalnum = result;
                            }
                            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                            if (success2 == true)
                            {
                                //threshold = result2;
                            }
                            success3 = float.TryParse(Txttime.Text, out float result3);
                            if (success3 == true)
                            {
                                //time = result3;
                            }
                            if (RectoptCB.Checked)
                            {
                                if (mouseright == false)
                                {
                                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                            //int findroi;
                            //findroi = ldfilepath1.IndexOf("roi");
                            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                            maxcc = int.MaxValue;

                            //if(findroi==-1)
                            //{
                            //    roimousexy = "";
                            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                            //    optst3result = op3code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            //}
                            //else
                            {
                                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, displacement, maxcc, totalnum: totalnum);
                                optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                            }

                            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                            //ini.IniWriteValue("Section", "CN", "", filenameini);
                            //ini.IniWriteValue("Section", "OptCN", "", filenameini);


                        }

                    }
                }
                if (!(ROIcheckBox.Checked || RectoptCB.Checked) && (CB1.Checked || CB2.Checked || CB3.Checked))
                    MessageBox.Show("請使用ROI或Rectpot勾選出ROI區域","提醒",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                newtarget1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                newtarget2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //if (!ROIcheckBox.Checked && !RectoptCB.Checked && CB1.Checked)
                //{
                //    if(!twolevelRB.Checked)
                //    {
                //        MessageBox.Show("請選擇Twolevel","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                //        return;
                //    }
                //    opt1start = true;
                //    roimousexy = "";
                //    ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                //    ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //    //newtarget1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                //    //newtarget2= ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //    if (twolevelRB.Checked && !fourlevelRB.Checked)
                //    {
                //        if (TxtCN.Text != "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success1 = int.TryParse(TxtTN.Text, out int result1);
                //            if (success1 == true)
                //            {
                //                totalnum = result1;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }


                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            if (findroi != -1)
                //            {
                //                MessageBox.Show("請使用整個圖檔的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}

                //            opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum);
                //            //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);

                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);

                //            optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);

                //        }
                //        else if (TxtCN.Text != "" && TxtTN.Text == "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, (float)(threshold / time * Math.Pow(10, -6)), maxcc, totalnum: int.MaxValue);
                //            //    optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {

                //                opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, (float)(threshold / time * Math.Pow(10, -6)), maxcc, totalnum: int.MaxValue);
                //                optst1result = opcode.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);

                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                //        }
                //        else if (TxtCN.Text == "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtTN.Text, out int result);
                //            if (success == true)
                //            {
                //                totalnum = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            maxcc = int.MaxValue;
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold , maxcc, totalnum: totalnum);
                //            //    optst1result = opcode.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                opcode = new optimization_1stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                //                optst1result = opcode.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);

                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", "", filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", "", filenameini);

                //        }
                //        newtarget1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                //        newtarget2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                //    }
                //}
                //if (!ROIcheckBox.Checked && !RectoptCB.Checked && CB2.Checked)
                //{
                //    opt2start = true;
                //    roimousexy = "";
                //    if (twolevelRB.Checked && !fourlevelRB.Checked)
                //    {
                //        if (newtarget1 != ldfilepath1)
                //        {
                //            ldfilepath1 = newtarget1;
                //            ldfilepath2 = newtarget2;
                //        }
                //        else
                //        {
                //            ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                //            ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //        }
                //        if (TxtCN.Text != "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success1 = int.TryParse(TxtTN.Text, out int result1);
                //            if (success1 == true)
                //            {
                //                totalnum = result1;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            if (findroi != -1)
                //            {
                //                MessageBox.Show("請使用整個圖檔的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }

                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum);
                //            //    //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                //            //    optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum);
                //                //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                //                optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                //        }
                //        else if (TxtCN.Text != "" && TxtTN.Text == "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: int.MaxValue);
                //            //    optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: int.MaxValue);
                //                optst2result = op2code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                //        }
                //        else if (TxtCN.Text == "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtTN.Text, out int result);
                //            if (success == true)
                //            {
                //                totalnum = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            maxcc = int.MaxValue;
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            int find2level;
                //            find2level = ldfilepath1.IndexOf("2level");
                //            if (find2level == -1)
                //            {
                //                MessageBox.Show("請使用2level的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                //            //    optst2result = op2code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op2code = new optimization_2stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                //                optst2result = op2code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }
                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", "", filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", "", filenameini);


                //        }
                //        newtarget1 = outputfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                //        newtarget2 = outputfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                //    }
                //}
                //if (!ROIcheckBox.Checked && !RectoptCB.Checked && CB3.Checked)
                //{
                //    roimousexy = "";
                //    opt3start = true;
                //    if (twolevelRB.Checked || fourlevelRB.Checked)
                //    {
                //        if (newtarget1 != ldfilepath1)
                //        {
                //            ldfilepath1 = newtarget1;
                //            ldfilepath2 = newtarget2;
                //        }
                //        else
                //        {
                //            ldfilepath1 = ini.IniReadValue("Section", "ldfilepath1", filenameini);
                //            ldfilepath2 = ini.IniReadValue("Section", "ldfilepath2", filenameini);
                //        }
                //        if (TxtCN.Text != "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success1 = int.TryParse(TxtTN.Text, out int result1);
                //            if (success1 == true)
                //            {
                //                totalnum = result1;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            if (findroi != -1)
                //            {
                //                MessageBox.Show("請使用整個圖檔的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }

                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum);
                //            //    //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                //            //    optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum);
                //                //opcode.optimize_total(ROIlefttop.X, ROIlefttop.Y, ROIrightbottom.X, ROIrightbottom.Y);
                //                optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                //        }
                //        else if (TxtCN.Text != "" && TxtTN.Text == "")
                //        {
                //            success = int.TryParse(TxtCN.Text, out int result);
                //            if (success == true)
                //            {
                //                maxcc = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            if (RectoptCB.Checked)
                //            {
                //                if (mouseright == false)
                //                {
                //                    MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                    return;
                //                }
                //            }
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            if (findroi != -1)
                //            {
                //                MessageBox.Show("請使用整個圖檔的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, (float)(threshold / time * Math.Pow(10, -6)), maxcc, totalnum: int.MaxValue);
                //            //    optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, (float)(threshold / time * Math.Pow(10, -6)), maxcc, totalnum: int.MaxValue);
                //                optst3result = op3code.optimize_maxcc(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }


                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", Convert.ToString(optresult[2]), filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", Convert.ToString(optresult[3]), filenameini);
                //        }
                //        else if (TxtCN.Text == "" && TxtTN.Text != "")
                //        {
                //            success = int.TryParse(TxtTN.Text, out int result);
                //            if (success == true)
                //            {
                //                totalnum = result;
                //            }
                //            success2 = float.TryParse(TxtThreshold.Text, out float result2);
                //            if (success2 == true)
                //            {
                //                //threshold = result2;
                //            }
                //            success3 = float.TryParse(Txttime.Text, out float result3);
                //            if (success3 == true)
                //            {
                //                //time = result3;
                //            }
                //            //if (RectoptCB.Checked)
                //            //{
                //            //    if (mouseright == false)
                //            //    {
                //            //        MessageBox.Show("請按下右鍵固定ROI大小", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //            //        return;
                //            //    }
                //            //}
                //            int findroi;
                //            findroi = ldfilepath1.IndexOf("roi");
                //            if (findroi != -1)
                //            {
                //                MessageBox.Show("請使用整個圖檔的LD檔", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            int roixlefttop = Convert.ToInt32(TxtlefttopxROI.Text);
                //            int roiylefttop = Convert.ToInt32(TxtlefttopyROI.Text);
                //            int roixrightbottom = Convert.ToInt32(TxtrightbottomxROI.Text);
                //            int roiyrightbottom = Convert.ToInt32(TxtrightbottomyROI.Text);
                //            maxcc = int.MaxValue;
                //            int findtiffname;
                //            findtiffname = ldfilepath1.IndexOf(tiffimg.imgfilename);
                //            if (findtiffname == -1)
                //            {
                //                MessageBox.Show("請使用正確的LD檔案，請跑一次Scanning程式或是由LDFile Modification按鈕挑選正確的LD檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //                return;
                //            }
                //            //if (findroi == -1)
                //            //{
                //            //    roimousexy = "";
                //            //    op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                //            //    optst3result = op3code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            //}
                //            //else
                //            {
                //                op3code = new optimization_3stage(tiffimg, ldfile1, ldfile2, tempfolderpath, beamfile, ldfilepath1, ldfilepath2, roimousexy, threshold, maxcc, totalnum: totalnum);
                //                optst3result = op3code.optimize_total(roixlefttop, roiylefttop, roixrightbottom, roiyrightbottom);
                //            }

                //            //ini.IniWriteValue("Section", "TN", Convert.ToString(optresult[0]), filenameini);
                //            //ini.IniWriteValue("Section", "OptTN", Convert.ToString(optresult[1]), filenameini);
                //            //ini.IniWriteValue("Section", "CN", "", filenameini);
                //            //ini.IniWriteValue("Section", "OptCN", "", filenameini);


                //        }

                //    }
                //}
                System.Threading.Thread.Sleep(1000);

                DateTime time_end = DateTime.Now;//計時結束 取得目前時間

                //後面的時間減前面的時間後 轉型成TimeSpan即可印出時間差

                result2 = ((TimeSpan)(time_end - time_start)).ToString();
            }
            if (overwriteCB.Checked)
            {

                {

                    //roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
                    roimousexy = "";
                    string ld_stage1_opt1, ld_stage1_opt2, ld_stage2_opt1, ld_stage2_opt2, ld_stage4_opt1, ld_stage4_opt2;
                    //if (!ROIcheckBox.Checked && !RectoptCB.Checked && CB1.Checked)
                    //{
                    //    ld_stage1_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                    //    ld_stage1_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                    //    ld_stage2_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_2stage_2level_opt.LD";
                    //    ld_stage2_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_2stage_2level_opt.LD";
                    //    if (File.Exists(ldfilepath1_2level) && File.Exists(ldfilepath2_2level))
                    //    {
                    //        if (File.Exists(ld_stage1_opt1))
                    //        {
                    //            string ldfile_2level_roi1 = tiffimg.imgfilename + "_LE1_2level_roi" + roimousexy + ".LD";
                    //            string ldfile_2level_roi1_opt = tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                    //            File.Replace(ldfile_2level_roi1, ldfile_2level_roi1_opt, ldfile_2level_roi1_opt);
                    //            //string ldfile_2level_roi2 = tiffimg.imgfilename + "_LE2_2level_roi.LD";
                    //            //string ldfile_2level_roi2_opt = tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                    //            //File.Replace(ldfile_2level_roi2, ldfile_2level_roi2_opt, ldfile_2level_roi2_opt);
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        }
                    //        if (File.Exists(ld_stage1_opt2))
                    //        {
                    //            string ldfile_2level_roi2 = tiffimg.imgfilename + "_LE2_2level_roi.LD";
                    //            string ldfile_2level_roi2_opt = tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";
                    //            File.Replace(ldfile_2level_roi2, ldfile_2level_roi2_opt, ldfile_2level_roi2_opt);
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        }

                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show("Temp資料夾不存在roi的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //    }



                    //}
                    if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB3.Checked)
                    {
                        if (ScanningCB.Checked)
                        {
                            MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        roimousexy = "";
                        ld_stage4_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_3stage_4level_opt.LD";
                        ld_stage4_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_3stage_4level_opt.LD";
                        string add4 = "";
                        if (File.Exists(overwriteld1))
                        {
                            if (File.Exists(ld_stage4_opt1))
                            {
                                string ldfile1_4level = tiffimg.imgfilename + "_LE1_4level.LD";
                                string ldfile1_4level_opt = tiffimg.imgfilename + "_LE1_3stage_4level_opt.LD";
                                //File.Replace(ldfile1_4level, ldfile1_4level_opt, ldfile1_4level_opt);


                                int find2level;
                                find2level = overwriteld1.IndexOf("2level");
                                if (find2level == -1)
                                {
                                    File.Delete(overwriteld1);
                                    File.Move(ld_stage4_opt1, overwriteld1);
                                }
                                else
                                {
                                    string remove2 = overwriteld1.Remove(find2level, 6);
                                    add4 = remove2.Insert(find2level, "4level");
                                    if (File.Exists(add4))
                                        File.Delete(add4);

                                    File.Move(ld_stage4_opt1, add4);
                                }

                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }




                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (File.Exists(overwriteld2))
                        {
                            if (File.Exists(ld_stage4_opt2))
                            {
                                string ldfile2_4level = tiffimg.imgfilename + "_LE2_2level.LD";
                                string ldfile2_4level_opt = tiffimg.imgfilename + "_LE2_2stage_2level_opt.LD";
                                File.Delete(overwriteld2);
                                int find2level;
                                find2level = overwriteld2.IndexOf("2level");
                                if (find2level == -1)
                                {
                                    File.Move(ld_stage4_opt2, overwriteld2);
                                }
                                else
                                {
                                    string remove2 = overwriteld2.Remove(find2level, 6);
                                    add4 = remove2.Insert(find2level, "4level");
                                    if (File.Exists(add4))
                                        File.Delete(add4);
                                    File.Move(ld_stage4_opt2, add4);
                                }

                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB2.Checked)
                    {
                        if (ScanningCB.Checked)
                        {
                            MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        roimousexy = "";
                        ld_stage2_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_2stage_4level_opt.LD";
                        ld_stage2_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_2stage_4level_opt.LD";
                        string add4 = "";
                        if (File.Exists(overwriteld1))
                        {
                            if (File.Exists(ld_stage2_opt1))
                            {
                                string ldfile1_2level = tiffimg.imgfilename + "_LE1_2level.LD";
                                string ldfile1_2level_opt = tiffimg.imgfilename + "_LE1_2stage_4level_opt.LD";

                                int find2level;
                                find2level = overwriteld1.IndexOf("2level");
                                if (find2level == -1)
                                {
                                    File.Delete(overwriteld1);
                                    File.Move(ld_stage2_opt1, overwriteld1);
                                }
                                else
                                {
                                    string remove2 = overwriteld1.Remove(find2level, 6);
                                    add4 = remove2.Insert(find2level, "4level");
                                    if (File.Exists(add4))
                                        File.Delete(add4);
                                    File.Move(ld_stage2_opt1, add4);
                                }

                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }




                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (File.Exists(overwriteld2))
                        {
                            if (File.Exists(ld_stage2_opt2))
                            {
                                string ldfile2_2level = tiffimg.imgfilename + "_LE2_2level.LD";
                                string ldfile2_2level_opt = tiffimg.imgfilename + "_LE2_2stage_4level_opt.LD";
                                File.Delete(overwriteld2);
                                int find2level;
                                find2level = overwriteld2.IndexOf("2level");
                                if (find2level == -1)
                                {
                                    File.Move(ld_stage2_opt2, overwriteld2);
                                }
                                else
                                {
                                    string remove2 = overwriteld2.Remove(find2level, 6);
                                    add4 = remove2.Insert(find2level, "4level");
                                    if (File.Exists(add4))
                                        File.Delete(add4);
                                    File.Move(ld_stage2_opt2, add4);
                                }

                                //File.Replace(ldfile2_2level, ldfile2_2level_opt, ldfile2_2level_opt);
                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else if ((ROIcheckBox.Checked || RectoptCB.Checked) && CB1.Checked)
                    {
                        if (ScanningCB.Checked)
                        {
                            MessageBox.Show("Overwrite只能取代整張圖的LD", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        roimousexy = "";
                        ld_stage1_opt1 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt.LD";
                        //string ld_stage1_opt1_backup= tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt_backup.LD";
                        //string ld_stage1_opt2_backup = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE1_1stage_2level_opt_backup.LD";
                        ld_stage1_opt2 = tempfolderpath + tiffimg.imgfilename + roimousexy + "_LE2_1stage_2level_opt.LD";

                        if (File.Exists(overwriteld1))
                        {
                            if (File.Exists(ld_stage1_opt1))
                            {
                                //string ldfile1_2level = tiffimg.imgfilename + "_LE1_2level.LD";
                                //string ldfile1_2level_opt = tiffimg.imgfilename + "_LE1_1stage_2level_opt.LD";
                                //File.Replace(ldfilepath1_2level, ld_stage1_opt1, ld_stage1_opt1_backup);
                                File.Delete(overwriteld1);
                                File.Move(ld_stage1_opt1, overwriteld1);
                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }




                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        if (File.Exists(overwriteld2))
                        {
                            if (File.Exists(ld_stage1_opt2))
                            {
                                //string ldfile2_2level = tiffimg.imgfilename + "_LE2_2level.LD";
                                //string ldfile2_2level_opt = tiffimg.imgfilename + "_LE2_1stage_2level_opt.LD";
                                File.Delete(overwriteld2);
                                File.Move(ld_stage1_opt2, overwriteld2);
                            }
                            else
                            {
                                MessageBox.Show("Temp資料夾不存在最佳化後的LE1的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                        }
                        else
                        {
                            MessageBox.Show("Output資料夾不存在最佳化前的LE2的LD檔案，Overwrite程式執行失敗", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }




                    }
                    else
                    {

                    }




                }

            }


            if (twolevelRB.Checked)
            {
                if (ScanningCB.Checked || DosageCB.Checked || ThresholdCB.Checked || XORCB.Checked || OptimizationCB.Checked || overwriteCB.Checked)
                {

                }
                else
                {
                    MessageBox.Show("請勾選想要執行的程式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            if (fourlevelRB.Checked)
            {
                if (ScanningCB.Checked || DosageCB.Checked || ThresholdCB.Checked || XORCB.Checked || OptimizationCB.Checked || overwriteCB.Checked)
                {

                }
                else
                {
                    MessageBox.Show("請勾選想要執行的程式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            MessageBox.Show("Run程式執行結束", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void Txtresolution_TextChanged(object sender, EventArgs e)
        {

        }

        private void ROIshowCB_CheckedChanged(object sender, EventArgs e)
        {
            //var s1 = PatternPB.Size;
            //var s2 = OriginalPB.Size;
            if (guimainflag == false)
            {
                roicheckbool = true;
                ROIshowCB.Checked = false;
                if (roicheckbool == true)
                {
                    MessageBox.Show("請先按下Original按鈕選擇原始Pattern", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    roicheckbool = false;

                }


                return;

            }
            if (combochange == true)
            {
                combochange = false;
            }
            else
            {

                comboflag = true;

            }
            int[] size = new int[4];
            //roimousexy = "_" + TxtlefttopxROI.Text + "_" + TxtlefttopyROI.Text + "_" + TxtrightbottomxROI.Text + "_" + TxtrightbottomyROI.Text;
            //roi_path = tempfolderpath + tiffimg.imgfilename + "_roi" + roimousexy + ".h5";
            //roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_roi_4level.h5";
            //ldimg_2levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi" + roimousexy + ".h5";
            //ldimg_4levelROI = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi" + roimousexy + ".h5";
            //dosage_2levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5";
            //dosage_4levelROI = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5";
            //Threshold_2levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_2level_roi" + roimousexy + ".h5";
            //Threshold_4levelROI = tempfolderpath + tiffimg.imgfilename + "_Threshold_4level_roi" + roimousexy + ".h5";
            //xor_roi_2level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_2level" + roimousexy + ".h5";
            //xor_roi_4level_path = tempfolderpath + tiffimg.imgfilename + "_xor_roi_4level" + roimousexy + ".h5";
            if (ROIshowCB.Checked)
            {
                oldxscroll = panel1.HorizontalScroll.Value;
                oldyscroll = panel1.VerticalScroll.Value;
                //roi_path = Originalpath + "_roi.h5";
                //roi_4level_path = Originalpath + "_roi_4level.h5";
                if (comboBox1.Text == "Dosage_2level" || comboBox1.Text == "LDFile_2level" || comboBox1.Text == "Threshold_2level" || comboBox1.Text == "XOR_2level")
                {
                    orgroi_img = new imgclass(roi_path, "roi.h5", ptbh, ptbw, 1, 2);
                }
                else if (comboBox1.Text == "Dosage_4level" || comboBox1.Text == "LDFile_4level" || comboBox1.Text == "Threshold_4level" || comboBox1.Text == "XOR_4level")
                {
                    orgroi_img = new imgclass(roi_4level_path, "roi_4level.h5", ptbh, ptbw, 1, 2);
                }
                else
                {
                    //MessageBox.Show("請由下拉式選單選取一張圖片","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    //Lablevel.Text = "";
                    //patternclear = true;
                    //PatternPB.Invalidate();
                    return;
                }

                if (orgroi_img.img == null)
                {
                    OriginalPB.Image = null;
                    PatternPB.Image = null;
                    return;
                }
                OriginalPB.Height = orgroi_img.img.Height;
                OriginalPB.Width = orgroi_img.img.Width;
                OriginalPB.Image = null;
                OriginalPB.SizeMode = PictureBoxSizeMode.Zoom;
                var s3 = PatternPB.Size;
                var s4 = OriginalPB.Size;
                int Patwidth = s3.Width;
                int Patheight = s3.Height;
                int Oriwidth = s4.Width;
                int Oriheight = s4.Height;
                size = orgroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                s3.Width = size[2];
                s3.Height = size[3];
                s4.Width = size[0];
                s4.Height = size[1];
                PatternPB.Size = s3;
                OriginalPB.Size = s4;
                OriginalPB.Invalidate();
                //PatternPB.Invalidate();
                checkroiflag = true;
                checkroiflag2 = true;
                colorbarGB.Visible = false;
                XORGB.Visible = false;
            }
            else
            {

            }




            if (comboBox1.Text == "Dosage_2level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(dosage_2levelROI, "Dosage_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = true;
                    XORGB.Visible = false;

                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(dosagename_2levelpath, "Dosage_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;


                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = true;
                        XORGB.Visible = false;
                    }

                }






            }
            else if (comboBox1.Text == "LDFile_2level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(ldimg_2levelROI, "LDFile_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(ldimgname_2levelpath, "LDFile_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    patimg.ratio = orgimg.ratio;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }

                }


            }
            else if (comboBox1.Text == "LDFile_4level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(ldimg_4levelROI, "LDFile_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(ldimgname_4levelpath, "LDFile_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    patimg.ratio = orgimg.ratio;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }
                }


            }
            else if (comboBox1.Text == "Dosage_4level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(dosage_4levelROI, "Dosage_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    roiflag = true;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = true;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(dosagename_4levelpath, "Dosage_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = true;
                        XORGB.Visible = false;
                    }
                }





            }
            else if (comboBox1.Text == "Threshold_2level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {

                    patroi_img = new imgclass(Threshold_2levelROI, "Threshold_2level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(Threshold_2levelpath, "Threshold_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }
                }





            }
            else if (comboBox1.Text == "Threshold_4level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(Threshold_4levelROI, "Threshold_4level_ROI", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = false;
                }
                else
                {
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    patimg = new imgclass(Threshold_4levelpath, "Threshold_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = false;
                    }

                }

            }
            else if (comboBox1.Text == "XOR_2level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(xor_roi_2level_path, "XOR_ROI_2level", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = true;
                }
                else
                {
                    patimg = new imgclass(xor_path, "xor", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    PatternPB.Invalidate();
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = true;
                    }

                }


            }
            else if (comboBox1.Text == "XOR_4level")
            {
                patternclear = false;
                if (ROIshowCB.Checked)
                {
                    patroi_img = new imgclass(xor_roi_4level_path, "XOR_ROI_4level", ptbh, ptbw, 1, 2);
                    if (patroi_img.img == null)
                    {
                        checkroiflag2 = false;
                        PatternPB.Image = null;
                        return;
                    }
                    PatternPB.Height = patroi_img.img.Height;
                    PatternPB.Width = patroi_img.img.Width;
                    OriginalPB.Height = orgroi_img.img.Height;
                    OriginalPB.Width = orgroi_img.img.Width;
                    PatternPB.Image = null;
                    PatternPB.SizeMode = PictureBoxSizeMode.Zoom;
                    roiflag = true;
                    checkroiflag = true;
                    checkroiflag2 = true;
                    var s3 = PatternPB.Size;
                    var s4 = OriginalPB.Size;
                    int Patwidth = s3.Width;
                    int Patheight = s3.Height;
                    int Oriwidth = s4.Width;
                    int Oriheight = s4.Height;
                    size = patroi_img.roi_updateimg(Patwidth, Patheight, Oriwidth, Oriheight, rationew);
                    s3.Width = size[2];
                    s3.Height = size[3];
                    s4.Width = size[0];
                    s4.Height = size[1];
                    PatternPB.Size = s3;
                    OriginalPB.Size = s4;
                    PatternPB.Invalidate();
                    OriginalPB.Invalidate();
                    colorbarGB.Visible = false;
                    XORGB.Visible = true;
                }
                else
                {
                    patimg = new imgclass(xor_4level_path, "xor_4level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom);
                    float orgres = float.Parse(Txtresolution.Text);
                    patimg.ratio = dosageres / orgres;
                    PatternPB.Invalidate();
                    OriginalPB.Width = ptbmax;
                    OriginalPB.Height = ptbmax;
                    PatternPB.Width = ptbmax;
                    PatternPB.Height = ptbmax;
                    //if (patimg.img != null)
                    {
                        PatternPB.Invalidate();
                        colorbarGB.Visible = false;
                        XORGB.Visible = true;
                    }
                }
            }
            else if (comboBox1.Text == "")
            {
                Lablevel.Text = "";
                patternclear = true;
                PatternPB.Invalidate();
            }
            //OriginalPB.Refresh();
            OriginalPB.Invalidate();

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void OriginalPB_PaddingChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {


        }

        private void PatternPB_MouseMove(object sender, MouseEventArgs e)
        {
            if (ROIshowCB.Checked)
            {
                //Txtmousex.Text = Convert.ToString(mousex);
                // Txtmousey.Text = Convert.ToString(mousey);
                //int lefttopx = Convert.ToInt32(TxtlefttopxROI.Text);
                //int lefttopy = Convert.ToInt32(TxtlefttopyROI.Text);
                //if(TxtlefttopxROI.Text!=""&& TxtlefttopyROI.Text!="")
                //{
                //    int lefttopx = Convert.ToInt32(TxtlefttopxROI.Text);
                //    int lefttopy = Convert.ToInt32(TxtlefttopyROI.Text);
                //}
                if (Txtltx.Text != "" && Txtlty.Text != "")
                {
                    ulong mousextopleft = (ulong)e.X & 0xffff;//40000
                    ulong mouseytopleft = (ulong)e.Y & 0xffff;//40000
                    int mousexint = (int)mousextopleft;
                    int mouseyint = (int)mouseytopleft;
                    int lefttopx = Convert.ToInt32(Txtltx.Text);
                    int lefttopy = Convert.ToInt32(Txtlty.Text);
                    double mouseratiox = Convert.ToDouble(Txtratio.Text);
                    mouseratiox = mouseratiox / 100;
                    mousexint = (int)(mousexint / mouseratiox);
                    mouseyint = (int)(mouseyint / mouseratiox);
                    lefttopx = lefttopx + mousexint;
                    lefttopy = lefttopy + mouseyint;



                    Txtmousex.Text = Convert.ToString(lefttopx);
                    Txtmousey.Text = Convert.ToString(lefttopy);
                }

            }

            if (!RectoptCB.Checked)
            {
                //if(guimainflag == false)
                //{
                //    return;
                //}

                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;

                mousex = e.X;
                mousey = e.Y;
                mousexvalue = (ulong)e.X & 0xffff;//40000
                mouseyvalue = (ulong)e.Y & 0xffff;//40000
                mouseratiox = (ulong)e.X & 0xffff;//40000
                mouseratioy = (ulong)e.Y & 0xffff;//40000
                mouseroix = (ulong)e.X & 0xffff;//40000
                mouseroiy = (ulong)e.Y & 0xffff;//40000


                mousex = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);//40082
                mousey = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);//40082
                mousevaluex = mousex;
                mousevaluey = mousey;
                if (orgimg != null)
                {

                    //img.callmatrixvalue();
                    if ((comboBox1.Text == "Dosage_2level" || comboBox1.Text == "Dosage_4level"))
                    {
                        float value;
                        if (!ROIshowCB.Checked)
                        {
                            if (patimg.img != null)
                            {

                                mousevaluex = mousevaluex - patimg.realposlefttop.X;
                                mousevaluey = mousevaluey - patimg.realposlefttop.Y;
                                float x;
                                float y;

                                if (mousevaluex < 0)
                                    mousevaluex = 0;
                                if (mousevaluey < 0)
                                    mousevaluey = 0;
                                if (mousevaluex > patimg.array.GetLength(1) - 1)
                                    mousevaluex = patimg.array.GetLength(1) - 1;
                                if (mousevaluey > patimg.array.GetLength(0) - 1)
                                    mousevaluey = patimg.array.GetLength(0) - 1;
                                if (patimg.array != null)
                                {
                                    value = patimg.array[mousevaluey, mousevaluex];
                                    value = (float)(value * (time));
                                    Txtxvalue.Text = Convert.ToString(value);

                                }
                            }

                        }
                        else
                        {
                            if (patroi_img != null)
                            {
                                double mouseratiox = Convert.ToDouble(Txtratio.Text);
                                mouseratiox = mouseratiox / 100;
                                mousevaluex = (int)mouseroix;//40000
                                mousevaluey = (int)mouseroiy;//40000


                                if (patroi_img.roiarray != null)
                                {
                                    mousevaluex = (int)(mousevaluex / mouseratiox);
                                    mousevaluey = (int)(mousevaluey / mouseratiox);
                                    if (mousevaluex < 0)
                                        mousevaluex = 0;
                                    if (mousevaluey < 0)
                                        mousevaluey = 0;
                                    if (mousevaluex > patroi_img.roiarray.GetLength(1) - 1)
                                        mousevaluex = patroi_img.roiarray.GetLength(1) - 1;
                                    if (mousevaluey > patroi_img.roiarray.GetLength(0) - 1)
                                        mousevaluey = patroi_img.roiarray.GetLength(0) - 1;
                                    value = patroi_img.roiarray[mousevaluey, mousevaluex];
                                    value = (float)(value * (time));
                                    Txtxvalue.Text = Convert.ToString(value);

                                }
                            }

                            //int mouselefttopx = mousevaluex+ 
                        }
                        //imgclass img = new imgclass(dosagename_2levelpath, "Dosage_2level", ptbh, ptbw, orgimg.realposlefttop, orgimg.realposrightbottom, 1, pos0, pos1);



                    }
                    else
                    {

                    }



                }





                if (!ROIshowCB.Checked)
                {
                    Txtmousex.Text = Convert.ToString(mousex);
                    Txtmousey.Text = Convert.ToString(mousey);
                }




                //Txtmousex.Text = Convert.ToString(mousex);
                //Txtmousey.Text = Convert.ToString(mousey);

                if (e.Button != MouseButtons.Left)
                    return;
                Point tempEndPoint = e.Location;
                int tempEndPointx = tempEndPoint.X;
                int tempEndPointy = tempEndPoint.Y;
                ulong tempEndPointxul = (ulong)tempEndPoint.X & 0xffff;//40000
                ulong tempEndPointyul = (ulong)tempEndPoint.Y & 0xffff;//40000

                tempEndPoint.X = (int)tempEndPointxul;//40000
                tempEndPoint.Y = (int)tempEndPointyul;//40000
                RectOriginal.Location = new Point(Math.Min(RectStartPoint.X, tempEndPoint.X), Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                RectPattern.Location = new Point(Math.Min(RectStartPoint.X, tempEndPoint.X), Math.Min(RectStartPoint.Y, tempEndPoint.Y));
                RectOriginal.Size = new Size(
                    Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
                RectPattern.Size = new Size(
                    Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));


            }
            else
            {
                if (orgflag == false)
                {
                    return;
                }
                //int pos0 = panel1.HorizontalScroll.Value;
                //int pos1 = panel1.VerticalScroll.Value;
                //mousexvalue = (ulong)e.X & 0xffff;//40000
                //mouseyvalue = (ulong)e.Y & 0xffff;//40000
                //mousex = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);
                //mousey = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);
                if (e.Button != MouseButtons.Left && RectoptCB.Checked && mouseright != true)
                {
                    int pos0 = panel1.HorizontalScroll.Value;
                    int pos1 = panel1.VerticalScroll.Value;
                    mousex = e.X;
                    mousey = e.Y;
                    mousexvalue = (ulong)e.X & 0xffff;//40000
                    mouseyvalue = (ulong)e.Y & 0xffff;//40000

                    mousex = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);
                    mousey = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);
                    if (!ROIshowCB.Checked)
                    {
                        Txtmousex.Text = Convert.ToString(mousex);
                        Txtmousey.Text = Convert.ToString(mousey);
                    }

                    rectoptflag = true;
                    bool success;
                    bool success1;
                    if (TxtRectX.Text != "" && TxtRectY.Text != "")
                    {
                        success = int.TryParse(TxtRectX.Text, out int result);
                        if (success == true)
                        {
                            newrectx = result;
                        }
                        success1 = int.TryParse(TxtRectY.Text, out int result1);
                        if (success1 == true)
                        {
                            newrecty = result1;
                        }
                        rectopt.Size = new Size(newrectx, newrecty);

                    }
                    else if (TxtRectX.Text != "" && TxtRectY.Text == "")
                    {
                        success = int.TryParse(TxtRectX.Text, out int result);
                        if (success == true)
                        {
                            newrectx = result;
                        }
                        rectopt.Size = new Size(newrectx, 300);
                    }
                    else if (TxtRectX.Text == "" && TxtRectY.Text != "")
                    {
                        success1 = int.TryParse(TxtRectY.Text, out int result1);
                        if (success1 == true)
                        {
                            newrecty = result1;
                        }
                        rectopt.Size = new Size(300, newrecty);
                    }
                    else
                    {
                        rectopt.Size = rectoptsize;
                    }
                    rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                    optlefttop.X = (int)mousexvalue;
                    optlefttop.Y = (int)mouseyvalue;
                    rectopt.Location = optlefttop;
                    rectopt.Size = rectnewsize;
                    OriginalPB.Invalidate();
                    PatternPB.Invalidate();
                    mouseright = false;


                }
                //if (e.Button == MouseButtons.Right && RectoptCB.Checked)
                //{
                //    //rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                //    rectoptflag = true;
                //    rectopt.Size = rectoptsize;
                //    rectnewsize = orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                //    rectopt.Size = rectnewsize;
                //    rectopt.Location = optlefttop;
                //    mouseright = true;
                //}
            }


        }

        private void ROIcheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mouseevent == true)
            {
                mouseevent = false;
            }
            else
            {
                mouseevent = true;
            }
        }

        private void OriginalPB_Move(object sender, EventArgs e)
        {

        }

        private void TxtThreshold_TextChanged(object sender, EventArgs e)
        {
            bool success;
            //float threshold = 0;
            success = float.TryParse(TxtThreshold.Text, out float result);
            if (success == true)
            {
                threshold = result;
                //threshold= threshold / time;
                ini.IniWriteValue("Section", "Threshold", threshold.ToString(), filenameini);
                threshold = threshold / time;
            }
            else
            {

            }

        }

        private void Txtratio_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            {
                // timer1.Enabled = true;
            }







        }
        private void Txtratio_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9 && e.KeyCode == Keys.Back)
            //{
            //    timer1.Enabled = true;
            //}
            //switch (e.KeyCode)
            //{

            //    case Keys.NumPad0:
            //        {

            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad1:
            //        {
            //           // timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad2:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad3:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad4:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad5:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad6:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad7:
            //        {
            //           //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad8:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }
            //    case Keys.NumPad9:
            //        {
            //            //timer1.Enabled = true;
            //            break;
            //        }









            //}




        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            //RemainderTime = EndTime - DateTime.Now;
            //if(RemainderTime.TotalSeconds==5)
            //{
            //    Txtratio_TextChanged(sender, e);
            //}
            //else if(RemainderTime.TotalSeconds<0)
            //{
            //    timer1.Stop();
            //}




        }

        private void Txtratio_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void PatternPB_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                if (ROIshowCB.Checked)
                {
                    return;
                }
                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;
                RectStartPoint = e.Location;
                ulong rectstartponitx = (ulong)RectStartPoint.X & 0xffff;
                ulong rectstartponity = (ulong)RectStartPoint.Y & 0xffff;

                ROIlefttop.X = mousescalex(rectstartponitx, rationew, pos0, currentlefttoppos.X);//40082
                ROIlefttop.Y = mousescaley(rectstartponity, rationew, pos1, currentlefttoppos.Y);//40082
                RectStartPoint.X = (int)rectstartponitx;//40000
                RectStartPoint.Y = (int)rectstartponity;//40000

                TxtlefttopxROI.Text = Convert.ToString(ROIlefttop.X);
                TxtlefttopyROI.Text = Convert.ToString(ROIlefttop.Y);


                down = true;

            }
            else if (e.Button == MouseButtons.Right)
            {
                //bool success;
                //bool success1;
                //if (TxtRectX.Text != "" && TxtRectY.Text != "")
                //{
                //    success = int.TryParse(TxtRectX.Text, out int result);
                //    if (success == true)
                //    {
                //        newrectx = result;
                //    }
                //    success1 = int.TryParse(TxtRectY.Text, out int result1);
                //    if (success1 == true)
                //    {
                //        newrecty = result1;
                //    }
                //    rectopt.Size = new Size(newrectx, newrecty);

                //}
                //else if (TxtRectX.Text != "" && TxtRectY.Text == "")
                //{
                //    success = int.TryParse(TxtRectX.Text, out int result);
                //    if (success == true)
                //    {
                //        newrectx = result;
                //    }
                //    rectopt.Size = new Size(newrectx, 300);
                //}
                //else if (TxtRectX.Text == "" && TxtRectY.Text != "")
                //{
                //    success1 = int.TryParse(TxtRectY.Text, out int result1);
                //    if (success1 == true)
                //    {
                //        newrecty = result1;
                //    }
                //    rectopt.Size = new Size(300, newrecty);
                //}
                //else
                //{
                //    rectopt.Size = rectoptsize;
                //}
                RectOriginal.Size = new Size(0, 0);
                RectPattern.Size = new Size(0, 0);
                optlefttop.X = (int)mousexvalue;
                optlefttop.Y = (int)mouseyvalue;
                int mousex40082;
                int mousey40082;
                int pos0 = panel1.HorizontalScroll.Value;
                int pos1 = panel1.VerticalScroll.Value;
                mousex40082 = mousescalex(mousexvalue, rationew, pos0, currentlefttoppos.X);//40082
                mousey40082 = mousescaley(mouseyvalue, rationew, pos1, currentlefttoppos.Y);//40082
                TxtlefttopxROI.Text = Convert.ToString(mousex40082);
                TxtlefttopyROI.Text = Convert.ToString(mousey40082);

                //optlefttop = orgimg.rectopt_point_updating((int)mousexvalue, (int)mouseyvalue, rationew);
                rectoptflag = true;
                //rectnewsize40000= orgimg.rectopt_updating(rectopt.Width, rectopt.Height, rationew);
                rectnewsize = orgimg.rectopt_updating_1(rectopt.Width, rectopt.Height, rationew);
                //rectopt.Size = rectnewsize40000;
                TxtrightbottomxROI.Text = Convert.ToString(mousex40082 + rectnewsize.Width);
                TxtrightbottomyROI.Text = Convert.ToString(mousey40082 + rectnewsize.Height);
                if (TxtlefttopxROI.Text != "" && TxtlefttopyROI.Text != "" && TxtrightbottomxROI.Text != "" && TxtrightbottomyROI.Text != "")
                {
                    int roiheight = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roiwidth = Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                    TxtroiHeight.Text = Convert.ToString(roiheight);
                    TxtroiWidth.Text = Convert.ToString(roiwidth);
                }
                rectopt.Location = optlefttop;
                mouseright = true;
                if (!RectoptCB.Checked)
                {
                    TxtlefttopyROI.Text = "";
                    TxtlefttopxROI.Text = "";
                    TxtrightbottomyROI.Text = "";
                    TxtrightbottomxROI.Text = "";
                }
                else
                {

                }

                OriginalPB.Invalidate();
                PatternPB.Invalidate();
            }
            else
            {
                return;
            }
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void OriginalPB_MouseUp(object sender, MouseEventArgs e)
        {

            if (mouseevent == true)
            {
                //Originalflag = true;
                if (orgflag == false)
                {
                    return;
                }
                if (e.Button != MouseButtons.Left)
                    return;

                int Rectwidth = RectOriginal.Width;
                int Rectheight = RectOriginal.Height;

                ROIrightbottom.X = ROIlefttop.X + (int)(Rectwidth / orgimg.ratio);
                ROIrightbottom.Y = ROIlefttop.Y + (int)(Rectheight / orgimg.ratio);

                TxtrightbottomxROI.Text = Convert.ToString(ROIrightbottom.X);
                TxtrightbottomyROI.Text = Convert.ToString(ROIrightbottom.Y);
                if(TxtlefttopxROI.Text!=""&& TxtlefttopyROI.Text!=""&& TxtrightbottomxROI.Text!="" && TxtrightbottomyROI.Text!="")
                {
                    int roiheight = Convert.ToInt32(TxtrightbottomyROI.Text) - Convert.ToInt32(TxtlefttopyROI.Text);
                    int roiwidth= Convert.ToInt32(TxtrightbottomxROI.Text) - Convert.ToInt32(TxtlefttopxROI.Text);
                    TxtroiHeight.Text = Convert.ToString(roiheight);
                    TxtroiWidth.Text = Convert.ToString(roiwidth);
                }
                down = false;
                OriginalPB.Invalidate();
                PatternPB.Invalidate();

            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }
        public byte[,] readtemp(string filename, long offsetrow, long offsetcol, int row, int col, int totallen)
        {

            byte[] buffer0 = new byte[col];
            byte[,] buffer = new byte[row, col];
            using (BinaryReader reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < row; ++i)
                {
                    reader.BaseStream.Seek(offsetrow * totallen + offsetcol + totallen * i, SeekOrigin.Begin);
                    //for(int i=0;i<row;++i)
                    reader.Read(buffer0, 0, col);

                    for (int j = 0; j < col; ++j)
                        buffer[i, j] = buffer0[j];
                }
            }
            return buffer;
        }
        public void ratio(float ratio, float realres)
        {
            //t = OriginalPB.Size;
            //t1 = PatternPB.Size;

            rationew = ratio / 100;
            orgimg.ratio = rationew;

            if (comboflag == true)
            {
                if (patimg != null)
                    patimg.ratio = rationew;
            }



            float resnew = realres / rationew;
            Txtresolution.Text = Convert.ToString(resnew);

            if (ratiofirst == true)
            {

                ratiofirst = false;
            }
            else
            {
                if (ratiosecond == true)
                {
                    oldratio = 1;
                    ratiosecond = false;

                }
                else
                {
                    oldratio = ratiotmp;
                }
                ratiotmp = rationew;
            }



            //if (ratio == 100)
            //{
            //    t = OriginalPB.Size;
            //    t1 = PatternPB.Size;
            //    orgheight = t.Height;
            //    orgwidth = t.Width;

            //    patheight = t1.Height;
            //    patwidth = t1.Width;
            //    oldratio = float.Parse(Txtratio.Text);
            //    if (comboBox1.Text == "Dosage")
            //    {
            //        PatternPB.Height = dosageh5image.rownum - 1;
            //        PatternPB.Width = dosageh5image.colnum - 1;
            //    }
            //    else if (comboBox1.Text == "LDFile")
            //    {

            //        PatternPB.Height = ldh5image.rownum - 1;
            //        PatternPB.Width = ldh5image.colnum - 1;
            //    }
            //    OriginalPB.Invalidate();
            //    PatternPB.Invalidate();
            //    ratiochangescendflag = false;
            //    ratiocnt = 1;
            //}
            //else

            {
                orgheight = orgimg.h5img.rownum;
                orgwidth = orgimg.h5img.colnum;
                int orgheightscale = (int)(orgheight * ratio / 100);
                int orgwidthscale = (int)(orgwidth * ratio / 100);
                magnifyratioheight = orgheightscale;
                magnifyratiowidth = orgwidthscale;

                if (orgheightscale >= ptbmax && orgwidthscale >= ptbmax)
                {
                    //orgheight = orgh5image.rownum - 1;
                    //orgwidth = orgh5image.colnum - 1;
                    //OriginalPB.Height = ptbmax;
                    //OriginalPB.Width = ptbmax;

                    //PatternPB.Height = ptbmax;
                    //PatternPB.Width = ptbmax;
                    //ptbmax=65535;
                    //magnifyratioheight = orgheightscale;
                    //magnifyratiowidth = orgwidthscale;
                    //if (comboBox1.Text == "Dosage")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosage(dosageimgpath, 0, 0);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, 0, 0, rationew);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //}
                    //else if (comboBox1.Text == "LDFile")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposld(ldimgpath, 0, 0);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposldfloatzoomout(ldimgpath, 0, 0, rationew);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //}
                    ptbmaxsizeall = true;

                    ptbmaxsizeheight = false;
                    ptbmaxsizewidth = false;
                    ptbminsizeall = false;
                    ptbminsizeheight = false;
                    ptbminsizewidth = false;
                }
                else if (orgheightscale >= ptbmax)
                {

                    //t = OriginalPB.Size;
                    //t1 = PatternPB.Size;
                    //orgheight = t.Height;
                    //orgwidth = t.Width;
                    //orgheight = orgh5image.rownum - 1;
                    //orgwidth = orgh5image.colnum - 1;
                    //patheight = t1.Height;
                    //patwidth = t1.Width;
                    //OriginalPB.Height = ptbmax;
                    //OriginalPB.Width = orgwidthscale;
                    //OriginalPB.Width = ptbmax;
                    //PatternPB.Height = ptbmax;
                    //PatternPB.Width = patwidthscale;
                    //PatternPB.Width = ptbmax;
                    //ptbmax=65535;
                    //magnifyratioheight = orgheightscale;
                    //magnifyratiowidth = orgwidthscale;
                    //if (comboBox1.Text == "Dosage")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosage(dosageimgpath, 0, 0);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, 0, 0, rationew);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //}
                    //else if (comboBox1.Text == "LDFile")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposld(ldimgpath, 0, 0);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposldfloatzoomout(ldimgpath, 0, 0, rationew);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //}
                    ptbmaxsizeheight = true;
                    ptbmaxsizeall = false;

                    ptbmaxsizewidth = false;
                    ptbminsizeall = false;
                    ptbminsizeheight = false;
                    ptbminsizewidth = false;
                }
                else if (orgwidthscale >= ptbmax)
                {
                    //t = OriginalPB.Size;
                    //t1 = PatternPB.Size;
                    //orgheight = t.Height;
                    //orgwidth = t.Width;
                    //orgheight = orgh5image.rownum - 1;
                    //orgwidth = orgh5image.colnum - 1;
                    //patheight = t1.Height;
                    //patwidth = t1.Width;
                    //OriginalPB.Height = orgheightscale;
                    //OriginalPB.Width = ptbmax;
                    //PatternPB.Height = patheightscale;
                    //PatternPB.Width = ptbmax;
                    //magnifyratioheight = orgheightscale;
                    //magnifyratiowidth = orgwidthscale;
                    //if (comboBox1.Text == "Dosage")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosage(dosageimgpath, 0, 0);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, 0, 0, rationew);
                    //        dosageimage = BufferTobinaryImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //}
                    //else if (comboBox1.Text == "LDFile")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposld(ldimgpath, 0, 0);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposldfloatzoomout(ldimgpath, 0, 0, rationew);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //}
                    ptbmaxsizewidth = true;
                    ptbmaxsizeall = false;
                    ptbmaxsizeheight = false;

                    ptbminsizeall = false;
                    ptbminsizeheight = false;
                    ptbminsizewidth = false;
                }
                else if (ratio < 100)
                {
                    //orgheight = orgh5image.rownum - 1;
                    //orgwidth = orgh5image.colnum - 1;
                    //OriginalPB.Height = orgheightscale;
                    //OriginalPB.Width = orgwidthscale;
                    //PatternPB.Height = patheightscale;
                    //PatternPB.Width = patwidthscale;
                    if (orgimg.img != null)
                    {
                        orgimg.updateimg(0, 0);
                    }


                    if (comboflag == true && patimg != null)
                    {
                        if (patimg.img != null)
                            patimg.updateimg(0, 0);
                    }
                    //Tuple<int[], byte[,]> currentarraytuple;
                    //currentarraytuple = calculateposorgfloatzoomout(Originalpath, 0, 0, rationew);
                    //if (orgh5image.rownum <= realposrightbottom.Y)
                    //{
                    //    realposrightbottom.Y = orgh5image.rownum - 1;
                    //}
                    //else if (orgh5image.colnum <= realposrightbottom.X)
                    //{
                    //    realposrightbottom.X = orgh5image.colnum - 1;
                    //}
                    //orgimage = BufferTobinaryImage(currentarraytuple.Item2);
                    //realposlefttop.X = currentarraytuple.Item1[0];
                    //realposlefttop.Y = currentarraytuple.Item1[1];
                    //realposrightbottom.X = currentarraytuple.Item1[2];
                    //realposrightbottom.Y = currentarraytuple.Item1[3];


                    //if (comboBox1.Text == "Dosage")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosage(dosageimgpath, 0, 0);
                    //        dosageimage = BufferToImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupledosage;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupledosage = calculateposdosagefloatzoomout(dosageimgpath, 0, 0, rationew);
                    //        dosageimage = BufferToImage(currentarraytupledosage.Item2);
                    //        realposlefttop.X = currentarraytupledosage.Item1[0];
                    //        realposlefttop.Y = currentarraytupledosage.Item1[1];
                    //        realposrightbottom.X = currentarraytupledosage.Item1[2];
                    //        realposrightbottom.Y = currentarraytupledosage.Item1[3];
                    //    }
                    //}
                    //else if (comboBox1.Text == "LDFile")
                    //{
                    //    if (rationew >= 1)
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposld(ldimgpath, 0, 0);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //    else
                    //    {
                    //        Tuple<int[], byte[,]> currentarraytupleld;
                    //        int pos0 = panel1.HorizontalScroll.Value;
                    //        int pos1 = panel1.VerticalScroll.Value;
                    //        currentarraytupleld = calculateposldfloatzoomout(ldimgpath, 0, 0, rationew);
                    //        ldimage = BufferTobinaryImage(currentarraytupleld.Item2);
                    //        realposlefttop.X = currentarraytupleld.Item1[0];
                    //        realposlefttop.Y = currentarraytupleld.Item1[1];
                    //        realposrightbottom.X = currentarraytupleld.Item1[2];
                    //        realposrightbottom.Y = currentarraytupleld.Item1[3];
                    //    }
                    //}
                    ptbminsizeall = true;
                    ptbmaxsizeall = false;
                    ptbmaxsizeheight = false;
                    ptbmaxsizewidth = false;
                    ptbminsizeheight = false;
                    ptbminsizewidth = false;
                }
                else
                {
                    //OriginalPB.Height = orgimg.h5img.rownum;
                    //OriginalPB.Width = orgimg.h5img.colnum;
                    //OriginalPB.Height = orgheightscale;
                    //OriginalPB.Width = orgwidthscale;
                    //orgheight = OriginalPB.Height;
                    //orgwidth = OriginalPB.Width;
                    //if(comboflag==true)
                    //{
                    //    PatternPB.Height = patimg.h5img.rownum;
                    //    PatternPB.Width = patimg.h5img.colnum;
                    //    patheight = PatternPB.Height;
                    //    patwidth = PatternPB.Width;
                    //}

                    //if (comboBox1.Text == "Dosage")
                    //{
                    //    PatternPB.Height = dosageh5image.rownum - 1;
                    //    PatternPB.Width = dosageh5image.colnum - 1;
                    //    patheight = PatternPB.Height;
                    //    patwidth = PatternPB.Width;
                    //}
                    //else if (comboBox1.Text == "LDFile")
                    //{

                    //    PatternPB.Height = ldh5image.rownum - 1;
                    //    PatternPB.Width = ldh5image.colnum - 1;
                    //    patheight = PatternPB.Height;
                    //    patwidth = PatternPB.Width;

                    //}
                    ptbmaxsizeall = false;
                    ptbmaxsizeheight = false;
                    ptbmaxsizewidth = false;
                    ptbminsizeall = false;
                    ptbminsizeheight = false;
                    ptbminsizewidth = false;

                    //PatternPB.Height = patheightscale;
                    //PatternPB.Width = patwidthscale;
                }

                //PatternPB.Height = OriginalPB.Height;
                //PatternPB.Width = OriginalPB.Width;

                if (comboflag == true)
                {
                    if (patimg == null)
                    {
                        return;
                    }
                    patheight = patimg.h5img.rownum;
                    patwidth = patimg.h5img.colnum;
                    int patheightscale = (int)(patheight * ratio / 100);
                    int patwidthscale = (int)(patwidth * ratio / 100);

                    if (patheightscale >= ptbmax && patwidthscale >= ptbmax)
                    {

                        //PatternPB.Height = ptbmax;
                        //PatternPB.Width = ptbmax;

                        //magnifyratioheight = orgheightscale;
                        //magnifyratiowidth = orgwidthscale;

                        ptbmaxsizeall = true;

                        ptbmaxsizeheight = false;
                        ptbmaxsizewidth = false;
                        ptbminsizeall = false;
                        ptbminsizeheight = false;
                        ptbminsizewidth = false;
                    }
                    else if (patheightscale >= ptbmax)
                    {


                        //PatternPB.Height = ptbmax;
                        //PatternPB.Width = patwidthscale;

                        //magnifyratioheight = orgheightscale;
                        //magnifyratiowidth = orgwidthscale;

                        ptbmaxsizeheight = true;
                        ptbmaxsizeall = false;

                        ptbmaxsizewidth = false;
                        ptbminsizeall = false;
                        ptbminsizeheight = false;
                        ptbminsizewidth = false;
                    }
                    else if (patwidthscale >= ptbmax)
                    {

                        //PatternPB.Height = patheightscale;
                        //PatternPB.Width = ptbmax;
                        //magnifyratioheight = orgheightscale;
                        //magnifyratiowidth = orgwidthscale;

                        ptbmaxsizewidth = true;
                        ptbmaxsizeall = false;
                        ptbmaxsizeheight = false;

                        ptbminsizeall = false;
                        ptbminsizeheight = false;
                        ptbminsizewidth = false;
                    }
                    else if (ratio < 100)
                    {

                        //PatternPB.Height = patheightscale;
                        //PatternPB.Width = patwidthscale;
                        if (orgimg.img != null)
                            orgimg.updateimg(0, 0);

                        if (comboflag == true && patimg.img != null)
                        {
                            patimg.updateimg(0, 0);
                        }

                        ptbminsizeall = true;
                        ptbmaxsizeall = false;
                        ptbmaxsizeheight = false;
                        ptbmaxsizewidth = false;
                        ptbminsizeheight = false;
                        ptbminsizewidth = false;
                    }
                    else
                    {
                        //PatternPB.Height = patheightscale;
                        //PatternPB.Width = patwidthscale;

                        ptbmaxsizeall = false;
                        ptbmaxsizeheight = false;
                        ptbmaxsizewidth = false;
                        ptbminsizeall = false;
                        ptbminsizeheight = false;
                        ptbminsizewidth = false;
                    }
                }

                //oldratio = ratio;
                //ratiocnt++;
                OriginalPB.Invalidate();
                PatternPB.Invalidate();
            }

            //PatternPB.Invalidate();

        }
        public void ratioforDosage(float ratio, float realres)
        {

        }
        private bool DrawImageCallback8(IntPtr callBackData)
        {

            // Test for call that passes callBackData parameter.
            if (callBackData == IntPtr.Zero)
            {

                // If no callBackData passed, abort DrawImage method.
                return true;
            }
            else
            {

                // If callBackData passed, continue DrawImage method.
                return false;
            }
        }



    }
}
