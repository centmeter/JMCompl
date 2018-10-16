using System;
using System.Drawing;

namespace JM.Image
{
    internal static class CoreUtility
    {
        #region Public Func
        
        /// <summary>
        /// 底片效果处理像素
        /// </summary>
        public static Color NegativePixel(Color pixel)
        {
            int r = 255 - pixel.R;

            int g = 255 - pixel.G;

            int b = 255 - pixel.B;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 浮雕效果处理像素
        /// </summary>
        public static Color ReliefPixel(Color pixelPre, Color pixelAft)
        {
            int r = Math.Abs(pixelPre.R - pixelAft.R + 128);

            int g = Math.Abs(pixelPre.G - pixelAft.G + 128);

            int b = Math.Abs(pixelPre.B - pixelAft.B + 128);

            r = Clamp(r, 0, 255);

            g = Clamp(g, 0, 255);

            b = Clamp(b, 0, 255);

            return Color.FromArgb(r, g, b);
        }

        

        #endregion

        #region Private Func

        /// <summary>
        /// 返回限制范围的数值
        /// </summary>
        private static int Clamp(int value, int min, int max)
        {
            return value > max ? max : value < min ? min : value;
        }

        #endregion
    }
}
