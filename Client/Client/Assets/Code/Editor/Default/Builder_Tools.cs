using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEditor.PackageManager;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;

static class Builder_raw
{
    [RPCQuery("Builder_raw")]
    static Dictionary<string, string> query(RPCArgs args) => Builder_Tools.query(args);

    [RPCAttribute("Builder_raw", 1)]
    static Task GitOrSvnPull(RPCArgs args) => Builder_Tools.GitOrSvnPull();

    [RPCAttribute("Builder_raw", 3)]
    static void Build_Bundle(RPCArgs args) => Builder_Tools.Build_Bundle_raw(args);

    [RPCAttribute("Builder_raw", 4)]
    static IAsyncEnumerable<string> Up_Bundles(RPCArgs args) => Builder_Tools.Up_Bundles(args);
}
static class Builder_res
{
    [RPCQuery("Builder_res")]
    static Dictionary<string, string> query(RPCArgs args) => Builder_Tools.query(args);

    [RPCAttribute("Builder_res", 1)]
    static Task GitOrSvnPull(RPCArgs args) => Builder_Tools.GitOrSvnPull();

    [RPCAttribute("Builder_res", 3)]
    static void Build_Bundle(RPCArgs args) => Builder_Tools.Build_Bundle_res(args);

    [RPCAttribute("Builder_res", 4)]
    static IAsyncEnumerable<string> Up_Bundles(RPCArgs args) => Builder_Tools.Up_Bundles(args);
}
static class Builder_Tools
{
    public static Dictionary<string, string> query(RPCArgs args)
    {
        var rst = new Dictionary<string, string>();
        if (Builder_Tools.GetBuildPackageVersion(args.GetValueOrDefault("PackageName"), out var vs))
            rst.Add("vs", vs);
        return rst;
    }


