public class Response
{
    public bool Error { get; set; }
    public string Message { get; set; }

    public ResponseTypeResults Result { get; set; }

    public Response()
    {
        Error   = false;
        Message = string.Empty;
        Result  = ResponseTypeResults.Ok;
    }

    public Response(bool error, string message, ResponseTypeResults result)
    {
        Error   = error;
        Message = message;
        Result  = result;
    }
    
    public Response(bool error, string message)
    {
        Error   = error;
        Message = message;
        Result  = ResponseTypeResults.Ok;
    }

    public void SetError(string message, ResponseTypeResults result)
    {
        Error   = true;
        Message = message;
        Result  = result;
    }
}

public enum ResponseTypeResults
{
    Ok,
    BadRequest,
    NotFound
}