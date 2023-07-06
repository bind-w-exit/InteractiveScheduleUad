namespace InteractiveScheduleUad.Api.Models.Dtos;

public class UserForRegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public UserRole UserRole { get; set; }
}