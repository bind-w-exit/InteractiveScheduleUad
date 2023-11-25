using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(ClassType), nameof(SubjectId), nameof(TeacherId), nameof(RoomId), IsUnique = true)]
public class Lesson
{
    public int Id { get; set; }

    public ClassType? ClassType { get; set; }

    // foreign keys

    public int SubjectId { get; set; }
    public int? TeacherId { get; set; }
    public int? RoomId { get; set; }

    // navigations

    [Required]
    public virtual Subject Subject { get; set; }

    public virtual Teacher? Teacher { get; set; }
    public virtual Room? Room { get; set; }
}