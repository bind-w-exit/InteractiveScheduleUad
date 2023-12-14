using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests.Models;

// classes for deserialization of schedule file

public class ScheduleClass
{
    public int index { get; set; }
    public string name { get; set; }
    public string room { get; set; }
    public string qualification { get; set; }
    public string teacher { get; set; }
    public double? label { get; set; }
    public bool? isBiweekly { get; set; }
    public int? week { get; set; }
}

public class Day
{
    public List<ScheduleClass> classes { get; set; }
}

public class ScheduleFile
{
    public Day monday { get; set; }
    public Day tuesday { get; set; }
    public Day wednesday { get; set; }
    public Day thursday { get; set; }
    public Day friday { get; set; }

    // note: copilot is cool
    public int GetAllClassesCount()
    {
        return monday.classes.Count +
               tuesday.classes.Count +
               wednesday.classes.Count +
               thursday.classes.Count +
               friday.classes.Count;
    }
}