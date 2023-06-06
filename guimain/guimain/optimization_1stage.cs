using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Data;
using System.Collections;
using System.Threading;
using System.Windows.Forms;
using HDF5DotNet;

namespace guimain
{
    //public class ldset
    //{
    //    public List<beam>[,] array;
    //    public int ldxstart, ldystart, ldxend, ldyend;
    //    public int leftx, rightx, lefty, righty;
    //    public int orgrownum, orgcolnum;
    //    public int xoffsetorg, yoffsetorg, orgxnum, orgynum;
    //}

    //public class beam
    //{
    //    public byte eon;
    //    public ushort spotorder;
    //    public ushort spotnum;
    //}

    //public class dosageset
    //{
    //    public float[,] array;
    //    public int xstart, ystart, xend, yend;
    //    public int xf1, yf1, xnum, ynum;
    //}

    //public class targetset
    //{
    //    public byte[,] array;
    //    public int xstart, ystart, xend, yend;
    //}

    //public class lossset
    //{
    //    public int lossvalue;
    //    public byte[,] zloss;
    //    //public byte[,] optergion;
    //    public List<int> lossposx;
    //    public List<int> lossposy;
    //    public int lossinsidetarget;
    //    public int lossroi;
    //}

    //public class zlossset
    //{
    //    public byte[,] zloss;
    //    public int loss;
    //}

    //public class ranloss : IComparable<ranloss>
    //{
    //    public int loss;
    //    public int idx;
    //    public int CompareTo(ranloss other)
    //    {
    //        //ranloss x1 = (ranloss)x;
    //        //ranloss y2 = (ranloss)y;

    //        if (this.loss > other.loss)
    //            return 1;

    //        if (this.loss < other.loss)
    //            return -1;
    //        else
    //            return 0;
    //    }
    //}

    //public class ranmaxcc : IComparable<ranmaxcc>
    //{
    //    public int maxcc;
    //    public int optcc;
    //    public int idx;
    //    public int CompareTo(ranmaxcc other)
    //    {
    //        if (this.optcc > other.optcc)
    //            return 1;
    //        else if (this.optcc < other.optcc)
    //            return -1;
    //        else if (this.maxcc > other.maxcc)
    //            return 1;
    //        else
    //            return 0;
    //    }
    //}

    //public class rancomp : Comparer<ranloss>
    //{
    //    public override int Compare(ranloss x, ranloss y)
    //    {
    //        ranloss x1 = (ranloss)x;
    //        ranloss y2 = (ranloss)y;

    //        return x.CompareTo(y);
    //    }
    //}

    //public class rancompcc : Comparer<ranmaxcc>
    //{
    //    public override int Compare(ranmaxcc x, ranmaxcc y)
    //    {
    //        ranmaxcc x1 = (ranmaxcc)x;
    //        ranmaxcc y2 = (ranmaxcc)y;

    //        return x.CompareTo(y);
    //    }
    //}

    //public int rancomp0<ranloss>()

    public class optimization_1stage
    {
        //List<beam>[,] beamarray;

        public tiffimage tiffimg;
        public ldconfig ldconfig1;
        public ldconfig ldconfig2;
        public string result2;
        public string ldfilepath1, ldfilepath2, ldfilenewpath1, ldfilenewpath2, outputpath;
        public string beamfilepath, beamfile1path, beamfile2path;
        public float threshold;
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public opt_parameter opt_para = new opt_parameter();
        float scale;
        int scaleoffset;
        int maxcc_criterion, totalnum_criterion;
        int roirownum, roicolnum;
        int beamrow, beamcol, halfrow, halfcol;
        int tempx, tempy, tempxscale, tempyscale;//temp = (ceil)((double)beamsize/scale) tempscale = temp*scale
        int xexp, yexp, xexpscale, yexpscale;
        int expandscale = 2, expandldscale = 1;
        int iniconstantiter;
        int iniepsilon;
        double iniproportion;
        double initabuproportion;
        int iniomaxiter;
        int[] optidx = new int[4];
        string roi_xy;
        string optoutputfolder;
        float displacement;
        int xlefttop;
        int ylefttop;
        int xrightbottom;
        int yrightbottom;
        public int lossweight;
       
        public optimization_1stage(tiffimage tiffimg, ldconfig ldconfig1, ldconfig ldconfig2, string outputpath, string beamfilepath, string ldfilepath1, string ldfilepath2, string roi_xy, float threshold, float displacement, int maxcc = int.MaxValue, int totalnum = int.MaxValue)
        {
            this.tiffimg = tiffimg;
            this.ldconfig1 = ldconfig1;
            this.ldconfig2 = ldconfig2;
            this.outputpath = outputpath;
            this.beamfilepath = beamfilepath;
            this.threshold = threshold;
            this.displacement = displacement;
            //ldfilepath1 = outputpath + tiffimg.imgfilename + "_LE1_2level.LD";
            //ldfilepath2 = outputpath + tiffimg.imgfilename + "_LE2_2level.LD";
            this.ldfilepath1 = ldfilepath1;
            this.ldfilepath2 = ldfilepath2;
            this.roi_xy = roi_xy;
            optoutputfolder = outputpath + "optimization\\";
            if (!Directory.Exists(optoutputfolder))
                Directory.CreateDirectory(optoutputfolder);

            iniomaxiter = Convert.ToInt32(ini.IniReadValue("Section", "Omaxiter", filenameini));
            iniproportion = Convert.ToDouble(ini.IniReadValue("Section", "Propotion", filenameini));
            iniconstantiter = Convert.ToInt32(ini.IniReadValue("Section", "Constantiteration", filenameini));
            iniepsilon = Convert.ToInt32(ini.IniReadValue("Section", "Epsilon", filenameini));
            initabuproportion = Convert.ToDouble(ini.IniReadValue("Section", "tabuproportion", filenameini));
            lossweight = Convert.ToInt16(ini.IniReadValue("Section", "LossWeight", filenameini));
            if (roi_xy != "")
            {
                ldfilenewpath1 = outputpath + tiffimg.imgfilename + roi_xy + "_LE1_1stage_2level_opt.LD";
                ldfilenewpath2 = outputpath + tiffimg.imgfilename + roi_xy + "_LE2_1stage_2level_opt.LD";

            }
            else
            {
                ldfilenewpath1 = outputpath + tiffimg.imgfilename + "_LE1_1stage_2level_opt.LD";
                ldfilenewpath2 = outputpath + tiffimg.imgfilename + "_LE2_1stage_2level_opt.LD";
            }

            beamfile1path = beamfilepath + "spotarrays100le1_0125.h5";
            beamfile2path = beamfilepath + "spotarrays100le2_0125.h5";
            maxcc_criterion = maxcc;
            totalnum_criterion = totalnum;
        }

        

