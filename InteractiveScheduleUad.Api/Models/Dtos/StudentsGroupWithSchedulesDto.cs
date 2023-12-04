namespace InteractiveScheduleUad.Api.Models.Dtos;

public class StudentsGroupWithSchedulesDto
{
    public string Name { get; set; }

    public WeekScheduleForReadDto? FirstWeekSchedule { get; set; }

    public WeekScheduleForReadDto? SecondWeekSchedule { get; set; }
}