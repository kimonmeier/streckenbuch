namespace Streckenbuch.Server.Helper;

public static class HashHelper
{
    public static string Hash(string toHash)
    {
        return BCrypt.Net.BCrypt.HashPassword(toHash);
    }

    public static bool Verify(string hashed, string unhashed)
    {
        return BCrypt.Net.BCrypt.Verify(unhashed, hashed);
    }
}