    // ============================================================
    // Git Pull（自动检测仓库和分支）
    // ============================================================
    public static async Task GitOrSvnPull()
    {
        // 从当前 Unity 项目目录向上查找 .git .svn 目录
        string currentDir = Application.dataPath;
        string repoPath = null;
        bool isGit = false;
        while (!string.IsNullOrEmpty(currentDir))
        {
            if (Directory.Exists(Path.Combine(currentDir, ".git")))
            {
                repoPath = currentDir;
                isGit = true;
                break;
            }
            if (Directory.Exists(Path.Combine(currentDir, ".svn")))
            {
                repoPath = currentDir;
                break;
            }
            currentDir = Directory.GetParent(currentDir)?.FullName;
        }

        if (string.IsNullOrEmpty(repoPath))
            throw new Exception("未找到 git仓库或svn根 目录");

        if (isGit)
        {
            // 执行 git pull
            ProcessStartInfo psi;
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                psi = new ProcessStartInfo("git", $"pull")
                {
                    WorkingDirectory = repoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                // 执行 git pull，并临时指定 credential.helper（确保生效）
                psi = new ProcessStartInfo("git", "-c credential.helper=osxkeychain pull")
                {
                    WorkingDirectory = repoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
            }
            else
                throw new NotSupportedException();

            using (var p = Process.Start(psi))
            {
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                p.WaitForExit();
                if (p.ExitCode != 0)
                    throw new Exception($"git pull 失败: {error}");
                UnityEngine.Debug.Log($"git pull 成功: {output}");
            }
        }
        else
        {
            var psi = new ProcessStartInfo
            {
                FileName = "svn.exe",
                Arguments = $"update \"{repoPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardOutputEncoding = System.Text.Encoding.UTF8
            };
            using (var p = Process.Start(psi))
            {
                string output = p.StandardOutput.ReadToEnd();
                string error = p.StandardError.ReadToEnd();
                p.WaitForExit();

                if (p.ExitCode != 0)
                    throw new Exception($"svn update 失败: {error}");
                UnityEngine.Debug.Log($"svn update 成功: {output}");
            }
        }
     
        AssetDatabase.Refresh();
        CompilationPipeline.RequestScriptCompilation();
        //等待程序集重载
        while (true)
            await Task.Delay(1);
    }

    // ============================================================
    // 构建资源包
    // ============================================================
    public static string Build_Bundle_raw(RPCArgs args)
    {
        var PackageName = args.GetValueOrDefault("PackageName");
        var vs = args.GetValueOrDefault("vs");
        var PipelineName = BundleBuilderSetting.GetPackageBuildPipeline(PackageName);
        
        var fileNameStyle = BundleBuilderSetting.GetPackageFileNameStyle(PackageName, PipelineName);
        var bundledCopyOption = BundleBuilderSetting.GetPackageBundledCopyOption(PackageName, PipelineName);
        var bundledCopyParams = BundleBuilderSetting.GetPackageBundledCopyParams(PackageName, PipelineName);
        var clearBuildCache = BundleBuilderSetting.GetPackageClearBuildCache(PackageName, PipelineName);
        var useAssetDependencyDB = BundleBuilderSetting.GetPackageUseAssetDependencyDB(PackageName, PipelineName);

        RawFileBuildParameters buildParameters = new RawFileBuildParameters();
        buildParameters.BuildOutputRoot = BundleBuilderHelper.GetDefaultBuildOutputRoot();
        buildParameters.BundledFileRoot = BundleBuilderHelper.GetStreamingAssetsRoot();
        buildParameters.BuildPipeline = PipelineName.ToString();
        buildParameters.BuildBundleType = (int)EBundleType.RawBundle;
        buildParameters.BuildTarget = EditorUserBuildSettings.activeBuildTarget;
        buildParameters.PackageName = PackageName;
        GetBuildPackageVersion(PackageName,out var vsStr);
        buildParameters.PackageVersion = string.IsNullOrEmpty(vs) ? vsStr : vs;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.FileNameStyle = fileNameStyle;
        buildParameters.BundledCopyOption = bundledCopyOption;
        buildParameters.BundledCopyParams = bundledCopyParams;
        buildParameters.ClearBuildCacheFiles = clearBuildCache;
        buildParameters.UseAssetDependencyDB = useAssetDependencyDB;
        buildParameters.BundleEncryptor = CreateBundleEncryptorInstance(PackageName);
        buildParameters.ManifestEncryptor = CreateManifestEncryptorInstance(PackageName);
        buildParameters.ManifestDecryptor = CreateManifestDecryptorInstance(PackageName);

        RawFileBuildPipeline pipeline = new RawFileBuildPipeline();
        var buildResult = pipeline.Run(buildParameters, true);
        if (!buildResult.Success)
            throw new Exception($"构建失败 [{buildResult.ErrorInfo}]");

        cull_res(buildParameters.PackageName);
        return buildParameters.PackageVersion;
    }
    public static string Build_Bundle_res(RPCArgs args)
    { 
        string[] guids = AssetDatabase.FindAssets("t:TMP_FontAsset");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<TMPro.TMP_FontAsset>(path);
            if (asset.atlasPopulationMode != TMPro.AtlasPopulationMode.Static)
                asset.ClearFontAssetData(true);
        }
        AssetDatabase.Refresh();
        var PackageName = args.GetValueOrDefault("PackageName");
        var vs = args.GetValueOrDefault("vs");
        var PipelineName = BundleBuilderSetting.GetPackageBuildPipeline(PackageName);

        var fileNameStyle = BundleBuilderSetting.GetPackageFileNameStyle(PackageName, PipelineName);
        var bundledCopyOption = BundleBuilderSetting.GetPackageBundledCopyOption(PackageName, PipelineName);
        var bundledCopyParams = BundleBuilderSetting.GetPackageBundledCopyParams(PackageName, PipelineName);
        var compressOption = BundleBuilderSetting.GetPackageCompressOption(PackageName, PipelineName);
        var clearBuildCache = BundleBuilderSetting.GetPackageClearBuildCache(PackageName, PipelineName);
        var useAssetDependencyDB = BundleBuilderSetting.GetPackageUseAssetDependencyDB(PackageName, PipelineName);

        ScriptableBuildParameters buildParameters = new ScriptableBuildParameters();
        buildParameters.BuildOutputRoot = BundleBuilderHelper.GetDefaultBuildOutputRoot();
        buildParameters.BundledFileRoot = BundleBuilderHelper.GetStreamingAssetsRoot();
        buildParameters.BuildPipeline = PipelineName.ToString();
        buildParameters.BuildBundleType = (int)EBundleType.AssetBundle;
        buildParameters.BuildTarget = EditorUserBuildSettings.activeBuildTarget;
        buildParameters.PackageName = PackageName;
        GetBuildPackageVersion(PackageName, out var vsStr);
        buildParameters.PackageVersion = string.IsNullOrEmpty(vs) ? vsStr : vs;
        buildParameters.EnableSharePackRule = true;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.FileNameStyle = fileNameStyle;
        buildParameters.BundledCopyOption = bundledCopyOption;
        buildParameters.BundledCopyParams = bundledCopyParams;
        buildParameters.CompressOption = compressOption;
        buildParameters.ClearBuildCacheFiles = clearBuildCache;
        buildParameters.UseAssetDependencyDB = useAssetDependencyDB;
        buildParameters.BundleEncryptor = CreateBundleEncryptorInstance(PackageName);
        buildParameters.ManifestEncryptor = CreateManifestEncryptorInstance(PackageName);
        buildParameters.ManifestDecryptor = CreateManifestDecryptorInstance(PackageName);
        buildParameters.BuiltinShadersBundleName = GetBuiltinShaderBundleName(PackageName);

        ScriptableBuildPipeline pipeline = new ScriptableBuildPipeline();
        var buildResult = pipeline.Run(buildParameters, true);
        if (!buildResult.Success)
            throw new Exception($"构建失败 [{buildResult.ErrorInfo}]");

        cull_res(buildParameters.PackageName);
        return buildParameters.PackageVersion;
    }
    static IBundleEncryptor CreateBundleEncryptorInstance(string PackageName)
    {
        var className = BundleBuilderSetting.GetPackageBundleEncryptorClassName(PackageName, BundleBuilderSetting.GetPackageBuildPipeline(PackageName));
        var classTypes = EditorAssemblyUtility.GetAssignableTypes(typeof(IBundleEncryptor));
        var classType = classTypes.Find(x => x.FullName.Equals(className));
        if (classType != null)
            return (IBundleEncryptor)Activator.CreateInstance(classType);

        UnityEngine.Debug.LogWarning($"Bundle encryptor class type not found: '{className}'.");
        return null;
    }
    static IManifestEncryptor CreateManifestEncryptorInstance(string PackageName)
    {
        var className = BundleBuilderSetting.GetPackageManifestEncryptorClassName(PackageName, BundleBuilderSetting.GetPackageBuildPipeline(PackageName));
        var classTypes = EditorAssemblyUtility.GetAssignableTypes(typeof(IManifestEncryptor));
        var classType = classTypes.Find(x => x.FullName.Equals(className));
        if (classType != null)
            return (IManifestEncryptor)Activator.CreateInstance(classType);

        UnityEngine.Debug.LogWarning($"Manifest encryptor class type not found: '{className}'.");
        return null;
    }
    static IManifestDecryptor CreateManifestDecryptorInstance(string PackageName)
    {
        var className = BundleBuilderSetting.GetPackageManifestDecryptorClassName(PackageName, BundleBuilderSetting.GetPackageBuildPipeline(PackageName));
        var classTypes = EditorAssemblyUtility.GetAssignableTypes(typeof(IManifestDecryptor));
        var classType = classTypes.Find(x => x.FullName.Equals(className));
        if (classType != null)
            return (IManifestDecryptor)Activator.CreateInstance(classType);

        UnityEngine.Debug.LogWarning($"Manifest decryptor class type not found: '{className}'.");
        return null;
    }
    static string GetBuiltinShaderBundleName(string PackageName)
    {
        var uniqueBundleName = BundleCollectorSettingData.Setting.UniqueBundleName;
        var packRuleResult = DefaultBundlePackRule.CreateShadersPackRuleResult();
        return packRuleResult.GetBundleName(PackageName, uniqueBundleName);
    }

