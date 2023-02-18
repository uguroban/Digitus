using Digitus.Models;
using Microsoft.AspNetCore.Mvc;

namespace Digitus.Controllers;

public class BaseController
{
    protected IActionResult CreateActionResult<T>(Response<T>? response)
    {
        return new ObjectResult(response)
        {
            StatusCode = response?.StatusCode
        };
    }
}