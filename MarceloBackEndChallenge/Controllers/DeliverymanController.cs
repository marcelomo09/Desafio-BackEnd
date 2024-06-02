using Microsoft.AspNetCore.Mvc;

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
        try
        {
            var response = await _deliverymanService.GetAll();

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPost("CreateDeliveryman")]
    public async Task<ActionResult> CreateDeliveryman([FromForm] CreateDeliverymanRequest request)
    {
        try
        {
            var response = await _deliverymanService.Create(request);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteDeliveryman")]
    public async Task<ActionResult> DeleteDeliveryman(string cnh)
    {
        try
        {
            var response = await _deliverymanService.Delete(cnh);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("UpdateImageCNHDeliveryman")]
    public async Task<ActionResult> UpdateImageCNHDeliveryman([FromForm] string cnh, IFormFile imageCNH)
    {
        try
        {
            var response = await _deliverymanService.UpdateImageCNH(cnh, imageCNH);

            return SendResponseMessage(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }
}