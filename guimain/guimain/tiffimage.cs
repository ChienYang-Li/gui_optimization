using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitMiracle.LibTiff.Classic;
using HDF5DotNet;

namespace guimain
{
    public class tiffimage
    {
        public Tiff image;
        public string imgfilename;
        public int width;
        public int height;
        public int stripMax;
        public float imgps = 0;//image pixel size

        public void opentiff(string filename)
        {
            image = Tiff.Open(filename, "r");

            if (image == null)
            {
                System.Console.Error.WriteLine("Could not open incoming image");
                return;
            }
            string[] patname = filename.Split('\\');
            imgfilename = patname[patname.Length - 1];
        }

        public void closetiff()
        {
            image.Close();
        }

        public void readtiffproperty()
        {
            FieldValue[] value;
            value = image.GetField(TiffTag.XRESOLUTION);
            float dpi = value[0].ToFloat();

            value = image.GetField(TiffTag.IMAGEWIDTH);
            width = value[0].ToInt();

            value = image.GetField(TiffTag.IMAGELENGTH);
            height = value[0].ToInt();

            stripMax = image.NumberOfStrips();

            value = image.GetField(TiffTag.RESOLUTIONUNIT);
            string resunit = value[0].ToString();

            if (resunit == "INCH")
                imgps = 25400 / dpi;//每個點幾um
            else if (resunit == "CENTIMETER")
                imgps = 10000 / dpi;

            imgps = 1.25f;
        }

