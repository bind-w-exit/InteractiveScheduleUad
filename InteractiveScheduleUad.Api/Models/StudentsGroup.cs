using Microsoft.EntityFrameworkCore;
using System;

namespace InteractiveScheduleUad.Api.Models;

public class StudentsGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; }

    public virtual IEnumerable<FullContextJunction>? FullContexts { get; set; }
}