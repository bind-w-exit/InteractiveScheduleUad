namespace InteractiveScheduleUad.Api.Models.Dtos;

public class UserForReadDto
{
    public required string Username { get; set; }
    public UserRole UserRole { get; set; }
}