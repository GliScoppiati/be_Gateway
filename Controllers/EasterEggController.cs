using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("sono-pazzo")]
public class EasterEggController : ControllerBase
{
    [HttpGet]
    public IActionResult CheckSanity([FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new
            {
                message = "😵‍💫 Serve almeno un nome per sapere se sei pazzo..."
            });
        }

        name = name.Trim().ToLowerInvariant();

        var message = name switch
        {
            "nromito" => "🧘 Va tutto bene Nico, tranquillo",
            _ => "🤔 È una domanda strana da fare qua. Vuoi un cocktail?"
        };

        return Ok(new
        {
            name,
            verdict = message,
            timestamp = DateTime.UtcNow
        });
    }
}
