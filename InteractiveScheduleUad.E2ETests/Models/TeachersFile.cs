using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace InteractiveScheduleUad.E2ETests.Models;

// classes for deserialization of teachers file

internal class TeachersFile : List<RawTeacher>
{
}

public class RawTeacher
{
    [JsonProperty("№")]
    public int? Number { get; set; }

    [JsonProperty("ПІБ викладача")]
    public string FullName { get; set; }

    [JsonProperty("Кваліфікаця")]
    public string Qualification { get; set; }

    [JsonProperty("E-mail")]
    public string Email { get; set; }

    [JsonProperty("Кафедра Абревіатура")]
    public string DepartmentAbbreviation { get; set; }

    [JsonProperty("Кафедра Повна назва")]
    public string DepartmentFullName { get; set; }

    [JsonProperty("Кафедра Посилання")]
    public string DepartmentLink { get; set; }
}