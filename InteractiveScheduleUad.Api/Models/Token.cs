using System.ComponentModel.DataAnnotations;

namespace InteractiveScheduleUad.Api.Models;

public class Token
{
    [Required]
    public string Value { get; set; }
}