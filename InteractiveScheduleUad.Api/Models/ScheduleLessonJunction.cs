using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models;

// aka contextualized lesson.Different from just lesson
[Index(nameof(FullContextId), IsUnique = true)]
public class ScheduleLessonJunction
{
    public int Id { get; set; }

    // foreign keys

    public int LessonId { get; set; }
    public int FullContextId { get; set; }

    // navigations

    [Required]
    public virtual Lesson Lesson { get; set; }

    [Required]
    public virtual FullContextJunction FullContext { get; set; }
}