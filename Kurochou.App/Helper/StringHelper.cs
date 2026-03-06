namespace Kurochou.App.Helper;

public static class StringHelper
{
    public static Guid ToGuid(this string? str)
    {
        if (Guid.TryParse(str, out var guid))
            return guid;

        return Guid.Empty;
    }

    public static string EncryptPassword(this string password)
        => BCrypt.Net.BCrypt.HashPassword(password);
}