        //public int[] optimize_total(int xlefttop, int ylefttop, int xrightbottom, int yrightbottom)
        //{
           
        //    //DateTime time_start = DateTime.Now;//計時開始 取得目前時間
        //    this.xlefttop = xlefttop;
        //    this.ylefttop = ylefttop;
        //    this.xrightbottom = xrightbottom;
        //    this.yrightbottom = yrightbottom;
        //    hdf5 verifyimg = new hdf5();
        //    roirownum = yrightbottom - ylefttop + 1;
        //    roicolnum = xrightbottom - xlefttop + 1;
        //    scale = tiffimg.imgps / 0.125f;
        //    scaleoffset = (int)(scale / 2);
        //    hdf5 beam1 = new hdf5(), beam2 = new hdf5();
        //    beam1.openfile(this.beamfile1path);
        //    beamrow = beam1.rownum;
        //    beamcol = beam1.colnum;
        //    halfrow = beamrow / 2;
        //    halfcol = beamcol / 2;
        //    //offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
        //    beam1.closefile();
        //    //beamrow = 21; beamcol = 41; halfrow = beamrow / 2; halfcol = beamcol / 2;
        //    //threshold = 15;
        //    tempx = (int)(Math.Ceiling(halfcol / scale));
        //    tempy = (int)(Math.Ceiling(halfrow / scale));
        //    opt_para.tiffimg = tiffimg;
        //    opt_para.ldconfig1 = ldconfig1;
        //    opt_para.ldconfig2 = ldconfig2;
        //    opt_para.ldfilepath1 = ldfilepath1;
        //    opt_para.ldfilepath2 = ldfilepath2;
        //    opt_para.ldfilenewpath1 = ldfilenewpath1;
        //    opt_para.ldfilenewpath2 = ldfilenewpath2;
        //    opt_para.outputpath = outputpath;
        //    opt_para.beamfilepath = beamfilepath;
        //    opt_para.beamfile1path = beamfile1path;
        //    opt_para.beamfile2path = beamfile2path;
        //    opt_para.threshold = threshold;
        //    opt_para.filenameini = filenameini;
        //    opt_para.tempx = tempx;
        //    opt_para.tempy = tempy;
        //    opt_para.tempxscale = tempxscale;
        //    opt_para.tempyscale = tempyscale;
        //    opt_para.scale = scale;
        //    opt_para.scaleoffset = scaleoffset;
        //    opt_para.maxcc_criterion = maxcc_criterion;
        //    opt_para.totalnum_criterion = totalnum_criterion;
        //    opt_para.beamrow = beamrow;
        //    opt_para.beamcol = beamcol;
        //    opt_para.halfrow = halfrow;
        //    opt_para.halfcol = halfcol;
        //    opt_para.xexp = xexp;
        //    opt_para.yexp = yexp;
        //    opt_para.xexpscale = xexpscale;
        //    opt_para.yexpscale = yexpscale;
        //    opt_para.expandscale = expandscale;
        //    opt_para.expandldscale = expandldscale;
        //    opt_para.iniconstantiter = iniconstantiter;
        //    opt_para.iniepsilon = iniepsilon;
        //    opt_para.iniproportion = iniproportion;
        //    opt_para.initabuproportion = initabuproportion;
        //    opt_para.iniomaxiter = iniomaxiter;
        //    opt_para.optidx = optidx;
        //    opt_para.roi_xy = roi_xy;
        //    opt_para.optoutputfolder = optoutputfolder;
        //    opt_para.xlefttop = xlefttop;
        //    opt_para.ylefttop = ylefttop;
        //    opt_para.xrightbottom = xrightbottom;
        //    opt_para.yrightbottom = yrightbottom;
        //    optimization_funcation opfunction = new optimization_funcation();



        //    ldset tld = opfunction.readld(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,1倍

        //    dosageset d = opfunction.readdosage(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld,2倍
        //    //float[,] d = new float[0, 0];
        //    targetset target = opfunction.readtarget(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,2倍
        //    ldset tldfull = opfunction.readld(xlefttop, ylefttop, xrightbottom, yrightbottom, expandscale);//resolution = 0.125um
        //    string optoutputfolder = outputpath + "optimization\\";
        //    if (!Directory.Exists(optoutputfolder))
        //        Directory.CreateDirectory(optoutputfolder);
        //    //verifyimg.writearray(tldfull.array, optoutputfolder + "orgld_1stage");
        //    verifyimg.writearray(d.array, optoutputfolder + "orgd_1stage");
        //    verifyimg.writearray(target.array, optoutputfolder + "targetroi_1stage");
        //    //byte[,] target = new byte[0, 0];
        //    //verifyimg.writearraytest(beamarray, optoutputfolder + "dospos");
        //    //verifyimg.writearraytest(tld.array, optoutputfolder + "ldpos");
        //    //int[,] zloss = new int[0, 0];//use d,threshold
        //    lossset loss = computeloss(d, target);//first loss value of zloss
        //    List<int> losshistory = new List<int>();
        //    List<int> curlosshistory = new List<int>();
        //    List<int> bestlosshistory = new List<int>();
        //    List<int> difflosshistory = new List<int>();
        //    int[] result = new int[0];

        //    if (loss.lossvalue > totalnum_criterion)
        //    {
        //        losshistory.Add(loss.lossvalue);
        //        int rangex = 4, rangey = 2, rangescale = 0, omaxiter = iniomaxiter, totalcnt = 1;
        //        float proportion = (float)iniproportion, tabuproportion = (float)initabuproportion;
        //        bool optimized = true, stop = false;
        //        int nonoptx = 0, nonopty = 0, distancex, distancey;
        //        byte[,] target0, zloss0;
        //        float[,] dorg;
        //        List<beam>[,] ldnow;
        //        int ldxoffset = (int)((tld.ldxstart - target.xstart) * scale);
        //        int ldyoffset = (int)((tld.ldystart - target.ystart) * scale);

        //        //int loss = computeloss(d, target);
        //        int k, f, x00, y00, x10, y10, lossorg, x0, y0, x1, y1, xoffset, yoffset;

