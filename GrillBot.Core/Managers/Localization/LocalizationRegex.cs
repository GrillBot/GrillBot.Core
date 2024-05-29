using System.Text.RegularExpressions;

namespace GrillBot.Core.Managers.Localization;

public static partial class LocalizationRegex
{
    [GeneratedRegex("\\w+.(?<locale>\\w{2}(?:-\\w{2})?).json", RegexOptions.Compiled | RegexOptions.Singleline)]
    public static partial Regex ParserRegex();
}
