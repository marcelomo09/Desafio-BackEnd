using Microsoft.AspNetCore.Mvc;

public class ProjectControllerBase: ControllerBase
{
    protected ActionResult SendResponseMessage(Response response)
    {
        switch (response.Result)
        {
            case ResponseTypeResults.NotFound  : return NotFound(response);
            case ResponseTypeResults.BadRequest: return BadRequest(response);
            default                            : return Ok(response);
        }
    }
}