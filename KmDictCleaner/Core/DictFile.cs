namespace KmDictCleaner.Core;

/// <summary> 处理词库文件 </summary>
internal static class DictFile
{
    /// <summary> 筛选词库文件，只保留能通过所有checks的词 </summary>
    /// <param name="inPath"> 输入文件路径 </param>
    /// <param name="checks"> 检查函数，返回true表示通过 </param>
    /// <returns> 处理前后的总词数 </returns>
    public static (long Pre, long Post) Proc(string inPath, Func<string, string, bool>[] checks) {
        long pre = 0, post = 0;
        var result = File.ReadLines(inPath)
            .AsParallel()
            .AsOrdered()
            .Select(line => {
                var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    return "";
                Interlocked.Add(ref pre, parts.Length - 1);
                var code = parts[0];
                List<string> words = new(parts.Length - 1) { code };
                foreach (var word in parts.AsSpan(1))
                    if (checks.All(check => check(code, word)))
                        words.Add(word);
                if (words.Count < 2)
                    return "";
                Interlocked.Add(ref post, words.Count - 1);
                return string.Join(' ', words);
            })
            .Where(static line => !string.IsNullOrEmpty(line));
        File.WriteAllLines(GenOutPath(inPath), result);
        return (pre, post);
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