        //        List<byte> tablenum1 = ldconfig1.checkrange(tld.ldxstart, tld.ldxend);
        //        List<byte> tablenum2 = ldconfig2.checkrange(tld.ldxstart, tld.ldxend);
        //        //List<beam>[,] beamarray0;
        //        //return result;
        //        Dictionary<int, float[,,]> spots;
        //        zlossset zlossset0;
        //        //string beamfile1path = beamfilepath + "spotarrays100le1.h5", beamfile2path = beamfilepath + "spotarrays100le2.h5";
        //        beam1.openfile(beamfile1path);
        //        beam2.openfile(beamfile2path);
        //        spots = storespots32(beam1, beam2, tablenum1, tablenum2);
        //        beam1.closefile();
        //        beam2.closefile();
        //        //string testfolder = outputpath + "test\\", dstr = testfolder + "dorg", tstr = testfolder + "target0", zstr = testfolder + "zloss0", ldstr = testfolder + "ldnow";
        //        int iternum = 1;
        //        int updatecnt = 0, updatetotalcnt = 0;

        //        while (!stop && loss.lossposx.Count != 0)
        //        {
        //            for (int pos = 0; pos < loss.lossposx.Count(); ++pos)
        //            {
        //                k = loss.lossposy[pos];
        //                f = loss.lossposx[pos];

        //                if (loss.zloss[k, f] == 1)
        //                {
        //                    if (optimized == false)//這點十分難優化
        //                    {
        //                        distancey = Math.Abs(nonopty - k);//nonopty還沒優化前的畫素
        //                        distancex = Math.Abs(nonoptx - f);
        //                        if (distancex <= rangex && distancey <= rangey)//在設定範圍內，就會跳掉
        //                        {
        //                            continue;
        //                        }
        //                    }
        //                    optimized = false;

        //                    x00 = f - halfcol * (2 + rangescale);
        //                    y00 = k - halfrow * (2 + rangescale);
        //                    if (x00 < 0)
        //                        x00 = 0;

        //                    if (y00 < 0)
        //                        y00 = 0;

        //                    x10 = f + halfcol * (2 + rangescale);
        //                    y10 = k + halfrow * (2 + rangescale);
        //                    if (x10 >= target.array.GetLength(1))
        //                        x10 = target.array.GetLength(1) - 1;

        //                    if (y10 >= target.array.GetLength(0))
        //                        y10 = target.array.GetLength(0) - 1;


        //                    target0 = parttarget(target.array, x00, y00, x10, y10);
        //                    //lossorg = computepartloss(loss.zloss, x00, y00, x10, y10);
        //                    dorg = partdosage(d.array, x00, y00, x10, y10);
        //                    //beamarray0 = partbeamarray(x00, y00, x10, y10);
        //                    zlossset0 = partzloss(dorg, target0);
        //                    //verifyimg.writearray(dorg, dstr);
        //                    //verifyimg.writearray(target0, tstr);
        //                    //verifyimg.writearray(zlossset0.zloss, zstr);

        //                    x0 = f - halfcol * (1 + rangescale) - ldxoffset;
        //                    y0 = k - halfrow * (1 + rangescale) - ldyoffset;
        //                    if (x0 < 0)
        //                        x0 = 0;

        //                    if (y0 < 0)
        //                        y0 = 0;

        //                    x1 = f + halfcol * (1 + rangescale) - ldxoffset;
        //                    y1 = k + halfrow * (1 + rangescale) - ldyoffset;
        //                    if (x1 >= tld.array.GetLength(1))
        //                        x1 = tld.array.GetLength(1) - 1;

        //                    if (y1 >= tld.array.GetLength(0))
        //                        y1 = tld.array.GetLength(0) - 1;

        //                    //xoffset = x0 - x00;
        //                    //yoffset = y0 - y00;

        //                    ldnow = partld(tld.array, x0, y0, x1, y1);
        //                    //verifyimg.writearray(ldnow, ldstr);



        //                    List<int> row0 = new List<int>();
        //                    List<int> col0 = new List<int>();
        //                    List<byte> code0 = new List<byte>();
        //                    List<byte> order0 = new List<byte>();
        //                    List<ushort> spotorder0 = new List<ushort>();
        //                    List<ushort> spotnum0 = new List<ushort>();

        //                    for (int i = 0; i < ldnow.GetLength(0); ++i)
        //                    {
        //                        for (int j = 0; j < ldnow.GetLength(1); ++j)
        //                        {
        //                            if (ldnow[i, j] != null)
        //                            {
        //                                for (byte k1 = 0; k1 < ldnow[i, j].Count; ++k1)
        //                                {
        //                                    row0.Add(i + halfrow);
        //                                    col0.Add(j + halfcol);
        //                                    code0.Add(ldnow[i, j][k1].eon);//對應光點能量大小
        //                                    order0.Add(k1);//記錄第幾次被打到
        //                                    spotorder0.Add(ldnow[i, j][k1].spotorder);//記錄第幾面第幾個ld
        //                                    spotnum0.Add(ldnow[i, j][k1].spotnum);//1~1000哪個
        //                                }
        //                            }
        //                        }
        //                    }

        //                    if (code0.Count == 0)
        //                        continue;

        //                    int[] row = row0.ToArray();
        //                    int[] col = col0.ToArray();
        //                    byte[] code = code0.ToArray();
        //                    byte[] order = order0.ToArray();
        //                    ushort[] spotorder = spotorder0.ToArray();
        //                    ushort[] spotnum = spotnum0.ToArray();

        //                    List<beam>[,] bestld = new List<beam>[ldnow.GetLength(0), ldnow.GetLength(1)];
        //                    copyld(ldnow, bestld);
        //                    int bestloss = zlossset0.loss;//loss點數
        //                    byte[] bestcode = new byte[code.Length];
        //                    Array.Copy(code, bestcode, code.Length);
        //                    float[,] bestd = new float[dorg.GetLength(0), dorg.GetLength(1)];
        //                    Array.Copy(dorg, bestd, dorg.Length);
        //                    byte[,] bestzloss = new byte[zlossset0.zloss.GetLength(0), zlossset0.zloss.GetLength(1)];
        //                    Array.Copy(zlossset0.zloss, bestzloss, zlossset0.zloss.Length);
        //                    Queue<int> tabu = new Queue<int>();
        //                    int oiter = 0;
        //                    int ocnt = 0;
        //                    int maxsize = (int)(tabuproportion * code.Length);
        //                    int maxiter = (int)(proportion * code.Length);//鄰域的比例
        //                    bool flag;
        //                    int[] losslist;
        //                    //MinHeap<ranloss> losslist;//pop時,抓最小值的Queue
        //                    //ranloss[] curranloss = new ranloss[maxiter];
        //                    //for (int i = 0; i < curranloss.Length; ++i)
        //                    //{
        //                    //    curranloss[i] = new ranloss();
        //                    //}
        //                    int[] ranlist;
        //                    //float[,] curd;
        //                    int idxstep, ranchoose = 0, losschoose = 0;
        //                    byte newcode;

        //                    //updatecnt = 0;

