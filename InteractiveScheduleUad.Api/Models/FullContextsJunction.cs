using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models;

[Index(nameof(StudentsGroupId), nameof(TimeContextId), IsUnique = true)]
public class FullContextJunction
{
    public int Id { get; set; }

    public int StudentsGroupId { get; set; }
    public int TimeContextId { get; set; }

    [Required]
    public virtual StudentsGroup StudentsGroup { get; set; }

    [Required]
    public virtual TimeContext TimeContext { get; set; }
}