using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace guimain
{
    public delegate int Scale_del(float x);
    class scaleclass
    {
        int ptbw;
        int ptbmax;
        float ratio;
        int magnifyratiowidth;
        int ptbw40082;
        int pos0;
        int posratio0;
        //(int)Math.Round((ptbw* ((double)(magnifyratiowidth - ptbw) / (ptbmax - ptbw))) / rationew);
        public scaleclass(int ptbw, int ptbmax, float ratio, int magnifyratiowidth, int pos0, int ptbw40082, int posratio0)
        {
            this.ptbw = ptbw;
            this.ptbmax = ptbmax;
            this.ratio = ratio;
            this.magnifyratiowidth = magnifyratiowidth;
            this.ptbw40082 = ptbw40082;
            this.pos0 = pos0;
            this.posratio0 = posratio0;

        }

        public int scalenot50000(float x)
        {
            return (int)Math.Round(x * ratio, MidpointRounding.AwayFromZero);
        }

        public int scale50000(float x)
        {
            return (int)Math.Round(((x * ratio) * ((double)(ptbmax - ptbw) / (magnifyratiowidth - ptbw))), MidpointRounding.AwayFromZero);
        }

        public int scaleptbmax(float x)
        {
            float x1 = x - pos0; //任一LD在pos0和pos1中的x位置
            float xr = x1 / ptbw40082;//計算放大比例
            int scalex = (int)Math.Round(ptbw * xr, MidpointRounding.AwayFromZero);
            return (posratio0 + scalex);//Max座標系
        }
       

    }

}
