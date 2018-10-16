using System;
using System.Drawing.Imaging;

namespace JM.Image
{
    public interface IJMEffect
    {
        /// <summary>
        /// 图片处理效果
        /// </summary>
        /// <param name="dstImagePath">保存路径</param>
        /// <param name="imageFormat">图片格式</param>
        void Effect(JMImageEffect effect, string dstImagePath, ImageFormat imageFormat);

        /// <summary>
        /// 图片处理效果
        /// </summary>
        /// <param name="imageFormat">图片格式</param>
        /// <param name="callback">图片字节数据回调</param>
        void Effect(JMImageEffect effect, ImageFormat imageFormat, Action<byte[]> callback = null);
    }

    /// <summary>
    /// 图片处理效果
    /// </summary>
    public enum JMImageEffect
    {
        /// <summary>
        /// 底片效果
        /// </summary>
        Negative,
        /// <summary>
        /// 浮雕效果
        /// </summary>
        Relief
    }
}
