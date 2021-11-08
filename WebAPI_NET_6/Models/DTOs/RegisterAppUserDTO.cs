namespace WebAPI_NET_6.Models.DTOs;

public class RegisterAppUserDTO
{
    [Required]
    public string FullName { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string PasswordConfirmation { get; set; }
}
