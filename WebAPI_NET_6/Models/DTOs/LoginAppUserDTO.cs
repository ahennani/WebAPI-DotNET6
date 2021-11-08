namespace WebAPI_NET_6.Models.DTOs;

    public class LoginAppUserDTO
    {

    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}