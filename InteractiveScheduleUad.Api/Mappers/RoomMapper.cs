using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using Riok.Mapperly.Abstractions;

namespace InteractiveScheduleUad.Api.Mappers;

[Mapper]
public static partial class RoomMapper
{
    [MapperIgnoreSource(nameof(Room.Lesson))]
    [MapperIgnoreSource(nameof(Room.LessonId))]
    public static partial RoomForReadDto RoomToRoomForReadDto(Room room);
}