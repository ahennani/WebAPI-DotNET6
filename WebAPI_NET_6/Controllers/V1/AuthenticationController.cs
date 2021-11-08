namespace WebAPI_NET_6.Controllers.V1;

[Route("api/auth")]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Authorize]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationManager _authenticationManager;

    public AuthenticationController(IAuthenticationManager authenticationManager)
    {
        _authenticationManager = authenticationManager;
    }


    // POST: /api/auth/register
    /// <summary>
    ///     Register new application user.
    /// </summary>
    /// <remarks>
    /// Scheme EXP:
    /// 
    ///     {
    ///         "fullname": "fullname-01"
    ///         "email": "username01@gmail.com"
    ///         "password": "username01"
    ///         "passwordConfirmation": "username01"
    ///     }
    /// </remarks>
    /// <param name="register"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(statusCode: StatusCodes.Status201Created, type: typeof(AuthResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status400BadRequest, type: typeof(List<ErrorResult>))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterAppUserDTO register)
    {
        if (ModelState.IsValid is false)
            return BadRequest();

        var result = await _authenticationManager.RegisterAppUserAsync(register);

        if (result.Succeeded is false)
        {
            var errorResult = new List<ErrorResult>();
            foreach (var error in result.Errors)
            {
                errorResult.Add(new ErrorResult() { ErrorCode = error.Code, Description = error.Description });
            }

            return BadRequest(errorResult);
        }

        var authResult = await _authenticationManager.GetSignedInAppUserToken(register.Email);

        return CreatedAtAction(nameof(Login), authResult);
    }

    // POST: /api/auth/login
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(AuthResult))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound, type: typeof(string))]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Login(LoginAppUserDTO login)
    {
        var user = await _authenticationManager.GetAppUserByEmail(login.Email);
        var isUserWithPassword = await _authenticationManager.LoginAppUserAsync(login);
        if (user is null || isUserWithPassword is false)
            return NotFound("Email or password is not correct.");

        var authResult = await _authenticationManager.GetSignedInAppUserToken(user);

        return Ok(authResult);
    }
}