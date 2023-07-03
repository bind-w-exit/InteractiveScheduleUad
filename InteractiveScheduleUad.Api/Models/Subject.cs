namespace InteractiveScheduleUad.Api.Models;

public class Subject
{
    public int Id { get; set; }

    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}