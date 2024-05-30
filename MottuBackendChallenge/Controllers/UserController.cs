using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Xunit.Sdk;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService srv)
    {
        _userService = srv;
    }

    [HttpGet("GetUsers")]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _userService.GetUsers();

        return Ok(users);
    }

    [HttpGet("GetUser/{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        var user = await _userService.GetUser(id);

        return user != null ? Ok(user) : NotFound("Não encontrado!");
    }

    [HttpPost("CreateUser")]
    public async Task<ActionResult> CreateUser(CreateUserParams user)
    {
        try
        {
            await _userService.CreateUser(new User(user));

            return Ok("Usuário criado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("UpdateUser")]
    public async Task<ActionResult> UpdateUser(User user)
    {
        try
        {
            if (user.Id == string.Empty)
            {
                return BadRequest("Id é um campo obrigatório!");
            }

            await _userService.UpdateUser(user);

            return Ok("Usuário atualizado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteUser")]
    public async Task<ActionResult> DeleteUser(string id)
    {
        try
        {
            var user = await _userService.GetUser(id);

            if (user == null) return NotFound("Usuário não encontrado pelo ID informado!");

            await _userService.DeleteUser(id);

            return Ok("Usuário excluído com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}