    // ============================================================
    // 上传资源包
    // ============================================================
    public static async IAsyncEnumerable<string> Up_Bundles(RPCArgs args)
    {
        string outputRoot = BundleBuilderHelper.GetDefaultBuildOutputRoot();
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string packageDir = Path.Combine(outputRoot, target.ToString(), args.GetValueOrDefault("PackageName"));

        if (!Directory.Exists(packageDir))
        {
            UnityEngine.Debug.LogError($"Up_Bundles: 包目录不存在: {packageDir}");
            yield return $"Up_Bundles: 包目录不存在: {packageDir}";
        }

        var versionDirs = Directory.GetDirectories(packageDir)
            .Select(d => new { Path = d, Name = Path.GetFileName(d) })
            .Where(x => Version.TryParse(x.Name, out _))
            .OrderByDescending(x => new Version(x.Name))
            .ToList();

        if (versionDirs.Count == 0)
        {
            UnityEngine.Debug.LogError($"Up_Bundles: 未找到版本目录 in {packageDir}");
            yield return $"Up_Bundles: 未找到版本目录 in {packageDir}";
        }

        var first = versionDirs.First();
        string latestVersionDir = first.Path;
        string version = Path.GetFileName(latestVersionDir);
        UnityEngine.Debug.Log($"Up_Bundles: 最新版本 {version}，路径 {latestVersionDir}");

        var files = Directory.GetFiles(latestVersionDir, "*.*", SearchOption.AllDirectories).ToList();
        if (files.Count == 0)
        {
            UnityEngine.Debug.LogError($"Up_Bundles: 在 {latestVersionDir} 中未找到任何文件");
            yield return $"Up_Bundles: 在 {latestVersionDir} 中未找到任何文件";
        }

        string username = args.GetValueOrDefault("user");
        string password = args.GetValueOrDefault("pass");
        var uploadMethod = args.GetValueOrDefault("upmethod");
        string url = args.GetValueOrDefault("url");
        if (uploadMethod == "ftp")
        {
            UnityEngine.Debug.Log($"Up_Bundles: FTP 上传到 {url}");
            await foreach (var item in UploadViaFTP(url, username, password, files))
                yield return item;
            yield return $"版本={version}";
        }
        else if (uploadMethod == "ssh")
        {
            UnityEngine.Debug.Log($"Up_Bundles: SSH 上传到 {url}");
            await foreach (var item in UploadViaSSH(url, username, password, args.GetValueOrDefault("sshKey"), files, args.GetValueOrDefault("path")))
                yield return item;
            yield return $"版本={version}";
        }
        else
            yield return "不支持非ftp或ssh上传";
    }

