namespace InteractiveScheduleUad.Api.Models.Dtos;

public class StudentsGroupForWriteDto
{
    public string GroupName { get; set; }

    public WeekScheduleForReadDto FirstWeekSchedules { get; set; }

    public WeekScheduleForReadDto SecondWeekSchedules { get; set; }
}