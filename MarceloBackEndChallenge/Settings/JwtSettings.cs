using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtSettings
{
    public string SecretKey { get; set; }
    
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public int ExpirationInMinutes { get; set; }

    public DateTime IssuedAt { get => DateTime.UtcNow; }

    public DateTime NotBefore { get => DateTime.UtcNow; }

    public SigningCredentials SigningCredentials 
    { 
        get 
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        }
    }

    public JwtSettings()
    {
        SecretKey = string.Empty;
        Issuer    = string.Empty;
        Audience  = string.Empty;
    }
}