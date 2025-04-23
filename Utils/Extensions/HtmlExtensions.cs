using System.Text.RegularExpressions;

namespace IntercomCsvConverter.Utils.Extensions;

public static class HtmlExtensions
{
    public static string StripHtml(this string input)
    {
        return string.IsNullOrWhiteSpace(input)
            ? string.Empty
            : Regex.Replace(input, "<.*?>", "").Replace("\n", " ").Replace("\r", " ").Trim();
    }
}