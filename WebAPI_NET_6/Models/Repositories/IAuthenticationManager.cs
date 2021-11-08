namespace WebAPI_NET_6.Models.Repositories;

public interface IAuthenticationManager
{
    Task<IdentityResult> RegisterAppUserAsync(RegisterAppUserDTO user);
    Task<bool> LoginAppUserAsync(LoginAppUserDTO user);
    Task<AppUser> GetAppUserByEmail(string email);
    Task<AuthResult> GetSignedInAppUserToken(string email);
    Task<AuthResult> GetSignedInAppUserToken(AppUser user);
}

