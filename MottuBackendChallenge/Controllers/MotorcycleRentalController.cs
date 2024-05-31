using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MotorcycleRentalController : ControllerBase
{
    private readonly MotorcycleRentalService _motorcycleRentalService;

    public MotorcycleRentalController(MotorcycleRentalService srv)
    {
        _motorcycleRentalService = srv;
    }

    [HttpGet("GetMotorcycleRentals")]
    public async Task<ActionResult> GetMotorcycleRentals()
    {
        var rentals = await _motorcycleRentalService.GetMotorcycleRentals();

        return Ok(rentals);
    }

    [HttpGet("GetMotorcycleRental/{id}")]
    public async Task<ActionResult> GetMotorcycleRental(string id)
    {
        var rental = await _motorcycleRentalService.GetMotorcycleRental(id);

        return rental != null ? Ok(rental) : NotFound("Locação não encontrada!");
    }

    [HttpGet("ConsultValueForMotorcycleRental")]
    public async Task<ActionResult> ConsultValueForMotorcycleRental([FromQuery] ConsultValueForMotorcycleRentalRequest request)
    {
        var response = await _motorcycleRentalService.ConsultValueForMotorcycleRental(request);

        return !response.Error ? Ok(response.Message) : BadRequest(response.Message);
    }

    [HttpPost("CreateMotorcycleRental")]
    public async Task<ActionResult> CreateMotorcycleRental([FromForm] CreateMotorcycleRentalRequest request)
    {   
        try
        {
            Response response = await _motorcycleRentalService.CreateMotorcycleRental(request);

            switch (response.Result)
            {
                case ResponseTypeResults.NotFound  : return NotFound(response.Message);
                case ResponseTypeResults.BadRequest: return BadRequest(response.Message);
                default                            : return Ok(response.Message);
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteMotorcycleRental/{id}")]
    public async Task<ActionResult> DeleteMotorcycleRental(string id)
    {
        try
        {
            Response response = await _motorcycleRentalService.DeleteMotorcycleRental(id);

            return !response.Error ? Ok(response.Message) : BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}