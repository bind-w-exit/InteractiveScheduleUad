using InteractiveScheduleUad.Api.Models.Dtos;
using InteractiveScheduleUad.Api.Models;
using InteractiveScheduleUad.E2ETests.Models;
using InteractiveScheduleUad.E2ETests.Constants;
using InteractiveScheduleUad.Core.Utils;
using RestSharp;
using InteractiveScheduleUad.Core.Extensions;

namespace InteractiveScheduleUad.E2ETests.UserActions;

// TODO: implement
public static class ScheduleActions
{
    // POSTS a complete lesson with related entities pre-created in advance
    public static LessonForReadDto CreateCompleteLesson(ScheduleClass rawScheduleClass, RestClient client)
    {
        LessonForWriteDto requestBody;
        LessonForReadDto responseData;

        // pre-create necessary related entities (if they don't exist already)
        TeacherForWriteDto teacherForWrite = new() { LastName = rawScheduleClass.teacher, FirstName = "" };
        var teacher = client.EnsureExists<TeacherForReadDto, TeacherForWriteDto>(
            ApiEndpoints.teachersEndpoint, null, teacherForWrite, (t) => t.LastName == rawScheduleClass.teacher);
        var teacherId = teacher.Id;

        var subject = client.EnsureExists<Subject, string>(
            ApiEndpoints.subjectsEndpoint, null, Utls.EncaseInQuotes(rawScheduleClass.name), (s) => s.Name == rawScheduleClass.name);
        var subjectId = subject.Id;

        var roomForWrite = new RoomForWriteDto { Name = rawScheduleClass.room };
        var room = client.EnsureExists<Room, RoomForWriteDto>(
            ApiEndpoints.roomsEndpoint, null, roomForWrite, (r) => r.Name == rawScheduleClass.room);
        var roomId = room.Id;

        // assemble final request
        requestBody = new()
        {
            RoomId = roomId,
            SubjectId = subjectId,
            TeacherId = teacherId
        };

        // POST the lesson with entities
        responseData = client.PostJson
            <LessonForWriteDto, LessonForReadDto>(
            ApiEndpoints.lessonsEndpoint, requestBody);

        return responseData;
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
}