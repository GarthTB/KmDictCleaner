using KmDictCleaner.Core;
using static System.Console;

try {
    WriteLine("空明码词库清理工具 v1.0.0 (20251229)");
    WriteLine("作者：Garth TB <g-art-h@outlook.com>");

    WriteLine("开始清理...");
    var (pre, post) = DictFile.Proc(
        @"G:\Code\Csharp\AAAA-TESTS\MasterDit.shp", // 输入文件（词库）路径
        [
            static (code, word) => !FaultCheck.Char1(code, word, '冕', 'M'),
            static (code, word) => !FaultCheck.Char2(code, word, '冕', 's')
        ]);
    WriteLine("清理完成。");

    var removed = pre - post;
    var ratio = 100d * removed / pre;
    WriteLine($"清理前总词数：{pre}");
    WriteLine($"清理后总词数：{post}");
    WriteLine($"清理掉词数：{removed}");
    WriteLine($"词数减少了：{ratio:0.###} %");
} catch (Exception ex) {
    ForegroundColor = ConsoleColor.Red;
    WriteLine($"出错：\n{ex.Message}\n\n{ex.StackTrace}");
    ResetColor();
} finally {
    WriteLine("程序结束。已退出。");
}
