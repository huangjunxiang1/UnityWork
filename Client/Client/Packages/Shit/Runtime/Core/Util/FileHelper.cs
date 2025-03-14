using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public static class FileHelper
    {
        public static void SyncDirectories(string sourceDir, string destinationDir, string searchPattern = "*", string exclude = null)
        {
            // 确保目标目录存在
            Directory.CreateDirectory(destinationDir);

            // 获取源目录和目标目录的文件和子目录
            var sourceFiles = Directory.GetFiles(sourceDir, searchPattern);
            var destinationFiles = Directory.GetFiles(destinationDir, searchPattern);
            var sourceDirectories = Directory.GetDirectories(sourceDir, searchPattern);
            var destinationDirectories = Directory.GetDirectories(destinationDir, searchPattern);

            // 同步文件
            foreach (var file in sourceFiles)
            {
                var relativePath = file.Substring(sourceDir.Length);
                var destinationFile = destinationDir + relativePath;
                var destinationFolder = Path.GetDirectoryName(destinationFile);

                // 确保目标文件夹存在
                Directory.CreateDirectory(destinationFolder);
                File.Copy(file, destinationFile, true);
            }

            // 删除目标目录中多余的文件
            foreach (var file in destinationFiles)
            {
                if (!string.IsNullOrEmpty(exclude))
                {
                    if (file.EndsWith(exclude))
                        continue;
                }
                var relativePath = file.Substring(destinationDir.Length);
                var sourceFile = sourceDir + "/" + relativePath;

                if (!File.Exists(sourceFile))
                {
                    File.Delete(file);
                }
            }

            // 同步文件夹
            foreach (var directory in sourceDirectories)
            {
                var relativePath = directory.Substring(sourceDir.Length);
                var destinationDirectory = destinationDir + "/" + relativePath;

                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }
            }

            // 删除目标目录中多余的子目录
            foreach (var directory in destinationDirectories)
            {
                var relativePath = directory.Substring(destinationDir.Length);
                var sourceDirectory = sourceDir + "/" + relativePath;

                if (!Directory.Exists(sourceDirectory))
                {
                    Directory.Delete(directory, true);
                }
            }

            // 递归同步子目录
            foreach (var subDir in sourceDirectories)
            {
                var relativePath = subDir.Substring(sourceDir.Length);
                var destinationSubDir = destinationDir + "/" + relativePath;

                // 递归调用同步子目录
                SyncDirectories(subDir, destinationSubDir, searchPattern);
            }
        }
        public static string ToAssetsDataPath(this string path)
        {
            var ps = path.Split("Assets");
            if (ps.Length != 2)
            {
                Loger.Error("非AssetsPath");
                return path;
            }
            return "Assets" + ps[1];
        }
    }
}
