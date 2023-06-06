using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using HDF5DotNet;



using BitMiracle.LibTiff.Classic;
namespace guimain
{
    class guimainCode
    {
        tiffimage tiffimg = GuiMain.tiffimg;
        public static string filenameini = "path.ini";
        public static InI ini = new InI();
        public double picturesize = Convert.ToDouble(ini.IniReadValue("Section", "MaxSize", filenameini));
        public int OrgWinRow = Convert.ToInt16(ini.IniReadValue("Section", "OrgWinRow", filenameini));//可使用Int32orInt64
        public int OrgWinCol = Convert.ToInt16(ini.IniReadValue("Section", "OrgWinCol", filenameini));
        public float displacement = Convert.ToSingle(ini.IniReadValue("Section", "Displacement", filenameini));
        private const int NumberOfRetries = 100;
        private const int DelayOnRetry = 1000;
        public byte[,] img;
        public void firstfunction()
        {
            int i = 0;
        }
        public static T[,] Copy<T>(T[,] a)
               where T : new()
        {
            long n1 = a.GetLongLength(0);
            long n2 = a.GetLongLength(1);
            long offset = 0;
            long length = n1 * n2;
            long maxlength = Int32.MaxValue;
            T[,] b = new T[n1, n2];
            while (length > maxlength)
            {
                System.Array.Copy(a, offset, b, offset, maxlength);
                offset += maxlength;
                length -= maxlength;
            }
            System.Array.Copy(a, offset, b, offset, length);
            return b;
        }

        public void scanning2level(ldconfig ldfile1, ldconfig ldfile2, tiffimage tiffimg, string outputfolder, string tempfolderpath)
        {

            //float scanlinepitch = 1.25F;
            //float LDps = ldfile1.LDps;

            float cnt_scale = displacement / tiffimg.imgps;
            //picturesize = 0.5;

            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);//宣告最大不能超過picturesize(GB)=2.5*1024*1024*1024Byte
            int fixedh = (int)((double)maxsize / (double)tiffimg.width);//w*h=maxsize,h=maxsize/w
            if (fixedh > tiffimg.height)//超出最大tiffimage大小就等於tiffimage
                fixedh = tiffimg.height;

