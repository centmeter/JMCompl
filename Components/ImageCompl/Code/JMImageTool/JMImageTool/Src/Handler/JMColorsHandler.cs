using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JM.Image
{
    public class JMColorsHandler : IJMEffect
    {
        #region Variable

        /// <summary>
        /// 原始像素色数组
        /// </summary>
        private Color[,] _colors;

        #endregion

        #region Public Func

        public JMColorsHandler(Color[,] colors)
        {
            this._colors = colors;
        }

        public void Effect(JMImageEffect effect, string dstImagePath, ImageFormat imageFormat)
        {
            if (_colors != null)
            {
                Bitmap bitmap = null;

                try
                {
                    FileUtility.CreateDir(dstImagePath);

                    using (bitmap = new Bitmap(_colors.GetLength(0), _colors.GetLength(1)))
                    {
                        ConvertColors(effect, _colors, bitmap);

                        bitmap.Save(dstImagePath);
                    }
                }
                catch
                {
                    if (bitmap != null)
                    {
                        bitmap.Dispose();
                    }
                }
            }
        }

        public void Effect(JMImageEffect effect, ImageFormat imageFormat, Action<byte[]> callback = null)
        {
            byte[] datas = null;

            Bitmap bitmap = null;

            try
            {
                if (_colors != null)
                {
                    using (bitmap = new Bitmap(_colors.GetLength(0), _colors.GetLength(1)))
                    {
                        ConvertColors(effect, _colors, bitmap);

                        datas = BitmapConvert.ToBuffer(bitmap, imageFormat);
                    }
                }

            }
            catch
            {
                if (bitmap != null)
                {
                    bitmap.Dispose();
                }
            }

            if (callback != null)
            {
                callback.Invoke(datas);
            }
        }

        #endregion

        #region Private Func

        /// <summary>
        /// 转换Colors
        /// </summary>
        private void ConvertColors(JMImageEffect effect, Color[,] colors, Bitmap newBitmap)
        {
            switch (effect)
            {
                case JMImageEffect.Negative:
                    NegativeEffectColors(colors, newBitmap);
                    break;

                case JMImageEffect.Relief:
                    ReliefEffectColors(colors, newBitmap);
                    break;
            }
        }

        /// <summary>
        /// 底片Colos处理
        /// </summary>
        private void NegativeEffectColors(Color[,] colors, Bitmap newBitmap)
        {
            if (colors != null && newBitmap != null)
            {
                Color pixel;

                for (int x = 0; x < colors.GetLength(0); x++)
                {
                    for (int y = 0; y < colors.GetLength(1); y++)
                    {
                        pixel = colors[x, y];

                        Color resPixel = CoreUtility.NegativePixel(pixel);

                        newBitmap.SetPixel(x, y, resPixel);
                    }
                }
            }
        }

        /// <summary>
        /// 浮雕Colors处理
        /// </summary>
        private void ReliefEffectColors(Color[,] colors, Bitmap newBitmap)
        {
            if (colors != null && newBitmap != null)
            {
                for (int x = 0; x < colors.GetLength(0) - 1; x++)
                {
                    for (int y = 0; y < colors.GetLength(1) - 1; y++)
                    {
                        Color resPixel = CoreUtility.ReliefPixel(colors[x, y], colors[x + 1, y + 1]);

                        newBitmap.SetPixel(x, y, resPixel);
                    }
                }
            }
        }

        #endregion
    }
}
