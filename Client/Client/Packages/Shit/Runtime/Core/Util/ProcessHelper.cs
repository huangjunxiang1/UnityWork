using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

public static class ProcessHelper
{
    public readonly struct RunResult
    {
        public readonly int ExitCode;
        public readonly string StdOut;
        public readonly string StdErr;

        public RunResult(int exitCode, string stdOut, string stdErr)
        {
            ExitCode = exitCode;
            StdOut = stdOut;
            StdErr = stdErr;
        }
    }

    /// <summary>
    /// 执行 .exe 或 .bat 并传入参数，等待结束后返回退出码与输出内容
    /// </summary>
    /// <param name="filePath">.exe 或 .bat 路径</param>
    /// <param name="args">参数列表（自动处理含空格的参数）</param>
    /// <param name="workingDirectory">工作目录，为空则默认使用目标文件所在目录</param>
    /// <param name="timeoutMs">超时时间（毫秒），-1 表示无限等待</param>
    /// <param name="createNoWindow">是否隐藏控制台窗口</param>
    public static RunResult Execute(
        string filePath,
        string[] args = null,
        string workingDirectory = null,
        int timeoutMs = -1,
        bool createNoWindow = true)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("文件路径不能为空", nameof(filePath));

        string fullPath = Path.GetFullPath(filePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("找不到目标文件", fullPath);

        string workDir = string.IsNullOrWhiteSpace(workingDirectory)
            ? Path.GetDirectoryName(fullPath)
            : workingDirectory;

        string escapedArgs = BuildArguments(args);
        bool isBatch = string.Equals(Path.GetExtension(fullPath), ".bat", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(Path.GetExtension(fullPath), ".cmd", StringComparison.OrdinalIgnoreCase);

        var psi = new ProcessStartInfo
        {
            WorkingDirectory = workDir,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            CreateNoWindow = createNoWindow
        };

        if (isBatch)
        {
            // .bat/.cmd 必须交给 cmd.exe 执行，注意双引号包裹完整命令行
            psi.FileName = "cmd.exe";
            string cmdLine = string.IsNullOrEmpty(escapedArgs)
                ? $"\"{fullPath}\""
                : $"\"{fullPath}\" {escapedArgs}";
            psi.Arguments = $"/d /s /c \"{cmdLine}\"";
        }
        else
        {
            psi.FileName = fullPath;
            psi.Arguments = escapedArgs;
        }

        var stdOutBuilder = new StringBuilder();
        var stdErrBuilder = new StringBuilder();

        using var process = new Process { StartInfo = psi };
        process.OutputDataReceived += (_, e) => { if (e.Data != null) stdOutBuilder.AppendLine(e.Data); };
        process.ErrorDataReceived += (_, e) => { if (e.Data != null) stdErrBuilder.AppendLine(e.Data); };

        if (!process.Start())
            throw new InvalidOperationException($"无法启动进程: {psi.FileName}");

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        bool exited;
        if (timeoutMs < 0)
        {
            process.WaitForExit();
            exited = true;
        }
        else
        {
            exited = process.WaitForExit(timeoutMs);
        }

        if (!exited)
        {
            try { process.Kill(); } catch { /* 忽略已退出的进程 */ }
            throw new TimeoutException($"进程执行超时 ({timeoutMs}ms): {fullPath}");
        }

        // 确保异步输出流全部flush完毕
        process.WaitForExit();

        return new RunResult(
            process.ExitCode,
            stdOutBuilder.ToString().TrimEnd(),
            stdErrBuilder.ToString().TrimEnd()
        );
    }

    private static string BuildArguments(string[] args)
    {
        if (args == null || args.Length == 0)
            return string.Empty;

        var sb = new StringBuilder();
        for (int i = 0; i < args.Length; i++)
        {
            if (i > 0) sb.Append(' ');
            sb.Append(EscapeArgument(args[i] ?? string.Empty));
        }
        return sb.ToString();
    }

    private static string EscapeArgument(string arg)
    {
        if (arg.Length == 0)
            return "\"\"";

        bool needsQuotes = false;
        foreach (char c in arg)
        {
            if (char.IsWhiteSpace(c) || c == '"')
            {
                needsQuotes = true;
                break;
            }
        }

        if (!needsQuotes)
            return arg;

        var sb = new StringBuilder();
        sb.Append('"');

        int backslashes = 0;
        foreach (char c in arg)
        {
            if (c == '\\')
            {
                backslashes++;
            }
            else if (c == '"')
            {
                // 引号前的反斜杠需要翻倍，再转义引号本身
                sb.Append('\\', backslashes * 2 + 1);
                sb.Append('"');
                backslashes = 0;
            }
            else
            {
                if (backslashes > 0)
                {
                    sb.Append('\\', backslashes);
                    backslashes = 0;
                }
                sb.Append(c);
            }
        }

        if (backslashes > 0)
            sb.Append('\\', backslashes * 2);

        sb.Append('"');
        return sb.ToString();
    }
}