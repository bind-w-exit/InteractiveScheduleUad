namespace InteractiveScheduleUad.Api.Models.Dtos;

public class UserForLoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}