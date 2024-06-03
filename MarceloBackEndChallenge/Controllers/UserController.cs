using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class UserController: ProjectControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService requestRaceService)
    {
        _userService = requestRaceService;
    }

    [HttpGet("GetAllUsers")]
    public async Task<ActionResult> GetAllUsers()
    {   
        var  response = await _userService.GetAll();

        return Ok(response);
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser([FromForm] CreateUserRequest request)
    {
        var response = await _userService.Create(request);

        return SendResponseMessage(response);
    }

    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(string userName)
    {
        var response = await _userService.Delete(userName);

        return SendResponseMessage(response);
    }
}