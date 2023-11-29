namespace DotnetAPI.Dtos;

public partial class UserForLoginConfirmationDto
{
    public byte[] PasswordHash { get; set; } = [0];
    public byte[] PasswordSalt { get; set; } = [0];
}