        //                    //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                    //writenewldfile(tld);
        //                    //dosageset d02 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                    //lossset loss_opt2 = computeloss(d02, target);//first loss value of zloss
        //                    //updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                    //lossset loss02 = computeloss(d, target);
        //                    //if (loss_opt2.lossvalue != loss02.lossvalue)
        //                    //    ;

        //                    while (oiter != omaxiter)//連續幾次都沒優化，就會停止
        //                    {
        //                        //++ocnt;
        //                        ++oiter;
        //                        if (bestloss == 0)
        //                            break;


        //                        //int levelnum = 2, levelminusone= levelnum-1;
        //                        losslist = new int[maxiter];
        //                        //losslist = new MinHeap<ranloss>(new rancomp(), maxiter);
        //                        ranlist = GenerateRandom(maxiter, 0, code.Length);//從0~code.length-1中取出maxiter個
        //                        flag = false;
        //                        //curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
        //                        //List<int> ranlist = GenerateRandom(maxiter, 0, code.Length);
        //                        //for (int iter = 0; iter < maxiter; ++iter)
        //                        Parallel.For(0, maxiter, iter =>
        //                        {
        //                            int ran = ranlist[iter];
        //                            float[,] curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
        //                            Array.Copy(dorg, curd, dorg.Length);

        //                            if (code[ran] == 1)
        //                            {
        //                                subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
        //                            }
        //                            else
        //                            {
        //                                addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
        //                            }

        //                            int orgcurloss = computeorgcurloss(zlossset0.zloss, row[ran], col[ran]);
        //                            int curloss = computecurloss(target0, curd, row[ran], col[ran]);

        //                            //losslist[iter] = zlossset0.loss + curloss - orgcurloss;
        //                            //curranloss[iter].loss = zlossset0.loss + curloss - orgcurloss;
        //                            //curranloss[iter].idx = iter;
        //                            //losslist.Add(curranloss[iter]);
        //                            losslist[iter] = zlossset0.loss + curloss - orgcurloss;
        //                        });

        //                        int[] indices = new int[maxiter];
        //                        for (int i = 0; i < maxiter; i++) indices[i] = i;
        //                        Array.Sort(losslist, indices);

        //                        for (int idx = 0; idx < maxiter; ++idx)
        //                        {
        //                            if (tabu.Contains(ranlist[indices[idx]]) == false)
        //                            {
        //                                ++ocnt;
        //                                ranchoose = ranlist[indices[idx]];
        //                                losschoose = losslist[idx];
        //                                tabu.Enqueue(ranlist[indices[idx]]);
        //                                flag = true;
        //                                break;
        //                            }
        //                            else if (losslist[idx] < bestloss)
        //                            {
        //                                ++ocnt;
        //                                ranchoose = ranlist[indices[idx]];
        //                                losschoose = losslist[idx];
        //                                tabu.Enqueue(ranlist[indices[idx]]);
        //                                flag = true;
        //                                break;
        //                            }
        //                        }

        //                        //ranloss ranlosschoose = new ranloss();
        //                        //for (int idx = 0; idx < maxiter; ++idx)
        //                        //{
        //                        //    ranlosschoose = losslist.GetMin();//loss總數最小的鄰居，以及idx(index)

        //                        //    if (tabu.Contains(ranlist[ranlosschoose.idx]) == false)
        //                        //    {
        //                        //        ++ocnt;//記錄Tabu list有幾個元素
        //                        //        ranchoose = ranlist[ranlosschoose.idx];//ranlist當下的值
        //                        //        losschoose = ranlosschoose.loss;//當下的loss總數
        //                        //        tabu.Enqueue(ranlist[ranlosschoose.idx]);
        //                        //        flag = true;
        //                        //        break;
        //                        //    }
        //                        //    else if (ranlosschoose.loss < bestloss)
        //                        //    {
        //                        //        ++ocnt;
        //                        //        ranchoose = ranlist[ranlosschoose.idx];
        //                        //        losschoose = ranlosschoose.loss;
        //                        //        tabu.Enqueue(ranlist[ranlosschoose.idx]);
        //                        //        flag = true;
        //                        //        break;
        //                        //    }

        //                        //    losslist.ExtractDominating();//把最小的移除

        //                        //}

        //                        if (ocnt > maxsize)//maxsize就是tabulist的最大長度
        //                        {
        //                            tabu.Dequeue();
        //                            --ocnt;
        //                        }

        //                        //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                        //writenewldfile(tld);
        //                        //dosageset d04 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                        //lossset loss_opt4 = computeloss(d04, target);//first loss value of zloss
        //                        //updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                        //lossset loss04 = computeloss(d, target);
        //                        //if (loss_opt4.lossvalue != loss04.lossvalue)
        //                        //    ;

        //                        if (flag == true)
        //                        {
        //                            float[,] curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
        //                            Array.Copy(dorg, curd, dorg.Length);
        //                            int ran = ranchoose;
        //                            if (code[ran] == 1)
        //                            {
        //                                newcode = 0;
        //                                subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
        //                            }
        //                            else
        //                            {
        //                                newcode = 1;
        //                                addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
        //                            }

        //                            code[ran] = newcode;

        //                            Array.Copy(curd, dorg, dorg.Length);

        //                            computenewzlossset(zlossset0, dorg, target0, row[ran], col[ran]);//loss總數
        //                            //computenewzlossset(zlossset0, curd, target0, row[ran], col[ran]);

        //                            ldnow[row[ran] - halfrow, col[ran] - halfcol][order[ran]].eon = newcode;
        //                            //++updatecnt;

        //                            //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                            //writenewldfile(tld);
        //                            //dosageset d05 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                            //lossset loss_opt5 = computeloss(d05, target);//first loss value of zloss
        //                            //updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                            //lossset loss05 = computeloss(d, target);
        //                            //if (loss_opt5.lossvalue != loss05.lossvalue)
        //                            //    ;
        //                            //ldnow[row[ran] - halfrow, col[ran] - halfcol][order[ran]].eon = 1;
        //                            //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                            //writenewldfile(tld);
        //                            //dosageset d06 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                            //lossset loss_opt6 = computeloss(d06, target);//first loss value of zloss
        //                            //;
        //                            //++updatetotalcnt;
        //                            //updateld(tld.array, ldnow, x0, y0, x1, y1);
        //                            //writenewldfile(tld);
        //                            //ldset tldfullnew0 = readld_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um
        //                            //ldset tldfullnew = readld_opt(xlefttop, ylefttop, xrightbottom, yrightbottom, expandscale);//resolution = 0.125um
        //                            //dosageset d_opt0 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                            //lossset loss_opt0 = computeloss(d_opt0, target);//first loss value of zloss
        //                            //verifyimg.writearray(tld.array, optoutputfolder + "optld_1stage_0");
        //                            //verifyimg.writearray(tldfullnew0.array, optoutputfolder + "optld_1stage");
        //                            //verifyimg.writearray(d.array, optoutputfolder + "optd_1stage_0");
        //                            //outputlist(losshistory, optoutputfolder + "losshistory_1stage");
        //                            //verifyimg.writearray(d_opt0.array, optoutputfolder + "optd_1stage");
        //                            //verifyimg.writearray(dorg, optoutputfolder + "optd_1stage_0");
        //                            //updatedosage(d.array, dorg, x00, y00, x10, y10);
        //                            //verifyimg.writearray(d.array, optoutputfolder + "optd_1stage_0");

