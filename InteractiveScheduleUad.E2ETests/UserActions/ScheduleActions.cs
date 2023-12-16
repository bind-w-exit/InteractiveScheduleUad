using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Constants;
using InteractiveScheduleUad.Core.Utils;
using RestSharp;
using InteractiveScheduleUad.Core.Extensions;
using Newtonsoft.Json;

namespace InteractiveScheduleUad.E2ETests.UserActions;

public static class ScheduleActions
{
    public static void CreateAllScheduleLessonsOfSchedule(RestClient client, string groupName, ScheduleFile scheduleFile)
    {
        // to create a schedule, you need to:
        // 1. create a group
        // 2. create schedule lessons:
        // 2.1 create all teachers, subjects, rooms first.
        // 2.2 create base lessons
        // 2.3 create schedule lessons that map base schedules to full contexts: groups and time contexts

        // Arrange

        // read "raw" schedule from file
        //var fileName = $"{groupName}.json";
        //var pathToScheduleFile = @$"Data\{fileName}";
        //var scheduleFile = ReadRawScheduleFromFile(pathToScheduleFile);

        // Act

        // POST new group
        var groupForWrite = new StudentsGroupForWriteDto { Name = groupName };
        var studentsGroup = client.PostJson<StudentsGroupForWriteDto, StudentsGroupForReadDto>(
            ApiEndpoints.groupsEndpoint, groupForWrite
            );
        var studentsGroupId = studentsGroup.Id;

        // iterate over raw data and create schedule lessons

        var scheduleFileProperties = typeof(ScheduleFile).GetProperties();
        foreach (var day in scheduleFileProperties)
        {
            string dayName = day.Name;
            var daySchedule = (Day)day.GetValue(scheduleFile);

            var dayNameCapitalized = string.Concat(dayName.First().ToString().ToUpper(), dayName.AsSpan(1));
            DayOfWeek dayOfWeek = Enum.Parse<DayOfWeek>(dayNameCapitalized);

            foreach (var rawClass in daySchedule.classes)
            {
                var scheduleLesson = CreateCompleteScheduleLesson(rawClass, groupName, dayOfWeek, client);
            }
        }
    }

    // POSTS a complete schedule lesson with related entities pre-created in advance
    public static ScheduleLessonForReadDto CreateCompleteScheduleLesson(
        ScheduleClass rawScheduleClass,
        string groupName,
        DayOfWeek weekDay,
        RestClient client)
    {
        // construct fields of full context
        TimeContextForWriteDto timeContext = new()
        {
            LessonIndex = rawScheduleClass.index,
            WeekDay = weekDay,
            WeekIndex = rawScheduleClass.week ?? 0,
        };

        var groupForWrite = new StudentsGroupForWriteDto { Name = groupName };
        var group = client.EnsureExists<StudentsGroup, StudentsGroupForWriteDto>(
            ApiEndpoints.groupsEndpoint, null, groupForWrite, (g) => g.Name == groupName);
        var groupId = group.Id;

        FullContextForWriteDto fullContext = new()
        {
            StudentsGroupId = groupId,
            TimeContext = timeContext
        };

        // create a base lesson and obtain its Id

        var newLesson = CreateCompleteLesson(rawScheduleClass, client);
        var newLessonId = newLesson.Id;

        // construct final schedule lesson

        var scheduleLessonForWrite = new ScheduleLessonForWriteDto
        {
            FullContext = fullContext,
            LessonId = newLessonId
        };

        // POST the schedule lesson
        var responseData = client.PostJson
            <ScheduleLessonForWriteDto, ScheduleLessonForReadDto>
            (ApiEndpoints.scheduleLessonsEndpoint, scheduleLessonForWrite);

        return responseData;
    }

    // POSTS a complete lesson with related entities pre-created in advance
    public static LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass, RestClient client)
    {
        LessonForWriteDto requestBody;
        LessonForReadDto responseData;

        // pre-create necessary related entities (if they don't exist already)
        int? teacherId = null;
        if (rawScheduleClass.teacher is not null)
        {
            TeacherForWriteDto teacherForWrite = new() { LastName = rawScheduleClass.teacher, FirstName = "" };
            var teacher = client.EnsureExists<TeacherForReadDto, TeacherForWriteDto>(
                ApiEndpoints.teachersEndpoint, null, teacherForWrite, (t) => t.LastName == rawScheduleClass.teacher);

            teacherId = teacher.Id;
        }

        var subjectForWrite = new Subject { Name = rawScheduleClass.name };
        var subject = client.EnsureExists<Subject, Subject>(
            ApiEndpoints.subjectsEndpoint, null, subjectForWrite, (s) => s.Name == rawScheduleClass.name);
        var subjectId = subject.Id;

        int? roomId = null;
        if (rawScheduleClass.room is not null)
        {
            var roomForWrite = new RoomForWriteDto { Name = rawScheduleClass.room };
            var room = client.EnsureExists<Room, RoomForWriteDto>(
                ApiEndpoints.roomsEndpoint, null, roomForWrite, (r) => r.Name == rawScheduleClass.room);
            roomId = room.Id;
        }

        // assemble final request
        requestBody = new()
        {
            RoomId = roomId,
            SubjectId = subjectId,
            TeacherId = teacherId
        };

        // POST the lesson with entities

        responseData = client.EnsureExists
            <LessonForReadDto, LessonForWriteDto>(
            ApiEndpoints.lessonsEndpoint,
            null,
            requestBody,
            (l) => l.Subject.Id == subjectId && l.Room?.Id == roomId && l.Teacher?.Id == teacherId);

        return responseData;
    }

    public static ScheduleFile ReadRawExampleScheduleFromFile()
    {
        string pathToRawExampleScheduleFile = @"Data\ІСТ-5.json";
        return ReadRawScheduleFromFile(pathToRawExampleScheduleFile);
    }

    public static ScheduleFile ReadRawScheduleFromFile(string pathToFile)
    {
        var rawScheduleText = File.ReadAllText(pathToFile);
        var rawScheduleObj = JsonConvert.DeserializeObject<ScheduleFile>(rawScheduleText)
            ?? throw new Exception("Failed to deserialize raw schedule file: result is null");

        return rawScheduleObj;
    }
}