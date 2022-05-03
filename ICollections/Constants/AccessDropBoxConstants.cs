namespace ICollections.Constants;

public static class AccessDropBoxConstants
{
    private const string accessToken =
        "sl.BG7c2FFAGXuOIOPEFt6qDvfqezD5L5fWj7NAIr4Om0etnRFSCVkSbcim-USD6Pzzb03QQwU5VnZRcJiHNxB0f-CkQxRFHKhI6BLaYLuO_QgP9IYXgAdQE95vKVUD_hbJwolHh37KpqL6";

    public static string GetToken() => accessToken;

    public const string Folder = "/Images";

    public static char[] ToCut = { '?', 'd', 'l', '=', '0' };
}