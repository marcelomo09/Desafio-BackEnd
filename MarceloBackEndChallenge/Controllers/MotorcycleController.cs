using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var response = await _motorcycleService.GetAll();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpGet("GetMotorcycleByPlate")]
    public async Task<ActionResult> GetMotorcycleByPlate([FromQuery] string plate)
    {
        try
        {
            var response = await _motorcycleService.GetMotorcycleByPlate(plate);

            return !string.IsNullOrEmpty(response.Plate) ? Ok(response) : NotFound("Moto não encontrada.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPost("CreateMotorcycle")]
    public async Task<ActionResult> CreateMotorcycle([FromForm] CreateMotorcycleRequest request)
    {
        try
        {
            var response = await _motorcycleService.Create(request);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteMotorcycle")]
    public async Task<ActionResult> DeleteMotorcycle(string plate)
    {
        try
        {
            var response = await _motorcycleService.Delete(plate);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("UpdateMotorcyclePlate")]
    public async Task<ActionResult> UpdateMotorcyclePlate(string oldPlate, string newPlate)
    {
        try
        {
            var response = await _motorcycleService.UpdatePlate(oldPlate, newPlate);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}