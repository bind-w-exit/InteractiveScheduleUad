namespace InteractiveScheduleUad.Api.Models.Dtos;

public class StudentsGroupForWriteDto
{
    public string GroupName { get; set; }

    public WeekScheduleForReadDto? FirstWeekSchedule { get; set; }

    public WeekScheduleForReadDto? SecondWeekSchedule { get; set; }
}