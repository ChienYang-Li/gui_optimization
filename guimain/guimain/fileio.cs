using System;
using System.Collections.Generic;
using System.IO;

namespace guimain
{
    class fileio
    {
        public List<int> startposition;
        //public List<int> segmentlen;

        //public void computelen(int last)
        //{
        //    segmentlen = (List<int>)startposition.Zip(startposition.Skip(1), (x, y) => y - x);
        //}

        public void existencecheck(string filename)
        {
            if (File.Exists(filename)) File.Delete(filename);
        }

        public void mergefile(string prefilename, string filename, int precolnum, int startrow, double[,] dosage, int rownum, int colnum)
        {

            byte[] buf = new byte[dosage.GetLength(0) * dosage.GetLength(1) * sizeof(double)];
            Buffer.BlockCopy(dosage, 0, buf, 0, buf.Length);

            using (BinaryReader prereader = new BinaryReader(new FileStream(prefilename, FileMode.Open, FileAccess.Read)))
            {
                using (FileStream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None))
                using (BinaryWriter bw = new BinaryWriter(fileStream))
                {
                    byte[] buffer0 = new byte[precolnum * sizeof(double)];
                    long offset = startrow * precolnum * sizeof(double);

                    prereader.BaseStream.Seek(offset, SeekOrigin.Begin);

                    for (int i = 0; i < rownum; ++i)
                    {
                        //for(int i=0;i<row;++i)
                        prereader.Read(buffer0, 0, precolnum * sizeof(double));

                        bw.Write(buffer0);
                        bw.Write(buf, i * dosage.GetLength(1) * sizeof(double), colnum * sizeof(double));
                    }
                }
            }
        }

