namespace WebAPI_NET_6.Controllers.V1;

[Route("api/auth")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
public class AuthenticationController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult RegisterUser()
    {
        return Ok();
    }
}