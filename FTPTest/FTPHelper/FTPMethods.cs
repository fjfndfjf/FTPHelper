using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FTPHelper
{
    public class FTPMethods:IDisposable
    {
        public FTPMethods(string requestUriString, string userName, string password)
        {
            this.requestUriString = requestUriString;
            this.userName = userName;
            this.password = password;
        }

        private string requestUriString;
        private string userName;
        private string password;
        private FtpWebRequest request;
        private FtpWebResponse response;

        /// <summary>
        /// 创建FTPWebRequest
        /// </summary>
        /// <param name="uriString">FTP路径</param>
        /// <param name="method">FTP方法</param>
        private void OpenRequest(string uriString, string method)
        {
            request = (FtpWebRequest)WebRequest.Create(new Uri(this.requestUriString + uriString));
            request.Credentials = new NetworkCredential(userName, password);
            request.UseBinary = true;
            request.Method = method;
        }

        /// <summary>
        /// 返回FTPWebResponse
        /// </summary>
        /// <param name="uriString">FTP路径</param>
        /// <param name="method">FTP方法</param>
        private void OpenResponse(string uriString, string method)
        {
            OpenRequest(uriString, method);
            response = request.GetResponse() as FtpWebResponse;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="directoryPath">要创建的目录名称</param>
        public void MakeDirectory(string directoryPath)
        {
            OpenResponse(directoryPath, WebRequestMethods.Ftp.MakeDirectory);
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="directoryPath"></param>
        public void RemoveDirectory(string directoryPath)
        {
            OpenResponse(directoryPath, WebRequestMethods.Ftp.RemoveDirectory);
        }

        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="originalFileName"></param>
        /// <param name="newFileName"></param>
        public void Rename(string originalFileName, string newFileName)
        {
            OpenResponse(originalFileName, WebRequestMethods.Ftp.Rename);
            request.RenameTo = newFileName;
            response = request.GetResponse() as FtpWebResponse;
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="localFullPath">要上传文件的全路径</param>
        public void UploadFile(string localFullPath)
        {
            UploadFile(localFullPath, false);
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="localFullPath">要上传的全路径</param>
        /// <param name="overWriteFile">是否重写服务器上的文件</param>
        public void UploadFile(string localFullPath, bool overWriteFile)
        {
            UploadFile(localFullPath, Path.GetFileName(localFullPath), overWriteFile);
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="localFullPath"></param>
        /// <param name="remoteFillName">上传文件的重命名</param>
        public void UploadFile(string localFullPath, string remoteFillName)
        {
            UploadFile(localFullPath, remoteFillName, false);
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="localFullPath">要伤处的本地文件的全路径</param>
        /// <param name="remoteFileName">上传后的文件重命名</param>
        /// <param name="overWriteFile">是否重写文件的重命名</param>
        public void UploadFile(string localFullPath, string remoteFileName, bool overWriteFile)
        {
            byte[] fileBytes = null;
            using (FileStream fileStream = new FileStream(localFullPath, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, (Int32)fileStream.Length);
            }
            UploadFile(fileBytes, remoteFileName, overWriteFile);

        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="fileBytes">上传文件的字节流</param>
        /// <param name="remoteFileName">上传后文件重命名为</param>
        public void UploadFile(byte[] fileBytes, string remoteFileName)
        {
            UploadFile(fileBytes, remoteFileName, false);
        }

        /// <summary>
        /// 上传文件到服务器
        /// </summary>
        /// <param name="fileBytes">上传文件的字节流</param>
        /// <param name="remoteFileName">上传文件后的重命名</param>
        /// <param name="overWriteFile">是否重写服务器上的文件</param>
        public void UploadFile(byte[] fileBytes, string remoteFileName, bool overWriteFile)
        {
            OpenResponse(overWriteFile ? remoteFileName : Gadget.ReturnFileNameWithCurrentDate(remoteFileName), WebRequestMethods.Ftp.UploadFile);
            using (Stream stream = request.GetRequestStream())
            {
                using (MemoryStream memoryStream = new MemoryStream(fileBytes))
                {
                    byte[] buffer = new byte[Constant.FTP.LenOfBuffer];
                    int bytesRead = 0;
                    int totalRead = 0;
                    while (true)
                    {
                        bytesRead = memoryStream.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        totalRead += bytesRead;
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
                response = request.GetResponse() as FtpWebResponse;
            }
        }

        /// <summary>
        /// 下载服务器文件到本地
        /// </summary>
        /// <param name="remoteFileName">要下载的服务器上的文件名</param>
        /// <param name="localPath">下载到本地的路径</param>
        public void DownloadFile(string remoteFileName, string localPath)
        {
            DownloadFile(remoteFileName, localPath, false);
        }

        /// <summary>
        /// 下载服务器文件到本地
        /// </summary>
        /// <param name="remoteFileName">要下载的服务器上的文件名</param>
        /// <param name="localPath">下载到本地的路径</param>
        /// <param name="overWriteFile">是否重写本地的文件</param>
        public void DownloadFile(string remoteFileName, string localPath, bool overWriteFile)
        {
            DownloadFile(remoteFileName, localPath, remoteFileName, overWriteFile);
        }

        /// <summary>
        /// 下载服务器文件到本地
        /// </summary>
        /// <param name="remoteFileName">要下载的服务器上的文件名</param>
        /// <param name="localPath">下载到本地的路径</param>
        /// <param name="localFileName">下载到本地的文件名</param>
        public void DownloadFile(string remoteFileName, string localPath, string localFileName)
        {
            DownloadFile(remoteFileName, localPath, localFileName, false);
        }

        /// <summary>
        /// 下载服务器文件到本地
        /// </summary>
        /// <param name="remoteFileName">要下载的服务器上的文件名</param>
        /// <param name="localPath">下载到本地的路径</param>
        /// <param name="localFileName">下载到本地的文件的重命名</param>
        /// <param name="overWriteFile">是否要重写本地的文件</param>
        public void DownloadFile(string remoteFileName, string localPath, string localFileName, bool overWriteFile)
        {
            byte[] fileBytes = DownloadFile(remoteFileName);
            if (fileBytes != null)
            {
                using (FileStream fileStream = new FileStream(Path.Combine(localPath, overWriteFile ? localFileName : Gadget.ReturnFileNameWithCurrentDate(localFileName)), FileMode.Create))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                    fileStream.Flush();
                }
            }
        }

        /// <summary>
        /// 下载服务器文件到本地
        /// </summary>
        /// <param name="remoteFileName">要下载的服务器文件名</param>
        /// <returns>文件的字节流</returns>
        public byte[] DownloadFile(string remoteFileName)
        {
            OpenResponse(remoteFileName, WebRequestMethods.Ftp.DownloadFile);
            using (Stream stream = response.GetResponseStream())
            {
                using (MemoryStream memoryStream = new MemoryStream(Constant.FTP.LenOfBuffer))
                {
                    byte[] buffer = new byte[Constant.FTP.LenOfBuffer];
                    int bytesRead = 0;
                    int bytesTotal = 0;
                    while (true)
                    {
                        bytesRead = stream.Read(buffer, 0, Constant.FTP.LenOfBuffer);
                        bytesTotal += bytesRead;
                        if (bytesRead == 0)
                            break;
                        memoryStream.Write(buffer, 0, bytesRead);
                    }
                    return memoryStream.Length > 0 ? memoryStream.ToArray() : null;
                }
            }
        }

        /// <summary>
        /// 删除服务器文件
        /// </summary>
        /// <param name="directoryPath">要删除的文件路径</param>
        /// <param name="fileName">要删除的文件名</param>
        public void DeleteFile(string directoryPath, string fileName)
        {
            DeleteFile(directoryPath + fileName);
        }

        /// <summary>
        /// 删除服务器文件
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        public void DeleteFile(string fileName)
        {
            
            OpenResponse(fileName, WebRequestMethods.Ftp.DeleteFile);
        }

        /// <summary>
        /// 列出服务服务器上指定目录下的所有文件
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public string[] ListFiles(string directoryPath)
        {
            List<string> filesList = new List<string>();
            OpenResponse(directoryPath, WebRequestMethods.Ftp.ListDirectory);
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                return Gadget.SplitString(streamReader.ReadToEnd(), Constant.TextConstant.FtpNewLine);
            }
        }

        /// <summary>
        /// 列出服务器上指定目录下的所有子目录
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public string[] ListDirectorys(string directoryPath)
        {
            List<string> directorysList = new List<string>();
            OpenResponse(directoryPath, WebRequestMethods.Ftp.ListDirectory);
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
                string line = streamReader.ReadLine();
                if (line != null)
                {
                    while (line != null)
                    {
                        line = line.Substring(line.LastIndexOf(Constant.TextConstant.Colon) + Constant.FTP.LenToDirectory);
                        if (!line.StartsWith(Constant.FileConstant.FileTypeSeperator))
                            directorysList.Add(line);
                        line = streamReader.ReadLine();
                    }
                }
                return directorysList.ToArray();
            }
        }

        /// <summary>
        /// 检查指定的目录是否在服务器上存在
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public bool DirectoryExist(string directory)
        {
            try
            {
                OpenResponse(directory, WebRequestMethods.Ftp.GetDateTimestamp);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 查询服务器是否存在此文件
        /// </summary>
        /// <param name="directoryPath">指定的目录</param>
        /// <param name="remoteFileName">指定的文件名</param>
        /// <returns></returns>
        public bool FileExist(string directoryPath, string remoteFileName)
        {
            string[] fileNames = ListFiles(directoryPath);
            foreach (var fileName in fileNames)
            {
                if (String.Compare(fileName, remoteFileName, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("及时释放资源");
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Close()
        {
            if (this.response != null)
            {
                this.response.Close();
                this.response = null;
            }
        }
    }
}
