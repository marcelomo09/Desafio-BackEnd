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
        var response = await _motorcycleRentalService.GetAll();

        return Ok(response);
    }

    [HttpGet("SimulationOfMotorcycleRentalValues")]
    public ActionResult SimulationOfMotorcycleRentalValues([FromQuery] SimulationOfMotorcycleRentalValuesRequest request)
    {
        var response = _motorcycleRentalService.SimulationOfMotorcycleRentalValues(request);

        return SendResponseMessage(response);
    }

    [HttpPost("CreateMotorcycleRental")]
    public async Task<ActionResult> CreateMotorcycleRental([FromForm] CreateMotorcycleRentalRequest request)
    {
        var response = await _motorcycleRentalService.Create(request);

        return SendResponseMessage(response);
    }

    [HttpPut("FinalizeRental")]
    public async Task<ActionResult> FinalizeRental(string cnh, string plate)
    {
        var response = await _motorcycleRentalService.FinalizeRental(cnh, plate);

        return SendResponseMessage(response);
    }
}