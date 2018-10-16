using System.IO;

namespace JM.Image
{
    internal static class FileUtility
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        public static void CreateDir(string path)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }


    }
}
