namespace InteractiveScheduleUad.Api.Models;

public class StudentsGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; }

    public WeekSchedule? FirstWeekSchedules { get; set; }

    public WeekSchedule? SecondWeekSchedules { get; set; }
}