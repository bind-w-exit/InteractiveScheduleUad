﻿namespace InteractiveScheduleUad.Api.Models;

public class StudentsGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; }

    public WeekSchedule? FirstWeekSchedule { get; set; }

    public WeekSchedule? SecondWeekSchedule { get; set; }
}