using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Constants;
using InteractiveScheduleUad.Core.Utils;
using RestSharp;
using InteractiveScheduleUad.Core.Extensions;
using Newtonsoft.Json;
using AutoFilterer.Types;
using InteractiveScheduleUad.Api.Models.Filters;

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

        var groupFilter = new StudentsGroupForReadDtoFilter { Name = groupName };
        var groupFilterSerialized = JsonConvert.SerializeObject(groupFilter);

        var group = client.EnsureExists<StudentsGroup, StudentsGroupForWriteDto>(
                       ApiEndpoints.groupsEndpoint, null, groupForWrite, groupFilterSerialized);

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

            TeacherForReadDtoFilter teacherFilter = new() { LastName = rawScheduleClass.teacher };
            string teacherFilterSerialized = JsonConvert.SerializeObject(teacherFilter);

            var teacher = client.EnsureExists<Teacher, TeacherForWriteDto>(
                               ApiEndpoints.teachersEndpoint, null, teacherForWrite, teacherFilterSerialized);

            teacherId = teacher.Id;
        }

        // create subject

        var subjectForWrite = new Subject { Name = rawScheduleClass.name };

        SubjectForReadDtoFilter subjectFilter = new() { Name = rawScheduleClass.name };
        string subjectFilterSerialized = JsonConvert.SerializeObject(subjectFilter);

        var subject = client.EnsureExists<Subject, Subject>(
                       ApiEndpoints.subjectsEndpoint, null, subjectForWrite, subjectFilterSerialized);

        var subjectId = subject.Id;

        // create room

        int? roomId = null;
        if (rawScheduleClass.room is not null)
        {
            var roomForWrite = new RoomForWriteDto { Name = rawScheduleClass.room };

            RoomForReadDtoFilter roomFilter = new() { Name = rawScheduleClass.room };
            string roomFilterSerialized = JsonConvert.SerializeObject(roomFilter);

            var room = client.EnsureExists<Room, RoomForWriteDto>(
                ApiEndpoints.roomsEndpoint, null, roomForWrite, roomFilterSerialized);

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

        var lessonFilter = new LessonForReadDtoFilter
        {
            Subject = new SubjectForReadDtoFilter { Id = subjectId },
            Room = new RoomForReadDtoFilter { Id = roomId },
            Teacher = new TeacherForReadDtoFilter { Id = teacherId }
        };

        var lessonFilterSerialized = JsonConvert.SerializeObject(lessonFilter);

        responseData = client.EnsureExists
            <LessonForReadDto, LessonForWriteDto>(
            ApiEndpoints.lessonsEndpoint,
            null,
            requestBody,
            lessonFilterSerialized);

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