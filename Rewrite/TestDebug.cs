using Microsoft.AspNetCore.Mvc.Infrastructure;

public class MyService
{
    private readonly IActionContextAccessor _actionContextAccessor;

    public MyService(IActionContextAccessor actionContextAccessor)
    {
        _actionContextAccessor = actionContextAccessor;
    }
}