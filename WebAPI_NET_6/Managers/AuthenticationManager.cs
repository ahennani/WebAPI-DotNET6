using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebAPI_NET_6.Managers;

public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<AuthenticationManager> _logger;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthenticationManager
        (
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            ILogger<AuthenticationManager> logger,
            IConfiguration configuration
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
    }


    public async Task<IdentityResult> RegisterAppUserAsync(RegisterAppUserDTO model)
    {
        var userApp = _mapper.Map<AppUser>(model);

        var result = await _userManager.CreateAsync(userApp, model.Password);

        await _userManager.AddToRoleAsync(userApp, Roles.User.ToString());

        return result;
    }

    public async Task<bool> LoginAppUserAsync(LoginAppUserDTO model)
    {
        var user = await GetAppUserByEmail(model.Email);

        return await _userManager.CheckPasswordAsync(user, model.Password);
    }

    public Task<AppUser> GetAppUserByEmail(string email) => _userManager.FindByEmailAsync(email);

    public async Task<AuthResult> GetSignedInAppUserToken(string email)
    {
        var appUser = await GetAppUserByEmail(email);

        return await GenerateTokenAsync(appUser);
    }

    public Task<AuthResult> GetSignedInAppUserToken(AppUser user) => GenerateTokenAsync(user);



    #region Private Fields

    private async Task<AuthResult> GenerateTokenAsync(AppUser appUser)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.GetSection("secret").Value));
        var claims = await GetClaimsAsync(appUser);

        var securyDesc = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256),
            Expires = DateTime.UtcNow.AddMinutes(30)
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(securyDesc);

        return new AuthResult()
        {
            FullName = appUser.FullName,
            Username = appUser.UserName,
            Expires = securityToken.ValidTo,
            Token = handler.WriteToken(securityToken)
        };
    }

    private async Task<IList<Claim>> GetClaimsAsync(AppUser appUser)
    {
        var claims = await _userManager.GetClaimsAsync(appUser);
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, appUser.UserName));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, appUser.Email));

        var roles = await _userManager.GetRolesAsync(appUser);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    #endregion
}