using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guimain
{
    class matoperation
    {

        public void beamsuperposition0(float[,] pattern, int px, int py, float[,] mask)
        {
            int xlen = mask.GetLength(0) / 2;
            int ylen = mask.GetLength(1) / 2;
            float a;
            for (int i = px - xlen, mrow = 0; i < px + xlen; ++i, ++mrow)
                for (int j = py - ylen, mcol = 0; j < py + ylen; ++j, ++mcol)
                {
                    pattern[i, j] += mask[mrow, mcol];
                    if (pattern[i, j] < 0)
                        a = pattern[i, j];
                }

        }

        public void beamsuperposition_imgps(float[,] pattern, int py, int px, float[,,] mask, int pos,float scale)
        {
            int height = (int)(mask.GetLength(0) / scale + 0.5);
            int width = (int)(mask.GetLength(1) / scale + 0.5);

            if(height%2==0)
            {
                height -= 1;
            }
            if (width % 2 == 0)
            {
                width -= 1;
            }
            int ylen = height / 2;
            
            int xlen = width / 2;
            
            for (int i = py - ylen, mrow = 0; i <= py + ylen; ++i, ++mrow)
                for (int j = px - xlen, mcol = 0; j <= px + xlen; ++j, ++mcol)
                {
                    pattern[i, j] += mask[(int)(mrow*scale+0.5), (int)(mcol*scale+0.5), pos];
                    
                }
        }
        public void beamsuperposition(float[,] pattern, int px, int py, float[,,] mask, int pos)
        {
            int xlen = mask.GetLength(0) / 2;
            int ylen = mask.GetLength(1) / 2;
            float a;
            for (int i = px - xlen, mrow = 0; i <= px + xlen; ++i, ++mrow)
                for (int j = py - ylen, mcol = 0; j <= py + ylen; ++j, ++mcol)
                {
                    pattern[i, j] += mask[mrow, mcol, pos];
                    if (pattern[i, j] < 0)
                        a = pattern[i, j];
                }

        }
        public void beamsuperposition(float[,] pattern, int rowcenter, int colcenter, float[,] mask)
        {
            int rowlen = mask.GetLength(0) / 2;
            int collen = mask.GetLength(1) / 2;
            //float a;
            for (int i = rowcenter - rowlen, mrow = 0; i < rowcenter + rowlen; ++i, ++mrow)
                for (int j = colcenter - collen, mcol = 0; j < colcenter + collen; ++j, ++mcol)
                {
                    pattern[i, j] += mask[mrow, mcol];
                }

        }

        public float[,] cuttingboundary0(float[,] dosage, int offsetrow, int offsetcol)
        {
            float[,] cutting = new float[dosage.GetLength(0) - offsetrow * 2, dosage.GetLength(1) - offsetcol * 2];
            int m = cutting.GetLength(0), n = cutting.GetLength(1);
            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                    cutting[i, j] = dosage[i + offsetrow, j + offsetcol];

            return cutting;
        }

        public float[,] cuttingboundary(float[,] dosage, int offsetrow, int offsetcol, int rows, int cols)
        {
            float[,] cutting = new float[rows, cols];
            //int m = cutting.GetLength(0), n = cutting.GetLength(1);
            //for (int i = 0; i < rows; ++i)
            Parallel.For(0, rows, i =>
            {
                for (int j = 0; j < cols; ++j)
                    cutting[i, j] = dosage[i + offsetrow, j + offsetcol];
            });

            return cutting;
        }

        public float[,] sum2array(float[,] array, float[,] temp)
        {
            int m = temp.GetLength(0), n = temp.GetLength(1);

            for (int i = 0; i < m; ++i)
                for (int j = 0; j < n; ++j)
                    array[i, j] += temp[i, j];
            return array;
        }

        public void temparray(float[,] array, int startrow, ref float[,] temp)
        {
            int m = temp.GetLength(0), n = temp.GetLength(1);

            for (int i = 0; i < m; ++i)
            {
                for (int j = 0; j < n; ++j)
                    temp[i, j] = array[i + startrow, j];
            }
        }

        public float[,] matcrop(float[,] array, int rownum, int colnum)
        {
            float[,] mat = new float[rownum, colnum];
            for (int i = 0; i < rownum; ++i)
                for (int j = 0; j < colnum; ++j)
                {
                    mat[i, j] = array[i, j];
                }
            return mat;
        }
    }
}
