using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.Api.Models.Dtos;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class UserMapper
{
    [MapperIgnoreSource(nameof(User.Id))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.PasswordSalt))]
    public static partial UserForReadDto UserToUserForReadDto(User user);

    [MapperIgnoreSource(nameof(User.Id))]
    [MapperIgnoreSource(nameof(User.PasswordHash))]
    [MapperIgnoreSource(nameof(User.PasswordSalt))]
    public static partial void UserToUserForReadDto(User user, UserForReadDto userForReadDto);
}