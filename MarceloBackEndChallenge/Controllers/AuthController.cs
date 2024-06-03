using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController: ProjectControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("Login")]
    public async Task<ActionResult> Login(string userName, string password)
    {
        var response = await _authService.Login(userName, password);

        return SendResponseMessage(response);
    }
}