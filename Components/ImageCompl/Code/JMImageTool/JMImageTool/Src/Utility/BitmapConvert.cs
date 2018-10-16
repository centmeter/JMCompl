using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace JM.Image
{
    internal static class BitmapConvert
    {
        /// <summary>
        /// bitmap转字节数组
        /// </summary>
        public static byte[] ToBuffer(Bitmap bitmap, ImageFormat imageFormat)
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
    }
}
