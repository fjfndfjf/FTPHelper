using FTPHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (FTPMethods fTPHelper = new FTPMethods("ftp://127.0.0.1:10088/", "", ""))
            {
                fTPHelper.DeleteFile(@"test\test\", "1.txt");
                Console.WriteLine("FTP执行成功！");
                //var result = fTPHelper.FileExist(@"test\test\", "1.txt");
                //Console.WriteLine(result + "FTP执行成功！");

                //fTPHelper.DeleteFile("1.txt");
                //Console.WriteLine("FTP执行成功！");
                //var result = fTPHelper.FileExist("","1.txt");
                //Console.WriteLine(result + "FTP执行成功！");

                //fTPHelper.DeleteFile("1.txt");
                //Console.WriteLine("执行完成！");
            }
            Console.ReadKey();
        }
    }
}
