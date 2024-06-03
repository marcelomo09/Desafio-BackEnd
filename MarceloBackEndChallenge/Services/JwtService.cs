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

    private static ClaimsIdentity GetClaimsIdentity(string userName)
    {
        List<Claim> claims = new List<Claim>();

        claims.Add(new Claim("jti"          , Guid.NewGuid().ToString()));
        claims.Add(new Claim("sub"          , userName));
        claims.Add(new Claim(ClaimTypes.Role, userName));

        return new ClaimsIdentity(new GenericIdentity(userName), claims);
    } 

    public JwtToken CreateJwtToken(string userName)
    {
        var identity      = GetClaimsIdentity(userName);
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