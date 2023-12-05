using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(LessonIndex), nameof(WeekDay), nameof(WeekIndex), IsUnique = true)]
public class TimeContext : Entity
{
    public int Id { get; set; }

    [Range(0, 5)]
    public int LessonIndex { get; set; }

    public DayOfWeek WeekDay { get; set; }

    // 0 means the lesson happens every week
    [Range(0, 3)]
    public int WeekIndex { get; set; }
}