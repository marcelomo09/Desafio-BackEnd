using Microsoft.EntityFrameworkCore;

public class AuthService : ServiceBase
{
    private readonly JwtService _jwtService;

    public AuthService(MongoDBContext mongoDBContext, JwtService jwtService) : base(mongoDBContext)
    {   
        _jwtService = jwtService;
    }

    public async Task<Response> Login(string userName, string password)
    {
        try
        {
            var passwordSHA256 = CryptoSHA256.ConvertStringToSHA256(password);

            // Busca o usuário
            var user = await _dbContext.Users.Where(u => u.UserName == userName && u.Password == passwordSHA256).FirstOrDefaultAsync();

            if (user == null) return new Response(true, "Usuário não encontrado.", ResponseTypeResults.NotFound);

            var token = _jwtService.CreateJwtToken(user.UserName);

            return new Response(false, $"{token.Type} {token.Access}");
        }
        catch (Exception ex)
        {
            return new Response(true, $"Ocorreu uma exeção no processo login: {ex.Message}", ResponseTypeResults.BadRequest);
        }
    }
}