        //                            if (losschoose < bestloss)
        //                            {
        //                                Array.Copy(dorg, bestd, dorg.Length);

        //                                Array.Copy(code, bestcode, code.Length);
        //                                bestloss = losschoose;
        //                                Array.Copy(zlossset0.zloss, bestzloss, zlossset0.zloss.Length);
        //                                copyld(ldnow, bestld);
        //                                oiter = 0;
        //                                optimized = true;
        //                                //updatetotalcnt += updatecnt;
        //                                //updatecnt = 0;

        //                                //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                                //writenewldfile(tld);
        //                                //dosageset d00 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                                //lossset loss_opt0 = computeloss(d00, target);//first loss value of zloss
        //                                //updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                                //lossset loss0 = computeloss(d, target);
        //                                //curlosshistory.Add(loss_opt0.lossvalue);
        //                                //bestlosshistory.Add(loss0.lossvalue);
        //                                //difflosshistory.Add(loss_opt0.lossvalue - loss0.lossvalue);
        //                                //if (loss_opt0.lossvalue != loss0.lossvalue)
        //                                //    ;
        //                            }
        //                            //else
        //                            //{
        //                            //    ;
        //                            //}
        //                        }
        //                    }

        //                    if (optimized == false)
        //                    {
        //                        nonoptx = f;
        //                        nonopty = k;
        //                    }
        //                    else
        //                    {
        //                        updateld(tld.array, bestld, x0, y0, x1, y1);
        //                        updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                        updatezloss(loss, d.array, target.array, x00, y00, x10, y10);

        //                        //updateld(tld.array, bestld, x0, y0, x1, y1);
        //                        //writenewldfile(tld);
        //                        //dosageset d01 = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                        //lossset loss_opt1 = computeloss(d01, target);//first loss value of zloss
        //                        ////updatedosage(d.array, bestd, x00, y00, x10, y10);
        //                        //lossset loss01 = computeloss(d, target);
        //                        //if (loss_opt1.lossvalue != loss01.lossvalue)
        //                        //    ;
        //                    }
        //                    losshistory.Add(loss.lossvalue);

        //                    //if (updatetotalcnt > 200)
        //                    //{
        //                    //    writenewldfile(tld);
        //                    //    d = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //                    //    updatetotalcnt = 0;
        //                    //}
        //                }

        //                int constantiter = iniconstantiter, epsilon = iniepsilon, lossvar;//epsilon為loss(像素)

        //                //if (losshistory.Count >= constantiter)
        //                //{
        //                //    lossvar = losshistory[losshistory.Count - constantiter] - losshistory[losshistory.Count - 1];
        //                //    if (lossvar < epsilon)//在constantiter個迴圈前減掉現在的迴圈數
        //                //    {
        //                //        stop = true;
        //                //        break;
        //                //    }
        //                //}

                        

        //                if (losshistory[losshistory.Count - 1] <= totalnum_criterion)
        //                {
        //                    stop = true;
        //                    break;
        //                }


        //            }
        //            updatelosspos(loss);
        //            iternum += 1;

        //            if(iternum> iniconstantiter)
        //            {
        //                break;
        //            }
        //        }


        //        //float[,] dnew = new float[d.ynum, d.xnum];
        //        //for (int i = 0; i < dnew.GetLength(0); ++i)
        //        //{
        //        //    for (int j = 0; j < dnew.GetLength(1); ++j)
        //        //    {
        //        //        dnew[i, j] = d.array[i + d.yf1, j + d.xf1];
        //        //    }
        //        //}

        //        //string dstr1 = outputpath + "dosageimg_new";
        //        //verifyimg.writearray(dnew, dstr1);

        //        writenewldfile(tld);
        //        ldset tldfullnew = readld_opt(xlefttop, ylefttop, xrightbottom, yrightbottom, expandscale);//resolution = 0.125um
        //        dosageset d_opt = readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
        //        lossset loss_opt = computeloss(d_opt, target);//first loss value of zloss
        //        //verifyimg.writearray(tldfullnew.array, optoutputfolder + "optld_1stage");
        //        //verifyimg.writearray(d.array, optoutputfolder + "optd_1stage_0");
        //        //outputlist(losshistory, optoutputfolder + "losshistory_1stage");
        //        verifyimg.writearray(d_opt.array, optoutputfolder + "optd_1stage");

        //        result = new int[5];
        //        result[0] = losshistory[0];
        //        result[1] = losshistory[losshistory.Count - 1];
        //        result[2] = 0;
        //        result[3] = 0;
        //        result[4] = loss_opt.lossinsidetarget;



        //    }

        //    return result;
        //}

