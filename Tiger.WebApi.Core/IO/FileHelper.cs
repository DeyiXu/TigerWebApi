using System.IO;
using System.Text.RegularExpressions;

namespace Tiger.WebApi.Core.IO
{
    public sealed class FileHelper
    {
        /// <summary>
        /// 判断文件是不是文件夹
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsDirectory(string fileName)
        {
            FileInfo fi = new FileInfo(fileName);

            return fi.Attributes == FileAttributes.Directory;
        }
    }
}
