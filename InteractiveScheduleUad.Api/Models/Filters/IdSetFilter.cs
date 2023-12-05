namespace InteractiveScheduleUad.Api.Models.Filters;

// generic filter for getting a subset of entities by their ids
public class IdSetFilter
{
    public int[] Id { get; set; }
}