using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HDF5DotNet;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace guimain
{
    public class hdf5
    {
        public H5FileId fileId;
        public H5DataSpaceId dsi;
        public H5DataSetId dataset;
        public H5GroupId groupId;
        public H5AttributeId attributeId;
        public H5DataTypeId attributeType;
        public int rownum;
        public int colnum;
        public int chunkrow;
        public int chunkcol;

        public void createfile(string filename, int ROWS, int COLS, int chunkrow, int chunkcol, int type = 1)//type 0 = byte , type 1 = float
        {
            rownum = ROWS;
            colnum = COLS;
            this.chunkrow = chunkrow;
            this.chunkcol = chunkcol;
            fileId = H5F.create(filename, H5F.CreateMode.ACC_TRUNC);
            dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5PropertyListId pli = H5P.create(H5P.PropertyListClass.DATASET_CREATE);  // added
            H5P.setChunk(pli, new long[] { chunkrow, chunkcol });  // added     
            if (type == 0)
                dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
            else if (type == 1)
                dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
        }

        public void openfile(string filename)
        {
            FileInfo file = new FileInfo(filename);

            if (File.Exists(filename))
            {
                if (file.Length > 0)
                {
                    try
                    {
                        fileId = H5F.open(filename, H5F.OpenMode.ACC_RDWR);
                        dataset = H5D.open(fileId, "array");
                        dsi = H5D.getSpace(dataset);
                        long[] dims = H5S.getSimpleExtentDims(dsi);
                        rownum = (int)dims[0];
                        colnum = (int)dims[1];
                    }
                    catch (H5FopenException)
                    {
                        MessageBox.Show(filename + "檔案不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        file.Delete();

                    }
                    catch (HDF5DotNet.H5DopenException)
                    {
                        MessageBox.Show("程式Run過程發生錯誤，請再跑一次程式", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        file.Delete();

                    }
                    finally
                    {

                    }
                }
                else if (file.Length == 0)
                {
                    file.Delete();
                }



            }
            else
            {
                MessageBox.Show(filename + "檔案不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        public int gettype()
        {
            var type = H5D.getType(dataset);
            var classtype = H5T.getClass(type);

            if ((int)(classtype) != 1)
                return 0;//byte
            else
                return 1;//float

        }


        public byte[,] readbytedata()
        {
            byte[,] data = new byte[rownum, colnum];
            try
            {
                H5D.read(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8),
               new H5Array<byte>(data));

                return data;
            }
            catch (IndexOutOfRangeException)
            {

                return data = null;
            }

        }

        public byte[,] readbytedata(int rowoffset, int coloffset, int subrow, int subcol)//左上座標及長寬
        {
            byte[,] data = new byte[subrow, subcol];
            try
            {
                H5DataSpaceId memspaceid = H5S.create_simple(2, new long[] { subrow, subcol });
                H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { rowoffset, coloffset }, new long[] { subrow, subcol });
                H5D.read(dataset, H5T.copy(H5T.H5Type.NATIVE_B8), memspaceid, dsi,
                    new H5PropertyListId(new H5P.Template()), new H5Array<byte>(data));

                return data;
            }
            catch (NullReferenceException)
            {
                return null;
            }



        }

        public float[,] readfloatdata()
        {
            try
            {
                float[,] data = new float[rownum, colnum];

                H5D.read(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT),
                    new H5Array<float>(data));

                return data;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("路徑下不存在檔案", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

        }

        public float[,] readfloatdata(int rowoffset, int coloffset, int subrow, int subcol)
        {
            try
            {
                float[,] data = new float[subrow, subcol];
                H5DataSpaceId memspaceid = H5S.create_simple(2, new long[] { subrow, subcol });
                H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { rowoffset, coloffset }, new long[] { subrow, subcol });
                H5D.read(dataset, H5T.copy(H5T.H5Type.NATIVE_FLOAT), memspaceid, dsi,
                    new H5PropertyListId(new H5P.Template()), new H5Array<float>(data));

                return data;
            }
            catch (IndexOutOfRangeException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;

            }


        }

        public float[,] readspotdata(uint label)
        {
            long[] dims = H5S.getSimpleExtentDims(dsi);
            float[,] data = new float[dims[0], dims[1]];
            H5DataSpaceId memspaceid = H5S.create_simple(2, new long[] { dims[0], dims[1] });
            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0, label }, new long[] { dims[0], dims[1], 1 });
            H5D.read(dataset, H5T.copy(H5T.H5Type.NATIVE_FLOAT), memspaceid, dsi,
                new H5PropertyListId(new H5P.Template()), new H5Array<float>(data));
            return data;

        }

        public float[,,] readspotdata1000(int label)
        {
            long[] dims = H5S.getSimpleExtentDims(dsi);
            float[,,] data = new float[dims[0], dims[1], 1000];
            H5DataSpaceId memspaceid = H5S.create_simple(3, new long[] { dims[0], dims[1], 1000 });
            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0, label * 1000 }, new long[] { dims[0], dims[1], 1000 });
            H5D.read(dataset, H5T.copy(H5T.H5Type.NATIVE_FLOAT), memspaceid, dsi,
                new H5PropertyListId(new H5P.Template()), new H5Array<float>(data));
            return data;

        }

        public void appenddata(int rowoffset, int coloffset, byte[,] data)
        {
            int ROWS = data.GetLength(0), COLS = data.GetLength(1);
            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { rowoffset, coloffset }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<byte>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi2, dsi, pli, new H5Array<byte>(data));
        }

        public void appenddata(int rowoffset, int coloffset, float[,] data)
        {
            int ROWS = data.GetLength(0), COLS = data.GetLength(1);
            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { rowoffset, coloffset }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<float>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT), dsi2, dsi, pli, new H5Array<float>(data));
        }

        public void closefile()
        {
            try
            {
                H5D.close(dataset);
                H5F.close(fileId);
                //H5D.close(attributeType);

                //H5D.close(attributeType);

            }
            catch (System.NullReferenceException)
            {

            }

        }
        public void writearray(float[,] d, string filename)
        {
            filename += ".h5";
            int ROWS = d.GetLength(0);
            int COLS = d.GetLength(1);
            int chunkrow = ROWS;
            int chunkcol = COLS;
            fileId = H5F.create(filename, H5F.CreateMode.ACC_TRUNC);
            dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5PropertyListId pli = H5P.create(H5P.PropertyListClass.DATASET_CREATE);  // added
            H5P.setChunk(pli, new long[] { chunkrow, chunkcol });  // added     
                                                                   //if (type == 0)
                                                                   //    dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
                                                                   //else if (type == 1)
            dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified


            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0 }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli2 = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<float>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_FLOAT), dsi2, dsi, pli2, new H5Array<float>(d));


            H5D.close(dataset);
            H5F.close(fileId);
        }

        public void writearray(byte[,] byteArray, string filename)
        {
            var d = new byte[byteArray.GetLength(0), byteArray.GetLength(1)];

            for (int i = 0; i < byteArray.GetLength(0); ++i)
            {
                for (int j = 0; j < byteArray.GetLength(1); ++j)
                {
                    d[i, j] = byteArray[i, j];
                }
            }

            filename += ".h5";
            int ROWS = d.GetLength(0);
            int COLS = d.GetLength(1);
            int chunkrow = ROWS;
            int chunkcol = COLS;
            fileId = H5F.create(filename, H5F.CreateMode.ACC_TRUNC);
            dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5PropertyListId pli = H5P.create(H5P.PropertyListClass.DATASET_CREATE);  // added
            H5P.setChunk(pli, new long[] { chunkrow, chunkcol });  // added     
                                                                   //if (type == 0)
                                                                   //    dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
                                                                   //else if (type == 1)
            dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified


            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0 }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli2 = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<byte>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi2, dsi, pli2, new H5Array<byte>(d));


            H5D.close(dataset);
            H5F.close(fileId);
        }

        public void writearray(List<beam>[,] byteArray, string filename, float imgps)
        {
            var d0 = new byte[byteArray.GetLength(0), byteArray.GetLength(1)];

            for (int i = 0; i < byteArray.GetLength(0); ++i)
            {
                for (int j = 0; j < byteArray.GetLength(1); ++j)
                {
                    if (byteArray[i, j] != null)
                        for (int k = 0; k < byteArray[i, j].Count; ++k)
                        {
                            if (byteArray[i, j][k].eon > 0)
                                d0[i, j] = 1;//+= byteArray[i, j][k].eon;
                        }
                }
            }

            interp itp = new interp();
            byte[,] d = itp.nearestscale(d0, byteArray.GetLength(1), byteArray.GetLength(0), imgps / 0.125f, imgps / 0.125f);


            filename += ".h5";
            int ROWS = d.GetLength(0);
            int COLS = d.GetLength(1);
            int chunkrow = ROWS;
            int chunkcol = COLS;
            fileId = H5F.create(filename, H5F.CreateMode.ACC_TRUNC);
            dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5PropertyListId pli = H5P.create(H5P.PropertyListClass.DATASET_CREATE);  // added
            H5P.setChunk(pli, new long[] { chunkrow, chunkcol });  // added     
                                                                   //if (type == 0)
                                                                   //    dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
                                                                   //else if (type == 1)
            dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified


            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0 }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli2 = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<byte>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi2, dsi, pli2, new H5Array<byte>(d));


            H5D.close(dataset);
            H5F.close(fileId);
        }

        public void writearrayall(List<beam>[,] byteArray, string filename)
        {
            var d = new byte[byteArray.GetLength(0), byteArray.GetLength(1)];

            for (int i = 0; i < byteArray.GetLength(0); ++i)
            {
                for (int j = 0; j < byteArray.GetLength(1); ++j)
                {
                    if (byteArray[i, j] != null)
                        for (int k = 0; k < byteArray[i, j].Count; ++k)
                        {
                            if (byteArray[i, j][k].eon > 0)
                                d[i, j] = 1;//+= byteArray[i, j][k].eon;
                        }
                }
            }

            filename += ".h5";
            int ROWS = d.GetLength(0);
            int COLS = d.GetLength(1);
            int chunkrow = ROWS;
            int chunkcol = COLS;
            fileId = H5F.create(filename, H5F.CreateMode.ACC_TRUNC);
            dsi = H5S.create_simple(2, new long[] { ROWS, COLS });
            H5PropertyListId pli = H5P.create(H5P.PropertyListClass.DATASET_CREATE);  // added
            H5P.setChunk(pli, new long[] { chunkrow, chunkcol });  // added     
                                                                   //if (type == 0)
                                                                   //    dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified
                                                                   //else if (type == 1)
            dataset = H5D.create(fileId, "array", new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi, H5P.create(H5P.PropertyListClass.LINK_CREATE), pli, H5P.create(H5P.PropertyListClass.DATASET_ACCESS));  // modified


            H5S.selectHyperslab(dsi, H5S.SelectOperator.SET, new long[] { 0, 0 }, new long[] { ROWS, COLS });
            H5DataSpaceId dsi2 = H5S.create_simple(2, new long[] { ROWS, COLS });  // added
            H5PropertyListId pli2 = new H5PropertyListId(H5P.Template.DEFAULT);  // added
            H5D.write<byte>(dataset, new H5DataTypeId(H5T.H5Type.NATIVE_B8), dsi2, dsi, pli2, new H5Array<byte>(d));


            H5D.close(dataset);
            H5F.close(fileId);
        }

        public void setmin(float min)
        {
            H5GroupId groupId = H5G.create(fileId, "Group2");
            H5DataTypeId attributeType0 = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
            H5DataSpaceId attributeSpace0 = H5S.create_simple(1, new long[] { 1 });
            H5AttributeId attributeId0 = H5A.create(groupId, "min", attributeType0, attributeSpace0);
            H5A.write<float>(attributeId0, attributeType0, new H5Array<float>(new float[] { min }));
            H5G.close(groupId);
            H5A.close(attributeId0);
        }

        public double getmin()
        {
            float[] min = new float[1];
            H5DataTypeId attributeType = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
            H5GroupId groupId = H5G.open(fileId, "Group2");
            H5AttributeId attributeId = H5A.openName(groupId, "min");
            H5A.read<float>(attributeId, attributeType, new H5Array<float>(min));
            H5G.close(groupId);
            H5A.close(attributeId);
            return min[0];
        }

        public void setmax(float max)
        {
            H5GroupId groupId = H5G.create(fileId, "Group");
            H5DataTypeId attributeType0 = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
            H5DataSpaceId attributeSpace0 = H5S.create_simple(1, new long[] { 1 });
            H5AttributeId attributeId0 = H5A.create(groupId, "max", attributeType0, attributeSpace0);
            H5A.write<float>(attributeId0, attributeType0, new H5Array<float>(new float[] { max }));
            H5G.close(groupId);
            H5A.close(attributeId0);
        }

        public double getmax()
        {
            float[] max = new float[1];
            H5DataTypeId attributeType = H5T.copy(H5T.H5Type.NATIVE_FLOAT);
            H5GroupId groupId = H5G.open(fileId, "Group");
            H5AttributeId attributeId = H5A.openName(groupId, "max");
            H5A.read<float>(attributeId, attributeType, new H5Array<float>(max));
            H5G.close(groupId);
            H5A.close(attributeId);
            return max[0];
        }

    }
}
