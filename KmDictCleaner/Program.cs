using KmDictCleaner;
using static System.Console;
using static System.IO.Path;

try {
    WriteLine("空明码词库清理工具 v1.1.0 (20260409)");
    WriteLine("作者：Garth TB | 天卜 <g-art-h@outlook.com>");

    WriteLine("开始清理...");
    var (befCnt, aftCnt) = Proc(
        @"G:\Code\Csharp\AAAA-TESTS\MasterDit.shp", // 输入文件（词库）路径
        [
            static (code, word) => !Checker.Pair1(code, word, '红', '军', 'G'),
            static (code, word) => !Checker.Pair1(code, word, '参', '数', 's'),
            static (code, word) => !Checker.Pair1(code, word, '重', '新', 'Z'),
            static (code, word) => !Checker.Pair1(code, word, '属', '性', 'Z'),
            static (code, word) => !Checker.Pair1(code, word, '单', '纯', 'C'),
            static (code, word) => !Checker.Pair1(code, word, '单', '纯', 'S'),
            static (code, word) => !Checker.Pair1(code, word, '朝', '鲜', 'Z')
        ]);
    WriteLine("清理完成");

    var remCnt = befCnt - aftCnt;
    WriteLine($"清理前词数：{befCnt}");
    WriteLine($"清理后词数：{aftCnt}");
    WriteLine($"清理量：{remCnt}\t{100d * remCnt / befCnt:0.###} %");
} catch (Exception ex) {
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"异常中断：\n{ex}");
    ResetColor();
} finally { WriteLine("程序结束"); }

static (long, long) Proc(string iPath, Func<string, string, bool>[] checkers) {
    long befCnt = 0, aftCnt = 0;
    var result = File.ReadLines(iPath)
        .AsParallel()
        .AsOrdered()
        .Select(line => {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2) return "";
            Interlocked.Add(ref befCnt, parts.Length - 1);
            var code = parts[0];
            List<string> words = new(parts.Length - 1) { code };
            words.AddRange(parts.Skip(1).Where(word => checkers.All(check => check(code, word))));
            if (words.Count < 2) return "";
            Interlocked.Add(ref aftCnt, words.Count - 1);
            return string.Join(' ', words);
        })
        .Where(static line => !string.IsNullOrEmpty(line));

    var dir = GetDirectoryName(iPath) ?? ".";
    var name = GetFileNameWithoutExtension(iPath);
    var ext = GetExtension(iPath);
    var oPath = Combine(dir, $"{name}_2{ext}");
    for (var i = 3; File.Exists(oPath); i++) oPath = Combine(dir, $"{name}_{i}{ext}");
    File.WriteAllLines(oPath, result);

    return (befCnt, aftCnt);
}
