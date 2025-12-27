namespace KmDictCleaner.Core;

/// <summary> 处理词库文件 </summary>
internal static class DictFile
{
    /// <summary> 筛除词库文件，只保留能通过所有checks的词 </summary>
    /// <param name="inPath"> 输入文件路径 </param>
    /// <param name="checks"> 检查函数，返回true表示通过 </param>
    public static void Proc(string inPath, Func<string, string, bool>[] checks) {
        var result = File.ReadLines(inPath)
            .Select(static line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .AsParallel()
            .AsOrdered()
            .Select(parts => {
                if (parts.Length < 2)
                    return (string[])[];
                var code = parts[0];
                return [code, ..parts[1..].Where(word => checks.All(check => check(code, word)))];
            })
            .Where(static parts => parts.Length > 1)
            .Select(static parts => string.Join(' ', parts));
        File.WriteAllLines(GenOutPath(inPath), result);
    }

    /// <summary> 生成输出文件路径 </summary>
    private static string GenOutPath(string inPath) {
        var dir = Path.GetDirectoryName(inPath);
        if (!Path.Exists(dir))
            throw new ArgumentException("无法获取输入路径的目录");
        var name = Path.GetFileNameWithoutExtension(inPath);
        var ext = Path.GetExtension(inPath);
        var outPath = Path.Combine(dir, $"{name}_2{ext}");
        for (var i = 3; File.Exists(outPath); i++)
            outPath = Path.Combine(dir, $"{name}_{i}{ext}");
        return outPath;
    }
}
