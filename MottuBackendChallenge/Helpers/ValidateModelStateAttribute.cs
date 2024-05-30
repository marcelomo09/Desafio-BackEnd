using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

//TODO: Ao final excluir arquivo
public class ValidateModelStateAttribute:   ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errorsMsg = context.ModelState.Values.SelectMany(s => s.Errors).Select(e => e.ErrorMessage).ToList();

            context.Result = new BadRequestObjectResult(new { Errors = errorsMsg });
        }

        base.OnActionExecuting(context);
    }
}