        public int[] optimize_maxcc(int xlefttop, int ylefttop, int xrightbottom, int yrightbottom)
        {
            this.xlefttop = xlefttop;
            this.ylefttop = ylefttop;
            this.xrightbottom = xrightbottom;
            this.yrightbottom = yrightbottom;
            hdf5 verifyimg = new hdf5();
            roirownum = yrightbottom - ylefttop + 1;
            roicolnum = xrightbottom - xlefttop + 1;
            scale = tiffimg.imgps / 0.125f;
            scaleoffset = (int)(scale / 2);
            hdf5 beam1 = new hdf5(), beam2 = new hdf5();
            beam1.openfile(this.beamfile1path);
            beamrow = beam1.rownum;
            beamcol = beam1.colnum;
            halfrow = beamrow / 2;
            halfcol = beamcol / 2;
            //offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            beam1.closefile();
            //beamrow = 21; beamcol = 41; halfrow = beamrow / 2; halfcol = beamcol / 2;
            //threshold = 15;
            tempx = (int)(Math.Ceiling(halfcol / scale));
            tempy = (int)(Math.Ceiling(halfrow / scale));
            opt_para.tiffimg = tiffimg;
            opt_para.ldconfig1 = ldconfig1;
            opt_para.ldconfig2 = ldconfig2;
            opt_para.ldfilepath1 = ldfilepath1;
            opt_para.ldfilepath2 = ldfilepath2;
            opt_para.ldfilenewpath1 = ldfilenewpath1;
            opt_para.ldfilenewpath2 = ldfilenewpath2;
            opt_para.outputpath = outputpath;
            opt_para.beamfilepath = beamfilepath;
            opt_para.beamfile1path = beamfile1path;
            opt_para.beamfile2path = beamfile2path;
            opt_para.threshold = threshold;
            opt_para.filenameini = filenameini;
            opt_para.tempx = tempx;
            opt_para.tempy = tempy;
            opt_para.tempxscale = tempxscale;
            opt_para.tempyscale = tempyscale;
            opt_para.scale = scale;
            opt_para.scaleoffset = scaleoffset;
            opt_para.maxcc_criterion = maxcc_criterion;
            opt_para.totalnum_criterion = totalnum_criterion;
            opt_para.beamrow = beamrow;
            opt_para.beamcol = beamcol;
            opt_para.halfrow = halfrow;
            opt_para.halfcol = halfcol;
            opt_para.xexp = xexp;
            opt_para.yexp = yexp;
            opt_para.xexpscale = xexpscale;
            opt_para.yexpscale = yexpscale;
            opt_para.expandscale = expandscale;
            opt_para.expandldscale = expandldscale;
            opt_para.iniconstantiter = iniconstantiter;
            opt_para.iniepsilon = iniepsilon;
            opt_para.iniproportion = iniproportion;
            opt_para.initabuproportion = initabuproportion;
            opt_para.iniomaxiter = iniomaxiter;
            opt_para.optidx = optidx;
            opt_para.roi_xy = roi_xy;
            opt_para.optoutputfolder = optoutputfolder;
            opt_para.xlefttop = xlefttop;
            opt_para.ylefttop = ylefttop;
            opt_para.xrightbottom = xrightbottom;
            opt_para.yrightbottom = yrightbottom;
            opt_para.roirownum = roirownum;
            opt_para.roicolnum = roicolnum;
            opt_para.lossweight = lossweight;
            opt_para.displacement = displacement;
            optimization_funcation opfunction = new optimization_funcation(opt_para);
          
            //int[] optidx0 = new int[4];
            ldset tld = opfunction.readld(xlefttop, ylefttop, xrightbottom, yrightbottom,"1");//resolution = 0.125um
            ldset tldfull = opfunction.readld(xlefttop, ylefttop, xrightbottom, yrightbottom, "1", expandscale);//resolution = 0.125um
            dosageset d = opfunction.readdosage(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
            //float[,] d = new float[0, 0];
            targetset target = opfunction.readtarget(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um byte[,] target = new byte[0, 0];
            yexpscale = opfunction.yexpscale;
            xexpscale = opfunction.xexpscale;
            optoutputfolder = outputpath + "optimization\\";
            if (!Directory.Exists(optoutputfolder))
                Directory.CreateDirectory(optoutputfolder);
            //verifyimg.writearray(tldfull.array, optoutputfolder + "orgld_1stage");
            verifyimg.writearray(d.array, optoutputfolder + "orgd_1stage");
            verifyimg.writearray(target.array, optoutputfolder + "targetroi_1stage");

            //int[,] zloss = new int[0, 0];//use d,threshold
            lossset loss = opfunction.computeloss(d, target);//first loss value of zloss
            ccl ccl0 = new ccl();
            int yexpscaleright = (int)(roirownum * scale) + yexpscale, xexpscaleright = (int)(roicolnum * scale) + xexpscale;
            optidx[0] = yexpscale;
            optidx[1] = yexpscale + (int)(roirownum * scale) - 1;
            optidx[2] = xexpscale;
            optidx[3] = xexpscale + (int)(roicolnum * scale) - 1;
            int cc = ccl0.findmaxcc(loss.zloss, xexpscale, yexpscale, xexpscaleright, yexpscaleright);
            List<int> losshistory = new List<int>();
            List<int> cchistory = new List<int>();
            //int initloss = opfunction.computeloss_int(loss.zloss);
            //List<int> cchistory_second = new List<int>();
            float ccthresholdratio = 2f / 3, ccthreshold;
            int[] result = new int[0];
            
            if (cc > maxcc_criterion || loss.lossvalue > totalnum_criterion)
            {
                losshistory.Add(loss.lossvalue);
                cchistory.Add(cc);

                Dictionary<int, List<pos>> ccpos = ccl0.findccpos(xexpscale, yexpscale, xexpscaleright, yexpscaleright);
                List<int> ccposidx = new List<int>();
                //ccthreshold = cchistory[cchistory.Count - 1] * ccthresholdratio;
                foreach (int key in ccpos.Keys)
                {
                    if (ccpos[key].Count >= maxcc_criterion)
                        ccposidx.Add(key);
                }


                int rangex = 0, rangey = 0, rangescale = 0, omaxiter = iniomaxiter, totalcnt = 1;
                float proportion = (float)iniproportion, tabuproportion = (float)initabuproportion;
                bool optimized = true, stop = false;
                int nonoptx = 0, nonopty = 0, distancex, distancey;
                byte[,] target0, zloss0, optregion0;
                float[,] dorg;
                int[] optidx = new int[4];
                List<beam>[,] ldnow;
                int ldxoffset = (int)((tld.ldxstart - target.xstart) * scale);
                int ldyoffset = (int)((tld.ldystart - target.ystart) * scale);
                //int loss = computeloss(d, target);
                int k, f, x00, y00, x10, y10, lossorg, x0, y0, x1, y1, xoffset, yoffset;

                List<byte> tablenum1 = ldconfig1.checkrange(tld.ldxstart, tld.ldxend);
                List<byte> tablenum2 = ldconfig2.checkrange(tld.ldxstart, tld.ldxend);

                Dictionary<int, float[,,]> spots;
                zlossset zlossset0;
                dlossset dlossset0;
                //string beamfile1path = beamfilepath + "spotarrays100le1.h5", beamfile2path = beamfilepath + "spotarrays100le2.h5";
                beam1.openfile(beamfile1path);
                beam2.openfile(beamfile2path);
                spots = opfunction.storespots32(beam1, beam2, tablenum1, tablenum2);
                beam1.closefile();
                beam2.closefile();
                //string testfolder = outputpath + "test\\", dstr = testfolder + "dorg", ldstr1 = testfolder + "ldorg", ldstr2 = testfolder + "bestld", tstr = testfolder + "target0", zstr = testfolder + "zloss0", ldstr = testfolder + "ldnow";
                int iternum = 1;

                while (!stop && loss.lossposx.Count != 0)
                {
                    //foreach (int key in ccposidx)
                    {
                        //for (int pos = 0; pos < ccpos[key].Count(); ++pos)
                        for (int pos = 0; pos < loss.lossposx.Count(); ++pos)
                        {
                            k = loss.lossposy[pos];
                            f = loss.lossposx[pos];
                            //k = ccpos[key][pos].y;
                            //f = ccpos[key][pos].x;
                            
                            if (loss.zloss[k, f] != 0)
                            {
                                if (optimized == false)
                                {
                                    distancey = Math.Abs(nonopty - k);
                                    distancex = Math.Abs(nonoptx - f);
                                    if (distancex <= rangex && distancey <= rangey)
                                    {
                                        continue;
                                    }
                                }
                                optimized = false;

                                x00 = f - halfcol * (2 + rangescale);
                                y00 = k - halfrow * (2 + rangescale);
                                if (x00 < 0)
                                    x00 = 0;

                                if (y00 < 0)
                                    y00 = 0;

                                x10 = f + halfcol * (2 + rangescale);
                                y10 = k + halfrow * (2 + rangescale);
                                if (x10 >= target.array.GetLength(1))
                                    x10 = target.array.GetLength(1) - 1;

                                if (y10 >= target.array.GetLength(0))
                                    y10 = target.array.GetLength(0) - 1;

                                target0 = opfunction.parttarget(target.array, x00, y00, x10, y10);
                                //lossorg = computepartloss(loss.zloss, x00, y00, x10, y10);
                                dorg = opfunction.partdosage(d.array, x00, y00, x10, y10);
                                zlossset0 = opfunction.partzloss(dorg, target0);
                                //verifyimg.writearray(dorg, dstr);
                                //verifyimg.writearray(target0, tstr);
                                //verifyimg.writearray(zlossset0.zloss, zstr);

                                x0 = f - halfcol * (1 + rangescale) - ldxoffset;
                                y0 = k - halfrow * (1 + rangescale) - ldyoffset;
                                if (x0 < 0)
                                    x0 = 0;

                                if (y0 < 0)
                                    y0 = 0;

                                x1 = f + halfcol * (1 + rangescale) - ldxoffset;
                                y1 = k + halfrow * (1 + rangescale) - ldyoffset;
                                if (x1 >= tld.array.GetLength(1))
                                    x1 = tld.array.GetLength(1) - 1;

                                if (y1 >= tld.array.GetLength(0))
                                    y1 = tld.array.GetLength(0) - 1;

                                //xoffset = x0 - x00;
                                //yoffset = y0 - y00;

                                ldnow = opfunction.partld(tld.array, x0, y0, x1, y1);
                                //verifyimg.writearray(ldnow, ldstr);

                                List<int> row0 = new List<int>();
                                List<int> col0 = new List<int>();
                                List<byte> code0 = new List<byte>();
                                List<byte> order0 = new List<byte>();
                                List<ushort> spotorder0 = new List<ushort>();
                                List<ushort> spotnum0 = new List<ushort>();

                                for (int i = 0; i < ldnow.GetLength(0); ++i)
                                {
                                    for (int j = 0; j < ldnow.GetLength(1); ++j)
                                    {
                                        if (ldnow[i, j] != null)
                                        {
                                            for (byte k1 = 0; k1 < ldnow[i, j].Count; ++k1)
                                            {
                                                row0.Add(i + halfrow);
                                                col0.Add(j + halfcol);
                                                code0.Add(ldnow[i, j][k1].eon);
                                                order0.Add(k1);
                                                spotorder0.Add(ldnow[i, j][k1].spotorder);
                                                spotnum0.Add(ldnow[i, j][k1].spotnum);
                                            }
                                        }
                                    }
                                }

                                if (code0.Count == 0)
                                    continue;

                                int[] row = row0.ToArray();
                                int[] col = col0.ToArray();
                                byte[] code = code0.ToArray();
                                byte[] order = order0.ToArray();
                                ushort[] spotorder = spotorder0.ToArray();
                                ushort[] spotnum = spotnum0.ToArray();

                                List<beam>[,] bestld = new List<beam>[ldnow.GetLength(0), ldnow.GetLength(1)];
                                opfunction.copyld(ldnow, bestld);
                                int bestloss = zlossset0.loss;
                                byte[] bestcode = new byte[code.Length];
                                Array.Copy(code, bestcode, code.Length);
                                float[,] bestd = new float[dorg.GetLength(0), dorg.GetLength(1)];
                                Array.Copy(dorg, bestd, dorg.Length);
                                int[,] bestzloss = new int[zlossset0.zloss.GetLength(0), zlossset0.zloss.GetLength(1)];
                                Array.Copy(zlossset0.zloss, bestzloss, zlossset0.zloss.Length);
                                Queue<int> tabu = new Queue<int>();
                                int oiter = 0;
                                int ocnt = 0;
                                int maxsize = (int)(tabuproportion * code.Length);
                                int maxiter = (int)(proportion * code.Length);
                                bool flag;
                                int[] losslist;
                                //MinHeap<ranloss> losslist;
                                //ranloss[] curranloss = new ranloss[maxiter];
                                //for (int i = 0; i < curranloss.Length; ++i)
                                //{
                                //    curranloss[i] = new ranloss();
                                //}
                                int[] ranlist;
                                //float[,] curd;
                                int idxstep, ranchoose = 0;
                                int losschoose = 0;
                                byte newcode;

                                while (oiter != omaxiter)
                                {
                                    //++ocnt;
                                    ++oiter;
                                    if (bestloss == 0)
                                        break;


                                    //int levelnum = 2, levelminusone= levelnum-1;
                                    losslist = new int[maxiter];
                                    //losslist = new MinHeap<ranloss>(new rancomp(), maxiter);
                                    ranlist = opfunction.GenerateRandom(maxiter, 0, code.Length);
                                    flag = false;
                                    //curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
                                    //List<int> ranlist = GenerateRandom(maxiter, 0, code.Length);
                                    //for (int iter = 0; iter < maxiter; ++iter)
                                    Parallel.For(0, maxiter, iter =>
                                    {
                                        int ran = ranlist[iter];
                                        float[,] curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
                                        Array.Copy(dorg, curd, dorg.Length);

                                        if (code[ran] == 1)
                                        {
                                            opfunction.subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }
                                        else
                                        {
                                            opfunction.addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }

                                        int orgcurloss = opfunction.computeorgcurloss(zlossset0.zloss, row[ran], col[ran]);
                                        int curloss = opfunction.computecurloss(target0, curd, row[ran], col[ran]);

                                        //losslist[iter] = zlossset0.loss + curloss - orgcurloss;
                                        //curranloss[iter].loss = zlossset0.loss + curloss - orgcurloss;
                                        //curranloss[iter].idx = iter;
                                        //losslist.Add(curranloss[iter]);
                                        losslist[iter] = zlossset0.loss + curloss - orgcurloss;
                                    });

                                    int[] indices = new int[maxiter];
                                    for (int i = 0; i < maxiter; i++) indices[i] = i;
                                    Array.Sort(losslist, indices);

                                    for (int idx = 0; idx < maxiter; ++idx)
                                    {
                                        if (tabu.Contains(ranlist[indices[idx]]) == false)
                                        {
                                            ++ocnt;
                                            ranchoose = ranlist[indices[idx]];
                                            losschoose = losslist[idx];
                                            tabu.Enqueue(ranlist[indices[idx]]);
                                            flag = true;
                                            break;
                                        }
                                        else if (losslist[idx] < bestloss)
                                        {
                                            ++ocnt;
                                            ranchoose = ranlist[indices[idx]];
                                            losschoose = losslist[idx];
                                            tabu.Enqueue(ranlist[indices[idx]]);
                                            flag = true;
                                            break;
                                        }
                                    }

                                    //ranloss ranlosschoose = new ranloss();
                                    //for (int idx = 0; idx < maxiter; ++idx)
                                    //{
                                    //    ranlosschoose = losslist.GetMin();

                                    //    if (tabu.Contains(ranlist[ranlosschoose.idx]) == false)
                                    //    {
                                    //        ++ocnt;
                                    //        ranchoose = ranlist[ranlosschoose.idx];
                                    //        losschoose = ranlosschoose.loss;
                                    //        tabu.Enqueue(ranlist[ranlosschoose.idx]);
                                    //        flag = true;
                                    //        break;
                                    //    }
                                    //    else if (ranlosschoose.loss < bestloss)
                                    //    {
                                    //        ++ocnt;
                                    //        ranchoose = ranlist[ranlosschoose.idx];
                                    //        losschoose = ranlosschoose.loss;
                                    //        tabu.Enqueue(ranlist[ranlosschoose.idx]);
                                    //        flag = true;
                                    //        break;
                                    //    }

                                    //    losslist.ExtractDominating();

                                    //}

                                    if (ocnt > maxsize)
                                    {
                                        tabu.Dequeue();
                                        --ocnt;
                                    }

                                    if (flag == true)
                                    {
                                        float[,] curd = new float[dorg.GetLength(0), dorg.GetLength(1)];
                                        Array.Copy(dorg, curd, dorg.Length);
                                        int ran = ranchoose;
                                        if (code[ran] == 1)
                                        {
                                            newcode = 0;
                                            opfunction.subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }
                                        else
                                        {
                                            newcode = 1;
                                            opfunction.addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }

                                        code[ran] = newcode;

                                        Array.Copy(curd, dorg, dorg.Length);
                                        opfunction.computenewzlossset(zlossset0, dorg, target0, row[ran], col[ran]);
                                        ldnow[row[ran] - halfrow, col[ran] - halfcol][order[ran]].eon = newcode;

                                        if (losschoose < bestloss)
                                        {
                                            Array.Copy(dorg, bestd, dorg.Length);
                                            Array.Copy(code, bestcode, code.Length);
                                            bestloss = losschoose;
                                            Array.Copy(zlossset0.zloss, bestzloss, zlossset0.zloss.Length);
                                            opfunction.copyld(ldnow, bestld);
                                            oiter = 0;
                                            optimized = true;
                                        }
                                    }
                                }

                                if (optimized == false)
                                {
                                    nonoptx = f;
                                    nonopty = k;
                                }

                                opfunction.updateld(tld.array, bestld, x0, y0, x1, y1);
                                opfunction.updatedosage(d.array, bestd, x00, y00, x10, y10);
                                opfunction.updatezloss(loss, d.array, target.array, x00, y00, x10, y10);
                                losshistory.Add(loss.lossvalue);
                            }

                            int constantiter = iniconstantiter, epsilon = iniepsilon, lossvar;

                            //if (losshistory.Count >= constantiter & !first)
                            //{
                            //    lossvar = losshistory[losshistory.Count - constantiter] - losshistory[losshistory.Count - 1];
                            //    if (lossvar < epsilon)
                            //    {
                            //        stop = true;
                            //        break;
                            //    }
                            //}

                            //if (losshistory[losshistory.Count - 1] <= totalnum_criterion)
                            //{
                            //    stop = true;
                            //    break;
                            //}

                        }
                    }
                    cc = ccl0.findmaxcc(loss.zloss, xexpscale, yexpscale, xexpscaleright, yexpscaleright);
                    cchistory.Add(cc);
                    if (cc <= maxcc_criterion && losshistory[losshistory.Count - 1] <= totalnum_criterion)
                    {
                        stop = true;
                        break;
                    }

                    //updatelosspos(loss);

                    //ccpos = ccl0.findccpos(xexpscale, yexpscale, xexpscaleright, yexpscaleright);
                    //ccposidx.Clear();
                    ////ccthreshold = cchistory[cchistory.Count - 1] * ccthresholdratio;
                    //foreach (int key in ccpos.Keys)
                    //{
                    //    if (ccpos[key].Count >= maxcc_criterion)
                    //        ccposidx.Add(key);
                    //}

                    opfunction.updatelosspos(loss);
                    iternum += 1;

                    if (iternum > iniconstantiter)
                    {
                        break;
                    }


                }

                //outputlist(cchistory, optoutputfolder + "cchistory_1stage.txt");
                //outputlist(losshistory, optoutputfolder + "losshistory_1stage.txt");
                opfunction.writenewldfile(tld);
                ldset tldfullnew = opfunction.readld_opt(xlefttop, ylefttop, xrightbottom, yrightbottom, "1", expandscale);//resolution = 0.125um
                dosageset d_opt = opfunction.readdosage_opt(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um,use tld
                lossset loss_opt = opfunction.computeloss(d_opt, target);//first loss value of zloss
                //verifyimg.writearray(tldfullnew.array, optoutputfolder + "optld_1stage");
                //verifyimg.writearray(d.array, optoutputfolder + "optd_1stage");
                //outputlist(losshistory, optoutputfolder + "losshistory_1stage");
                verifyimg.writearray(d_opt.array, optoutputfolder + "optd_1stage");
                //verifyimg.writearray(tldfullnew.array, optoutputfolder + "optld_1stage");
                //verifyimg.writearray(d.array, optoutputfolder + "optd_1stage");

                result = new int[5];
                result[0] = loss.lossroi;//before
                result[1] = loss_opt.lossroi;//after
                result[2] = cchistory[0];//maxcc
                result[3] = cchistory[cchistory.Count - 1];
                result[4] = loss_opt.lossinsidetarget;

            }

            return result;
        }

      
    }
}