        public void AppendData(string filename, byte[,] Data)
        {
            using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(fileStream))
            {
                byte[] buf = new byte[Data.GetLength(0) * Data.GetLength(1)];
                Buffer.BlockCopy(Data, 0, buf, 0, Data.GetLength(0) * Data.GetLength(1));
                bw.Write(buf);
            }
        }

        public void AppendData(string filename, ushort[,] Data)
        {
            using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(fileStream))
            {
                byte[] buf = new byte[Data.GetLength(0) * Data.GetLength(1) * 2];
                Buffer.BlockCopy(Data, 0, buf, 0, Data.GetLength(0) * Data.GetLength(1) * 2);
                bw.Write(buf);
            }
        }

        public void AppendData(string filename, List<int> listdata)
        {
            int[] Data = new int[listdata.Count];
            listdata.CopyTo(Data);

            using (FileStream fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(fileStream))
            {
                byte[] buf = new byte[Data.Length * 4];
                Buffer.BlockCopy(Data, 0, buf, 0, Data.Length * 4);
                bw.Write(buf);
            }
        }

        public void AppendData(string filename, float[,] data, int offsetrow, int offsetcol, int rownum, int colnum)
        {
            float[] buf = new float[rownum * colnum];
            for (int i = 0; i < rownum; ++i)
            {
                for (int j = 0; j < colnum; ++j)
                {
                    buf[i * colnum + j] = data[offsetrow + i, offsetcol + j];
                }
            }

            using (FileStream fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write, FileShare.None))
            using (BinaryWriter bw = new BinaryWriter(fileStream))
            {
                byte[] buff = new byte[rownum * colnum * sizeof(float)];
                Buffer.BlockCopy(buf, 0, buff, 0, buff.Length);
                bw.Write(buff);
            }
        }


        public void writeld(string name, byte[,] buffer)
        {
            FileStream myFile = File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            BinaryWriter myWriter = new BinaryWriter(myFile);
            byte[] buf = new byte[buffer.GetLength(0) * buffer.GetLength(1)];
            Buffer.BlockCopy(buffer, 0, buf, 0, buffer.GetLength(0) * buffer.GetLength(1));
            myWriter.Write(buf);
            myWriter.Close();
            myFile.Close();
        }

        public byte[,] readtemp(string filename, long offsetrow, long offsetcol, int row, int col, int totallen)
        {

            byte[] buffer0 = new byte[col];
            byte[,] buffer = new byte[row, col];
            int index1 = startposition.BinarySearch((int)offsetrow);
            if (index1 < 0)
            {
                index1 = ~index1 - 1;
            }
            int index2 = startposition.BinarySearch((int)offsetrow + row - 1);
            if (index2 < 0)
            {
                index2 = ~index2 - 1;
            }

            int rownum, currow = 0;
            for (int k = index1; k <= index2; ++k)
            {
                rownum = startposition[k + 1] - (int)offsetrow;
                if (rownum > row)
                    rownum = row;
                else
                    row -= rownum;
                using (BinaryReader reader = new BinaryReader(new FileStream(filename + k.ToString() + ".txt", FileMode.Open, FileAccess.Read)))
                {

                    for (int i = 0; i < rownum; ++i)
                    {
                        reader.BaseStream.Seek((offsetrow - startposition[k]) * totallen + offsetcol + totallen * i, SeekOrigin.Begin);
                        //for(int i=0;i<row;++i)
                        reader.Read(buffer0, 0, col);

                        for (int j = 0; j < col; ++j)
                            buffer[currow, j] = buffer0[j];

                        ++currow;
                    }
                }

                offsetrow = startposition[k + 1];
            }
            return buffer;
        }

        public ushort[,] readtempushort(string filename, long offsetrow, long offsetcol, int row, int col, int totallen)
        {

            byte[] buffer0 = new byte[col * 2];
            ushort[,] buffer = new ushort[row, col];
            int index1 = startposition.BinarySearch((int)offsetrow);
            if (index1 < 0)
            {
                index1 = ~index1 - 1;
            }
            int index2 = startposition.BinarySearch((int)offsetrow + row - 1);
            if (index2 < 0)
            {
                index2 = ~index2 - 1;
            }

            int rownum, currow = 0;
            for (int k = index1; k <= index2; ++k)
            {
                rownum = startposition[k + 1] - (int)offsetrow;
                if (rownum > row)
                    rownum = row;
                else
                    row -= rownum;
                using (BinaryReader reader = new BinaryReader(new FileStream(filename + k.ToString() + ".txt", FileMode.Open, FileAccess.Read)))
                {

                    for (int i = 0; i < rownum; ++i)
                    {
                        reader.BaseStream.Seek(((offsetrow - startposition[k]) * totallen + offsetcol + totallen * i) * 2, SeekOrigin.Begin);
                        //for(int i=0;i<row;++i)
                        reader.Read(buffer0, 0, buffer0.Length);

                        using (var memoryStream = new MemoryStream(buffer0))
                        using (var reader2 = new BinaryReader(memoryStream))
                            for (int j = 0; j < col; ++j)
                                buffer[currow, j] = reader2.ReadUInt16();

                        ++currow;
                    }
                }

                offsetrow = startposition[k + 1];
            }
            return buffer;
        }

        public float[,] readtempfloat(string filename, long offsetrow, long offsetcol, int row, int col, int totallen)
        {

            byte[] buffer0 = new byte[col * sizeof(float)];
            float[,] buffer = new float[row, col];
            using (BinaryReader reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                for (int i = 0; i < row; ++i)
                {
                    reader.BaseStream.Seek((offsetrow * totallen + offsetcol + totallen * i) * sizeof(float), SeekOrigin.Begin);
                    //for(int i=0;i<row;++i)
                    reader.Read(buffer0, 0, col * sizeof(float));

                    using (var memoryStream = new MemoryStream(buffer0))
                    using (var reader2 = new BinaryReader(memoryStream))
                        for (int j = 0; j < col; ++j)
                            buffer[i, j] = reader2.ReadSingle();
                }
            }
            return buffer;
        }
    }
}
