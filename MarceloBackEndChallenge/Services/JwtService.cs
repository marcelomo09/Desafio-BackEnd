using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtService: ServiceBase
{
    private readonly IOptions<JwtSettings> _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings, MongoDBContext mongoDBContext) : base(mongoDBContext)
    {
        _jwtSettings = jwtSettings;
    }

    private static ClaimsIdentity GetClaimsIdentity(string userGroup)
    {
        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim("jti"          , Guid.NewGuid().ToString()));
        claims.Add(new Claim("sub"          , userGroup));
        claims.Add(new Claim(ClaimTypes.Role, userGroup));

        return new ClaimsIdentity(new GenericIdentity(userGroup), claims);
    } 

    public JwtToken CreateJwtToken(string userGroup)
    {
        var identity      = GetClaimsIdentity(userGroup);
        var handler       = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(new SecurityTokenDescriptor { Subject            = identity,
                                                                              Issuer             = _jwtSettings.Value.Issuer,
                                                                              Audience           = _jwtSettings.Value.Audience,
                                                                              IssuedAt           = _jwtSettings.Value.IssuedAt,
                                                                              NotBefore          = _jwtSettings.Value.NotBefore,
                                                                              Expires            = DateTime.UtcNow.AddMinutes(_jwtSettings.Value.ExpirationInMinutes),
                                                                              SigningCredentials = _jwtSettings.Value.SigningCredentials });

        var jwtToken = handler.WriteToken(securityToken);

        return new JwtToken(jwtToken, "bearer", _jwtSettings.Value.ExpirationInMinutes * 60);
    }
}