    // ============================================================
    // FTP 上传
    // ============================================================
    private static async IAsyncEnumerable<string> UploadViaFTP(string url, string username, string password, List<string> files)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL 不能为空", nameof(url));
        if (files == null || files.Count == 0)
        {
            UnityEngine.Debug.Log("FTP 文件列表为空，无需上传");
            yield return "FTP 文件列表为空，无需上传";
        }

        // 确保 url 以 '/' 结尾，方便拼接文件名
        string baseUrl = url.TrimEnd('/') + "/";

        // 尝试创建目标目录（如果不存在）
        try
        {
            FtpWebRequest mkdirReq = (FtpWebRequest)WebRequest.Create(baseUrl);
            mkdirReq.Method = WebRequestMethods.Ftp.MakeDirectory;
            mkdirReq.Credentials = new NetworkCredential(username, password);
            mkdirReq.UsePassive = true;
            using (FtpWebResponse resp = (FtpWebResponse)mkdirReq.GetResponse()) { }
            UnityEngine.Debug.Log($"FTP 目录已创建: {baseUrl}");
        }
        catch (WebException ex)
        {
            // 如果目录已存在（状态码 550）则忽略，否则抛出
            if (ex.Response is FtpWebResponse resp)
            {
                if (resp.StatusCode != FtpStatusCode.ActionNotTakenFileUnavailable)
                    throw;
            }
            else
            {
                throw; // 非 FTP 响应，重新抛出
            }
        }

        // 上传每个文件
        for (int i = 0; i < files.Count; i++)
        {
            var localFile = files[i];
            if (!File.Exists(localFile))
            {
                UnityEngine.Debug.LogWarning($"文件不存在，跳过: {localFile}");
                continue;
            }

            string fileName = Path.GetFileName(localFile);
            string ftpUrl = baseUrl + fileName;

            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpUrl);
            req.Method = WebRequestMethods.Ftp.UploadFile;
            req.Credentials = new NetworkCredential(username, password);
            req.UsePassive = true;
            req.UseBinary = true;

            byte[] data = File.ReadAllBytes(localFile);
            req.ContentLength = data.Length;

            using (Stream s = req.GetRequestStream())
                await s.WriteAsync(data, 0, data.Length);

