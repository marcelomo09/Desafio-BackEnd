using Microsoft.AspNetCore.Mvc;

public class ProjectControllerBase: ControllerBase
{
    protected ActionResult SendResponseMessage(Response response)
    {
        switch (response.Result)
        {
            case ResponseTypeResults.NotFound  : return NotFound(response.Message);
            case ResponseTypeResults.BadRequest: return BadRequest(response.Message);
            default                            : return Ok(response.Message);
        }
    }
}