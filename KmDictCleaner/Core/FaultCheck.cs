namespace KmDictCleaner.Core;

/// <summary> 检查编码错误 </summary>
internal static class FaultCheck
{
    /// <summary> 某字的首码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi"> 首码出错的字 </param>
    /// <param name="errMa"> 出错的首码 </param>
    public static bool Char1(string code, string word, char errZi, char errMa) =>
        (word.Length, code.Length) switch {
            (1, > 1) => word[0] == errZi && code[0] == errMa, // 非1码单字
            (2, 2) => (word[0] == errZi && code[0] == errMa)
                   || (word[1] == errZi && code[1] == errMa), // 2码2字词
            (2, 4) => (word[0] == errZi && code[0] == errMa)
                   || (word[1] == errZi && code[2] == errMa), // 4码2字词
            (3, 4) => (word[0] == errZi && code[0] == errMa)
                   || (word[1] == errZi && code[2] == errMa)
                   || (word[2] == errZi && code[3] == errMa), // 4码3字词
            (> 3, > 3) => Enumerable.Range(0, code.Length - 1)
                              .Any(i => word[i] == errZi && code[i] == errMa)
                       || (word[^1] == errZi && code[^1] == errMa), // 多码多字词
            _ => false
        };

    /// <summary> 某字的次码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi"> 次码出错的字 </param>
    /// <param name="errMa"> 出错的次码 </param>
    public static bool Char2(string code, string word, char errZi, char errMa) =>
        (word.Length, code.Length) switch {
            (1, > 1) => word[0] == errZi && code[1] == errMa, // 非1码单字
            (2, 4) => (word[0] == errZi && code[1] == errMa)
                   || (word[1] == errZi && code[3] == errMa), // 4码2字词
            (3, 4) => word[0] == errZi && code[1] == errMa, // 4码3字词
            _ => false
        };

    /// <summary> 某字的前2码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi"> 前2码出错的字 </param>
    /// <param name="errMa1"> 出错的首码 </param>
    /// <param name="errMa2"> 出错的次码 </param>
    public static bool Char12(string code, string word, char errZi, char errMa1, char errMa2) =>
        (word.Length, code.Length) switch {
            (1, > 1) => word[0] == errZi && code[0] == errMa1 && code[1] == errMa2, // 非1码单字
            (2, 4) => (word[0] == errZi && code[0] == errMa1 && code[1] == errMa2)
                   || (word[1] == errZi && code[2] == errMa1 && code[3] == errMa2), // 4码2字词
            (3, 4) => word[0] == errZi && code[0] == errMa1 && code[1] == errMa2, // 4码3字词
            _ => false
        };

    /// <summary> 某2字组合的首码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa"> 出错的首码 </param>
    public static bool Pair1(string code, string word, char errZi1, char errZi2, char errMa) =>
        (word.Length, code.Length) switch {
            (2, 4) => word[0] == errZi1 && word[1] == errZi2 && code[0] == errMa, // 4码2字词
            (3, 4) => (word[0] == errZi1 && word[1] == errZi2 && code[0] == errMa)
                   || (word[1] == errZi1 && word[2] == errZi2 && code[2] == errMa), // 4码3字词
            (> 3, > 3) => Enumerable.Range(0, code.Length - 1)
                .Any(i => word[i] == errZi1 && word[i + 1] == errZi2 && code[i] == errMa), // 多码多字词
            _ => false
        };

    /// <summary> 某2字组合的次码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa"> 出错的次码 </param>
    public static bool Pair2(string code, string word, char errZi1, char errZi2, char errMa) =>
        word.Length is 2 or 3
     && code.Length == 4
     && word[0] == errZi1
     && word[1] == errZi2
     && code[1] == errMa;

    /// <summary> 某2字组合的第3码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa"> 出错的第3码 </param>
    public static bool Pair3(string code, string word, char errZi1, char errZi2, char errMa) =>
        (word.Length, code.Length) switch {
            (2, 4) => word[0] == errZi1 && word[1] == errZi2 && code[2] == errMa, // 4码2字词
            (3, 4) => (word[0] == errZi1 && word[1] == errZi2 && code[2] == errMa)
                   || (word[1] == errZi1 && word[2] == errZi2 && code[3] == errMa), // 4码3字词
            (> 3, > 3) => Enumerable.Range(1, code.Length - 1)
                .Any(i => word[i - 1] == errZi1 && word[i] == errZi2 && code[i] == errMa), // 多码多字词
            _ => false
        };

    /// <summary> 某2字组合的第4码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa"> 出错的第4码 </param>
    public static bool Pair4(string code, string word, char errZi1, char errZi2, char errMa) =>
        word.Length == 2
     && code.Length == 4
     && word[0] == errZi1
     && word[1] == errZi2
     && code[3] == errMa;

    /// <summary> 某2字组合的前2码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa1"> 出错的首码 </param>
    /// <param name="errMa2"> 出错的次码 </param>
    public static bool Pair12(
        string code,
        string word,
        char errZi1,
        char errZi2,
        char errMa1,
        char errMa2) =>
        word.Length is 2 or 3
     && code.Length == 4
     && word[0] == errZi1
     && word[1] == errZi2
     && code[0] == errMa1
     && code[1] == errMa2;

    /// <summary> 某2字组合的后2码出错，检查该词库条目是否涉及 </summary>
    /// <param name="code"> 编码 </param>
    /// <param name="word"> 字词 </param>
    /// <param name="errZi1"> 2字组合的首字 </param>
    /// <param name="errZi2"> 2字组合的次字 </param>
    /// <param name="errMa3"> 出错的第3码 </param>
    /// <param name="errMa4"> 出错的第4码 </param>
    public static bool Pair34(
        string code,
        string word,
        char errZi1,
        char errZi2,
        char errMa3,
        char errMa4) =>
        word.Length == 2
     && code.Length == 4
     && word[0] == errZi1
     && word[1] == errZi2
     && code[2] == errMa3
     && code[3] == errMa4;
}
