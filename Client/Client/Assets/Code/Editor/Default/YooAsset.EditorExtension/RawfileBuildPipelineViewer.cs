using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using YooAsset.Editor;
using TMPro;

class Ex
{
    static void cull_res()
    {
        var dirs = Directory.GetDirectories($"{Application.dataPath}/../Bundles/");
        foreach (var dir in dirs)
        {
            foreach (var dir2 in Directory.GetDirectories(dir))
            {
                var ds = Directory.GetDirectories(dir2).Select(t => new DirectoryInfo(t)).Where(t => t.Name.Contains('.')).ToList();
                ds.Sort((x, y) =>
                {
                    Version v1 = null;
                    Version v2 = null;
                    try { v1 = new(x.Name); }
                    catch (Exception) { }
                    try { v2 = new(y.Name); }
                    catch (Exception) { }
                    if (v1 < v2) return -1;
                    if (v1 > v2) return 1;
                    return 0;
                });
                var src = ds.LastOrDefault();
                var cull = src.GetFiles().Select(t => t.Name);
                //File.Delete($"{src}/PackageManifest_{AssetBundleBuilderSettingData.Setting.BuildPackage}.version");
                HashSet<string> olds = new(100000);
                for (int i = 0; i < ds.Count - 1; i++)
                {
                    var target = ds[i];
                    foreach (var item in target.GetFiles())
                        olds.Add(item.Name);
                }
                foreach (var item in src.GetFiles())
                {
                    if (item.Name.EndsWith(".version"))
                        continue;
                    if (item.Name.EndsWith(".json") || item.Name.EndsWith(".xml") || item.Name.EndsWith(".report"))
                        item.Delete();
                    if (olds.Contains(item.Name))
                        item.Delete();
                }
            }
        }
        EditorUtility.DisplayDialog("提示", "剔除完毕", "确定", "取消");
    }

    [BuildPipelineAttribute(nameof(EBuildPipeline.RawFileBuildPipeline))]
    internal class Raw : RawfileBuildpipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget,this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            Button myButton = new Button();
            myButton.style.marginTop = 10;
            myButton.text = "剔除重复资源";
            myButton.style.height = 50;
            myButton.AddToClassList("my-button-style");
            myButton.clicked += cull_res;
            Root.Add(myButton);

