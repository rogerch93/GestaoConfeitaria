namespace GestaoConfeitaria.Auth;

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "GestaoConfeitaria";
    public string Audience { get; set; } = "GestaoConfeitaria";
    public int ExpiresInMinutes { get; set; } = 60; //1 hora
}