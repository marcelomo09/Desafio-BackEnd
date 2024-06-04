using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "admin")]
[ApiController]
[Route("api/[controller]")]
public class MotorcycleController: ProjectControllerBase
{
    private readonly MotorcycleService _motorcycleService;

    public MotorcycleController(MotorcycleService motorcycleService)
    {
        _motorcycleService = motorcycleService;
    }

    [HttpGet("GetAllMotorcycles")]
    public async Task<ActionResult> GetAllMotorcycles()
    {
        var response = await _motorcycleService.GetAll();

        return Ok(response);
    }

    [HttpGet("GetMotorcycleByPlate")]
    public async Task<ActionResult> GetMotorcycleByPlate([FromQuery] string plate)
    {
        var response = await _motorcycleService.GetMotorcycleByPlate(plate);

        return !string.IsNullOrEmpty(response.Plate) ? Ok(response) : NotFound("Moto não encontrada.");
    }

    [HttpPost("CreateMotorcycle")]
    public async Task<ActionResult> CreateMotorcycle([FromForm] CreateMotorcycleRequest request)
    {
        var response = await _motorcycleService.Create(request);

        return SendResponseMessage(response);
    }

    [HttpDelete("DeleteMotorcycle")]
    public async Task<ActionResult> DeleteMotorcycle(string plate)
    {
        var response = await _motorcycleService.Delete(plate);

        return SendResponseMessage(response);
    }

    [HttpPut("UpdateMotorcyclePlate")]
    public async Task<ActionResult> UpdateMotorcyclePlate(string oldPlate, string newPlate)
    {
        var response = await _motorcycleService.UpdatePlate(oldPlate, newPlate);

        return SendResponseMessage(response);
    }
}