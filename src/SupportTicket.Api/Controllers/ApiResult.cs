using Microsoft.AspNetCore.Mvc;
using SupportTicket.Api.DTOs.Responses;
using SupportTicket.Api.Services.Common;

namespace SupportTicket.Api.Controllers;

public static class ApiResult
{
    public static ActionResult<T> From<T>(ServiceResult<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        var error = new ErrorResponse(result.Error!, result.Code);

        if (result.IsNotFound)
        {
            return new NotFoundObjectResult(error);
        }

        return new BadRequestObjectResult(error);
    }

    public static ActionResult<T> Created<T>(string location, ServiceResult<T> result)
    {
        if (result.IsSuccess)
        {
            return new CreatedResult(location, result.Value);
        }

        var error = new ErrorResponse(result.Error!, result.Code);

        if (result.IsNotFound)
        {
            return new NotFoundObjectResult(error);
        }

        return new BadRequestObjectResult(error);
    }
}