        public List<int> readpattern(string path)
        {
            int rowsize = image.ScanlineSize();
            byte[] buf = new byte[rowsize];
            long maxsize = (long)((double)2.5 * 1024 * 1024 * 1024);
            List<int> imgstartrow = new List<int>();

            int fixedh = (int)((double)maxsize / (double)width);

            if (fixedh > height)
                fixedh = height;
            string imgname = imgfilename + "_img.h5";
            hdf5 imgfile = new hdf5();
            imgfile.createfile(path+imgname, height, width, fixedh, width, 0);
            for (int row = 0, cnt = 0; row < height; row += fixedh, ++cnt)
            {
                if (row + fixedh >= height)
                    fixedh = height - row;

                byte[] buff = new byte[rowsize * fixedh];
                byte[,] img = new byte[fixedh, width];

                for (int num = 0; num < fixedh; ++num)
                {
                    image.ReadScanline(buf, row + num);
                    Buffer.BlockCopy(buf, 0, buff, rowsize * num, buf.Length);
                }

                // Deal with photometric interpretations
                FieldValue[] value = image.GetField(TiffTag.PHOTOMETRIC);
                if (value == null)
                {
                    System.Console.Error.WriteLine("Image has an undefined photometric interpretation");
                    //return;
                }
                else
                {
                    Photometric photo = (Photometric)value[0].ToInt();
                    if (photo != Photometric.MINISWHITE)
                    {
                        // Flip bits
                        System.Console.Out.WriteLine("Fixing the photometric interpretation");

                        for (int count = 0; count < buff.Length; count++)
                            buff[count] = (byte)~buff[count];
                    }
                }

                // Deal with fillorder 
                value = image.GetField(TiffTag.FILLORDER);
                if (value == null)
                {
                    //System.Console.Error.WriteLine("Image has an undefined fillorder");

                    for (int i = 0; i < fixedh; ++i)
                    {
                        for (int count = 0, mask = 128; count < width; ++count)
                        {

                            if ((buff[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                            if (mask != 1)
                                mask /= 2;
                            else
                                mask = 128;
                        }
                    }
                }
                else
                {
                    FillOrder fillorder = (FillOrder)value[0].ToInt();
                    if (fillorder == FillOrder.LSB2MSB)
                    {
                        //We need to swap bits -- ABCDEFGH becomes HGFEDCBA
                        //System.Console.Out.WriteLine("Fixing the fillorder");

                        for (int i = 0; i < fixedh; ++i)
                        {
                            for (int count = 0, mask = 1; count < width; ++count)
                            {

                                if ((buff[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                                if (mask != 1)
                                    mask *= 2;
                                else
                                    mask = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < fixedh; ++i)
                        {
                            for (int count = 0, mask = 128; count < width; ++count)
                            {

                                if ((buff[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                                if (mask != 1)
                                    mask /= 2;
                                else
                                    mask = 128;
                            }
                        }
                    }
                }

                //AppendData(imgname+cnt.ToString()+".txt",row,0,img);
                imgfile.appenddata(row, 0, img);

                imgstartrow.Add(row);
            }
            imgstartrow.Add(height);
            imgfile.closefile();
            return imgstartrow;

        }

        //public static byte[] Compress(byte[] data)
        //{
        //    MemoryStream output = new MemoryStream();
        //    using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Fastest))
        //    {
        //        dstream.Write(data, 0, data.Length);
        //    }
        //    return output.ToArray();
        //}

        //public static byte[] Decompress(byte[] data)
        //{
        //    MemoryStream input = new MemoryStream(data);
        //    MemoryStream output = new MemoryStream();
        //    using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
        //    {
        //        dstream.CopyTo(output);
        //    }
        //    return output.ToArray();
        //}

        public byte[,] readstrip(int stripnumber)
        {

            // Check that it is of a type that we support
            FieldValue[] value;

            value = image.GetField(TiffTag.ROWSPERSTRIP);
            int rps = value[0].ToInt();

            // Read in the possibly multiple strips 
            int stripSize = image.StripSize();

            if (stripnumber == stripMax - 1)
                rps = height - rps * (stripMax - 1);


            byte[] buffer = new byte[stripSize];
            byte[,] img = new byte[rps, width];


            int result = image.ReadEncodedStrip(stripnumber, buffer, 0, stripSize);
            if (result == -1)
            {
                System.Console.Error.WriteLine("Read error on input strip number {0}", stripnumber);
                //return;
            }




            // Deal with photometric interpretations 
            value = image.GetField(TiffTag.PHOTOMETRIC);
            if (value == null)
            {
                System.Console.Error.WriteLine("Image has an undefined photometric interpretation");
                //return;
            }
            else
            {
                Photometric photo = (Photometric)value[0].ToInt();
                if (photo != Photometric.MINISWHITE)
                {
                    // Flip bits
                    System.Console.Out.WriteLine("Fixing the photometric interpretation");

                    for (int count = 0; count < stripSize; count++)
                        buffer[count] = (byte)~buffer[count];
                }
            }

            int rowsize = image.ScanlineSize();
            // Deal with fillorder 
            value = image.GetField(TiffTag.FILLORDER);
            if (value == null)
            {
                //System.Console.Error.WriteLine("Image has an undefined fillorder");

                for (int i = 0; i < rps; ++i)
                {
                    for (int count = 0, mask = 128; count < width; ++count)
                    {

                        if ((buffer[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                        if (mask != 1)
                            mask /= 2;
                        else
                            mask = 128;
                    }
                }
            }
            else
            {
                FillOrder fillorder = (FillOrder)value[0].ToInt();
                if (fillorder == FillOrder.LSB2MSB)
                {
                    //We need to swap bits -- ABCDEFGH becomes HGFEDCBA
                    //System.Console.Out.WriteLine("Fixing the fillorder");

                    for (int i = 0; i < rps; ++i)
                    {
                        for (int count = 0, mask = 1; count < width; ++count)
                        {

                            if ((buffer[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                            if (mask != 1)
                                mask *= 2;
                            else
                                mask = 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < rps; ++i)
                    {
                        for (int count = 0, mask = 128; count < width; ++count)
                        {

                            if ((buffer[i * rowsize + count / 8] & mask) != 0) img[i, count] = 1;

                            if (mask != 1)
                                mask /= 2;
                            else
                                mask = 128;
                        }
                    }
                }
            }
            // Do whatever it is we do with the buffer - we dump it in hex 
            value = image.GetField(TiffTag.IMAGEWIDTH);
            if (value == null)
            {
                System.Console.Error.WriteLine("Image does not define its width");
                //return;
            }
            else
            {

                //for (int count = 0; count < stripSize; count++)
                //{
                //    System.Console.Out.Write("{0:X2}", buffer[count]);
                //    if ((count + 1) % (width / 8) == 0)
                //        System.Console.Out.WriteLine();
                //    else
                //        System.Console.Out.Write(" ");
                //}
            }
            return img;

        }

        public void AppendData(string filename, int rowoffset, int coloffset, byte[,] Data)
        {
            //using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            //using (BinaryWriter bw = new BinaryWriter(fileStream))
            //{
            //    byte[] buf = new byte[Data.GetLength(0) * Data.GetLength(1)];
            //    Buffer.BlockCopy(Data, 0, buf, 0, Data.GetLength(0) * Data.GetLength(1));
            //    bw.Write(buf);
            //}
            int ROWS = Data.GetLength(0), COLS = Data.GetLength(1);
            H5FileId fileId = H5F.create(filename + ".h5", H5F.CreateMode.ACC_TRUNC);
            H5DataSpaceId dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5DataSetId dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi);
            //byte[,] array = new byte[ROWS, COLS];
            //byte value = 1;
            //for (int i = 0; i < array.GetLength(0); i++)
            //{
            //    for (int j = 0; j < array.GetLength(1); j++)
            //    {
            //        array[i, j] = value++;
            //    }
            //}
            //for (int i = 0; i < ROWS; i++)

            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { rowoffset, coloffset }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            //byte[,] row = new byte[1, COLS];
            //for (int j = 0; j < COLS; j++)
            //{
            //    row[0, j] = Data[i, j];
            //}
            H5PropertyListId pli = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<byte>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi2, dsi, pli, new H5Array<byte>(Data));  // modified

            H5D.close(dataset);
            H5F.close(fileId);

        }


    }
}
