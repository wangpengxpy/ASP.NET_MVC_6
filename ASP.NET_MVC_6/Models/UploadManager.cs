using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Hosting;

namespace ASP.NET_MVC_6.Models
{
    public static class UploadManager
    {
        #region 属性

        private const string FilesSubdir = "attachments";
        private const string TempExtension = "_temp";

        /// <summary>
        /// 上传到服务器的物理路径
        /// </summary>
        public static string UploadFolderPhysicalPath { get; set; }

        /// <summary>
        /// 上传到服务器的相对路径
        /// </summary>
        public static string UploadFolderRelativePath { get; set; }


        #endregion

        #region Ctor

        static UploadManager()
        {
            //从配置文件中获取上传文件夹
            if (String.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings["UploadFolder"]))
                UploadFolderRelativePath = @"~/upload";
            else
                UploadFolderRelativePath = WebConfigurationManager.AppSettings["UploadFolder"];

            UploadFolderPhysicalPath = HostingEnvironment.MapPath(UploadFolderRelativePath);

            if (!Directory.Exists(UploadFolderPhysicalPath))
                Directory.CreateDirectory(UploadFolderPhysicalPath);
        }

        #endregion

        #region 保存文件

        [SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public static bool SaveFile(Stream stream, string fileName, string userName, string guid)
        {
            string tempPath = string.Empty, targetPath = string.Empty;
            long fileLength = 0;

            try
            {
                string tempFileName = GetTempFilePath(fileName);

                if (userName != null)
                {
                    var contentType = userName;
                    var contentId = guid;

                    tempPath = GetTempFilePath(tempFileName);
                    targetPath = GetTargetFilePath(fileName, contentType, contentId, string.Empty, FilesSubdir);


                    // 若上传文件夹未存在则创建
                    var file = new FileInfo(targetPath);
                    if (file.Directory != null && !file.Directory.Exists)
                        file.Directory.Create();

                    using (FileStream fs = File.Open(tempPath, FileMode.Append))
                    {
                        if (stream.Length > 0)
                        {
                            fileLength = stream.Length;
                            SaveFile(stream, fs);
                        }
                        fs.Close();
                    }
                    //上传完毕将临时文件移动到目标文件
                    File.Move(tempPath, targetPath);
                }
            }
            catch (Exception)
            {
                // 若上传出错，则删除上传到文件夹文件
                if (File.Exists(targetPath))
                    File.Delete(targetPath);

                // 删除临时文件
                if (File.Exists(tempPath))
                    File.Delete(tempPath);

                return false;
            }
            finally
            {
                // 删除临时文件
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
            return true;
        }

        /// <summary>
        /// 读取流到文件中
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="fs"></param>
        public static void SaveFile(Stream stream, FileStream fs)
        {
            var buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                fs.Write(buffer, 0, bytesRead);
            }
        }

        #endregion

        #region 获取临时文件夹路径

        public static string GetTempFilePath(string fileName)
        {
            fileName = fileName + TempExtension;
            return Path.Combine(@HostingEnvironment.ApplicationPhysicalPath, Path.Combine(UploadFolderPhysicalPath, fileName));
        }

        public static string GetTargetFilePath(string fileName, string contentType, string contentId, string culture = "",
                                               string optionalSubdir = "")
        {
            return Path.Combine(UploadFolderPhysicalPath, contentType, culture, contentId,
                                optionalSubdir, fileName);
        }

        #region 依据路径删除文件

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
                File.Delete(path);
        }

        #endregion

        #endregion
    }
}