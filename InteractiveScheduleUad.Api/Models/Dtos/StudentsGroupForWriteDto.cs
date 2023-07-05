namespace InteractiveScheduleUad.Api.Models.Dtos;

public class StudentsGroupForWriteDto
{
    public required string GroupName { get; set; }

    public WeekScheduleForReadDto? FirstWeekSchedule { get; set; }

    public WeekScheduleForReadDto? SecondWeekSchedule { get; set; }
}