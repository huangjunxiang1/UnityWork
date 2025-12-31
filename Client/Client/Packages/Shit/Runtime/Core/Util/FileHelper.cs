using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Core
{
    public static class FileHelper
    {
        public static void SyncDirectories(string source, string target, Func<string, bool> copyFilter = null, Func<string, bool> deleteFilter = null)
        {
            // 确保源目录存在
            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException($"Source directory not found: {source}");
            // 确保目标目录存在
            if (!Directory.Exists(target))
                Directory.CreateDirectory(target);
            // 复制所有文件
            foreach (string sourceFilePath in Directory.GetFiles(source))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(target, fileName);
                if (copyFilter == null || copyFilter(sourceFilePath))
                    File.Copy(sourceFilePath, destFilePath, true); // 覆盖已存在的文件
            }
            // 递归复制子目录
            foreach (string sourceSubDir in Directory.GetDirectories(source))
            {
                string subDirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(target, subDirName);
                SyncDirectories(sourceSubDir, destSubDir, copyFilter, deleteFilter);
            }
            // 删除目标目录中多余的文件
            foreach (string destFilePath in Directory.GetFiles(target))
            {
                if (destFilePath.EndsWith(".meta"))
                    continue;
                string fileName = Path.GetFileName(destFilePath);
                string sourceFilePath = Path.Combine(source, fileName);
                if (!File.Exists(sourceFilePath))
                {
                    if (deleteFilter == null || deleteFilter(destFilePath))
                        File.Delete(destFilePath);
                }
            }
            // 删除目标目录中多余的子目录
            foreach (string destSubDir in Directory.GetDirectories(target))
            {
                string subDirName = Path.GetFileName(destSubDir);
                string sourceSubDir = Path.Combine(source, subDirName);
                if (!Directory.Exists(sourceSubDir))
                {
                    Directory.Delete(destSubDir, true); // 递归删除
                    if (File.Exists(destSubDir + ".meta"))
                        File.Delete(destSubDir + ".meta");
                }
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
        public static string ToFullPath(this string path)
        {
            if (path.StartsWith("Assets/"))
                return $"{Application.dataPath}/{path.Replace("Assets/", null)}";
            return path;
        }
    }
}
