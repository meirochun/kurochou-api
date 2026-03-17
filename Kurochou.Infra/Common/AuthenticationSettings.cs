namespace Kurochou.Infra.Common;

public class AuthenticationSettings
{
    public JwtSettings Jwt { get; set; } = new();
    public GoogleSettings Google { get; set; } = new();
}

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

public class GoogleSettings
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}