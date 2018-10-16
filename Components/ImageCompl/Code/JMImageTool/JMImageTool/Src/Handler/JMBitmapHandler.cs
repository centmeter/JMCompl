using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace JM.Image
{
    public class JMBitmapHandler : IJMEffect, IDisposable
    {
        #region Variable

        /// <summary>
        /// 原始Bitmap
        /// </summary>
        private Bitmap _originBitmap;

        #endregion

        #region Public Func

        public JMBitmapHandler(string filePath)
        {
            if (File.Exists(filePath))
            {
                _originBitmap = new Bitmap(filePath);
            }
        }

        public void Effect(JMImageEffect effect, string dstImagePath, ImageFormat imageFormat)
        {
            Bitmap bitmap = null;

            try
            {
                FileUtility.CreateDir(dstImagePath);

                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        ConvertBitmap(effect, _originBitmap, bitmap);

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

        public void Effect(JMImageEffect effect, ImageFormat imageFormat, Action<byte[]> callback = null)
        {
            Bitmap bitmap = null;

            byte[] datas = null;

            try
            {
                if (_originBitmap != null)
                {
                    using (bitmap = new Bitmap(_originBitmap.Width, _originBitmap.Height))
                    {
                        ConvertBitmap(effect, _originBitmap, bitmap);

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
        /// 转换Bitmap
        /// </summary>
        private void ConvertBitmap(JMImageEffect effect, Bitmap oldBitmap, Bitmap newBitmap)
        {
            switch (effect)
            {
                case JMImageEffect.Negative:
                    NegativeEffectBitmap(oldBitmap, newBitmap);
                    break;

                case JMImageEffect.Relief:
                    ReliefEffectBitmap(oldBitmap, newBitmap);
                    break;
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
                        pixel = oldBitmap.GetPixel(x, y);

                        Color resPixel = CoreUtility.NegativePixel(pixel);

                        newBitmap.SetPixel(x, y, resPixel);
                    }
                }
            }
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
                        pixel1 = oldBitmap.GetPixel(x, y);

                        pixel2 = oldBitmap.GetPixel(x + 1, y + 1);

                        Color resPixel = CoreUtility.ReliefPixel(pixel1, pixel2);

                        newBitmap.SetPixel(x, y, resPixel);
                    }
                }
            }
        }

        #endregion
    }
}

