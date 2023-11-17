using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteractiveScheduleUad.E2ETests.Models;

internal class Class
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

internal class Day
{
    public List<Class> classes { get; set; }
}

internal class ScheduleFile
{
    public Day monday { get; set; }
    public Day tuesday { get; set; }
    public Day wednesday { get; set; }
    public Day thursday { get; set; }
    public Day friday { get; set; }
}