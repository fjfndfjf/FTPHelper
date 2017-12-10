using System;
using System.IO;

namespace FTPTest
{
    class Gadget
    {
        public static string ReturnFileNameWithCurrentDate(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName) + Constant.TextConstant.UnderLine + DateTime.Now.ToString(Constant.DateTimeFormate.YYMMDDHHMMSS) + Constant.FileConstant.FileTypeSeperator + Path.GetExtension(fileName);
        }

        public static string[] SplitString(string str,string split)
        {
            return str.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