            int space ,t0;//space為掃描空間的時間，t0為初始時間
            //bool lightengine2 = true;
            //if (tiffimg.width - 1 > ldfile1.xmax)
            //    lightengine2 = true;//LE2啟動
            //if (lightengine2 == true)
            {
                t0 = (int)(Math.Min(ldfile1.ymin, ldfile2.ymin)/ cnt_scale);
                space = (int)Math.Ceiling((Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin)) / cnt_scale);
            }
            float space0 = Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin);
            int heightscale = (int)Math.Ceiling(tiffimg.height / cnt_scale);
            byte[,] output1 = new byte[(t0 + space + heightscale+1) * 1000, 4];
            byte[,] output2 = new byte[(t0 + space + heightscale+1) * 1000, 4];

            int t = 0;
            string orgimgname = tempfolderpath + GuiMain.filename + "_img.h5", ldimgname = tempfolderpath + GuiMain.filename + "_ldimg_2level.h5", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp", ldappendname = "ldappend", recordname = "recordtemp";

            fileio fileprocess = new fileio();

            hdf5 file = new hdf5();
            file.openfile(orgimgname);
            //fileprocess.startposition = imgstartrow;
            hdf5 ldimg = new hdf5();
            if (File.Exists(ldimgname))
            {
                File.Delete(ldimgname);
            }
            ldimg.createfile(ldimgname, file.rownum, file.colnum, fixedh, tiffimg.width, 0);
            //fileprocess.computelen(tiffimg.height);
            //fileprocess.existencecheck(ldfacetname);
            //fileprocess.existencecheck(ldldname);
            //fileprocess.existencecheck(ldtname);
            //fileprocess.existencecheck(ldappendname);
            //mysql mysqlobj = new mysql();
            //List<record> appendld = new List<record>();
            //record rc;
            bool lastiter = false;

            //mysqlobj.Initialize();
            //mysqlobj.OpenConnection();
            //mysqlobj.temptablecreate(recordname, 1);
            //mysqlobj.CloseConnection();



            Thread savedatathread = new Thread(firstfunction);
            savedatathread.Start();
            for (int row = 0; ; row += fixedh)
            {
                byte[,] patstrip0, imgstrip;

                //patstrip0 = tiffimg.readstrip(k);
                t = (int)(row / cnt_scale);
                //fileprocess.AppendData(orgimgname, patstrip0);
                if (row + fixedh >= tiffimg.height)//最後一個row
                {
                    fixedh = tiffimg.height - row;
                    lastiter = true;
                }

                patstrip0 = file.readbytedata(row, 0, fixedh, tiffimg.width);//讀取byte

                //imgstrip = imresize(imgstrip0, imgps / LDps,'nearest');
                int rownum = fixedh;
                int colnum = tiffimg.width;
                int tlength = (int)Math.Ceiling((rownum + space0) / cnt_scale);

                //List<int> appendld = new List<int>();

                //facet = (t + t0) % 8;
                //if (facet == 0) facet = 8;

                byte[,] result = new byte[rownum, colnum];
                //byte[,] ldfacet = new byte[rownum, colnum];
                //byte[,] ldld = new byte[rownum, colnum];
                //ushort[,] ldtime = new ushort[rownum, colnum];


                //int LDrangex = Math.Min(colnum - 1, ldfile1.xmax);

                //for (int cnt = t + t0; cnt <= t + t0 + tlength; ++cnt)
                //Parallel.For(t + t0, t + t0 + tlength + 1, cnt =>
                //Parallel.For(t + t0, t + t0 + tlength + 1, cnt =>
                for (int cnt = t + t0; cnt <= t + t0 + tlength; ++cnt)
                {
                    int facet = cnt % 8;
                    if (facet == 0) facet = 8;

                    int laseridx, tidx, laseridx1, laseridx2, offset;
                    int ldnowx;
                    int ldnowy;
                    offset = 20000 * (facet - 1);//計算對應的初光位置檔位置
                    for (int i = 0; i < 20000; ++i)//當下facet的20000個點
                    {
                        ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile1.ldy[offset + i], MidpointRounding.AwayFromZero);
                        ldnowx = (int)Math.Round(ldfile1.ldx[offset + i], MidpointRounding.AwayFromZero);

                        if (ldnowx == -1) continue;

                        if (ldnowx <= colnum - 1 && ldnowy >= row && ldnowy < row + rownum)//ldnowy在row及row+rownum範圍內
                            if (patstrip0[ldnowy - row, ldnowx] > 0)//每次都要剪掉row的長度，使目前的ldnowy做標起始值都是0
                            {
                                //if (result[(-1 * ldnowy) - row, ldnowx] == 0)
                                {
                                    result[ldnowy - row, ldnowx] = (byte)facet;//輸出的hdf5檔案

                                    laseridx = (int)Math.Floor((double)i / 1000);//算哪個雷射二極體LD
                                    tidx = i % 1000;//算哪一個record
                                    //if (tidx == 0) tidx = 1000;

                                    laseridx1 = (int)Math.Floor((double)laseridx / 8);//計算哪一個byte
                                    laseridx2 = (int)Math.Pow(2, laseridx % 8);//計算哪一個bits

                                    //rc.row = (uint)(-1 * ldnowy);
                                    //rc.col = (uint)ldnowx;
                                    //rc.label = (uint)((facet - 1) * 20000 + i+1);//i+1 代表label從1開始
                                    //appendld.Add(rc);

                                    output1[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                }
                            }
                    }

                    //if (lightengine2 == true)
                    {
                        for (int i = 0; i < 20000; ++i)
                        {
                            ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile2.ldy[offset + i], MidpointRounding.AwayFromZero);
                            ldnowx = (int)Math.Round(ldfile2.ldx[offset + i], MidpointRounding.AwayFromZero);

                            if (ldnowx == -1) continue;

                            if (ldnowx <= colnum - 1 && ldnowy >= row && ldnowy < row + rownum)
                                if (patstrip0[ldnowy - row, ldnowx] > 0)
                                {
                                    //if (result[(-1 * ldnowy) - row, ldnowx] == 0)
                                    {
                                        result[ldnowy - row, ldnowx] = (byte)facet;

                                        laseridx = (int)Math.Floor((double)i / 1000);
                                        tidx = i % 1000;
                                        //if (tidx == 0) tidx = 1000;

                                        laseridx1 = (int)Math.Floor((double)laseridx / 8);
                                        laseridx2 = (int)Math.Pow(2, laseridx % 8);

                                        //rc.row = (uint)(-1 * ldnowy);
                                        //rc.col = (uint)ldnowx;
                                        //rc.label = (uint)((facet - 1) * 20000 + i+1)+160000;
                                        //appendld.Add(rc);

                                        output2[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                    }
                                }
                        }
                    }


                    //if (facet == 8) facet = 1;
                    //else facet = facet + 1;
                }
                //});
                //ldimg.appenddata(row, 0, result);
                //savedatathread.Join();
                //byte[,] resultcopy = new byte[rownum, colnum];
                //resultcopy = guimainCode.Copy(result);
                //int rowcopy = row;
                //savedatathread = new Thread(() => ldimg.appenddata(rowcopy, 0, resultcopy));
                //savedatathread.Start();

                ldimg.appenddata(row, 0, result);
                //fileprocess.existencecheck(ldimgname+k.ToString() + ".txt");

                //fileprocess.AppendData(ldimgname+k.ToString()+ ".txt", result);
                //mysqlobj.Initialize();
                //mysqlobj.OpenConnection();
                //mysqlobj.AppendData(recordname, appendld);
                //mysqlobj.CloseConnection();
                //appendld.Clear();
                //fileprocess.AppendData(ldfacetname + k.ToString()+ ".txt", ldfacet);
                //fileprocess.AppendData(ldldname + k.ToString()+ ".txt", ldld);
                //fileprocess.AppendData(ldtname + k.ToString()+ ".txt", ldtime);
                //fileprocess.AppendData(ldappendname + k.ToString()+ ".txt", appendld);

                if (lastiter == true)
                {
                    file.closefile();
                    string fname1 = outputfolder + tiffimg.imgfilename + "_LE1_2level.LD", fname2 = outputfolder + tiffimg.imgfilename + "_LE2_2level.LD";
                    Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
                    saveld1.Start();
                    //if (lightengine2)
                    {
                        //Thread saveld2;
                        //saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
                        //saveld2.Start();
                        fileprocess.writeld(fname2, output2);
                        savedatathread.Join();
                        saveld1.Join();
                        //saveld2.Join();
                    }
                    //else
                    //{
                    //    savedatathread.Join();
                    //    saveld1.Join();
                    //}
                    break;
                }
                //row = rownum + row;


            }
            ldimg.closefile();
            //string[] patname = imgfilename.Split('\\');



            //Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
            //saveld1.Start();
            //fileprocess.writeld(fname1, output1);
            //Thread saveld2;
            //if (lightengine2)
            //{
            //    saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
            //    saveld2.Start();
            //    //fileprocess.writeld(fname2, output2);
            //}
        }
        public void scanning2levelroi(int roirowoffset, int roicoloffset, int roirownum, int roicolnum, ldconfig ldfile1, ldconfig ldfile2, tiffimage tiffimg, string outpath, string beamfilepath, string tempfolderpath, string roimousexy)
        {

            //float scanlinepitch = 1.25F;
            //float LDps = ldfile1.LDps;
            float cnt_scale = displacement / tiffimg.imgps;

            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

            hdf5 beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";//ldfilepath1 = tiffimg.imgfilename + "1.LD", ldfilepath2 = tiffimg.imgfilename + "2.LD";
            beam1.openfile(beamfile1path);
            int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = 6000, orgwindowcol = 3000;
            beam1.closefile();

            int ystart = roirowoffset, xstart = roicoloffset, yend = roirowoffset + roirownum - 1, xend = roicoloffset + roicolnum - 1;
            int tempx = (int)Math.Ceiling(halfcol * 1.25 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 1.25 / tiffimg.imgps);
            int yoffset, xoffset;
            int ldystart = ystart - tempy * 3;
            if (ldystart < 0)
                ldystart = 0;
            int ldxstart = xstart - tempx * 3;
            if (ldxstart < 0)
                ldxstart = 0;
            int ldyend = yend + tempy * 3;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;
            int ldxend = xend + tempx * 3;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;
            yoffset = ystart - ldystart;
            xoffset = xstart - ldxstart;
            int ldrownum = ldyend - ldystart + 1, ldcolnum = ldxend - ldxstart + 1;
            int fixedh = (int)((double)maxsize / (double)ldcolnum);
            if (fixedh > ldrownum)
                fixedh = ldrownum;

            //int space = ldfile1.ymax - ldfile1.ymin, t0 = ldfile1.ymin;
            int space, t0;

            //bool lightengine2 = true;
            //if (ldxend > ldfile1.xmax)
            //    lightengine2 = true;
            //if (lightengine2 == true)
            {
                t0 = (int)(Math.Min(ldfile1.ymin, ldfile2.ymin) / cnt_scale);
                space = (int)Math.Ceiling((Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin)) / cnt_scale);
            }
            float space0 = Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin);
            int heightscale = (int)Math.Ceiling(tiffimg.height / cnt_scale);
            byte[,] output1 = new byte[(t0 + space + heightscale+1) * 1000, 4];
            byte[,] output2 = new byte[(t0 + space + heightscale+1) * 1000, 4];
            
            int t = 0;
            string orgimgname = tempfolderpath + tiffimg.imgfilename + "_img.h5", ldimgname = tempfolderpath + tiffimg.imgfilename + "_ldimg_2level_roi" + roimousexy + ".h5", ldimgname0 = "ldimgroi0.h5", ldldname = "ldtemp", ldtname = "timetemp", ldappendname = "ldappend", recordname = "recordtemp";

            fileio fileprocess = new fileio();

            hdf5 file = new hdf5();
            file.openfile(orgimgname);
            //fileprocess.startposition = imgstartrow;
            hdf5 ldimg = new hdf5();
            if (File.Exists(ldimgname))
            {
                File.Delete(ldimgname);
            }
            ldimg.createfile(ldimgname, ldrownum, ldcolnum, ldrownum, ldcolnum, 0);
            //fileprocess.computelen(tiffimg.height);
            //fileprocess.existencecheck(ldfacetname);
            //fileprocess.existencecheck(ldldname);
            //fileprocess.existencecheck(ldtname);
            //fileprocess.existencecheck(ldappendname);
            //mysql mysqlobj = new mysql();
            //List<record> appendld = new List<record>();
            //record rc;
            bool lastiter = false;

            //mysqlobj.Initialize();
            //mysqlobj.OpenConnection();
            //mysqlobj.temptablecreate(recordname, 1);
            //mysqlobj.CloseConnection();


            Thread savedatathread = new Thread(firstfunction);
            savedatathread.Start();
            t = t0 + (int)(ldystart / cnt_scale);
            for (int row = 0; ; row += fixedh)
            {
                byte[,] patstrip0, imgstrip;

                //patstrip0 = tiffimg.readstrip(k);

                //fileprocess.AppendData(orgimgname, patstrip0);
                if (row + fixedh >= ldrownum)
                {
                    fixedh = ldrownum - row;
                    lastiter = true;
                }

                patstrip0 = file.readbytedata(ldystart, ldxstart, fixedh, ldcolnum);

                //imgstrip = imresize(imgstrip0, imgps / LDps,'nearest');
                int rownum = fixedh;
                int colnum = ldcolnum;
                int tlength = (int)Math.Ceiling((rownum + space0) / cnt_scale);
                //List<int> appendld = new List<int>();

                //facet = t % 8;
                //if (facet == 0) facet = 8;

                byte[,] result = new byte[rownum, colnum];
                //byte[,] ldfacet = new byte[rownum, colnum];
                //byte[,] ldld = new byte[rownum, colnum];
                //ushort[,] ldtime = new ushort[rownum, colnum];

                //int LDrangex = Math.Min(colnum - 1, ldfile1.xmax);

                //for (int cnt = t; cnt <= t + tlength; ++cnt)
                Parallel.For(t, t + tlength + 1, cnt =>
                {
                    int facet = cnt % 8;
                    if (facet == 0) facet = 8;

                    int laseridx, tidx, laseridx1, laseridx2, offset;
                    int ldnowx;
                    int ldnowy;
                    offset = 20000 * (facet - 1);
                    for (int i = 0; i < 20000; ++i)
                    {
                        ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile1.ldy[offset + i], MidpointRounding.AwayFromZero);
                        ldnowx = (int)Math.Round(ldfile1.ldx[offset + i], MidpointRounding.AwayFromZero);
                        //ldnowy = ldfile1.ldy[offset + i] - (int)(cnt * cnt_scale + 0.5);
                        //ldnowx = ldfile1.ldx[offset + i];

                        if (ldnowx == -1) continue;

                        if (ldnowx >= ldxstart && ldnowx <= ldxend && ldnowy >= ldystart && ldnowy < ldystart + rownum)
                            if (patstrip0[ldnowy - ldystart, ldnowx - ldxstart] > 0)
                            {
                                //if (result[(-1 * ldnowy) - ldystart, ldnowx - ldxstart] == 0)
                                {
                                    result[ldnowy - ldystart, ldnowx - ldxstart] = 1;

                                    laseridx = (int)Math.Floor((double)i / 1000);
                                    tidx = i % 1000;
                                    //if (tidx == 0) tidx = 1000;

                                    laseridx1 = (int)Math.Floor((double)laseridx / 8);
                                    laseridx2 = (int)Math.Pow(2, laseridx % 8);

                                    //rc.row = (uint)(-1 * ldnowy);
                                    //rc.col = (uint)ldnowx;
                                    //rc.label = (uint)((facet - 1) * 20000 + i+1);//i+1 代表label從1開始
                                    //appendld.Add(rc);

                                    output1[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                }
                            }
                    }

                    //if (lightengine2 == true)
                    {
                        for (int i = 0; i < 20000; ++i)
                        {
                            ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile2.ldy[offset + i], MidpointRounding.AwayFromZero);
                            ldnowx = (int)Math.Round(ldfile2.ldx[offset + i], MidpointRounding.AwayFromZero);
                            //ldnowy = ldfile2.ldy[offset + i] - (int)(cnt * cnt_scale + 0.5);
                            //ldnowx = ldfile2.ldx[offset + i];

                            if (ldnowx == -1) continue;

                            if (ldnowx >= ldxstart && ldnowx <= ldxend && ldnowy >= ldystart && ldnowy < ldystart + rownum)
                                if (patstrip0[ldnowy - ldystart, ldnowx - ldxstart] > 0)
                                {
                                    //if (result[(-1 * ldnowy) - ldystart, ldnowx - ldxstart] == 0)
                                    {
                                        result[ldnowy - ldystart, ldnowx - ldxstart] = 1;

                                        laseridx = (int)Math.Floor((double)i / 1000);
                                        tidx = i % 1000;
                                        //if (tidx == 0) tidx = 1000;

                                        laseridx1 = (int)Math.Floor((double)laseridx / 8);
                                        laseridx2 = (int)Math.Pow(2, laseridx % 8);

                                        //rc.row = (uint)(-1 * ldnowy);
                                        //rc.col = (uint)ldnowx;
                                        //rc.label = (uint)((facet - 1) * 20000 + i+1)+160000;
                                        //appendld.Add(rc);

                                        output2[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                    }
                                }
                        }
                    }


                    //if (facet == 8) facet = 1;
                    //else facet = facet + 1;

                });
                //ldimg.appenddata(row, 0, result);
                savedatathread.Join();
                byte[,] resultcopy = new byte[rownum, colnum];
                resultcopy = guimainCode.Copy(result);
                int rowcopy = row;
                savedatathread = new Thread(() => ldimg.appenddata(rowcopy, 0, resultcopy));
                savedatathread.Start();
                //fileprocess.existencecheck(ldimgname+k.ToString() + ".txt");

                //fileprocess.AppendData(ldimgname+k.ToString()+ ".txt", result);
                //mysqlobj.Initialize();
                //mysqlobj.OpenConnection();
                //mysqlobj.AppendData(recordname, appendld);
                //mysqlobj.CloseConnection();
                //appendld.Clear();
                //fileprocess.AppendData(ldfacetname + k.ToString()+ ".txt", ldfacet);
                //fileprocess.AppendData(ldldname + k.ToString()+ ".txt", ldld);
                //fileprocess.AppendData(ldtname + k.ToString()+ ".txt", ldtime);
                //fileprocess.AppendData(ldappendname + k.ToString()+ ".txt", appendld);

                if (lastiter == true)
                {
                    file.closefile();
                    string fname1 = tempfolderpath + tiffimg.imgfilename + "_LE1_2level_roi" + roimousexy + ".LD", fname2 = tempfolderpath + tiffimg.imgfilename + "_LE2_2level_roi" + roimousexy + ".LD";
                    Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
                    saveld1.Start();
                    //if (lightengine2)
                    {
                        //Thread saveld2;
                        //saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
                        //saveld2.Start();
                        fileprocess.writeld(fname2, output2);
                        savedatathread.Join();
                        saveld1.Join();
                        //saveld2.Join();
                    }
                    //else
                    //{
                    //    savedatathread.Join();
                    //    saveld1.Join();
                    //}
                    break;
                }
                ldystart += fixedh;
                t += fixedh;

            }
            ldimg.closefile();

            hdf5 oldldimg = new hdf5();
            oldldimg.openfile(ldimgname);
            byte[,] data = oldldimg.readbytedata(yoffset, xoffset, roirownum, roicolnum);
            oldldimg.closefile();
            hdf5 newldimg = new hdf5();
            newldimg.createfile(ldimgname, roirownum, roicolnum, roirownum, roicolnum, 0);
            newldimg.appenddata(0, 0, data);

            newldimg.closefile();
            //string[] patname = imgfilename.Split('\\');

            //string fname1 = tiffimg.imgfilename + "roi1.LD", fname2 = tiffimg.imgfilename + "roi2.LD";

            //fileprocess.writeld(fname1, output1);
            //if (lightengine2) fileprocess.writeld(fname2, output2);

        }
        public void scanning4level(ldconfig ldfile1, ldconfig ldfile2, tiffimage tiffimg, string outpath, string tempfolderpath)
        {

            //float scanlinepitch = 1.25F;
            //float LDps = ldfile1.LDps;
            float cnt_scale = displacement / tiffimg.imgps;

            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);
            int fixedh = (int)((double)maxsize / (double)tiffimg.width);
            if (fixedh > tiffimg.height)
                fixedh = tiffimg.height;

            int space ,t0;
            //bool lightengine2 = true;
            //if (tiffimg.width - 1 > ldfile1.xmax)
            //    lightengine2 = true;
            //if (lightengine2 == true)
            {
                t0 = (int)(Math.Min(ldfile1.ymin, ldfile2.ymin) / cnt_scale);
                space = (int)Math.Ceiling((Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin)) / cnt_scale);
            }
            float space0 = Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin);
            int heightscale = (int)Math.Ceiling(tiffimg.height / cnt_scale);
            byte[,] output1 = new byte[(t0 + space + heightscale+1) * 1000, 5];
            byte[,] output2 = new byte[(t0 + space + heightscale+1) * 1000, 5];
            
            int t = 0;
            string orgimgname = tempfolderpath + tiffimg.imgfilename + "_img.h5", ldimgname = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level.h5", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp", ldappendname = "ldappend", recordname = "recordtemp";

            fileio fileprocess = new fileio();

            hdf5 file = new hdf5();
            file.openfile(orgimgname);
            //fileprocess.startposition = imgstartrow;
            hdf5 ldimg = new hdf5();
            if (File.Exists(ldimgname))
            {
                File.Delete(ldimgname);
            }
            ldimg.createfile(ldimgname, file.rownum, file.colnum, fixedh, tiffimg.width, 0);
            //fileprocess.computelen(tiffimg.height);
            //fileprocess.existencecheck(ldfacetname);
            //fileprocess.existencecheck(ldldname);
            //fileprocess.existencecheck(ldtname);
            //fileprocess.existencecheck(ldappendname);
            //mysql mysqlobj = new mysql();
            //List<record> appendld = new List<record>();
            //record rc;
            bool lastiter = false;

            //mysqlobj.Initialize();
            //mysqlobj.OpenConnection();
            //mysqlobj.temptablecreate(recordname, 1);
            //mysqlobj.CloseConnection();

            Thread savedatathread = new Thread(firstfunction);
            savedatathread.Start();
            for (int row = 0; ; row += fixedh)
            {
                byte[,] patstrip0, imgstrip;

                //patstrip0 = tiffimg.readstrip(k);
                t = (int)(row / cnt_scale);
                //fileprocess.AppendData(orgimgname, patstrip0);
                if (row + fixedh >= tiffimg.height)
                {
                    fixedh = tiffimg.height - row;
                    lastiter = true;
                }

                patstrip0 = file.readbytedata(row, 0, fixedh, tiffimg.width);

                //imgstrip = imresize(imgstrip0, imgps / LDps,'nearest');
                int rownum = fixedh;
                int colnum = tiffimg.width;
                int tlength = (int)Math.Ceiling((rownum + space0) / cnt_scale);
                //List<int> appendld = new List<int>();

                //facet = (t + t0) % 8;
                //if (facet == 0) facet = 8;

                byte[,] result = new byte[rownum, colnum];
                //byte[,] ldfacet = new byte[rownum, colnum];
                //byte[,] ldld = new byte[rownum, colnum];
                //ushort[,] ldtime = new ushort[rownum, colnum];


                //int LDrangex = Math.Min(colnum - 1, ldfile1.xmax);

                //for (int cnt = t + t0; cnt <= t + t0 + tlength; ++cnt)
                Parallel.For(t + t0, t + t0 + tlength + 1, cnt =>
                {
                    byte hi, lo;
                    double value;
                    int facet = cnt % 8;
                    if (facet == 0) facet = 8;

                    int laseridx, tidx, laseridx1, laseridx2, offset, bitpos;
                    int ldnowx;
                    int ldnowy;
                    offset = 20000 * (facet - 1);
                    for (int i = 0; i < 20000; ++i)
                    {
                        ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile1.ldy[offset + i], MidpointRounding.AwayFromZero);
                        ldnowx = (int)Math.Round(ldfile1.ldx[offset + i], MidpointRounding.AwayFromZero);
                        
                        if (ldnowx == -1) continue;

                        if (ldnowx <= colnum - 1 && ldnowy >= row && ldnowy < row + rownum)
                            if (patstrip0[ ldnowy - row, ldnowx] > 0)
                            {
                                //if (result[(-1 * ldnowy) - row, ldnowx]==0)
                                {
                                    //value = patstrip0[(-1 * ldnowy) - row, ldnowx];
                                    result[ ldnowy - row, ldnowx] = 1;

                                    laseridx = (int)Math.Floor((double)i / 1000);
                                    tidx = i % 1000;
                                    //if (tidx == 0) tidx = 1000;

                                    laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                    hi = 1;//(byte)Math.Floor(value / 2);
                                    lo = 1;//(byte)(value % 2);
                                    bitpos = ((laseridx % 4) * 2);
                                    laseridx2 = hi * (int)Math.Pow(2, bitpos + 1) + lo * (int)Math.Pow(2, bitpos);

                                    //rc.row = (uint)(-1 * ldnowy);
                                    //rc.col = (uint)ldnowx;
                                    //rc.label = (uint)((facet - 1) * 20000 + i+1);//i+1 代表label從1開始
                                    //appendld.Add(rc);

                                    output1[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                }
                            }
                    }

                    //if (lightengine2 == true)
                    {
                        for (int i = 0; i < 20000; ++i)
                        {
                            ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile2.ldy[offset + i], MidpointRounding.AwayFromZero);
                            ldnowx = (int)Math.Round(ldfile2.ldx[offset + i], MidpointRounding.AwayFromZero);
                            
                            if (ldnowx == -1) continue;

                            if (ldnowx <= colnum - 1 && ldnowy >= row && ldnowy < row + rownum)
                                if (patstrip0[ ldnowy - row, ldnowx] > 0)
                                {
                                    //if (result[(-1 * ldnowy) - row, ldnowx] == 0)
                                    {
                                        //value = patstrip0[(-1 * ldnowy) - row, ldnowx];
                                        result[ ldnowy - row, ldnowx] = 1;

                                        laseridx = (int)Math.Floor((double)i / 1000);
                                        tidx = i % 1000;
                                        //if (tidx == 0) tidx = 1000;

                                        //laseridx1 = (int)Math.Floor((double)laseridx / 8);
                                        //laseridx2 = (int)Math.Pow(2, laseridx % 8);

                                        laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                        hi = 1;//(byte)Math.Floor(value / 2);
                                        lo = 1;//(byte)(value % 2);
                                        bitpos = ((laseridx % 4) * 2);
                                        laseridx2 = hi * (int)Math.Pow(2, bitpos + 1) + lo * (int)Math.Pow(2, bitpos);

                                        //rc.row = (uint)(-1 * ldnowy);
                                        //rc.col = (uint)ldnowx;
                                        //rc.label = (uint)((facet - 1) * 20000 + i+1)+160000;
                                        //appendld.Add(rc);

                                        output2[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                    }
                                }
                        }
                    }


                    //if (facet == 8) facet = 1;
                    //else facet = facet + 1;

                });
                //ldimg.appenddata(row, 0, result);
                savedatathread.Join();
                byte[,] resultcopy = new byte[rownum, colnum];
                resultcopy = guimainCode.Copy(result);
                int rowcopy = row;
                savedatathread = new Thread(() => ldimg.appenddata(rowcopy, 0, resultcopy));
                savedatathread.Start();
                //fileprocess.existencecheck(ldimgname+k.ToString() + ".txt");

                //fileprocess.AppendData(ldimgname+k.ToString()+ ".txt", result);
                //mysqlobj.Initialize();
                //mysqlobj.OpenConnection();
                //mysqlobj.AppendData(recordname, appendld);
                //mysqlobj.CloseConnection();
                //appendld.Clear();
                //fileprocess.AppendData(ldfacetname + k.ToString()+ ".txt", ldfacet);
                //fileprocess.AppendData(ldldname + k.ToString()+ ".txt", ldld);
                //fileprocess.AppendData(ldtname + k.ToString()+ ".txt", ldtime);
                //fileprocess.AppendData(ldappendname + k.ToString()+ ".txt", appendld);

                if (lastiter == true)
                {
                    file.closefile();
                    string fname1 = outpath + tiffimg.imgfilename + "_LE1_4level.LD", fname2 = outpath + tiffimg.imgfilename + "_LE2_4level.LD";
                    Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
                    saveld1.Start();
                    //if (lightengine2)
                    {
                        //Thread saveld2;
                        //saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
                        //saveld2.Start();
                        fileprocess.writeld(fname2, output2);
                        savedatathread.Join();
                        saveld1.Join();
                        //saveld2.Join();
                    }
                    //else
                    //{
                    //    savedatathread.Join();
                    //    saveld1.Join();
                    //}
                    break;
                }
                //row = rownum + row;


            }
            ldimg.closefile();
            //string[] patname = imgfilename.Split('\\');



            //Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
            //saveld1.Start();
            //fileprocess.writeld(fname1, output1);
            //Thread saveld2;
            //if (lightengine2)
            //{
            //    saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
            //    saveld2.Start();
            //    //fileprocess.writeld(fname2, output2);
            //}
        }
        public void scanning4levelroi(int roirowoffset, int roicoloffset, int roirownum, int roicolnum, ldconfig ldfile1, ldconfig ldfile2, tiffimage tiffimg, string outpath, string beamfilepath, string tempfolderpath, string roimousexy)
        {

            //float scanlinepitch = 1.25F;
            //float LDps = ldfile1.LDps;
            float cnt_scale = displacement / tiffimg.imgps;

            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

            hdf5 beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";
            //string beamfile1path = @"D:\spottemp\spotarrays100le1.h5", beamfile2path = "", ldfilepath1 = tiffimg.imgfilename + "1.LD", ldfilepath2 = tiffimg.imgfilename + "2.LD";
            beam1.openfile(beamfile1path);
            int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = 6000, orgwindowcol = 3000;
            beam1.closefile();

            int ystart = roirowoffset, xstart = roicoloffset, yend = roirowoffset + roirownum - 1, xend = roicoloffset + roicolnum - 1;
            int tempx = (int)Math.Ceiling(halfcol * 1.25 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 1.25 / tiffimg.imgps);
            int xoffset, yoffset;
            int ldystart = ystart - tempy * 2;
            if (ldystart < 0)
                ldystart = 0;
            int ldxstart = xstart - tempx * 2;
            if (ldxstart < 0)
                ldxstart = 0;
            int ldyend = yend + tempy * 2;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;
            int ldxend = xend + tempx * 2;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;
            yoffset = ystart - ldystart;
            xoffset = xstart - ldxstart;
            int ldrownum = ldyend - ldystart + 1, ldcolnum = ldxend - ldxstart + 1;
            int fixedh = (int)((double)maxsize / (double)ldcolnum);
            if (fixedh > ldrownum)
                fixedh = ldrownum;

            int space, t0;
            //bool lightengine2 = true;
            //if (ldxend > ldfile1.xmax)
            //    lightengine2 = true;
            //if (lightengine2 == true)
            {
                t0 = (int)(Math.Min(ldfile1.ymin, ldfile2.ymin) / cnt_scale);
                space = (int)Math.Ceiling((Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin))/cnt_scale);
            }
            float space0 = Math.Max(ldfile1.ymax, ldfile2.ymax) - Math.Min(ldfile1.ymin, ldfile2.ymin);
            int heightscale = (int)Math.Ceiling(tiffimg.height / cnt_scale);
            byte[,] output1 = new byte[(t0 + space + heightscale+1) * 1000, 5];
            byte[,] output2 = new byte[(t0 + space + heightscale+1) * 1000, 5];
            
            int t = 0;
            string orgimgname = tempfolderpath + tiffimg.imgfilename + "_img.h5", ldimgname = tempfolderpath + tiffimg.imgfilename + "_ldimg_4level_roi" + roimousexy + ".h5", ldimgname0 = "ldimgroi0_4level.h5", ldldname = "ldtemp", ldtname = "timetemp", ldappendname = "ldappend", recordname = "recordtemp";

            fileio fileprocess = new fileio();

            hdf5 file = new hdf5();
            file.openfile(orgimgname);
            //fileprocess.startposition = imgstartrow;
            hdf5 ldimg = new hdf5();
            if (File.Exists(ldimgname))
            {
                File.Delete(ldimgname);
            }
            ldimg.createfile(ldimgname, ldrownum, ldcolnum, ldrownum, ldcolnum, 0);
            //fileprocess.computelen(tiffimg.height);
            //fileprocess.existencecheck(ldfacetname);
            //fileprocess.existencecheck(ldldname);
            //fileprocess.existencecheck(ldtname);
            //fileprocess.existencecheck(ldappendname);
            //mysql mysqlobj = new mysql();
            //List<record> appendld = new List<record>();
            //record rc;
            bool lastiter = false;

            //mysqlobj.Initialize();
            //mysqlobj.OpenConnection();
            //mysqlobj.temptablecreate(recordname, 1);
            //mysqlobj.CloseConnection();


            Thread savedatathread = new Thread(firstfunction);
            savedatathread.Start();
            t = t0 + (int)(ldystart / cnt_scale);
            for (int row = 0; ; row += fixedh)
            {
                byte[,] patstrip0, imgstrip;

                //patstrip0 = tiffimg.readstrip(k);

                //fileprocess.AppendData(orgimgname, patstrip0);
                if (row + fixedh >= ldrownum)
                {
                    fixedh = ldrownum - row;
                    lastiter = true;
                }

                patstrip0 = file.readbytedata(ldystart, ldxstart, fixedh, ldcolnum);

                //imgstrip = imresize(imgstrip0, imgps / LDps,'nearest');
                int rownum = fixedh;
                int colnum = ldcolnum;
                int tlength = (int)Math.Ceiling((rownum + space0) / cnt_scale);
                //List<int> appendld = new List<int>();

                //facet = t % 8;
                //if (facet == 0) facet = 8;

                byte[,] result = new byte[rownum, colnum];
                //byte[,] ldfacet = new byte[rownum, colnum];
                //byte[,] ldld = new byte[rownum, colnum];
                //ushort[,] ldtime = new ushort[rownum, colnum];

                //int LDrangex = Math.Min(colnum - 1, ldfile1.xmax);

                //for (int cnt = t; cnt <= t + tlength; ++cnt)
                //Parallel.For(t, t + tlength + 1, cnt =>
                //for (int cnt = t; cnt <= t + tlength; ++cnt)
                Parallel.For(t, t + tlength + 1, cnt =>
                {
                    byte hi, lo;
                    double value;
                    int facet = cnt % 8;
                    if (facet == 0) facet = 8;

                    int laseridx, tidx, laseridx1, laseridx2, offset, bitpos; 
                    int ldnowx;
                    int ldnowy;
                    offset = 20000 * (facet - 1);
                    for (int i = 0; i < 20000; ++i)
                    {
                        ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile1.ldy[offset + i], MidpointRounding.AwayFromZero);
                        ldnowx = (int)Math.Round(ldfile1.ldx[offset + i], MidpointRounding.AwayFromZero);

                        if (ldnowx == -1) continue;

                        if (ldnowx >= ldxstart && ldnowx <= ldxend && ldnowy >= ldystart && ldnowy < ldystart + rownum)
                            if (patstrip0[ldnowy - ldystart, ldnowx - ldxstart] > 0)
                            {
                                //if (result[(-1 * ldnowy) - ldystart, ldnowx - ldxstart] == 0)
                                {
                                    //value = patstrip0[(-1 * ldnowy) - ldystart, ldnowx - ldxstart];
                                    result[ldnowy - ldystart, ldnowx - ldxstart] = 1;

                                    laseridx = (int)Math.Floor((double)i / 1000);
                                    tidx = i % 1000;
                                    //if (tidx == 0) tidx = 1000;

                                    laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                    hi = 1;//(byte)Math.Floor(value / 2);
                                    lo = 1;//(byte)(value % 2);
                                    bitpos = ((laseridx % 4) * 2);
                                    laseridx2 = hi * (int)Math.Pow(2, bitpos + 1) + lo * (int)Math.Pow(2, bitpos);

                                    //rc.row = (uint)(-1 * ldnowy);
                                    //rc.col = (uint)ldnowx;
                                    //rc.label = (uint)((facet - 1) * 20000 + i+1);//i+1 代表label從1開始
                                    //appendld.Add(rc);

                                    output1[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                }
                            }
                    }

                    //if (lightengine2 == true)
                    {
                        for (int i = 0; i < 20000; ++i)
                        {
                            ldnowy = (int)Math.Round(cnt * cnt_scale - ldfile2.ldy[offset + i], MidpointRounding.AwayFromZero);
                            ldnowx = (int)Math.Round(ldfile2.ldx[offset + i], MidpointRounding.AwayFromZero);

                            if (ldnowx == -1) continue;

                            if (ldnowx >= ldxstart && ldnowx <= ldxend && ldnowy >= ldystart && ldnowy < ldystart + rownum)
                                if (patstrip0[ldnowy - ldystart, ldnowx - ldxstart] > 0)
                                {
                                    //if (result[(-1 * ldnowy) - ldystart, ldnowx - ldxstart] ==0)
                                    {
                                        //value = patstrip0[(-1 * ldnowy) - ldystart, ldnowx - ldxstart];
                                        result[ldnowy - ldystart, ldnowx - ldxstart] = 1;

                                        laseridx = (int)Math.Floor((double)i / 1000);
                                        tidx = i % 1000;
                                        //if (tidx == 0) tidx = 1000;

                                        //laseridx1 = (int)Math.Floor((double)laseridx / 8);
                                        //laseridx2 = (int)Math.Pow(2, laseridx % 8);

                                        laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                        hi = 1;//(byte)Math.Floor(value / 2);
                                        lo = 1;//(byte)(value % 2);
                                        bitpos = ((laseridx % 4) * 2);
                                        laseridx2 = hi * (int)Math.Pow(2, bitpos + 1) + lo * (int)Math.Pow(2, bitpos);

                                        //rc.row = (uint)(-1 * ldnowy);
                                        //rc.col = (uint)ldnowx;
                                        //rc.label = (uint)((facet - 1) * 20000 + i+1)+160000;
                                        //appendld.Add(rc);

                                        output2[(cnt) * 1000 + tidx, laseridx1] += (byte)laseridx2;//cnt-1 代表t=1才紀錄第一條scanline
                                    }
                                }
                        }
                    }


                    //if (facet == 8) facet = 1;
                    //else facet = facet + 1;

                //}
                });
                //ldimg.appenddata(row, 0, result);
                savedatathread.Join();
                byte[,] resultcopy = new byte[rownum, colnum];
                resultcopy = guimainCode.Copy(result);
                int rowcopy = row;
                savedatathread = new Thread(() => ldimg.appenddata(rowcopy, 0, resultcopy));
                savedatathread.Start();
                //fileprocess.existencecheck(ldimgname+k.ToString() + ".txt");

                //fileprocess.AppendData(ldimgname+k.ToString()+ ".txt", result);
                //mysqlobj.Initialize();
                //mysqlobj.OpenConnection();
                //mysqlobj.AppendData(recordname, appendld);
                //mysqlobj.CloseConnection();
                //appendld.Clear();
                //fileprocess.AppendData(ldfacetname + k.ToString()+ ".txt", ldfacet);
                //fileprocess.AppendData(ldldname + k.ToString()+ ".txt", ldld);
                //fileprocess.AppendData(ldtname + k.ToString()+ ".txt", ldtime);
                //fileprocess.AppendData(ldappendname + k.ToString()+ ".txt", appendld);

                if (lastiter == true)
                {
                    file.closefile();
                    string fname1 = tempfolderpath + tiffimg.imgfilename + "_LE1_4level_roi" + roimousexy + ".LD", fname2 = tempfolderpath + tiffimg.imgfilename + "_LE2_4level_roi" + roimousexy + ".LD";
                    //string fname1 = tiffimg.imgfilename + "roi1_4level.LD", fname2 = tiffimg.imgfilename + "roi2_4level.LD";
                    Thread saveld1 = new Thread(() => fileprocess.writeld(fname1, output1));
                    saveld1.Start();
                    //if (lightengine2)
                    {
                        //Thread saveld2;
                        //saveld2 = new Thread(() => fileprocess.writeld(fname2, output2));
                        //saveld2.Start();
                        fileprocess.writeld(fname2, output2);
                        savedatathread.Join();
                        saveld1.Join();
                        //saveld2.Join();
                    }
                    //else
                    //{
                    //    savedatathread.Join();
                    //    saveld1.Join();
                    //}
                    break;
                }
                ldystart += fixedh;
                t += fixedh;

            }
            ldimg.closefile();

            hdf5 oldldimg = new hdf5();
            oldldimg.openfile(ldimgname);
            byte[,] data = oldldimg.readbytedata(yoffset, xoffset, roirownum, roicolnum);
            oldldimg.closefile();
            hdf5 newldimg = new hdf5();
            newldimg.createfile(ldimgname, roirownum, roicolnum, roirownum, roicolnum, 0);
            newldimg.appenddata(0, 0, data);

            newldimg.closefile();
            //string[] patname = imgfilename.Split('\\');

            //string fname1 = tiffimg.imgfilename + "roi1.LD", fname2 = tiffimg.imgfilename + "roi2.LD";

            //fileprocess.writeld(fname1, output1);
            //if (lightengine2) fileprocess.writeld(fname2, output2);

        }
        public void dosage(tiffimage tiffimg, ldconfig ldconfig1, ldconfig ldconfig2, string tempfolderpath, string beampath, string ldfilepath1, string ldfilepath2)
        {
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";
            float cnt_scale = displacement / tiffimg.imgps; 
            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();
            float h5ps = 1.25f;
            float beamscale = tiffimg.imgps / h5ps;
            
            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beampath + "spotarrays100le1_125.h5", beamfile2path = beampath + "spotarrays100le2_125.h5";//, ldfilepath1 = outputfolderpath + tiffimg.imgfilename + "1.LD", ldfilepath2 = outputfolderpath + tiffimg.imgfilename + "2.LD";
            beam1.openfile(beamfile1path);
            int beamrow = (int)(beam1.rownum / beamscale + 0.5), beamcol = (int)(beam1.colnum / beamscale + 0.5);
            if (beamrow % 2 == 0)
            {
                beamrow -= 1;
            }
            if (beamcol % 2 == 0)
            {
                beamcol -= 1;
            }
            int halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2;
            int orgwindowrow = OrgWinRow, orgwindowcol = OrgWinCol;
            beam1.closefile();

            if (height < orgwindowrow)
                orgwindowrow = height;
            if (width < orgwindowcol)
                orgwindowcol = width;

            double scalerow = tiffimg.imgps / ldconfig1.ldps, scalecol = tiffimg.imgps / ldconfig1.ldps;//修改
            int windowrow = (int)(((orgwindowrow - 1) * scalerow) + 0.5) + 1, windowcol = (int)(((orgwindowcol - 1) * scalecol) + 0.5) + 1;
            int scaleh = (int)(((height - 1) * scalerow) + 0.5) + 1, scalew = (int)(((width - 1) * scalecol) + 0.5) + 1;
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level.h5", buffername = tempfolderpath + "buffertemp";
            int lefttoprow = 0, lefttopcol = 0, firstitertemprow = halfrow;
            int startrow = 0, totalcol = 0;
            float[,] beam = new float[beamrow, beamcol];

            //byte[,] pointimg;
            //byte[,] facetimg;
            //byte[,] ldimg;
            //ushort[,] timg;
            
            float[,] expnddosage;
            float[,] dosage;
            bool lastrow = false, lastcol = false;
            List<byte> tablenum = new List<byte>();
            List<byte> tablenum2 = new List<byte>();
            Tuple<float, float> yinterval1, yinterval2, yinterval;
            int t0;
            float ydistance;
            Dictionary<int, float[,,]> spots1, spots2;
            //record[] records;
            fileio fileprocess = new fileio();
            //fileprocess.startposition = imgstartrow;


            matoperation matop = new matoperation();

            int xstart = 0, xend = orgwindowrow - 1, xend0 = orgwindowrow, ystart = 0, yend = orgwindowrow - 1, yend0 = orgwindowrow, maxorgwindowrow = orgwindowrow;
            float min = float.MaxValue, max = 0;
            //width = 2000;

            if (File.Exists(dosagename))
            {
                File.Delete(dosagename);
            }
            //if (File.Exists(dosagename))
            //{
            //    for (int i = 1; i <= NumberOfRetries; ++i)
            //    {
            //        try
            //        {
            //            File.Delete(dosagename); // Do stuff with file
            //            break; // When done we can break loop
            //        }
            //        catch (IOException e) when (i <= NumberOfRetries)
            //        {
            //            // You may check error code to filter some exceptions, not every error
            //            // can be recovered.
            //            Thread.Sleep(DelayOnRetry);
            //        }
            //    }


            //}
            file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = 0, xend0 = orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= width)//當window圖檔超出width，則等於width
                {
                    xend0 = width;
                    lastcol = true;//最後一個col
                }

                xend = xend0 - 1;
                //if (scalew - maxwindowcol * q < maxwindowcol)
                //{
                //    windowcol = scalew - maxwindowcol * q;
                //    lastcol = true;
                //}

                //orgwindowcol = (int)(windowcol / scalecol + 0.5);

                int tempx = halfcol, tempy = halfrow;//半個光點寬度大小
                int ldxstart = xstart - tempx;//x左上角向外半個光點寬度
                if (ldxstart < 0)
                    ldxstart = 0;

                int ldxend = xend + tempx;//x右下角向外半個光點寬度
                if (ldxend >= width)
                    ldxend = width - 1;


                tablenum = ldconfig1.checkrange(ldxstart, ldxend);//看xstart xend在哪個LD範圍內，並把那個LD記下來
                yinterval1 = ldconfig1.scanrange(tablenum);//找出現要掃描的y區間

                tablenum2 = ldconfig2.checkrange(ldxstart, ldxend);//le2
                yinterval2 = ldconfig2.scanrange(tablenum2);//le2

                if (tablenum.Count == 0 && tablenum2.Count == 0)
                {
                    if (lastcol == true)
                        break;
                    else
                        continue;
                }

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                ydistance = yinterval.Item1 - yinterval.Item2;//y掃描的距離

                t0 = (int)(Math.Min(yinterval1.Item2, yinterval2.Item2) / cnt_scale);
                

                beam1.openfile(beamfile1path);
                spots1 = storespots(beam1, tablenum);
                //float[,] spots01 = beam1.readspotdata(1);
                beam1.closefile();
                beam2.openfile(beamfile2path);
                spots2 = storespots(beam2, tablenum2);
                beam2.closefile();



                //float[,] temprow = new float[halfrow * 2, windowcol + halfcol * 2 - firstitercol];
                //float[,] tempcol;

                //fileprocess.existencecheck(dosagename + q.ToString() + ".txt");



                //for (int k = 0; k < Math.Ceiling((double)scaleh / maxwindowrow); ++k)
                for (ystart = 0, yend0 = orgwindowrow; ; ystart += orgwindowrow, yend0 += orgwindowrow)
                {
                    /*ystart為起始值，每次抓orgwindowrow的大小,yend0為最後的y，每次也是加上orgwindowrow*/
                    if (yend0 >= height)//超出圖檔的高，就等於高
                    {
                        yend0 = height;
                        lastrow = true;
                    }
                    yend = yend0 - 1;
                    //if (scaleh - maxwindowrow * k < maxwindowrow)
                    //{
                    //    windowrow = scaleh - maxwindowrow * k;
                    //    lastrow = true;
                    //}

                    int ldystart = ystart - tempy;//y左上角向外半個光點寬度
                    if (ldystart < 0)
                        ldystart = 0;

                    int ldyend = yend + tempy;//y右下角向外半個光點寬度
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow) + 0.5) - (int)((ldxstart * scalerow) + 0.5);//擴展的寬度
                    int ldyoffsetscale = (int)((ystart * scalecol) + 0.5) - (int)((ldystart * scalecol) + 0.5);

                    //int orgrow = yend - ystart;
                    //int orgcol = xend - xstart;

                    int orgrowscale = (int)((yend * scalerow) + 0.5) - (int)((ystart * scalerow) + 0.5) + 1;//原始的大小
                    int orgcolscale = (int)((xend * scalecol) + 0.5) - (int)((xstart * scalecol) + 0.5) + 1;

                    //int ldrow = ldyend - ldystart + 1;
                    //int ldcol = ldxend - ldxstart + 1;

                    int ldrowscale = (int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;//擴展後的寬度
                    int ldcolscale = (int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    expnddosage = new float[ldrowscale + halfrow * 2, ldcolscale + halfcol * 2];
                    //expnddosage = new float[windowrow + halfrow * 2 + halfrow * 2 - firstiterrow, windowcol + halfcol * 2 + halfcol * 2 - firstitercol];
                    //dosage = new float[ldrowscale, ldcolscale];
                    //dosage = new float[orgrowscale, orgcolscale];

                    //dosage = new float[windowrow + halfrow * 2 - firstiterrow, windowcol + halfcol * 2 - firstitercol];

                    //orgwindowrow = (int)(windowrow / scalerow + 0.5);

                    //records = readlabel(xstart, xend - 1, ystart, yend - 1);


                    //record currc;
                    //for(int i=0;i< records.Length;++i)
                    //{
                    //    currc = records[i];
                    //    //if (pointimg[currc.row - lefttoprow, currc.col - lefttopcol] >0)
                    //    //{
                    //    //    --pointimg[currc.row - lefttoprow, currc.col - lefttopcol];

                    //        matop.beamsuperposition(expnddosage, (int)((currc.row- lefttoprow) * scalerow + 0.5) + offsetrow + offsetafterfirstrow, (int)((currc.col- lefttopcol) * scalecol + 0.5) + offsetcol + offsetafterfirstcol, spots[currc.label]);

                    //    //}
                    //}
                    int toffset = t0 + (int)(ldystart/ cnt_scale), ldoffset;//運行時間
                    int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
                    byte[] buffer = new byte[4000];
                    BitArray bits;
                    byte facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart <= ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))//抓入LD檔
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);//移動初始值到toffset，而一個Record是4000byte

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 4000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);

                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>  //s是哪一個Record
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                            {

                                                for (int ldnum = 0; ldnum < 20; ++ldnum)
                                                {
                                                    if (bits[ldnum + startbit] == true)
                                                    {
                                                        int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        int ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);

                                                            matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s,beamscale);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 32;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    //bool flag = false;
                    //toffset = t0 + ldystart;//運行時間
                    //buffer = new byte[4000];
                    facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart > ldconfig2.xmin)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 4000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 20; ++ldnum)
                                                {
                                                    if (bits[ldnum + startbit] == true)
                                                    {
                                                        int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        int ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s, beamscale);

                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 24;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    dosage = matop.cuttingboundary(expnddosage, halfrow + ldyoffsetscale, halfcol + ldxoffsetscale, orgrowscale, orgcolscale);
                    //dosage = matop.sum2array(dosage, temprow);

                    //file.appenddata(ystart - halfrow + firstiterrow, xstart - halfcol + firstitercol, dosage);
                    max = updatemax(dosage, max);
                    min = updatemin(dosage, min);
                    file.appenddata(ystart, xstart, dosage);


                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            file.setmax(max);
            file.setmin(min);
            file.closefile();

        }
        public void dosageroi(tiffimage tiffimg, ldconfig ldconfig1, ldconfig ldconfig2, int rowoffset, int coloffset, int roirownum, int roicolnum, string tempfolderpath, string beamfilepath, string ldfilepath1, string ldfilepath2, string roimousexy)
        {
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";
            float cnt_scale = displacement / tiffimg.imgps;
            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();
            
            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";//, ldfilepath1 = outpath + tiffimg.imgfilename + "_LE1_2level_roi.LD", ldfilepath2 = outpath + tiffimg.imgfilename + "_LE2_2level_roi.LD";
            beam1.openfile(beamfile1path);
            float h5ps = 1.25f;
            float beamscale = tiffimg.imgps / h5ps;
            int beamrow = (int)(beam1.rownum / beamscale + 0.5), beamcol = (int)(beam1.colnum / beamscale + 0.5);
            if (beamrow % 2 == 0)
            {
                beamrow -= 1;
            }
            if (beamcol % 2 == 0)
            {
                beamcol -= 1;
            }
            int halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = OrgWinRow, orgwindowcol = OrgWinCol;
            beam1.closefile();

            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / ldconfig1.ldps, scalecol = tiffimg.imgps / ldconfig1.ldps;
            int windowrow = (int)(((orgwindowrow) * scalerow) + 0.5), windowcol = (int)(((orgwindowcol) * scalecol) + 0.5);
            int scaleh = (int)(((roirownum) * scalerow) + 0.5), scalew = (int)(((roicolnum) * scalecol) + 0.5);
            int scaleoffset = (int)(scalerow / 2);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = tempfolderpath + tiffimg.imgfilename + "_dosageimg_2level_roi" + roimousexy + ".h5", buffername = "buffertemp";
            int lefttoprow = 0, lefttopcol = 0, firstitertemprow = halfrow;
            int startrow = 0, totalcol = 0;
            float[,] beam = new float[beamrow, beamcol];
            
            float[,] expnddosage;
            float[,] dosage;
            bool lastrow = false, lastcol = false;
            List<byte> tablenum = new List<byte>();
            List<byte> tablenum2 = new List<byte>();
            Tuple<float, float> yinterval1, yinterval2, yinterval;
            int t0;
            float ydistance;
            Dictionary<int, float[,,]> spots1, spots2;
            //record[] records;
            fileio fileprocess = new fileio();
            //fileprocess.startposition = imgstartrow;

            matoperation matop = new matoperation();

            int xstart = 0, xend = orgwindowrow - 1, xend0 = orgwindowrow, ystart = 0, yend = orgwindowrow - 1, yend0 = orgwindowrow, maxorgwindowrow = orgwindowrow;
            float min = float.MaxValue, max = 0;
            //width = 2000;
            if (scaleh < windowrow)
                windowrow = scaleh;
            if (scalew < windowcol)
                windowcol = scalew;
            if (File.Exists(dosagename))
            {
                File.Delete(dosagename);
            }

            file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int tempx = halfcol, tempy = halfrow;
                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;
                
                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(xstart, xend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(xstart, xend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                ydistance = yinterval.Item1 - yinterval.Item2;

                t0 = (int)(Math.Min(yinterval1.Item2, yinterval2.Item2) / cnt_scale);

                beam1.openfile(beamfile1path);
                spots1 = storespots(beam1, tablenum);
                //float[,] spots01 = beam1.readspotdata(1);
                beam1.closefile();
                beam2.openfile(beamfile2path);
                spots2 = storespots(beam2, tablenum2);
                beam2.closefile();

                for (ystart = rowoffset, yend0 = rowoffset + orgwindowrow; ; ystart += orgwindowrow, yend0 += orgwindowrow)
                {
                    if (yend0 >= rowoffset + roirownum)
                    {
                        yend0 = rowoffset + roirownum;
                        lastrow = true;
                    }
                    yend = yend0 - 1;

                    

                    int ldystart = ystart - tempy;
                    if (ldystart < 0)
                        ldystart = 0;
                    
                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));
                    int ldyoffsetscale = (int)((ystart * scalecol)) - (int)((ldystart * scalecol));

                    //int orgrow = yend - ystart;
                    //int orgcol = xend - xstart;
                    int orgrowscale = (int)((yend - ystart + 1) * scalerow);//(int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;
                    int orgcolscale = (int)((xend - xstart + 1) * scalecol); //(int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    //int ldrow = ldyend - ldystart + 1;
                    //int ldcol = ldxend - ldxstart + 1;

                    int ldrowscale = (int)((ldyend - ldystart + 1) * scalerow);//(int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;
                    int ldcolscale = (int)((ldxend - ldxstart + 1) * scalecol); //(int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    expnddosage = new float[ldrowscale + halfrow * 2, ldcolscale + halfcol * 2];

                    int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
                    int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
                    byte[] buffer = new byte[4000];
                    BitArray bits;
                    byte facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart <= ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 4000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 20; ++ldnum)
                                                {
                                                    if (bits[ldnum + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                        
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[ldnum * 8 + facet - 1], s, beamscale);
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 32;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxend > ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 4000) > 0)
                                {

                                    //startbit = 0;
                                    //startbyte = 0;
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 20; ++ldnum)
                                                {
                                                    if (bits[ldnum + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                        
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[ldnum * 8 + facet - 1], s, beamscale);
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 24;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    dosage = matop.cuttingboundary(expnddosage, halfrow + ldyoffsetscale, halfcol + ldxoffsetscale, orgrowscale, orgcolscale);
                    //dosage = matop.sum2array(dosage, temprow);

                    //file.appenddata(ystart - halfrow + firstiterrow, xstart - halfcol + firstitercol, dosage);
                    max = updatemax(dosage, max);
                    min = updatemin(dosage, min);
                    file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);


                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }

            file.setmax(max);
            file.setmin(min);
            file.closefile();

        }

        public void dosage4level(tiffimage tiffimg, ldconfig ldconfig1, ldconfig ldconfig2, string tempfolderpath, string beamfilepath, string ldfilepath1, string ldfilepath2)
        {
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";
            float cnt_scale = displacement / tiffimg.imgps;
            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";//, ldfilepath1 = outpath + tiffimg.imgfilename + "_LE1_4level.LD", ldfilepath2 = outpath + tiffimg.imgfilename + "_LE2_4level.LD";
            beam1.openfile(beamfile1path);
            float h5ps = 1.25f;
            float beamscale = tiffimg.imgps / h5ps;
            int beamrow = (int)(beam1.rownum / beamscale + 0.5), beamcol = (int)(beam1.colnum / beamscale + 0.5);
            if (beamrow % 2 == 0)
            {
                beamrow -= 1;
            }
            if (beamcol % 2 == 0)
            {
                beamcol -= 1;
            }
            int halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = OrgWinRow, orgwindowcol = OrgWinCol;
            beam1.closefile();

            if (height < orgwindowrow)
                orgwindowrow = height;
            if (width < orgwindowcol)
                orgwindowcol = width;

            double scalerow = tiffimg.imgps / ldconfig1.ldps, scalecol = tiffimg.imgps / ldconfig1.ldps;
            int windowrow = (int)(((orgwindowrow) * scalerow) + 0.5), windowcol = (int)(((orgwindowcol) * scalecol) + 0.5);
            int scaleh = (int)(((height) * scalerow) + 0.5), scalew = (int)(((width) * scalecol) + 0.5);
            int scaleoffset = (int)(scalerow / 2);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level.h5", buffername = "buffertemp";
            int lefttoprow = 0, lefttopcol = 0, firstitertemprow = halfrow;
            int startrow = 0, totalcol = 0;
            float[,] beam = new float[beamrow, beamcol];
            
            float[,] expnddosage;
            float[,] dosage;
            bool lastrow = false, lastcol = false;
            List<byte> tablenum = new List<byte>();
            List<byte> tablenum2 = new List<byte>();
            Tuple<float, float> yinterval1, yinterval2, yinterval;
            int t0;
            float ydistance;
            Dictionary<int, float[,,]> spots1, spots2;
            //record[] records;
            fileio fileprocess = new fileio();
            //fileprocess.startposition = imgstartrow;

            matoperation matop = new matoperation();

            int xstart = 0, xend = orgwindowrow - 1, xend0 = orgwindowrow, ystart = 0, yend = orgwindowrow - 1, yend0 = orgwindowrow, maxorgwindowrow = orgwindowrow;
            float min = float.MaxValue, max = 0;
            //width = 2000;
            if (File.Exists(dosagename))
            {
                File.Delete(dosagename);
            }
            file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = 0, xend0 = orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= width)
                {
                    xend0 = width;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int tempx = halfcol, tempy = halfrow;
                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;
                
                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(xstart, xend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(xstart, xend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                ydistance = yinterval.Item1 - yinterval.Item2;

                
                t0 = (int)(Math.Min(yinterval1.Item2, yinterval2.Item2) / cnt_scale);

                beam1.openfile(beamfile1path);
                spots1 = storespots(beam1, tablenum);
                //float[,] spots01 = beam1.readspotdata(1);
                beam1.closefile();
                beam2.openfile(beamfile2path);
                spots2 = storespots(beam2, tablenum2);
                beam2.closefile();

                for (ystart = 0, yend0 = orgwindowrow; ; ystart += orgwindowrow, yend0 += orgwindowrow)
                {
                    if (yend0 >= height)
                    {
                        yend0 = height;
                        lastrow = true;
                    }
                    yend = yend0 - 1;

                    
                    int ldystart = ystart - tempy;
                    if (ldystart < 0)
                        ldystart = 0;
                    
                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));
                    int ldyoffsetscale = (int)((ystart * scalecol)) - (int)((ldystart * scalecol));

                    //int orgrow = yend - ystart;
                    //int orgcol = xend - xstart;

                    int orgrowscale = (int)((yend - ystart + 1) * scalerow);//(int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;
                    int orgcolscale = (int)((xend - xstart + 1) * scalecol); //(int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    //int ldrow = ldyend - ldystart + 1;
                    //int ldcol = ldxend - ldxstart + 1;

                    int ldrowscale = (int)((ldyend - ldystart + 1) * scalerow);//(int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;
                    int ldcolscale = (int)((ldxend - ldxstart + 1) * scalecol); //(int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    expnddosage = new float[ldrowscale + halfrow * 2, ldcolscale + halfcol * 2];

                    int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
                    int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
                    byte[] buffer = new byte[5000];
                    BitArray bits;
                    byte facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart <= ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(5000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 5000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);

                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 40;//8*5=40bits
                                            int startbyte = s * 5;//5bytes
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0 || buffer[startbyte + 4] != 0)//判斷5個bytes裡不為0代表需要疊光點
                                            {
                                                for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                                {
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                       
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)//ldnow有在出光位置檔的位置進行光點疊加
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 32;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxend > ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 5000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 40;
                                            int startbyte = s * 5;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0 || buffer[startbyte + 4] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                                {
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 24;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    dosage = matop.cuttingboundary(expnddosage, halfrow + ldyoffsetscale, halfcol + ldxoffsetscale, orgrowscale, orgcolscale);

                    max = updatemax(dosage, max);
                    min = updatemin(dosage, min);
                    file.appenddata(ystart, xstart, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }

            file.setmax(max);
            file.setmin(min);
            file.closefile();

        }
        public void dosage4levelroi(tiffimage tiffimg, ldconfig ldconfig1, ldconfig ldconfig2, int rowoffset, int coloffset, int roirownum, int roicolnum, string tempfolderpath, string beamfilepath, string ldfilepath1, string ldfilepath2, string roimousexy)
        {
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";
            float cnt_scale = displacement / tiffimg.imgps;
            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";//, ldfilepath1 = outpath + tiffimg.imgfilename + "_LE1_4level_roi.LD", ldfilepath2 = outpath + tiffimg.imgfilename + "_LE2_4level_roi.LD";
            beam1.openfile(beamfile1path);

            float h5ps = 1.25f;
            float beamscale = tiffimg.imgps / h5ps;
            int beamrow = (int)(beam1.rownum / beamscale + 0.5), beamcol = (int)(beam1.colnum / beamscale + 0.5);
            if (beamrow % 2 == 0)
            {
                beamrow -= 1;
            }
            if (beamcol % 2 == 0)
            {
                beamcol -= 1;
            }
            int halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = OrgWinRow, orgwindowcol = OrgWinCol;
            beam1.closefile();

            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / ldconfig1.ldps, scalecol = tiffimg.imgps / ldconfig1.ldps;
            int windowrow = (int)(((orgwindowrow) * scalerow) + 0.5), windowcol = (int)(((orgwindowcol) * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int scaleoffset = (int)(scalerow / 2);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);
            
            string dosagename = tempfolderpath + tiffimg.imgfilename + "_dosageimg_4level_roi" + roimousexy + ".h5", buffername = "buffertemp";
            int lefttoprow = 0, lefttopcol = 0, firstitertemprow = halfrow;
            int startrow = 0, totalcol = 0;
            float[,] beam = new float[beamrow, beamcol];

            float[,] expnddosage;
            float[,] dosage;
            bool lastrow = false, lastcol = false;
            List<byte> tablenum = new List<byte>();
            List<byte> tablenum2 = new List<byte>();
            Tuple<float, float> yinterval1, yinterval2, yinterval;
            int t0;
            float ydistance;
            Dictionary<int, float[,,]> spots1, spots2;
            //record[] records;
            fileio fileprocess = new fileio();
            //fileprocess.startposition = imgstartrow;

            matoperation matop = new matoperation();

            int xstart = 0, xend = orgwindowrow - 1, xend0 = orgwindowrow, ystart = 0, yend = orgwindowrow - 1, yend0 = orgwindowrow, maxorgwindowrow = orgwindowrow;
            float min = float.MaxValue, max = 0;
            //width = 2000;
            if (scaleh < windowrow)
                windowrow = scaleh;
            if (scalew < windowcol)
                windowcol = scalew;
            if (File.Exists(dosagename))
            {
                File.Delete(dosagename);
            }

            file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int tempx = halfcol, tempy = halfrow;
                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;
                
                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(xstart, xend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(xstart, xend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                ydistance = yinterval.Item1 - yinterval.Item2;

                
                t0 = (int)(Math.Min(yinterval1.Item2, yinterval2.Item2) / cnt_scale);

                beam1.openfile(beamfile1path);
                spots1 = storespots(beam1, tablenum);
                beam1.closefile();
                beam2.openfile(beamfile2path);
                spots2 = storespots(beam2, tablenum2);
                beam2.closefile();

                for (ystart = rowoffset, yend0 = rowoffset + orgwindowrow; ; ystart += orgwindowrow, yend0 += orgwindowrow)
                {
                    if (yend0 >= rowoffset + roirownum)
                    {
                        yend0 = rowoffset + roirownum;
                        lastrow = true;
                    }
                    yend = yend0 - 1;

                    int ldystart = ystart - tempy;
                    if (ldystart < 0)
                        ldystart = 0;
                    
                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));
                    int ldyoffsetscale = (int)((ystart * scalecol)) - (int)((ldystart * scalecol));

                    //int orgrow = yend - ystart;
                    //int orgcol = xend - xstart;
                    int orgrowscale = (int)((yend - ystart + 1) * scalerow);
                    int orgcolscale = (int)((xend - xstart + 1) * scalecol);

                    //int ldrow = ldyend - ldystart + 1;
                    //int ldcol = ldxend - ldxstart + 1;

                    int ldrowscale = (int)((ldyend - ldystart + 1) * scalerow);//(int)((ldyend * scalerow) + 0.5) - (int)((ldystart * scalerow) + 0.5) + 1;
                    int ldcolscale = (int)((ldxend - ldxstart + 1) * scalecol); //(int)((ldxend * scalecol) + 0.5) - (int)((ldxstart * scalecol) + 0.5) + 1;

                    expnddosage = new float[ldrowscale + halfrow * 2, ldcolscale + halfcol * 2];

                    int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
                    int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
                    byte[] buffer = new byte[5000];
                    BitArray bits;
                    byte facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart <= ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(5000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 5000) > 0)
                                {
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 40;
                                            int startbyte = s * 5;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0 || buffer[startbyte + 4] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                                {
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 32;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxend > ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(5000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 5000) > 0)
                                {

                                    //startbit = 0;
                                    //startbyte = 0;
                                    if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 40;
                                            int startbyte = s * 5;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0 || buffer[startbyte + 4] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                                {
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (int)(ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        
                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition_imgps(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s, beamscale);
                                                            }
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);

                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                }
                                            }
                                            //startbit += 24;
                                            //startbyte += 4;
                                        }//);

                                    }
                                    if (facet == 8)
                                        facet = 1;
                                    else
                                        ++facet;
                                }
                                else
                                {
                                    break;
                                }
                            }

                        }
                    }

                    dosage = matop.cuttingboundary(expnddosage, halfrow + ldyoffsetscale, halfcol + ldxoffsetscale, orgrowscale, orgcolscale);

                    max = updatemax(dosage, max);
                    min = updatemin(dosage, min);
                    file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }

            file.setmax(max);
            file.setmin(min);
            file.closefile();

        }

        public float updatemax(float[,] d, float max)
        {
            for (int i = 0; i < d.GetLength(0); ++i)
                for (int j = 0; j < d.GetLength(1); ++j)
                {
                    if (d[i, j] > max)
                        max = d[i, j];
                }
            return max;
        }

        public float updatemin(float[,] d, float min)
        {
            for (int i = 0; i < d.GetLength(0); ++i)
                for (int j = 0; j < d.GetLength(1); ++j)
                {
                    if (d[i, j] < min)
                        min = d[i, j];
                }
            return min;
        }
        public Dictionary<int, float[,,]> storespots(hdf5 beam, List<byte> tablenum)
        {
            Dictionary<int, float[,,]> spots = new Dictionary<int, float[,,]>();
            foreach (int i in tablenum)
            {
                for (int j = 0; j < 8; ++j)
                    spots[i * 8 + j] = beam.readspotdata1000(i * 8 + j);//把LD對應哪個facet面下的1000個光點拿出來
            }
            return spots;
        }

        public void Threshold(string inputpath, string tempfolderpath, float threshold, float time)//inputpath是dosagepath
        {

            hdf5 h5 = new hdf5();
            //string filename = outputfolderpath + GuiMain.filename + "_dosageimg_2level.h5";
            h5.openfile(inputpath);
            if (!File.Exists(inputpath))
            {
                MessageBox.Show("不存在Dosage檔案,Threshold程式執行失敗，請先執行Dosage程式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);//picturesize輸入為(GB)需要轉換成Bytes

            int fixedh = (int)((double)maxsize / (double)h5.colnum);//w* h = maxsize, h = maxsize / w

            if (fixedh > h5.rownum)
                fixedh = h5.rownum;
            //string imgname = outputfolderpath + GuiMain.filename  + "_threshold2level_img.h5";

            hdf5 imgfile = new hdf5();
            if (File.Exists(tempfolderpath))
            {
                File.Delete(tempfolderpath);
            }
            imgfile.createfile(tempfolderpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
            for (int row = 0, cnt = 0; row < h5.rownum; row += fixedh, ++cnt)
            {
                if (row + fixedh >= h5.rownum)
                    fixedh = h5.rownum - row;
                float[,] floatimg = h5.readfloatdata(row, 0, fixedh, h5.colnum);
                byte[,] img = new byte[fixedh, h5.colnum];

                for (int i = 0; i < fixedh; ++i)
                    for (int j = 0; j < h5.colnum; ++j)
                    {
                        if (floatimg[i, j] > threshold)
                        {
                            img[i, j] = 1;
                        }



                    }

                imgfile.appenddata(row, 0, img);
            }
            h5.closefile();
            imgfile.closefile();
        }
        public void Threshold_ROI(string inputpath, string tempfolderpath, float threshold, float time)//inputpath為Dosage_ROI圖片,outputpath為輸出Threshold_ROI的路徑
        {
            hdf5 dosageroi = new hdf5();
            hdf5 thresholdroi = new hdf5();
            dosageroi.openfile(inputpath);
            if (File.Exists(tempfolderpath))
            {
                File.Delete(tempfolderpath);
            }
            thresholdroi.createfile(tempfolderpath, dosageroi.rownum, dosageroi.colnum, dosageroi.rownum, dosageroi.colnum, 0);
            float[,] dosage_roi = dosageroi.readfloatdata();
            byte[,] img = new byte[dosageroi.rownum, dosageroi.colnum];
            for (int i = 0; i < dosageroi.rownum; ++i)
                for (int j = 0; j < dosageroi.colnum; ++j)
                {
                    if (dosage_roi[i, j] > threshold)
                        img[i, j] = 1;
                }
            thresholdroi.appenddata(0, 0, img);
            dosageroi.closefile();
            thresholdroi.closefile();
        }
        public void xor_new(string inputpath1, string inputpath2, string tempfolderpath)
        {

            {
                hdf5 h5 = new hdf5();
                hdf5 h5_2 = new hdf5();
                h5.openfile(inputpath1);
                h5_2.openfile(inputpath2);
                if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在原始Pattern及Threshold檔案","提醒",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath1))
                {
                    //MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

                int fixedh = (int)((double)maxsize / (double)h5.colnum);

                if (fixedh > h5.rownum)
                    fixedh = h5.rownum;
                //string imgname = outputfolderpath + GuiMain.filename  + "_threshold2level_img.h5";

                hdf5 imgfile = new hdf5();
                //hdf5 imgfile2 = new hdf5();
                if (File.Exists(tempfolderpath))
                {
                    File.Delete(tempfolderpath);
                }
                imgfile.createfile(tempfolderpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
                //imgfile2.createfile(outputpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
                for (int row = 0, cnt = 0; row < h5.rownum; row += fixedh, ++cnt)
                {
                    if (row + fixedh >= h5.rownum)
                        fixedh = h5.rownum - row;

                    img = new byte[fixedh, h5.colnum];
                    string[,] color = new string[fixedh, h5.colnum];
                    if (h5.gettype() == 1 && h5_2.gettype() == 0)
                    {
                        float[,] floatimg = h5.readfloatdata(row, 0, fixedh, h5.colnum);
                        byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                        for (int i = 0; i < fixedh; ++i)
                            for (int j = 0; j < h5.colnum; ++j)
                            {
                                if (floatimg[i, j] != byteimg2[i, j])
                                {
                                    img[i, j] = 1;
                                }
                            }
                    }
                    if (h5.gettype() == 0 && h5_2.gettype() == 0)//0=byte
                    {
                        byte[,] byteimg = h5.readbytedata(row, 0, fixedh, h5.colnum);
                        byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                        for (int i = 0; i < fixedh; ++i)
                            for (int j = 0; j < h5.colnum; ++j)
                            {
                                if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 0)
                                {
                                    img[i, j] = 0;
                                    color[i, j] = "black";
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 1)
                                {
                                    img[i, j] = 1;
                                    color[i, j] = "white";
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == 1)
                                {
                                    img[i, j] = 2;
                                    color[i, j] = "RoyalBlue";//凹
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == -1)
                                {
                                    img[i, j] = 3;
                                    color[i, j] = "Red";//凸
                                }
                            }
                    }






                    imgfile.appenddata(row, 0, img);
                }
                h5.closefile();
                h5_2.closefile();
                imgfile.closefile();
                //imgfile2.closefile();

            }
        }
        public void xor_xor(string inputpath1, string inputpath2, string tempfolderpath)
        {

            {
                hdf5 h5 = new hdf5();
                hdf5 h5_2 = new hdf5();
                h5.openfile(inputpath1);
                h5_2.openfile(inputpath2);
                if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在原始Pattern及Threshold檔案","提醒",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath1))
                {
                    //MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

                int fixedh = (int)((double)maxsize / (double)h5.colnum);

                if (fixedh > h5.rownum)
                    fixedh = h5.rownum;
                //string imgname = outputfolderpath + GuiMain.filename  + "_threshold2level_img.h5";

                hdf5 imgfile = new hdf5();
                //hdf5 imgfile2 = new hdf5();
                if (File.Exists(tempfolderpath))
                {
                    File.Delete(tempfolderpath);
                }
                imgfile.createfile(tempfolderpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
                //imgfile2.createfile(outputpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
                for (int row = 0, cnt = 0; row < h5.rownum; row += fixedh, ++cnt)
                {
                    if (row + fixedh >= h5.rownum)
                        fixedh = h5.rownum - row;

                    img = new byte[fixedh, h5.colnum];
                    string[,] color = new string[fixedh, h5.colnum];
                    if (h5.gettype() == 1 && h5_2.gettype() == 0)
                    {
                        float[,] floatimg = h5.readfloatdata(row, 0, fixedh, h5.colnum);
                        byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                        for (int i = 0; i < fixedh; ++i)
                            for (int j = 0; j < h5.colnum; ++j)
                            {
                                if (floatimg[i, j] != byteimg2[i, j])
                                {
                                    img[i, j] = 1;
                                }
                            }
                    }
                    if (h5.gettype() == 0 && h5_2.gettype() == 0)//0=byte
                    {
                        byte[,] byteimg = h5.readbytedata(row, 0, fixedh, h5.colnum);
                        byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                        for (int i = 0; i < fixedh; ++i)
                            for (int j = 0; j < h5.colnum; ++j)
                            {
                                if (byteimg[i, j] > 0)
                                {
                                    byteimg[i, j] = 1;
                                }
                                if (byteimg2[i, j] > 0)
                                {
                                    byteimg2[i, j] = 1;
                                }
                                if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 0)
                                {
                                    img[i, j] = 0;
                                    color[i, j] = "black";
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 1)
                                {
                                    img[i, j] = 1;
                                    color[i, j] = "white";
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == 1)
                                {
                                    img[i, j] = 2;
                                    color[i, j] = "RoyalBlue";//凹
                                }
                                else if (byteimg[i, j] - byteimg2[i, j] == -1)
                                {
                                    img[i, j] = 3;
                                    color[i, j] = "Red";//凸
                                }
                            }
                    }






                    imgfile.appenddata(row, 0, img);
                }
                h5.closefile();
                h5_2.closefile();
                imgfile.closefile();
                //imgfile2.closefile();

            }
        }
        public void xor(string inputpath1, string inputpath2, string tempfolderpath)//inputpath1為原始pattern，intputpath2為Thrshold的圖
        {
            hdf5 h5 = new hdf5();
            hdf5 h5_2 = new hdf5();
            h5.openfile(inputpath1);
            h5_2.openfile(inputpath2);
            if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
            {
                //MessageBox.Show("不存在原始Pattern及Threshold檔案","提醒",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            else if (!File.Exists(inputpath1))
            {
                //MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!File.Exists(inputpath2))
            {
                //MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

            int fixedh = (int)((double)maxsize / (double)h5.colnum);

            if (fixedh > h5.rownum)
                fixedh = h5.rownum;
            //string imgname = outputfolderpath + GuiMain.filename  + "_threshold2level_img.h5";

            hdf5 imgfile = new hdf5();
            //hdf5 imgfile2 = new hdf5();
            if (File.Exists(tempfolderpath))
            {
                File.Delete(tempfolderpath);
            }
            imgfile.createfile(tempfolderpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
            //imgfile2.createfile(outputpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
            for (int row = 0, cnt = 0; row < h5.rownum; row += fixedh, ++cnt)
            {
                if (row + fixedh >= h5.rownum)
                    fixedh = h5.rownum - row;

                byte[,] img = new byte[fixedh, h5.colnum];
                if (h5.gettype() == 1 && h5_2.gettype() == 0)
                {
                    float[,] floatimg = h5.readfloatdata(row, 0, fixedh, h5.colnum);
                    byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                    for (int i = 0; i < fixedh; ++i)
                        for (int j = 0; j < h5.colnum; ++j)
                        {
                            if (floatimg[i, j] != byteimg2[i, j])
                            {
                                img[i, j] = 1;
                            }
                        }
                }
                if (h5.gettype() == 0 && h5_2.gettype() == 0)//0=byte
                {
                    byte[,] byteimg = h5.readbytedata(row, 0, fixedh, h5.colnum);
                    byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                    for (int i = 0; i < fixedh; ++i)
                        for (int j = 0; j < h5.colnum; ++j)
                        {
                            if (byteimg[i, j] != byteimg2[i, j])
                            {
                                img[i, j] = 1;
                            }
                        }
                }






                imgfile.appenddata(row, 0, img);
            }
            h5.closefile();
            h5_2.closefile();
            imgfile.closefile();
            //imgfile2.closefile();

        }
        public void xor_ROI(string inputpath1, string inputpath2, string tempfolderpath)//inputpath1為原始pattern，intputpath2為Thrshold的圖
        {
            hdf5 orgpat = new hdf5();
            hdf5 thdpat = new hdf5();
            hdf5 xorpat = new hdf5();
            orgpat.openfile(inputpath1);
            thdpat.openfile(inputpath2);
            byte[,] orgmatrix = new byte[orgpat.rownum, orgpat.colnum];
            byte[,] thdmatrxi = new byte[thdpat.rownum, thdpat.colnum];
            byte[,] xormatrix = new byte[orgpat.rownum, orgpat.colnum];
            if (File.Exists(tempfolderpath))
            {
                File.Delete(tempfolderpath);
            }
            xorpat.createfile(tempfolderpath, thdpat.rownum, thdpat.colnum, thdpat.rownum, thdpat.colnum, 0);
            orgmatrix = orgpat.readbytedata();
            thdmatrxi = thdpat.readbytedata();
            for (int i = 0; i < orgpat.rownum; ++i)
                for (int j = 0; j < orgpat.colnum; ++j)
                {
                    if (orgmatrix[i, j] != thdmatrxi[i, j])
                    {
                        xormatrix[i, j] = 1;
                    }
                }
            xorpat.appenddata(0, 0, xormatrix);
            orgpat.closefile();
            thdpat.closefile();
            xorpat.closefile();
        }
        public void xor_ROI_new(string inputpath1, string inputpath2, string tempfolderpath)//inputpath1為原始pattern，intputpath2為Thrshold的圖
        {
            hdf5 orgpat = new hdf5();
            hdf5 thdpat = new hdf5();
            hdf5 xorpat = new hdf5();
            orgpat.openfile(inputpath1);
            thdpat.openfile(inputpath2);
            byte[,] orgmatrix = new byte[orgpat.rownum, orgpat.colnum];
            byte[,] thdmatrxi = new byte[thdpat.rownum, thdpat.colnum];
            byte[,] xormatrix = new byte[orgpat.rownum, orgpat.colnum];
            if (File.Exists(tempfolderpath))
            {
                File.Delete(tempfolderpath);
            }
            xorpat.createfile(tempfolderpath, thdpat.rownum, thdpat.colnum, thdpat.rownum, thdpat.colnum, 0);
            orgmatrix = orgpat.readbytedata();
            thdmatrxi = thdpat.readbytedata();
            for (int i = 0; i < orgpat.rownum; ++i)
                for (int j = 0; j < orgpat.colnum; ++j)
                {
                    if (orgmatrix[i, j] - thdmatrxi[i, j] == 0 && orgmatrix[i, j] == 0)
                    {
                        xormatrix[i, j] = 0;
                    }
                    else if (orgmatrix[i, j] - thdmatrxi[i, j] == 0 && orgmatrix[i, j] == 1)
                    {
                        xormatrix[i, j] = 1;
                    }
                    else if (orgmatrix[i, j] - thdmatrxi[i, j] == 1)
                    {
                        xormatrix[i, j] = 2;
                    }
                    else if (orgmatrix[i, j] - thdmatrxi[i, j] == -1)
                    {
                        xormatrix[i, j] = 3;
                    }
                }
            xorpat.appenddata(0, 0, xormatrix);
            orgpat.closefile();
            thdpat.closefile();
            xorpat.closefile();
        }
        public void org_roi(int rowoffset, int coloffset, int rownum, int colnum, string h5path, string beamfilepath, float imgps, string outputpath)
        {
            hdf5 h5 = new hdf5();
            h5.openfile(h5path);
            byte[,] h5image;

            int ystart = rowoffset, xstart = coloffset, yend = rowoffset + rownum - 1, xend = coloffset + colnum - 1;
            hdf5 beam1 = new hdf5();
            string beamfile1path = beamfilepath + "spotarrays100le1_125.h5", beamfile2path = beamfilepath + "spotarrays100le2_125.h5";
            beam1.openfile(beamfile1path);
            int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = 3000, orgwindowcol = 3000;
            beam1.closefile();
            int tempx = (int)Math.Ceiling(halfcol * 1.25 / imgps), tempy = (int)Math.Ceiling(halfrow * 1.25 / imgps);
            //int xoffset, yoffset;
            //int ldystart = ystart - tempy;
            //if (ldystart < 0)
            //    ldystart = 0;
            //int ldxstart = xstart - tempx;
            //if (ldxstart < 0)
            //    ldxstart = 0;
            //int ldyend = yend + tempy;
            //if (ldyend >= tiffimg.height)
            //    ldyend = tiffimg.height - 1;
            //int ldxend = xend + tempx;
            //if (ldxend >= tiffimg.width)
            //    ldxend = tiffimg.width - 1;
            //yoffset = ystart - ldystart;
            //xoffset = xstart - ldxstart;
            //int newcolnum = ldxend - ldxstart + 1;
            //int newrownum = ldyend - ldystart + 1;
            h5image = h5.readbytedata(ystart, xstart, rownum, colnum);
            if (File.Exists(outputpath))
            {
                File.Delete(outputpath);
            }
            h5.createfile(outputpath, rownum, colnum, rownum, colnum, 0);
            h5.appenddata(0, 0, h5image);
            h5.closefile();
        }
        public void xor_new_float(string inputpath1, string inputpath2, string tempfolderpath)
        {

            {
                hdf5 h5 = new hdf5();
                hdf5 h5_2 = new hdf5();
                h5.openfile(inputpath1);
                h5_2.openfile(inputpath2);
                if (!File.Exists(inputpath1) && !File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在原始Pattern及Threshold檔案","提醒",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath1))
                {
                    //MessageBox.Show("不存在原始Pattern檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (!File.Exists(inputpath2))
                {
                    //MessageBox.Show("不存在Threshold檔案", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                long maxsize = (long)((double)picturesize * 1024 * 1024 * 1024);

                int fixedh = (int)((double)maxsize / (double)h5.colnum);

                if (fixedh > h5.rownum)
                    fixedh = h5.rownum;
                //string imgname = outputfolderpath + GuiMain.filename  + "_threshold2level_img.h5";

                hdf5 imgfile = new hdf5();
                //hdf5 imgfile2 = new hdf5();
                if (File.Exists(tempfolderpath))
                {
                    File.Delete(tempfolderpath);
                }
                imgfile.createfile(tempfolderpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 1);
                //imgfile2.createfile(outputpath, h5.rownum, h5.colnum, fixedh, h5.colnum, 0);
                for (int row = 0, cnt = 0; row < h5.rownum; row += fixedh, ++cnt)
                {
                    if (row + fixedh >= h5.rownum)
                        fixedh = h5.rownum - row;

                    float[,] img = new float[fixedh, h5.colnum];
                    string[,] color = new string[fixedh, h5.colnum];
                    float epsion = 0.001f;
                    if (h5.gettype() == 1 && h5_2.gettype() == 1)
                    {
                        float[,] floatimg = h5.readfloatdata(row, 0, fixedh, h5.colnum);
                        //byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                        float[,] floatimg2 = h5_2.readfloatdata(row, 0, fixedh, h5.colnum);
                        for (int i = 0; i < fixedh; ++i)
                            for (int j = 0; j < h5.colnum; ++j)
                            {


                                img[i, j] = floatimg[i, j] - floatimg2[i, j];


                            }
                    }
                    //if (h5.gettype() == 0 && h5_2.gettype() == 0)//0=byte
                    //{
                    //    byte[,] byteimg = h5.readbytedata(row, 0, fixedh, h5.colnum);
                    //    byte[,] byteimg2 = h5_2.readbytedata(row, 0, fixedh, h5.colnum);
                    //    for (int i = 0; i < fixedh; ++i)
                    //        for (int j = 0; j < h5.colnum; ++j)
                    //        {
                    //            if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 0)
                    //            {
                    //                img[i, j] = 0;
                    //                color[i, j] = "black";
                    //            }
                    //            else if (byteimg[i, j] - byteimg2[i, j] == 0 && byteimg[i, j] == 1)
                    //            {
                    //                img[i, j] = 1;
                    //                color[i, j] = "white";
                    //            }
                    //            else if (byteimg[i, j] - byteimg2[i, j] == 1)
                    //            {
                    //                img[i, j] = 2;
                    //                color[i, j] = "RoyalBlue";//凹
                    //            }
                    //            else if (byteimg[i, j] - byteimg2[i, j] == -1)
                    //            {
                    //                img[i, j] = 3;
                    //                color[i, j] = "Red";//凸
                    //            }
                    //        }
                    //}






                    imgfile.appenddata(row, 0, img);
                }
                h5.closefile();
                h5_2.closefile();
                imgfile.closefile();
                //imgfile2.closefile();

            }
        }

        public void updateldimg(string orgldimg,string temppath,string beamfilepath, int xlefttop, int ylefttop, int xrightbottom, int yrightbottom)
        {
            string optroiname = temppath + "optimization\\" + "optld_" + xlefttop.ToString() + "_" + ylefttop.ToString() + "_" + xrightbottom.ToString() + "_" + yrightbottom.ToString()+".h5";

            hdf5 orgimg = new hdf5();
            hdf5 roiimg = new hdf5();

            orgimg.openfile(orgldimg);
            roiimg.openfile(optroiname);
            byte[,] roi = roiimg.readbytedata();

            hdf5 beam1 = new hdf5();//, beam2 = new hdf5();
            float scale = GuiMain.tiffimg.imgps / 0.125f;
            beam1.openfile(beamfilepath);
            int beamrow = beam1.rownum;
            int beamcol = beam1.colnum;
            int halfrow = beamrow / 2;
            int halfcol = beamcol / 2;
            //offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            beam1.closefile();
            //beamrow = 21; beamcol = 41; halfrow = beamrow / 2; halfcol = beamcol / 2;
            //threshold = 15;
            int tempx = (int)(Math.Ceiling(halfcol / scale));
            int tempy = (int)(Math.Ceiling(halfrow / scale));

            ylefttop -= tempy;
            xlefttop -= tempx;

            if (ylefttop < 0)
            {
                ylefttop = 0;
            }
            
            if (xlefttop < 0)
            {
                xlefttop = 0;
            }

            orgimg.appenddata(ylefttop, xlefttop, roi);

            orgimg.closefile();
            roiimg.closefile();
        }
    }
}
