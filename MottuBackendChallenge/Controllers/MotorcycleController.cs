using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MotorcycleController: ControllerBase
{
    private readonly MotorcycleService _motorcycleService;

    public MotorcycleController(MotorcycleService srv)
    {
        _motorcycleService = srv;
    }

    [HttpGet("GetMotorcycles")]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> Get()
    {
        var motors = await _motorcycleService.GetMotorcycles();

        return Ok(motors);
    }

    [HttpGet("GetMotorcycle/{id}")]
    public async Task<ActionResult<IEnumerable<Motorcycle>>> GetMotorcycle(string id)
    {
        var motors = await _motorcycleService.GetMotorcycle(id);

        return motors != null ? Ok(motors) : NotFound("Moto não encontrada!");
    }

    [HttpGet("GetMotorcycleForPlate")]
    public async Task<ActionResult> GetMotorcycleForPlate(string plate)
    {
        var motors = await _motorcycleService.GetMotorcycleForPlate(plate);

        return motors != null ? Ok(motors) : NotFound("Moto não encontrada!");
    }

    [HttpPost("CreateMotorcycle")]
    public async Task<ActionResult> CreateMotorcycle(CreateMotorcycleRequest motorcycle)
    {
        try
        {
            Response response = await _motorcycleService.CreateMotorcycle(new Motorcycle(motorcycle));

            return !response.Error ? Ok(response.Message) : BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("UpdateMotorcycle")]
    public async Task<ActionResult> UpdateMotorcycle(Motorcycle motorcycle)
    {
        try
        {
            Response response = await _motorcycleService.UpdateMotorcycle(motorcycle);

            switch (response.Result)
            {
                case ResponseTypeResults.BadRequest: return BadRequest(response.Message);
                case ResponseTypeResults.NotFound  : return NotFound(response.Message);
                default: return Ok(response.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteMotorcycle")]
    public async Task<ActionResult> DeleteMotorcycle(string id)
    {
        try
        {
            var response = await _motorcycleService.DeleteMotorcycle(id);

            return !response.Error ? Ok(response.Message) : NotFound(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}