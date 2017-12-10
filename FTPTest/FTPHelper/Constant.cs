using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPHelper
{
    class Constant
    {
        public class TextConstant
        {
            public static readonly string Tab = "\t";
            public static readonly string NewLine = "\n";
            public static readonly string UnderLine = "_";
            public static readonly string FtpNewLine = "\r\n";
            public static readonly string Colon = ":";
        }
        public class HtmlConstant
        {
            public static readonly string NewLine = "<br />";
        }
        public class FileConstant
        {
            public static readonly string FileTypeForTXT = ".txt";
            public static readonly string FileTypeForExcel = ".xls";
            public static readonly string FileTypeForZip = ".zip";
            public static readonly string FileTypeSeperator = ".";
            public static readonly string FileFullMatchSymbol = "*.*";
            public static readonly string FolderSeperator = @"\";
        }
        public class DateTimeFormate
        {
            public static readonly string YYMMDDHHMMSS = "yyMMddHHmmss";
        }

        public class FTP
        {
            public static readonly int LenToDirectory = 4;
            public static readonly int LenOfBuffer = 1024;
            public static readonly int CapacityofMemeoryStream = 1024 * 500;
        }
    }
}
