using System.Text;

namespace ICollections.Constants;

public static class AccessDropBoxConstants
{
    private const string accessTokenCoded =
        "736c2e42473466456f4c45644567335f35556f4b4676445276686a5741567134573861725143396a676351495938424f4179725976545f35425350345f6f637672745369416a68445938523775546877536c6e793774394b516669414c43637a305f554d6952677354445465554d462d634271383739534556616448736b30317063754d6875437066754e6f673371";


    private static string accessToken;
    
    public const string Folder = "/Images";

    public static char[] ToCutView = { '?', 'd', 'l', '=', '0' };

    public static void SetAccessToken() => ConvertHexToString(accessTokenCoded, Encoding.UTF8);
    public static string GetToken() => accessToken;
    
    private static void ConvertHexToString(string hexInput, Encoding encoding)
    {
        var numberChars = hexInput.Length;
        var bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
        }
        accessToken = encoding.GetString(bytes);
    }
}