namespace DataCat.Server.Application.Utils;

public static partial class StringExtensions  
{
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var startUnderscores = StartUnderscoreRegex().Match(input);
        return startUnderscores + SnackCaseRegex().Replace(input, "$1_$2").ToLower();
    }

    [GeneratedRegex("([a-z0-9])([A-Z])")]
    private static partial Regex SnackCaseRegex();
    
    [GeneratedRegex("^_+")]
    private static partial Regex StartUnderscoreRegex();
}