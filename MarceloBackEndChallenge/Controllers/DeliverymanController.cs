using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "entregador")]
[ApiController]
[Route("api/[controller]")]
public class DeliverymanController: ProjectControllerBase
{
    private readonly DeliverymanService _deliverymanService;

    public DeliverymanController(DeliverymanService deliverymanService)
    {
        _deliverymanService = deliverymanService;
    }

    [HttpGet("GetAllDeliveryRiders")]
    public async Task<ActionResult> GetAllDeliveryRiders()
    {
        var response = await _deliverymanService.GetAll();

        return Ok(response);
    }

    [HttpPost("CreateDeliveryman")]
    public async Task<ActionResult> CreateDeliveryman([FromForm] CreateDeliverymanRequest request)
    {
        var response = await _deliverymanService.Create(request);

        return SendResponseMessage(response);
    }

    [HttpDelete("DeleteDeliveryman")]
    public async Task<ActionResult> DeleteDeliveryman(string cnh)
    {
        var response = await _deliverymanService.Delete(cnh);

        return SendResponseMessage(response);
    }

    [HttpPut("UpdateImageCNHDeliveryman")]
    public async Task<ActionResult> UpdateImageCNHDeliveryman([FromForm] string cnh, IFormFile imageCNH)
    {
        var response = await _deliverymanService.UpdateImageCNH(cnh, imageCNH);

        return SendResponseMessage(response);
    }
}