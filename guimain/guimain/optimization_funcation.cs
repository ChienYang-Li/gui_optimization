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

    public class opt_parameter
    {
        public tiffimage tiffimg;
        public ldconfig ldconfig1;
        public ldconfig ldconfig2;
        public string result2;
        public string ldfilepath1, ldfilepath2, ldfilenewpath1, ldfilenewpath2, outputpath;
        public string beamfilepath, beamfile1path, beamfile2path;
        public float threshold;
        public string filenameini;
        public int tempx, tempy, tempxscale, tempyscale;//temp = (ceil)((double)beamsize/scale) tempscale = temp*scale
        public float scale;
        public int scaleoffset;
        public int maxcc_criterion, totalnum_criterion;
        public int beamrow, beamcol, halfrow, halfcol;
        public int xexp, yexp, xexpscale, yexpscale;
        public int expandscale = 2, expandldscale = 1;
        public int iniconstantiter;
        public int iniepsilon;
        public double iniproportion;
        public double initabuproportion;
        public int iniomaxiter;
        public int[] optidx = new int[4];
        public string roi_xy;
        public int lossweight;
        public string optoutputfolder;
        public int xlefttop;
        public int ylefttop;
        public int xrightbottom;
        public int yrightbottom;
        public float displacement;
        public int roirownum, roicolnum;
        public string ldfilepath1_4level;
        public string ldfilepath2_4level;
        public int beamonlevel;
    }


    public class optimization_funcation
    {

        public tiffimage tiffimg;
        public ldconfig ldconfig1;
        public ldconfig ldconfig2;
        public string result2;
        public string ldfilepath1, ldfilepath2, ldfilenewpath1, ldfilenewpath2, outputpath;
        public string beamfilepath, beamfile1path, beamfile2path;
        public float threshold;
        public string filenameini = "path.ini";
        public InI ini = new InI();
        public float displacement;
        public int tempx, tempy, tempxscale, tempyscale;//temp = (ceil)((double)beamsize/scale) tempscale = temp*scale
        public float scale;
        public int scaleoffset;
        public int maxcc_criterion, totalnum_criterion;
        public int beamrow, beamcol, halfrow, halfcol;
        public int xexp, yexp, xexpscale, yexpscale;
        public int expandscale = 2, expandldscale = 1;
        public int iniconstantiter;
        public int iniepsilon;
        public double iniproportion;
        public double initabuproportion;
        public int iniomaxiter;
        public int[] optidx = new int[4];
        public string roi_xy;
        public int lossweight;
        public string optoutputfolder;
        public int xlefttop;
        public int ylefttop;
        public int xrightbottom;
        public int yrightbottom;
        public int roirownum, roicolnum;
        public string ldfilepath1_4level;
        public string ldfilepath2_4level;
        public int beamonlevel;
       
        public optimization_funcation(opt_parameter opt)//建構函式
        {
          
         
            this.tiffimg = opt.tiffimg;
            this.ldconfig1 = opt.ldconfig1;
            this.ldconfig2 = opt.ldconfig2;
            this.outputpath = opt.outputpath;
            this.beamfilepath = opt.beamfilepath;
            this.threshold = opt.threshold;
            this.displacement = opt.displacement;
            this.tempx = opt.tempx;
            this.tempy = opt.tempy;
            this.tempxscale = opt.tempxscale;
            this.tempxscale = opt.tempxscale;
            this.tempyscale = opt.tempyscale;
            this.scale = opt.scale;
            this.scaleoffset = opt.scaleoffset;
            this.maxcc_criterion = opt.maxcc_criterion;
            this.totalnum_criterion = opt.totalnum_criterion;
            this.beamrow = opt.beamrow;
            this.beamcol = opt.beamcol;
            this.halfrow = opt.halfrow;
            this.halfcol = opt.halfcol;
            this.xexp = opt.xexp;
            this.yexp = opt.yexp;
            this.xexpscale = opt.xexpscale;
            this.yexpscale = opt.yexpscale;
            this.expandscale = opt.expandscale;
            this.expandldscale = opt.expandldscale;
            this.iniconstantiter = opt.iniconstantiter;
            this.iniepsilon = opt.iniepsilon;
            this.iniproportion = opt.iniproportion;
            this.initabuproportion = opt.initabuproportion;
            this.iniomaxiter = opt.iniomaxiter;
            this.optidx = opt.optidx;
            this.roi_xy = opt.roi_xy;
            this.optoutputfolder = opt.optoutputfolder;
            this.lossweight = opt.lossweight;
            this.xlefttop = opt.xlefttop;
            this.ylefttop = opt.xlefttop;
            this.xrightbottom = opt.xrightbottom;
            this.yrightbottom = opt.yrightbottom;
            this.roirownum = opt.roirownum;
            this.roicolnum = opt.roicolnum;
            this.ldfilepath1 = opt.ldfilepath1;
            this.ldfilepath2 = opt.ldfilepath2;
            this.ldfilenewpath1 = opt.ldfilenewpath1;
            this.ldfilenewpath2 = opt.ldfilenewpath2;
            this.beamfile1path = opt.beamfile1path;
            this.beamfile2path = opt.beamfile2path;
            /*2 & 3 stage專用*/
            this.ldfilepath1_4level = opt.ldfilepath1_4level;
            this.ldfilepath2_4level = opt.ldfilepath2_4level;
            this.beamonlevel = opt.beamonlevel;
        }



        public ldset readld(int xstart, int ystart, int xend, int yend, string stage, int expandldscale = 1)
        {
            //int ldxstart
            float cnt_scale = displacement / tiffimg.imgps;
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            t0 = (int)(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale);

            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xf0 = 0, yf0 = 0;
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                xf0 = -ldxstart;
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                yf0 = -ldystart;
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            //int xf1 = xstart - ldxstart,yf1 = ystart - ldystart;

            List<beam>[,] ldarray = new List<beam>[ldyend - ldystart + 1, ldxend - ldxstart + 1];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    ldarray[i, j] = new List<beam>();
                }
            }

            int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
            int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
            byte[] buffer = new byte[4000];
            beam b;
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
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);

                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 32;
                                    int startbyte = s * 4;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 20; ++ldnum)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                int ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                    b = new beam();
                                                    b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                    b.spotnum = (ushort)s;
                                                    if (bits[ldnum + startbit] == true)
                                                        b.eon = 1;
                                                    else
                                                        b.eon = 0;

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 32;
                                    //startbyte += 4;
                                }

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

            //toffset = t0 + (int)(ldystart / cnt_scale);
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
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);
                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 32;
                                    int startbyte = s * 4;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 20; ++ldnum)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                int ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                    b = new beam();
                                                    b.spotorder = (ushort)((ldnum * 8 + facet - 1) + 160);
                                                    b.spotnum = (ushort)s;
                                                    if (bits[ldnum + startbit] == true)
                                                        b.eon = 1;
                                                    else
                                                        b.eon = 0;

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 24;
                                    //startbyte += 4;
                                }

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

            double scale = tiffimg.imgps / 0.125;

            int yfixedlen = yend - ystart + 1 + ((tempy * expandldscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandldscale) << 1);
            List<beam>[,] ldarrayscale = new List<beam>[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];

            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarrayscale[(int)((i + yf0) * scale + scaleoffset), (int)((j + xf0) * scale + scaleoffset)] = ldarray[i, j];
                }
            }

            List<beam>[,] ldarray0 = new List<beam>[(int)(yfixedlen), (int)(xfixedlen)];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarray0[(int)((i + yf0)), (int)((j + xf0))] = ldarray[i, j];
                }
            }



            if (expandldscale == 2)
            {
                hdf5 verifyimg = new hdf5();
                verifyimg.writearray(ldarray0, optoutputfolder + "orgld_"+ stage+"stage", tiffimg.imgps);
            }
            //string ldstr = outputpath + "ldimg_org";
            //List<beam>[,] ldarrayorg = new List<beam>[yend - ystart + 1, xend - xstart + 1];
            //for(int i=0;i< ldarrayorg.GetLength(0);++i)
            //{
            //    for(int j=0;j<ldarrayorg.GetLength(1);++j)
            //    {
            //        if (ldarray[i+ yf1, j+xf1].Count > 0)
            //        {
            //            ldarrayorg[i, j] = new List<beam>();
            //            ldarrayorg[i, j] = ldarray[i+yf1, j+xf1];
            //        }
            //    }
            //}

            //verifyimg.writearray(ldarray, ldstr);




            ldset result = new ldset();

            result.array = ldarrayscale;
            result.ldxstart = ldxstart;
            result.ldxend = ldxend;
            result.ldystart = ldystart;
            result.ldyend = ldyend;
            result.leftx = xf0;
            result.lefty = yf0;
            result.rightx = xf0 + ldarray.GetLength(1) - 1;
            result.righty = yf0 + ldarray.GetLength(0) - 1;
            //result.xoffsetorg = xf1;
            //result.yoffsetorg = yf1;
            //result.orgxnum = ldarrayorg.GetLength(1);
            //result.orgynum = ldarrayorg.GetLength(0);
            result.orgrownum = ldarray.GetLength(0);
            result.orgcolnum = ldarray.GetLength(1);

            return result;
        }

        public ldset readld_opt(int xstart, int ystart, int xend, int yend, string stage, int expandldscale = 1)
        {
            //int ldxstart
            float cnt_scale = displacement / tiffimg.imgps;
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{

            t0 = (int)(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale);

            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xf0 = 0, yf0 = 0;
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                xf0 = -ldxstart;
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                yf0 = -ldystart;
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            //int xf1 = xstart - ldxstart,yf1 = ystart - ldystart;

            List<beam>[,] ldarray = new List<beam>[ldyend - ldystart + 1, ldxend - ldxstart + 1];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    ldarray[i, j] = new List<beam>();
                }
            }

            int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
            int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
            byte[] buffer = new byte[4000];
            beam b;
            BitArray bits;
            byte facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;

            if (ldxstart <= ldconfig1.xmax)
            {
                using (FileStream ldfile = new FileStream(ldfilenewpath1, FileMode.Open, FileAccess.Read))
                {
                    ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                    //while (ldfile.Read(buffer, 0, 4000) > 0)

                    for (int t = toffset; t <= space + toffset; ++t)
                    {
                        if (ldfile.Read(buffer, 0, 4000) > 0)
                        {
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);

                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 32;
                                    int startbyte = s * 4;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 20; ++ldnum)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                int ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                    b = new beam();
                                                    b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                    b.spotnum = (ushort)s;
                                                    if (bits[ldnum + startbit] == true)
                                                        b.eon = 1;
                                                    else
                                                        b.eon = 0;

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 32;
                                    //startbyte += 4;
                                }

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

            //toffset = t0 + (int)(ldystart / cnt_scale);
            facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;

            if (ldxend > ldconfig1.xmax)
            {
                using (FileStream ldfile = new FileStream(ldfilenewpath2, FileMode.Open, FileAccess.Read))
                {
                    ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                    //while (ldfile.Read(buffer, 0, 4000) > 0)
                    for (int t = toffset; t <= space + toffset; ++t)
                    {
                        if (ldfile.Read(buffer, 0, 4000) > 0)
                        {
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);
                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 32;
                                    int startbyte = s * 4;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 20; ++ldnum)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                int ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                int ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                    b = new beam();
                                                    b.spotorder = (ushort)((ldnum * 8 + facet - 1) + 160);
                                                    b.spotnum = (ushort)s;
                                                    if (bits[ldnum + startbit] == true)
                                                        b.eon = 1;
                                                    else
                                                        b.eon = 0;

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 24;
                                    //startbyte += 4;
                                }

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

            double scale = tiffimg.imgps / 0.125;

            int yfixedlen = yend - ystart + 1 + ((tempy * expandldscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandldscale) << 1);
            List<beam>[,] ldarrayscale = new List<beam>[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];

            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarrayscale[(int)((i + yf0) * scale + scaleoffset), (int)((j + xf0) * scale + scaleoffset)] = ldarray[i, j];
                }
            }

            List<beam>[,] ldarray0 = new List<beam>[(int)(yfixedlen), (int)(xfixedlen)];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarray0[(int)((i + yf0)), (int)((j + xf0))] = ldarray[i, j];
                }
            }



            if (expandldscale == 2)
            {
                hdf5 verifyimg = new hdf5();
                verifyimg.writearray(ldarray0, optoutputfolder + "optld_" + stage + "stage", tiffimg.imgps);
            }
            //string ldstr = outputpath + "ldimg_org";
            //List<beam>[,] ldarrayorg = new List<beam>[yend - ystart + 1, xend - xstart + 1];
            //for(int i=0;i< ldarrayorg.GetLength(0);++i)
            //{
            //    for(int j=0;j<ldarrayorg.GetLength(1);++j)
            //    {
            //        if (ldarray[i+ yf1, j+xf1].Count > 0)
            //        {
            //            ldarrayorg[i, j] = new List<beam>();
            //            ldarrayorg[i, j] = ldarray[i+yf1, j+xf1];
            //        }
            //    }
            //}

            //verifyimg.writearray(ldarray, ldstr);




            ldset result = new ldset();

            result.array = ldarrayscale;
            result.ldxstart = ldxstart;
            result.ldxend = ldxend;
            result.ldystart = ldystart;
            result.ldyend = ldyend;
            result.leftx = xf0;
            result.lefty = yf0;
            result.rightx = xf0 + ldarray.GetLength(1) - 1;
            result.righty = yf0 + ldarray.GetLength(0) - 1;
            //result.xoffsetorg = xf1;
            //result.yoffsetorg = yf1;
            //result.orgxnum = ldarrayorg.GetLength(1);
            //result.orgynum = ldarrayorg.GetLength(0);
            result.orgrownum = ldarray.GetLength(0);
            result.orgcolnum = ldarray.GetLength(1);

            return result;
        }
        public ldset readld_4level_opt(int xstart, int ystart, int xend, int yend, string stage, int expandldscale = 1)
        {
            //int ldxstart
            float cnt_scale = displacement / tiffimg.imgps;
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{

            t0 = (int)(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale);
            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xf0 = 0, yf0 = 0;
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                xf0 = -ldxstart;
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                yf0 = -ldystart;
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            //int xf1 = xstart - ldxstart,yf1 = ystart - ldystart;

            List<beam>[,] ldarray = new List<beam>[ldyend - ldystart + 1, ldxend - ldxstart + 1];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    ldarray[i, j] = new List<beam>();
                }
            }

            int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
            int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
            byte[] buffer = new byte[5000];
            beam b;
            BitArray bits;
            byte facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;

            if (ldxstart <= ldconfig1.xmax)
            {
                using (FileStream ldfile = new FileStream(ldfilenewpath1, FileMode.Open, FileAccess.Read))
                {
                    ldfile.Seek(5000 * (toffset), SeekOrigin.Begin);

                    //while (ldfile.Read(buffer, 0, 4000) > 0)

                    for (int t = toffset; t <= space + toffset; ++t)
                    {
                        if (ldfile.Read(buffer, 0, 5000) > 0)
                        {
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);

                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 40;
                                    int startbyte = s * 5;
                                    int ldnowy, ldnowx;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);


                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                    b = new beam();
                                                    b.spotorder = (ushort)(((ldnum / 2) * 8 + facet - 1));
                                                    b.spotnum = (ushort)s;

                                                    if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 3;

                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 2;
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                    {
                                                        b.eon = 1;
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else
                                                    {
                                                        b.eon = 0;
                                                    }

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 32;
                                    //startbyte += 4;
                                }

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

            //toffset = t0 + (int)(ldystart / cnt_scale);
            facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;

            if (ldxend > ldconfig1.xmax)
            {
                using (FileStream ldfile = new FileStream(ldfilenewpath2, FileMode.Open, FileAccess.Read))
                {
                    ldfile.Seek(5000 * (toffset), SeekOrigin.Begin);

                    //while (ldfile.Read(buffer, 0, 4000) > 0)
                    for (int t = toffset; t <= space + toffset; ++t)
                    {
                        if (ldfile.Read(buffer, 0, 5000) > 0)
                        {
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);
                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 40;
                                    int startbyte = s * 5;
                                    int ldnowy, ldnowx;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);


                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                    b = new beam();
                                                    b.spotorder = (ushort)(((ldnum / 2) * 8 + facet - 1) + 160);
                                                    b.spotnum = (ushort)s;

                                                    if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 3;
                                                    }
                                                    else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 2;
                                                    }
                                                    else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                    {
                                                        b.eon = 1;
                                                    }
                                                    else
                                                    {
                                                        b.eon = 0;
                                                    }


                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 24;
                                    //startbyte += 4;
                                }

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

            double scale = tiffimg.imgps / 0.125;

            int yfixedlen = yend - ystart + 1 + ((tempy * expandldscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandldscale) << 1);
            List<beam>[,] ldarrayscale = new List<beam>[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];

            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarrayscale[(int)((i + yf0) * scale + scaleoffset), (int)((j + xf0) * scale + scaleoffset)] = ldarray[i, j];
                }
            }

            hdf5 verifyimg = new hdf5();

            List<beam>[,] ldarray0 = new List<beam>[(int)(yfixedlen), (int)(xfixedlen)];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarray0[(int)((i + yf0)), (int)((j + xf0))] = ldarray[i, j];
                }
            }

            if (expandldscale == 2)
                verifyimg.writearray(ldarray0, optoutputfolder + "optld_" + stage + "stage", tiffimg.imgps);

            //hdf5 verifyimg = new hdf5();
            //string ldstr = outputpath + "ldimg_org";
            //List<beam>[,] ldarrayorg = new List<beam>[yend - ystart + 1, xend - xstart + 1];
            //for(int i=0;i< ldarrayorg.GetLength(0);++i)
            //{
            //    for(int j=0;j<ldarrayorg.GetLength(1);++j)
            //    {
            //        if (ldarray[i+ yf1, j+xf1].Count > 0)
            //        {
            //            ldarrayorg[i, j] = new List<beam>();
            //            ldarrayorg[i, j] = ldarray[i+yf1, j+xf1];
            //        }
            //    }
            //}

            //verifyimg.writearray(ldarray, ldstr);


            ldset result = new ldset();

            result.array = ldarrayscale;
            result.ldxstart = ldxstart;
            result.ldxend = ldxend;
            result.ldystart = ldystart;
            result.ldyend = ldyend;
            result.leftx = xf0;
            result.lefty = yf0;
            result.rightx = xf0 + ldarray.GetLength(1) - 1;
            result.righty = yf0 + ldarray.GetLength(0) - 1;
            //result.xoffsetorg = xf1;
            //result.yoffsetorg = yf1;
            //result.orgxnum = ldarrayorg.GetLength(1);
            //result.orgynum = ldarrayorg.GetLength(0);
            result.orgrownum = ldarray.GetLength(0);
            result.orgcolnum = ldarray.GetLength(1);

            return result;
        }

      

        public ldset readld_4level(int xstart, int ystart, int xend, int yend, string stage, int expandldscale = 1)
        {
            //int ldxstart
            float cnt_scale = displacement / tiffimg.imgps;
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{

            t0 = (int)(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale);
            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xf0 = 0, yf0 = 0;
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                xf0 = -ldxstart;
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                yf0 = -ldystart;
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            //int xf1 = xstart - ldxstart,yf1 = ystart - ldystart;

            List<beam>[,] ldarray = new List<beam>[ldyend - ldystart + 1, ldxend - ldxstart + 1];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    ldarray[i, j] = new List<beam>();
                }
            }

            int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
            int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
            byte[] buffer = new byte[5000];
            beam b;
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
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);

                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 40;
                                    int startbyte = s * 5;
                                    int ldnowy, ldnowx;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);


                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                    b = new beam();
                                                    b.spotorder = (ushort)(((ldnum / 2) * 8 + facet - 1));
                                                    b.spotnum = (ushort)s;

                                                    if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 3;

                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 2;
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                    {
                                                        b.eon = 1;
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                        //       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                    }
                                                    else
                                                    {
                                                        b.eon = 0;
                                                    }

                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 32;
                                    //startbyte += 4;
                                }

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

            //toffset = t0 + (int)(ldystart / cnt_scale);
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
                            //if (!buffer.All(x => x == 0))//test if all values are empty
                            {
                                bits = new BitArray(buffer);
                                ldoffset = 20000 * (facet - 1);
                                for (int s = 0; s < 1000; ++s)
                                //Parallel.For(0, 1000, s =>
                                {

                                    int startbit = s * 40;
                                    int startbyte = s * 5;
                                    int ldnowy, ldnowx;
                                    //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                    {
                                        for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                        {
                                            //if (bits[ldnum + startbit] == true)
                                            {
                                                ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);


                                                if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                {
                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //      (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);

                                                    //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                    //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                    b = new beam();
                                                    b.spotorder = (ushort)(((ldnum / 2) * 8 + facet - 1) + 160);
                                                    b.spotnum = (ushort)s;

                                                    if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 3;
                                                    }
                                                    else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                    {
                                                        b.eon = 2;
                                                    }
                                                    else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                    {
                                                        b.eon = 1;
                                                    }
                                                    else
                                                    {
                                                        b.eon = 0;
                                                    }


                                                    ldarray[ldnowy - ldystart, ldnowx - ldxstart].Add(b);
                                                }
                                                //result[-1 * ldnowy, ldnowx] += 1;
                                            }
                                        }
                                    }
                                    //startbit += 24;
                                    //startbyte += 4;
                                }

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

            double scale = tiffimg.imgps / 0.125;

            int yfixedlen = yend - ystart + 1 + ((tempy * expandldscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandldscale) << 1);
            List<beam>[,] ldarrayscale = new List<beam>[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];

            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarrayscale[(int)((i + yf0) * scale + scaleoffset), (int)((j + xf0) * scale + scaleoffset)] = ldarray[i, j];
                }
            }

            hdf5 verifyimg = new hdf5();

            List<beam>[,] ldarray0 = new List<beam>[(int)(yfixedlen), (int)(xfixedlen)];
            for (int i = 0; i < ldarray.GetLength(0); ++i)
            {
                for (int j = 0; j < ldarray.GetLength(1); ++j)
                {
                    if (ldarray[i, j].Count != 0)
                        ldarray0[(int)((i + yf0)), (int)((j + xf0))] = ldarray[i, j];
                }
            }

            if (expandldscale == 2)
                verifyimg.writearray(ldarray0, optoutputfolder + "orgld_" + stage + "stage", tiffimg.imgps);

            //hdf5 verifyimg = new hdf5();
            //string ldstr = outputpath + "ldimg_org";
            //List<beam>[,] ldarrayorg = new List<beam>[yend - ystart + 1, xend - xstart + 1];
            //for(int i=0;i< ldarrayorg.GetLength(0);++i)
            //{
            //    for(int j=0;j<ldarrayorg.GetLength(1);++j)
            //    {
            //        if (ldarray[i+ yf1, j+xf1].Count > 0)
            //        {
            //            ldarrayorg[i, j] = new List<beam>();
            //            ldarrayorg[i, j] = ldarray[i+yf1, j+xf1];
            //        }
            //    }
            //}

            //verifyimg.writearray(ldarray, ldstr);


            ldset result = new ldset();

            result.array = ldarrayscale;
            result.ldxstart = ldxstart;
            result.ldxend = ldxend;
            result.ldystart = ldystart;
            result.ldyend = ldyend;
            result.leftx = xf0;
            result.lefty = yf0;
            result.rightx = xf0 + ldarray.GetLength(1) - 1;
            result.righty = yf0 + ldarray.GetLength(0) - 1;
            //result.xoffsetorg = xf1;
            //result.yoffsetorg = yf1;
            //result.orgxnum = ldarrayorg.GetLength(1);
            //result.orgynum = ldarrayorg.GetLength(0);
            result.orgrownum = ldarray.GetLength(0);
            result.orgcolnum = ldarray.GetLength(1);

            return result;
        }
        public targetset readtarget(int xstart, int ystart, int xend, int yend)
        {
            string orgimgname = outputpath + tiffimg.imgfilename + "_img.h5";
            hdf5 orgimg = new hdf5();
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int xf0 = 0, yf0 = 0;
            int xstartexp = xstart - tempx * expandscale;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;

            xexp = tempx * expandscale;
            yexp = tempy * expandscale;
            yexpscale = (int)(yexp * scale);
            xexpscale = (int)(xexp * scale);

            //hdf5 verify = new hdf5();
            //string targetstr = outputpath + "test\\targetorg", targetstr0 = outputpath + "test\\targetscale";
            orgimg.openfile(orgimgname);
            int yf00 = 0, xf00 = 0;

            int xstartpad = xstartexp - 1;
            if (xstartpad < 0)
            {
                xstartpad = 0;
                xf00 = 1;
            }
            int ystartpad = ystartexp - 1;
            if (ystartpad < 0)
            {
                ystartpad = 0;
                yf00 = 1;
            }
            int xendpad = xendexp + 1;
            if (xendpad >= tiffimg.width)
            {
                xendpad = tiffimg.width - 1;
            }
            int yendpad = yendexp + 1;
            if (yendpad >= tiffimg.height)
            {
                yendpad = tiffimg.height - 1;
            }
            byte[,] target = orgimg.readbytedata(ystartpad, xstartpad, yendpad - ystartpad + 1, xendpad - xstartpad + 1);
            byte[,] targetpad = new byte[yendexp - ystartexp + 3, xendexp - xstartexp + 3];
            for (int i = 0; i < target.GetLength(0); ++i)
            {
                for (int j = 0; j < target.GetLength(1); ++j)
                {
                    targetpad[i + yf00, j + xf00] = target[i, j];
                }
            }

            interp inte = new interp();
            //byte[,] targetscale = inte.nearestscale(targetpad, xendexp - xstartexp + 1, yendexp - ystartexp + 1, (int)(tiffimg.imgps / 0.125f), (int)(tiffimg.imgps / 0.125f));
            float[,] targetscale0 = inte.linearscale(targetpad, xendexp - xstartexp + 1, yendexp - ystartexp + 1, (int)(tiffimg.imgps / 0.125f), (int)(tiffimg.imgps / 0.125f));
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            byte[,] targetscale = new byte[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < targetscale0.GetLength(0); ++i)
            {
                for (int j = 0; j < targetscale0.GetLength(1); ++j)
                {
                    if (targetscale0[i, j] >= 0.5)
                        targetscale[i + yf0, j + xf0] = 1;
                }
            }
            //float[,] targetscale0 = inte.linearscale(target, target.GetLength(1), target.GetLength(0), (int)(tiffimg.imgps / 0.125f), (int)(tiffimg.imgps / 0.125f));
            //verify.writearray(targetpad, targetstr); 
            //verify.writearray(targetscale, targetstr0);

            //hdf5 verifyimg = new hdf5();
            //string tstr = outputpath + "targetimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (ldxstart - xstartpad), yf1 = (ldystart - ystartpad);

            byte[,] targetorg = new byte[(ldyend - ldystart + 1), (ldxend - ldxstart + 1)];
            for (int i = 0; i < targetorg.GetLength(0); ++i)
            {
                for (int j = 0; j < targetorg.GetLength(1); ++j)
                {
                    targetorg[i, j] = target[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(targetorg, tstr);

            targetset result = new targetset();
            result.array = targetscale;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;



            return result;
        }

        public lossset computeloss(dosageset d, targetset target)
        {
            byte[,] zloss = new byte[target.array.GetLength(0), target.array.GetLength(1)];
            //byte[,] optregion = new byte[target.array.GetLength(0), target.array.GetLength(1)];
            int loss = 0;
            int lossinsidetarget = 0;
            int lossroi = 0;
            List<int> lossposx = new List<int>();
            List<int> lossposy = new List<int>();
            for (int i = 0; i < target.array.GetLength(0); ++i)
            {
                for (int j = 0; j < target.array.GetLength(1); ++j)
                {
                    if (d.array[i, j] >= threshold)
                    {
                        if (target.array[i, j] == 0)
                        {
                            ++loss;
                            zloss[i, j] = 1;
                        }
                    }
                    else
                    {
                        if (target.array[i, j] == 1)
                        {
                            loss += lossweight;
                            ++lossinsidetarget;
                            zloss[i, j] = (byte)lossweight;
                        }
                    }
                }
            }

            for (int i = yexpscale; i < roirownum * scale + yexpscale; ++i)//yexpscale是原始大小
            {
                for (int j = xexpscale; j < roicolnum * scale + xexpscale; ++j)
                {
                    if (zloss[i, j] != 0)
                    {
                        ++lossroi;
                        lossposx.Add(j);//這是原始ROI大小的LOSS，最佳化要進行優化的點
                        lossposy.Add(i);
                    }

                    //optregion[i, j] = 1;
                }
            }


            lossset result = new lossset();

            result.zloss = zloss;
            result.lossvalue = loss;
            result.lossposx = lossposx;
            result.lossposy = lossposy;
            result.lossinsidetarget = lossinsidetarget;
            result.lossroi = lossroi;
            //result.optergion = optregion;
            return result;
        }
        //public int computeloss_int(byte[,] zloss)
        //{

        //    int loss = 0;

        //    for (int i = yexpscale; i < roirownum * scale + yexpscale; ++i)//yexpscale是原始大小
        //    {
        //        for (int j = xexpscale; j < roicolnum * scale + xexpscale; ++j)
        //        {
        //            //loss += zloss[i, j];
        //            loss += zloss[i, j];
        //        }
        //    }


        //    lossset result = new lossset();

        //    return loss;
        //}
        public void ld2to4()
         {
            byte[] buffer = new byte[4000], newbuffer;

            BitArray bits, newbits = new BitArray(5000 * 8);


            using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))
            {
                using (FileStream ldfile4 = new FileStream(ldfilepath1_4level, FileMode.Create, FileAccess.Write))
                //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                //while (ldfile.Read(buffer, 0, 4000) > 0)

                //for (int t = 0; ; ++t)
                {
                    while (ldfile.Read(buffer, 0, 4000) > 0)
                    {
                        //if (!buffer.All(x => x == 0))//test if all values are empty
                        {
                            newbuffer = new byte[5000];

                            bits = new BitArray(buffer);

                            for (int i = 0; i < 4000; ++i)
                            {
                                if (buffer[i] > 0)
                                {
                                    int byteoffset = (i / 4) * 5, bitoffset = (i % 4) << 3, laseridx, laseridx1, laseridx2, bitpos;
                                    for (int j = 0; j < 8; ++j)
                                    {
                                        if (bits[i * 8 + j])
                                        {
                                            laseridx = (bitoffset + j);
                                            laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                            bitpos = (laseridx % 4) * 2;
                                            laseridx2 = (1 << (bitpos + 1)) + (1 << bitpos);
                                            newbuffer[byteoffset + laseridx1] += (byte)laseridx2;
                                        }
                                    }
                                }
                            }

                            ldfile4.Write(newbuffer, 0, newbuffer.Length);
                        }
                    }
                }

            }

            using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
            {
                using (FileStream ldfile4 = new FileStream(ldfilepath2_4level, FileMode.Create, FileAccess.Write))
                //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                //while (ldfile.Read(buffer, 0, 4000) > 0)

                //for (int t = 0; ; ++t)
                {
                    while (ldfile.Read(buffer, 0, 4000) > 0)
                    {
                        //if (!buffer.All(x => x == 0))//test if all values are empty
                        {
                            newbuffer = new byte[5000];

                            bits = new BitArray(buffer);

                            for (int i = 0; i < 4000; ++i)
                            {
                                if (buffer[i] > 0)
                                {
                                    int byteoffste = (i >> 2) * 5, bitoffset = (i % 4) << 3, laseridx, laseridx1, laseridx2, bitpos;
                                    for (int j = 0; j < 8; ++j)
                                    {
                                        if (bits[i * 8 + j])
                                        {
                                            laseridx = (bitoffset + j);
                                            laseridx1 = (int)Math.Floor((double)laseridx / 4);
                                            bitpos = (laseridx % 4) * 2;
                                            laseridx2 = (1 << (bitpos + 1)) + (1 << bitpos);
                                            newbuffer[byteoffste + laseridx1] += (byte)laseridx2;
                                        }
                                    }
                                }
                            }

                            ldfile4.Write(newbuffer, 0, newbuffer.Length);
                        }
                    }
                }

            }

        }


        public dosageset readdosage(int xstart, int ystart, int xend, int yend)
        {
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            float cnt_scale = displacement / tiffimg.imgps;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            t0 = (int)Math.Round(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale, MidpointRounding.AwayFromZero);

            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}

            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xstartexp = xstart - tempx * expandscale, xf0 = 0, yf0 = 0;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;



            float[,] dosage = dosageroi0125(ystartexp, xstartexp, yendexp - ystartexp + 1, xendexp - xstartexp + 1);
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            float[,] dosageexp = new float[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < dosage.GetLength(0); ++i)
            {
                for (int j = 0; j < dosage.GetLength(1); ++j)
                {
                    dosageexp[i + yf0, j + xf0] = dosage[i, j];
                }
            }

            hdf5 verifyimg = new hdf5();
            //string dstr = outputpath + "dosageimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (int)((ldxstart - xstart + tempx * expandscale) * scale), yf1 = (int)((ldystart - ystart + tempy * expandscale) * scale);



            float[,] darrayorg = new float[(int)((ldyend - ldystart + 1) * scale), (int)((ldxend - ldxstart + 1) * scale)];
            for (int i = 0; i < darrayorg.GetLength(0); ++i)
            {
                for (int j = 0; j < darrayorg.GetLength(1); ++j)
                {
                    darrayorg[i, j] = dosageexp[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(darrayorg, dstr);


            dosageset result = new dosageset();
            result.array = dosageexp;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;
            result.xf1 = xf1;
            result.yf1 = yf1;
            result.xnum = darrayorg.GetLength(1);
            result.ynum = darrayorg.GetLength(0);

            return result;
        }
        public dosageset readdosage(int xstart, int ystart, int xend, int yend, string beamfile1path, string beamfile2path)
        {
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            float cnt_scale = displacement / tiffimg.imgps;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            t0 = (int)Math.Round(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale, MidpointRounding.AwayFromZero);

            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}

            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xstartexp = xstart - tempx * expandscale, xf0 = 0, yf0 = 0;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;



            float[,] dosage = dosageroi0125(ystartexp, xstartexp, yendexp - ystartexp + 1, xendexp - xstartexp + 1, beamfile1path, beamfile2path);
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            float[,] dosageexp = new float[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < dosage.GetLength(0); ++i)
            {
                for (int j = 0; j < dosage.GetLength(1); ++j)
                {
                    dosageexp[i + yf0, j + xf0] = dosage[i, j];
                }
            }

            hdf5 verifyimg = new hdf5();
            //string dstr = outputpath + "dosageimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (int)((ldxstart - xstart + tempx * expandscale) * scale), yf1 = (int)((ldystart - ystart + tempy * expandscale) * scale);



            float[,] darrayorg = new float[(int)((ldyend - ldystart + 1) * scale), (int)((ldxend - ldxstart + 1) * scale)];
            for (int i = 0; i < darrayorg.GetLength(0); ++i)
            {
                for (int j = 0; j < darrayorg.GetLength(1); ++j)
                {
                    darrayorg[i, j] = dosageexp[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(darrayorg, dstr);


            dosageset result = new dosageset();
            result.array = dosageexp;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;
            result.xf1 = xf1;
            result.yf1 = yf1;
            result.xnum = darrayorg.GetLength(1);
            result.ynum = darrayorg.GetLength(0);

            return result;
        }
        public dosageset readdosage_opt(int xstart, int ystart, int xend, int yend)
        {
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            float cnt_scale = displacement / tiffimg.imgps;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            t0 = (int)Math.Round(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale, MidpointRounding.AwayFromZero);

            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}

            Tuple<float, float> yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            float ydistance = yinterval.Item1 - yinterval.Item2;
            int xstartexp = xstart - tempx * expandscale, xf0 = 0, yf0 = 0;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;



            float[,] dosage = dosageroi0125_opt(ystartexp, xstartexp, yendexp - ystartexp + 1, xendexp - xstartexp + 1);
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            float[,] dosageexp = new float[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < dosage.GetLength(0); ++i)
            {
                for (int j = 0; j < dosage.GetLength(1); ++j)
                {
                    dosageexp[i + yf0, j + xf0] = dosage[i, j];
                }
            }

            hdf5 verifyimg = new hdf5();
            //string dstr = outputpath + "dosageimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (int)((ldxstart - xstart + tempx * expandscale) * scale), yf1 = (int)((ldystart - ystart + tempy * expandscale) * scale);



            float[,] darrayorg = new float[(int)((ldyend - ldystart + 1) * scale), (int)((ldxend - ldxstart + 1) * scale)];
            for (int i = 0; i < darrayorg.GetLength(0); ++i)
            {
                for (int j = 0; j < darrayorg.GetLength(1); ++j)
                {
                    darrayorg[i, j] = dosageexp[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(darrayorg, dstr);


            dosageset result = new dosageset();
            result.array = dosageexp;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;
            result.xf1 = xf1;
            result.yf1 = yf1;
            result.xnum = darrayorg.GetLength(1);
            result.ynum = darrayorg.GetLength(0);

            return result;
        }
        public dosageset readdosage_4level(int xstart, int ystart, int xend, int yend)
        {
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            //t0 = Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            //Tuple<int, int> yinterval = new Tuple<int, int>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            //int ydistance = yinterval.Item1 - yinterval.Item2;
            int xstartexp = xstart - tempx * expandscale, xf0 = 0, yf0 = 0;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;

            float[,] dosage = dosageroi0125_4level(ystartexp, xstartexp, yendexp - ystartexp + 1, xendexp - xstartexp + 1);
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            float[,] dosageexp = new float[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < dosage.GetLength(0); ++i)
            {
                for (int j = 0; j < dosage.GetLength(1); ++j)
                {
                    dosageexp[i + yf0, j + xf0] = dosage[i, j];
                }
            }

            //hdf5 verifyimg = new hdf5();
            //string dstr = outputpath + "dosageimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (int)((ldxstart - xstart + tempx * expandscale) * scale), yf1 = (int)((ldystart - ystart + tempy * expandscale) * scale);



            float[,] darrayorg = new float[(int)((ldyend - ldystart + 1) * scale), (int)((ldxend - ldxstart + 1) * scale)];
            for (int i = 0; i < darrayorg.GetLength(0); ++i)
            {
                for (int j = 0; j < darrayorg.GetLength(1); ++j)
                {
                    darrayorg[i, j] = dosageexp[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(darrayorg, dstr);


            dosageset result = new dosageset();
            result.array = dosageexp;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;
            result.xf1 = xf1;
            result.yf1 = yf1;
            result.xnum = darrayorg.GetLength(1);
            result.ynum = darrayorg.GetLength(0);

            return result;
        }
        public dosageset readdosage_4level_opt(int xstart, int ystart, int xend, int yend)
        {
            //int tempx = 0, tempy = 0;//tempx = halfcol/scale, tempy = halfrow/scale;
            int t0 = 0;//t0 = ldfile.ymin
            bool lightengine2 = false;
            if (tiffimg.width - 1 > ldconfig1.xmax)
                lightengine2 = true;
            //if (lightengine2 == true)
            //{
            //t0 = Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //space = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);
            //}



            //Tuple<int, int> yinterval = new Tuple<int, int>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
            //int ydistance = yinterval.Item1 - yinterval.Item2;
            int xstartexp = xstart - tempx * expandscale, xf0 = 0, yf0 = 0;
            if (xstartexp < 0)
            {
                xf0 = (int)(-xstartexp * scale);
                xstartexp = 0;
            }
            int ystartexp = ystart - tempy * expandscale;
            if (ystartexp < 0)
            {
                yf0 = (int)(-ystartexp * scale);
                ystartexp = 0;
            }
            int xendexp = xend + tempx * expandscale;
            if (xendexp >= tiffimg.width)
                xendexp = tiffimg.width - 1;

            int yendexp = yend + tempy * expandscale;
            if (yendexp >= tiffimg.height)
                yendexp = tiffimg.height - 1;

            float[,] dosage = dosageroi0125_4level_opt(ystartexp, xstartexp, yendexp - ystartexp + 1, xendexp - xstartexp + 1);
            int yfixedlen = yend - ystart + 1 + ((tempy * expandscale) << 1), xfixedlen = xend - xstart + 1 + ((tempx * expandscale) << 1);
            float[,] dosageexp = new float[(int)(yfixedlen * scale), (int)(xfixedlen * scale)];
            for (int i = 0; i < dosage.GetLength(0); ++i)
            {
                for (int j = 0; j < dosage.GetLength(1); ++j)
                {
                    dosageexp[i + yf0, j + xf0] = dosage[i, j];
                }
            }

            //hdf5 verifyimg = new hdf5();
            //string dstr = outputpath + "dosageimg_org";
            int ldxstart = xstart - tempx * expandldscale;
            if (ldxstart < 0)
            {
                ldxstart = 0;
            }
            int ldystart = ystart - tempy * expandldscale;
            if (ldystart < 0)
            {
                ldystart = 0;
            }
            int ldxend = xend + tempx * expandldscale;
            if (ldxend >= tiffimg.width)
                ldxend = tiffimg.width - 1;

            int ldyend = yend + tempy * expandldscale;
            if (ldyend >= tiffimg.height)
                ldyend = tiffimg.height - 1;

            int xf1 = (int)((ldxstart - xstart + tempx * expandscale) * scale), yf1 = (int)((ldystart - ystart + tempy * expandscale) * scale);



            float[,] darrayorg = new float[(int)((ldyend - ldystart + 1) * scale), (int)((ldxend - ldxstart + 1) * scale)];
            for (int i = 0; i < darrayorg.GetLength(0); ++i)
            {
                for (int j = 0; j < darrayorg.GetLength(1); ++j)
                {
                    darrayorg[i, j] = dosageexp[i + yf1, j + xf1];
                }
            }

            //verifyimg.writearray(darrayorg, dstr);


            dosageset result = new dosageset();
            result.array = dosageexp;
            result.xstart = xstartexp;
            result.xend = xendexp;
            result.ystart = ystartexp;
            result.yend = yendexp;
            result.xf1 = xf1;
            result.yf1 = yf1;
            result.xnum = darrayorg.GetLength(1);
            result.ynum = darrayorg.GetLength(0);

            return result;
        }
        public void copyld(List<beam>[,] ld, List<beam>[,] newld)
        {

            beam b;
            for (int i = 0; i < ld.GetLength(0); ++i)
            {
                for (int j = 0; j < ld.GetLength(1); ++j)
                {
                    if (ld[i, j] != null)
                    {
                        //if (tld[j, i].Count != bestld[j0, i0].Count)
                        //    ;

                        newld[i, j] = new List<beam>();
                        for (int k = 0; k < ld[i, j].Count; ++k)
                        {
                            //if (tld[j, i][k].spotnum != bestld[j0, i0][k].spotnum)
                            //    ;
                            //if (tld[j, i][k].spotorder != bestld[j0, i0][k].spotorder)
                            //    ;

                            b = new beam();
                            b.eon = ld[i, j][k].eon;
                            b.spotnum = ld[i, j][k].spotnum;
                            b.spotorder = ld[i, j][k].spotorder;
                            newld[i, j].Add(b);

                            //tld[j, i][k].on = bestld[j0, i0][k].on;
                        }
                    }
                }
            }
        }

        public void outputlist(List<int> list, string path)
        {
            using (StreamWriter outputFile = new StreamWriter(path))
            {
                foreach (int i in list)
                    outputFile.WriteLine(i);
            }
        }

        public List<beam>[,] scaleld(ldset tld)
        {
            List<beam>[,] tldorg = new List<beam>[tld.orgrownum, tld.orgcolnum];
            beam b;
            for (int i0 = 0; i0 < tld.orgrownum; ++i0)
            {
                for (int j0 = 0; j0 < tld.orgcolnum; ++j0)
                {
                    int i = (int)((i0 + tld.lefty) * scale + scaleoffset);
                    int j = (int)((j0 + tld.leftx) * scale + scaleoffset);

                    if (tld.array[i, j] != null)
                    {
                        tldorg[i0, j0] = new List<beam>();

                        for (int k = 0; k < tld.array[i, j].Count; ++k)
                        {
                            //if (tld[j, i][k].spotnum != bestld[j0, i0][k].spotnum)
                            //    ;
                            //if (tld[j, i][k].spotorder != bestld[j0, i0][k].spotorder)
                            //    ;

                            b = new beam();
                            b.eon = tld.array[i, j][k].eon;
                            b.spotnum = tld.array[i, j][k].spotnum;
                            b.spotorder = tld.array[i, j][k].spotorder;
                            tldorg[i0, j0].Add(b);

                            //tld[j, i][k].on = bestld[j0, i0][k].on;
                        }
                    }

                    //tldorg[i, j] = tld.array[(int)((i + tld.lefty) * scale + scaleoffset), (int)((j + tld.leftx) * scale + scaleoffset)];
                }
            }


            hdf5 verifyimg = new hdf5();
            //string ldstr = outputpath + "ldimg_new";
            verifyimg.writearrayall(tldorg, outputpath + "optimization\\" + "optld_" + xlefttop.ToString() + "_" + ylefttop.ToString() + "_" + xrightbottom.ToString() + "_" + yrightbottom.ToString());
            //verifyimg.writearray(tldorg, ldstr);

            return tldorg;
        }

        public void writenewldfile(ldset tld)
        {
            List<beam>[,] tldorg = scaleld(tld);

            float cnt_scale = displacement / tiffimg.imgps;
            //string ldfilenewpath1 = outputpath+ "_LE1_2level_opt.LD", ldfilenewpath2 = outputpath+ "_LE1_2level_opt.LD";

            int t0 = (int)Math.Round(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale, MidpointRounding.AwayFromZero);
            int space = (int)Math.Round((Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin)) / cnt_scale, MidpointRounding.AwayFromZero);

            float space0 = Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin);


            //byte[,] output1 = new byte[(t0 + space + tiffimg.height) * 1000, 4];

            int toffset = t0 + (int)(tld.ldystart / cnt_scale + 0.5), ldoffset;
            byte[] buffer = new byte[4000];

            BitArray bits;
            byte facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;
            //if (tld.ldxstart <= ldconfig1.xmax)
            {
                using (FileStream ldfilenew = File.Create(ldfilenewpath1))
                {
                    using (FileStream ldfile = new FileStream(ldfilepath1, FileMode.Open, FileAccess.Read))
                    {
                        for (int i = 0; i < toffset; ++i)
                        {
                            if (ldfile.Read(buffer, 0, 4000) > 0)
                            {
                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                        }

                        //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                        //while (ldfile.Read(buffer, 0, 4000) > 0)
                        for (int t = toffset; t <= ((tld.ldyend - tld.ldystart + space) / cnt_scale + 0.5) + toffset; ++t)
                        {
                            if (ldfile.Read(buffer, 0, 4000) > 0)
                            {
                                //if (!buffer.All(x => x == 0))//test if all values are empty
                                {
                                    bits = new BitArray(buffer);
                                    ldoffset = 20000 * (facet - 1);
                                    //for (int s = 0; s < 1000; ++s)
                                    Parallel.For(0, 1000, s =>
                                    {
                                        int startbit = s * 32;
                                        int startbyte = s * 4;
                                        int ldnowy, ldnowx;
                                        //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                        {
                                            for (int ldnum = 0; ldnum < 20; ++ldnum)
                                            {
                                                //if (bits[ldnum + startbit] == true)
                                                {
                                                    ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                    ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);

                                                    if (tld.ldxstart <= ldnowx && ldnowx <= tld.ldxend && tld.ldystart <= ldnowy && tld.ldyend >= ldnowy)
                                                    {
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //          (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                        List<beam> listnow = tldorg[ldnowy - tld.ldystart, ldnowx - tld.ldxstart];
                                                        int spotorder = ldnum * 8 + facet - 1;
                                                        if (listnow != null)
                                                        {
                                                            for (int i = 0; i < listnow.Count; ++i)
                                                            {
                                                                if (listnow[i].spotorder == spotorder && listnow[i].spotnum == s)
                                                                {
                                                                    if (bits[ldnum + startbit] == true && listnow[i].eon == 0)
                                                                    {
                                                                        buffer[startbyte + ldnum / 8] -= (byte)(1 << (ldnum % 8));
                                                                    }
                                                                    else if (bits[ldnum + startbit] == false && listnow[i].eon == 1)
                                                                    {
                                                                        buffer[startbyte + ldnum / 8] += (byte)(1 << (ldnum % 8));
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //result[-1 * ldnowy, ldnowx] += 1;
                                                }
                                            }
                                        }
                                        //startbit += 32;
                                        //startbyte += 4;
                                    });

                                }
                                if (facet == 8)
                                    facet = 1;
                                else
                                    ++facet;

                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (ldfile.Read(buffer, 0, 4000) > 0)
                        {
                            ldfilenew.Write(buffer, 0, buffer.Length);
                        }

                    }
                }
            }

            facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;
            //if (tld.ldxend > ldconfig1.xmax)
            {
                using (FileStream ldfilenew = File.Create(ldfilenewpath2))
                {
                    using (FileStream ldfile = new FileStream(ldfilepath2, FileMode.Open, FileAccess.Read))
                    {
                        for (int i = 0; i < toffset; ++i)
                        {
                            if (ldfile.Read(buffer, 0, 4000) > 0)
                            {
                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                        }
                        //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                        //while (ldfile.Read(buffer, 0, 4000) > 0)
                        for (int t = toffset; t < ((tld.ldyend - tld.ldystart + space) / cnt_scale + 0.5) + toffset; ++t)
                        {
                            if (ldfile.Read(buffer, 0, 4000) > 0)
                            {

                                //startbit = 0;
                                //startbyte = 0;
                                //if (!buffer.All(x => x == 0))//test if all values are empty
                                {
                                    bits = new BitArray(buffer);
                                    ldoffset = 20000 * (facet - 1);
                                    //for (int s = 0; s < 1000; ++s)
                                    Parallel.For(0, 1000, s =>
                                    {
                                        int startbit = s * 32;
                                        int startbyte = s * 4;
                                        int ldnowy, ldnowx;
                                        //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                        {
                                            for (int ldnum = 0; ldnum < 20; ++ldnum)
                                            {
                                                if (bits[ldnum + startbit] == true)
                                                {
                                                    ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);
                                                    ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + ldnum * 1000 + s], MidpointRounding.AwayFromZero);


                                                    if (tld.ldxstart <= ldnowx && ldnowx <= tld.ldxend && tld.ldystart <= ldnowy && tld.ldyend >= ldnowy)
                                                    {
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //          (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                        List<beam> listnow = tldorg[ldnowy - tld.ldystart, ldnowx - tld.ldxstart];
                                                        int spotorder = ldnum * 8 + facet - 1 + 160;
                                                        if (listnow != null)
                                                        {
                                                            for (int i = 0; i < listnow.Count; ++i)
                                                            {
                                                                if (listnow[i].spotorder == spotorder && listnow[i].spotnum == s)
                                                                {
                                                                    if (bits[ldnum + startbit] == true && listnow[i].eon == 0)
                                                                    {
                                                                        buffer[startbyte + ldnum / 8] -= (byte)(1 << (ldnum % 8));
                                                                    }
                                                                    else if (bits[ldnum + startbit] == false && listnow[i].eon == 1)
                                                                    {
                                                                        buffer[startbyte + ldnum / 8] += (byte)(1 << (ldnum % 8));
                                                                    }
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //result[-1 * ldnowy, ldnowx] += 1;
                                                }
                                            }
                                        }
                                        //startbit += 24;
                                        //startbyte += 4;
                                    });

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

                        while (ldfile.Read(buffer, 0, 4000) > 0)
                        {
                            ldfilenew.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }


        }

        public void writenewldfile_4level(ldset tld)
        {
            List<beam>[,] tldorg = scaleld(tld);

            float cnt_scale = displacement / tiffimg.imgps;
            //string ldfilenewpath1 = outputpath + "ldfile1_4level_new.ld", ldfilenewpath2 = outputpath + "ldfile2_4level_new.ld";
            int t0 = (int)Math.Round(Math.Min(ldconfig1.ymin, ldconfig2.ymin) / cnt_scale, MidpointRounding.AwayFromZero);
            int space = (int)Math.Round((Math.Max(ldconfig1.ymax, ldconfig2.ymax) - Math.Min(ldconfig1.ymin, ldconfig2.ymin)) / cnt_scale, MidpointRounding.AwayFromZero);

            //byte[,] output1 = new byte[(t0 + space + tiffimg.height) * 1000, 4];

            int toffset = t0 + (int)(tld.ldystart / cnt_scale + 0.5), ldoffset;
            byte[] buffer = new byte[5000];

            BitArray bits;
            byte facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;
            //if (tld.ldxstart <= ldconfig1.xmax)
            {
                using (FileStream ldfilenew = File.Create(ldfilenewpath1))
                {
                    using (FileStream ldfile = new FileStream(ldfilepath1_4level, FileMode.Open, FileAccess.Read))
                    {
                        for (int i = 0; i < toffset; ++i)
                        {
                            if (ldfile.Read(buffer, 0, 5000) > 0)
                            {
                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                        }

                        //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                        //while (ldfile.Read(buffer, 0, 4000) > 0)
                        for (int t = toffset; t <= ((tld.ldyend - tld.ldystart + space) / cnt_scale + 0.5) + toffset; ++t)
                        {
                            if (ldfile.Read(buffer, 0, 5000) > 0)
                            {
                                //if (!buffer.All(x => x == 0))//test if all values are empty
                                {
                                    bits = new BitArray(buffer);
                                    ldoffset = 20000 * (facet - 1);
                                    //for (int s = 0; s < 1000; ++s)
                                    Parallel.For(0, 1000, s =>
                                    {
                                        int startbit = s * 40;
                                        int startbyte = s * 5;
                                        int ldnowy, ldnowx;
                                        //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                        {
                                            for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                            {
                                                //if (bits[ldnum + startbit] == true)
                                                {
                                                    ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                    ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                    if (tld.ldxstart <= ldnowx && ldnowx <= tld.ldxend && tld.ldystart <= ldnowy && tld.ldyend >= ldnowy)
                                                    {
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //          (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                        List<beam> listnow = tldorg[ldnowy - tld.ldystart, ldnowx - tld.ldxstart];
                                                        int spotorder = (ldnum / 2) * 8 + facet - 1;
                                                        if (listnow != null)
                                                        {
                                                            for (int i = 0; i < listnow.Count; ++i)
                                                            {
                                                                if (listnow[i].spotorder == spotorder && listnow[i].spotnum == s)
                                                                {
                                                                    if (listnow[i].eon == 1)
                                                                    {
                                                                        if (beamonlevel == 3)
                                                                        {
                                                                            if (bits[ldnum + startbit] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                        else if (beamonlevel == 2)
                                                                        {
                                                                            if (bits[ldnum + startbit] != false)
                                                                                buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                        else if (beamonlevel == 1)
                                                                        {
                                                                            if (bits[ldnum + startbit] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != false)
                                                                                buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                    }
                                                                    else if (listnow[i].eon == 0)
                                                                    {
                                                                        if (bits[ldnum + startbit] != false)
                                                                            buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                        if (bits[ldnum + startbit + 1] != false)
                                                                            buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                    }


                                                                    //if ((bits[ldnum + startbit] != true|| bits[ldnum + startbit+1] != true)&&listnow[i].on == 3)
                                                                    //{
                                                                    //    buffer[startbyte + (ldnum/2) / 4] -= (byte)(1 << (((ldnum/2) % 4) <<1)) + ;
                                                                    //}
                                                                    //else if (bits[ldnum + startbit] == false&&bits[ldnum + startbit+1] == true && listnow[i].on == 1)
                                                                    //{
                                                                    //    buffer[startbyte + ldnum / 8] += (byte)(1 << (ldnum % 8));
                                                                    //}
                                                                    //break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //result[-1 * ldnowy, ldnowx] += 1;
                                                }
                                            }
                                        }
                                        //startbit += 32;
                                        //startbyte += 4;
                                    });

                                }
                                if (facet == 8)
                                    facet = 1;
                                else
                                    ++facet;

                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                            else
                            {
                                break;
                            }
                        }

                        while (ldfile.Read(buffer, 0, 5000) > 0)
                        {
                            ldfilenew.Write(buffer, 0, buffer.Length);
                        }

                    }
                }
            }

            facet = (byte)(toffset % 8);
            if (facet == 0)
                facet = 8;
            //if (tld.ldxend > ldconfig1.xmax)
            {
                using (FileStream ldfilenew = File.Create(ldfilenewpath2))
                {
                    using (FileStream ldfile = new FileStream(ldfilepath2_4level, FileMode.Open, FileAccess.Read))
                    {
                        for (int i = 0; i < toffset; ++i)
                        {
                            if (ldfile.Read(buffer, 0, 5000) > 0)
                            {
                                ldfilenew.Write(buffer, 0, buffer.Length);
                            }
                        }
                        //ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                        //while (ldfile.Read(buffer, 0, 4000) > 0)
                        for (int t = toffset; t < ((tld.ldyend - tld.ldystart + space) / cnt_scale + 0.5) + toffset; ++t)
                        {
                            if (ldfile.Read(buffer, 0, 5000) > 0)
                            {

                                //startbit = 0;
                                //startbyte = 0;
                                //if (!buffer.All(x => x == 0))//test if all values are empty
                                {
                                    bits = new BitArray(buffer);
                                    ldoffset = 20000 * (facet - 1);
                                    //for (int s = 0; s < 1000; ++s)
                                    Parallel.For(0, 1000, s =>
                                    {
                                        int startbit = s * 40;
                                        int startbyte = s * 5;
                                        int ldnowy, ldnowx;
                                        //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
                                        {
                                            for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                            {
                                                //if (bits[ldnum + startbit] == true)
                                                {
                                                    ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                    ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                    if (tld.ldxstart <= ldnowx && ldnowx <= tld.ldxend && tld.ldystart <= ldnowy && tld.ldyend >= ldnowy)
                                                    {
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowx - ldxstart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowy - ldystart) * scalecol + 0.5) + halfcol, spots1[facet * 20 + ldnum], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //          (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots2[ldnum * 8 + facet - 1], s);
                                                        //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                        //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                        List<beam> listnow = tldorg[ldnowy - tld.ldystart, ldnowx - tld.ldxstart];
                                                        int spotorder = (ldnum / 2) * 8 + facet - 1 + 160;
                                                        if (listnow != null)
                                                        {
                                                            for (int i = 0; i < listnow.Count; ++i)
                                                            {
                                                                if (listnow[i].spotorder == spotorder && listnow[i].spotnum == s)
                                                                {
                                                                    if (listnow[i].eon == 1)
                                                                    {
                                                                        if (beamonlevel == 3)
                                                                        {
                                                                            if (bits[ldnum + startbit] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                        else if (beamonlevel == 2)
                                                                        {
                                                                            if (bits[ldnum + startbit] != false)
                                                                                buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                        else if (beamonlevel == 1)
                                                                        {
                                                                            if (bits[ldnum + startbit] != true)
                                                                                buffer[startbyte + (ldnum / 2) / 4] += (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                            if (bits[ldnum + startbit + 1] != false)
                                                                                buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                        }
                                                                    }
                                                                    else if (listnow[i].eon == 0)
                                                                    {
                                                                        if (bits[ldnum + startbit] != false)
                                                                            buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1));
                                                                        if (bits[ldnum + startbit + 1] != false)
                                                                            buffer[startbyte + (ldnum / 2) / 4] -= (byte)(1 << (((ldnum / 2) % 4) << 1) + 1);
                                                                    }

                                                                    //if (bits[ldnum + startbit] == true && listnow[i].on == 0)
                                                                    //{
                                                                    //    buffer[startbyte + ldnum / 8] -= (byte)(1 << (ldnum % 8));
                                                                    //}
                                                                    //else if (bits[ldnum + startbit] == false && listnow[i].on == 1)
                                                                    //{
                                                                    //    buffer[startbyte + ldnum / 8] += (byte)(1 << (ldnum % 8));
                                                                    //}
                                                                    //break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    //result[-1 * ldnowy, ldnowx] += 1;
                                                }
                                            }
                                        }
                                        //startbit += 24;
                                        //startbyte += 4;
                                    });

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

                        while (ldfile.Read(buffer, 0, 5000) > 0)
                        {
                            ldfilenew.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
            }


        }
        
        public void updatelosspos(lossset loss)
        {
            loss.lossposx.Clear();
            loss.lossposy.Clear();
            for (int i = yexpscale; i < roirownum * scale + yexpscale; ++i)
            {
                for (int j = xexpscale; j < roicolnum * scale + xexpscale; ++j)
                {
                    if (loss.zloss[i, j] != 0)
                    {
                        loss.lossposx.Add(j);
                        loss.lossposy.Add(i);
                    }
                }
            }
        }

        public void updatezloss(lossset loss, float[,] d, byte[,] target, int x00, int y00, int x10, int y10)
        {
            int lossvalue = loss.lossvalue;
            for (int i = y00, i0 = 0; i <= y10; ++i, ++i0)
                for (int j = x00, j0 = 0; j <= x10; ++j, ++j0)
                {

                    if (d[i, j] >= threshold)
                    {
                        if (target[i, j] == 0)
                        {
                            if (loss.zloss[i, j] != 1)
                            {
                                ++lossvalue;
                                loss.zloss[i, j] = 1;
                            }
                        }
                        else
                        {
                            if (loss.zloss[i, j] != 0)
                            {
                                lossvalue -= lossweight;
                                loss.zloss[i, j] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (target[i, j] == 1)
                        {
                            if (loss.zloss[i, j] != 100)
                            {
                                lossvalue += lossweight;
                                loss.zloss[i, j] = 100;
                            }
                        }
                        else
                        {
                            if (loss.zloss[i, j] != 0)
                            {
                                --lossvalue;
                                loss.zloss[i, j] = 0;
                            }
                        }
                    }
                }

            loss.lossvalue = lossvalue;
        }

        public void updateld(List<beam>[,] tld, List<beam>[,] bestld, int x0, int y0, int x1, int y1)
        {
            beam b;
            for (int i = x0, i0 = 0; i <= x1; ++i, ++i0)
            {
                for (int j = y0, j0 = 0; j <= y1; ++j, ++j0)
                {
                    if (bestld[j0, i0] != null)
                    {
                        //if (tld[j, i].Count != bestld[j0, i0].Count)
                        //    ;

                        tld[j, i] = new List<beam>();
                        for (int k = 0; k < bestld[j0, i0].Count; ++k)
                        {
                            //if (tld[j, i][k].spotnum != bestld[j0, i0][k].spotnum)
                            //    ;
                            //if (tld[j, i][k].spotorder != bestld[j0, i0][k].spotorder)
                            //    ;

                            b = new beam();
                            b.eon = bestld[j0, i0][k].eon;
                            b.spotnum = bestld[j0, i0][k].spotnum;
                            b.spotorder = bestld[j0, i0][k].spotorder;
                            tld[j, i].Add(b);

                            //tld[j, i][k].on = bestld[j0, i0][k].on;
                        }
                    }
                }
            }
        }


        public void updatedosage(float[,] d, float[,] bestd, int x00, int y00, int x10, int y10)
        {
            for (int i = y00, i0 = 0; i <= y10; ++i, ++i0)
                for (int j = x00, j0 = 0; j <= x10; ++j, ++j0)
                {
                    d[i, j] = bestd[i0, j0];
                }
        }

        public void computenewzlossset(zlossset zlossset0, float[,] dorg, byte[,] target0, int row, int col)
        {
            int loss = zlossset0.loss;
            for (int i = row - halfrow; i <= row + halfrow; ++i)
            {
                for (int j = col - halfcol; j <= col + halfcol; ++j)
                {
                    //loss -= zlossset0.zloss[i, j];

                    if (dorg[i, j] >= threshold)
                    {
                        if (target0[i, j] == 0)
                        {
                            if (zlossset0.zloss[i, j] != 1)
                            {
                                ++loss;
                                zlossset0.zloss[i, j] = 1;
                            }
                        }
                        else
                        {
                            if (zlossset0.zloss[i, j] != 0)
                            {
                                loss -= lossweight;
                                zlossset0.zloss[i, j] = 0;
                            }
                        }
                    }
                    else
                    {
                        if (target0[i, j] == 1)
                        {
                            if (zlossset0.zloss[i, j] != lossweight)
                            {
                                loss += lossweight;
                                zlossset0.zloss[i, j] = 100;
                            }
                        }
                        else
                        {
                            if (zlossset0.zloss[i, j] != 0)
                            {
                                --loss;
                                zlossset0.zloss[i, j] = 0;
                            }
                        }
                    }
                }
            }
            zlossset0.loss = loss;
        }


        public int computecurloss(byte[,] target0, float[,] curd, int row, int col)
        {
            int loss = 0;
            for (int i = row - halfrow; i <= row + halfrow; ++i)
            {
                for (int j = col - halfcol; j <= col + halfcol; ++j)
                {
                    if (curd[i, j] >= threshold)
                    {
                        if (target0[i, j] == 0)
                        {
                            ++loss;
                            //zloss[i, j] = 1;
                        }
                    }
                    else
                    {
                        if (target0[i, j] == 1)
                        {
                            loss += lossweight;
                            //zloss[i, j] = 1;
                        }
                    }
                }
            }

            return loss;
        }

        public void computecurzloss(byte[,] target0, float[,] curd, byte[,] zloss, int row, int col)
        {
            //int loss = 0;
            for (int i = row - halfrow; i <= row + halfrow; ++i)
            {
                for (int j = col - halfcol; j <= col + halfcol; ++j)
                {
                    if (curd[i, j] >= threshold)
                    {
                        if (target0[i, j] == 0)
                        {
                            //++loss;
                            zloss[i, j] = 1;
                        }
                        else
                        {
                            zloss[i, j] = 0;
                        }
                    }
                    else
                    {
                        if (target0[i, j] == 1)
                        {
                            //++loss;
                            zloss[i, j] = 1;
                        }
                        else
                        {
                            zloss[i, j] = 0;
                        }
                    }
                }
            }

            //return loss;
        }

        public int computeorgcurloss(byte[,] zloss, int row, int col)
        {
            int loss = 0;
            for (int i = row - halfrow; i <= row + halfrow; ++i)
            {
                for (int j = col - halfcol; j <= col + halfcol; ++j)
                {
                    if (zloss[i, j] == 1)
                    {
                        ++loss;
                    }
                    else if (zloss[i, j] == 100)
                    {
                        loss += lossweight;
                    }
                }
            }

            return loss;
        }

        public void subtractbeam(float[,] curd, int row, int col, float[,,] beam, int spotnum)
        {
            for (int i = row - halfrow, i0 = 0; i <= row + halfrow; ++i, ++i0)
            {
                for (int j = col - halfcol, j0 = 0; j <= col + halfcol; ++j, ++j0)
                {
                    curd[i, j] -= beam[i0, j0, spotnum];//減掉光點能量
                }
            }
        }

        public void addbeam(float[,] curd, int row, int col, float[,,] beam, int spotnum)
        {
            for (int i = row - halfrow, i0 = 0; i <= row + halfrow; ++i, ++i0)
            {
                for (int j = col - halfcol, j0 = 0; j <= col + halfcol; ++j, ++j0)
                {
                    curd[i, j] += beam[i0, j0, spotnum];//增加光點能量
                }
            }
        }

        public int[] GenerateRandom(int count, int min, int max)//random numbers excluded max 
        {
            Random random = new Random();
            //  initialize set S to empty
            //  for J := N-M + 1 to N do
            //    T := RandInt(1, J)
            //    if T is not in S then
            //      insert T in S
            //    else
            //      insert J in S
            //
            // adapted for C# which does not have an inclusive Next(..)
            // and to make it from configurable range not just 1.

            if (max <= min || count < 0 ||
                    // max - min > 0 required to avoid overflow
                    (count > max - min && max - min > 0))
            {
                // need to use 64-bit to support big ranges (negative min, positive max)
                throw new ArgumentOutOfRangeException("Range " + min + " to " + max +
                        " (" + ((Int64)max - (Int64)min) + " values), or count " + count + " is illegal");
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - count; top < max; top++)
            {
                // May strike a duplicate.
                // Need to add +1 to make inclusive generator
                // +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                {
                    // collision, add inclusive max.
                    // which could not possibly have been added before.
                    candidates.Add(top);
                }
            }

            int[] result = new int[candidates.Count];
            candidates.CopyTo(result);
            // load them in to a list, to sort
            //List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            //for (int i = result.Count - 1; i > 0; i--)
            //{
            //    int k = random.Next(i + 1);
            //    int tmp = result[k];
            //    result[k] = result[i];
            //    result[i] = tmp;
            //}
            return result;
        }

        public byte[,] parttarget(byte[,] target0, int x00, int y00, int x10, int y10)
        {
            byte[,] part = new byte[y10 - y00 + 1, x10 - x00 + 1];
            for (int i = y00; i <= y10; ++i)
                for (int j = x00; j <= x10; ++j)
                {
                    part[i - y00, j - x00] = target0[i, j];
                }

            return part;
        }

        public float[,] partdosage(float[,] d, int x00, int y00, int x10, int y10)
        {
            float[,] part = new float[y10 - y00 + 1, x10 - x00 + 1];
            for (int i = y00; i <= y10; ++i)
                for (int j = x00; j <= x10; ++j)
                {
                    part[i - y00, j - x00] = d[i, j];
                }

            return part;
        }

        //public List<beam>[,] partbeamarray(int x00, int y00, int x10, int y10)
        //{
        //    List<beam>[,] part = new List<beam>[y10 - y00 + 1, x10 - x00 + 1];
        //    for (int i = y00; i <= y10; ++i)
        //    {
        //        for (int j = x00; j <= x10; ++j)
        //        {
        //            part[i - y00, j - x00] = beamarray[i, j];
        //        }
        //    }

        //    return part;
        //}

        public zlossset partzloss(float[,] dorg, byte[,] target0)
        {
            byte[,] zloss0 = new byte[dorg.GetLength(0), dorg.GetLength(1)];
            int loss = 0;
            for (int i = 0; i < dorg.GetLength(0); ++i)
            {
                for (int j = 0; j < dorg.GetLength(1); ++j)
                {
                    if (dorg[i, j] >= threshold)
                    {
                        if (target0[i, j] == 0)
                        {
                            ++loss;
                            zloss0[i, j] = 1;
                        }
                    }
                    else
                    {
                        if (target0[i, j] == 1)
                        {
                            loss += lossweight;
                            zloss0[i, j] = 100;
                        }
                    }
                }
            }
            zlossset zlossset0 = new zlossset();
            zlossset0.zloss = zloss0;
            zlossset0.loss = loss;

            return zlossset0;
        }

        public List<beam>[,] partld(List<beam>[,] ld, int x00, int y00, int x10, int y10)
        {
            List<beam>[,] part = new List<beam>[y10 - y00 + 1, x10 - x00 + 1];

            beam b;
            for (int i = y00; i <= y10; ++i)
                for (int j = x00; j <= x10; ++j)
                {
                    //part[i - y00, j - x00] = ld[i, j];
                    if (ld[i, j] != null)
                    {
                        part[i - y00, j - x00] = new List<beam>();
                        for (int k = 0; k < ld[i, j].Count; ++k)
                        {
                            b = new beam();
                            b.eon = ld[i, j][k].eon;
                            b.spotnum = ld[i, j][k].spotnum;
                            b.spotorder = ld[i, j][k].spotorder;

                            part[i - y00, j - x00].Add(b);
                        }
                    }
                }

            return part;
        }

        public int computepartloss(byte[,] zloss, int x00, int y00, int x10, int y10)
        {
            int lossorg = 0;
            for (int i = y00; i <= y10; ++i)
                for (int j = x00; j <= x10; ++j)
                {
                    if (zloss[i, j] == 1)
                        ++lossorg;
                }
            return lossorg;
        }

        public float[,] dosageroi0125(int rowoffset, int coloffset, int roirownum, int roicolnum)
        {
            float cnt_scale = displacement / tiffimg.imgps;
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";

            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            //string beamfile1path = beamfilepath+"spotarrays100le1.h5", beamfile2path = beamfilepath+"spotarrays100le2.h5", ldfilepath1 = tiffimg.imgfilename + "roi1.LD", ldfilepath2 = tiffimg.imgfilename + "roi2.LD";
            //beam1.openfile(beamfile1path);
            //int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            //beam1.closefile();
            int offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / 0.125, scalecol = tiffimg.imgps / 0.125;
            int scaleoffsetrow = (int)(scalerow / 2), scaleoffsetcol = (int)(scalecol / 2);
            int windowrow = (int)((orgwindowrow * scalerow) + 0.5), windowcol = (int)((orgwindowcol * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = "dosageimgroi.h5", buffername = "buffertemp";
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
            //width = 2000;

            //file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;

                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(ldxstart, ldxend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(ldxstart, ldxend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                //yinterval = new Tuple<int, int>(Math.Max(yinterval1.Item1, yinterval2.Item1), Math.Min(yinterval1.Item2, yinterval2.Item2));
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

                    int tempx = (int)Math.Ceiling(halfcol * 0.125 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 0.125 / tiffimg.imgps);
                    //int ldxstart = xstart - tempx;
                    //if (ldxstart < 0)
                    //    ldxstart = 0;

                    int ldystart = ystart - tempy;
                    if (ldystart < -t0)
                        ldystart = -t0;

                    //int ldxend = xend + tempx;
                    //if (ldxend >= width)
                    //    ldxend = width - 1;

                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));//省略減掉scale後的offset
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
                    //beamarray = new List<beam>[ldrowscale, ldcolscale];
                    //for (int i = 0; i < beamarray.GetLength(0); ++i)
                    //{
                    //    for (int j = 0; j < beamarray.GetLength(1); ++j)
                    //    {
                    //        beamarray[i, j] = new List<beam>();
                    //    }
                    //}

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
                                    //if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            int ldnowy, ldnowx;
                                            //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                            //beam b = new beam();
                                                            //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                            //b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                            //b.eon = 1;

                                                            //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                    //else
                                                    //{
                                                    //    ldnowy = t - ldconfig1.ldy[ldoffset + ldnum * 1000 + s];
                                                    //    ldnowx = ldconfig1.ldx[ldoffset + ldnum * 1000 + s];
                                                    //    if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                    //    {
                                                    //        beam b = new beam();
                                                    //        //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                    //        b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                    //        b.eon = 0;

                                                    //        //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                    //    }
                                                    //}
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots2[ldnum * 8 + facet - 1], s);
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
                    //file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            //file.closefile();
            return dosage;

        }

        public float[,] dosageroi0125(int rowoffset, int coloffset, int roirownum, int roicolnum, string beamfile1path, string beamfile2path)
        {
            float cnt_scale = displacement / tiffimg.imgps;
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";

            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            //string beamfile1path = beamfilepath+"spotarrays100le1.h5", beamfile2path = beamfilepath+"spotarrays100le2.h5", ldfilepath1 = tiffimg.imgfilename + "roi1.LD", ldfilepath2 = tiffimg.imgfilename + "roi2.LD";
            //beam1.openfile(beamfile1path);
            //int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            //beam1.closefile();
            int offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / 0.125, scalecol = tiffimg.imgps / 0.125;
            int scaleoffsetrow = (int)(scalerow / 2), scaleoffsetcol = (int)(scalecol / 2);
            int windowrow = (int)((orgwindowrow * scalerow) + 0.5), windowcol = (int)((orgwindowcol * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = "dosageimgroi.h5", buffername = "buffertemp";
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
            //width = 2000;

            //file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;

                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(ldxstart, ldxend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(ldxstart, ldxend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                //yinterval = new Tuple<int, int>(Math.Max(yinterval1.Item1, yinterval2.Item1), Math.Min(yinterval1.Item2, yinterval2.Item2));
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

                    int tempx = (int)Math.Ceiling(halfcol * 0.125 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 0.125 / tiffimg.imgps);
                    //int ldxstart = xstart - tempx;
                    //if (ldxstart < 0)
                    //    ldxstart = 0;

                    int ldystart = ystart - tempy;
                    if (ldystart < -t0)
                        ldystart = -t0;

                    //int ldxend = xend + tempx;
                    //if (ldxend >= width)
                    //    ldxend = width - 1;

                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));//省略減掉scale後的offset
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
                    //beamarray = new List<beam>[ldrowscale, ldcolscale];
                    //for (int i = 0; i < beamarray.GetLength(0); ++i)
                    //{
                    //    for (int j = 0; j < beamarray.GetLength(1); ++j)
                    //    {
                    //        beamarray[i, j] = new List<beam>();
                    //    }
                    //}

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
                                    //if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            int ldnowy, ldnowx;
                                            //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                            //beam b = new beam();
                                                            //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                            //b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                            //b.eon = 1;

                                                            //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                    //else
                                                    //{
                                                    //    ldnowy = t - ldconfig1.ldy[ldoffset + ldnum * 1000 + s];
                                                    //    ldnowx = ldconfig1.ldx[ldoffset + ldnum * 1000 + s];
                                                    //    if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                    //    {
                                                    //        beam b = new beam();
                                                    //        //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                    //        b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                    //        b.eon = 0;

                                                    //        //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                    //    }
                                                    //}
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots2[ldnum * 8 + facet - 1], s);
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
                    //file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            //file.closefile();
            return dosage;

        }
        public float[,] dosageroi0125_opt(int rowoffset, int coloffset, int roirownum, int roicolnum)
        {
            float cnt_scale = displacement / tiffimg.imgps;
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";

            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            //string beamfile1path = beamfilepath+"spotarrays100le1.h5", beamfile2path = beamfilepath+"spotarrays100le2.h5", ldfilepath1 = tiffimg.imgfilename + "roi1.LD", ldfilepath2 = tiffimg.imgfilename + "roi2.LD";
            //beam1.openfile(beamfile1path);
            //int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            //beam1.closefile();
            int offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / 0.125, scalecol = tiffimg.imgps / 0.125;
            int scaleoffsetrow = (int)(scalerow / 2), scaleoffsetcol = (int)(scalecol / 2);
            int windowrow = (int)((orgwindowrow * scalerow) + 0.5), windowcol = (int)((orgwindowcol * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = "dosageimgroi.h5", buffername = "buffertemp";
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
            //width = 2000;

            //file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                int ldxstart = xstart - tempx;
                if (ldxstart < 0)
                    ldxstart = 0;

                int ldxend = xend + tempx;
                if (ldxend >= width)
                    ldxend = width - 1;

                tablenum = ldconfig1.checkrange(ldxstart, ldxend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(ldxstart, ldxend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                //yinterval = new Tuple<int, int>(Math.Max(yinterval1.Item1, yinterval2.Item1), Math.Min(yinterval1.Item2, yinterval2.Item2));
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

                    int tempx = (int)Math.Ceiling(halfcol * 0.125 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 0.125 / tiffimg.imgps);
                    //int ldxstart = xstart - tempx;
                    //if (ldxstart < 0)
                    //    ldxstart = 0;

                    int ldystart = ystart - tempy;
                    if (ldystart < -t0)
                        ldystart = -t0;

                    //int ldxend = xend + tempx;
                    //if (ldxend >= width)
                    //    ldxend = width - 1;

                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));//省略減掉scale後的offset
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
                    //beamarray = new List<beam>[ldrowscale, ldcolscale];
                    //for (int i = 0; i < beamarray.GetLength(0); ++i)
                    //{
                    //    for (int j = 0; j < beamarray.GetLength(1); ++j)
                    //    {
                    //        beamarray[i, j] = new List<beam>();
                    //    }
                    //}

                    int toffset = t0 + (int)(ldystart / cnt_scale), ldoffset;
                    int space = (int)Math.Ceiling((ldyend - ldystart + ydistance) / cnt_scale + 0.5);
                    byte[] buffer = new byte[4000];
                    BitArray bits;
                    byte facet = (byte)(toffset % 8);
                    if (facet == 0)
                        facet = 8;

                    if (ldxstart <= ldconfig1.xmax)
                    {
                        using (FileStream ldfile = new FileStream(ldfilenewpath1, FileMode.Open, FileAccess.Read))
                        {
                            ldfile.Seek(4000 * (toffset), SeekOrigin.Begin);

                            //while (ldfile.Read(buffer, 0, 4000) > 0)
                            for (int t = toffset; t <= space + toffset; ++t)
                            {
                                if (ldfile.Read(buffer, 0, 4000) > 0)
                                {
                                    //if (!buffer.All(x => x == 0))//test if all values are empty
                                    {
                                        bits = new BitArray(buffer);
                                        ldoffset = 20000 * (facet - 1);
                                        for (int s = 0; s < 1000; ++s)
                                        //Parallel.For(0, 1000, s =>
                                        {
                                            int startbit = s * 32;
                                            int startbyte = s * 4;
                                            int ldnowy, ldnowx;
                                            //if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0)
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots1[ldnum * 8 + facet - 1], s);
                                                            //matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + 0.5) + halfrow,
                                                            //    (int)((ldnowx - ldxstart) * scalecol + 0.5) + halfcol, spots01);
                                                            //beam b = new beam();
                                                            //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                            //b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                            //b.eon = 1;

                                                            //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                        }
                                                        //result[-1 * ldnowy, ldnowx] += 1;
                                                    }
                                                    //else
                                                    //{
                                                    //    ldnowy = t - ldconfig1.ldy[ldoffset + ldnum * 1000 + s];
                                                    //    ldnowx = ldconfig1.ldx[ldoffset + ldnum * 1000 + s];
                                                    //    if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                    //    {
                                                    //        beam b = new beam();
                                                    //        //b.spotorder = (ushort)((ldnum * 8 + facet - 1));
                                                    //        b.spotnum = (ushort)(ldoffset + ldnum * 1000 + s);
                                                    //        b.eon = 0;

                                                    //        //beamarray[(int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow, (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol].Add(b);
                                                    //    }
                                                    //}
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
                        using (FileStream ldfile = new FileStream(ldfilenewpath2, FileMode.Open, FileAccess.Read))
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
                                                            matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow) + scaleoffsetrow + halfrow,
                                                                  (int)((ldnowx - ldxstart) * scalecol) + scaleoffsetcol + halfcol, spots2[ldnum * 8 + facet - 1], s);
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
                    //file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            //file.closefile();
            return dosage;

        }
        public float[,] dosageroi0125_4level(int rowoffset, int coloffset, int roirownum, int roicolnum)
        {
            float cnt_scale = displacement / tiffimg.imgps;
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";

            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            //string beamfile1path = beamfilepath+"spotarrays100le1.h5", beamfile2path = beamfilepath+"spotarrays100le2.h5", ldfilepath1 = tiffimg.imgfilename + "roi1.LD", ldfilepath2 = tiffimg.imgfilename + "roi2.LD";
            //beam1.openfile(beamfile1path);
            //int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            //beam1.closefile();
            int offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / 0.125, scalecol = tiffimg.imgps / 0.125;
            int scaleoffsetrow = (int)(scalerow / 2), scaleoffsetcol = (int)(scalecol / 2);
            int windowrow = (int)((orgwindowrow * scalerow) + 0.5), windowcol = (int)((orgwindowcol * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = "dosageimgroi.h5", buffername = "buffertemp";
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
            //width = 2000;

            //file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                tablenum = ldconfig1.checkrange(xstart, xend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(xstart, xend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                //yinterval = new Tuple<int, int>(Math.Max(yinterval1.Item1, yinterval2.Item1), Math.Min(yinterval1.Item2, yinterval2.Item2));
                ydistance = yinterval.Item1 - yinterval.Item2;

                t0 = (int)Math.Round(Math.Min(yinterval1.Item2, yinterval2.Item2) / cnt_scale, MidpointRounding.AwayFromZero);


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

                    int tempx = (int)Math.Ceiling(halfcol * 0.125 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 0.125 / tiffimg.imgps);
                    int ldxstart = xstart - tempx;
                    if (ldxstart < 0)
                        ldxstart = 0;

                    int ldystart = ystart - tempy;
                    if (ldystart < -t0)
                        ldystart = -t0;

                    int ldxend = xend + tempx;
                    if (ldxend >= width)
                        ldxend = width - 1;

                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));//省略減掉scale後的offset
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
                                            int startbit = s * 40;
                                            int startbyte = s * 5;
                                            int ldnowy, ldnowx;
                                            if (buffer[startbyte] != 0 || buffer[startbyte + 1] != 0 || buffer[startbyte + 2] != 0 || buffer[startbyte + 3] != 0 || buffer[startbyte + 4] != 0)
                                            {
                                                for (int ldnum = 0; ldnum < 40; ldnum += 2)
                                                {
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + startbit + 1] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
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
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + startbit + 1] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {

                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }

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
                    //file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            //file.closefile();
            return dosage;

        }
        public float[,] dosageroi0125_4level_opt(int rowoffset, int coloffset, int roirownum, int roicolnum)
        {
            float cnt_scale = displacement / tiffimg.imgps;
            int height = tiffimg.height, width = tiffimg.width;
            string orgimgname = "imgtemp", ldimgname = "ldimgtemp", ldfacetname = "facettemp", ldldname = "ldtemp", ldtname = "timetemp";

            ldconfig1.lightspotrange();
            ldconfig2.lightspotrange();

            hdf5 file = new hdf5(), beam1 = new hdf5(), beam2 = new hdf5();
            //string beamfile1path = beamfilepath+"spotarrays100le1.h5", beamfile2path = beamfilepath+"spotarrays100le2.h5", ldfilepath1 = tiffimg.imgfilename + "roi1.LD", ldfilepath2 = tiffimg.imgfilename + "roi2.LD";
            //beam1.openfile(beamfile1path);
            //int beamrow = beam1.rownum, beamcol = beam1.colnum, halfrow = beamrow / 2, halfcol = beamcol / 2, offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            //beam1.closefile();
            int offsetrow = beamrow / 2, offsetcol = beamcol / 2, orgwindowrow = roirownum, orgwindowcol = roicolnum;
            if (roirownum < orgwindowrow)
                orgwindowrow = roirownum;
            if (roicolnum < orgwindowcol)
                orgwindowcol = roicolnum;

            double scalerow = tiffimg.imgps / 0.125, scalecol = tiffimg.imgps / 0.125;
            int scaleoffsetrow = (int)(scalerow / 2), scaleoffsetcol = (int)(scalecol / 2);
            int windowrow = (int)((orgwindowrow * scalerow) + 0.5), windowcol = (int)((orgwindowcol * scalecol) + 0.5);
            int scaleh = (int)((roirownum * scalerow) + 0.5), scalew = (int)((roicolnum * scalecol) + 0.5);
            int offsetafterfirstrow = 0, offsetafterfirstcol = 0, firstiterrow = halfrow, firstitercol = halfcol, maxwindowrow = windowrow, maxwindowcol = windowcol;
            windowrow = Math.Max(windowrow, (int)1.5 * beamrow);

            string dosagename = "dosageimgroi.h5", buffername = "buffertemp";
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
            //width = 2000;

            //file.createfile(dosagename, scaleh, scalew, windowrow, windowcol);
            for (xstart = coloffset, xend0 = coloffset + orgwindowcol; ; xstart += orgwindowcol, xend0 += orgwindowcol)
            {
                if (xend0 >= coloffset + roicolnum)
                {
                    xend0 = coloffset + roicolnum;
                    lastcol = true;
                }

                xend = xend0 - 1;

                tablenum = ldconfig1.checkrange(xstart, xend);
                yinterval1 = ldconfig1.scanrange(tablenum);

                tablenum2 = ldconfig2.checkrange(xstart, xend);
                yinterval2 = ldconfig2.scanrange(tablenum2);

                yinterval = new Tuple<float, float>(Math.Max(ldconfig1.ymax, ldconfig2.ymax), Math.Min(ldconfig1.ymin, ldconfig2.ymin));
                //yinterval = new Tuple<int, int>(Math.Max(yinterval1.Item1, yinterval2.Item1), Math.Min(yinterval1.Item2, yinterval2.Item2));
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

                    int tempx = (int)Math.Ceiling(halfcol * 0.125 / tiffimg.imgps), tempy = (int)Math.Ceiling(halfrow * 0.125 / tiffimg.imgps);
                    int ldxstart = xstart - tempx;
                    if (ldxstart < 0)
                        ldxstart = 0;

                    int ldystart = ystart - tempy;
                    if (ldystart < -t0)
                        ldystart = -t0;

                    int ldxend = xend + tempx;
                    if (ldxend >= width)
                        ldxend = width - 1;

                    int ldyend = yend + tempy;
                    if (ldyend >= height)
                        ldyend = height - 1;

                    //int ldxoffset = xstart - ldxstart;
                    //int ldyoffset = ystart - ldystart;

                    int ldxoffsetscale = (int)((xstart * scalerow)) - (int)((ldxstart * scalerow));//省略減掉scale後的offset
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
                        using (FileStream ldfile = new FileStream(ldfilenewpath1, FileMode.Open, FileAccess.Read))
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
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + startbit + 1] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig1.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig1.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {
                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots1[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
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
                        using (FileStream ldfile = new FileStream(ldfilenewpath2, FileMode.Open, FileAccess.Read))
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
                                                    if (bits[ldnum + startbit] == true || bits[ldnum + startbit + 1] == true)
                                                    {
                                                        ldnowy = (int)Math.Round(t * cnt_scale - ldconfig2.ldy[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);
                                                        ldnowx = (int)Math.Round(ldconfig2.ldx[ldoffset + (ldnum / 2) * 1000 + s], MidpointRounding.AwayFromZero);

                                                        if (ldxstart <= ldnowx && ldnowx <= ldxend && ldystart <= ldnowy && ldyend >= ldnowy)
                                                        {

                                                            if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] != true && bits[ldnum + 1 + startbit] == true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }
                                                            else if (bits[ldnum + startbit] == true && bits[ldnum + 1 + startbit] != true)
                                                            {
                                                                matop.beamsuperposition(expnddosage, (int)((ldnowy - ldystart) * scalerow + scaleoffset) + halfrow,
                                                                       (int)((ldnowx - ldxstart) * scalecol + scaleoffset) + halfcol, spots2[(int)(ldnum / 2) * 8 + facet - 1], s);
                                                            }

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
                    //file.appenddata(ystart - rowoffset, xstart - coloffset, dosage);

                    if (lastrow == true)
                        break;
                }

                lastrow = false;

                if (lastcol == true)
                    break;
            }
            //file.closefile();
            return dosage;

        }

        public Dictionary<int, float[,,]> storespots(hdf5 beam, List<byte> tablenum)
        {
            Dictionary<int, float[,,]> spots = new Dictionary<int, float[,,]>();
            foreach (int i in tablenum)
            {
                for (int j = 0; j < 8; ++j)
                    spots[i * 8 + j] = beam.readspotdata1000(i * 8 + j);
            }
            return spots;
        }

        public Dictionary<int, float[,,]> storespots32(hdf5 beam1, hdf5 beam2, List<byte> tablenum1, List<byte> tablenum2)
        {
            Dictionary<int, float[,,]> spots = new Dictionary<int, float[,,]>();
            //Dictionary<int, uint[,,]> spots_uint = new Dictionary<int, uint[,,]>();

            foreach (int i in tablenum1)
            {
                for (int j = 0; j < 8; ++j)
                    spots[i * 8 + j] = beam1.readspotdata1000(i * 8 + j);
            }
            foreach (int i in tablenum2)
            {
                for (int j = 0; j < 8; ++j)
                    spots[i * 8 + j + 160] = beam2.readspotdata1000(i * 8 + j);
            }
            //foreach (var entry in spots)
            //{
            //    spots_uint[entry.Key] = new uint[entry.Value.GetLength(0), entry.Value.GetLength(1), 1000];

            //    for (int i=0;i< entry.Value.GetLength(0);++i)
            //    {
            //        for(int j = 0; j < entry.Value.GetLength(1); ++j)
            //        {
            //            for (int k = 0; k < entry.Value.GetLength(2); ++k)
            //            {
            //                spots_uint[entry.Key][i,j,k] = (uint)(spots[entry.Key][i,j,k] *1000000); 
            //            }
            //        }
            //    }
            //}

            return spots;
        }

        public optset optimize_maxcc_test(int xlefttop, int ylefttop, int xrightbottom, int yrightbottom, string beamfile1path, string beamfile2path, int beamlevel)
        {
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

            int[] optidx0 = new int[4];
            ldset tld = readld(xlefttop, ylefttop, xrightbottom, yrightbottom, "2");//resolution = 0.125um
            ldset tldfull = readld(xlefttop, ylefttop, xrightbottom, yrightbottom, "2", expandscale);//resolution = 0.125um
            dosageset d = readdosage(xlefttop, ylefttop, xrightbottom, yrightbottom, beamfile1path, beamfile2path);//resolution = 0.125um,use tld
            //float[,] d = new float[0, 0];
            targetset target = readtarget(xlefttop, ylefttop, xrightbottom, yrightbottom);//resolution = 0.125um byte[,] target = new byte[0, 0];
            string optoutputfolder = outputpath + "optimization\\";
            if (beamlevel == 100)
            {
                if (!Directory.Exists(optoutputfolder))
                    Directory.CreateDirectory(optoutputfolder);
                //verifyimg.writearray(tldfull.array, optoutputfolder + "orgld_2stage");
                verifyimg.writearray(d.array, optoutputfolder + "orgd_2stage");
                verifyimg.writearray(target.array, optoutputfolder + "targetroi_2stage");
            }
            int[,] zloss = new int[0, 0];//use d,threshold
            lossset loss = computeloss(d, target);//first loss value of zloss
            ccl ccl0 = new ccl();
            int yexpscaleright = (int)(roirownum * scale) + yexpscale, xexpscaleright = (int)(roicolnum * scale) + xexpscale;
            optidx[0] = yexpscale;
            optidx[1] = yexpscale + (int)(roirownum * scale) - 1;
            optidx[2] = xexpscale;
            optidx[3] = xexpscale + (int)(roicolnum * scale) - 1;
            int cc = ccl0.findmaxcc(loss.zloss, xexpscale, yexpscale, xexpscaleright, yexpscaleright);
            List<int> losshistory = new List<int>();
            List<int> cchistory = new List<int>();
            //List<int> cchistory_second = new List<int>();
            float ccthresholdratio = 2f / 3, ccthreshold;
            //int initloss = computeloss_int(loss.zloss);

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


                int rangex = 20, rangey = 10, rangescale = 0, omaxiter = iniomaxiter, totalcnt = 1;
                float proportion = (float)iniproportion, tabuproportion = 0;
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
                spots = storespots32(beam1, beam2, tablenum1, tablenum2);
                beam1.closefile();
                beam2.closefile();
                //string testfolder = outputpath + "test\\", dstr = testfolder + "dorg", ldstr1 = testfolder + "ldorg", ldstr2 = testfolder + "bestld", tstr = testfolder + "target0", zstr = testfolder + "zloss0", ldstr = testfolder + "ldnow";
                //bool first = true;

                //while (!stop && ccpos.Count != 0)
                //while (!stop && loss.lossposx.Count != 0)
                {
                    //foreach (int key in ccposidx)
                    {
                        //for (int pos = 0; pos < ccpos[key].Count(); ++pos)
                        //for (int pos = 0; pos < loss.lossposx.Count(); ++pos)
                        for (int pos = 0; pos < 0; ++pos)
                        {
                            k = loss.lossposy[pos];
                            f = loss.lossposx[pos];
                            //k = ccpos[key][pos].y;
                            //f = ccpos[key][pos].x;

                            if (loss.zloss[k, f] == 1)
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

                                target0 = parttarget(target.array, x00, y00, x10, y10);
                                //lossorg = computepartloss(loss.zloss, x00, y00, x10, y10);
                                dorg = partdosage(d.array, x00, y00, x10, y10);
                                zlossset0 = partzloss(dorg, target0);
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

                                ldnow = partld(tld.array, x0, y0, x1, y1);
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
                                copyld(ldnow, bestld);
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
                                int [] losslist;
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
                                    ranlist = GenerateRandom(maxiter, 0, code.Length);
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
                                            subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }
                                        else
                                        {
                                            addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }

                                        int orgcurloss = computeorgcurloss(zlossset0.zloss, row[ran], col[ran]);
                                        int curloss = computecurloss(target0, curd, row[ran], col[ran]);

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
                                            subtractbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }
                                        else
                                        {
                                            newcode = 1;
                                            addbeam(curd, row[ran], col[ran], spots[spotorder[ran]], spotnum[ran]);
                                        }

                                        code[ran] = newcode;

                                        Array.Copy(curd, dorg, dorg.Length);
                                        computenewzlossset(zlossset0, dorg, target0, row[ran], col[ran]);
                                        ldnow[row[ran] - halfrow, col[ran] - halfcol][order[ran]].eon = newcode;

                                        if (losschoose < bestloss)
                                        {
                                            Array.Copy(dorg, bestd, dorg.Length);
                                            Array.Copy(code, bestcode, code.Length);
                                            bestloss = losschoose;
                                            Array.Copy(zlossset0.zloss, bestzloss, zlossset0.zloss.Length);
                                            copyld(ldnow, bestld);
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

                                updateld(tld.array, bestld, x0, y0, x1, y1);
                                updatedosage(d.array, bestd, x00, y00, x10, y10);
                                updatezloss(loss, d.array, target.array, x00, y00, x10, y10);
                                losshistory.Add(loss.lossvalue);
                            }

                            int constantiter = iniconstantiter, epsilon = iniepsilon;
                            float lossvar;

                            if (losshistory.Count >= constantiter)
                            {
                                lossvar = losshistory[losshistory.Count - constantiter] - losshistory[losshistory.Count - 1];
                                if (lossvar < epsilon)
                                {
                                    stop = true;
                                    break;
                                }
                            }

                            //if (losshistory[losshistory.Count - 1] <= totalnum_criterion)
                            //{
                            //    stop = true;
                            //    break;
                            //}

                        }
                    }
                    cc = ccl0.findmaxcc(loss.zloss, xexpscale, yexpscale, xexpscaleright, yexpscaleright);
                    cchistory.Add(cc);
                    //if (cc <= maxcc_criterion && losshistory[losshistory.Count - 1] <= totalnum_criterion)
                    //    break;

                    //updatelosspos(loss);

                    ccpos = ccl0.findccpos(xexpscale, yexpscale, xexpscaleright, yexpscaleright);
                    ccposidx.Clear();
                    //ccthreshold = cchistory[cchistory.Count - 1] * ccthresholdratio;
                    foreach (int key in ccpos.Keys)
                    {
                        if (ccpos[key].Count >= maxcc_criterion)
                            ccposidx.Add(key);
                    }
                    updatelosspos(loss);

                }

            }
            optset optsetresult = new optset();
            optsetresult.target = target;
            optsetresult.tld = tld;
            optsetresult.d = d;
            Array.Copy(d.array, optsetresult.d.array, d.array.Length);
            optsetresult.loss = loss;
            optsetresult.losshistory = losshistory;
            optsetresult.lossdiff = losshistory[0] - losshistory[losshistory.Count - 1];
            optsetresult.cchistory = cchistory;
            optsetresult.ccdiff = cchistory[0] - cchistory[cchistory.Count - 1];

            return optsetresult;
            //float[,] dnew = new float[d.ynum, d.xnum];
            //for (int i = 0; i < dnew.GetLength(0); ++i)
            //{
            //    for (int j = 0; j < dnew.GetLength(1); ++j)
            //    {
            //        dnew[i, j] = d.array[i + d.yf1, j + d.xf1];
            //    }
            //}

            //string dstr1 = outputpath + "dosageimg_new";
            //verifyimg.writearray(dnew, dstr1);

            //outputlist(cchistory, optoutputfolder + "cchistory_2stage.txt");
            //outputlist(losshistory, optoutputfolder + "losshistory_2stage.txt");
            //writenewldfile(tld);
            //ldset tldfullnew = readld_opt(xlefttop, ylefttop, xrightbottom, yrightbottom, expandscale);//resolution = 0.125um

            //verifyimg.writearray(tldfullnew.array, optoutputfolder + "optld_2stage");
            //verifyimg.writearray(d.array, optoutputfolder + "optd_2stage");

        }

    }
    public class optset
    {
        public ldset tld;
        public dosageset d;
        public targetset target;
        public lossset loss;
        public List<int> losshistory;
        public float lossdiff;
        public List<int> cchistory;
        public int ccdiff;
    }
    public class ldset
    {
        public List<beam>[,] array;
        public int ldxstart, ldystart, ldxend, ldyend;
        public int leftx, rightx, lefty, righty;
        public int orgrownum, orgcolnum;
        public int xoffsetorg, yoffsetorg, orgxnum, orgynum;
    }
    public class beam
    {
        public byte eon;
        public ushort spotorder;
        public ushort spotnum;
    }
    public class dosageset
    {
        public float[,] array;
        public int xstart, ystart, xend, yend;
        public int xf1, yf1, xnum, ynum;
    }

    public class targetset
    {
        public byte[,] array;
        public int xstart, ystart, xend, yend;
    }

    public class lossset
    {
        public int lossvalue;
        public byte[,] zloss;
        //public byte[,] optergion;
        public float[,] dlloss;
        public List<int> lossposx;
        public List<int> lossposy;
        public int lossinsidetarget;
        public int lossroi;
    }

    public class zlossset
    {
        public byte[,] zloss;
        public int loss;
    }
    public class dlossset
    {
        public float[,] dloss;
        public float loss;
    }

    public class ranloss : IComparable<ranloss>
    {
        public int loss;
        public int idx;
        public int CompareTo(ranloss other)
        {
            //ranloss x1 = (ranloss)x;
            //ranloss y2 = (ranloss)y;

            if (this.loss > other.loss)
                return 1;

            if (this.loss < other.loss)
                return -1;
            else
                return 0;
        }
    }

    public class ranmaxcc : IComparable<ranmaxcc>
    {
        public int maxcc;
        public int optcc;
        public int idx;
        public int CompareTo(ranmaxcc other)
        {
            if (this.optcc > other.optcc)
                return 1;
            else if (this.optcc < other.optcc)
                return -1;
            else if (this.maxcc > other.maxcc)
                return 1;
            else
                return 0;
        }
    }

    public class rancomp : Comparer<ranloss>
    {
        public override int Compare(ranloss x, ranloss y)
        {
            ranloss x1 = (ranloss)x;
            ranloss y2 = (ranloss)y;

            return x.CompareTo(y);
        }
    }

    public class rancompcc : Comparer<ranmaxcc>
    {
        public override int Compare(ranmaxcc x, ranmaxcc y)
        {
            ranmaxcc x1 = (ranmaxcc)x;
            ranmaxcc y2 = (ranmaxcc)y;

            return x.CompareTo(y);
        }
    }



}
