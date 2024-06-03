using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "entregador")]
[ApiController]
[Route("api/[controller]")]
public class MotorcycleRentalController: ProjectControllerBase
{
    private readonly MotorcycleRentalService _motorcycleRentalService;

    public MotorcycleRentalController(MotorcycleRentalService motorcycleRentalService)
    {
        _motorcycleRentalService = motorcycleRentalService;
    }

    [HttpGet("GetAllMotorcycleRental")]
    public async Task<ActionResult> GetAllMotorcycleRental()
    {
        try
        {
            var response = await _motorcycleRentalService.GetAll();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpGet("SimulationOfMotorcycleRentalValues")]
    public ActionResult SimulationOfMotorcycleRentalValues([FromQuery] SimulationOfMotorcycleRentalValuesRequest request)
    {
        try
        {
            var response = _motorcycleRentalService.SimulationOfMotorcycleRentalValues(request);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPost("CreateMotorcycleRental")]
    public async Task<ActionResult> CreateMotorcycleRental([FromForm] CreateMotorcycleRentalRequest request)
    {
        try
        {
            var response = await _motorcycleRentalService.Create(request);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("FinalizeRental")]
    public async Task<ActionResult> FinalizeRental(string cnh, string plate)
    {
        try
        {
            var response = await _motorcycleRentalService.FinalizeRental(cnh, plate);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}