using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

[ApiController]
[Route("api/[controller]")]
public class DeliverymanController: ControllerBase
{
    private readonly DeliverymanService _deliverymanService;
    private readonly IWebHostEnvironment _environment;

    public DeliverymanController(DeliverymanService srv, IWebHostEnvironment environment)
    {
        _deliverymanService = srv;
        _environment        = environment;
    }

    [HttpGet("GetDeliveryDrivers")]
    public async Task<ActionResult> GetDeliveryDrivers()
    {
        var drivers = await _deliverymanService.GetDeliveryDrivers();

        return Ok(drivers);
    }

    [HttpGet("GetDeliveryman/{id}")]
    public async Task<ActionResult> GetDeliveryman(string id)
    {
        var deliveryman = await _deliverymanService.GetDeliveryman(id);

        return deliveryman != null ? Ok(deliveryman) : NotFound("Entregador não encontra-se cadastrado!");
    }

    [HttpPost("CreateDeliveryman")]
    public async Task<ActionResult> CreateDeliveryman([FromForm] CreateDeliveryman deliveryman)
    {
        try
        {
            if (deliveryman.ImageCNH != null && deliveryman.ImageCNH.ContentType != "image/png" && deliveryman.ImageCNH.ContentType != "image/bmp") return BadRequest("Foto da CNH deve ser em .PNG ou .BMP!");

            string imagePath = await GetPathImageCNHPathAndSaveOrReplace(deliveryman.ImageCNH);

            if (string.IsNullOrEmpty(imagePath)) return BadRequest("A Foto da CNH e necessária para o cadastro.");

            Response response = await _deliverymanService.CreateDeliveryman(new Deliveryman(deliveryman, imagePath));

            return !response.Error ? Ok(response.Message) : BadRequest(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpPut("UpdateDeliveryman")]
    public async Task<ActionResult> UpdatDeliveryman([FromForm] UpdateDeliveryman deliveryman)
    {
        try
        {
            if (deliveryman.ImageCNH != null && deliveryman.ImageCNH.ContentType != "image/png" && deliveryman.ImageCNH.ContentType != "image/bmp") return BadRequest("Foto da CNH deve ser em .PNG ou .BMP!");

              string imagePath = await GetPathImageCNHPathAndSaveOrReplace(deliveryman.ImageCNH);

            Response response = await _deliverymanService.UpdateDeliveryman(new Deliveryman(deliveryman, imagePath));

            return !response.Error ? Ok(response.Message) : NotFound(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    [HttpDelete("DeleteDeliveryman")]
    public async Task<ActionResult> DeleteDeliveryman(string id)
    {
        try
        {
            Response response = await _deliverymanService.DeleteDeliveryman(id);

            return !response.Error ? Ok(response.Message) : NotFound(response.Message);
        }
        catch (Exception ex)
        {
            return BadRequest($"Ocorreu uma exceção durante o processamento: {ex.Message}");
        }
    }

    private async Task<string> GetPathImageCNHPathAndSaveOrReplace(IFormFile? file)
    {
        if (file == null || file.Length == 0) return string.Empty;

        string filepath = Path.Combine(Directory.GetCurrentDirectory(), "UploadsCNH", file.FileName);

        if (System.IO.File.Exists(filepath))
        {
            System.IO.File.Delete(filepath);    
        }

        using (var stream = new FileStream(filepath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return filepath;
    }
}