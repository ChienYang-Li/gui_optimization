using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace guimain
{
    public class ldconfig
    {
        public float[] ldx = new float[160000];
        public float[] ldy = new float[160000];
        public float[] ldxmax = new float[20];
        public float[] ldxmin = new float[20];
        public float[] ldymax = new float[20];
        public float[] ldymin = new float[20];
        public float ldps;
        public float xmin = float.MaxValue, xmax = 0, ymin = float.MaxValue, ymax = 0;

        public void readldconfig(string filename,float imgps)
        {
            if(!File.Exists(filename))
            {
                return;
            }
            StreamReader config = new StreamReader(filename);
            xmin = float.MaxValue;
            xmax = 0;
            ymin = float.MaxValue;
            ymax = 0;
            int cnt = 0;
            float x0 =0, y0=0,tempy=0,tempx=0;

            while (!config.EndOfStream)
            {                                           // 每次讀取一行，直到檔尾
                string line = config.ReadLine();            // 讀取文字到 line 變數
                if (line == "")
                    continue;
                bool first = true;
                bool get = false;
                string[] substr = line.Split(' ');
                for(int i=0;i<substr.Length;++i)
                {
                    if(substr[i]!=""&&first)
                    {
                        x0 = int.Parse(substr[i]);
                        //tempx = (int)(float.Parse(substr[i]));
                        //y0 = (int)(float.Parse(substr[i]));

                        //x0 = (int)(float.Parse(substr[i]));//test1
                        first = false;
                    }
                    else if(substr[i]!="")
                    {
                        y0 = int.Parse(substr[i]);
                        //tempy = (int)(float.Parse(substr[i]));
                        //x0 = (int)(float.Parse(substr[i]));
                        //y0 = (int)(float.Parse(substr[i]));//test1
                        get = true;
                    }
                }
                //x0 = int.Parse(substr[0]);
                //y0 = int.Parse(substr[1]);

                //x0 = (float)(tempx * Math.Cos(Math.PI / 4) - tempy * Math.Sin(Math.PI / 4));
                //y0 = (float)(tempx * Math.Sin(Math.PI / 4) + tempy * Math.Cos(Math.PI / 4));

                if (get)
                {
                    if (x0 == -1)
                    {
                        ldx[cnt] = x0;
                        ldy[cnt++] = y0;
                    }
                    else
                    {
                        ldx[cnt] = x0 / imgps;
                        ldy[cnt] = y0 / imgps;

                        if (ldx[cnt] > xmax)
                            xmax = ldx[cnt];
                        if (ldx[cnt] < xmin)
                            xmin = ldx[cnt];
                        if (ldy[cnt] > ymax)
                            ymax = ldy[cnt];
                        if (ldy[cnt] < ymin)
                            ymin = ldy[cnt];

                        ++cnt;
                    }
                }
                
            }
            config.Close();

            ldps = imgps;
        }

        public void lightspotrange()
        {
            float ldmax0 = Int32.MinValue, ldmin0 = Int32.MaxValue;

            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 1000; ++k)
                    {
                        if (ldx[k + i * 1000 + j * 20000] == -1)
                            continue;
                        if (ldx[k + i * 1000 + j * 20000] > ldmax0)
                            ldmax0 = ldx[k + i * 1000 + j * 20000];
                        if (ldx[k + i * 1000 + j * 20000] < ldmin0)
                            ldmin0 = ldx[k + i * 1000 + j * 20000];
                    }
                }
                ldxmax[i] = ldmax0;//每個LD的X最大值
                ldxmin[i] = ldmin0;//每個LD的X最小值
                ldmax0 = Int32.MinValue;
                ldmin0 = Int32.MaxValue;
            }

            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 1000; ++k)
                    {
                        if (ldy[k + i * 1000 + j * 20000] == -1)
                            continue;
                        if (ldy[k + i * 1000 + j * 20000] > ldmax0)
                            ldmax0 = ldy[k + i * 1000 + j * 20000];
                        if (ldy[k + i * 1000 + j * 20000] < ldmin0)
                            ldmin0 = ldy[k + i * 1000 + j * 20000];
                    }
                }
                ldymax[i] = ldmax0;
                ldymin[i] = ldmin0;
                ldmax0 = Int32.MinValue;
                ldmin0 = Int32.MaxValue;
            }

        }

        public List<byte> checkrange(int start, int end)
        {
            List<byte> table = new List<byte>();
            for (int i = 0; i < ldxmax.Length; ++i)
            {
                if (start > ldxmax[i] || end < ldxmin[i])//不在LD範圍內就continue;
                    continue;
                table.Add((byte)(i));//否則記錄下當下的LD的範圍
            }
            return table;
        }

        public Tuple<float, float> scanrange(List<byte> table)
        {
            float ldmax = 0, ldmin = int.MaxValue;

            for (int i = 0; i < table.Count; ++i)//從tablenum裡的範圍中找到LD中的最大及最小的y座標
            {
                if (ldymax[table[i]] > ldmax)
                    ldmax = ldymax[table[i]];//找現在最大的ldmax

                if (ldymin[table[i]] < ldmin)
                    ldmin = ldymin[table[i]];//找現在最小的ldmin
            }
            return new Tuple<float, float>(ldmax, ldmin);
        }
        public void addxyoffset(int xoffset,int yoffset)
        {
           for(int i=0;i<160000;++i)
           {
                if (ldx[i] == -1)
                {
                    continue;
                }



                ldx[i] += xoffset;
                
                
                ldy[i] += yoffset;
               
           }
            xmin += xoffset;
            xmax += xoffset;
            ymin += yoffset;
            ymax += yoffset;
          
        }
        public void Multires(float res)
        {
            for (int i = 0; i < 160000; ++i)
            {
                if (ldx[i] == -1)
                {
                    continue;
                }



                ldx[i] = (ldx[i]*res);


                ldy[i] = (ldy[i]*res);

            }

            xmax = (xmax* res);
            xmin = (xmin * res);
            ymax = (ymax * res);
            ymin = (ymin * res);
        }
        public void modifyres(float newps)
        {
            if (Math.Abs(ldps - newps) > 0.00001)
            {
                float ratio = ldps / newps;

                for (int i = 0; i < 160000; ++i)
                {
                    if (ldx[i] == -1)
                    {
                        continue;
                    }



                    ldx[i] = (ldx[i] * ratio);


                    ldy[i] = (ldy[i] * ratio);

                }

                xmax = (xmax * ratio);
                xmin = (xmin * ratio);
                ymax = (ymax * ratio);
                ymin = (ymin * ratio);

                ldps = newps;
            }
        }
        public void  showfacet()
        {
            int[] ldyfacet = new int[ldy.GetLength(0)];
            int[] ldxfacet = new int[ldx.GetLength(0)];
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 1000; ++k)
                    {
                        int cnt=0;
                        if (j==0)
                        {
                            cnt = 1;
                            ldyfacet[k + i * 1000 + j * 20000] = cnt;
                        }
                        else
                        {
                            cnt = j+1;
                            ldyfacet[k + i * 1000 + j * 20000] = cnt;
                        }
                       



                    }
                }
               
            }
            for (int i = 0; i < 20; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    for (int k = 0; k < 1000; ++k)
                    {
                        int cnt = 0;
                        if (j == 0)
                        {
                            cnt = 1;
                            ldxfacet[k + i * 1000 + j * 20000] = cnt;
                        }
                        else
                        {
                            cnt = j + 1;
                            ldxfacet[k + i * 1000 + j * 20000] = cnt;
                        }
                    }
                }
                
            }
        }
        //public void addxoffset(int xoffset)
        //{

        //    for (int i = 0; i < 160000; ++i)
        //    {
        //        ldx[i] += xoffset;

        //    }
        //    xmin += xoffset;
        //    xmax += xoffset;

        //}
        //public void addyoffset(int yoffset)
        //{
        //    for(int i=0; i<160000;++i)
        //    {
        //        ldy[i] += yoffset;
        //    }
        //    ymin += yoffset;
        //    ymax += yoffset;
        //}

    }
}
