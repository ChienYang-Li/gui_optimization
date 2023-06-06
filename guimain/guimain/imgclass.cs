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
    public class imgclass
    {
        public hdf5 h5img = new hdf5();
        public string path;
        public string name;
        public Point realposlefttop;
        public Point realposrightbottom;
        //public int bufferrow = 3000;
        //public int buffercol = 3000;
        public int ptbw;
        public int ptbh;
        public Image img;
        public float ratio;
        public float max = 0;
        public float min = float.MaxValue;
        public float[,] matrixvaluenew;
        public int currentx1;
        public int currenty1;
        public int currentx2;
        public int currenty2;
        public float[,] array;
        public float[,] roiarray;
        public byte[,] roibytearray;
        public int beamrow, beamcol, halfrow, halfcol;
        public float scale;
        public int tempx, tempy;
        //public float patratio;
        //public int epsilon;
        public imgclass()
        {

        }


        public imgclass(string path, string name, int ptbh, int ptbw)
        {
            this.path = path;
            this.name = name;
            this.ptbh = ptbh;
            this.ptbw = ptbw;
            updateimg(0, 0);
        }
        public imgclass(string path, string name, int ptbh, int ptbw, int useless, int useless2)
        {
            this.path = path;
            this.name = name;
            this.ptbh = ptbh;
            this.ptbw = ptbw;
            readh5img(path);
        }
        public imgclass(string path, string name, int ptbh, int ptbw, float ratio)
        {
            this.path = path;
            this.name = name;
            this.ptbh = ptbh;
            this.ptbw = ptbw;
            this.ratio = ratio;
            updateimg(0, 0);
        }

        public imgclass(string path, string name, int ptbh, int ptbw, Point realposlefttop, Point realposrightbottom)
        {
            this.path = path;
            this.name = name;
            this.ptbh = ptbh;
            this.ptbw = ptbw;
            readparth5img(realposlefttop, realposrightbottom);
        }
        public imgclass(string path, string name, int ptbh, int ptbw, Point realposlefttop, Point realposrightbottom, int useless, int pos0, int pos1)
        {
            this.path = path;
            this.name = name;
            this.ptbh = ptbh;
            this.ptbw = ptbw;
            readparth5img_new(realposlefttop, realposrightbottom, pos0, pos1);
        }
        public void openh5()
        {
            h5img.openfile(path);
        }
        public void readimgproperty(string path)
        {

            h5img.openfile(path);
            if (name == "Dosage_2level" || name == "Dosage_4level")
            {
                float max3 = 0;
                float min3 = float.MaxValue;
                float[,] array = h5img.readfloatdata();
                h5img.closefile();
                if (array == null)
                {

                }
                else
                {
                    for (int i = 0; i < array.GetLength(0); ++i)
                        for (int j = 0; j < array.GetLength(1); ++j)
                        {
                            if (array[i, j] > max3)
                            {
                                max3 = array[i, j];
                            }
                            if (array[i, j] < min3)
                            {
                                min3 = array[i, j];
                            }

                        }

                    if (max3 > max)
                        max = max3;
                    if (min3 < min)
                        min = min3;


                }

            }


        }
        public void updateimg(int pos0, int pos1)
        {
            h5img.openfile(path);
            //byte[,] currentarray;
            int[] currentarraymatrix = new int[4];
            int buffercol = 3000;
            int bufferrow = 3000;
            if (ratio < 1)
            {
                buffercol = (int)(ptbw / ratio);
                bufferrow = (int)(ptbh / ratio);
            }
            int currentarrayx1 = pos0 - buffercol;
            int currentarrayy1 = pos1 - bufferrow;
            int currentarrayx2 = pos0 + (int)(ptbw / ratio) + buffercol;
            int currentarrayy2 = pos1 + (int)(ptbh / ratio) + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;
            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= h5img.colnum)
            {
                currentarrayx2 = h5img.colnum - 1;
            }
            if (currentarrayy2 >= h5img.rownum)
            {
                currentarrayy2 = h5img.rownum - 1;
            }
            if (currentarrayy2 < currentarrayy1)
            {
                int temp;
                temp = currentarrayy1;
                currentarrayy1 = currentarrayy2 - ptbh;
                currentarrayy2 = h5img.rownum - 1;


            }
            if (currentarrayx2 < currentarrayx1)
            {
                int temp;
                temp = currentarrayx1;
                currentarrayx1 = currentarrayx2 - ptbw;
                currentarrayx2 = h5img.colnum - 1;

            }


            //currentarraymatrix[0] = currentarrayx1;
            //currentarraymatrix[1] = currentarrayy1;
            //currentarraymatrix[2] = currentarrayx2;
            //currentarraymatrix[3] = currentarrayy2;

            if (name == "Dosage_2level" || name == "Dosage_4level")
            {

                array = h5img.readfloatdata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);



                float h5min = (float)h5img.getmin();
                float h5max = (float)h5img.getmax();
                //for (int i = 0; i < array.GetLength(0); ++i)
                //    for (int j = 0; j < array.GetLength(1); ++j)
                //    {
                //        matrixvaluenew[i, j] = array[i, j];
                //        if (array[i, j] > maxtemp)
                //        {
                //            maxtemp = array[i, j];

                //        }
                //        else if (array[i, j] < mintemp)
                //        {
                //            mintemp = array[i, j];

                //        }
                //    }
                //if (maxtemp > max)
                //{
                //    max = maxtemp;
                //}
                //if (mintemp < min)
                //{
                //    min = mintemp;
                //}
                h5img.closefile();
                byte[,] normarray = normalization_new(array, h5min, h5max);
                img = BufferToImage15(normarray);

                //Array.Copy(matrixvalue, floatmatrix, array.GetLength(0) * array.GetLength(1));
                //imgclass img11 = new imgclass();
                //img11.callmatrixvalue(floatmatrix);
                realposlefttop.X = currentarrayx1;
                realposlefttop.Y = currentarrayy1;
                realposrightbottom.X = currentarrayx2;
                realposrightbottom.Y = currentarrayy2;
            }
            else if (  name == "Originalimg" || name == "LDFile_4level" || name == "Threshold_2level" || name == "Threshold_4level")
            {
                byte[,] array = h5img.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
                h5img.closefile();
                //byte[,] normarray = normalization(array);
                img = BufferTobinaryImage(array);

                realposlefttop.X = currentarrayx1;
                realposlefttop.Y = currentarrayy1;
                realposrightbottom.X = currentarrayx2;
                realposrightbottom.Y = currentarrayy2;
            }
            else if (name == "xor" || name == "xor_4level")
            {
                byte[,] array = h5img.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
                h5img.closefile();
                //byte[,] normarray = normalization(array);
                img = BufferToImage4(array);

                realposlefttop.X = currentarrayx1;
                realposlefttop.Y = currentarrayy1;
                realposrightbottom.X = currentarrayx2;
                realposrightbottom.Y = currentarrayy2;
            }
            else if(name == "LDFile_2level")
            {
                byte[,] array = h5img.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
                h5img.closefile();
                //byte[,] normarray = normalization(array);
                img = BufferToImage8(array);

                realposlefttop.X = currentarrayx1;
                realposlefttop.Y = currentarrayy1;
                realposrightbottom.X = currentarrayx2;
                realposrightbottom.Y = currentarrayy2;
            }
            else if (name == "")
            {
                img = null;
            }

            //currentarray = h5img.readbytedata(currentarrayy1, currentarrayx1, currentarrayy2 - currentarrayy1 + 1, currentarrayx2 - currentarrayx1 + 1);
            //OriginalPB.Invalidate();
            //h5img.closefile();
            //return new Tuple<int[], byte[,]>(currentarraymatrix, currentarray);
            //img = BufferTobinaryImage(currentarray);
        }
        public Image BufferToImage4(byte[,] myMatrix)
        {
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
            pal.Entries[2] = Color.RoyalBlue;
            pal.Entries[3] = Color.Red;

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
        public Image BufferToImage8(byte[,] myMatrix)
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

               
                //pal.Entries[0] = Color.Black;
                //pal.Entries[1] = Color.White;
                //pal.Entries[0] = Color.RoyalBlue;
                pal.Entries[0] = Color.Black;
                pal.Entries[1] = Color.Orange;
                pal.Entries[2] = Color.Yellow;
                pal.Entries[3] = Color.Green;
                pal.Entries[4] = Color.DodgerBlue;
                pal.Entries[5] = Color.Cyan;
                pal.Entries[6] = Color.Orchid;
                pal.Entries[7] = Color.OldLace;
                pal.Entries[8] = Color.Red;
                //for (int i = 1; i < 86; ++i)
                //{

                //    pal.Entries[i] = Color.FromArgb(0, 0, i + 170);
                //    //all the values exclude 0 are white


                //}




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
        public void readparth5img(Point realposlefttop, Point realposrightbottom)
        {
            h5img.openfile(path);
            this.realposlefttop = realposlefttop;
            this.realposrightbottom = realposrightbottom;

            if (name == "Dosage_2level" || name == "Dosage_4level")
            {
                array = h5img.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);

                float maxtemp = 0;
                float mintemp = float.MaxValue;
                float h5min;
                float h5max;
                //float[,] floatmatrix = new float[array.GetLength(0), array.GetLength(1)];
                //matrixvaluenew = new float[array.GetLength(0), array.GetLength(1)];

                //readimgproperty(path);

                //Array.Copy(matrixvalue, matrixvaluenew, array.GetLength(0)*array.GetLength(1));
                //imgclass img11 = new imgclass();
                //img11.callmatrixvalue(floatmatrix);
                if (array == null)
                {
                    GuiMain.patfail = true;
                    h5img.closefile();
                }
                else
                {
                    //float[,] matrixvalue = new float[array.GetLength(0), array.GetLength(1)];
                    //for (int i = 0; i < array.GetLength(0); ++i)
                    //    for (int j = 0; j < array.GetLength(1); ++j)
                    //    {
                    //        //matrixvaluenew[i, j] = array[i, j];
                    //        if (array[i, j] > maxtemp)
                    //        {
                    //            maxtemp = array[i, j];

                    //        }
                    //        else if (array[i, j] < min)
                    //        {
                    //            min = array[i, j];


                    //        }
                    //    }
                    //if (maxtemp > max)
                    //{
                    //    max = maxtemp;
                    //}
                    //if (mintemp < min)
                    //{
                    //    min = mintemp;
                    //}

                    h5min = (float)h5img.getmin();
                    h5max = (float)h5img.getmax();
                    h5img.closefile();
                    GuiMain.patfail = false;
                    byte[,] normarray = normalization_new(array, h5min, h5max);
                    img = BufferToImage15(normarray);
                }

            }
            else if (  name == "Originalimg" || name == "Threshold_2level" || name == "LDFile_4level" || name == "Threshold_4level")
            {
                byte[,] array = h5img.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
                h5img.closefile();
                if (array == null)
                {
                    GuiMain.patfail = true;
                }
                else
                {
                    GuiMain.patfail = false;
                    img = BufferTobinaryImage(array);
                }
                //byte[,] normarray = normalization(array);

            }
            else if (name == "xor" || name == "xor_4level")
            {
                byte[,] array = h5img.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
                h5img.closefile();
                if (array == null)
                {
                    GuiMain.patfail = true;
                }
                else
                {
                    GuiMain.patfail = false;
                    img = BufferToImage4(array);
                }
                //byte[,] normarray = normalization(array);
            }
            else if(name == "LDFile_2level")
            {
                byte[,] array = h5img.readbytedata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
                h5img.closefile();
                if (array == null)
                {
                    GuiMain.patfail = true;
                }
                else
                {
                    GuiMain.patfail = false;
                    img = BufferToImage8(array);
                }
            }




        }
        public void readh5img(string path)
        {




            h5img.openfile(path);
            if (name == "Dosage_2level_ROI" || name == "Dosage_4level_ROI" || name == "Dosage" || name == "Dosage_opt")
            {
                roiarray = h5img.readfloatdata();
                h5img.closefile();
                if (roiarray == null)
                {

                }
                else
                {
                    byte[,] normarray = normalization(roiarray);
                    img = BufferToImage15(normarray);
                }

            }
            else if (name == "LDFile_2level_ROI" || name == "LDFile_4level_ROI" || name == "Threshold_2level_ROI" || name == "Threshold_4level_ROI" || name == "Threshold_Org" || name == "Threshold_Opt" || name == "LDFile" || name == "LDFile_opt" || name == "Original")
            {
                roibytearray = h5img.readbytedata();
                h5img.closefile();
                if (roibytearray == null)
                {

                }
                else
                {

                    //byte[,] normarray = normalization(array);
                    img = BufferTobinaryImage(roibytearray);
                }

            }
            else if (name == "roi.h5" || name == "roi_4level.h5")
            {
                roibytearray = h5img.readbytedata();
                h5img.closefile();
                if (roibytearray == null)
                {

                }
                else
                {
                    img = BufferTobinaryImage(roibytearray);

                }


                //byte[,] normarray = normalization(array);

            }
            else if (name == "XOR_ROI_2level" || name == "XOR_ROI_4level" || name == "XOR_Org" || name == "XOR_Opt" || name == "Threshold_XOR" || name == "XOR_XOR" || name == "LD_XOR")
            {
                roibytearray = h5img.readbytedata();
                h5img.closefile();
                if (roibytearray == null)
                {

                }
                else
                {

                    //byte[,] normarray = normalization(array);
                    img = BufferToImage4(roibytearray);
                }
            }
            else if (name == "Dosage_XOR")
            {


                roiarray = h5img.readfloatdata();
                h5img.closefile();
                if (roiarray == null)
                {

                }
                else
                {
                    byte[,] normarray = normalization_dxor(roiarray);
                    img = BufferToImage15(normarray);
                }

            }

        }
        public void readparth5img_new(Point realposlefttop, Point realposrightbottom, int pos0, int pos1)
        {
            h5img.openfile(path);
            this.realposlefttop = realposlefttop;
            this.realposrightbottom = realposrightbottom;
            int buffercol = 3000;
            int bufferrow = 3000;
            //if (ratio < 1)
            //{
            //    buffercol = (int)(ptbw / ratio);
            //    bufferrow = (int)(ptbh / ratio);
            //}
            int currentarrayx1 = pos0 - buffercol;
            int currentarrayy1 = pos1 - bufferrow;
            int currentarrayx2 = pos0 + (int)(ptbw / 1) + buffercol;
            int currentarrayy2 = pos1 + (int)(ptbh / 1) + bufferrow;
            if (currentarrayx1 < 0)
            {
                currentarrayx1 = 0;
            }
            if (currentarrayy1 < 0)
            {
                currentarrayy1 = 0;
            }
            if (currentarrayx2 >= h5img.colnum)
            {
                currentarrayx2 = h5img.colnum - 1;
            }
            if (currentarrayy2 >= h5img.rownum)
            {
                currentarrayy2 = h5img.rownum - 1;
            }
            if (currentarrayy2 < currentarrayy1)
            {
                int temp;
                temp = currentarrayy1;
                currentarrayy1 = currentarrayy2 - ptbh;
                currentarrayy2 = h5img.rownum - 1;


            }
            if (currentarrayx2 < currentarrayx1)
            {
                int temp;
                temp = currentarrayx1;
                currentarrayx1 = currentarrayx2 - ptbw;
                currentarrayx2 = h5img.colnum - 1;

            }
            currentx1 = currentarrayx1;
            currenty1 = currentarrayy1;
            currentx2 = currentarrayx2;
            currenty2 = currentarrayy2;
            if (name == "Dosage_2level" || name == "Dosage_4level")
            {
                float[,] array = h5img.readfloatdata(realposlefttop.Y, realposlefttop.X, realposrightbottom.Y - realposlefttop.Y + 1, realposrightbottom.X - realposlefttop.X + 1);
                float[,] matrixvalue = new float[array.GetLength(0), array.GetLength(1)];
                //float[,] floatmatrix = new float[array.GetLength(0), array.GetLength(1)];
                matrixvaluenew = new float[array.GetLength(0), array.GetLength(1)];
                h5img.closefile();
                for (int i = 0; i < array.GetLength(0); ++i)
                    for (int j = 0; j < array.GetLength(1); ++j)
                    {
                        matrixvaluenew[i, j] = array[i, j];
                        if (array[i, j] > max)
                        {
                            max = array[i, j];
                        }
                    }
                //Array.Copy(matrixvalue, matrixvaluenew, array.GetLength(0)*array.GetLength(1));
                //imgclass img11 = new imgclass();
                //img11.callmatrixvalue(floatmatrix);
                if (array == null)
                {
                    GuiMain.patfail = true;
                }
                else
                {
                    GuiMain.patfail = false;
                    byte[,] normarray = normalization(array);
                    img = BufferToImage15(normarray);
                }

            }





        }
        public byte[,] normalization(float[,] currentarrayfloat)
        {
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

            return currentarray;
        }
        public byte[,] normalization_new(float[,] currentarrayfloat, float min1, float max1)
        {
            int m = currentarrayfloat.GetLength(0);
            int n = currentarrayfloat.GetLength(1);
            byte[,] currentarray = new byte[m, n];
            float min2 = float.MaxValue, max2 = 0;
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                {
                    if (max2 < currentarrayfloat[i, j])
                    { max2 = currentarrayfloat[i, j]; }
                    if (min2 > currentarrayfloat[i, j])
                    { min2 = currentarrayfloat[i, j]; }
                }
            if (max2 > max1)
            {
                max1 = max2;
            }
            if (min2 < min1)
            {
                min1 = min2;
            }
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    currentarrayfloat[i, j] = (currentarrayfloat[i, j] - min1) / (max1 - min1);
                    currentarray[i, j] = (byte)(currentarrayfloat[i, j] * 255);
                }
            }

            return currentarray;
        }
        public byte[,] normalization_dxor(float[,] currentarrayfloat)
        {
            int m = currentarrayfloat.GetLength(0);
            int n = currentarrayfloat.GetLength(1);
            byte[,] currentarray = new byte[m, n];
            float min2 = float.MaxValue, max2 = 0;
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                {
                    if (max2 < currentarrayfloat[i, j])
                    { max2 = currentarrayfloat[i, j]; }
                    if (min2 > currentarrayfloat[i, j])
                    { min2 = currentarrayfloat[i, j]; }
                }
            //if (max2 > max1)
            //{
            //    max1 = max2;
            //}
            //if (min2 < min1)
            //{
            //    min1 = min2;
            //}
            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (currentarrayfloat[i, j] != 0)
                    {
                        currentarrayfloat[i, j] = (currentarrayfloat[i, j] - min2) / (max2 - min2);
                        currentarray[i, j] = (byte)(currentarrayfloat[i, j] * 255);

                    }




                }
            }

            return currentarray;
        }

        public bool checkifupdate(int pos0, int pos1, int epsilon)
        {
            int maxheightsize = h5img.rownum - 1;
            int maxwidthsize = h5img.colnum - 1;

            if (realposlefttop.X == 0 && realposlefttop.Y == 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon)
            {
                return false;
            }
            else if (realposlefttop.X == 0 && realposlefttop.Y != 0 && realposrightbottom.Y - pos1 > epsilon && pos1 - realposlefttop.Y > epsilon && realposrightbottom.X - pos0 > epsilon)
            {
                return false;
            }
            else if (realposlefttop.Y == 0 && realposlefttop.X != 0 && realposrightbottom.X - pos0 > epsilon && realposrightbottom.Y - pos1 > epsilon && pos0 - realposlefttop.X > epsilon)
            {
                return false;
            }
            else if (realposrightbottom.Y == maxheightsize && realposrightbottom.X != maxwidthsize && pos0 - realposlefttop.X > epsilon && realposrightbottom.X - pos0 > epsilon && pos1 - realposlefttop.Y > epsilon)
            {
                return false;
            }
            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y != maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon && realposrightbottom.Y - pos1 > epsilon)
            {
                return false;
            }
            else if (realposrightbottom.X == maxwidthsize && realposrightbottom.Y == maxheightsize && pos1 - realposlefttop.Y > epsilon && pos0 - realposlefttop.X > epsilon)
            {
                return false;
            }
            else if (pos0 - realposlefttop.X < epsilon || pos1 - realposlefttop.Y < epsilon || realposrightbottom.X - pos0 < epsilon || realposrightbottom.Y - pos1 < epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int[] roi_updateimg(int Patwidth, int Patheight, int Oriwidth, int Oriheight, float rationew)
        {
            //var t = PatternPB.Size;
            //var t1 = OriginalPB.Size;
            int[] size = new int[4];
            Oriwidth = (int)(Oriwidth * rationew);
            Oriheight = (int)(Oriheight * rationew);
            Patwidth = (int)(Patwidth * rationew);
            Patheight = (int)(Patheight * rationew);
            size[0] = Oriwidth;
            size[1] = Oriheight;
            size[2] = Patwidth;
            size[3] = Patheight;
            return size;
        }
        public int[] rect_updateimg(int Oriwidth, int Oriheight, float rationew)
        {
            //var t = PatternPB.Size;
            //var t1 = OriginalPB.Size;
            int[] size = new int[2];
            Oriwidth = (int)(Oriwidth * rationew);
            Oriheight = (int)(Oriheight * rationew);

            size[0] = Oriwidth;
            size[1] = Oriheight;

            return size;
        }
        public int[] rectlocation_update(int x, int y, int orgwidth, int orgheight, float rationew)
        {
            int[] xy = new int[2];
            int diffx, diffy;
            int orgwidthnew, orgheightnew;
            orgwidthnew = (int)(orgwidth * rationew);
            orgheightnew = (int)(orgheight * rationew);
            x = (int)(x * rationew);
            y = (int)(y * rationew);
            //diffx = orgwidthnew - orgwidth;
            //diffy = orgheightnew - orgheight;
            //x = x - (diffx/2);
            //if (x < 0)
            //    x = 0;
            //y = y - (diffy/2);
            //if (y < 0)
            //    y = 0;
            //if(rationew!=1)
            //{
            //    double xr = ((double)x / orgwidth);

            //    double yr = ((double)y / orgheight);

            //    xy[0] = (int)(orgwidthnew * xr);
            //    xy[1] = (int)(orgheightnew * yr);
            //}
            //else
            //{
               xy[0] = x;
               xy[1] = y;
            //}


            return xy;


        }
        public Size rectopt_updating(int width, int height, float rationew)
        {
            Size rectsize;
            width = (int)(width * rationew);
            height = (int)(height * rationew);
            rectsize = new Size(width, height);
            return rectsize;
        }
        public Size rectopt_updating_1(int width, int height, float rationew)
        {
            Size rectsize;
            width = (int)(width / rationew);

            height = (int)(height / rationew);
            rectsize = new Size(width, height);
            return rectsize;
        }
        public Tuple<int, int> showoptrect(string path)
        {
            hdf5 beam1 = new hdf5();//, beam2 = new hdf5();
            scale = GuiMain.tiffimg.imgps / 0.125f;
            beam1.openfile(path);
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
            tempx = (int)(tempx * scale);
            tempy = (int)(tempy * scale);
            Tuple<int, int> optrect = new Tuple<int, int>(tempx, tempy);
            return optrect;
        }
        public int[] showopt(byte[,] array)
        {
            int[] result = new int[4];
            int toprow = 0, bottomrow = 0;
            int topcol = 0, bottomcol = 0;
            bool first = true;
            for (int i = 0; i < array.GetLength(0); ++i)
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    if (array[i, j] != 0)
                    {
                        if (first == true)
                        {
                            toprow = i;
                            first = false;
                        }
                        else
                        {
                            bottomrow = i;
                        }
                        break;
                    }

                }
            first = true;
            for (int i = 0; i < array.GetLength(1); ++i)
                for (int j = toprow; j <= bottomrow; ++j)
                {
                    if (array[j, i] != 0)
                    {
                        if (first == true)
                        {
                            topcol = i;
                            first = false;
                        }
                        else
                        {
                            bottomcol = i;
                        }
                        break;
                    }
                }
            result[0] = toprow;
            result[1] = bottomrow;
            result[2] = topcol;
            result[3] = bottomcol;
            return result;

        }
        public int[] showopt(float[,] array)
        {
            int[] result = new int[4];
            int toprow = 0, bottomrow = 0;
            int topcol = 0, bottomcol = 0;
            bool first = true;
            for (int i = 0; i < array.GetLength(0); ++i)
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    if (array[i, j] != 0)
                    {
                        if (first == true)
                        {
                            toprow = i;
                            first = false;
                        }
                        else
                        {
                            bottomrow = i;
                        }
                        break;
                    }

                }
            first = true;
            for (int i = 0; i < array.GetLength(1); ++i)
                for (int j = toprow; j <= bottomrow; ++j)
                {
                    if (array[j, i] != 0)
                    {
                        if (first == true)
                        {
                            topcol = i;
                            first = false;
                        }
                        else
                        {
                            bottomcol = i;
                        }
                        break;
                    }
                }
            result[0] = toprow;
            result[1] = bottomrow;
            result[2] = topcol;
            result[3] = bottomcol;
            return result;

        }
        //public Point rectopt_point_updating(int lefttopx, int lefttopy, float rationew)
        //{
        //    Point rectsize;
        //    lefttopx *= (int)rationew;
        //    lefttopy *= (int)rationew;
        //    rectsize = new Point(lefttopx, lefttopy);
        //    return rectsize;
        //}
    }

    //class patimgclass : orgimgclass
    //{


    //}
}
