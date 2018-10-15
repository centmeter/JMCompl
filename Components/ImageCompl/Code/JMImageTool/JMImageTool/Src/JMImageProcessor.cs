using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace JM.Image
{
    public class JMImageProcessor : IDisposable
    {
        #region Variable

        /// <summary>
        /// 原始Bitmap
        /// </summary>
        private Bitmap _originBitmap;

        #endregion

        #region Public Func

        public JMImageProcessor(string filePath)
        {
            if (File.Exists(filePath))
            {
                _originBitmap = new Bitmap(filePath);
            }
        }
        
        /// <summary>
        /// 底片效果
        /// </summary>
        public void NegativeEffect(string dstImagePath, ImageFormat imageFormat)
        {
            Bitmap bitmap = null;

            try
            {
                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        NegativeEffectBitmap(_originBitmap, bitmap);

                        bitmap.Save(dstImagePath, imageFormat);
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
        }

        /// <summary>
        /// 底片效果
        /// </summary>
        public void NegativeEffect(ImageFormat imageFormat, Action<byte[]> callback = null)
        {
            Bitmap bitmap = null;

            byte[] datas = null;

            try
            {
                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        NegativeEffectBitmap(_originBitmap, bitmap);

                        datas = BitmapToBuffer(bitmap, imageFormat);
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
        
        /// <summary>
        /// 浮雕效果
        /// </summary>
        public void ReliefEffect(string dstImagePath, ImageFormat imageFormat)
        {
            Bitmap bitmap = null;

            try
            {
                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        ReliefEffectBitmap(_originBitmap, bitmap);

                        bitmap.Save(dstImagePath, imageFormat);
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
        }

        /// <summary>
        /// 浮雕效果
        /// </summary>
        public void ReliefEffect(ImageFormat imageFormat, Action<byte[]> callback = null)
        {
            byte[] datas = null;

            Bitmap bitmap = null;

            try
            {
                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        ReliefEffectBitmap(_originBitmap, bitmap);

                        datas = BitmapToBuffer(bitmap, imageFormat);
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

        public void Dispose()
        {
            if (_originBitmap != null)
            {
                _originBitmap.Dispose();

                _originBitmap = null;
            }
        }

        #endregion

        #region Private Func

        /// <summary>
        /// 返回限制范围的数值
        /// </summary>
        private int Clamp(int value, int min, int max)
        {
            return value > max ? max : value < min ? min : value;
        }

        /// <summary>
        /// 浮雕Bitmap处理
        /// </summary>
        private void ReliefEffectBitmap(Bitmap oldBitmap, Bitmap newBitmap)
        {
            if (oldBitmap != null && newBitmap != null)
            {
                Color pixel1;

                Color pixel2;

                for (int x = 0; x < oldBitmap.Width - 1; x++)
                {
                    for (int y = 0; y < oldBitmap.Height - 1; y++)
                    {
                        int r = 0, g = 0, b = 0;

                        pixel1 = oldBitmap.GetPixel(x, y);

                        pixel2 = oldBitmap.GetPixel(x + 1, y + 1);

                        r = Math.Abs(pixel1.R - pixel2.R + 128);

                        g = Math.Abs(pixel1.G - pixel2.G + 128);

                        b = Math.Abs(pixel1.B - pixel2.B + 128);

                        r = Clamp(r, 0, 255);

                        g = Clamp(g, 0, 255);

                        b = Clamp(b, 0, 255);

                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
        }

        /// <summary>
        /// 底片Bitmap处理
        /// </summary>
        private void NegativeEffectBitmap(Bitmap oldBitmap, Bitmap newBitmap)
        {
            if (oldBitmap != null && newBitmap != null)
            {
                Color pixel;

                for (int x = 0; x < oldBitmap.Width; x++)
                {
                    for (int y = 0; y < oldBitmap.Height; y++)
                    {
                        int r, g, b;

                        pixel = _originBitmap.GetPixel(x, y);

                        r = 255 - pixel.R;

                        g = 255 - pixel.G;

                        b = 255 - pixel.B;

                        newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                }
            }
        }

        /// <summary>
        /// bitmap转byte数组
        /// </summary>
        private byte[] BitmapToBuffer(Bitmap bitmap, ImageFormat imageFormat)
        {
            byte[] res = null;

            if (bitmap != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, imageFormat);

                    res = stream.GetBuffer();

                    stream.Close();
                }
            }

            return res;
        }
        #endregion
    }
}

