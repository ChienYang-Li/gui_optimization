using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guimain
{
    public class interp
    {
        float lerp(float s, float e, float t) { return s + (e - s) * t; }
        float blerp(float c00, float c10, float c01, float c11, float tx, float ty)
        {
            return lerp(lerp(c00, c10, tx), lerp(c01, c11, tx), ty);
        }

        public float[,] linearscale(float[,] src, int w, int h, float scalex, float scaley)
        {

            int newWidth = (int)(w * scalex);//新的寬度
            int newHeight = (int)(h * scaley);//新的長度
            float[,] dst = new float[newHeight, newWidth];//新的矩陣
            int x, y;
            for (y = 0; y < newHeight; ++y)
            {
                for (x = 0; x < newWidth; ++x)
                {
                    float gx = x / (float)(newWidth) * (w - 1);
                    float gy = y / (float)(newHeight) * (h - 1);
                    int gxi = (int)gx;
                    int gyi = (int)gy;

                    float c00 = src[gyi, gxi];
                    float c10 = src[gyi, gxi + 1];
                    float c01 = src[gyi + 1, gxi];
                    float c11 = src[gyi + 1, gxi + 1];

                    //((uint8_t*)&result)[i] = blerp( ((uint8_t*)&c00)[i], ((uint8_t*)&c10)[i], ((uint8_t*)&c01)[i], ((uint8_t*)&c11)[i], gxi - gx, gyi - gy); // this is shady
                    dst[y, x] = blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                }

            }
            return dst;
        }
        public float[,] linearscale(byte[,] src, int w, int h, float scalex, float scaley)
        {
            int newWidth = (int)(w * scalex + 0.5);
            int newHeight = (int)(h * scaley + 0.5);
            //if (newWidth % 2 == 0)
            //{
            //    newWidth += 1;
            //}
            //if (newHeight % 2 == 0)
            //{
            //    newHeight += 1;
            //}

            float[,] dst = new float[newHeight, newWidth];
            if (w == 1 && h == 1)
            {

                int x, y;
                for (y = 0; y < newHeight; ++y)
                {
                    for (x = 0; x < newWidth; ++x)
                    {

                        dst[y, x] = src[0, 0];
                    }

                }
            }
            else
            {

                int x, y;
                for (y = 0; y < newHeight; ++y)
                {
                    for (x = 0; x < newWidth; ++x)
                    {
                        float gx = x / scalex;//(float)(newWidth) * (w - 1);
                        float gy = y / scaley;//(float)(newHeight) * (h - 1);
                        int gxi = (int)gx;
                        int gyi = (int)gy;

                        float c00 = src[gyi, gxi];
                        float c10 = src[gyi, gxi + 1];
                        float c01 = src[gyi + 1, gxi];
                        float c11 = src[gyi + 1, gxi + 1];

                        //((uint8_t*)&result)[i] = blerp( ((uint8_t*)&c00)[i], ((uint8_t*)&c10)[i], ((uint8_t*)&c01)[i], ((uint8_t*)&c11)[i], gxi - gx, gyi - gy); // this is shady
                        dst[y, x] = blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                    }
                }

                //for (int i = 0; i < src.GetLength(0); ++i)
                //{
                //    for (int j = 0; j < src.GetLength(1); ++j)
                //    {
                //        dst[(int)(i * scaley + 0.5), (int)(j * scalex + 0.5)] = src[i, j];
                //    }
                //}

                //float[,] srccopy = new float[src.GetLength(0) + 4, src.GetLength(1) + 4];
                //for (int i = 0; i < src.GetLength(0); ++i)
                //{
                //    for (int j = 0; j < src.GetLength(1); ++j)
                //    {
                //        srccopy[i + 2, j + 2] = src[i, j];
                //    }
                //}
                //for (int i = 0; i < srccopy.GetLength(0); ++i)
                //{
                //    srccopy[i, 0] = srccopy[i, 2] * 0.9f;
                //    srccopy[i, 1] = srccopy[i, 2] * 0.9f;
                //    srccopy[i, srccopy.GetLength(1) - 1] = srccopy[i, srccopy.GetLength(1) - 3] * 0.9f;
                //    srccopy[i, srccopy.GetLength(1) - 2] = srccopy[i, srccopy.GetLength(1) - 3] * 0.9f;
                //}
                //for (int i = 0; i < srccopy.GetLength(1); ++i)
                //{
                //    srccopy[0, i] = srccopy[2, i] * 0.9f;
                //    srccopy[1, i] = srccopy[2, i] * 0.9f;
                //    srccopy[srccopy.GetLength(0) - 1, i] = srccopy[srccopy.GetLength(0) - 3, i] * 0.9f;
                //    srccopy[srccopy.GetLength(0) - 2, i] = srccopy[srccopy.GetLength(0) - 3, i] * 0.9f;
                //}

                //int x, y;
                //for (y = 0; y < newHeight; ++y)
                //{
                //    for (x = 0; x < newWidth; ++x)
                //    {
                //        float yidx = y / scaley, xidx = x / scalex;
                //        //if (Math.Round(y / scaley) - (src.GetLength(0) - 1) >=1)
                //        //    yidx -= 1;
                //        //if (Math.Round(x / scalex) - (src.GetLength(1) - 1) >= 1)
                //        //    xidx -= 1;
                //        dst[y, x] = getValue(srccopy, yidx + 2, xidx + 2);
                //    }
                //}

                //if (scalex < 1 || scaley < 1)
                //{
                //    int offsetx = (int)(scalex / 2 + 0.5), offsety = (int)(scaley / 2 + 0.5);
                //    for (int i = 0; i < src.GetLength(0); ++i)
                //    {
                //        for (int j = 0; j < src.GetLength(1); ++j)
                //        {
                //            dst[(int)(i * scaley) + 1, (int)(j * scalex) + 1] = src[i, j];

                //        }
                //    }
                //}
                //dst[0, 0] = src[0, 0] * 0.9999f;
            }

            return dst;
        }
        public byte[,] linearscalebyte(byte[,] src, int w, int h, float scalex, float scaley)
        {
            int newWidth = (int)(w * scalex + 0.5);
            int newHeight = (int)(h * scaley + 0.5);
            //if (newWidth % 2 == 0)
            //{
            //    newWidth += 1;
            //}
            //if (newHeight % 2 == 0)
            //{
            //    newHeight += 1;
            //}

            float[,] dst = new float[newHeight, newWidth];
            byte[,] dstbyte = new byte[newHeight, newWidth];
            if (w == 1 && h == 1)
            {

                int x, y;
                for (y = 0; y < newHeight; ++y)
                {
                    for (x = 0; x < newWidth; ++x)
                    {

                        dst[y, x] = src[0, 0];
                    }

                }
            }
            else
            {

                int x, y;
                for (y = 0; y < newHeight; ++y)
                {
                    for (x = 0; x < newWidth; ++x)
                    {
                        float gx = x / scalex;//(float)(newWidth) * (w - 1);
                        float gy = y / scaley;//(float)(newHeight) * (h - 1);
                        int gxi = (int)gx;
                        int gyi = (int)gy;

                        float c00 = src[gyi, gxi];
                        float c10 = src[gyi, gxi + 1];
                        float c01 = src[gyi + 1, gxi];
                        float c11 = src[gyi + 1, gxi + 1];

                        //((uint8_t*)&result)[i] = blerp( ((uint8_t*)&c00)[i], ((uint8_t*)&c10)[i], ((uint8_t*)&c01)[i], ((uint8_t*)&c11)[i], gxi - gx, gyi - gy); // this is shady
                        dst[y, x] = blerp(c00, c10, c01, c11, gx - gxi, gy - gyi);
                    }
                }

                //for (int i = 0; i < src.GetLength(0); ++i)
                //{
                //    for (int j = 0; j < src.GetLength(1); ++j)
                //    {
                //        dst[(int)(i * scaley + 0.5), (int)(j * scalex + 0.5)] = src[i, j];
                //    }
                //}

                //float[,] srccopy = new float[src.GetLength(0) + 4, src.GetLength(1) + 4];
                //for (int i = 0; i < src.GetLength(0); ++i)
                //{
                //    for (int j = 0; j < src.GetLength(1); ++j)
                //    {
                //        srccopy[i + 2, j + 2] = src[i, j];
                //    }
                //}
                //for (int i = 0; i < srccopy.GetLength(0); ++i)
                //{
                //    srccopy[i, 0] = srccopy[i, 2] * 0.9f;
                //    srccopy[i, 1] = srccopy[i, 2] * 0.9f;
                //    srccopy[i, srccopy.GetLength(1) - 1] = srccopy[i, srccopy.GetLength(1) - 3] * 0.9f;
                //    srccopy[i, srccopy.GetLength(1) - 2] = srccopy[i, srccopy.GetLength(1) - 3] * 0.9f;
                //}
                //for (int i = 0; i < srccopy.GetLength(1); ++i)
                //{
                //    srccopy[0, i] = srccopy[2, i] * 0.9f;
                //    srccopy[1, i] = srccopy[2, i] * 0.9f;
                //    srccopy[srccopy.GetLength(0) - 1, i] = srccopy[srccopy.GetLength(0) - 3, i] * 0.9f;
                //    srccopy[srccopy.GetLength(0) - 2, i] = srccopy[srccopy.GetLength(0) - 3, i] * 0.9f;
                //}

                //int x, y;
                //for (y = 0; y < newHeight; ++y)
                //{
                //    for (x = 0; x < newWidth; ++x)
                //    {
                //        float yidx = y / scaley, xidx = x / scalex;
                //        //if (Math.Round(y / scaley) - (src.GetLength(0) - 1) >=1)
                //        //    yidx -= 1;
                //        //if (Math.Round(x / scalex) - (src.GetLength(1) - 1) >= 1)
                //        //    xidx -= 1;
                //        dst[y, x] = getValue(srccopy, yidx + 2, xidx + 2);
                //    }
                //}

                //if (scalex < 1 || scaley < 1)
                //{
                //    int offsetx = (int)(scalex / 2 + 0.5), offsety = (int)(scaley / 2 + 0.5);
                //    for (int i = 0; i < src.GetLength(0); ++i)
                //    {
                //        for (int j = 0; j < src.GetLength(1); ++j)
                //        {
                //            dst[(int)(i * scaley) + 1, (int)(j * scalex) + 1] = src[i, j];

                //        }
                //    }
                //}
                //dst[0, 0] = src[0, 0] * 0.9999f;
            }
            for (int i = 0; i < dstbyte.GetLength(0); ++i)
            {
                for (int j = 0; j < dstbyte.GetLength(1); ++j)
                {
                    if (dst[i, j] >= 0.5)
                        dstbyte[i, j] = 1;
                }
            }
            return dstbyte;
        }
        public byte[,] nearestscale(byte[,] src, int w, int h, float scalex, float scaley)//2值圖用
        {
            int x1, y1, x2, y2;
            int newh = (int)(h * scaley + 0.5);
            int neww = (int)(w * scalex + 0.5);
            byte[,] dst = new byte[newh, neww];

            for (y2 = 0; y2 < newh; y2++)
            {
                for (x2 = 0; x2 < neww; x2++)
                {
                    x1 = (int)(x2 / scalex);
                    y1 = (int)(y2 / scaley);
                    dst[y2, x2] = src[y1, x1];
                }
            }

            return dst;
        }
    }
}
