using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;

namespace guimain
{
    public class imageconversion
    {
        public Image BufferToImage(byte[,] myMatrix)
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
                //Buffer.BlockCopy(pixels, 0, myMatrix, 0, pixels.Length);

                //create a new Bitmap
                Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

                //create grayscale palette

                pal.Entries[0] = Color.White;
                for (int i = 1; i < 256; i++)
                {
                    pal.Entries[i] = Color.Black;//all the values exclude 0 are white 
                }


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

        public static byte[] ImageToBuffer(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
    }
}
