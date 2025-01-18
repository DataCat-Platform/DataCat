namespace DataCat.Server.Application.Utils;

public static class FileHasher
{
    public static string CalculateFileHash(string filePath)
    {
        using var md5 = MD5.Create();
        using var fileStream = File.OpenRead(filePath);
        var hashBytes = md5.ComputeHash(fileStream);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}