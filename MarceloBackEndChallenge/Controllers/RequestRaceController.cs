using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RequestRaceController : ProjectControllerBase
{
    private readonly RequestRaceService _requestRaceService;

    public RequestRaceController(RequestRaceService requestRaceService)
    {
        _requestRaceService = requestRaceService;
    }

    [HttpGet("GetAllRequestRace")]
    public async Task<ActionResult> GetAllRequestRace()
    {
        try
        {
            var response = await _requestRaceService.GetAll();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("GetAllNotificattionsDeliveryDrivers")]
    public async Task<ActionResult> GetAllNotificattionsDeliveryDrivers()
    {
        var response = await _requestRaceService.GetAllNotificattionsDeliveryDrivers();

        return Ok(response);
    }

    [Authorize(Roles = "admin")]
    [HttpPost("CreateRequestRace")]
    public async Task<ActionResult> CreateRequestRace([FromForm] CreateRequestReceRequest request)
    {
        var response = await _requestRaceService.Create(request);

        return SendResponseMessage(response);
    }

    [Authorize(Roles = "entregador")]
    [HttpPut("AcceptRequestRace")]
    public async Task<ActionResult> AcceptRequestRace(string idRequestRace, string cnh)
    {
        var response = await _requestRaceService.Accept(idRequestRace, cnh);

        return SendResponseMessage(response);
    }

    [Authorize(Roles = "entregador")]
    [HttpPut("DeliverRequestRace")]
    public async Task<ActionResult> DeliverRequestRace(string idRequestRace, string cnh)
    {
        var response = await _requestRaceService.Deliver(idRequestRace, cnh);

        return SendResponseMessage(response);
    }
}