            using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
            {

            }
            yield return $"上传成功={i + 1}/{files.Count} ->{fileName}";
        }
    }

    // ============================================================
    // SSH 上传（暂未实现）
    // ============================================================
    private static async IAsyncEnumerable<string> UploadViaSSH(string url, string username, string password, string privateKeyPath, List<string> files, string path)
    {
        if (string.IsNullOrEmpty(url))
            throw new ArgumentException("URL 不能为空", nameof(url));
        if (string.IsNullOrEmpty(username))
            throw new ArgumentException("用户名不能为空", nameof(username));
        if (files == null || files.Count == 0)
        {
            UnityEngine.Debug.Log("文件列表为空，无需上传");
            yield return "文件列表为空，无需上传";
        }

        // 解析主机和端口（格式: "host" 或 "host:port"）
        string host = url;
        int port = 22;
        int colonIndex = url.IndexOf(':');
        if (colonIndex > 0 && int.TryParse(url.Substring(colonIndex + 1), out int parsedPort))
        {
            host = url.Substring(0, colonIndex);
            port = parsedPort;
        }

        // 构建连接信息
        ConnectionInfo connectionInfo;
        if (!string.IsNullOrEmpty(privateKeyPath) && File.Exists(privateKeyPath))
        {
            var privateKeyFile = new PrivateKeyFile(privateKeyPath);
            var authMethod = new PrivateKeyAuthenticationMethod(username, privateKeyFile);
            connectionInfo = new ConnectionInfo(host, port, username, authMethod);
            UnityEngine.Debug.Log($"使用私钥认证: {privateKeyPath}");
        }
        else if (!string.IsNullOrEmpty(password))
        {
            var authMethod = new PasswordAuthenticationMethod(username, password);
            connectionInfo = new ConnectionInfo(host, port, username, authMethod);
            UnityEngine.Debug.Log("使用密码认证");
        }
        else
        {
            throw new ArgumentException("必须提供密码或有效的私钥路径");
        }

        using (var client = new SftpClient(connectionInfo))
        {
            client.Connect();
            UnityEngine.Debug.Log($"已连接到 {host}:{port}");

            void CreateRemoteDirectory(SftpClient client, string path)
            {
                if (string.IsNullOrEmpty(path)) return;
                path = path.Replace('\\', '/');
                // 分割路径
                string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string currentPath = "/";
                foreach (string part in parts)
                {
                    currentPath = string.IsNullOrEmpty(currentPath) ? "/" + part : currentPath + "/" + part;
                    if (!client.Exists(currentPath))
                    {
                        client.CreateDirectory(currentPath);
                        UnityEngine.Debug.Log($"创建远程目录: {currentPath}");
                    }
                }
            }
            // 如果指定了远程目录，则切换（若目录不存在则创建）
            if (!string.IsNullOrEmpty(path))
            {
                CreateRemoteDirectory(client, path);
                client.ChangeDirectory(path);
                UnityEngine.Debug.Log($"切换到远程目录: {path}");
            }

            for (int i = 0; i < files.Count; i++)
            {
                string filePath = files[i];
                if (!File.Exists(filePath))
                    continue;

                string fileName = Path.GetFileName(filePath);
                using (var fileStream = File.OpenRead(filePath))
                    await client.UploadFileAsync(fileStream, fileName);
                yield return $"上传成功={i + 1}/{files.Count} ->{fileName}";
            }

            if (client.IsConnected)
                client.Disconnect();
        }
    }

    // ============================================================
    // 获取版本号
    // ============================================================
    static bool GetBuildPackageVersion(string packageName, out string vs)
    {
        string dir = $"{BundleBuilderHelper.GetDefaultBuildOutputRoot()}/{EditorUserBuildSettings.activeBuildTarget}/{packageName}";

        if (Directory.Exists(dir))
        {
            var ds = Directory.GetDirectories(dir)
            .Select(t => new DirectoryInfo(t))
            .Where(t => Version.TryParse(t.Name, out _))
            .ToList();
            ds.Sort((x, y) =>
            {
                Version v1 = new Version(x.Name);
                Version v2 = new Version(y.Name);
                return v1.CompareTo(v2);
            });

            if (!Version.TryParse(ds.LastOrDefault().Name, out var ret))
            {
                vs = "1.0.0";
                return false;
            }
            vs = $"{ret.Major}.{ret.Minor}.{ret.Build + 1}";
            return true;
        }
        else
        {
            vs = "1.0.0";
            return false;
        }
    }

    // ============================================================
    // 清理旧版本
    // ============================================================
    static void cull_res(string packageName)
    {
        string dir = $"{BundleBuilderHelper.GetDefaultBuildOutputRoot()}/{EditorUserBuildSettings.activeBuildTarget}/{packageName}";
        if (!Directory.Exists(dir)) return;

        var ds = Directory.GetDirectories(dir)
            .Select(t => new DirectoryInfo(t))
            .Where(t => Version.TryParse(t.Name, out _))
            .ToList();

        if (ds.Count == 0) return;

        ds.Sort((x, y) =>
        {
            Version v1 = new Version(x.Name);
            Version v2 = new Version(y.Name);
            return v1.CompareTo(v2);
        });

        var src = ds.LastOrDefault();
        if (src == null) return;

        HashSet<string> oldFiles = new HashSet<string>();
        for (int i = 0; i < ds.Count - 1; i++)
        {
            foreach (var item in ds[i].GetFiles())
                oldFiles.Add(item.Name);
        }

        foreach (var item in src.GetFiles())
        {
            if (item.Name.EndsWith(".version"))
                continue;
            if (item.Name.EndsWith(".json") || item.Name.EndsWith(".xml") || item.Name.EndsWith(".report"))
                item.Delete();
            else if (oldFiles.Contains(item.Name))
                item.Delete();
        }
    }
}