            // 添加 FTP 上传面板
            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName);
        }
    }
    [BuildPipelineAttribute(nameof(EBuildPipeline.ScriptableBuildPipeline))]
    internal class Res : ScriptableBuildPipelineViewer
    {
        protected override string GetDefaultPackageVersion() => getVer(this.BuildTarget, this.PackageName);
        public override void CreateView(VisualElement parent)
        {
            base.CreateView(parent);

            Button myButton = new Button();
            myButton.style.marginTop = 10;
            myButton.text = "剔除重复资源";
            myButton.style.height = 50;
            myButton.AddToClassList("my-button-style");
            myButton.clicked += cull_res;
            Root.Add(myButton);

            Ex.AddFtpUploader(Root, this.BuildTarget, this.PackageName);
        }
        protected override void ExecuteBuild()
        {
            // 需要引入 TMPro 命名空间: using TMPro;
            string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(path);
                if (asset.atlasPopulationMode != AtlasPopulationMode.Static)
                    asset.ClearFontAssetData(true);
            }
            AssetDatabase.Refresh();
            base.ExecuteBuild();
        }
    }

    static string getVer(BuildTarget BuildTarget,string PackageName)
    {
        var output = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{BuildTarget}/{PackageName}";
        var min = new Version("1.0.0");
        if (!Directory.Exists(output))
            return min.ToString();
        var dirs = Directory.GetDirectories(output);
        var max = min;
        for (int i = 0; i < dirs.Length; i++)
        {
            try
            {
                var v = new Version(new DirectoryInfo(dirs[i]).Name);
                if (max < v)
                    max = v;
            }
            catch (Exception)
            {
                continue;
            }
        }
        return $"{max.Major}.{max.Minor}.{max.Build + 1}";
    }

    // ---------- FTP 上传相关辅助方法与 UI 组件 ----------
    static void AddFtpUploader(VisualElement root, BuildTarget buildTarget, string packageName)
    {
        // 容器
        var container = new VisualElement();
        container.style.marginTop = 10;
        container.style.flexDirection = FlexDirection.Column;
        container.style.paddingLeft = 4;
        container.style.paddingRight = 4;

        // 标题
        var title = new Label("FTP 上传");
        title.style.unityFontStyleAndWeight = FontStyle.Bold;
        title.style.marginTop = 6;
        container.Add(title);

        // EditorPrefs 键前缀（确保唯一）
        string keyPrefix = $"YooAsset.Ftp";
        string keyHost = keyPrefix + ".Host";
        string keyUser = keyPrefix + ".User";
        string keyPass = keyPrefix + ".Pass";
        string keyRemoteRoot = keyPrefix + ".RemoteRoot";

        // 输入：FTP 主机
        var ftpHostField = new TextField("FTP 主机（例如 ftp://example.com 或 example.com）");
        ftpHostField.value = EditorPrefs.GetString(keyHost, "");
        container.Add(ftpHostField);

        // 输入：用户名
        var userField = new TextField("用户名");
        userField.value = EditorPrefs.GetString(keyUser, "");
        container.Add(userField);

        // 输入：密码（密码域）
        var passField = new TextField("密码");
        passField.value = EditorPrefs.GetString(keyPass, "");
        container.Add(passField);

        // 输入：远程根路径
        var remotePathField = new TextField("远程根路径（可选）");
        remotePathField.value = EditorPrefs.GetString(keyRemoteRoot, "");
        container.Add(remotePathField);

        // 当任意字段变更时保存到 EditorPrefs（本地缓存）
        ftpHostField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyHost, evt.newValue ?? "");
        });
        userField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyUser, evt.newValue ?? "");
        });
        passField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyPass, evt.newValue ?? "");
        });
        remotePathField.RegisterValueChangedCallback(evt =>
        {
            EditorPrefs.SetString(keyRemoteRoot, evt.newValue ?? "");
        });

        // 进度条与状态
        var progressBar = new ProgressBar();
        progressBar.lowValue = 0f;
        progressBar.highValue = 1f;
        progressBar.value = 0f;
        progressBar.style.height = 18;
        progressBar.style.marginTop = 6;
        container.Add(progressBar);

        var statusLabel = new Label("就绪");
        statusLabel.style.marginTop = 4;
        container.Add(statusLabel);

        // 上传按钮
        var uploadBtn = new Button();
        uploadBtn.text = "上传到 FTP";
        uploadBtn.style.marginTop = 6;
        uploadBtn.style.height = 30;
        container.Add(uploadBtn);

        root.Add(container);

        // 本地函数：格式化字节大小
        static string FormatBytes(long bytes)
        {
            if (bytes < 1024) return $"{bytes} B";
            double kb = bytes / 1024.0;
            if (kb < 1024) return $"{kb:F2} KB";
            double mb = kb / 1024.0;
            if (mb < 1024) return $"{mb:F2} MB";
            double gb = mb / 1024.0;
            return $"{gb:F2} GB";
        }

        uploadBtn.clicked += () =>
        {
            // 点击前把当前值再次保存一次，防止回调遗漏
            EditorPrefs.SetString(keyHost, ftpHostField.value ?? "");
            EditorPrefs.SetString(keyUser, userField.value ?? "");
            EditorPrefs.SetString(keyPass, passField.value ?? "");
            EditorPrefs.SetString(keyRemoteRoot, remotePathField.value ?? "");

            uploadBtn.SetEnabled(false);
            progressBar.value = 0f;
            statusLabel.text = "准备上传...";

            string host = ftpHostField.value?.Trim() ?? "";
            string user = userField.value ?? "";
            string pass = passField.value ?? "";
            string remoteRoot = remotePathField.value?.Trim() ?? "";

            Task.Run(async () =>
            {
                try
                {
                    string localRoot = GetLatestOutputDir(buildTarget, packageName);
                    if (string.IsNullOrEmpty(localRoot) || !Directory.Exists(localRoot))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("错误", "未找到最新构建输出目录，无法上传。", "确定");
                            statusLabel.text = "未找到本地目录";
                            uploadBtn.SetEnabled(true);
                        };
                        return;
                    }

                    // 规范化 host
                    if (!host.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) &&
                        !host.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
                    {
                        host = "ftp://" + host;
                    }
                    // 移除末尾的斜杠
                    host = host.TrimEnd('/');

                    var files = Directory.GetFiles(localRoot, "*", SearchOption.AllDirectories);
                    long totalBytes = 0;
                    var fileInfos = new List<FileInfo>(files.Length);
                    foreach (var f in files)
                    {
                        var fi = new FileInfo(f);
                        fileInfos.Add(fi);
                        totalBytes += fi.Length;
                    }
                    long uploadedBytes = 0;

                    if (files.Length == 0)
                    {
                        EditorApplication.delayCall += () =>
                        {
                            EditorUtility.DisplayDialog("提示", "目录为空，无需上传。", "确定");
                            statusLabel.text = "目录为空";
                            uploadBtn.SetEnabled(true);
                        };
                        return;
                    }

                    // 忽略 SSL 证书验证（若使用 FTPS 并有自签名证书）
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    for (int i = 0; i < files.Length; i++)
                    {
                        var file = files[i];
                        var fi = fileInfos[i];
                        var relative = file.Substring(localRoot.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        string remoteFilePath = string.IsNullOrEmpty(remoteRoot) ? relative : $"{remoteRoot.TrimStart('/').TrimEnd('/')}/{relative.Replace('\\','/')}";
                        long fileLength = fi.Length;
                        long currentFileSent = 0;

                        // 先确保目录存在
                        var remoteDir = Path.GetDirectoryName(remoteFilePath)?.Replace('\\','/');
                        if (!string.IsNullOrEmpty(remoteDir))
                        {
                            EnsureFtpDirectoryExists(host, user, pass, remoteDir);
                        }

                        // 上传文件（分块），在回调中同时更新“当前文件进度”和“总进度”
                        string uri = $"{host}/{remoteFilePath}";
                        await UploadFileWithProgress(uri, user, pass, file, (sentBytes) =>
                        {
                            // 更新已上传字节（总和当前文件
                            currentFileSent += sentBytes;
                            uploadedBytes += sentBytes;

                            float totalProgress = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                            float currentFileProgress = fileLength > 0 ? Math.Min(1f, (float)currentFileSent / fileLength) : 1f;

                            EditorApplication.delayCall += () =>
                            {
                                progressBar.value = totalProgress;
                                // 显示总进度与当前文件进度，附带已上传字节/文件总字节的可读文本
                                statusLabel.text = $"总 { (int)(totalProgress * 100) }%  |  文件 { (int)(currentFileProgress * 100) }%  —  {relative} ({FormatBytes(currentFileSent)}/{FormatBytes(fileLength)})";
                            };
                        });

                        // 确保上传完一个文件后 UI 显示其完成状态（避免最后一块回调丢失 UI 更新
                        EditorApplication.delayCall += () =>
                        {
                            float totalProgressAfterFile = totalBytes > 0 ? Math.Min(1f, (float)uploadedBytes / totalBytes) : 1f;
                            progressBar.value = totalProgressAfterFile;
                            statusLabel.text = $"总 { (int)(totalProgressAfterFile * 100) }%  |  文件 100%  —  {relative} ({FormatBytes(fileLength)}/{FormatBytes(fileLength)})";
                        };
                    }

                    EditorApplication.delayCall += () =>
                    {
                        progressBar.value = 1f;
                        statusLabel.text = "上传完成";
                        EditorUtility.DisplayDialog("完成", "FTP 上传完成。", "确定");
                        uploadBtn.SetEnabled(true);
                    };
                }
                catch (Exception ex)
                {
                    EditorApplication.delayCall += () =>
                    {
                        statusLabel.text = "上传失败: " + ex.Message;
                        EditorUtility.DisplayDialog("上传失败", ex.Message, "确定");
                        uploadBtn.SetEnabled(true);
                    };
                }
            });
        };
    }

    static string GetLatestOutputDir(BuildTarget buildTarget, string packageName)
    {
        var outputRoot = $"{AssetBundleBuilderHelper.GetDefaultBuildOutputRoot()}/{buildTarget}/{packageName}";
        if (!Directory.Exists(outputRoot))
            return null;
        var dirs = Directory.GetDirectories(outputRoot);
        Version max = new Version("1.0.0");
        string bestDir = null;
        foreach (var d in dirs)
        {
            try
            {
                var v = new Version(new DirectoryInfo(d).Name);
                if (max < v)
                {
                    max = v;
                    bestDir = d;
                }
            }
            catch
            {
                continue;
            }
        }
        return bestDir;
    }

    static void EnsureFtpDirectoryExists(string host, string user, string pass, string dir)
    {
        // dir 形如 "path/to/dir"
        var parts = dir.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        string cur = "";
        foreach (var p in parts)
        {
            cur = string.IsNullOrEmpty(cur) ? p : $"{cur}/{p}";
            try
            {
                string uri = $"{host}/{cur}";
                var request = (FtpWebRequest)WebRequest.Create(uri);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.Credentials = new NetworkCredential(user, pass);
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                using var resp = (FtpWebResponse)request.GetResponse();
                // Directory created or already exists
            }
            catch (WebException wex)
            {
                // 如果目录已存在，某些服务器会返回错误；忽略该错误
                var resp = wex.Response as FtpWebResponse;
                if (resp != null)
                {
                    // 550 目录已存在 或 权限等问题，忽略并继续
                }
                else
                {
                    // 其它类型的错误也忽略，以提高鲁棒性
                }
            }
            catch
            {
                // 忽略所有错误，继续尝试创建更深层级目录
            }
        }
    }

    static async Task UploadFileWithProgress(string uri, string user, string pass, string localFilePath, Action<long> onChunkSent)
    {
        const int bufferSize = 81920; // 80 KB
        var request = (FtpWebRequest)WebRequest.Create(uri);
        request.Method = WebRequestMethods.Ftp.UploadFile;
        request.Credentials = new NetworkCredential(user, pass);
        request.UseBinary = true;
        request.UsePassive = true;
        request.KeepAlive = false;
        // 一些服务器在上传大文件时需要设置 ContentLength
        var fileInfo = new FileInfo(localFilePath);
        request.ContentLength = fileInfo.Length;

        using var fileStream = File.OpenRead(localFilePath);
        using var requestStream = await request.GetRequestStreamAsync();

        byte[] buffer = new byte[bufferSize];
        int read;
        while ((read = await fileStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            await requestStream.WriteAsync(buffer, 0, read);
            // 回调已发送字节数
            onChunkSent(read);
        }
        // 结束请求
        using var response = (FtpWebResponse)await request.GetResponseAsync();
        // response.StatusDescription 可用于日志
    }
}