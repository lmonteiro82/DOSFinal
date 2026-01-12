using Microsoft.AspNetCore.Mvc;

namespace DOSFinal.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatusController : ControllerBase
{
    /// <summary>
    /// Health check endpoint - identifies that the application is running
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult GetStatus()
    {
        return Ok(new
        {
            status = "healthy",
            application = "DOSFinal Restaurant Reservation API",
            version = "1.0.0",
            timestamp = DateTime.UtcNow
        });
    }
}
