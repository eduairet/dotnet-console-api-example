using Microsoft.AspNetCore.Mvc;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace DotnetAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UtilsController(IConfiguration config) : ControllerBase
{
    private readonly DataContext _data = new(config);

    [AllowAnonymous]
    [HttpGet("test-connection")]
    public IActionResult TestConnection()
    {
        return Ok(DateTime.Now);
    }

    [AllowAnonymous]
    [HttpGet("test-connection-db")]
    public DateTime TestConnectionDb()
    {
        return _data.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpPost("populate-db")]
    public ActionResult<string> PopulateDB()
    {
        try
        {
            _data.PopulateAll();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        return "DB Populated";
    }
}